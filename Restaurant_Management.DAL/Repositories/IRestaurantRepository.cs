using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.DAL.Repositories;

public interface IRestaurantRepository
{
    Task<Restaurant?> GetByIdAsync(int id);
    Task<Restaurant?> GetByNameAsync(string name);
    Task<Restaurant?> GetByBranchCodeAsync(int branchCode);
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task AddAsync(Restaurant restaurant);
    Task UpdateAsync(Restaurant restaurant);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
