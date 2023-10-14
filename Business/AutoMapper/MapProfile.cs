﻿using AutoMapper;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using Entities.DTOs.ProductDTOs;
using Entities.DTOs.UserDTOs;

namespace Business.AutoMapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<CategoryCreateDTO, Category>().ReverseMap();
            CreateMap<CategoryUpdateDTO, Category>().ReverseMap();
            CreateMap<UserLoginDTO, User>().ReverseMap();
            CreateMap<UserRegisterDTO, User>().ReverseMap();
            CreateMap<Category, CategoryAdminListDTO>().ReverseMap();
            CreateMap<Category, CategoryHomeNavbarDTO>().ReverseMap();
            CreateMap<Category, CategoryFeaturedDTO>().ReverseMap();
            CreateMap<ProductCreateDTO, Product>().ReverseMap();
            CreateMap<ProductUpdateDTO, Product>().ReverseMap();
            CreateMap<Product, ProductDetailDTO>().ReverseMap();
            CreateMap<Product, ProductFeaturedDTO>().ReverseMap();
            CreateMap<Product, ProductRecentDTO>().ReverseMap();
        }

    }
}
