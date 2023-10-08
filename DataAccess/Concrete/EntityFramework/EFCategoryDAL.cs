
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFCategoryDAL : EFRepositoryBase<Category, AppDbContext>, ICategoryDAL
    {
        public List<Category> GetCategories()
        {
            throw new NotImplementedException();
        }

        public List<Category> GetFeaturedCategories()
        {
            throw new NotImplementedException();
        }
    }
}
