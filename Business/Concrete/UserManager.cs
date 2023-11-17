using AutoMapper;
using Business.Abstract;
using Core.Utilities.Business;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.UserDTOs;
using Entities.ShareModels;
using MassTransit;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDAL _userDAL;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public UserManager(IUserDAL userDAL, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _userDAL = userDAL;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }



        public IResult Login(UserLoginDTO userLoginDTO)
        {
            //  // Checking business rules before attempting login
            var result = BusinessRules.Check(CheckUserConfirmedEmail(userLoginDTO.Email),
                           CheckUserPasswordVerify(userLoginDTO.Email, userLoginDTO.Password),
                           CheckUserLoginAttempt(userLoginDTO.Email));
            // Retrieving user by email
            var user = _userDAL.Get(x => x.Email == userLoginDTO.Email);
            // Handling results and generating a token if successful
            if (!result.Success)
            {
                return new ErrorResult("Email is not Confirmed!");
            }

            if (CheckUserExist(userLoginDTO.Email).Success)
            {
                user.LoginAttempt += 1;
                return new ErrorResult("User does not exist");
            }
            user.LoginAttempt = 0;
            var token = Token.TokenGenerator(user, "User");
            return new SuccessResult(token);
        }







        public IResult Register(UserRegisterDTO userRegisterDTO)
        {
            // Checking if the user already exists  
            var result = BusinessRules.Check(CheckUserExist(userRegisterDTO.Email));
            if (!result.Success)
            {
                return new ErrorResult("Email is already exist!!");
            }
            // Mapping DTO to entity and hashing the password
            var map = _mapper.Map<User>(userRegisterDTO);
            HashingHelper.HashPassword(userRegisterDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
            map.PasswordSalt = passwordSalt;
            map.PasswordHash = passwordHash;
            map.Token = Guid.NewGuid().ToString();
            map.TokenExpiresDate = DateTime.Now.AddMinutes(10);
            // Adding the user to the database
            _userDAL.Add(map);
            // Sending an email verification command
            SendEmailCommand sendEmailCommand = new()
            {
                Firstname = map.FirstName,
                Lastname = map.LastName,
                Email = map.Email,
                Token = map.Token,
            };
            _publishEndpoint.Publish<SendEmailCommand>(sendEmailCommand);
            return new SuccessResult("User Resgitered Successfully");
        }




        public IResult VerifyEmail(string email, string verifyToken)
        {
            var user = _userDAL.Get(x => x.Email == email);
            if (user == null)
            {
                return new ErrorResult("User isn't exist!!");
            }
            if (user.Token == verifyToken)
            {
                if (DateTime.Compare(user.TokenExpiresDate, DateTime.Now) < 0)
                {
                    return new SuccessResult();
                }
                return new ErrorResult();
            }
            user.EmailConfirmed = true;
            _userDAL.Update(user);
            return new SuccessResult();
        }

        private IResult CheckUserExist(string email)
        {
            //Checks if a user with the given email already exists.
            var user = _userDAL.Get(x => x.Email == email);
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
                user.TokenExpiresDate = DateTime.Now.AddMinutes(10);
                SendEmailCommand sendEmailCommand = new()
                {
                    Lastname = user.LastName,
                    Firstname = user.FirstName,
                    Token = user.Token,
                    Email = user.Email
                };
                _publishEndpoint.Publish<SendEmailCommand>(sendEmailCommand);
            }
            return new SuccessResult();
        }



        private IResult CheckUserPasswordVerify(string email, string password)
        {
            var user = _userDAL.Get(x => x.Email == email);
            var result = HashingHelper.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
            if (!result)
            {
                return new ErrorResult("Email or Password is not true");
            }
            return new SuccessResult();
        }
        private IResult CheckUserLoginAttempt(string email)
        {
            //Retrieve the user from the database based on the provided email
            var user = _userDAL.Get(x => x.Email == email);

            if (user.LoginAttempt > 3)
            {
                if (user.LoginAttemptExpires == null)
                {
                    user.LoginAttemptExpires = DateTime.Now.AddMinutes(10);
                }
                return new ErrorResult("Login Attempt more than 3 please wait 10 minute");
            }

            if (DateTime.Compare(user.LoginAttemptExpires, DateTime.Now) < 0)
            {
                return new SuccessResult();
            }
            return new SuccessResult();
        }

        public IDataResult<UserOrderDTO> GetUserOrders(int userId)
        {
            //retrieves user orders based on the user's ID.
            var result = _userDAL.GetUserOrders(userId);
            var map = _mapper.Map<UserOrderDTO>(result);
            return new SuccessDataResult<UserOrderDTO>(map);
        }
    }
}
