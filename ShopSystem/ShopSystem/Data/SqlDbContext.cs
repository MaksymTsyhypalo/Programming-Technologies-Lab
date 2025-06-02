// ShopSystem/Data/SqlDbContext.cs
using Microsoft.EntityFrameworkCore;
using ShopSystem.Data.Models;

namespace ShopSystem.Data
{
    public class SqlDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<State> States { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=ShopSystemDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False");
        }
    }
}