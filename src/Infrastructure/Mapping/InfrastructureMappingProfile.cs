using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using OrderItem = Infrastructure.Persistence.Entities.OrderItem;
using ShoppingCart = Infrastructure.Persistence.Entities.ShoppingCart;

namespace Infrastructure.Mapping;

internal sealed class InfrastructureMappingProfile : Profile
{
    public InfrastructureMappingProfile()
    {
        // Ignore EF navigation properties globally
        ShouldMapProperty = p => !p.Name.StartsWith("Nav", StringComparison.Ordinal);

        ConfigureOrderMappings();
        ConfigureShoppingCartMappings();
        ConfigureProductMappings();
        ConfigureUserMappings();
    }

    /* =========================================================
     * ORDER
     * ========================================================= */

    private void ConfigureOrderMappings()
    {
        CreateMap<Persistence.Entities.Order, Order>()
            .ConstructUsing(CreateOrder)
            .ForMember(d => d.Items, o => o.Ignore())
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreatedOn));

        CreateMap<Order, Persistence.Entities.Order>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<OrderItem, Domain.Entities.OrderItem>()
            .ConstructUsing(i =>
                new Domain.Entities.OrderItem(
                    i.ProductId,
                    i.ProductName,
                    i.ProductPrice,
                    i.Quantity));

        CreateMap<Domain.Entities.OrderItem, OrderItem>();
    }

    private static Order CreateOrder(
        Persistence.Entities.Order src,
        ResolutionContext resolutionContext)
    {
        return new Order(
            src.UserId,
            src.Items.Select(i =>
                    new Domain.Entities.OrderItem(
                        i.ProductId,
                        i.ProductName,
                        i.ProductPrice,
                        i.Quantity))
                .ToList(),
            src.TotalAmount
        );
    }

    /* =========================================================
     * SHOPPING CART
     * ========================================================= */

    private void ConfigureShoppingCartMappings()
    {
        CreateMap<Domain.Entities.ShoppingCart, ShoppingCart>();

        CreateMap<ShoppingCart, Domain.Entities.ShoppingCart>()
            .ConstructUsing(src => new Domain.Entities.ShoppingCart(src.UserId))
            .ForMember(d => d.Items, o => o.Ignore())
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            .AfterMap(HydrateCartItems);

        CreateMap<ShoppingCartItem, Persistence.Entities.ShoppingCartItem>().ReverseMap();
    }

    private static void HydrateCartItems(ShoppingCart src, Domain.Entities.ShoppingCart dest)
    {
        foreach (var item in src.Items)
            dest.AddItem(
                item.ProductId,
                item.ProductName,
                item.ProductPrice,
                item.Quantity);
    }

    /* =========================================================
     * PRODUCT
     * ========================================================= */

    private void ConfigureProductMappings()
    {
        CreateMap<Product, Persistence.Entities.Product>().ReverseMap();
    }

    /* =========================================================
     * USER
     * ========================================================= */

    private void ConfigureUserMappings()
    {
        CreateMap<User, Persistence.Entities.User>();

        CreateMap<Persistence.Entities.User, User>()
            .ConstructUsing(CreateUser)
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Roles, o => o.Ignore());
    }

    private static User CreateUser(Persistence.Entities.User src, ResolutionContext resolutionContext)
    {
        return new User(
            src.Username,
            src.Email,
            src.FullName,
            src.Roles.Select(Enum.Parse<UserRole>).ToList()
        );
    }
}