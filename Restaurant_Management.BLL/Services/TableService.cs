using Restaurant_Management.BLL.Exceptions;
using Restaurant_Management.DAL.Repositories;
using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.BLL.Services;

public class TableService : ITableService
{
    private readonly ITableRepository _tableRepository;
    private readonly IRestaurantRepository _restaurantRepository;

    public TableService(ITableRepository tableRepository, IRestaurantRepository restaurantRepository)
    {
        _tableRepository = tableRepository;
        _restaurantRepository = restaurantRepository;
    }

    public async Task<Table> CreateTableAsync(int restaurantId, int tableNumber, int capacity)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
        if (restaurant == null)
            throw new BusinessException("Restaurant not found.");

        if (tableNumber <= 0)
            throw new BusinessException("Table number must be greater than 0.");

        if (capacity <= 0)
            throw new BusinessException("Capacity must be greater than 0.");

        var existingTable = await _tableRepository.GetByTableNumberAsync(restaurantId, tableNumber);
        if (existingTable != null)
            throw new BusinessException("Table number must be unique within the restaurant.");

        var table = new Table
        {
            TableNumber = tableNumber,
            Capacity = capacity,
            RestaurantId = restaurantId,
            OrderCount = 0
        };

        await _tableRepository.AddAsync(table);
        await _tableRepository.SaveChangesAsync();

        return table;
    }

    public async Task<Table?> GetTableAsync(int id)
    {
        return await _tableRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Table>> GetTablesByRestaurantAsync(int restaurantId)
    {
        return await _tableRepository.GetTablesByRestaurantAsync(restaurantId);
    }

    public async Task UpdateTableAsync(Table table)
    {
        if (table == null)
            throw new BusinessException("Table cannot be null.");

        var existing = await _tableRepository.GetByIdAsync(table.Id);
        if (existing == null)
            throw new BusinessException("Table not found.");

        if (table.Capacity <= 0)
            throw new BusinessException("Capacity must be greater than 0.");

        await _tableRepository.UpdateAsync(table);
        await _tableRepository.SaveChangesAsync();
    }

    public async Task DeleteTableAsync(int id)
    {
        var table = await _tableRepository.GetByIdAsync(id);
        if (table == null)
            throw new BusinessException("Table not found.");

        await _tableRepository.DeleteAsync(id);
        await _tableRepository.SaveChangesAsync();
    }
}
