using Application.Interfaces.Data;
using Domain.Entities;
using MediatR;

namespace Application.Operations.UseCases.AccessCustomerData;

public class AccessCustomerShoppingCartQueryHandler(IShoppingCartQueryRepository shoppingCartQueryRepository)
    : IRequestHandler<AccessCustomerShoppingCartQuery, ShoppingCart?>
{
    public Task<ShoppingCart?> Handle(AccessCustomerShoppingCartQuery request, CancellationToken cancellationToken)
    {
        return shoppingCartQueryRepository.GetByUserIdAsync(request.CustomerUserId);
    }
}