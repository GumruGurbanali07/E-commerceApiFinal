using Core.Utilities.Results.Abstract;
using Entities.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        IResult AddCategory(CategoryCreateDTO categoryCreateDTO);
        IResult DeleteCategory(int categoryId);
        IResult UpdateCategory(CategoryUpdateDTO  categoryUpdateDTO);
        IResult CategoryChangeStatus(int categoryId);   
        IDataResult<List<CategoryAdminListDTO>> CategoryAdminCategories();
        IDataResult<List<CategoryHomeNavbarDTO>>GetNavbarCategories();
        IDataResult<List<CategoryFeaturedDTO>>GetFeaturedCategories();
    }
}
