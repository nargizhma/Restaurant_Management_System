using Restaurant_Management.BLL.Exceptions;
using Restaurant_Management.DAL.Repositories;
using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.BLL.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly ITableRepository _tableRepository;
    private readonly IMenuItemRepository _menuItemRepository;

    public OrderService(
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository,
        IRestaurantRepository restaurantRepository,
        ITableRepository tableRepository,
        IMenuItemRepository menuItemRepository)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _restaurantRepository = restaurantRepository;
        _tableRepository = tableRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<Order> CreateOrderAsync(int restaurantId, int tableId)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
        if (restaurant == null)
            throw new BusinessException("Restaurant not found.");

        var table = await _tableRepository.GetByIdAsync(tableId);
        if (table == null)
            throw new BusinessException("Table not found.");

        if (table.RestaurantId != restaurantId)
            throw new BusinessException("Table does not belong to the specified restaurant.");

        var order = new Order
        {
            RestaurantId = restaurantId,
            TableId = tableId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = 0
        };

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();

        return order;
    }

    public async Task AddItemToOrderAsync(int orderId, int menuItemId, int quantity)
    {
        if (quantity <= 0)
            throw new BusinessException("Quantity must be greater than 0.");

        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new BusinessException("Order not found.");

        var menuItem = await _menuItemRepository.GetByIdAsync(menuItemId);
        if (menuItem == null)
            throw new BusinessException("Menu item not found.");

        if (menuItem.RestaurantId != order.RestaurantId)
            throw new BusinessException("Menu item does not belong to the restaurant.");

        var orderItem = new OrderItem
        {
            OrderId = orderId,
            MenuItemId = menuItemId,
            Quantity = quantity,
            Price = menuItem.Price
        };

        await _orderItemRepository.AddAsync(orderItem);
        await _orderItemRepository.SaveChangesAsync();

        await RecalculateOrderTotalsAsync(order.Id);
    }

    public async Task<Order?> GetOrderAsync(int id)
    {
        return await _orderRepository.GetByIdWithItemsAsync(id);
    }

    public async Task<IEnumerable<Order>> GetOrdersByRestaurantAsync(int restaurantId)
    {
        return await _orderRepository.GetOrdersByRestaurantAsync(restaurantId);
    }

    public async Task<IEnumerable<Order>> GetOrdersByTableAsync(int tableId)
    {
        return await _orderRepository.GetOrdersByTableAsync(tableId);
    }

    public async Task CompleteOrderAsync(int id)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id);
        if (order == null)
            throw new BusinessException("Order not found.");

        var restaurant = await _restaurantRepository.GetByIdAsync(order.RestaurantId);
        if (restaurant == null)
            throw new BusinessException("Restaurant not found.");

        var table = await _tableRepository.GetByIdAsync(order.TableId);
        if (table == null)
            throw new BusinessException("Table not found.");

        await RecalculateRestaurantStatsAsync(restaurant.Id);
        await _restaurantRepository.UpdateAsync(restaurant);
        await _restaurantRepository.SaveChangesAsync();
    }

    public async Task DeleteOrderAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            throw new BusinessException("Order not found.");

        await _orderRepository.DeleteAsync(id);
        await _orderRepository.SaveChangesAsync();

        await RecalculateRestaurantStatsAsync(order.RestaurantId);
    }

    private async Task RecalculateOrderTotalsAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(orderId);
        if (order == null)
            throw new BusinessException("Order not found.");

        order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.Price);

        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveChangesAsync();
    }

    private async Task RecalculateRestaurantStatsAsync(int restaurantId)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
        if (restaurant == null)
            throw new BusinessException("Restaurant not found.");

        var orders = await _orderRepository.GetOrdersByRestaurantAsync(restaurantId);
        var ordersList = orders.ToList();

        restaurant.TotalOrders = ordersList.Count;
        restaurant.TotalRevenue = ordersList.Sum(o => o.TotalAmount);

        var tableIds = ordersList.Select(o => o.TableId).Distinct();
        restaurant.ActiveTables = tableIds.Count();

        var tables = new List<Table>();
        foreach (var tableId in tableIds)
        {
            var table = await _tableRepository.GetByIdAsync(tableId);
            if (table != null)
            {
                var tableOrders = ordersList.Where(o => o.TableId == tableId).ToList();
                table.OrderCount = tableOrders.Count;
                tables.Add(table);
                await _tableRepository.UpdateAsync(table);
            }
        }

        var menuItems = await _menuItemRepository.GetMenuItemsByRestaurantAsync(restaurantId);
        foreach (var item in menuItems)
        {
            var totalSold = ordersList
                .SelectMany(o => o.OrderItems)
                .Where(oi => oi.MenuItemId == item.Id)
                .Sum(oi => oi.Quantity);

            item.TotalSold = totalSold;
            await _menuItemRepository.UpdateAsync(item);
        }

        await _restaurantRepository.UpdateAsync(restaurant);
        await _restaurantRepository.SaveChangesAsync();
    }
}
