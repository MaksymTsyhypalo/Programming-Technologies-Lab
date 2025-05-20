using System.Collections.Generic;
using System.Linq;
using ShopSystem.Data.Interfaces;
using ShopSystem.Data.Models;

namespace ShopSystem.Tests.Helpers
{
    public class InMemoryRepository : IDataRepository
    {
        public List<User> Users { get; set; } = new();
        public List<Event> Events { get; set; } = new();
        public List<State> States { get; set; } = new();
        public List<CatalogItem> Catalog { get; set; } = new();

        // User operations
        public IEnumerable<User> GetUsers() => Users;
        public void AddUser(User user) => Users.Add(user);
        public void UpdateUser(User user)
        {
            var existing = Users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                Users.Remove(existing);
                Users.Add(user);
            }
        }
        public void DeleteUser(int userId) => Users.RemoveAll(u => u.Id == userId);

        // Event operations
        public IEnumerable<Event> GetEvents() => Events;
        public void RegisterEvent(Event e) => Events.Add(e);
        public void RemoveEvent(int eventId) => Events.RemoveAll(e => e.Id == eventId);

        // State operations
        public IEnumerable<State> GetStates() => States;
        public State GetCurrentState() => States.OrderByDescending(s => s.Id).FirstOrDefault();
        public void UpdateState(State state)
        {
            States.RemoveAll(s => s.Id == state.Id);
            States.Add(state);
        }

        // Catalog operations
        public CatalogItem GetCatalogItem(int id) => Catalog.FirstOrDefault(c => c.Id == id);
        public IEnumerable<CatalogItem> GetCatalog() => Catalog;
        public void AddCatalogItem(CatalogItem item) => Catalog.Add(item);
        public void UpdateCatalogItem(CatalogItem item)
        {
            Catalog.RemoveAll(c => c.Id == item.Id);
            Catalog.Add(item);
        }
        public void RemoveCatalogItem(int itemId) => Catalog.RemoveAll(c => c.Id == itemId);
    }
}
