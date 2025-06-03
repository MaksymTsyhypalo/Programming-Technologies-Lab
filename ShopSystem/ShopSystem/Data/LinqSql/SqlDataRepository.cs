// ShopSystem/Data/LinqSql/SqlDataRepository.cs
using Microsoft.EntityFrameworkCore;
using ShopSystem.Data.Context;
using ShopSystem.Data.Interfaces;
using ShopSystem.Data.Models;
using System;
using System.Linq;

namespace ShopSystem.Data.LinqSql
{
    public class SqlDataRepository : IDataRepository
    {
        private readonly DataContext _context;

        public SqlDataRepository(DataContext context)
        {
            _context = context;
        }

        // Users
        public IQueryable<User> GetUsers() => _context.Users.AsQueryable();
        public IQueryable<User> GetEmployees() => _context.Users.OfType<Employee>().AsQueryable(); // More idiomatic EF Core for inheritance
        public IQueryable<User> GetCustomers() => _context.Users.OfType<Customer>().AsQueryable(); // More idiomatic EF Core for inheritance
        public void AddUser(User user) { _context.Users.Add(user); _context.SaveChanges(); } // Added SaveChanges
        public void UpdateUser(User user) { _context.Users.Update(user); _context.SaveChanges(); } // Added SaveChanges
        public void DeleteUser(int userId)
        {
            var userToDelete = _context.Users.Find(userId);
            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
                _context.SaveChanges(); // Added SaveChanges
            }
        }

        // Events
        // Eager loading related entities when retrieving Events
        public IQueryable<Event> GetEvents() => _context.Events
                                                    .Include(e => e.TriggeredBy)
                                                    .Include(e => (e as PurchaseEvent).Item) // Include Item for PurchaseEvent
                                                    .Include(e => (e as ReturnEvent).Item)    // Include Item for ReturnEvent
                                                    .Include(e => (e as DestructionEvent).Item); // Include Item for DestructionEvent


        public IQueryable<Event> GetRecentEvents(int days = 7) =>
            GetEvents().Where(e => e.Timestamp >= DateTime.Now.AddDays(-days))
                        .OrderByDescending(e => e.Timestamp);

        public void RegisterEvent(Event e) { _context.Events.Add(e); _context.SaveChanges(); } // Added SaveChanges
        public void RemoveEvent(int eventId)
        {
            var eventToRemove = _context.Events.Find(eventId);
            if (eventToRemove != null)
            {
                _context.Events.Remove(eventToRemove);
                _context.SaveChanges(); // Added SaveChanges
            }
        }

        // States
        // States
        public IQueryable<State> GetStates() => _context.States.Include(s => s.InventoryEntries).ThenInclude(ie => ie.CatalogItem);
        public State GetCurrentState() => GetStates().OrderByDescending(s => s.Id).FirstOrDefault();
        public void AddState(State state) { _context.States.Add(state); _context.SaveChanges(); }
        public void UpdateState(State state) { _context.States.Update(state); _context.SaveChanges(); }

        // Catalog
        public IQueryable<CatalogItem> GetCatalog() => _context.Catalog.AsQueryable();
        public IQueryable<CatalogItem> GetAffordableItems(decimal maxPrice) =>
            _context.Catalog.Where(c => c.Price <= maxPrice)
                           .OrderBy(c => c.Price);

        public CatalogItem GetCatalogItem(int id) => _context.Catalog.Find(id);
        public void AddCatalogItem(CatalogItem item) { _context.Catalog.Add(item); _context.SaveChanges(); } // Added SaveChanges
        public void UpdateCatalogItem(CatalogItem item) { _context.Catalog.Update(item); _context.SaveChanges(); } // Added SaveChanges
        public void RemoveCatalogItem(int itemId)
        {
            var itemToRemove = _context.Catalog.Find(itemId);
            if (itemToRemove != null)
            {
                _context.Catalog.Remove(itemToRemove);
                _context.SaveChanges(); // Added SaveChanges
            }
        }
    }
}