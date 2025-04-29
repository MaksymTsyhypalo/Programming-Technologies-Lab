using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopTest.Helpers;
using ShopSystem.Data.Models;
using System.Linq;

namespace ShopSystem.Tests.Repositories
{
    [TestClass]
    public class InMemoryRepositoryTest
    {
        [TestMethod]
        public void AddUser_ShouldAddUser()
        {
            var repo = new InMemoryRepository();
            var user = new Customer { Id = 1, Name = "TestUser" };

            repo.AddUser(user);
            Assert.AreEqual(1, repo.GetUsers().Count());
        }

        [TestMethod]
        public void GetCatalogItem_ShouldReturnCorrectItem()
        {
            var repo = new InMemoryRepository();
            repo.SeedCatalog();

            var item = repo.GetCatalogItem(1);
            Assert.IsNotNull(item);
            Assert.AreEqual("Item A", item.Name);
        }
    }
}
