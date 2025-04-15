using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopSystem.Data.Models;
using ShopSystem.Data.Interfaces;
using ShopSystem.Logic.Services;
using System.Linq;
using System;
using ShopTest.Helpers;

namespace ShopSystem.Tests
{
    [TestClass]
    public class DataServiceTest
    {
        private InMemoryRepository _repository;
        private DataService _service;

        [TestInitialize]
        public void Setup()
        {
            _repository = new InMemoryRepository();
            _service = new DataService(_repository);
        }

        [TestMethod]
        public void AddUser_ShouldStoreUserInRepository()
        {
            var user = new User { Id = 1, Name = "John" };
            _service.AddUser(user);
            var users = _repository.GetUsers();
            Assert.IsTrue(users.Contains(user));
        }

        [TestMethod]
        public void RegisterEvent_ShouldAddEventToRepository()
        {
            var employee = new User { Id = 2, Name = "Alice", Role = UserRole.Employee };
            _repository.AddUser(employee);

            var e = new Event
            {
                Id = 101,
                Name = "New Year Sale",
                Timestamp = DateTime.Now,
                Description = "Holiday discount event",
                TriggeredBy = employee
            };

            _service.RegisterEvent(e);

            Assert.IsTrue(_repository.Events.Contains(e));
        }


        [TestMethod]
        public void GetCurrentState_ShouldReturnDefaultIfNoneExists()
        {
            var state = _service.GetCurrentState();
            Assert.IsNotNull(state);
        }

        [TestMethod]
        public void GetCatalogItem_ShouldReturnItemIfExists()
        {
            var item = new CatalogItem { Id = 10, Name = "Laptop" };
            _repository.Catalog[10] = item;
            var result = _service.GetCatalogItem(10);
            Assert.AreEqual(item, result);
        }

        [TestMethod]
        public void GetCatalogItem_ShouldReturnNullIfNotExists()
        {
            var result = _service.GetCatalogItem(999);
            Assert.IsNull(result);
        }
    }
}
