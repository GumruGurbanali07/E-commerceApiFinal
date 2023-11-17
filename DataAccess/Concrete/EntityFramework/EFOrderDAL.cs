using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.OrderDTOs;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFOrderDAL : EFRepositoryBase<Order, AppDbContext>, IOrderDAL
    {
        public void AddRange(int userId, List<Order> orders)
        {
            using var context = new AppDbContext();
            //For each order in the orders list,
            //properties like UserId, CreatedDate, OrderNumber, and OrderEnum
            //are modified before adding them to the context.
            var result = orders.Select(x => {
                x.UserId = userId;
                x.CreatedDate = DateTime.Now;
                //generate unique identitifier and vonvert it string representation,0-stat index,18 length
                x.OrderNumber = Guid.NewGuid().ToString().Substring(0, 18);
                x.OrderEnum = OrderEnum.OnPending;
                return x; }).ToList();
            //modified orders are added to the context
            context.Orders.AddRange(result);
            context.SaveChanges();
        }
    }
}
