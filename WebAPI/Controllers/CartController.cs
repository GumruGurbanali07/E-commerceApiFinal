using DataAccess.Concrete.EntityFramework;
using ECommerce.Core.Utilities.OrderHelper;
using ECommerce.Entities.DTOs.CartDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public IActionResult GetCart(string productId)
        //{
        //    CartDTO cartDTO=new CartDTO();
        //    cartDTO.CartItems=new List<CartItemDTO>();
        //    cartDTO.SubTotal=0;
        //    cartDTO.ShippingFee = OrderHelper.ShippingFee;
        //    cartDTO.TotalPrice = 0;
        //    var productDictionary=OrderHelper.GetProductDictionary(productId);
        //    foreach (var item in productDictionary)
        //    {
        //        int productid = item.Key;
        //        var product = _context.Products.Find(productid);
        //        if (product == null)
        //        {
        //            continue;
        //        }
        //        var cartItemDTO = new CartItemDTO();
        //        cartItemDTO.Product= product;
        //        cartItemDTO.Quantity = item.Value;

        //        cartDTO.CartItems.Add(cartItemDTO);
        //        cartDTO.SubTotal += product.Price * item.Value;
        //        cartDTO.TotalPrice = cartDTO.SubTotal + cartDTO.ShippingFee;
        //    }
        //    return Ok(productDictionary);
        //}
        [HttpGet]
        public IActionResult GetCart(string productId)
        {
            CartDTO cartDTO = new CartDTO();
            cartDTO.CartItems = new List<CartItemDTO>();
            cartDTO.SubTotal = 0;
            cartDTO.ShippingFee = OrderHelper.ShippingFee;
            cartDTO.TotalPrice = 0;

            var productDictionary = OrderHelper.GetProductDictionary(productId);

            foreach (var item in productDictionary)
            {
                try
                {
                    int productid = Convert.ToInt32(item.Key);

                    var product = _context.Products.FirstOrDefault(p => p.Id == productid);

                    if (product == null)
                    {
                        continue;
                    }

                    var cartItemDTO = new CartItemDTO
                    {
                        Product = product,
                        Quantity = item.Value
                    };

                    cartDTO.CartItems.Add(cartItemDTO);
                    cartDTO.SubTotal += product.Price * item.Value;
                }
                catch (FormatException)
                {
                    // Handle invalid product ID here
                    return BadRequest($"Invalid product ID: {item.Key}");
                }
            }

            cartDTO.TotalPrice = cartDTO.SubTotal + cartDTO.ShippingFee;
            return Ok(cartDTO);
        }




    }
}
