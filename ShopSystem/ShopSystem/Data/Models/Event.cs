using System;

namespace ShopSystem.Data.Models
{
    public abstract class Event
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public User TriggeredBy { get; set; }
        public abstract string Name { get; }
    }

    public class PurchaseEvent : Event
    {
        public override string Name => "Purchase";
    }

    public class ReturnEvent : Event
    {
        public override string Name => "Return";
    }

    public class DestructionEvent : Event
    {
        public override string Name => "Destruction";
    }
}
