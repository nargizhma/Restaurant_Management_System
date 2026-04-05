using Restaurant_Management.BLL.Services;
using Restaurant_Management.UI.Utilities;

namespace Restaurant_Management.UI.Menus;

public class OrderMenu
{
    private readonly IOrderService _orderService;
    private readonly IRestaurantService _restaurantService;
    private readonly ITableService _tableService;
    private readonly IMenuItemService _menuItemService;

    public OrderMenu(
        IOrderService orderService,
        IRestaurantService restaurantService,
        ITableService tableService,
        IMenuItemService menuItemService)
    {
        _orderService = orderService;
        _restaurantService = restaurantService;
        _tableService = tableService;
        _menuItemService = menuItemService;
    }

    public async Task ShowMenuAsync()
    {
        int restaurantId = await SelectRestaurantAsync();
        if (restaurantId <= 0)
            return;

        while (true)
        {
            ConsoleHelper.PrintHeader("Order Management");
            Console.WriteLine("1. Create New Order");
            Console.WriteLine("2. View Orders");
            Console.WriteLine("3. Complete Order");
            Console.WriteLine("4. Delete Order");
            Console.WriteLine("5. Back");
            Console.WriteLine();

            string choice = ConsoleHelper.ReadString("Select an option: ") ?? "";

            try
            {
                switch (choice)
                {
                    case "1":
                        await CreateOrderAsync(restaurantId);
                        break;
                    case "2":
                        await ViewOrdersAsync(restaurantId);
                        break;
                    case "3":
                        await CompleteOrderAsync();
                        break;
                    case "4":
                        await DeleteOrderAsync();
                        break;
                    case "5":
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

    private async Task CreateOrderAsync(int restaurantId)
    {
        ConsoleHelper.PrintSubHeader("Create New Order");

        var tables = await _tableService.GetTablesByRestaurantAsync(restaurantId);
        var tableList = tables.ToList();

        if (!tableList.Any())
        {
            ConsoleHelper.PrintError("No tables available in this restaurant.");
            return;
        }

        Console.WriteLine("\nAvailable tables:");
        ConsoleHelper.PrintTable(tableList,
            ("ID", t => t.Id.ToString()),
            ("Table No", t => t.TableNumber.ToString()),
            ("Capacity", t => t.Capacity.ToString())
        );

        int tableId = ConsoleHelper.ReadInt("Select table ID: ");

        var table = await _tableService.GetTableAsync(tableId);
        if (table == null || table.RestaurantId != restaurantId)
        {
            ConsoleHelper.PrintError("Invalid table selection.");
            return;
        }

        var order = await _orderService.CreateOrderAsync(restaurantId, tableId);
        ConsoleHelper.PrintSuccess($"Order {order.Id} created for table {table.TableNumber}");

        while (true)
        {
            Console.Write("\nAdd item to order? (y/n): ");
            string addItem = Console.ReadLine() ?? "";

            if (!addItem.Equals("y", StringComparison.OrdinalIgnoreCase))
                break;

            var menuItems = await _menuItemService.GetMenuItemsByRestaurantAsync(restaurantId);
            var menuList = menuItems.ToList();

            if (!menuList.Any())
            {
                ConsoleHelper.PrintError("No menu items available.");
                break;
            }

            Console.WriteLine("\nAvailable menu items:");
            ConsoleHelper.PrintTable(menuList,
                ("ID", m => m.Id.ToString()),
                ("Name", m => m.Name),
                ("Price", m => $"${m.Price:F2}"),
                ("Category", m => m.Category)
            );

            int menuItemId = ConsoleHelper.ReadInt("Select menu item ID: ");
            int quantity = ConsoleHelper.ReadInt("Enter quantity: ");

            await _orderService.AddItemToOrderAsync(order.Id, menuItemId, quantity);
            ConsoleHelper.PrintSuccess("Item added to order.");
        }
    }

    private async Task ViewOrdersAsync(int restaurantId)
    {
        ConsoleHelper.PrintSubHeader("Orders");

        var orders = await _orderService.GetOrdersByRestaurantAsync(restaurantId);

        ConsoleHelper.PrintTable(orders,
            ("ID", o => o.Id.ToString()),
            ("Table ID", o => o.TableId.ToString()),
            ("Date", o => o.OrderDate.ToString("yyyy-MM-dd HH:mm")),
            ("Total", o => $"${o.TotalAmount:F2}"),
            ("Items", o => o.OrderItems.Count.ToString())
        );
    }

    private async Task CompleteOrderAsync()
    {
        ConsoleHelper.PrintSubHeader("Complete Order");

        int orderId = ConsoleHelper.ReadInt("Enter order ID to complete: ");

        var order = await _orderService.GetOrderAsync(orderId);
        if (order == null)
        {
            ConsoleHelper.PrintError("Order not found.");
            return;
        }

        await _orderService.CompleteOrderAsync(orderId);
        ConsoleHelper.PrintSuccess($"Order {orderId} completed. Total: ${order.TotalAmount:F2}");
    }

    private async Task DeleteOrderAsync()
    {
        ConsoleHelper.PrintSubHeader("Delete Order");

        int orderId = ConsoleHelper.ReadInt("Enter order ID to delete: ");

        var order = await _orderService.GetOrderAsync(orderId);
        if (order == null)
        {
            ConsoleHelper.PrintError("Order not found.");
            return;
        }

        Console.Write($"Are you sure you want to delete order {orderId}? (y/n): ");
        string confirm = Console.ReadLine() ?? "";

        if (confirm.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            await _orderService.DeleteOrderAsync(orderId);
            ConsoleHelper.PrintSuccess("Order deleted successfully.");
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
