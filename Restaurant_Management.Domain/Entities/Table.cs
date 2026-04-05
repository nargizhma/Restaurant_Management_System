namespace Restaurant_Management.Domain.Entities;

public class Table
{
    public int Id { get; set; }
    public int TableNumber { get; set; }
    public int Capacity { get; set; }
    public int RestaurantId { get; set; }
    public int OrderCount { get; set; }

    public Restaurant Restaurant { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
