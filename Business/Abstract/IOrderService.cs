using Core.Utilities.Results.Abstract;
using Entities.DTOs.OrderDTOs;
using Entities.DTOs.UserDTOs;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IOrderService
    {
        IResult CreateOrder(int userId, List<OrderCreateDTO> orderCreateDTOs);
        IResult ChangeOrderStatus(string orderNumber, OrderEnum orderEnum);
        IDataResult<UserOrderDTO> GetOrdersByUser(int userId);
    }
}
