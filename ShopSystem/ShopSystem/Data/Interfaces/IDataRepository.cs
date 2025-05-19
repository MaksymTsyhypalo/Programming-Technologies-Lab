namespace ShopSystem.Data.Interfaces
{
    using Models;
    using System.Collections.Generic;

    public interface IDataRepository
    {
        IEnumerable<User> GetUsers();
        void AddUser(User user);

        IEnumerable<Event> GetEvents();
        void RegisterEvent(Event e);

        IEnumerable<State> GetStates();
        State GetCurrentState();

        CatalogItem GetCatalogItem(int id);
        IEnumerable<CatalogItem> GetCatalog();
    }
}