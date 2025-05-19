namespace ShopSystem.Data.Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Customer : User { }

    public class Employee : User { }
}
