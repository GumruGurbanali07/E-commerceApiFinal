using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IUserDAL : IRepositoryBase<User>
    {
        //for retrieving the orders for the user with the specified userId.
        User GetUserOrders(int userId);
    }
}
