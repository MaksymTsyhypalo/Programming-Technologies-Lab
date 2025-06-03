// ShopSystem/Data/Interfaces/IDataRepository.cs
using ShopSystem.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShopSystem.Data.Interfaces
{
    public interface IDataRepository
    {
        // Users
        IQueryable<User> GetUsers();
        IQueryable<User> GetEmployees();
        IQueryable<User> GetCustomers();
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int userId);

        // Events
        IQueryable<Event> GetEvents();
        IQueryable<Event> GetRecentEvents(int days = 7);
        void RegisterEvent(Event e);
        void RemoveEvent(int eventId);

        // States
        IQueryable<State> GetStates();
        State GetCurrentState();
        void AddState(State state); // Added for initial state creation
        void UpdateState(State state);

        // Catalog
        IQueryable<CatalogItem> GetCatalog();
        IQueryable<CatalogItem> GetAffordableItems(decimal maxPrice);
        CatalogItem GetCatalogItem(int id);
        void AddCatalogItem(CatalogItem item);
        void UpdateCatalogItem(CatalogItem item);
        void RemoveCatalogItem(int itemId);
    }
}