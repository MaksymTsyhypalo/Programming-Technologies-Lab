// ShopSystem/Data/Models/State.cs
using System.Collections.Generic;

namespace ShopSystem.Data.Models
{
    public abstract class State
    {
        public int Id { get; set; }
        // Changed to ICollection for EF Core conventions and potentially a linking entity
        public virtual ICollection<InventoryEntry> InventoryEntries { get; set; } = new List<InventoryEntry>();
    }

    public class ConcreteState : State
    {
        // The constructor might need adjustment based on how InventoryEntries are added.
        // For EF Core, you'd typically add InventoryEntry objects to the collection.
        public ConcreteState() { } // Parameterless constructor for EF Core
        public ConcreteState(int id)
        {
            Id = id;
        }
    }

    // New linking entity for inventory with quantity
    public class InventoryEntry
    {
        public int Id { get; set; } // Primary key for InventoryEntry
        public int StateId { get; set; }
        public int CatalogItemId { get; set; }
        public int Quantity { get; set; }

        public virtual State State { get; set; }
        public virtual CatalogItem CatalogItem { get; set; }
    }
}