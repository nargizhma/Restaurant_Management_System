using Restaurant_Management.BLL.Services;

namespace Restaurant_Management.UI.Utilities;

public static class SeedDataHelper
{
    public static async Task SeedSampleDataAsync(
        IRestaurantService restaurantService,
        ITableService tableService,
        IMenuItemService menuItemService,
        IOrderService orderService)
    {
        try
        {
            var restaurants = await restaurantService.GetAllRestaurantsAsync();
            if (restaurants.Any())
                return;

            ConsoleHelper.PrintInfo("Seeding sample data...");

            var restaurant1 = await restaurantService.CreateRestaurantAsync("Xaraş Restoran", 1);
            var restaurant2 = await restaurantService.CreateRestaurantAsync("Şəhər Qapısı", 2);

            var table1 = await tableService.CreateTableAsync(restaurant1.Id, 1, 4);
            var table2 = await tableService.CreateTableAsync(restaurant1.Id, 2, 6);
            var table3 = await tableService.CreateTableAsync(restaurant2.Id, 1, 4);

            var menu1 = await menuItemService.CreateMenuItemAsync(restaurant1.Id, "Lula Kəbab", 12.50m, "Ən");
            var menu2 = await menuItemService.CreateMenuItemAsync(restaurant1.Id, "Şiş Kəbab", 14.00m, "Ən");
            var menu3 = await menuItemService.CreateMenuItemAsync(restaurant1.Id, "Çay", 1.50m, "İçkisiz");

            var menu4 = await menuItemService.CreateMenuItemAsync(restaurant2.Id, "Plov", 10.00m, "Əsas");
            var menu5 = await menuItemService.CreateMenuItemAsync(restaurant2.Id, "Mərci", 8.00m, "Əsas");

            ConsoleHelper.PrintSuccess("Sample data created successfully!");
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintWarning($"Could not seed data: {ex.Message}");
        }
    }
}
