using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.OrderDTOs;
using Entities.DTOs.ProductDTOs;
using Entities.Enums;

namespace Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDAL _orderDAL;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;


        public OrderManager(IOrderDAL orderDAL, IMapper mapper, IProductService productService)
        {
            _orderDAL = orderDAL;
            _mapper = mapper;
            _productService = productService;

        }



        public IResult CreateOrder(int userId, List<OrderCreateDTO> orderCreateDTOs)
        {
            //we must take ProductId and Quantity of order of each orderCreateDTO
            var productIds = orderCreateDTOs.Select(x => x.ProductId).ToList();//?
            var quantities = orderCreateDTOs.Select(x => x.Quantity).ToList();
            //// Mapping OrderCreateDTOs to Order entities
            var map = _mapper.Map<List<Order>>(orderCreateDTOs);
            // Adding the orders to the database
            _orderDAL.AddRange(userId, map);
            // Creating a list of ProductDecrementQuantityDTOs for product service
            var products = orderCreateDTOs.Select(x => new ProductDecrementQuantityDTO
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
            }).ToList();
            // Removing quantities from product stock
            _productService.RemoveProductCount(products);//??
            return new SuccessResult("Order Created Successfully!");
        }
        public IResult ChangeOrderStatus(string orderNumber, OrderEnum orderEnum)
        {
            // Retrieving the order by orderNumber
            var order = _orderDAL.Get(x => x.OrderNumber == orderNumber);
            // Changing the order status
            order.OrderEnum = orderEnum;
            // Updating the order in the database
            _orderDAL.Update(order);
            return new SuccessResult();
        }


        //private IResult IsProductInStock(List<int> productIds)
        //{
        //    var product = _productService.CheckProductCount(productIds);
        //    if (!product.Data)
        //    {
        //        return new ErrorResult();
        //    }
        //    else
        //    {
        //        return new SuccessResult();
        //    }
        //}
    }
}
