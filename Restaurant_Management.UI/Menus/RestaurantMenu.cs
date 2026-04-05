using Restaurant_Management.BLL.Services;
using Restaurant_Management.UI.Utilities;

namespace Restaurant_Management.UI.Menus;

public class RestaurantMenu
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantMenu(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    public async Task ShowMenuAsync()
    {
        while (true)
        {
            ConsoleHelper.PrintHeader("Restaurant Management");
            Console.WriteLine("1. Create Restaurant");
            Console.WriteLine("2. View All Restaurants");
            Console.WriteLine("3. Delete Restaurant");
            Console.WriteLine("4. Back");
            Console.WriteLine();

            string choice = ConsoleHelper.ReadString("Select an option: ") ?? "";

            try
            {
                switch (choice)
                {
                    case "1":
                        await CreateRestaurantAsync();
                        break;
                    case "2":
                        await ViewRestaurantsAsync();
                        break;
                    case "3":
                        await DeleteRestaurantAsync();
                        break;
                    case "4":
                        return;
                    default:
                        ConsoleHelper.PrintError("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private async Task CreateRestaurantAsync()
    {
        ConsoleHelper.PrintSubHeader("Create New Restaurant");

        var name = ConsoleHelper.ReadString("Enter restaurant name: ");
        if (string.IsNullOrWhiteSpace(name))
        {
            ConsoleHelper.PrintError("Name cannot be empty.");
            return;
        }

        int branchCode = ConsoleHelper.ReadInt("Enter branch code (1-99): ");

        var restaurant = await _restaurantService.CreateRestaurantAsync(name, branchCode);
        ConsoleHelper.PrintSuccess($"Restaurant '{restaurant.Name}' created successfully with ID: {restaurant.Id}");
    }

    private async Task ViewRestaurantsAsync()
    {
        ConsoleHelper.PrintSubHeader("All Restaurants");

        var restaurants = await _restaurantService.GetAllRestaurantsAsync();

        ConsoleHelper.PrintTable(restaurants,
            ("ID", r => r.Id.ToString()),
            ("Name", r => r.Name),
            ("Branch Code", r => r.BranchCode.ToString()),
            ("Orders", r => r.TotalOrders.ToString()),
            ("Revenue", r => $"${r.TotalRevenue:F2}"),
            ("Active Tables", r => r.ActiveTables.ToString())
        );
    }

    private async Task DeleteRestaurantAsync()
    {
        ConsoleHelper.PrintSubHeader("Delete Restaurant");

        int id = ConsoleHelper.ReadInt("Enter restaurant ID to delete: ");

        var restaurant = await _restaurantService.GetRestaurantAsync(id);
        if (restaurant == null)
        {
            ConsoleHelper.PrintError("Restaurant not found.");
            return;
        }

        Console.Write($"Are you sure you want to delete '{restaurant.Name}'? (y/n): ");
        string confirm = Console.ReadLine() ?? "";

        if (confirm.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            await _restaurantService.DeleteRestaurantAsync(id);
            ConsoleHelper.PrintSuccess("Restaurant deleted successfully.");
        }
        else
        {
            ConsoleHelper.PrintInfo("Deletion cancelled.");
        }
    }
}
