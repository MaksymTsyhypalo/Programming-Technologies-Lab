using ShopSystem.Data.Models;

namespace ShopSystem.Data.Context
{
    public class DataContext
    {
        public List<User> Users { get; set; } = new();
        public Dictionary<int, CatalogItem> Catalog { get; set; } = new();
        public List<State> States { get; set; } = new();
        public List<Event> Events { get; set; } = new();
    }
}
