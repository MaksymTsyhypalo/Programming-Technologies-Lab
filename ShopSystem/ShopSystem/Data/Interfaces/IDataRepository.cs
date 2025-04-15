using System.Collections.Generic;
using ShopSystem.Data.Models;

namespace ShopSystem.Data.Interfaces
{
    public interface IDataRepository
    {
        IEnumerable<User> GetUsers();
        void AddUser(User user);

        void RegisterEvent(Event e);
        State GetCurrentState();
        CatalogItem GetCatalogItem(int id);

        Dictionary<int, CatalogItem> Catalog { get; }
    }
}