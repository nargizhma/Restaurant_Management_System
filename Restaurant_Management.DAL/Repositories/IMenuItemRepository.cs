using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.DAL.Repositories;

public interface IMenuItemRepository
{
    Task<MenuItem?> GetByIdAsync(int id);
    Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantAsync(int restaurantId);
    Task<MenuItem?> GetByNameAsync(int restaurantId, string name);
    Task AddAsync(MenuItem menuItem);
    Task UpdateAsync(MenuItem menuItem);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
