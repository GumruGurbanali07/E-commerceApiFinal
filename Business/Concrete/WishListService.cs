using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.WishListDTO;
using Serilog;

namespace Business.Concrete
{
    public class WishListService : IWishListService
    {
        private readonly IWishListDAL _wishListDAL;
        private readonly IMapper _mapper;

        public WishListService(IWishListDAL wishListDAL, IMapper mapper)
        {
            _wishListDAL = wishListDAL;
            _mapper = mapper;
            Log.Logger = new LoggerConfiguration()
                   .MinimumLevel.Debug()
                   .WriteTo.Console()
                   .WriteTo.File("logs/myWishListLogs-.txt", rollingInterval: RollingInterval.Day)
                   .CreateLogger();

            Log.Information("WishListService instance created.");
        }

        public IResult AddWishList(int userId, WishListAddItemDTO wishListAddItemDTO)
        {
            try
            {
                Log.Information($"Adding item to wish list for user ID: {userId}");

                var map = _mapper.Map<WishList>(wishListAddItemDTO);
                map.CreatedDate = DateTime.Now;
                map.UserId = userId;
                map.Status = true;
                _wishListDAL.Add(map);

                Log.Information("Item added to wish list successfully.");
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while adding item to wish list.");
                throw;
            }
        }

        public IDataResult<List<WishListItemDTO>> GetUserWishList(int userId)
        {
            try
            {
                Log.Information($"Getting wish list for user ID: {userId}");

                var userWishList = _wishListDAL.GetUserWishList(userId);
                var map = _mapper.Map<List<WishListItemDTO>>(userWishList);

                if (!userWishList.Any())
                {
                    Log.Warning("User wish list is empty.");
                    return new ErrorDataResult<List<WishListItemDTO>>();
                }

                Log.Information("Wish list retrieved successfully.");
                return new SuccessDataResult<List<WishListItemDTO>>(map);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while getting user wish list.");
                throw;
            }
        }
    }
}
