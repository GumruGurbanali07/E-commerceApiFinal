using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using Microsoft.Extensions.Caching.Memory;

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
        }

        public IResult AddCategory(CategoryCreateDTO categoryCreateDTO)
        {
            // Map the CategoryCreateDTO object to a Category object.
            var category = _mapper.Map<Category>(categoryCreateDTO);

            // Set the Status property of the Category object to true.
            category.Status = true;

            // Set the CreatedDate property of the Category object to the current date and time.
            category.CreatedDate = DateTime.Now;

            if (categoryCreateDTO == null)
                throw new ArgumentNullException(nameof(categoryCreateDTO), "CategoryCreateDTO cannot be null");

            // Eyni adda Category varmi deye yoxlayir
            if (_categoryDAL.Any(x => x.CategoryName == categoryCreateDTO.CategoryName))
                return new ErrorResult("A category with the same name already exists.");

            // Add the Category object to the database.
            _categoryDAL.Add(category);

            // Return a SuccessResult object to indicate that the operation was successful.
            return new SuccessResult();
        }

        public IResult DeleteCategory(int categoryId)
        {
            // Get the Category object from the database with the specified ID.
            var category = _categoryDAL.Get(x => x.Id == categoryId);

            // Delete the Category object from the database.
            _categoryDAL.Delete(category);

            // Return a SuccessResult object to indicate that the operation was successful.
            return new SuccessResult("Category has already deleted");
        }

        public IDataResult<List<CategoryHomeNavbarDTO>> GetNavbarCategories()
        {
            //// Get the list of categories from the database that are marked as active.
            //var categories = _categoryDAL.GetNavbarCategories();

            //// Map the list of Category objects to a list of CategoryHomeNavbarDTO objects.
            //var categoryHomeNavbarDTOs = _mapper.Map<List<CategoryHomeNavbarDTO>>(categories);

            //// Return a SuccessDataResult object with the list of CategoryHomeNavbarDTO objects.
            //return new SuccessDataResult<List<CategoryHomeNavbarDTO>>(categoryHomeNavbarDTOs);

            var cacheKey = "NavbarCategoriesCacheKey";

            if (_memoryCache.TryGetValue(cacheKey, out List<CategoryHomeNavbarDTO> cachedCategories))
            {
                // Categories are cached, return cached data
                return new SuccessDataResult<List<CategoryHomeNavbarDTO>>(cachedCategories);
            }

            // Categories are not cached, retrieve from the database
            var categories = _categoryDAL.GetNavbarCategories();
            var categoryHomeNavbarDTOs = _mapper.Map<List<CategoryHomeNavbarDTO>>(categories);

            // Cache the data for future use
            _memoryCache.Set(cacheKey, categoryHomeNavbarDTOs, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Adjust the cache expiration time as needed
            });

            // Return a SuccessDataResult object with the list of CategoryHomeNavbarDTO objects.
            return new SuccessDataResult<List<CategoryHomeNavbarDTO>>(categoryHomeNavbarDTOs);
        }

        public IDataResult<List<CategoryFeaturedDTO>> GetFeaturedCategories()
        {
            var cacheKey = "FeaturedCategoriesCacheKey";

            if (_memoryCache.TryGetValue(cacheKey, out List<CategoryFeaturedDTO> cachedCategories))
            {
                // Featured categories are cached, return cached data
                return new SuccessDataResult<List<CategoryFeaturedDTO>>(cachedCategories);
            }

            // Featured categories are not cached, retrieve from the database
            var categories = _categoryDAL.GetFeaturedCategories();
            var categoryFeaturedDTOs = _mapper.Map<List<CategoryFeaturedDTO>>(categories);

            // Cache the data for future use
            _memoryCache.Set(cacheKey, categoryFeaturedDTOs, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Adjust the cache expiration time as needed
            });

            // Return a SuccessDataResult object with the list of CategoryFeaturedDTO objects.
            return new SuccessDataResult<List<CategoryFeaturedDTO>>(categoryFeaturedDTOs);
        }








        //public IDataResult<List<CategoryFeaturedDTO>> GetFeaturedCategories()
        //    {
        //        // Get the list of featured categories from the database.
        //        var categories = _categoryDAL.GetFeaturedCategories();

        //        // Map the list of Category objects to a list of CategoryFeaturedDTO objects.
        //        var categoryFeaturedDTOs = _mapper.Map<List<CategoryFeaturedDTO>>(categories);

        //        // Return a SuccessDataResult object with the list of CategoryFeaturedDTO objects.
        //        return new SuccessDataResult<List<CategoryFeaturedDTO>>(categoryFeaturedDTOs);
        //    }

        public IResult UpdateCategory(CategoryUpdateDTO categoryUpdateDTO)
        {
            // Get the category from the database.
            var categories = _categoryDAL.Get(x => x.Id == categoryUpdateDTO.Id);

            if (categoryUpdateDTO == null)
                throw new ArgumentNullException(nameof(categoryUpdateDTO), "CategoryCreateDTO cannot be null");

            // Eyni adda Category varmi deye yoxlayir
            if (_categoryDAL.Any(x => x.CategoryName == categoryUpdateDTO.CategoryName))
                return new ErrorResult("A category with the same name already exists.");

            // Map the CategoryUpdateDTO object to the Category object.
            var map = _mapper.Map<Category>(categoryUpdateDTO);
            //corresponding properties of the map
            categories.PhotoUrl = map.PhotoUrl;
            categories.CategoryName = map.CategoryName;

           

            // Update the category in the database.
            _categoryDAL.Update(categories);
            // Return a SuccessResult object to indicate that the operation was successful.
            return new SuccessResult("Category Updated");
        }



        //public IDataResult<List<CategoryAdminListDTO>> CategoryAdminCategories()
        //{
        //    // Get all of the categories from the database.
        //    var categories = _categoryDAL.GetAll();
        //    // Map the list of Category objects to a list of CategoryAdminListDTO objects.
        //    var map = _mapper.Map<List<CategoryAdminListDTO>>(categories);
        //    // Return a SuccessDataResult object with the list of CategoryAdminListDTO objects
        //    return new SuccessDataResult<List<CategoryAdminListDTO>>(map);
        //}
        public IDataResult<List<CategoryAdminListDTO>> CategoryAdminCategories()
        {
            var cacheKey = "AdminCategoriesCacheKey";

            if (_memoryCache.TryGetValue(cacheKey, out List<CategoryAdminListDTO> cachedCategories))
            {
                // Admin categories are cached, return cached data
                return new SuccessDataResult<List<CategoryAdminListDTO>>(cachedCategories);
            }

            // Admin categories are not cached, retrieve from the database
            var categories = _categoryDAL.GetAll();
            var categoryAdminListDTOs = _mapper.Map<List<CategoryAdminListDTO>>(categories);

            // Cache the data for future use
            _memoryCache.Set(cacheKey, categoryAdminListDTOs, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Adjust the cache expiration time as needed
            });

            // Return a SuccessDataResult object with the list of CategoryAdminListDTO objects.
            return new SuccessDataResult<List<CategoryAdminListDTO>>(categoryAdminListDTOs);
        }

        //public IResult CategoryChangeStatus(int categoryId)
        //{
        //    // Get the category from the database.
        //    var categories = _categoryDAL.Get(x => x.Id == categoryId);
        //    // Toggle the Status property of the category.
        //    if (categories.Status)
        //    {
        //        categories.Status = false;
        //    }
        //    else
        //    {
        //        categories.Status = true;
        //    }
        //    // Update the category in the database.
        //    _categoryDAL.Update(categories);
        //    // Return a SuccessResult object to indicate that the operation was successful.
        //    return new SuccessResult("Change Category Status");


        //}
        public IResult CategoryChangeStatus(int categoryId)
        {
            var cacheKey = $"CategoryStatusCacheKey_{categoryId}";

            // Check if the category status is in the cache
            if (_memoryCache.TryGetValue(cacheKey, out bool cachedStatus))
            {
                // Category status is cached, toggle the cached status
                cachedStatus = !cachedStatus;
            }
            else
            {
                // Category status is not cached, fetch it from the database
                var categories = _categoryDAL.Get(x => x.Id == categoryId);
                cachedStatus = categories.Status;

                // Cache the category status for future use
                _memoryCache.Set(cacheKey, cachedStatus, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Adjust the cache expiration time as needed
                });
            }

            // Toggle the Status property of the category.
            var newStatus = !cachedStatus;

            // Update the category status in the database.
            var updatedCategory = _categoryDAL.Get(x => x.Id == categoryId);
            updatedCategory.Status = newStatus;
            _categoryDAL.Update(updatedCategory);

            // Return a SuccessResult object to indicate that the operation was successful.
            return new SuccessResult("Change Category Status");
        }

    }
}
