using AutoMapper;
using catalog.Application.Commands;
using catalog.Application.Responses;
using catalog.Core.Entities;
using catalog.Core.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Application.Mappers
{
   public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductBrand, BrandResponseDto>().ReverseMap();
            CreateMap<Product, ProductResponeDto>().ReverseMap();
            CreateMap<ProductType, TypesResponseDto>().ReverseMap();
            CreateMap<Pagination<Product>, Pagination<ProductResponeDto>>().ReverseMap();
            CreateMap<CreateProductCommand, Product>().ReverseMap();
             CreateMap<UpdateProductCommand, Product>().ReverseMap();
        }
    }
}
