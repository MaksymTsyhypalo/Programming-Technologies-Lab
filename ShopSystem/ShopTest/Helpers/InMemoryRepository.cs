// ShopSystem/Data/InMemory/InMemoryRepository.cs
using ShopSystem.Data.Interfaces;
using ShopSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopSystem.Data.InMemory
{
    public class InMemoryRepository : IDataRepository
    {
        private readonly List<User> _users = new();
        private readonly List<Event> _events = new();
        private readonly List<State> _states = new();
        private readonly List<CatalogItem> _catalog = new();

        // Users
        public IQueryable<User> GetUsers() => _users.AsQueryable();
        public IQueryable<User> GetEmployees() =>
            _users.Where(u => u is Employee).AsQueryable();
        public IQueryable<User> GetCustomers() =>
            _users.Where(u => u is Customer).AsQueryable();
        public void AddUser(User user) => _users.Add(user);
        public void UpdateUser(User user)
        {
            var index = _users.FindIndex(u => u.Id == user.Id);
            if (index >= 0) _users[index] = user;
        }
        public void DeleteUser(int userId) =>
            _users.RemoveAll(u => u.Id == userId);

        // Events
        public IQueryable<Event> GetEvents() => _events.AsQueryable();
        public IQueryable<Event> GetRecentEvents(int days = 7) =>
            _events.Where(e => e.Timestamp >= DateTime.Now.AddDays(-days))
                   .OrderByDescending(e => e.Timestamp)
                   .AsQueryable();
        public void RegisterEvent(Event e) => _events.Add(e);
        public void RemoveEvent(int eventId) =>
            _events.RemoveAll(e => e.Id == eventId);

        // States
        public IQueryable<State> GetStates() => _states.AsQueryable();
        public State GetCurrentState() => _states.OrderByDescending(s => s.Id).FirstOrDefault();
        public void UpdateState(State state)
        {
            var index = _states.FindIndex(s => s.Id == state.Id);
            if (index >= 0) _states[index] = state;
        }

        // Catalog
        public IQueryable<CatalogItem> GetCatalog() => _catalog.AsQueryable();
        public IQueryable<CatalogItem> GetAffordableItems(decimal maxPrice) =>
            _catalog.Where(c => c.Price <= maxPrice).OrderBy(c => c.Price).AsQueryable();
        public CatalogItem GetCatalogItem(int id) => _catalog.FirstOrDefault(c => c.Id == id);
        public void AddCatalogItem(CatalogItem item) => _catalog.Add(item);
        public void UpdateCatalogItem(CatalogItem item)
        {
            var index = _catalog.FindIndex(c => c.Id == item.Id);
            if (index >= 0) _catalog[index] = item;
        }
        public void RemoveCatalogItem(int itemId) =>
            _catalog.RemoveAll(c => c.Id == itemId);
    }
}
