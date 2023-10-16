using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IOrderDAL : IRepositoryBase<Order>
    {
        void AddRange(int userId, List<Order> orders);
    }
}
