using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Restaurant_Management.DAL.Context;

public class RestaurantDbContextFactory : IDesignTimeDbContextFactory<RestaurantDbContext>
{
    // EDIT CONNECTION STRING HERE
    private const string ConnectionString = "Data Source=localhost;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name=\"SQL Server Management Studio\";Command Timeout=0";

    public RestaurantDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RestaurantDbContext>();
        optionsBuilder.UseSqlite(ConnectionString);

        return new RestaurantDbContext(optionsBuilder.Options);
    }
}
