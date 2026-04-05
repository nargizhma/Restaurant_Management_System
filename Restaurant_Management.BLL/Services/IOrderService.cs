using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.BLL.Services;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(int restaurantId, int tableId);
    Task AddItemToOrderAsync(int orderId, int menuItemId, int quantity);
    Task<Order?> GetOrderAsync(int id);
    Task<IEnumerable<Order>> GetOrdersByRestaurantAsync(int restaurantId);
    Task<IEnumerable<Order>> GetOrdersByTableAsync(int tableId);
    Task CompleteOrderAsync(int id);
    Task DeleteOrderAsync(int id);
}
