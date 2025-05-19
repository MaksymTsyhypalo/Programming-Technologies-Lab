using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopSystem.Data.Interfaces;
using ShopSystem.Data.Models;
using ShopSystem.Logic;
using System.Collections.Generic;

namespace ShopTest.Logic
{
    [TestClass]
    public class ShopServiceTests
    {
        [TestMethod]
        public void GetAllUsers_ShouldReturnUsers()
        {
            var mockRepo = new Mock<IDataRepository>();
            mockRepo.Setup(r => r.GetUsers()).Returns(new List<User> { new Customer { Id = 1, Name = "Alice" } });

            var service = new ShopService(mockRepo.Object);
            var users = service.GetAllUsers();

            Assert.AreEqual(1, new List<User>(users).Count);
        }
    }
}