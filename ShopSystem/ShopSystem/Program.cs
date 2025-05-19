using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ShopSystem.Data.Context;
using ShopSystem.Data.Interfaces;
using ShopSystem.Data.LinqSql;
using ShopSystem.Logic;
using ShopSystem.Logic.Interfaces;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

                services.AddScoped<IDataRepository, SqlDataRepository>();
                services.AddScoped<IShopService, ShopService>();

                // Add other services or ViewModels here if needed
            })
            .Build();

        // Example usage
        var service = host.Services.GetRequiredService<IShopService>();
        // Do something, like service.RegisterUser(...)
    }
}
