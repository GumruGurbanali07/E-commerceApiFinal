using AutoMapper;
using Business.Concrete;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using Entities.DTOs.ProductDTOs;
using Entities.DTOs.SpecificationDTOs;
using Entities.DTOs.UserDTOs;
using Entities.DTOs.WishListDTO;

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
            CreateMap<Product, ProductFilterDTO>().ReverseMap();
            CreateMap<Product, ProductRecentDTO>().ReverseMap();
            CreateMap<SpecificationAddDTO, Specification>().ReverseMap();
            CreateMap<Specification, SpecificationListDTO>().ReverseMap();
            CreateMap<WishListAddItemDTO,WishList>().ReverseMap();
            CreateMap<WishList, WishListItemDTO>()
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Product.ProductName))
                .ForMember(x => x.Price, y => y.MapFrom(z => z.Product.Price));
        }

    }
}
