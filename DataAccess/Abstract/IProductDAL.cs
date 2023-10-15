using Core.DataAccess;
using Entities.Concrete;
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
        int GetProductCountByCategory(int categoryId);  
    }
}
