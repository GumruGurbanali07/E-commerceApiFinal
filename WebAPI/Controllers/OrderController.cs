using Business.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using Entities.DTOs.OrderDTOs;
using Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        [HttpPost("orderproduct")]
        [ProducesResponseType(typeof(List<OrderCreateDTO>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(List<OrderCreateDTO>), StatusCodes.Status401Unauthorized)]
        public IActionResult OrderProduct([FromBody] List<OrderCreateDTO> orderCreateDTOs)
        {
            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(_bearer_token);
            var userId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "nameid").Value;
            var user = Convert.ToInt32(userId);
            var result = _orderService.CreateOrder(user, orderCreateDTOs);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }




        [HttpPatch("changestatus/{orderNumber}")]
        public IActionResult ChangeOrderStatus(string orderNumber, [FromBody] OrderEnum orderEnum)
        {
            var result = _orderService.ChangeOrderStatus(orderNumber, orderEnum);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }


        [HttpGet("userOrder/{userId}")]
        [ProducesResponseType(typeof(SuccessDataResult<OrderCreateDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDataResult<OrderCreateDTO>), StatusCodes.Status400BadRequest)]
        public IActionResult GetUserOrder(int userId)
        {
            var result = _userService.GetUserOrders(userId);
            if (result.Success) return Ok(result);

            return BadRequest(result);
        }
    }
}
