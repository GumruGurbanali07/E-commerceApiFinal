using Business.Abstract;
using Entities.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        
        [HttpPost("addcategory")]
        public IActionResult AddCategory([FromBody] CategoryCreateDTO categoryCreateDTO)
        {
            var result = _categoryService.AddCategory(categoryCreateDTO);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
