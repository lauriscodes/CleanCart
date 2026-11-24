using Application.Operations.Commands.User;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<OrderItem, ShoppingCartItem>().ReverseMap();
        CreateMap<UserCreateModel, User>();
    }
}