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
            if (!context.Products.Any())
            {
                var fakeProducts = new Faker<Product>();

                fakeProducts.RuleFor(x => x.ProductName, z => z.Commerce.ProductName());
                fakeProducts.RuleFor(x => x.CategoryId, z => z.Random.Int(1, 20));
                fakeProducts.RuleFor(x => x.Quantity, z => z.Random.Int(1, 100));
                fakeProducts.RuleFor(x => x.Price, z => z.Random.Decimal(100, 10000));
                fakeProducts.RuleFor(x => x.Discount, z => z.Random.Decimal(0, 10000));
                fakeProducts.RuleFor(x => x.IsFeatured, z => z.Random.Bool());
                fakeProducts.RuleFor(x => x.Status, z => z.Random.Bool());
                fakeProducts.RuleFor(x => x.CreatedDate, z => z.Date.Recent());
                fakeProducts.RuleFor(x => x.Description, z => z.Commerce.ProductAdjective());
                fakeProducts.RuleFor(x => x.PhotoUrl, z => z.Image.PicsumUrl());

                var products = fakeProducts.Generate(100);
                context.Products.AddRange(products);
                context.SaveChanges();
            }

        }
    }
}
