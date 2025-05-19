using ShopSystem.Data.Interfaces;
using ShopSystem.Data.Models;
using ShopSystem.Logic.Interfaces;
using System.Collections.Generic;

namespace ShopSystem.Logic
{
    public class ShopService : IShopService
    {
        private readonly IDataRepository _repository;

        public ShopService(IDataRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<User> GetAllUsers() => _repository.GetUsers();
        public void AddUser(User user) => _repository.AddUser(user);
        public IEnumerable<CatalogItem> GetCatalog() => _repository.GetCatalog();
        public CatalogItem GetCatalogItem(int id) => _repository.GetCatalogItem(id);
    }
}
