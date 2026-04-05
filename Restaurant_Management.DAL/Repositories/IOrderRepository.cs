using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.DAL.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<Order?> GetByIdWithItemsAsync(int id);
    Task<IEnumerable<Order>> GetOrdersByRestaurantAsync(int restaurantId);
    Task<IEnumerable<Order>> GetOrdersByTableAsync(int tableId);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
