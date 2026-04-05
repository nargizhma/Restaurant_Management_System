using Restaurant_Management.BLL.Services;
using Restaurant_Management.UI.Utilities;

namespace Restaurant_Management.UI.Menus;

public class TableMenu
{
    private readonly ITableService _tableService;
    private readonly IRestaurantService _restaurantService;

    public TableMenu(ITableService tableService, IRestaurantService restaurantService)
    {
        _tableService = tableService;
        _restaurantService = restaurantService;
    }

    public async Task ShowMenuAsync()
    {
        int restaurantId = await SelectRestaurantAsync();
        if (restaurantId <= 0)
            return;

        while (true)
        {
            ConsoleHelper.PrintHeader("Table Management");
            Console.WriteLine("1. Create Table");
            Console.WriteLine("2. View Tables");
            Console.WriteLine("3. Delete Table");
            Console.WriteLine("4. Back");
            Console.WriteLine();

            string choice = ConsoleHelper.ReadString("Select an option: ") ?? "";

            try
            {
                switch (choice)
                {
                    case "1":
                        await CreateTableAsync(restaurantId);
                        break;
                    case "2":
                        await ViewTablesAsync(restaurantId);
                        break;
                    case "3":
                        await DeleteTableAsync();
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

    private async Task CreateTableAsync(int restaurantId)
    {
        ConsoleHelper.PrintSubHeader("Create New Table");

        int tableNumber = ConsoleHelper.ReadInt("Enter table number: ");
        int capacity = ConsoleHelper.ReadInt("Enter table capacity: ");

        var table = await _tableService.CreateTableAsync(restaurantId, tableNumber, capacity);
        ConsoleHelper.PrintSuccess($"Table {table.TableNumber} created successfully with capacity {table.Capacity}");
    }

    private async Task ViewTablesAsync(int restaurantId)
    {
        ConsoleHelper.PrintSubHeader("Tables in Restaurant");

        var tables = await _tableService.GetTablesByRestaurantAsync(restaurantId);

        ConsoleHelper.PrintTable(tables,
            ("ID", t => t.Id.ToString()),
            ("Table No", t => t.TableNumber.ToString()),
            ("Capacity", t => t.Capacity.ToString()),
            ("Orders", t => t.OrderCount.ToString())
        );
    }

    private async Task DeleteTableAsync()
    {
        ConsoleHelper.PrintSubHeader("Delete Table");

        int id = ConsoleHelper.ReadInt("Enter table ID to delete: ");

        var table = await _tableService.GetTableAsync(id);
        if (table == null)
        {
            ConsoleHelper.PrintError("Table not found.");
            return;
        }

        Console.Write($"Are you sure you want to delete table {table.TableNumber}? (y/n): ");
        string confirm = Console.ReadLine() ?? "";

        if (confirm.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            await _tableService.DeleteTableAsync(id);
            ConsoleHelper.PrintSuccess("Table deleted successfully.");
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
