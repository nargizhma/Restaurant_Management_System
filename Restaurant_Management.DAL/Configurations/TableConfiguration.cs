using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.DAL.Configurations;

public class TableConfiguration : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TableNumber)
            .IsRequired();

        builder.Property(t => t.Capacity)
            .IsRequired();

        builder.Property(t => t.RestaurantId)
            .IsRequired();

        builder.HasIndex(t => new { t.RestaurantId, t.TableNumber })
            .IsUnique()
            .HasDatabaseName("UK_Table_RestaurantId_TableNumber");

        builder.HasMany(t => t.Orders)
            .WithOne(o => o.Table)
            .HasForeignKey(o => o.TableId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
