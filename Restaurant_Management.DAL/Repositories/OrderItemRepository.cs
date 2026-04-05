using Microsoft.EntityFrameworkCore;
using Restaurant_Management.DAL.Context;
using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.DAL.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly RestaurantDbContext _context;

    public OrderItemRepository(RestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<OrderItem?> GetByIdAsync(int id)
    {
        return await _context.OrderItems.FindAsync(id);
    }

    public async Task<IEnumerable<OrderItem>> GetItemsByOrderAsync(int orderId)
    {
        return await _context.OrderItems
            .Where(oi => oi.OrderId == orderId)
            .Include(oi => oi.MenuItem)
            .ToListAsync();
    }

    public async Task AddAsync(OrderItem orderItem)
    {
        await _context.OrderItems.AddAsync(orderItem);
    }

    public async Task UpdateAsync(OrderItem orderItem)
    {
        _context.OrderItems.Update(orderItem);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var orderItem = await GetByIdAsync(id);
        if (orderItem != null)
        {
            _context.OrderItems.Remove(orderItem);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
