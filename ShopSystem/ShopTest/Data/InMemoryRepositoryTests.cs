using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopSystem.Data.Models;
using ShopSystem.Tests.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace ShopTest.Data
{
    [TestClass]
    public class InMemoryRepositoryTests
    {
        [TestMethod]
        public void AddUser_ShouldAddUser()
        {
            var repo = new InMemoryRepository();
            var user = new Customer { Id = 1, Name = "Alice" };

            repo.AddUser(user);

            Assert.AreEqual(1, repo.GetUsers().Count());
            Assert.AreEqual("Alice", repo.GetUsers().First().Name);
        }

        [TestMethod]
        public void UpdateUser_ShouldUpdateUser()
        {
            var repo = new InMemoryRepository();
            var user = new Customer { Id = 1, Name = "Alice" };
            repo.AddUser(user);

            var updatedUser = new Customer { Id = 1, Name = "Alice Updated" };
            repo.UpdateUser(updatedUser);

            Assert.AreEqual("Alice Updated", repo.GetUsers().First().Name);
        }

        [TestMethod]
        public void DeleteUser_ShouldRemoveUser()
        {
            var repo = new InMemoryRepository();
            var user = new Customer { Id = 1, Name = "Alice" };
            repo.AddUser(user);

            repo.DeleteUser(1);

            Assert.AreEqual(0, repo.GetUsers().Count());
        }

        [TestMethod]
        public void AddCatalogItem_ShouldAddItem()
        {
            var repo = new InMemoryRepository();
            var item = new ConcreteCatalogItem { Id = 1, Name = "Item1", Price = 10m };

            repo.AddCatalogItem(item);

            Assert.AreEqual(1, repo.GetCatalog().Count());
            Assert.AreEqual("Item1", repo.GetCatalog().First().Name);
        }

        [TestMethod]
        public void UpdateCatalogItem_ShouldUpdateItem()
        {
            var repo = new InMemoryRepository();
            var item = new ConcreteCatalogItem { Id = 1, Name = "Item1", Price = 10m };
            repo.AddCatalogItem(item);

            var updatedItem = new ConcreteCatalogItem { Id = 1, Name = "Item1 Updated", Price = 20m };
            repo.UpdateCatalogItem(updatedItem);

            Assert.AreEqual("Item1 Updated", repo.GetCatalog().First().Name);
            Assert.AreEqual(20m, repo.GetCatalog().First().Price);
        }

        [TestMethod]
        public void RemoveCatalogItem_ShouldRemoveItem()
        {
            var repo = new InMemoryRepository();
            var item = new ConcreteCatalogItem { Id = 1, Name = "Item1", Price = 10m };
            repo.AddCatalogItem(item);

            repo.RemoveCatalogItem(1);

            Assert.AreEqual(0, repo.GetCatalog().Count());
        }

        [TestMethod]
        public void RegisterEvent_ShouldAddEvent()
        {
            var repo = new InMemoryRepository();
            var user = new Customer { Id = 1, Name = "Alice" };
            repo.AddUser(user);
            var ev = new PurchaseEvent { Id = 1, Description = "Test Event", TriggeredBy = user };

            repo.RegisterEvent(ev);

            Assert.AreEqual(1, repo.GetEvents().Count());
            Assert.AreEqual("Test Event", repo.GetEvents().First().Description);
        }

        [TestMethod]
        public void RemoveEvent_ShouldRemoveEvent()
        {
            var repo = new InMemoryRepository();
            var user = new Customer { Id = 1, Name = "Alice" };
            repo.AddUser(user);
            var ev = new PurchaseEvent { Id = 1, Description = "Test Event", TriggeredBy = user };
            repo.RegisterEvent(ev);

            repo.RemoveEvent(1);

            Assert.AreEqual(0, repo.GetEvents().Count());
        }

        [TestMethod]
        public void AddAndGetState_ShouldWork()
        {
            var repo = new InMemoryRepository();
            var state = new ConcreteState { Id = 1, Inventory = new List<CatalogItem>() };
            repo.States.Add(state);

            Assert.AreEqual(1, repo.GetStates().Count());
            Assert.AreEqual(state, repo.GetCurrentState());
        }

        [TestMethod]
        public void UpdateState_ShouldUpdateState()
        {
            var repo = new InMemoryRepository();
            var state = new ConcreteState { Id = 1, Inventory = new List<CatalogItem>() };
            repo.States.Add(state);

            var updatedState = new ConcreteState { Id = 1, Inventory = new List<CatalogItem> { new ConcreteCatalogItem { Id = 2, Name = "Item2", Price = 5m } } };
            repo.UpdateState(updatedState);

            Assert.AreEqual(1, repo.GetStates().Count());
            Assert.AreEqual(1, repo.GetCurrentState().Inventory.Count);
            Assert.AreEqual("Item2", repo.GetCurrentState().Inventory.First().Name);
        }
    }

    // Concrete implementations for testing
    public class ConcreteCatalogItem : CatalogItem { }
    public class PurchaseEvent : Event { }
    public class ConcreteState : State { }
}