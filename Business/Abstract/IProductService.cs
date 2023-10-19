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
        IResult ProductDelete(int productId);
        IDataResult<ProductDetailDTO> ProductDetail(int productId);
        IDataResult<List<ProductFeaturedDTO>> ProductFeaturedList();
        IDataResult<List<ProductRecentDTO>> ProductRecentList();
        IDataResult<List<ProductFilterDTO>> ProductFilterList(int categoryId,int minPrice,int maxPrice);
        IDataResult<bool> CheckProductCount(List<int> productIds);
        IResult RemoveProductCount(List<ProductDecrementQuantityDTO> productDecrementQuantityDTOs);
    }
}
