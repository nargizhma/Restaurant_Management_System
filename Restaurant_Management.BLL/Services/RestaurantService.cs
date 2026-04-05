using Restaurant_Management.BLL.Exceptions;
using Restaurant_Management.DAL.Repositories;
using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.BLL.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IRestaurantRepository _restaurantRepository;

    public RestaurantService(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<Restaurant> CreateRestaurantAsync(string name, int branchCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessException("Restaurant name cannot be empty.");

        if (branchCode < 1 || branchCode > 99)
            throw new BusinessException("Branch code must be between 1 and 99.");

        var existingByName = await _restaurantRepository.GetByNameAsync(name);
        if (existingByName != null)
            throw new BusinessException("Restaurant name must be unique.");

        var existingByCode = await _restaurantRepository.GetByBranchCodeAsync(branchCode);
        if (existingByCode != null)
            throw new BusinessException("Branch code must be unique.");

        var restaurant = new Restaurant
        {
            Name = name,
            BranchCode = branchCode,
            TotalOrders = 0,
            TotalRevenue = 0,
            ActiveTables = 0
        };

        await _restaurantRepository.AddAsync(restaurant);
        await _restaurantRepository.SaveChangesAsync();

        return restaurant;
    }

    public async Task<Restaurant?> GetRestaurantAsync(int id)
    {
        return await _restaurantRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
    {
        return await _restaurantRepository.GetAllAsync();
    }

    public async Task UpdateRestaurantAsync(Restaurant restaurant)
    {
        if (restaurant == null)
            throw new BusinessException("Restaurant cannot be null.");

        var existing = await _restaurantRepository.GetByIdAsync(restaurant.Id);
        if (existing == null)
            throw new BusinessException("Restaurant not found.");

        if (!existing.Name.Equals(restaurant.Name, StringComparison.OrdinalIgnoreCase))
        {
            var duplicate = await _restaurantRepository.GetByNameAsync(restaurant.Name);
            if (duplicate != null && duplicate.Id != restaurant.Id)
                throw new BusinessException("Restaurant name must be unique.");
        }

        await _restaurantRepository.UpdateAsync(restaurant);
        await _restaurantRepository.SaveChangesAsync();
    }

    public async Task DeleteRestaurantAsync(int id)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(id);
        if (restaurant == null)
            throw new BusinessException("Restaurant not found.");

        await _restaurantRepository.DeleteAsync(id);
        await _restaurantRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<Restaurant>> GetRestaurantsSortedByRevenueAsync()
    {
        var restaurants = await _restaurantRepository.GetAllAsync();
        return restaurants
            .OrderByDescending(r => r.TotalRevenue)
            .ThenByDescending(r => r.TotalOrders)
            .ThenBy(r => r.Name)
            .ToList();
    }
}
