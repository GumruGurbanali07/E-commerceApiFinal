using Bogus;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataHelper
{
    public static class DataSeeder
    {
        public static void AddData()
        {
            using AppDbContext context = new();

            if (!context.Categories.Any())
            {
                var fakeCategories = new Faker<Category>();

                fakeCategories.RuleFor(x => x.CategoryName, z => z.Commerce.Categories(1)[0]);
                fakeCategories.RuleFor(x => x.PhotoUrl, z => z.Image.PicsumUrl());
                fakeCategories.RuleFor(x => x.Status, z => z.Random.Bool());
                fakeCategories.RuleFor(x => x.CreatedDate, z => z.Date.Recent());

                var categories = fakeCategories.Generate(20);
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

           
        }
    }
}
