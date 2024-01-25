using AutoMapper;
using Warehouse.Application.Models;
using Warehouse.Domain.Entities;

namespace Warehouse.Application.Mapper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateProductModel, Product>();
        CreateMap<CreateCartModel, Cart>();
        CreateMap<SignUpModel, Client>();
        CreateMap<Product, ReturnProductModel>();
        CreateMap<Cart, ReturnCartModel>();
        CreateMap<Order, ReturnOrderModel>();
        CreateMap<CreateOrderModel, Order>();
        CreateMap<InternalSignUpModel, Worker>();
    }
}