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
    }
}