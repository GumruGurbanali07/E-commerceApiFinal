using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.ProductDTOs;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Concrete
{
    public class ProductService : IProductService
    {

        private readonly IProductDAL _productDAL;
        private readonly IMapper _mapper;
        private readonly ISpecificationService _specificationService;
        private readonly IMemoryCache _memoryCache;



        public ProductService(IProductDAL productDAL, IMapper mapper, ISpecificationService specificationService, IMemoryCache memoryCache)
        {
            _productDAL = productDAL;
            _mapper = mapper;
            _specificationService = specificationService;
            _memoryCache = memoryCache;
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

        //public IDataResult<ProductDetailDTO> ProductDetail(int productId)
        //{
        //    //retrieves detailed information about the product with the specified productId from the data access layer.
        //    var product = _productDAL.GetProduct(productId);
        //    //retrieved product information is mapped to a ProductDetailDTO 
        //    var map = _mapper.Map<ProductDetailDTO>(product);
        //    //product's category name is set in the DTO.
        //    map.CategoryName = product.Category.CategoryName;
        //    return new SuccessDataResult<ProductDetailDTO>(map);
        //}
        public IDataResult<ProductDetailDTO> ProductDetail(int productId)
        {
            // Try to get the product details from the cache
            if (_memoryCache.TryGetValue($"ProductDetail_{productId}", out ProductDetailDTO cachedProductDetail))
            {
                return new SuccessDataResult<ProductDetailDTO>(cachedProductDetail);
            }

            // If not found in the cache, retrieve detailed information about the product from the data access layer
            var product = _productDAL.GetProduct(productId);

            if (product != null)
            {
                // Map product information to a ProductDetailDTO 
                var productDetail = _mapper.Map<ProductDetailDTO>(product);

                // Set product's category name in the DTO
                productDetail.CategoryName = product.Category.CategoryName;

                // Cache the product details for a short period (e.g., 5 minutes)
                CacheProductDetails(productId, productDetail);

                return new SuccessDataResult<ProductDetailDTO>(productDetail);
            }
            else
            {
                return new ErrorDataResult<ProductDetailDTO>("Product not found");
            }
        }

        private void CacheProductDetails(int productId, ProductDetailDTO productDetail)
        {
            var cacheKey = $"ProductDetail_{productId}";

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _memoryCache.Set(cacheKey, productDetail, cacheEntryOptions);
        }

        //public IDataResult<List<ProductFeaturedDTO>> ProductFeaturedList()
        //{
        //    //retrieves a list of featured products from the data access layer 
        //    var products = _productDAL.GetFeaturedProducts();
        //    //list of products is then mapped to a list of ProductFeaturedDTO 
        //    var map = _mapper.Map<List<ProductFeaturedDTO>>(products);
        //    return new SuccessDataResult<List<ProductFeaturedDTO>>(map);
        //}
        public IDataResult<List<ProductFeaturedDTO>> ProductFeaturedList()
        {
            // Try to get the products from the cache
            if (_memoryCache.TryGetValue("ProductFeaturedList", out List<ProductFeaturedDTO> cachedProducts))
            {
                return new SuccessDataResult<List<ProductFeaturedDTO>>(cachedProducts);
            }

            // If not found in the cache, retrieve from the data access layer
            var products = _productDAL.GetFeaturedProducts();
            var mappedProducts = _mapper.Map<List<ProductFeaturedDTO>>(products);

            // Store the products in the cache with a specified expiration time (e.g., 10 minutes)
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
            _memoryCache.Set("ProductFeaturedList", mappedProducts, cacheEntryOptions);

            return new SuccessDataResult<List<ProductFeaturedDTO>>(mappedProducts);
        }


        //public IDataResult<List<ProductFilterDTO>> ProductFilterList(int categoryId, int minPrice, int maxPrice)
        //{
        //    //retrieves a list of products from the data access layer based on the specified filters.
        //    var products = _productDAL
        //       .GetAll(x => x.CategoryId == categoryId && x.Price >= minPrice && x.Price <= maxPrice).ToList();
        //    // list of products is then mapped to a list of ProductFilterDTO
        //    var map = _mapper.Map<List<ProductFilterDTO>>(products);
        //    return new SuccessDataResult<List<ProductFilterDTO>>(map);
        //}
        public IDataResult<List<ProductFilterDTO>> ProductFilterList(int categoryId, int minPrice, int maxPrice)
        {
            // Try to get the filtered products from the cache
            if (_memoryCache.TryGetValue($"ProductFilterList_{categoryId}_{minPrice}_{maxPrice}", out List<ProductFilterDTO> cachedProducts))
            {
                return new SuccessDataResult<List<ProductFilterDTO>>(cachedProducts);
            }

            // If not found in the cache, retrieve the list of products from the data access layer based on the specified filters
            var products = _productDAL
                .GetAll(x => x.CategoryId == categoryId && x.Price >= minPrice && x.Price <= maxPrice).ToList();

            // Map the list of products to a list of ProductFilterDTO
            var mappedProducts = _mapper.Map<List<ProductFilterDTO>>(products);

            // Cache the filtered products for a short period (e.g., 5 minutes)
            CacheProductFilterList(categoryId, minPrice, maxPrice, mappedProducts);

            return new SuccessDataResult<List<ProductFilterDTO>>(mappedProducts);
        }

        private void CacheProductFilterList(int categoryId, int minPrice, int maxPrice, List<ProductFilterDTO> products)
        {
            var cacheKey = $"ProductFilterList_{categoryId}_{minPrice}_{maxPrice}";

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _memoryCache.Set(cacheKey, products, cacheEntryOptions);
        }


        //public IDataResult<List<ProductRecentDTO>> ProductRecentList()
        //{
        //    //retrieves a list of recent products from the data access layer
        //    var products = _productDAL.GetRecentProducts();
        //    //list of products is then mapped to a list of ProductRecentDTO 
        //    var map = _mapper.Map<List<ProductRecentDTO>>(products);
        //    return new SuccessDataResult<List<ProductRecentDTO>>(map);
        //}
        public IDataResult<List<ProductRecentDTO>> ProductRecentList()
        {
            // Try to get the recent products from the cache
            if (_memoryCache.TryGetValue("ProductRecentList", out List<ProductRecentDTO> cachedProducts))
            {
                return new SuccessDataResult<List<ProductRecentDTO>>(cachedProducts);
            }

            // If not found in the cache, retrieve the list of recent products from the data access layer
            var products = _productDAL.GetRecentProducts();

            // Map the list of products to a list of ProductRecentDTO
            var mappedProducts = _mapper.Map<List<ProductRecentDTO>>(products);

            // Cache the recent products for a short period (e.g., 5 minutes)
            CacheProductRecentList(mappedProducts);

            return new SuccessDataResult<List<ProductRecentDTO>>(mappedProducts);
        }

        private void CacheProductRecentList(List<ProductRecentDTO> products)
        {
            var cacheKey = "ProductRecentList";

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _memoryCache.Set(cacheKey, products, cacheEntryOptions);
        }

        //public IResult ProductUpdate(ProductUpdateDTO productUpdateDTO)
        //{
        //    //retrieves the existing product from the data access layer based on the Id in the DTO.
        //    var product = _productDAL.Get(x => x.Id == productUpdateDTO.Id);
        //    //data from the DTO is mapped to the existing product using 
        //    var map = _mapper.Map<Product>(productUpdateDTO);
        //    //Individual properties of the existing product
        //    //are updated with the corresponding values from the mapped product.
        //    product.Status = map.Status;
        //    product.ProductName = map.ProductName;
        //    product.Price = map.Price;
        //    product.Description = map.Description;
        //    product.Quantity = map.Quantity;
        //    product.CategoryId = map.CategoryId;
        //    product.IsFeatured = map.IsFeatured;
        //    product.Discount = map.Discount;
        //    product.PhotoUrl = map.PhotoUrl;
        //    //product is then updated in the data access layer
        //    _productDAL.Update(map);
        //    return new SuccessResult("Product Updated");
        //}
        public IResult ProductUpdate(ProductUpdateDTO productUpdateDTO)
        {
            // Retrieves the existing product from the data access layer based on the Id in the DTO.
            var existingProduct = _productDAL.Get(x => x.Id == productUpdateDTO.Id);

            if (existingProduct != null)
            {
                // Map data from the DTO to the existing product
                var updatedProduct = _mapper.Map<Product>(productUpdateDTO);

                // Update individual properties of the existing product with the corresponding values from the mapped product
                existingProduct.Status = updatedProduct.Status;
                existingProduct.ProductName = updatedProduct.ProductName;
                existingProduct.Price = updatedProduct.Price;
                existingProduct.Description = updatedProduct.Description;
                existingProduct.Quantity = updatedProduct.Quantity;
                existingProduct.CategoryId = updatedProduct.CategoryId;
                existingProduct.IsFeatured = updatedProduct.IsFeatured;
                existingProduct.Discount = updatedProduct.Discount;
                existingProduct.PhotoUrl = updatedProduct.PhotoUrl;

                // Update the existing product in the data access layer
                _productDAL.Update(existingProduct);

                // Invalidate the cache for the updated product details
                InvalidateProductDetailsCache(existingProduct.Id);

                return new SuccessResult("Product Updated");
            }
            else
            {
                return new ErrorResult("Product not found");
            }
        }

        private void InvalidateProductDetailsCache(int productId)
        {
            var cacheKey = $"ProductDetail_{productId}";

            // Remove the cached product details for the updated product
            _memoryCache.Remove(cacheKey);
        }


        public IResult RemoveProductCount(List<ProductDecrementQuantityDTO> productDecrementQuantityDTOs)
        {
            //removing product counts to the data access layer 
            _productDAL.RemoveProductCount(productDecrementQuantityDTOs);
            return new SuccessResult();
        }

        //public IDataResult<bool> CheckProductCount(List<int> productIds)
        //{
        //    var product = _productDAL.GetAll(x => productIds.Contains(x.Id));//?
        //    //checks if any product in a given list has zero quantity
        //    if (product.Where(x => x.Quantity == 0).Any())
        //    {
        //        return new ErrorDataResult<bool>(false);
        //    }
        //    return new SuccessDataResult<bool>(true);
        //}
        public IDataResult<bool> CheckProductCount(List<int> productIds)
        {
            // Try to get the result from the cache
            if (_memoryCache.TryGetValue(GetCacheKeyForProductCountCheck(productIds), out bool cachedResult))
            {
                return new SuccessDataResult<bool>(cachedResult);
            }

            // If not found in the cache, retrieve product information from the data access layer
            var products = _productDAL.GetAll(x => productIds.Contains(x.Id));

            // Check if any product in the given list has zero quantity
            bool hasZeroQuantity = products.Any(x => x.Quantity == 0);

            // Cache the result for a short period (e.g., 5 minutes)
            CacheProductCountCheckResult(productIds, hasZeroQuantity);

            if (hasZeroQuantity)
            {
                return new ErrorDataResult<bool>(false);
            }

            return new SuccessDataResult<bool>(true);
        }

        private string GetCacheKeyForProductCountCheck(List<int> productIds)
        {
            return $"ProductCountCheck_{string.Join("_", productIds)}";
        }

        private void CacheProductCountCheckResult(List<int> productIds, bool result)
        {
            var cacheKey = GetCacheKeyForProductCountCheck(productIds);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _memoryCache.Set(cacheKey, result, cacheEntryOptions);
        }

    }
}
