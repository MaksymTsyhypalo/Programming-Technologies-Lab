// ShopSystem/Data/Models/Event.cs
using System;

namespace ShopSystem.Data.Models
{
    public abstract class Event
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public User TriggeredBy { get; set; }

        public abstract string Description { get; }
    }

    public class PurchaseEvent : Event
    {
        public int Quantity { get; set; }
        public CatalogItem Item { get; set; }

        public override string Description =>
            $"Purchase of {Quantity} × {Item?.Name ?? "Unknown Item"}";
    }

    public class ReturnEvent : Event
    {
        public int Quantity { get; set; }
        public CatalogItem Item { get; set; }

        public override string Description =>
            $"Return of {Quantity} × {Item?.Name ?? "Unknown Item"}";
    }

    public class DestructionEvent : Event
    {
        public int Quantity { get; set; }
        public CatalogItem Item { get; set; }

        public override string Description =>
            $"Destruction of {Quantity} × {Item?.Name ?? "Unknown Item"}";
    }
}
