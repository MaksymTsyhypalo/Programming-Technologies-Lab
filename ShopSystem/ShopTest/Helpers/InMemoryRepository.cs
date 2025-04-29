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
        public List<State> States { get; } = new();

        private readonly Dictionary<int, CatalogItem> _catalog = new();
        public Dictionary<int, CatalogItem> Catalog => _catalog;

        public IEnumerable<User> GetUsers() => Users;
        public void AddUser(User user) => Users.Add(user);
        public void RegisterEvent(Event e) => Events.Add(e);
        public State GetCurrentState() => States.Any() ? States.Last() : new ConcreteState();

        public CatalogItem GetCatalogItem(int id) => _catalog.TryGetValue(id, out var item) ? item : null;

        public void SeedUsers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (i % 2 == 0)
                    AddUser(new Employee { Id = i, Name = $"Employee {i}" });
                else
                    AddUser(new Customer { Id = i, Name = $"Customer {i}" });
            }
        }

        public void SeedCatalog()
        {
            _catalog[1] = new ConcreteCatalogItem { Id = 3, Name = "Item A", Price = 10.99m };
            _catalog[2] = new ConcreteCatalogItem { Id = 4, Name = "Item B", Price = 24.99m };
        }

        public void SeedStates()
        {
            States.Add(new ConcreteState(1, new List<CatalogItem>(_catalog.Values)));
            States.Add(new ConcreteState(2, new List<CatalogItem>(_catalog.Values)));
        }

        public void SeedEvents()
        {
            var user = new Employee { Id = 99, Name = "System" };
            Events.Add(new PurchaseEvent { Id = 1, Description = "Bought item", TriggeredBy = user });
            Events.Add(new ReturnEvent { Id = 2, Description = "Returned item", TriggeredBy = user });
            Events.Add(new DestructionEvent { Id = 3, Description = "Destroyed item", TriggeredBy = user });
        }
    }
}
