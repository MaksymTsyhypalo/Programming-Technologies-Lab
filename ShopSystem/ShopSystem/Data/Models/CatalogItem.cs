// ShopSystem/Data/Models/CatalogItem.cs (No changes, but ensure it's virtual for lazy loading)
namespace ShopSystem.Data.Models
{
    public abstract class CatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class ConcreteCatalogItem : CatalogItem { }
}