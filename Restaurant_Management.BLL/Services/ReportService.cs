using Restaurant_Management.BLL.Exceptions;
using Restaurant_Management.DAL.Repositories;

namespace Restaurant_Management.BLL.Services;

public class ReportService : IReportService
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IMenuItemRepository _menuItemRepository;

    public ReportService(IRestaurantRepository restaurantRepository, IMenuItemRepository menuItemRepository)
    {
        _restaurantRepository = restaurantRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<RestaurantReportDto> GetRestaurantStateAsync(int restaurantId)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
        if (restaurant == null)
            throw new BusinessException("Restaurant not found.");

        return new RestaurantReportDto
        {
            Name = restaurant.Name,
            TotalOrders = restaurant.TotalOrders,
            TotalRevenue = restaurant.TotalRevenue,
            ActiveTables = restaurant.ActiveTables
        };
    }

    public async Task<IEnumerable<RestaurantRevenueReportDto>> GetRestaurantsSortedByRevenueAsync()
    {
        var restaurants = await _restaurantRepository.GetAllAsync();

        return restaurants
            .OrderByDescending(r => r.TotalRevenue)
            .ThenByDescending(r => r.TotalOrders)
            .ThenBy(r => r.Name)
            .Select(r => new RestaurantRevenueReportDto
            {
                Name = r.Name,
                TotalRevenue = r.TotalRevenue,
                TotalOrders = r.TotalOrders
            })
            .ToList();
    }

    public async Task<IEnumerable<MenuItemSalesReportDto>> GetMostSoldMenuItemsAsync(int restaurantId)
    {
        var menuItems = await _menuItemRepository.GetMenuItemsByRestaurantAsync(restaurantId);

        return menuItems
            .OrderByDescending(m => m.TotalSold)
            .ThenBy(m => m.Name)
            .Select(m => new MenuItemSalesReportDto
            {
                Name = m.Name,
                TotalSold = m.TotalSold
            })
            .ToList();
    }
}
