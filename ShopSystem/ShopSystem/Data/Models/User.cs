namespace ShopSystem.Data.Models
{
    public enum UserRole
    {
        Customer,
        Employee
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }

        public bool CanManageCatalog() => Role == UserRole.Employee;
        public bool CanPurchase() => Role == UserRole.Customer;
    }
}