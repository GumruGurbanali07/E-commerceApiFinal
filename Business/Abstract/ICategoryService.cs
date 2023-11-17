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
        //Adds a new category to the system.
        IResult AddCategory(CategoryCreateDTO categoryCreateDTO);
        //Deletes a category from the system.
        IResult DeleteCategory(int categoryId);
        //Updates an existing category in the system.
        IResult UpdateCategory(CategoryUpdateDTO  categoryUpdateDTO);
        //Changes the status of a category (e.g., from active to inactive or vice versa).
        IResult CategoryChangeStatus(int categoryId);
        //Gets a list of all categories in the system, for use by administrators.
        IDataResult<List<CategoryAdminListDTO>> CategoryAdminCategories();
       // Gets a list of categories to be displayed in the navbar of the website.
        IDataResult<List<CategoryHomeNavbarDTO>>GetNavbarCategories();
        //Gets a list of featured categories to be displayed on the homepage of the website.
        IDataResult<List<CategoryFeaturedDTO>>GetFeaturedCategories();
    }
}
