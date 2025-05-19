using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopSystem.Data.Models;
using ShopSystem.Logic.Interfaces;
using ShopSystem.Presentation.ViewModels;
using System.Collections.Generic;

namespace ShopTest
{
    [TestClass]
    public class ViewModelTests
    {
        [TestMethod]
        public void ViewModel_LoadsDataCorrectly()
        {
            var mockService = new Mock<IShopService>();
            mockService.Setup(s => s.GetAllUsers()).Returns(new List<User> { new Customer { Id = 1, Name = "Test" } });
            mockService.Setup(s => s.GetCatalog()).Returns(new List<CatalogItem> { new ConcreteCatalogItem { Id = 1, Name = "Item", Price = 10 } });

            var vm = new ShopViewModel(mockService.Object);

            Assert.AreEqual(1, vm.Users.Count);
            Assert.AreEqual(1, vm.Catalog.Count);
        }
    }
}