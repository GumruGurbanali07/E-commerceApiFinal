using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using ECommerce.Business.Validations.OrderValidator;
using Entities.Concrete;
using Entities.DTOs.OrderDTOs;
using Entities.DTOs.ProductDTOs;
using Entities.Enums;
using FluentValidation.Results;
using Serilog;

namespace Business.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDAL _orderDAL;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;


        public OrderService(IOrderDAL orderDAL, IMapper mapper, IProductService productService)
        {
            _orderDAL = orderDAL;
            _mapper = mapper;
            _productService = productService;


            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .WriteTo.File("logs/myOrderLogs-.txt", rollingInterval: RollingInterval.Day)
               .CreateLogger();

            Log.Information("OrderService instance created.");
        }



        public IResult CreateOrder(int userId, List<OrderCreateDTO> orderCreateDTOs)
        {
            try
            {
                Log.Information($"Creating order for user ID: {userId}");

                var productIds = orderCreateDTOs.Select(x => x.ProductId).ToList();
                var quantities = orderCreateDTOs.Select(x => x.Quantity).ToList();
                var map = _mapper.Map<List<Order>>(orderCreateDTOs);
                _orderDAL.AddRange(userId, map);

                foreach (var orderCreateDTO in orderCreateDTOs)
                {
                    var validationResult = ValidateOrderCreate(orderCreateDTO);
                    if (!validationResult.IsValid)
                    {
                        Log.Warning($"Validation failed for order creation. Error: {validationResult.Errors.First().ErrorMessage}");
                        return new ErrorResult(validationResult.Errors.First().ErrorMessage);
                    }
                }

                var products = orderCreateDTOs.Select(x => new ProductDecrementQuantityDTO
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                }).ToList();

                _productService.RemoveProductCount(products);

                Log.Information("Order created successfully.");
                return new SuccessResult("Order Created Successfully!");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating an order.");
                throw;
            }
        }

        public IResult ChangeOrderStatus(string orderNumber, OrderEnum orderEnum)
        {
            try
            {
                Log.Information($"Changing order status for order number: {orderNumber}");

                var order = _orderDAL.Get(x => x.OrderNumber == orderNumber);

                if (order == null)
                {
                    Log.Warning($"Order with order number {orderNumber} not found.");
                    return new ErrorResult("Order not found.");
                }

                order.OrderEnum = orderEnum;

                // Log the new order status here
                Log.Information($"New order status: {orderEnum}");

                _orderDAL.Update(order);

                Log.Information("Order status changed successfully.");
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while changing order status.");
                throw;
            }
        }

        public ValidationResult ValidateOrderCreate(OrderCreateDTO orderCreateDTO)
        {
            try
            {
                Log.Information("Validating order creation");

                var validator = new OrderCreateDtoValidator();
                return validator.Validate(orderCreateDTO);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while validating order creation.");
                throw;
            }
        }
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

