

namespace ShopSystem.Data.Models
{
    public class State
    {
        public int Id { get; set; }
        public List<CatalogItem> Inventory { get; set; } = new();
    }
}
