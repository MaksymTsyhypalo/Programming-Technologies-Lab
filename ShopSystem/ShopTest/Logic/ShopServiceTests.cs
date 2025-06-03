// ShopTest/Logic/ShopServiceTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopSystem.Data.Interfaces;
using ShopSystem.Data.Models;
using ShopSystem.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopTest.Logic
{
    [TestClass]
    public class ShopServiceTests
    {
        private Mock<IDataRepository> _mockRepo;
        private ShopService _shopService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepo = new Mock<IDataRepository>();
            _shopService = new ShopService(_mockRepo.Object);
        }

        // Data Generation Method 1: Inline Test Data
        private List<User> GetSampleUsers()
        {
            return new List<User>
            {
                new Customer { Id = 1, Name = "Alice Customer" },
                new Employee { Id = 2, Name = "Bob Employee" }
            };
        }

        // Data Generation Method 2: Test Data Factory
        private static class TestDataFactory
        {
            public static CatalogItem CreateCatalogItem(int id, string name, decimal price)
            {
                return new ConcreteCatalogItem { Id = id, Name = name, Price = price };
            }

            public static State CreateStateWithInventory(int stateId, Dictionary<int, int> inventoryQuantities, List<CatalogItem> allItems)
            {
                var state = new ConcreteState { Id = stateId };
                foreach (var entry in inventoryQuantities)
                {
                    var item = allItems.FirstOrDefault(i => i.Id == entry.Key);
                    if (item != null)
                    {
                        state.InventoryEntries.Add(new InventoryEntry
                        {
                            Id = new Random().Next(1000, 9999), // Simulate DB ID
                            StateId = stateId,
                            CatalogItemId = item.Id,
                            Quantity = entry.Value,
                            CatalogItem = item,
                            State = state
                        });
                    }
                }
                return state;
            }
        }


        [TestMethod]
        public void GetAllUsers_ShouldReturnUsers_UsingInlineData()
        {
            _mockRepo.Setup(r => r.GetUsers()).Returns(GetSampleUsers().AsQueryable());
            var users = _shopService.GetAllUsers();
            Assert.AreEqual(2, users.Count());
            Assert.IsTrue(users.Any(u => u.Name == "Alice Customer"));
        }

        [TestMethod]
        public void GetCatalog_ShouldReturnCatalogItems_UsingFactoryData()
        {
            var catalogItems = new List<CatalogItem>
            {
                TestDataFactory.CreateCatalogItem(101, "Laptop", 1200.00m),
                TestDataFactory.CreateCatalogItem(102, "Mouse", 25.00m)
            };
            _mockRepo.Setup(r => r.GetCatalog()).Returns(catalogItems.AsQueryable());
            var catalog = _shopService.GetCatalog();
            Assert.AreEqual(2, catalog.Count());
            Assert.IsTrue(catalog.Any(item => item.Name == "Laptop"));
        }

        [TestMethod]
        public void ProcessPurchase_ShouldDecreaseInventoryAndRegisterEvent()
        {
            // Setup initial state and catalog
            var user = GetSampleUsers().First();
            var laptop = TestDataFactory.CreateCatalogItem(101, "Laptop", 1200.00m);
            var allCatalogItems = new List<CatalogItem> { laptop };
            var initialState = TestDataFactory.CreateStateWithInventory(1, new Dictionary<int, int> { { 101, 5 } }, allCatalogItems);

            _mockRepo.Setup(r => r.GetUsers()).Returns(new List<User> { user }.AsQueryable());
            _mockRepo.Setup(r => r.GetCatalog()).Returns(allCatalogItems.AsQueryable());
            _mockRepo.Setup(r => r.GetCatalogItem(laptop.Id)).Returns(laptop);
            _mockRepo.Setup(r => r.GetCurrentState()).Returns(initialState);
            _mockRepo.Setup(r => r.UpdateState(It.IsAny<State>())).Callback<State>(s => {
                // Simulate state update in mock if needed for subsequent calls
                // In a real mock, you might store the updated state in a private field
                // to reflect changes in successive GetCurrentState() calls.
                // For simplicity, here we just verify it was called.
            });
            _mockRepo.Setup(r => r.RegisterEvent(It.IsAny<PurchaseEvent>()));

            _shopService.ProcessPurchase(user.Id, laptop.Id, 2);

            // Verify inventory decreased
            Assert.AreEqual(3, initialState.InventoryEntries.First(ie => ie.CatalogItemId == laptop.Id).Quantity);
            _mockRepo.Verify(r => r.UpdateState(initialState), Times.Once);

            // Verify event registered
            _mockRepo.Verify(r => r.RegisterEvent(It.Is<PurchaseEvent>(
                e => e.TriggeredBy.Id == user.Id && e.Item.Id == laptop.Id && e.Quantity == 2)), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ProcessPurchase_ShouldThrowException_WhenInsufficientStock()
        {
            var user = GetSampleUsers().First();
            var laptop = TestDataFactory.CreateCatalogItem(101, "Laptop", 1200.00m);
            var allCatalogItems = new List<CatalogItem> { laptop };
            var initialState = TestDataFactory.CreateStateWithInventory(1, new Dictionary<int, int> { { 101, 1 } }, allCatalogItems);

            _mockRepo.Setup(r => r.GetUsers()).Returns(new List<User> { user }.AsQueryable());
            _mockRepo.Setup(r => r.GetCatalogItem(laptop.Id)).Returns(laptop);
            _mockRepo.Setup(r => r.GetCurrentState()).Returns(initialState);

            _shopService.ProcessPurchase(user.Id, laptop.Id, 2); // Attempt to buy 2, but only 1 in stock
        }

        [TestMethod]
        public void ProcessReturn_ShouldIncreaseInventoryAndRegisterEvent()
        {
            var user = GetSampleUsers().First();
            var mouse = TestDataFactory.CreateCatalogItem(102, "Mouse", 25.00m);
            var allCatalogItems = new List<CatalogItem> { mouse };
            var initialState = TestDataFactory.CreateStateWithInventory(1, new Dictionary<int, int> { { 102, 5 } }, allCatalogItems);

            _mockRepo.Setup(r => r.GetUsers()).Returns(new List<User> { user }.AsQueryable());
            _mockRepo.Setup(r => r.GetCatalog()).Returns(allCatalogItems.AsQueryable());
            _mockRepo.Setup(r => r.GetCatalogItem(mouse.Id)).Returns(mouse);
            _mockRepo.Setup(r => r.GetCurrentState()).Returns(initialState);
            _mockRepo.Setup(r => r.UpdateState(It.IsAny<State>()));
            _mockRepo.Setup(r => r.RegisterEvent(It.IsAny<ReturnEvent>()));

            _shopService.ProcessReturn(user.Id, mouse.Id, 3);

            Assert.AreEqual(8, initialState.InventoryEntries.First(ie => ie.CatalogItemId == mouse.Id).Quantity);
            _mockRepo.Verify(r => r.UpdateState(initialState), Times.Once);
            _mockRepo.Verify(r => r.RegisterEvent(It.Is<ReturnEvent>(
                e => e.TriggeredBy.Id == user.Id && e.Item.Id == mouse.Id && e.Quantity == 3)), Times.Once);
        }

        [TestMethod]
        public void InitializeShopState_ShouldCreateStateIfNoneExists()
        {
            _mockRepo.Setup(r => r.GetCurrentState()).Returns((State)null); // Simulate no current state
            _mockRepo.Setup(r => r.AddState(It.IsAny<ConcreteState>()));

            _shopService.InitializeShopState();

            _mockRepo.Verify(r => r.AddState(It.IsAny<ConcreteState>()), Times.Once);
        }

        [TestMethod]
        public void InitializeShopState_ShouldDoNothingIfStateExists()
        {
            _mockRepo.Setup(r => r.GetCurrentState()).Returns(new ConcreteState()); // Simulate existing state
            _mockRepo.Setup(r => r.AddState(It.IsAny<ConcreteState>()));

            _shopService.InitializeShopState();

            _mockRepo.Verify(r => r.AddState(It.IsAny<ConcreteState>()), Times.Never);
        }
    }
}