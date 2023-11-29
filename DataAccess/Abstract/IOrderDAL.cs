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
        //add a range of orders associated with a specific user.
        void AddRange(int userId, List<Order> orders);
        void Update(Order entity);
    }
}
