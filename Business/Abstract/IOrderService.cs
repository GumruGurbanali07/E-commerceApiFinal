using Core.Utilities.Results.Abstract;
using Entities.DTOs.OrderDTOs;
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
    }
}
