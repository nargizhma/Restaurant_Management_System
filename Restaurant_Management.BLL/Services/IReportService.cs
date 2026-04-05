using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.BLL.Services;

public interface IReportService
{
    Task<RestaurantReportDto> GetRestaurantStateAsync(int restaurantId);
    Task<IEnumerable<RestaurantRevenueReportDto>> GetRestaurantsSortedByRevenueAsync();
    Task<IEnumerable<MenuItemSalesReportDto>> GetMostSoldMenuItemsAsync(int restaurantId);
}

public class RestaurantReportDto
{
    public string Name { get; set; } = null!;
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int ActiveTables { get; set; }
}

public class RestaurantRevenueReportDto
{
    public string Name { get; set; } = null!;
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
}

public class MenuItemSalesReportDto
{
    public string Name { get; set; } = null!;
    public int TotalSold { get; set; }
}
