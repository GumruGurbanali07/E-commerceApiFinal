using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.ProductDTOs;

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




       


        public IResult ProductCreate(ProductCreateDTO productCreateDTO)
        {
            //to map the data from productCreateDTO to a new Product entity (map).
            var map = _mapper.Map<Product>(productCreateDTO);
            //product is set to the current date and time.
            map.CreatedDate = DateTime.Now;
            //mapped product is then added to the data access layer 
            _productDAL.Add(map);
            //product's specifications are created using the _specificationService
            _specificationService.CreateSpecification(map.Id, productCreateDTO.SpecificationAddDTOs);
            return new SuccessResult("Product Added");
        }






        public IResult ProductDelete(int productId)
        {
            //retrieves the product from the data access layer with the specified productId.
            var product = _productDAL.Get(x => x.Id == productId);
            //retrieved product is then deleted from the data access layer 
            _productDAL.Delete(product);
            return new SuccessResult("Product Deleted");
        }

        public IDataResult<ProductDetailDTO> ProductDetail(int productId)
        {
            //retrieves detailed information about the product with the specified productId from the data access layer.
            var product = _productDAL.GetProduct(productId);
            //retrieved product information is mapped to a ProductDetailDTO 
            var map = _mapper.Map<ProductDetailDTO>(product);
            //product's category name is set in the DTO.
            map.CategoryName = product.Category.CategoryName;
            return new SuccessDataResult<ProductDetailDTO>(map);
        }

        public IDataResult<List<ProductFeaturedDTO>> ProductFeaturedList()
        {
            //retrieves a list of featured products from the data access layer 
            var products = _productDAL.GetFeaturedProducts();
            //list of products is then mapped to a list of ProductFeaturedDTO 
            var map = _mapper.Map<List<ProductFeaturedDTO>>(products);
            return new SuccessDataResult<List<ProductFeaturedDTO>>(map);
        }

        public IDataResult<List<ProductFilterDTO>> ProductFilterList(int categoryId, int minPrice, int maxPrice)
        {
            //retrieves a list of products from the data access layer based on the specified filters.
            var products = _productDAL
               .GetAll(x => x.CategoryId == categoryId && x.Price >= minPrice && x.Price <= maxPrice).ToList();
            // list of products is then mapped to a list of ProductFilterDTO
            var map = _mapper.Map<List<ProductFilterDTO>>(products);
            return new SuccessDataResult<List<ProductFilterDTO>>(map);
        }


        public IDataResult<List<ProductRecentDTO>> ProductRecentList()
        {
            //retrieves a list of recent products from the data access layer
            var products = _productDAL.GetRecentProducts();
            //list of products is then mapped to a list of ProductRecentDTO 
            var map = _mapper.Map<List<ProductRecentDTO>>(products);
            return new SuccessDataResult<List<ProductRecentDTO>>(map);
        }

        public IResult ProductUpdate(ProductUpdateDTO productUpdateDTO)
        {
            //retrieves the existing product from the data access layer based on the Id in the DTO.
            var product = _productDAL.Get(x => x.Id == productUpdateDTO.Id);
            //data from the DTO is mapped to the existing product using 
            var map = _mapper.Map<Product>(productUpdateDTO);
            //Individual properties of the existing product are updated with the corresponding values from the mapped product.
            product.Status = map.Status;
            product.ProductName = map.ProductName;
            product.Price = map.Price;
            product.Description = map.Description;
            product.Quantity = map.Quantity;
            product.CategoryId = map.CategoryId;
            product.IsFeatured = map.IsFeatured;
            product.Discount = map.Discount;
            product.PhotoUrl = map.PhotoUrl;
            //product is then updated in the data access layer
            _productDAL.Update(map);
            return new SuccessResult("Product Updated");
        }

        public IResult RemoveProductCount(List<ProductDecrementQuantityDTO> productDecrementQuantityDTOs)
        {
            //removing product counts to the data access layer 
            _productDAL.RemoveProductCount(productDecrementQuantityDTOs);
            return new SuccessResult();
        }

        public IDataResult<bool> CheckProductCount(List<int> productIds)
        {
            var product = _productDAL.GetAll(x => productIds.Contains(x.Id));
            //checks if any product in a given list has zero quantity
            if (product.Where(x => x.Quantity == 0).Any())
            {
                return new ErrorDataResult<bool>(false);
            }
            return new SuccessDataResult<bool>(true);
        }
    }
}
