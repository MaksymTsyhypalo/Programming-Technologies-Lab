namespace ShopSystem.Data.Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public abstract bool CanManageCatalog();
    }

    public class Customer : User
    {
        public override bool CanManageCatalog() => false;
    }

    public class Employee : User
    {
        public override bool CanManageCatalog() => true;
    }
}
