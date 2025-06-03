// ShopTest/ViewModelTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopSystem.Data.Models;
using ShopSystem.Logic.Interfaces;
using ShopSystem.Presentation.ViewModels;
using System.Collections.Generic;
using System.Linq; // Added for .AsQueryable() in setup
using System.Collections.ObjectModel;
using System.ComponentModel; // Required for PropertyChanged event testing

namespace ShopTest
{
    [TestClass]
    public class ViewModelTests
    {
        private Mock<IShopService> _mockService;
        private ShopViewModel _viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockService = new Mock<IShopService>();
            // Setup default behavior for services used by ViewModel's constructor or LoadData
            _mockService.Setup(s => s.GetAllUsers()).Returns(new List<User>());
            _mockService.Setup(s => s.GetCatalog()).Returns(new List<CatalogItem>());
            _mockService.Setup(s => s.GetInventory()).Returns(new Dictionary<CatalogItem, int>());
            _mockService.Setup(s => s.InitializeShopState()); // Ensure this is mocked
            _mockService.Setup(s => s.GetAllEvents()).Returns(new List<Event>()); // Mock for recent events

            _viewModel = new ShopViewModel(_mockService.Object);
        }

        [TestMethod]
        public void ViewModel_LoadsDataCorrectly()
        {
            var users = new List<User> { new Customer { Id = 1, Name = "Test User" } };
            var items = new List<CatalogItem> { new ConcreteCatalogItem { Id = 10, Name = "Test Item", Price = 100 } };
            var inventory = new Dictionary<CatalogItem, int> { { items[0], 5 } };

            _mockService.Setup(s => s.GetAllUsers()).Returns(users);
            _mockService.Setup(s => s.GetCatalog()).Returns(items);
            _mockService.Setup(s => s.GetInventory()).Returns(inventory);

            // Reload data to use the new mock setups
            _viewModel.LoadDataCommand.Execute(null);

            Assert.AreEqual(1, _viewModel.Users.Count);
            Assert.AreEqual(users[0].Name, _viewModel.Users[0].Name);
            Assert.AreEqual(1, _viewModel.Catalog.Count);
            Assert.AreEqual(items[0].Name, _viewModel.Catalog[0].Name);
            Assert.AreEqual(1, _viewModel.Inventory.Count);
            Assert.AreEqual(items[0].Name, _viewModel.Inventory[0].Item.Name);
            Assert.AreEqual(5, _viewModel.Inventory[0].Quantity);
        }

        [TestMethod]
        public void PurchaseCommand_CanExecute_ReturnsTrueWhenUserAndItemSelected()
        {
            _viewModel.SelectedUser = new Customer { Id = 1 };
            _viewModel.SelectedItem = new ConcreteCatalogItem { Id = 10 };
            _viewModel.PurchaseQuantity = 1;

            Assert.IsTrue(_viewModel.PurchaseCommand.CanExecute(null));
        }

        [TestMethod]
        public void PurchaseCommand_CanExecute_ReturnsFalseWhenUserNotSelected()
        {
            _viewModel.SelectedUser = null;
            _viewModel.SelectedItem = new ConcreteCatalogItem { Id = 10 };
            _viewModel.PurchaseQuantity = 1;

            Assert.IsFalse(_viewModel.PurchaseCommand.CanExecute(null));
        }

        [TestMethod]
        public void PurchaseCommand_CanExecute_ReturnsFalseWhenItemNotSelected()
        {
            _viewModel.SelectedUser = new Customer { Id = 1 };
            _viewModel.SelectedItem = null;
            _viewModel.PurchaseQuantity = 1;

            Assert.IsFalse(_viewModel.PurchaseCommand.CanExecute(null));
        }

        [TestMethod]
        public void PurchaseCommand_CanExecute_ReturnsFalseWhenQuantityIsZero()
        {
            _viewModel.SelectedUser = new Customer { Id = 1 };
            _viewModel.SelectedItem = new ConcreteCatalogItem { Id = 10 };
            _viewModel.PurchaseQuantity = 0;

            Assert.IsFalse(_viewModel.PurchaseCommand.CanExecute(null));
        }

        [TestMethod]
        public void PurchaseCommand_CallsProcessPurchaseAndRefreshesInventory()
        {
            var user = new Customer { Id = 1, Name = "Alice" };
            var item = new ConcreteCatalogItem { Id = 10, Name = "Laptop" };
            var initialInventory = new Dictionary<CatalogItem, int> { { item, 5 } };
            var updatedInventory = new Dictionary<CatalogItem, int> { { item, 4 } }; // After purchase

            _mockService.Setup(s => s.GetUserById(user.Id)).Returns(user);
            _mockService.Setup(s => s.GetCatalogItem(item.Id)).Returns(item);
            _mockService.Setup(s => s.GetInventory()).Returns(initialInventory).Callback(() => _mockService.Setup(s => s.GetInventory()).Returns(updatedInventory)); // Simulate state change
            _mockService.Setup(s => s.ProcessPurchase(user.Id, item.Id, 1)).Verifiable();

            _viewModel.SelectedUser = user;
            _viewModel.SelectedItem = item;
            _viewModel.PurchaseQuantity = 1;

            _viewModel.PurchaseCommand.Execute(null);

            _mockService.Verify(s => s.ProcessPurchase(user.Id, item.Id, 1), Times.Once);
            Assert.AreEqual(1, _viewModel.Inventory.Count);
            Assert.AreEqual(4, _viewModel.Inventory[0].Quantity); // Verify inventory updated
        }

        [TestMethod]
        public void SelectedUser_PropertyChanged_RaisesEvent()
        {
            var raised = false;
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(ShopViewModel.SelectedUser))
                    raised = true;
            };

            _viewModel.SelectedUser = new Customer { Id = 1 };

            Assert.IsTrue(raised);
        }

        [TestMethod]
        public void SelectedItem_PropertyChanged_RaisesEvent()
        {
            var raised = false;
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(ShopViewModel.SelectedItem))
                    raised = true;
            };

            _viewModel.SelectedItem = new ConcreteCatalogItem { Id = 10 };

            Assert.IsTrue(raised);
        }

        [TestMethod]
        public void PurchaseQuantity_PropertyChanged_RaisesEvent()
        {
            var raised = false;
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(ShopViewModel.PurchaseQuantity))
                    raised = true;
            };

            _viewModel.PurchaseQuantity = 5;

            Assert.IsTrue(raised);
        }
    }
}