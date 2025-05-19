namespace ShopSystem.Data.Models
{
    public abstract class Event
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public User TriggeredBy { get; set; }
    }

    public class PurchaseEvent : Event { }
    public class ReturnEvent : Event { }
    public class DestructionEvent : Event { }
}
