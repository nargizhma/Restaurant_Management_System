using Restaurant_Management.BLL.Services;
using Restaurant_Management.UI.Utilities;

namespace Restaurant_Management.UI.Menus;

public class MenuItemMenu
{
    private readonly IMenuItemService _menuItemService;
    private readonly IRestaurantService _restaurantService;

    public MenuItemMenu(IMenuItemService menuItemService, IRestaurantService restaurantService)
    {
        _menuItemService = menuItemService;
        _restaurantService = restaurantService;
    }

    public async Task ShowMenuAsync()
    {
        int restaurantId = await SelectRestaurantAsync();
        if (restaurantId <= 0)
            return;

        while (true)
        {
            ConsoleHelper.PrintHeader("Menu Item Management");
            Console.WriteLine("1. Create Menu Item");
            Console.WriteLine("2. View Menu Items");
            Console.WriteLine("3. Delete Menu Item");
            Console.WriteLine("4. Back");
            Console.WriteLine();

            string choice = ConsoleHelper.ReadString("Select an option: ") ?? "";

            try
            {
                switch (choice)
                {
                    case "1":
                        await CreateMenuItemAsync(restaurantId);
                        break;
                    case "2":
                        await ViewMenuItemsAsync(restaurantId);
                        break;
                    case "3":
                        await DeleteMenuItemAsync();
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

    private async Task CreateMenuItemAsync(int restaurantId)
    {
        ConsoleHelper.PrintSubHeader("Create New Menu Item");

        var name = ConsoleHelper.ReadString("Enter item name: ");
        if (string.IsNullOrWhiteSpace(name))
        {
            ConsoleHelper.PrintError("Name cannot be empty.");
            return;
        }

        decimal price = ConsoleHelper.ReadDecimal("Enter price: $");
        var category = ConsoleHelper.ReadString("Enter category: ");
        if (string.IsNullOrWhiteSpace(category))
        {
            ConsoleHelper.PrintError("Category cannot be empty.");
            return;
        }

        var menuItem = await _menuItemService.CreateMenuItemAsync(restaurantId, name, price, category);
        ConsoleHelper.PrintSuccess($"Menu item '{menuItem.Name}' created successfully at ${menuItem.Price:F2}");
    }

    private async Task ViewMenuItemsAsync(int restaurantId)
    {
        ConsoleHelper.PrintSubHeader("Menu Items");

        var menuItems = await _menuItemService.GetMenuItemsByRestaurantAsync(restaurantId);

        ConsoleHelper.PrintTable(menuItems,
            ("ID", m => m.Id.ToString()),
            ("Name", m => m.Name),
            ("Price", m => $"${m.Price:F2}"),
            ("Category", m => m.Category),
            ("Sold", m => m.TotalSold.ToString())
        );
    }

    private async Task DeleteMenuItemAsync()
    {
        ConsoleHelper.PrintSubHeader("Delete Menu Item");

        int id = ConsoleHelper.ReadInt("Enter menu item ID to delete: ");

        var menuItem = await _menuItemService.GetMenuItemAsync(id);
        if (menuItem == null)
        {
            ConsoleHelper.PrintError("Menu item not found.");
            return;
        }

        Console.Write($"Are you sure you want to delete '{menuItem.Name}'? (y/n): ");
        string confirm = Console.ReadLine() ?? "";

        if (confirm.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            await _menuItemService.DeleteMenuItemAsync(id);
            ConsoleHelper.PrintSuccess("Menu item deleted successfully.");
        }
        else
        {
            ConsoleHelper.PrintInfo("Deletion cancelled.");
        }
    }

    private async Task<int> SelectRestaurantAsync()
    {
        ConsoleHelper.PrintSubHeader("Select Restaurant");

        var restaurants = await _restaurantService.GetAllRestaurantsAsync();
        var restaurantList = restaurants.ToList();

        if (!restaurantList.Any())
        {
            ConsoleHelper.PrintError("No restaurants found. Please create one first.");
            return 0;
        }

        ConsoleHelper.PrintTable(restaurantList,
            ("ID", r => r.Id.ToString()),
            ("Name", r => r.Name)
        );

        int restaurantId = ConsoleHelper.ReadInt("Select restaurant ID: ");
        return restaurantId;
    }
}
