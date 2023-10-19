using AutoMapper;
using Business.Abstract;
using Core.Utilities.Business;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.OrderDTOs;
using Entities.DTOs.ProductDTOs;
using Entities.DTOs.UserDTOs;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDAL _orderDAL;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public OrderManager(IOrderDAL orderDAL, IMapper mapper, IProductService productService, IUserService userService)
        {
            _orderDAL = orderDAL;
            _mapper = mapper;
            _productService = productService;
            _userService = userService;
        }

        public IResult ChangeOrderStatus(string orderNumber, OrderEnum orderEnum)
        {
            var order = _orderDAL.Get(x => x.OrderNumber == orderNumber);
            order.OrderEnum = orderEnum;
            _orderDAL.Update(order);
            return new SuccessResult();

        }

        public IResult CreateOrder(int userId, List<OrderCreateDTO> orderCreateDTOs)
        {
            var productIds = orderCreateDTOs.Select(x => x.ProductId).ToList();
            var quantities = orderCreateDTOs.Select(x => x.Quantity).ToList();
            //var result = BusinessRules.Check(IsProductInStock(productIds));
            //if (!result.Success)
            //{
            //    return new ErrorResult();
            //}
            var map = _mapper.Map<List<Order>>(orderCreateDTOs);
            _orderDAL.AddRange(userId, map);
            var products = orderCreateDTOs.Select(x => new ProductDecrementQuantityDTO
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
            }).ToList();
            _productService.RemoveProductCount(products);
            return new SuccessResult("Order Created Successfully!");
        }

        public IDataResult<UserOrderDTO> GetOrdersByUser(int userId)
        {
            throw new NotImplementedException();
        }
        private IResult IsProductInStock(List<int> productIds)
        {
            var product = _productService.CheckProductCount(productIds);
            if (!product.Data)
            {
                return new SuccessResult();
            }
            else
            {
                return new ErrorResult();
            }
        }
    }
}
