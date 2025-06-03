using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ShopSystem.Data.Context;
using ShopSystem.Data.Interfaces;
using ShopSystem.Data.LinqSql;
using ShopSystem.Logic;
using ShopSystem.Logic.Interfaces;
using ShopSystem.Presentation.ViewModels;
using System;
using System.Linq;


class Program
{
    static void Main(string[] args)
    {
        var connectionString = "Server=(localdb)\\mssqllocaldb;Database=ShopSystemDb;Trusted_Connection=True;";

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure()));

                services.AddScoped<IDataRepository, SqlDataRepository>();
                services.AddScoped<IShopService, ShopService>();
                services.AddTransient<ShopViewModel>();
            })
            .Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var dbContext = services.GetRequiredService<DataContext>();
                if (dbContext.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
                    dbContext.Database.OpenConnection();

                var service = services.GetRequiredService<IShopService>();

                var users = service.GetAllUsers().ToList();
                Console.WriteLine("Users:");
                foreach (var user in users)
                    Console.WriteLine($"  {user.Id}: {user.Name}");

                var items = service.GetCatalog().ToList();
                Console.WriteLine("Catalog Items:");
                foreach (var item in items)
                    Console.WriteLine($"  {item.Id}: {item.Name} (${item.Price})");

                if (users.Any() && items.Any())
                {
                    var user = users.First();
                    var item = items.First();
                    service.ProcessPurchase(user.Id, item.Id);
                    Console.WriteLine($"\nPurchase performed: {user.Name} bought {item.Name}");
                }
                else
                {
                    Console.WriteLine("\nNo users or items available to perform a purchase.");
                }

                var events = service.GetAllEvents().ToList();
                Console.WriteLine("\nEvents:");
                foreach (var ev in events)
                    Console.WriteLine($"  {ev.Id}: {ev.Description} (By: {ev.TriggeredBy?.Name})");

                if (dbContext.Database.GetDbConnection().State != System.Data.ConnectionState.Closed)
                    dbContext.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}