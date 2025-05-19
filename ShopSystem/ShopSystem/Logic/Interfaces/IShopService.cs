using ShopSystem.Data.Models;
using System.Collections.Generic;

namespace ShopSystem.Logic.Interfaces
{
    public interface IShopService
    {
        IEnumerable<User> GetAllUsers();
        void AddUser(User user);
        IEnumerable<CatalogItem> GetCatalog();
        CatalogItem GetCatalogItem(int id);
    }
}