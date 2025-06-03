// ShopSystem/Logic/ShopService.cs
using ShopSystem.Data.Interfaces;
using ShopSystem.Data.Models;
using ShopSystem.Logic.Interfaces;
using System;
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
        public IEnumerable<User> GetAllUsers() => _repository.GetUsers().ToList();
        public User GetUserById(int id) => _repository.GetUsers().FirstOrDefault(u => u.Id == id);
        public void AddUser(User user) => _repository.AddUser(user);
        public void UpdateUser(User user) => _repository.UpdateUser(user);
        public void DeleteUser(int userId) => _repository.DeleteUser(userId);

        // Catalog operations
        public IEnumerable<CatalogItem> GetCatalog() => _repository.GetCatalog().ToList();
        public CatalogItem GetCatalogItem(int id) => _repository.GetCatalogItem(id);
        public void AddCatalogItem(CatalogItem item) => _repository.AddCatalogItem(item);
        public void UpdateCatalogItem(CatalogItem item) => _repository.UpdateCatalogItem(item);
        public void RemoveCatalogItem(int itemId) => _repository.RemoveCatalogItem(itemId);

        // Event operations
        public IEnumerable<Event> GetAllEvents() => _repository.GetEvents().ToList();
        public void RegisterEvent(Event e) => _repository.RegisterEvent(e);
        public void RemoveEvent(int eventId) => _repository.RemoveEvent(eventId);

        // State operations
        public IEnumerable<State> GetAllStates() => _repository.GetStates().ToList();
        public State GetCurrentState() => _repository.GetCurrentState();

        public void InitializeShopState()
        {
            if (GetCurrentState() == null)
            {
                // Create an initial empty state if none exists
                _repository.AddState(new ConcreteState { Id = 1 }); // Assuming IDs are managed externally or by DB
            }
        }

        public void UpdateState(State state) => _repository.UpdateState(state);


        // Business logic for Inventory
        public Dictionary<CatalogItem, int> GetInventory()
        {
            var currentState = GetCurrentState();
            if (currentState == null)
            {
                return new Dictionary<CatalogItem, int>();
            }
            return currentState.InventoryEntries.ToDictionary(ie => ie.CatalogItem, ie => ie.Quantity);
        }

        public void ProcessPurchase(int userId, int itemId, int quantity)
        {
            var user = GetUserById(userId);
            var item = GetCatalogItem(itemId);
            var currentState = GetCurrentState();

            if (user == null) throw new ArgumentException($"User with ID {userId} not found.");
            if (item == null) throw new ArgumentException($"Catalog item with ID {itemId} not found.");
            if (currentState == null) throw new InvalidOperationException("Shop state not initialized. Call InitializeShopState first.");

            var inventoryEntry = currentState.InventoryEntries.FirstOrDefault(ie => ie.CatalogItemId == itemId);

            if (inventoryEntry == null || inventoryEntry.Quantity < quantity)
            {
                throw new InvalidOperationException($"Not enough {item.Name} in stock. Available: {inventoryEntry?.Quantity ?? 0}, Requested: {quantity}");
            }

            inventoryEntry.Quantity -= quantity;
            _repository.UpdateState(currentState); // Save changes to inventory

            RegisterEvent(new PurchaseEvent
            {
                Timestamp = DateTime.Now,
                TriggeredBy = user,
                Quantity = quantity,
                Item = item,
                Description = $"User '{user.Name}' purchased {quantity} x '{item.Name}'"
            });
        }

        public void ProcessReturn(int userId, int itemId, int quantity)
        {
            var user = GetUserById(userId);
            var item = GetCatalogItem(itemId);
            var currentState = GetCurrentState();

            if (user == null) throw new ArgumentException($"User with ID {userId} not found.");
            if (item == null) throw new ArgumentException($"Catalog item with ID {itemId} not found.");
            if (currentState == null) throw new InvalidOperationException("Shop state not initialized. Call InitializeShopState first.");

            var inventoryEntry = currentState.InventoryEntries.FirstOrDefault(ie => ie.CatalogItemId == itemId);

            if (inventoryEntry == null)
            {
                // If the item wasn't in inventory, add it
                inventoryEntry = new InventoryEntry
                {
                    CatalogItemId = itemId,
                    CatalogItem = item, // Attach item for easier reference
                    Quantity = quantity,
                    StateId = currentState.Id,
                    State = currentState // Attach state
                };
                currentState.InventoryEntries.Add(inventoryEntry);
            }
            else
            {
                inventoryEntry.Quantity += quantity;
            }

            _repository.UpdateState(currentState); // Save changes to inventory

            RegisterEvent(new ReturnEvent
            {
                Timestamp = DateTime.Now,
                TriggeredBy = user,
                Quantity = quantity,
                Item = item,
                Description = $"User '{user.Name}' returned {quantity} x '{item.Name}'"
            });
        }

        public void ProcessDestruction(int userId, int itemId, int quantity)
        {
            var user = GetUserById(userId);
            var item = GetCatalogItem(itemId);
            var currentState = GetCurrentState();

            if (user == null) throw new ArgumentException($"User with ID {userId} not found.");
            if (item == null) throw new ArgumentException($"Catalog item with ID {itemId} not found.");
            if (currentState == null) throw new InvalidOperationException("Shop state not initialized. Call InitializeShopState first.");

            var inventoryEntry = currentState.InventoryEntries.FirstOrDefault(ie => ie.CatalogItemId == itemId);

            if (inventoryEntry == null || inventoryEntry.Quantity < quantity)
            {
                throw new InvalidOperationException($"Not enough {item.Name} in stock to destroy. Available: {inventoryEntry?.Quantity ?? 0}, Requested: {quantity}");
            }

            inventoryEntry.Quantity -= quantity;
            _repository.UpdateState(currentState); // Save changes to inventory

            RegisterEvent(new DestructionEvent
            {
                Timestamp = DateTime.Now,
                TriggeredBy = user,
                Quantity = quantity,
                Item = item,
                Description = $"Employee '{user.Name}' destroyed {quantity} x '{item.Name}'"
            });
        }
    }
}