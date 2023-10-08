using AutoMapper;
using Business.Abstract;
using Core.Utilities.Business;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Concrete;
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
            var result = BusinessRules.Check(CheckUserExist(userRegisterDTO.Email));
            if (!result.Success)
            {
                return new ErrorResult("Email is already exist!!");
            }
            var map = _mapper.Map<User>(userRegisterDTO);
            HashingHelper.HashPassword(userRegisterDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
           
            map.Token=Guid.NewGuid().ToString();
            map.TokenExpiresDate=DateTime.Now.AddMinutes(10);
            
            map.PasswordSalt = passwordSalt;
            map.PasswordHash=passwordHash;
            _userDAL.Add(map);
            return new SuccessResult();
        }

        public IResult VerifyEmail(string email, string verifyToken)
        {
            throw new NotImplementedException();
        }
        public IResult CheckUserExist(string email)
        {
           var user= _userDAL.Get(x=>x.Email==email);
            if (user != null)
            {
                return new ErrorResult("Email exist");
            }
            return new SuccessResult();
        }

        private IResult CheckUserConfirmedEmail(string email)
        {
            var user = _userDAL.Get(x => x.Email == email);
            if (!user.EmailConfirmed)
            {
                _userDAL.Delete(user);
            }
            return new SuccessResult();
        }


    }
}
