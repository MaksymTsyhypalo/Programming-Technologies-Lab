// ShopSystem/Data/Context/DataContext.cs
using Microsoft.EntityFrameworkCore;
using ShopSystem.Data.Models;
using System.Collections.Generic; // Added for List<CatalogItem> if still used directly

namespace ShopSystem.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<CatalogItem> Catalog { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<InventoryEntry> InventoryEntries { get; set; } // Add DbSet for InventoryEntry

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Employee>("Employee")
                .HasValue<Customer>("Customer");

            modelBuilder.Entity<Event>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<PurchaseEvent>("Purchase")
                .HasValue<ReturnEvent>("Return")
                .HasValue<DestructionEvent>("Destruction");

            // Configure InventoryEntry relationship
            modelBuilder.Entity<InventoryEntry>()
                .HasOne(ie => ie.State)
                .WithMany(s => s.InventoryEntries)
                .HasForeignKey(ie => ie.StateId);

            modelBuilder.Entity<InventoryEntry>()
                .HasOne(ie => ie.CatalogItem)
                .WithMany() // No direct navigation property on CatalogItem back to InventoryEntry
                .HasForeignKey(ie => ie.CatalogItemId);

            // If you want to configure composite key for InventoryEntry
            // modelBuilder.Entity<InventoryEntry>().HasKey(ie => new { ie.StateId, ie.CatalogItemId });
            // If InventoryEntry has its own Id, then it's a simple primary key.
        }
    }
}