using ShopSystem.Data.Models;
using System.Collections.Generic;

namespace ShopSystem.Data.Interfaces
{
    public interface IDataRepository
    {
        
        IEnumerable<User> GetUsers();
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int userId);

        IEnumerable<Event> GetEvents();
        void RegisterEvent(Event e);
        void RemoveEvent(int eventId);

        IEnumerable<State> GetStates();
        State GetCurrentState();
        void UpdateState(State state);

        CatalogItem GetCatalogItem(int id);
        IEnumerable<CatalogItem> GetCatalog();
        void AddCatalogItem(CatalogItem item);
        void UpdateCatalogItem(CatalogItem item);
        void RemoveCatalogItem(int itemId);
    }
}