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

        public void AddUser(User user) => Users.Add(user);
        public IEnumerable<User> GetUsers() => Users;

        public void RegisterEvent(Event e) => Events.Add(e);
        public IEnumerable<Event> GetEvents() => Events;

        public IEnumerable<State> GetStates() => States;
        public State GetCurrentState() => States.OrderByDescending(s => s.Id).FirstOrDefault();

        public CatalogItem GetCatalogItem(int id) => Catalog.FirstOrDefault(c => c.Id == id);
        public IEnumerable<CatalogItem> GetCatalog() => Catalog;
    }
}
