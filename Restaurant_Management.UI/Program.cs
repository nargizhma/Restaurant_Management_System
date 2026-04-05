using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurant_Management.BLL.Services;
using Restaurant_Management.DAL.Context;
using Restaurant_Management.DAL.Repositories;
using Restaurant_Management.UI.Menus;
using Restaurant_Management.UI.Utilities;

// EDIT CONNECTION STRING HERE
const string ConnectionString = "Data Source=restaurant.db";

var services = new ServiceCollection();

services.AddDbContext<RestaurantDbContext>(options =>
    options.UseSqlite(ConnectionString)
);

services.AddScoped<IRestaurantRepository, RestaurantRepository>();
services.AddScoped<ITableRepository, TableRepository>();
services.AddScoped<IMenuItemRepository, MenuItemRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IOrderItemRepository, OrderItemRepository>();

services.AddScoped<IRestaurantService, RestaurantService>();
services.AddScoped<ITableService, TableService>();
services.AddScoped<IMenuItemService, MenuItemService>();
services.AddScoped<IOrderService, OrderService>();
services.AddScoped<IReportService, ReportService>();

services.AddScoped<RestaurantMenu>();
services.AddScoped<TableMenu>();
services.AddScoped<MenuItemMenu>();
services.AddScoped<OrderMenu>();
services.AddScoped<ReportMenu>();

var serviceProvider = services.BuildServiceProvider();

using (var context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<RestaurantDbContext>())
{
    await context.Database.MigrateAsync();
}

await RunApplicationAsync(serviceProvider);

async Task RunApplicationAsync(IServiceProvider provider)
{
    var restaurantMenu = provider.GetRequiredService<RestaurantMenu>();
    var tableMenu = provider.GetRequiredService<TableMenu>();
    var menuItemMenu = provider.GetRequiredService<MenuItemMenu>();
    var orderMenu = provider.GetRequiredService<OrderMenu>();
    var reportMenu = provider.GetRequiredService<ReportMenu>();
    var restaurantService = provider.GetRequiredService<IRestaurantService>();
    var tableService = provider.GetRequiredService<ITableService>();
    var menuItemService = provider.GetRequiredService<IMenuItemService>();
    var orderService = provider.GetRequiredService<IOrderService>();

    while (true)
    {
        Console.Clear();
        ConsoleHelper.PrintHeader("Restoran İdarəetmə Sistemi");
        Console.WriteLine("1. Restaurant Management");
        Console.WriteLine("2. Table Management");
        Console.WriteLine("3. Menu Item Management");
        Console.WriteLine("4. Order Management");
        Console.WriteLine("5. Reports");
        Console.WriteLine("6. Seed Sample Data");
        Console.WriteLine("7. Exit");
        Console.WriteLine();

        string choice = ConsoleHelper.ReadString("Select an option: ") ?? "";

        try
        {
            switch (choice)
            {
                case "1":
                    await restaurantMenu.ShowMenuAsync();
                    break;
                case "2":
                    await tableMenu.ShowMenuAsync();
                    break;
                case "3":
                    await menuItemMenu.ShowMenuAsync();
                    break;
                case "4":
                    await orderMenu.ShowMenuAsync();
                    break;
                case "5":
                    await reportMenu.ShowMenuAsync();
                    break;
                case "6":
                    await SeedDataHelper.SeedSampleDataAsync(restaurantService, tableService, menuItemService, orderService);
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case "7":
                    ConsoleHelper.PrintSuccess("Thank you for using Restoran İdarəetmə Sistemi!");
                    return;
                default:
                    ConsoleHelper.PrintError("Invalid choice. Please try again.");
                    break;
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintError($"Error: {ex.Message}");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
