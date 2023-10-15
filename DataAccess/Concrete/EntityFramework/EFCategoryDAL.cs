
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
    public class EFCategoryDAL : EFRepositoryBase<Category, AppDbContext>, ICategoryDAL
    {
        public List<Category> GetNavbarCategories()
        {
           using var context=new AppDbContext();
            var categories=context.Categories.Where(x=>x.Status==true).Take(10).ToList();
            return categories;
        }

        public List<Category> GetFeaturedCategories()
        {
            using var context = new AppDbContext();
            var categories = context.Categories.Include(x=>x.Products).Where(x => x.Status == true).Take(10).ToList();
            return categories;
        }

       
    }
}
