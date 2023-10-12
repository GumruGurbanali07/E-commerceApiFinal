using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFProductDAL : EFRepositoryBase<Product, AppDbContext>, IProductDAL
    {
        public Product GetProduct(int id)
        {
            using var context= new AppDbContext();
            var product = context.Products.Include(x=>x.Category).SingleOrDefault(p => p.Id == id);
            return product;
        }
    }
}
