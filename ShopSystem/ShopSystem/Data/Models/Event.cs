using System;

namespace ShopSystem.Data.Models
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Description { get; set; }
        public User TriggeredBy { get; set; }
        public string Name { get; set; }
    }
}
