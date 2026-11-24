using EntityAxis.Abstractions;

namespace Domain.Entities;

public class Product(string name, decimal price, int stockLevel, string imageUrl) : IEntityId<Guid>
{
    public string Name { get; } = name;
    public decimal Price { get; } = price;
    public int StockLevel { get; private set; } = stockLevel;
    public string ImageUrl { get; } = imageUrl;
    public Guid Id { get; } = Guid.NewGuid();

    public void UpdateStockLevel(int stockLevel)
    {
        if (stockLevel < 0) throw new ArgumentException("Stock level cannot be negative", nameof(stockLevel));

        StockLevel = stockLevel;
    }
}