using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {

        private readonly IProductDAL _productDAL;
        private readonly IMapper _mapper;
        private readonly ISpecificationService _specificationService;

        public ProductManager(IProductDAL productDAL, IMapper mapper, ISpecificationService specificationService)
        {
            _productDAL = productDAL;
            _mapper = mapper;
            _specificationService = specificationService;
        }

        public IDataResult<bool> CheckProductCount(List<int> productIds)
        {
            var product = _productDAL.GetAll(x => productIds.Contains(x.Id));
            if (product.Where(x => x.Quantity == 0).Any())
            {
                return new ErrorDataResult<bool>(false);

            }
            return new SuccessDataResult<bool>(true);

        }

        public IResult ProductCreate(ProductCreateDTO productCreateDTO)
        {
            var map=_mapper.Map<Product>(productCreateDTO);
            map.CreatedDate = DateTime.Now;
           
            _productDAL.Add(map);
            _specificationService.CreateSpecification(map.Id, productCreateDTO.SpecificationAddDTOs);
            return new SuccessResult("Product Added");
        }

        public IResult ProductDelete(int productId)
        {
            var product=_productDAL.Get(x=>x.Id==productId);
            _productDAL.Delete(product);
            return new SuccessResult("Product Deleted");
        }

        public IDataResult<ProductDetailDTO> ProductDetail(int productId)
        {
            var product=_productDAL.GetProduct(productId);
            var map=_mapper.Map<ProductDetailDTO>(product);
            map.CategoryName = product.Category.CategoryName;
            return new SuccessDataResult<ProductDetailDTO>(map);
        }

        public IDataResult<List<ProductFeaturedDTO>> ProductFeaturedList()
        {
            var products = _productDAL.GetFeaturedProducts();
            var map = _mapper.Map<List<ProductFeaturedDTO>>(products);
            return new SuccessDataResult<List<ProductFeaturedDTO>>(map);
        }

        public IDataResult<List<ProductFilterDTO>> ProductFilterList(int categoryId, int minPrice, int maxPrice)
        {
            var products = _productDAL
               .GetAll(x => x.CategoryId == categoryId && x.Price >= minPrice && x.Price <= maxPrice).ToList();
            var map = _mapper.Map<List<ProductFilterDTO>>(products);

            return new SuccessDataResult<List<ProductFilterDTO>>(map);
        }

        public IDataResult<List<ProductRecentDTO>> ProductRecentList()
        {
            var products = _productDAL.GetRecentProducts();
            var map = _mapper.Map<List<ProductRecentDTO>>(products);
            return new SuccessDataResult<List<ProductRecentDTO>>(map);
        }

        public IResult ProductUpdate(ProductUpdateDTO productUpdateDTO)
        {
           var product=_productDAL.Get(x=>x.Id==productUpdateDTO.Id);
            var map = _mapper.Map<Product>(productUpdateDTO);
            product.Status = map.Status;
            product.ProductName = map.ProductName;
            product.Price = map.Price;
            product.Description = map.Description;
            product.Quantity = map.Quantity;
            product.CategoryId = map.CategoryId;
            product.IsFeatured = map.IsFeatured;
            product.Discount = map.Discount;
            product.PhotoUrl = map.PhotoUrl;
            _productDAL.Update(map);
            return new SuccessResult("Product Updated");
        }
    }
}
