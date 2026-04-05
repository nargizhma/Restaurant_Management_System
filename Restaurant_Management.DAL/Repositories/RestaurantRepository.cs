using Microsoft.EntityFrameworkCore;
using Restaurant_Management.DAL.Context;
using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.DAL.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly RestaurantDbContext _context;

    public RestaurantRepository(RestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        return await _context.Restaurants.FindAsync(id);
    }

    public async Task<Restaurant?> GetByNameAsync(string name)
    {
        return await _context.Restaurants.FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<Restaurant?> GetByBranchCodeAsync(int branchCode)
    {
        return await _context.Restaurants.FirstOrDefaultAsync(r => r.BranchCode == branchCode);
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return await _context.Restaurants.ToListAsync();
    }

    public async Task AddAsync(Restaurant restaurant)
    {
        await _context.Restaurants.AddAsync(restaurant);
    }

    public async Task UpdateAsync(Restaurant restaurant)
    {
        _context.Restaurants.Update(restaurant);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var restaurant = await GetByIdAsync(id);
        if (restaurant != null)
        {
            _context.Restaurants.Remove(restaurant);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
