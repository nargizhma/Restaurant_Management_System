using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.DAL.Repositories;

public interface IOrderItemRepository
{
    Task<OrderItem?> GetByIdAsync(int id);
    Task<IEnumerable<OrderItem>> GetItemsByOrderAsync(int orderId);
    Task AddAsync(OrderItem orderItem);
    Task UpdateAsync(OrderItem orderItem);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
