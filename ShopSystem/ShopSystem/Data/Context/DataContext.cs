// ShopSystem/Data/Context/DataContext.cs
using Microsoft.EntityFrameworkCore;
using ShopSystem.Data.Models;

namespace ShopSystem.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<CatalogItem> Catalog { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure inheritance
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Employee>("Employee")
                .HasValue<Customer>("Customer");

            modelBuilder.Entity<Event>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<PurchaseEvent>("Purchase")
                .HasValue<ReturnEvent>("Return")
                .HasValue<DestructionEvent>("Destruction");
        }
    }
}