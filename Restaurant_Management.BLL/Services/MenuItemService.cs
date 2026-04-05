using Restaurant_Management.BLL.Exceptions;
using Restaurant_Management.DAL.Repositories;
using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.BLL.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly IRestaurantRepository _restaurantRepository;

    public MenuItemService(IMenuItemRepository menuItemRepository, IRestaurantRepository restaurantRepository)
    {
        _menuItemRepository = menuItemRepository;
        _restaurantRepository = restaurantRepository;
    }

    public async Task<MenuItem> CreateMenuItemAsync(int restaurantId, string name, decimal price, string category)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
        if (restaurant == null)
            throw new BusinessException("Restaurant not found.");

        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessException("Menu item name cannot be empty.");

        if (price <= 0)
            throw new BusinessException("Price must be greater than 0.");

        if (string.IsNullOrWhiteSpace(category))
            throw new BusinessException("Category cannot be empty.");

        var existingItem = await _menuItemRepository.GetByNameAsync(restaurantId, name);
        if (existingItem != null)
            throw new BusinessException("Menu item name must be unique within the restaurant.");

        var menuItem = new MenuItem
        {
            Name = name,
            Price = price,
            Category = category,
            RestaurantId = restaurantId,
            TotalSold = 0
        };

        await _menuItemRepository.AddAsync(menuItem);
        await _menuItemRepository.SaveChangesAsync();

        return menuItem;
    }

    public async Task<MenuItem?> GetMenuItemAsync(int id)
    {
        return await _menuItemRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantAsync(int restaurantId)
    {
        return await _menuItemRepository.GetMenuItemsByRestaurantAsync(restaurantId);
    }

    public async Task UpdateMenuItemAsync(MenuItem menuItem)
    {
        if (menuItem == null)
            throw new BusinessException("Menu item cannot be null.");

        var existing = await _menuItemRepository.GetByIdAsync(menuItem.Id);
        if (existing == null)
            throw new BusinessException("Menu item not found.");

        if (menuItem.Price <= 0)
            throw new BusinessException("Price must be greater than 0.");

        await _menuItemRepository.UpdateAsync(menuItem);
        await _menuItemRepository.SaveChangesAsync();
    }

    public async Task DeleteMenuItemAsync(int id)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(id);
        if (menuItem == null)
            throw new BusinessException("Menu item not found.");

        await _menuItemRepository.DeleteAsync(id);
        await _menuItemRepository.SaveChangesAsync();
    }
}
