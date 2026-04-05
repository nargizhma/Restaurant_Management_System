using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant_Management.Domain.Entities;

namespace Restaurant_Management.DAL.Configurations;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(m => m.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.RestaurantId)
            .IsRequired();

        builder.HasIndex(m => new { m.RestaurantId, m.Name })
            .IsUnique()
            .HasDatabaseName("UK_MenuItem_RestaurantId_Name");

        builder.HasMany(m => m.OrderItems)
            .WithOne(oi => oi.MenuItem)
            .HasForeignKey(oi => oi.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
