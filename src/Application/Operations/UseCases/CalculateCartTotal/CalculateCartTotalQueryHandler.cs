using Application.Interfaces.Data;
using Domain.Entities;
using MediatR;

namespace Application.Operations.UseCases.CalculateCartTotal;

public class CalculateCartTotalQueryHandler(IShoppingCartQueryRepository shoppingCartQueryRepository)
    : IRequestHandler<CalculateCartTotalQuery, decimal>
{
    public async Task<decimal> Handle(CalculateCartTotalQuery query, CancellationToken cancellationToken)
    {
        var shoppingCart = await shoppingCartQueryRepository.GetByUserIdAsync(query.UserId);

        if (shoppingCart == null) return 0;

        var subtotal = CalculateSubtotal(shoppingCart);
        var taxes = CalculateTaxes(subtotal);
        var total = subtotal + taxes;

        return total;
    }

    private static decimal CalculateSubtotal(ShoppingCart shoppingCart)
    {
        return shoppingCart.Items.Sum(item => item.ProductPrice * item.Quantity);
    }

    private static decimal CalculateTaxes(decimal subtotal)
    {
        // Implement tax calculation logic here based on business rules.
        // For simplicity, we assume a flat tax rate in this example.
        const decimal taxRate = 0.08M;
        return subtotal * taxRate;
    }
}