using EntityAxis.Abstractions;

namespace Infrastructure.Persistence.Entities;

public class ShoppingCart : IEntityId<Guid>
{
    public Guid UserId { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new();
    public User? NavUser { get; set; }
    public Guid Id { get; set; }
}