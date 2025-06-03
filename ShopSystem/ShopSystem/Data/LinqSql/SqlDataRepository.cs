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

        public void OpenConnection()
        {
            if (_context.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
                _context.Database.OpenConnection();
        }

        public void CloseConnection()
        {
            if (_context.Database.GetDbConnection().State != System.Data.ConnectionState.Closed)
                _context.Database.CloseConnection();
        }

        public IEnumerable<User> GetUsers() => _context.Users.ToList();

        public IEnumerable<User> GetUsersByName(string name)
        {
            return _context.Users.Where(u => u.Name == name).ToList();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            var existing = (from u in _context.Users
                            where u.Id == user.Id
                            select u).FirstOrDefault();
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(user);
                _context.SaveChanges();
            }
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

        public IEnumerable<Event> GetEvents()
        {
            var query = from e in _context.Events.Include(e => e.TriggeredBy)
                        select e;
            return query.ToList();
        }

        public void RegisterEvent(Event e)
        {
            _context.Events.Add(e);
            _context.SaveChanges();
        }

        public void RemoveEvent(int eventId)
        {
            var eventToRemove = (from e in _context.Events
                                 where e.Id == eventId
                                 select e).FirstOrDefault();
            if (eventToRemove != null)
            {
                _context.Events.Remove(eventToRemove);
                _context.SaveChanges();
            }
        }

        public IEnumerable<State> GetStates() => _context.States
            .Include(s => s.Inventory)
            .ToList();

        public State GetCurrentState()
        {
            var query = from s in _context.States.Include(s => s.Inventory)
                        orderby s.Id descending
                        select s;
            return query.FirstOrDefault();
        }

        public void UpdateState(State state)
        {
            _context.States.Update(state);
            _context.SaveChanges();
        }

        public CatalogItem GetCatalogItem(int id)
        {
            var query = from c in _context.Catalog
                        where c.Id == id
                        select c;
            return query.FirstOrDefault();
        }

        public IEnumerable<CatalogItem> GetCatalog() => _context.Catalog.ToList();

        public void AddCatalogItem(CatalogItem item)
        {
            var exists = (from c in _context.Catalog
                          where c.Id == item.Id
                          select c).Any();
            if (!exists)
            {
                _context.Catalog.Add(item);
                _context.SaveChanges();
            }
        }

        public void UpdateCatalogItem(CatalogItem item)
        {
            _context.Catalog.Update(item);
            _context.SaveChanges();
        }

        public void RemoveCatalogItem(int itemId)
        {
            var item = (from c in _context.Catalog
                        where c.Id == itemId
                        select c).FirstOrDefault();
            if (item != null)
            {
                _context.Catalog.Remove(item);
                _context.SaveChanges();
            }
        }
    }
}