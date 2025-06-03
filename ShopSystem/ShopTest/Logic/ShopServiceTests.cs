using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopSystem.Data.Interfaces;
using ShopSystem.Data.Models;
using ShopSystem.Logic;
using ShopSystem.Tests.Helpers;
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

    [TestClass]
    public class ShopServiceInMemoryTests
    {
        [TestMethod]
        public void GetAllUsers_WithInMemoryRepository_ReturnsUsers()
        {
            var repo = new InMemoryRepository();
            repo.Users.Add(new Customer { Id = 2, Name = "Bob" });

            var service = new ShopService(repo);
            var users = service.GetAllUsers();

            Assert.AreEqual(1, new List<User>(users).Count);
            Assert.AreEqual("Bob", new List<User>(users)[0].Name);
        }
    }
}