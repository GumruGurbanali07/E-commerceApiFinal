using Business.Abstract;
using Entities.DTOs.ProductDTOs;
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
            var result = _productService.ProductCreate(productCreateDTO);
            if (result.Success)
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


        [HttpGet("productdetail/{productId}")]
        public IActionResult ProductDetail(int productId)
        {
            var result = _productService.ProductDetail(productId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("featuredproducts")]
        public IActionResult ProductFeatured()
        {
            var product = _productService.ProductFeaturedList();
            if (product.Success)
                return Ok(product);
            return BadRequest(product);
        }
        [HttpGet("recentproducts")]
        public IActionResult ProductRecent()
        {
            var product = _productService.ProductRecentList();
            if (product.Success)
                return Ok(product);
            return BadRequest(product);
        }
        [HttpPost("productdelete/{productId}")]
        public IActionResult ProductDelete(int productId)
        {
            var result = _productService.ProductDelete(productId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("filterproducts")]
        public IActionResult ProductFilter([FromQuery] int categoryId, [FromQuery] int minPrice, [FromQuery] int maxPrice)
        {
            var product = _productService.ProductFilterList(categoryId, minPrice, maxPrice);
            if (product.Success)
                return Ok(product);
            return BadRequest(product);
        }
    }
}
