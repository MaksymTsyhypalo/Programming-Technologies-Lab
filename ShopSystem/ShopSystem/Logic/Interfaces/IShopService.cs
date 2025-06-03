// ShopSystem/Logic/Interfaces/IShopService.cs
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
        void InitializeShopState(); // New method to ensure an initial state exists
        void UpdateState(State state); // This method should likely be internal to the service
                                       // as state management is a business logic concern.
                                       // For this exercise, keeping it public as per interface.

        // Combined operations
        Dictionary<CatalogItem, int> GetInventory(); // Change return type to show quantity
        void ProcessPurchase(int userId, int itemId, int quantity); // Add quantity
        void ProcessReturn(int userId, int itemId, int quantity);   // Add quantity
        void ProcessDestruction(int userId, int itemId, int quantity); // New: for DestructionEvent
    }
}