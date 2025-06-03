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

        public IQueryable<User> GetEmployees() =>
            _context.Users.Where(u => EF.Property<string>(u, "Discriminator") == "Employee");

        public IQueryable<User> GetCustomers() =>
            _context.Users.Where(u => EF.Property<string>(u, "Discriminator") == "Customer");

        public void AddUser(User user) => _context.Users.Add(user);
        public void UpdateUser(User user) => _context.Users.Update(user);
        public void DeleteUser(int userId) => _context.Users.Remove(_context.Users.Find(userId));

        // Events
        public IQueryable<Event> GetEvents() => _context.Events.Include(e => e.TriggeredBy);

        public IQueryable<Event> GetRecentEvents(int days = 7) =>
            GetEvents().Where(e => e.Timestamp >= DateTime.Now.AddDays(-days))
                      .OrderByDescending(e => e.Timestamp);

        public void RegisterEvent(Event e) => _context.Events.Add(e);
        public void RemoveEvent(int eventId) => _context.Events.Remove(_context.Events.Find(eventId));

        // States
        public IQueryable<State> GetStates() => _context.States.Include(s => s.Inventory);
        public State GetCurrentState() => GetStates().OrderByDescending(s => s.Id).FirstOrDefault();
        public void UpdateState(State state) => _context.States.Update(state);

        // Catalog
        public IQueryable<CatalogItem> GetCatalog() => _context.Catalog.AsQueryable();

        public IQueryable<CatalogItem> GetAffordableItems(decimal maxPrice) =>
            _context.Catalog.Where(c => c.Price <= maxPrice)
                           .OrderBy(c => c.Price);

        public CatalogItem GetCatalogItem(int id) => _context.Catalog.Find(id);
        public void AddCatalogItem(CatalogItem item) => _context.Catalog.Add(item);
        public void UpdateCatalogItem(CatalogItem item) => _context.Catalog.Update(item);
        public void RemoveCatalogItem(int itemId) => _context.Catalog.Remove(_context.Catalog.Find(itemId));

        public void SaveChanges() => _context.SaveChanges();
    }
}