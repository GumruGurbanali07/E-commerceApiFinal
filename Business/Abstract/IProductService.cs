using Core.Utilities.Results.Abstract;
using Entities.DTOs.ProductDTOs;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductService
    {
        //creates a new product in the database. 
        IResult ProductCreate(ProductCreateDTO productCreateDTO);
        //updates an existing product in the database. 
        IResult ProductUpdate(ProductUpdateDTO productUpdateDTO);
        //deletes an existing product from the database. 
        IResult ProductDelete(int productId);
        //retrieves the details of an existing product from the database.
        //The productId parameter specifies the ID of the product to retrieve.
        IDataResult<ProductDetailDTO> ProductDetail(int productId);
        //retrieves a list of featured products from the database.
        IDataResult<List<ProductFeaturedDTO>> ProductFeaturedList();
        //retrieves a list of recent products from the database.
        IDataResult<List<ProductRecentDTO>> ProductRecentList();
        //The categoryId parameter specifies the ID of the category to filter by,
        //the minPrice parameter specifies the minimum price of the products to filter by,
        //and the maxPrice parameter specifies the maximum price of the products to filter by.
        IDataResult<List<ProductFilterDTO>> ProductFilterList(int categoryId,int minPrice,int maxPrice);
        //checks whether the specified products have enough stock to be ordered.
        //The productIds parameter specifies the IDs of the products to check.
        IDataResult<bool> CheckProductCount(List<int> productIds);
        //method removes the specified quantity from the stock of each product in the list of productDecrementQuantityDTOs. 
        IResult RemoveProductCount(List<ProductDecrementQuantityDTO> productDecrementQuantityDTOs);
        ValidationResult ValidateProduct(ProductCreateDTO productCreateDTO);

      

    }
}
