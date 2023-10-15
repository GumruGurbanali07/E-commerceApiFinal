using Core.Utilities.Results.Abstract;
using Entities.DTOs.WishListDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IWishListService
    {
        IDataResult<List<WishListItemDTO>> GetUserWishList(int userId);
        IResult AddWishList(int userId, WishListAddItemDTO wishListAddItemDTO);
    }
}
