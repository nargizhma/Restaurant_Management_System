using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.BLL.Services;

public interface IMenuItemService
{
    Task<MenuItem> CreateMenuItemAsync(int restaurantId, string name, decimal price, string category);
    Task<MenuItem?> GetMenuItemAsync(int id);
    Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantAsync(int restaurantId);
    Task UpdateMenuItemAsync(MenuItem menuItem);
    Task DeleteMenuItemAsync(int id);
}
