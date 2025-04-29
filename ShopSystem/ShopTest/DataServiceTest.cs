using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopSystem.Data.Models;
using ShopSystem.Logic.Services;
using ShopSystem.Tests.Repositories;
using ShopTest.Helpers;
using System;
using System.Linq;

namespace ShopSystem.Tests.Logic
{
    [TestClass]
    public class DataServiceTests
    {
        private InMemoryRepository _repository;
        private DataService _service;

        [TestInitialize]
        public void Setup()
        {
            _repository = new InMemoryRepository();
            _service = new DataService(_repository);
        }

        public Employee GenerateEmployee() => new Employee { Id = 1, Name = "Alice" };
        public Customer GenerateCustomer() => new Customer { Id = 2, Name = "Bob" };
        public PurchaseEvent GeneratePurchaseEvent(User user) =>
            new PurchaseEvent { Id = 101, Description = "Buy item", TriggeredBy = user };

        [TestMethod]
        public void AddUser_ShouldStoreUser()
        {
            var emp = GenerateEmployee();
            _service.AddUser(emp);
            Assert.IsTrue(_repository.GetUsers().Contains(emp));
        }

        [TestMethod]
        public void RegisterEvent_WithEmployee_ShouldAddEvent()
        {
            var emp = GenerateEmployee();
            _service.AddUser(emp);
            var evt = GeneratePurchaseEvent(emp);
            _service.RegisterEvent(evt);
            Assert.IsTrue(_repository.Events.Contains(evt));
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void RegisterEvent_WithCustomer_ShouldFail()
        {
            var cust = GenerateCustomer();
            _service.AddUser(cust);
            var evt = GeneratePurchaseEvent(cust);
            _service.RegisterEvent(evt);
        }
    }
}
