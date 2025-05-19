using ShopSystem.Data.Context;
using ShopSystem.Data.Interfaces;
using ShopSystem.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShopSystem.Data.LinqSql
{
    public class SqlDataRepository : IDataRepository
    {
        private readonly SqlDbContext _context;

        public SqlDataRepository(SqlDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetUsers() => _context.Users.ToList();
        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public IEnumerable<Event> GetEvents() => _context.Events.ToList();
        public void RegisterEvent(Event e)
        {
            _context.Events.Add(e);
            _context.SaveChanges();
        }

        public IEnumerable<State> GetStates() => _context.States.ToList();
        public State GetCurrentState() => _context.States.OrderByDescending(s => s.Id).FirstOrDefault();

        public CatalogItem GetCatalogItem(int id) => _context.Catalog.FirstOrDefault(c => c.Id == id);
        public IEnumerable<CatalogItem> GetCatalog() => _context.Catalog.ToList();
    }
}