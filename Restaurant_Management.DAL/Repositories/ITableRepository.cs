using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.DAL.Repositories;

public interface ITableRepository
{
    Task<Table?> GetByIdAsync(int id);
    Task<IEnumerable<Table>> GetTablesByRestaurantAsync(int restaurantId);
    Task<Table?> GetByTableNumberAsync(int restaurantId, int tableNumber);
    Task AddAsync(Table table);
    Task UpdateAsync(Table table);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
