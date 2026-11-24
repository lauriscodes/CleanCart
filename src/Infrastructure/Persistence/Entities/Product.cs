using EntityAxis.Abstractions;

namespace Infrastructure.Persistence.Entities;

public class Product : IEntityId<Guid>
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockLevel { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public Guid Id { get; set; }
}