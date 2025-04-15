using System.Collections.Generic;
using System.Linq;
using ShopSystem.Data.Models;
using ShopSystem.Data.Interfaces;

namespace ShopTest.Helpers
{
    public class InMemoryRepository : IDataRepository
    {
        public List<User> Users { get; } = new();
        public List<Event> Events { get; } = new();
        public Dictionary<int, CatalogItem> Catalog { get; } = new();
        public List<State> States { get; } = new();

        public IEnumerable<User> GetUsers() => Users;
        public void AddUser(User user) => Users.Add(user);
        public void RegisterEvent(Event e) => Events.Add(e);
        public State GetCurrentState() => States.Any() ? States.Last() : new State();
        public CatalogItem GetCatalogItem(int id) => Catalog.ContainsKey(id) ? Catalog[id] : null;

        // Data generation helpers
        public void SeedUsers(int count)
        {
            for (int i = 0; i < count; i++)
                AddUser(new User { Id = i, Name = $"User {i}" });
        }

        public void SeedCatalog()
        {
            Catalog[1] = new CatalogItem { Id = 3, Name = "Certain item 1" };
            Catalog[2] = new CatalogItem { Id = 4, Name = "Certain item 2" };
        }
    }
}
