namespace Restaurant_Management.Domain.Entities;

public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
    public int RestaurantId { get; set; }
    public int TotalSold { get; set; }

    public Restaurant Restaurant { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
