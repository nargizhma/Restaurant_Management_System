using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.BLL.Services;

public interface ITableService
{
    Task<Table> CreateTableAsync(int restaurantId, int tableNumber, int capacity);
    Task<Table?> GetTableAsync(int id);
    Task<IEnumerable<Table>> GetTablesByRestaurantAsync(int restaurantId);
    Task UpdateTableAsync(Table table);
    Task DeleteTableAsync(int id);
}
