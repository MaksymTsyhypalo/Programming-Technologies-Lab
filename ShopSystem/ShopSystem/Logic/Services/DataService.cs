using System;
using System.Collections.Generic;
using ShopSystem.Data.Interfaces;
using ShopSystem.Data.Models;

namespace ShopSystem.Logic.Services
{
    public class DataService
    {
        private readonly IDataRepository _repository;

        public DataService(IDataRepository repository)
        {
            _repository = repository;
        }

        public void AddUser(User user)
        {
            _repository.AddUser(user);
        }

        public CatalogItem GetCatalogItem(int id)
        {
            return _repository.GetCatalogItem(id);
        }

        public State GetCurrentState()
        {
            return _repository.GetCurrentState();
        }

        public void RegisterEvent(Event e)
        {
            if (e.TriggeredBy != null && e.TriggeredBy.CanManageCatalog())
            {
                _repository.RegisterEvent(e);
            }
            else
            {
                throw new UnauthorizedAccessException("Only employees can register events.");
            }
        }

        public IEnumerable<User> GetUsers()
        {
            return _repository.GetUsers();
        }

        public void AddCatalogItem(User user, int id, CatalogItem item)
        {
            if (user.CanManageCatalog())
            {
                _repository.Catalog[id] = item;
            }
            else
            {
                throw new UnauthorizedAccessException("Only employees can add catalog items.");
            }
        }
    }
}
