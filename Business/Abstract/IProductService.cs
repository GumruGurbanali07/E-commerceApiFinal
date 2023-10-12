using Core.Utilities.Results.Abstract;
using Entities.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductService
    {
        IResult ProductCreate(ProductCreateDTO productCreateDTO);
        IResult ProductUpdate(ProductUpdateDTO productUpdateDTO);
        IDataResult<ProductDetailDTO> ProductDetail(int productId);
    }
}
