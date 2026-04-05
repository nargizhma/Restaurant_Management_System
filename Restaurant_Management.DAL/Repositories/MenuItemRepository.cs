using Microsoft.EntityFrameworkCore;
using Restaurant_Management.DAL.Context;
using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.DAL.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly RestaurantDbContext _context;

    public MenuItemRepository(RestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<MenuItem?> GetByIdAsync(int id)
    {
        return await _context.MenuItems.FindAsync(id);
    }

    public async Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantAsync(int restaurantId)
    {
        return await _context.MenuItems
            .Where(m => m.RestaurantId == restaurantId)
            .ToListAsync();
    }

    public async Task<MenuItem?> GetByNameAsync(int restaurantId, string name)
    {
        return await _context.MenuItems
            .FirstOrDefaultAsync(m => m.RestaurantId == restaurantId && m.Name == name);
    }

    public async Task AddAsync(MenuItem menuItem)
    {
        await _context.MenuItems.AddAsync(menuItem);
    }

    public async Task UpdateAsync(MenuItem menuItem)
    {
        _context.MenuItems.Update(menuItem);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var menuItem = await GetByIdAsync(id);
        if (menuItem != null)
        {
            _context.MenuItems.Remove(menuItem);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
