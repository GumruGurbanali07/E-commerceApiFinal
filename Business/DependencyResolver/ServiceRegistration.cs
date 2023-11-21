
using AutoMapper;
using Business.Abstract;
using Business.AutoMapper;
using Business.Concrete;
using Core.Utilities.MailHelper;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace Business.DependencyResolvers
{
    public static class ServiceRegistration
    {
        public static void Run(this IServiceCollection service)
        {
            service.AddScoped<AppDbContext>();

            service.AddScoped<ICategoryDAL, EFCategoryDAL>();
            service.AddScoped<ICategoryService, CategoryService>();
            service.AddScoped<IProductDAL, EFProductDAL>();
            service.AddScoped<IProductService, ProductService>();
            service.AddScoped<IOrderDAL, EFOrderDAL>();
            service.AddScoped<IOrderService, OrderService>();
            service.AddScoped<IOrderDAL, EFOrderDAL>();
            service.AddScoped<IOrderService, OrderService>();
            service.AddScoped<ISpecificationDAL, EFSpecificationDAL>();
            service.AddScoped<ISpecificationService, SpecificationService>();
            service.AddScoped<IUserDAL, EFUserDAL>();
            service.AddScoped<IUserService, UserManager>();
            service.AddScoped<IWishListDAL, EFWishListDAL>();
            service.AddScoped<IWishListService, WishListService>();
            service.AddScoped<IEmailHelper, EmailHelper>();
            //create configuration of AutoMapper is name MApProfile
            var mapperConfig = new MapperConfiguration(mc =>
                       {
                           mc.AddProfile<MapProfile>();
                       });

            IMapper mapper = mapperConfig.CreateMapper();
            service.AddSingleton(mapper);









        }
    }
}
