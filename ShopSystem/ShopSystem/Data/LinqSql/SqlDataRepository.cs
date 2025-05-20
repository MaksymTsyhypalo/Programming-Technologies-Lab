using Microsoft.EntityFrameworkCore;
using ShopSystem.Data.Context;
using ShopSystem.Data.Interfaces;
using ShopSystem.Data.Models;
using System.Collections.Generic;
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

        // User operations
        public IEnumerable<User> GetUsers() => _context.Users.ToList();

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        // Event operations
        public IEnumerable<Event> GetEvents() => _context.Events
            .Include(e => e.TriggeredBy)
            .ToList();

        public void RegisterEvent(Event e)
        {
            _context.Events.Add(e);
            _context.SaveChanges();
        }

        public void RemoveEvent(int eventId)
        {
            var eventToRemove = _context.Events.Find(eventId);
            if (eventToRemove != null)
            {
                _context.Events.Remove(eventToRemove);
                _context.SaveChanges();
            }
        }

        // State operations
        public IEnumerable<State> GetStates() => _context.States
            .Include(s => s.Inventory)
            .ToList();

        public State GetCurrentState() => _context.States
            .Include(s => s.Inventory)
            .OrderByDescending(s => s.Id)
            .FirstOrDefault();

        public void UpdateState(State state)
        {
            _context.States.Update(state);
            _context.SaveChanges();
        }

        // Catalog operations
        public CatalogItem GetCatalogItem(int id) => _context.Catalog
            .FirstOrDefault(c => c.Id == id);

        public IEnumerable<CatalogItem> GetCatalog() => _context.Catalog.ToList();

        public void AddCatalogItem(CatalogItem item)
        {
            _context.Catalog.Add(item);
            _context.SaveChanges();
        }

        public void UpdateCatalogItem(CatalogItem item)
        {
            _context.Catalog.Update(item);
            _context.SaveChanges();
        }

        public void RemoveCatalogItem(int itemId)
        {
            var item = _context.Catalog.Find(itemId);
            if (item != null)
            {
                _context.Catalog.Remove(item);
                _context.SaveChanges();
            }
        }
    }
}