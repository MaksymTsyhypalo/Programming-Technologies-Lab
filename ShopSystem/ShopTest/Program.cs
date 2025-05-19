using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ShopSystem.Data.Context;
using ShopSystem.Data.Interfaces;
using ShopSystem.Data.LinqSql;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ShopSystemDb;Trusted_Connection=True;"));

        services.AddScoped<IDataRepository, SqlDataRepository>();

    })
    .Build();
var repository = host.Services.GetRequiredService<IDataRepository>();
