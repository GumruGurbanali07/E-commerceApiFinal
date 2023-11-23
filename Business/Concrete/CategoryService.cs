using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using ECommerce.Business.Validations.CategoryValidator;
using ECommerce.Business.Validations.ProductValidator;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using Entities.DTOs.ProductDTOs;
using FluentValidation.Results;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace Business.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDAL _categoryDAL;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public CategoryService(ICategoryDAL categoryDAL, IMapper mapper, IMemoryCache memoryCache)
        {
            _categoryDAL = categoryDAL;
            _mapper = mapper;
            _memoryCache = memoryCache;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/myAllLogs-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public IResult AddCategory(CategoryCreateDTO categoryCreateDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryCreateDTO);
                category.Status = true;
                category.CreatedDate = DateTime.Now;

                if (categoryCreateDTO == null)
                    throw new ArgumentNullException(nameof(categoryCreateDTO), "CategoryCreateDTO cannot be null");

                var validationResult = ValidateCategoryCreate(categoryCreateDTO);
                if (!validationResult.IsValid)
                {
                    Log.Error("Validation failed while adding category: {ErrorMessage}", validationResult.Errors.First().ErrorMessage);
                    return new ErrorResult(validationResult.Errors.First().ErrorMessage);
                }

                if (_categoryDAL.Any(x => x.CategoryName == categoryCreateDTO.CategoryName))
                {
                    Log.Error("A category with the same name already exists: {CategoryName}", categoryCreateDTO.CategoryName);
                    return new ErrorResult("A category with the same name already exists.");
                }

                _categoryDAL.Add(category);

                Log.Information("Category added successfully: {CategoryName}", categoryCreateDTO.CategoryName);

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while adding category: {ErrorMessage}", ex.Message);
                return new ErrorResult("An error occurred while adding the category.");
            }
        }

        public IDataResult<List<CategoryAdminListDTO>> CategoryAdminCategories()
        {
            try
            {
                var cacheKey = "AdminCategoriesCacheKey";

                if (_memoryCache.TryGetValue(cacheKey, out List<CategoryAdminListDTO> cachedCategories))
                {
                    Log.Information("Admin categories retrieved from cache.");
                    return new SuccessDataResult<List<CategoryAdminListDTO>>(cachedCategories);
                }

                var categories = _categoryDAL.GetAll();
                var categoryAdminListDTOs = _mapper.Map<List<CategoryAdminListDTO>>(categories);

                _memoryCache.Set(cacheKey, categoryAdminListDTOs, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

                Log.Information("Admin categories retrieved from the database.");

                return new SuccessDataResult<List<CategoryAdminListDTO>>(categoryAdminListDTOs);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while retrieving admin categories: {ErrorMessage}", ex.Message);
                return new ErrorDataResult<List<CategoryAdminListDTO>>("An error occurred while retrieving admin categories.");
            }
        }

        public IResult CategoryChangeStatus(int categoryId)
        {
            try
            {
                var cacheKey = $"CategoryStatusCacheKey_{categoryId}";

                if (_memoryCache.TryGetValue(cacheKey, out bool cachedStatus))
                {
                    cachedStatus = !cachedStatus;
                }
                else
                {
                    var categories = _categoryDAL.Get(x => x.Id == categoryId);
                    cachedStatus = categories.Status;

                    _memoryCache.Set(cacheKey, cachedStatus, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
                }

                var newStatus = !cachedStatus;

                var updatedCategory = _categoryDAL.Get(x => x.Id == categoryId);
                updatedCategory.Status = newStatus;
                _categoryDAL.Update(updatedCategory);

                Log.Information("Category status changed successfully: {CategoryId}", categoryId);

                return new SuccessResult("Change Category Status");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while changing category status: {ErrorMessage}", ex.Message);
                return new ErrorResult("An error occurred while changing the category status.");
            }
        }

        public IResult DeleteCategory(int categoryId)
        {
            try
            {
                var category = _categoryDAL.Get(x => x.Id == categoryId);
                _categoryDAL.Delete(category);

                Log.Information("Category deleted successfully: {CategoryId}", categoryId);

                return new SuccessResult("Category has already deleted");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while deleting category: {ErrorMessage}", ex.Message);
                return new ErrorResult("An error occurred while deleting the category.");
            }
        }

        public IDataResult<List<CategoryFeaturedDTO>> GetFeaturedCategories()
        {
            try
            {
                var cacheKey = "FeaturedCategoriesCacheKey";

                if (_memoryCache.TryGetValue(cacheKey, out List<CategoryFeaturedDTO> cachedCategories))
                {
                    Log.Information("Featured categories retrieved from cache.");
                    return new SuccessDataResult<List<CategoryFeaturedDTO>>(cachedCategories);
                }

                var categories = _categoryDAL.GetFeaturedCategories();
                var categoryFeaturedDTOs = _mapper.Map<List<CategoryFeaturedDTO>>(categories);

                _memoryCache.Set(cacheKey, categoryFeaturedDTOs, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

                Log.Information("Featured categories retrieved from the database.");

                return new SuccessDataResult<List<CategoryFeaturedDTO>>(categoryFeaturedDTOs);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while retrieving featured categories: {ErrorMessage}", ex.Message);
                return new ErrorDataResult<List<CategoryFeaturedDTO>>("An error occurred while retrieving featured categories.");
            }
        }

        public IDataResult<List<CategoryHomeNavbarDTO>> GetNavbarCategories()
        {
            try
            {
                var cacheKey = "NavbarCategoriesCacheKey";

                if (_memoryCache.TryGetValue(cacheKey, out List<CategoryHomeNavbarDTO> cachedCategories))
                {
                    Log.Information("Navbar categories retrieved from cache.");
                    return new SuccessDataResult<List<CategoryHomeNavbarDTO>>(cachedCategories);
                }

                var categories = _categoryDAL.GetNavbarCategories();
                var categoryHomeNavbarDTOs = _mapper.Map<List<CategoryHomeNavbarDTO>>(categories);

                _memoryCache.Set(cacheKey, categoryHomeNavbarDTOs, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

                Log.Information("Navbar categories retrieved from the database.");

                return new SuccessDataResult<List<CategoryHomeNavbarDTO>>(categoryHomeNavbarDTOs);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while retrieving navbar categories: {ErrorMessage}", ex.Message);
                return new ErrorDataResult<List<CategoryHomeNavbarDTO>>("An error occurred while retrieving navbar categories.");
            }
        }

        public IResult UpdateCategory(CategoryUpdateDTO categoryUpdateDTO)
        {
            try
            {
                var categories = _categoryDAL.Get(x => x.Id == categoryUpdateDTO.Id);

                if (categoryUpdateDTO == null)
                    throw new ArgumentNullException(nameof(categoryUpdateDTO), "CategoryUpdateDTO cannot be null");

                var validationResult = ValidateCategoryUpdate(categoryUpdateDTO);
                if (!validationResult.IsValid)
                {
                    Log.Error("Validation failed while updating category: {ErrorMessage}", validationResult.Errors.First().ErrorMessage);
                    return new ErrorResult(validationResult.Errors.First().ErrorMessage);
                }

                if (_categoryDAL.Any(x => x.CategoryName == categoryUpdateDTO.CategoryName))
                {
                    Log.Error("A category with the same name already exists: {CategoryName}", categoryUpdateDTO.CategoryName);
                    return new ErrorResult("A category with the same name already exists.");
                }

                var map = _mapper.Map<Category>(categoryUpdateDTO);
                categories.PhotoUrl = map.PhotoUrl;
                categories.CategoryName = map.CategoryName;

                _categoryDAL.Update(categories);

                Log.Information("Category updated successfully: {CategoryId}", categoryUpdateDTO.Id);

                return new SuccessResult("Category Updated");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while updating category: {ErrorMessage}", ex.Message);
                return new ErrorResult("An error occurred while updating the category.");
            }
        }

        public ValidationResult ValidateCategoryCreate(CategoryCreateDTO categoryCreateDTO)
        {
            var validator = new CategoryCreateDtoValidator();
            return validator.Validate(categoryCreateDTO);
        }

        public ValidationResult ValidateCategoryUpdate(CategoryUpdateDTO categoryUpdateDTO)
        {
            var validator = new CategoryUpdateDtoValidator();
            return validator.Validate(categoryUpdateDTO);
        }
    }
}
