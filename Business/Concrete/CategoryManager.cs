using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDAL _categoryDAL;
            private readonly IMapper _mapper;

        public CategoryManager(ICategoryDAL categoryDAL, IMapper mapper)
        {
            _categoryDAL = categoryDAL;
            _mapper = mapper;
        }

        public IResult AddCategory(CategoryCreateDTO categoryCreateDTO)
        {
            var map = _mapper.Map<Category>(categoryCreateDTO);
            map.Status = true;
            map.CreatedDate=DateTime.Now;
            _categoryDAL.Add(map);
            return new SuccessResult();
                
        }

        public IResult DeleteCategory(int categoryId)
        {
            try
            {
                var categories=_categoryDAL.Get(x=>x.Id==categoryId);
                _categoryDAL.Delete(categories);
                return new SuccessResult("Category has already deleted");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

       

       

        public IDataResult<List<CategoryHomeNavbarDTO>> GetNavbarCategories()
        {
            var categories=_categoryDAL.GetNavbarCategories();
            var map=_mapper.Map<List<CategoryHomeNavbarDTO>>(categories);
            return new SuccessDataResult<List<CategoryHomeNavbarDTO>>(map);
        }

        public IDataResult<List<CategoryFeaturedDTO>> GetFeaturedCategories()
        {
           var categories=_categoryDAL.GetFeaturedCategories();
            var map=_mapper.Map<List<CategoryFeaturedDTO>>(categories);
            return new SuccessDataResult<List<CategoryFeaturedDTO>>(map);
        }

        public IResult UpdateCategory(CategoryUpdateDTO categoryUpdateDTO)
        {
            var categories = _categoryDAL.Get(x => x.Id == categoryUpdateDTO.Id);
            var map = _mapper.Map<Category>(categoryUpdateDTO);

            categories.PhotoUrl = map.PhotoUrl;
            categories.CategoryName= map.CategoryName;

            _categoryDAL.Update(categories);
            return new SuccessResult("Category Updated");
        }

        public IDataResult<List<CategoryAdminListDTO>> CategoryAdminCategories()
        {
           var categories=_categoryDAL.GetAll();
            var map = _mapper.Map<List<CategoryAdminListDTO>>(categories);
            return new SuccessDataResult<List<CategoryAdminListDTO>>(map);
        }

        public IResult CategoryChangeStatus(int categoryId)
        {
           var categories=_categoryDAL.Get(x=>x.Id==categoryId);
            if (categories.Status)
            {
                categories.Status = false;
            }
            else
            {
                categories.Status = true;
            }
            _categoryDAL.Update(categories);
            return new SuccessResult("Change Category Status");

                
        }
    }
}
