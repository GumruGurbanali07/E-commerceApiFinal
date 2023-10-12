using Business.Abstract;
using Entities.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost("createproduct")]
        public IActionResult CreateProduct([FromBody] ProductCreateDTO productCreateDTO)
        {
            var result=_productService.ProductCreate(productCreateDTO);
            if(result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("updatedproduct")]
        public IActionResult UpdateProduct([FromBody] ProductUpdateDTO productUpdateDTO)
        {
            var result = _productService.ProductUpdate(productUpdateDTO);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
