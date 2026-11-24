using Domain.Entities;
using MediatR;

namespace Application.Operations.UseCases.AccessCustomerData;

public class AccessCustomerShoppingCartQuery(Guid customerUserId) : IRequest<ShoppingCart?>
{
    public Guid CustomerUserId { get; } = customerUserId;
}