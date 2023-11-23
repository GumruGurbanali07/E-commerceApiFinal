using Core.Utilities.Results.Abstract;
using Entities.DTOs.OrderDTOs;
using Entities.DTOs.UserDTOs;
using Entities.Enums;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IOrderService
    {
        // for creating orders.
        IResult CreateOrder(int userId, List<OrderCreateDTO> orderCreateDTOs);
        //for changing the status of an order identified by its number.
        IResult ChangeOrderStatus(string orderNumber, OrderEnum orderEnum);
        ValidationResult ValidateOrderCreate(OrderCreateDTO orderCreateDTO);
    }
}
