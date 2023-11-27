using Business.Abstract;
using ECommerce.Entities.DTOs.ProductDTOs;
using Entities.Concrete;
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
        //[HttpPost("createproduct")]
        //public IActionResult CreateProduct([FromBody] ProductCreateDTO productCreateDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var result = _productService.ProductCreate(productCreateDTO);
        //    if (result.Success)
        //        return Ok(result);
        //    return BadRequest(result);
        //}
        [HttpPost("createproduct")]
        public IActionResult CreateProduct([FromBody] ProductCreateDTO productCreateDTO)
        {
            var validationResult = _productService.ValidateProduct(productCreateDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(new { errors = validationResult.Errors });
            }

            var result = _productService.ProductCreate(productCreateDTO);

            if (result.Success)
            {
                return Ok(new { message = "Product Added", success = true });
            }

            return BadRequest(new { message = "Failed to add product", success = false });
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
        [HttpDelete("productdelete/{productId}")]
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
        [HttpGet("searchproducts")]
        public IActionResult SearchProducts([FromQuery] string query)
        {
            try
            {
                var result = _productService.SearchProducts(query);

                if (result.Data.Any()) // Bu satırda Any metodunu kullanıyoruz.
                {
                    // Eğer ürün varsa, başarılı bir şekilde döndür.
                    return Ok(new { data = result.Data, success = true });
                }
                else
                {
                    // Eğer hiç ürün bulunamazsa, başarılı bir şekilde boş bir liste döndür.
                    return Ok(new { data = new List<ProductSearchDTO>(), success = true });
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını döndür.
                return BadRequest(new { message = "Failed to retrieve products", success = false, error = ex.Message });
            }
        }


    }
}
