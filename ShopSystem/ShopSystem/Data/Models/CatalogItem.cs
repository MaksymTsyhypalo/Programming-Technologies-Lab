
using ShopSystem.Data.Models;

namespace ShopSystem.Data.Models
{
    public abstract class CatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
public class ConcreteCatalogItem : CatalogItem
{

}