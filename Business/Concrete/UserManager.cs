using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using Entities.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDAL _userDAL;
        private readonly IMapper _mapper;

        public UserManager(IUserDAL userDAL, IMapper mapper)
        {
            _userDAL = userDAL;
            _mapper = mapper;
        }

        public IResult Login(UserLoginDTO userLoginDTO)
        {
            throw new NotImplementedException();
        }

        public IResult Register(UserRegisterDTO userRegisterDTO)
        {
            throw new NotImplementedException();
        }

        public IResult VerifyEmail(string email, string verifyToken)
        {
            throw new NotImplementedException();
        }
    }
}
