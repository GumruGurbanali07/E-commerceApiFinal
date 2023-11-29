using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using ECommerce.Business.Validations.ProductValidator;
using ECommerce.Entities.DTOs.ProductDTOs;
using Entities.Concrete;
using Entities.DTOs.ProductDTOs;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

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

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/myProductLogs-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("ProductService instance created.");
        }







        public IResult ProductCreate(ProductCreateDTO productCreateDTO)
        {
            try
            {
                Log.Information($"Creating product: {productCreateDTO.ProductName}");

                var map = _mapper.Map<Product>(productCreateDTO);
                map.CreatedDate = DateTime.Now;
                _productDAL.Add(map);
                _specificationService.CreateSpecification(map.Id, productCreateDTO.SpecificationAddDTOs);

                Log.Information("Product created successfully.");
                return new SuccessResult("Product Added");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating a product.");
                throw;
            }
        }

        public IResult ProductDelete(int productId)
        {
            try
            {
                Log.Information($"Deleting product with ID: {productId}");

                var product = _productDAL.Get(x => x.Id == productId);
                _productDAL.Delete(product);

                Log.Information("Product deleted successfully.");
                return new SuccessResult("Product Deleted");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while deleting product: {ErrorMessage}", ex.Message);
                return new ErrorResult("An error occurred while deleting the product.");
            }
        }

        public IDataResult<ProductDetailDTO> ProductDetail(int productId)
        {
            try
            {
                Log.Information($"Getting product details for ID: {productId}");

                if (_memoryCache.TryGetValue($"ProductDetail_{productId}", out ProductDetailDTO cachedProductDetail))
                {
                    return new SuccessDataResult<ProductDetailDTO>(cachedProductDetail);
                }

                var product = _productDAL.GetProduct(productId);

                if (product != null)
                {
                    var productDetail = _mapper.Map<ProductDetailDTO>(product);
                    productDetail.CategoryName = product.Category.CategoryName;
                    CacheProductDetails(productId, productDetail);

                    Log.Information("Retrieved product details successfully.");
                    return new SuccessDataResult<ProductDetailDTO>(productDetail);
                }
                else
                {
                    Log.Warning("Product not found.");
                    return new ErrorDataResult<ProductDetailDTO>("Product not found");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while getting product details.");
                throw;
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

        public IDataResult<List<ProductFeaturedDTO>> ProductFeaturedList()
        {
            try
            {
                Log.Information("Getting featured product list");

                if (_memoryCache.TryGetValue("ProductFeaturedList", out List<ProductFeaturedDTO> cachedProducts))
                {
                    return new SuccessDataResult<List<ProductFeaturedDTO>>(cachedProducts);
                }

                var products = _productDAL.GetFeaturedProducts();
                var mappedProducts = _mapper.Map<List<ProductFeaturedDTO>>(products);

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
                _memoryCache.Set("ProductFeaturedList", mappedProducts, cacheEntryOptions);

                Log.Information("Retrieved featured product list successfully.");
                return new SuccessDataResult<List<ProductFeaturedDTO>>(mappedProducts);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while getting featured product list.");
                throw;
            }
        }

        public IDataResult<List<ProductFilterDTO>> ProductFilterList(int categoryId, int minPrice, int maxPrice)
        {
            try
            {
                Log.Information($"Filtering products by Category ID: {categoryId}, Min Price: {minPrice}, Max Price: {maxPrice}");

                if (_memoryCache.TryGetValue($"ProductFilterList_{categoryId}_{minPrice}_{maxPrice}", out List<ProductFilterDTO> cachedProducts))
                {
                    return new SuccessDataResult<List<ProductFilterDTO>>(cachedProducts);
                }

                var products = _productDAL
                    .GetAll(x => x.CategoryId == categoryId && x.Price >= minPrice && x.Price <= maxPrice).ToList();

                var mappedProducts = _mapper.Map<List<ProductFilterDTO>>(products);

                CacheProductFilterList(categoryId, minPrice, maxPrice, mappedProducts);

                Log.Information("Filtered products successfully.");
                return new SuccessDataResult<List<ProductFilterDTO>>(mappedProducts);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while filtering products.");
                throw;
            }
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

        public IDataResult<List<ProductRecentDTO>> ProductRecentList()
        {
            try
            {
                Log.Information("Getting recent product list");

                if (_memoryCache.TryGetValue("ProductRecentList", out List<ProductRecentDTO> cachedProducts))
                {
                    return new SuccessDataResult<List<ProductRecentDTO>>(cachedProducts);
                }

                var products = _productDAL.GetRecentProducts();
                var mappedProducts = _mapper.Map<List<ProductRecentDTO>>(products);

                CacheProductRecentList(mappedProducts);

                Log.Information("Retrieved recent product list successfully.");
                return new SuccessDataResult<List<ProductRecentDTO>>(mappedProducts);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while getting recent product list.");
                throw;
            }
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

        public IResult ProductUpdate(ProductUpdateDTO productUpdateDTO)
        {
            try
            {
                Log.Information($"Updating product with ID: {productUpdateDTO.Id}");

                var validator = new ProductUpdateDTOValidator();
                var validationResult = validator.Validate(productUpdateDTO);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var existingProduct = _productDAL.Get(x => x.Id == productUpdateDTO.Id);

                if (existingProduct != null)
                {
                    var updatedProduct = _mapper.Map<Product>(productUpdateDTO);

                    existingProduct.Status = updatedProduct.Status;
                    existingProduct.ProductName = updatedProduct.ProductName;
                    existingProduct.Price = updatedProduct.Price;
                    existingProduct.Description = updatedProduct.Description;
                    existingProduct.Quantity = updatedProduct.Quantity;
                    existingProduct.CategoryId = updatedProduct.CategoryId;
                    existingProduct.IsFeatured = updatedProduct.IsFeatured;
                    existingProduct.Discount = updatedProduct.Discount;
                    existingProduct.PhotoUrl = updatedProduct.PhotoUrl;

                    _productDAL.Update(existingProduct);

                    InvalidateProductDetailsCache(existingProduct.Id);

                    Log.Information("Product updated successfully.");
                    return new SuccessResult("Product Updated");
                }
                else
                {
                    Log.Warning("Product not found.");
                    return new ErrorResult("Product not found");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating a product.");
                throw;
            }
        }

        private void InvalidateProductDetailsCache(int productId)
        {
            var cacheKey = $"ProductDetail_{productId}";

            _memoryCache.Remove(cacheKey);
        }

        public IResult RemoveProductCount(List<ProductDecrementQuantityDTO> productDecrementQuantityDTOs)
        {
            try
            {
                Log.Information("Removing product counts");

                _productDAL.RemoveProductCount(productDecrementQuantityDTOs);

                Log.Information("Product counts removed successfully.");
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while removing product counts.");
                throw;
            }
        }

        public IDataResult<bool> CheckProductCount(List<int> productIds)
        {
            try
            {
                Log.Information($"Checking product counts for IDs: {string.Join(", ", productIds)}");

                if (_memoryCache.TryGetValue(GetCacheKeyForProductCountCheck(productIds), out bool cachedResult))
                {
                    return new SuccessDataResult<bool>(cachedResult);
                }

                var products = _productDAL.GetAll(x => productIds.Contains(x.Id));

                bool hasZeroQuantity = products.Any(x => x.Quantity == 0);

                CacheProductCountCheckResult(productIds, hasZeroQuantity);

                if (hasZeroQuantity)
                {
                    Log.Information("Some products have zero quantity.");
                    return new ErrorDataResult<bool>(false);
                }

                Log.Information("All products have sufficient quantity.");
                return new SuccessDataResult<bool>(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while checking product counts.");
                throw;
            }
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

        public ValidationResult ValidateProduct(ProductCreateDTO productCreateDTO)
        {
            try
            {
                Log.Information("Validating product creation");

                var validator = new ProductCreateDTOValidator();
                return validator.Validate(productCreateDTO);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while validating product creation.");
                throw;
            }
        }

        public IDataResult<List<ProductSearchDTO>> SearchProducts(string query)
        {
            try
            {
                Log.Information($"Searching products for query: {query}");

                var products = _productDAL
                    .GetAll(x => EF.Functions.Like(x.ProductName, $"%{query}%"))
                    .ToList();


                var mappedProducts = _mapper.Map<List<ProductSearchDTO>>(products);

                Log.Information("Product search completed successfully.");
                return new SuccessDataResult<List<ProductSearchDTO>>(mappedProducts);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while searching products.");
                throw;
            }
        }
    }
}
