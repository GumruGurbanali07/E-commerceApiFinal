using Core.DataAccess;
using ECommerce.Entities.DTOs.ProductDTOs;
using Entities.Concrete;
using Entities.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IProductDAL : IRepositoryBase<Product>
    {
        Product GetProduct(int id);
        List<Product> GetFeaturedProducts();
        List<Product> GetRecentProducts();

        //int GetProductCountByCategory(int categoryId);
        void RemoveProductCount(List<ProductDecrementQuantityDTO> productDecrementQuantityDTOs);
        List<ProductSearchDTO> SearchProducts(string query);
    }
}
