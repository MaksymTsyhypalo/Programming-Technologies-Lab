using ShopSystem.Data.Models;
using System.Collections.Generic;

namespace ShopSystem.Logic.Interfaces
{
    public interface IShopService
    {
        // User operations
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int userId);

        // Catalog operations
        IEnumerable<CatalogItem> GetCatalog();
        CatalogItem GetCatalogItem(int id);
        void AddCatalogItem(CatalogItem item);
        void UpdateCatalogItem(CatalogItem item);
        void RemoveCatalogItem(int itemId);

        // Event operations
        IEnumerable<Event> GetAllEvents();
        void RegisterEvent(Event e);
        void RemoveEvent(int eventId);

        // State operations
        IEnumerable<State> GetAllStates();
        State GetCurrentState();
        void UpdateState(State state);

        // Combined operations
        IEnumerable<CatalogItem> GetInventory();
        void ProcessPurchase(int userId, int itemId);
        void ProcessReturn(int userId, int itemId);
    }
}