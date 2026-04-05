using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.BLL.Services;

public interface IRestaurantService
{
    Task<Restaurant> CreateRestaurantAsync(string name, int branchCode);
    Task<Restaurant?> GetRestaurantAsync(int id);
    Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
    Task UpdateRestaurantAsync(Restaurant restaurant);
    Task DeleteRestaurantAsync(int id);
    Task<IEnumerable<Restaurant>> GetRestaurantsSortedByRevenueAsync();
}
