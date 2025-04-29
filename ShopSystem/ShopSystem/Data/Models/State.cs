

namespace ShopSystem.Data.Models
{
    public abstract class State
    {
        public int Id { get; set; }
        public List<CatalogItem> Inventory { get; set; } = new();
    }
    public class ConcreteState : State
    {
        public ConcreteState(int id, List<CatalogItem> inventory)
        {
            Id = id;
            Inventory = inventory;
        }

        public ConcreteState() : this(0, new List<CatalogItem>()) { }
    }
}
