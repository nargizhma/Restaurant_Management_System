using Restaurant_Management.BLL.Services;
using Restaurant_Management.UI.Utilities;

namespace Restaurant_Management.UI.Menus;

public class ReportMenu
{
    private readonly IReportService _reportService;
    private readonly IRestaurantService _restaurantService;

    public ReportMenu(IReportService reportService, IRestaurantService restaurantService)
    {
        _reportService = reportService;
        _restaurantService = restaurantService;
    }

    public async Task ShowMenuAsync()
    {
        while (true)
        {
            ConsoleHelper.PrintHeader("Reports");
            Console.WriteLine("1. Restaurant State Report");
            Console.WriteLine("2. Restaurants Ranked by Revenue");
            Console.WriteLine("3. Most Sold Menu Items");
            Console.WriteLine("4. Back");
            Console.WriteLine();

            string choice = ConsoleHelper.ReadString("Select an option: ") ?? "";

            try
            {
                switch (choice)
                {
                    case "1":
                        await ShowRestaurantStateAsync();
                        break;
                    case "2":
                        await ShowRevenueRankingAsync();
                        break;
                    case "3":
                        await ShowMostSoldItemsAsync();
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

    private async Task ShowRestaurantStateAsync()
    {
        ConsoleHelper.PrintSubHeader("Restaurant State Report");

        int restaurantId = await SelectRestaurantAsync();
        if (restaurantId <= 0)
            return;

        var report = await _reportService.GetRestaurantStateAsync(restaurantId);

        Console.WriteLine();
        Console.WriteLine($"Restaurant Name:  {report.Name}");
        Console.WriteLine($"Total Orders:    {report.TotalOrders}");
        Console.WriteLine($"Total Revenue:   ${report.TotalRevenue:F2}");
        Console.WriteLine($"Active Tables:   {report.ActiveTables}");
        Console.WriteLine();
    }

    private async Task ShowRevenueRankingAsync()
    {
        ConsoleHelper.PrintSubHeader("Restaurants Ranked by Revenue");

        var reports = await _reportService.GetRestaurantsSortedByRevenueAsync();
        var reportList = reports.ToList();

        ConsoleHelper.PrintTable(reportList,
            ("Rank", r => (reportList.IndexOf(r) + 1).ToString()),
            ("Name", r => r.Name),
            ("Revenue", r => $"${r.TotalRevenue:F2}"),
            ("Orders", r => r.TotalOrders.ToString())
        );
    }

    private async Task ShowMostSoldItemsAsync()
    {
        ConsoleHelper.PrintSubHeader("Most Sold Menu Items");

        int restaurantId = await SelectRestaurantAsync();
        if (restaurantId <= 0)
            return;

        var items = await _reportService.GetMostSoldMenuItemsAsync(restaurantId);

        ConsoleHelper.PrintTable(items,
            ("Name", m => m.Name),
            ("Total Sold", m => m.TotalSold.ToString())
        );
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
