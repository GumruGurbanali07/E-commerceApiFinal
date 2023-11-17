using Core.Utilities.Results.Abstract;
using Entities.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
   public interface IUserService
    {
        //use exist user
        IResult Login(UserLoginDTO userLoginDTO);
        //create new user
        IResult Register(UserRegisterDTO userRegisterDTO);
        IResult VerifyEmail(string email, string verifyToken);
        IDataResult<UserOrderDTO> GetUserOrders(int userId);
       
    }
}
