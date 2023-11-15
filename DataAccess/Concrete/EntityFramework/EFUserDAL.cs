using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFUserDAL : EFRepositoryBase<User, AppDbContext>, IUserDAL
    {
        public User GetUserOrders(int userId)
        {
           using var context=new AppDbContext();
            var user=context.AppUsers.Include(x=>x.Orders).ThenInclude(x=>x.Product).FirstOrDefault(x=>x.Id==userId );
            return user;
         }       
        
    }
}
