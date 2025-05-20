using ShopSystem.Data.Interfaces;
using ShopSystem.Data.Models;
using ShopSystem.Logic.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ShopSystem.Logic
{
    public class ShopService : IShopService
    {
        private readonly IDataRepository _repository;

        public ShopService(IDataRepository repository)
        {
            _repository = repository;
        }

        // User operations
        public IEnumerable<User> GetAllUsers() => _repository.GetUsers();

        public User GetUserById(int id) => _repository.GetUsers().FirstOrDefault(u => u.Id == id);

        public void AddUser(User user) => _repository.AddUser(user);

        public void UpdateUser(User user) => _repository.UpdateUser(user);

        public void DeleteUser(int userId) => _repository.DeleteUser(userId);

        // Catalog operations
        public IEnumerable<CatalogItem> GetCatalog() => _repository.GetCatalog();

        public CatalogItem GetCatalogItem(int id) => _repository.GetCatalogItem(id);

        public void AddCatalogItem(CatalogItem item) => _repository.AddCatalogItem(item);

        public void UpdateCatalogItem(CatalogItem item) => _repository.UpdateCatalogItem(item);

        public void RemoveCatalogItem(int itemId) => _repository.RemoveCatalogItem(itemId);

        // Event operations
        public IEnumerable<Event> GetAllEvents() => _repository.GetEvents();

        public void RegisterEvent(Event e) => _repository.RegisterEvent(e);

        public void RemoveEvent(int eventId) => _repository.RemoveEvent(eventId);

        // State operations
        public IEnumerable<State> GetAllStates() => _repository.GetStates();

        public State GetCurrentState() => _repository.GetCurrentState();

        public void UpdateState(State state) => _repository.UpdateState(state);

        // Business logic
        public IEnumerable<CatalogItem> GetInventory() => GetCurrentState()?.Inventory ?? new List<CatalogItem>();

        public void ProcessPurchase(int userId, int itemId)
        {
            var user = GetUserById(userId);
            var item = GetCatalogItem(itemId);

            if (user != null && item != null)
            {
                RegisterEvent(new PurchaseEvent
                {
                    Description = $"{user.Name} purchased {item.Name}",
                    TriggeredBy = user
                });
            }
        }

        public void ProcessReturn(int userId, int itemId)
        {
            var user = GetUserById(userId);
            var item = GetCatalogItem(itemId);

            if (user != null && item != null)
            {
                RegisterEvent(new ReturnEvent
                {
                    Description = $"{user.Name} returned {item.Name}",
                    TriggeredBy = user
                });
            }
        }
    }
}