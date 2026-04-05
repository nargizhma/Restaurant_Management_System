using Microsoft.EntityFrameworkCore;
using Restaurant_Management.DAL.Context;
using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.DAL.Repositories;

public class TableRepository : ITableRepository
{
    private readonly RestaurantDbContext _context;

    public TableRepository(RestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<Table?> GetByIdAsync(int id)
    {
        return await _context.Tables.FindAsync(id);
    }

    public async Task<IEnumerable<Table>> GetTablesByRestaurantAsync(int restaurantId)
    {
        return await _context.Tables
            .Where(t => t.RestaurantId == restaurantId)
            .ToListAsync();
    }

    public async Task<Table?> GetByTableNumberAsync(int restaurantId, int tableNumber)
    {
        return await _context.Tables
            .FirstOrDefaultAsync(t => t.RestaurantId == restaurantId && t.TableNumber == tableNumber);
    }

    public async Task AddAsync(Table table)
    {
        await _context.Tables.AddAsync(table);
    }

    public async Task UpdateAsync(Table table)
    {
        _context.Tables.Update(table);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var table = await GetByIdAsync(id);
        if (table != null)
        {
            _context.Tables.Remove(table);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
