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
            var result = orders.Select(x => { x.UserId = userId;
                x.CreatedDate = DateTime.Now;
                x.OrderNumber = Guid.NewGuid().ToString().Substring(0, 18);
                x.OrderEnum = OrderEnum.OnPending;
                return x; }).ToList();
            context.Orders.AddRange(result);
            context.SaveChanges();
        }
    }
}
