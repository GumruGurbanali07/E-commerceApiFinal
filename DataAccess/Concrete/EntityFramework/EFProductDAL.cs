using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using ECommerce.Entities.DTOs.ProductDTOs;
using Entities.Concrete;
using Entities.DTOs.ProductDTOs;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFProductDAL : EFRepositoryBase<Product, AppDbContext>, IProductDAL
    {
        public List<Product> GetFeaturedProducts()
        {
            //take in 8 products in descending order where these products is featured and status is true
            using var context = new AppDbContext();
            var products = context.Products.Where(x => x.IsFeatured == true && x.Status == true).OrderByDescending(x => x.CreatedDate).Take(8).ToList();
            return products;
        }

        public Product GetProduct(int id)
        {
            //gets a product from the database based on its ID,
            //and it also includes additional details about the product's specifications and category.
            using var context = new AppDbContext();
            var product = context.Products.Include(x => x.Specifications).Include(x => x.Category).SingleOrDefault(x => x.Id == id);
            return product;
        }

        //public int GetProductCountByCategory(int categoryId)
        //{
        //    using var context = new AppDbContext();
        //    var result = context.Products.Where(x => x.CategoryId == categoryId).Count();
        //    return result;
        //}

        public List<Product> GetRecentProducts()
        {
            using var context = new AppDbContext();
            var products = context.Products.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).Take(8).ToList();
            return products;
        }

        public void RemoveProductCount(List<ProductDecrementQuantityDTO> productDecrementQuantityDTOs)
        {
            using var context = new AppDbContext();
            //taking a list of DTOs that contain information about products and the quantity to be reduced.
            //For each item in the list, it finds the corresponding product in the database,
            //decreases its quantity, and updates the database with the new quantity.
            for (int i = 0; i < productDecrementQuantityDTOs.Count; i++)
            {
                var products = context.Products.FirstOrDefault(x => x.Id == productDecrementQuantityDTOs[i].ProductId);
                products.Quantity -= productDecrementQuantityDTOs[i].Quantity;
                context.Products.Update(products);
                context.SaveChanges();
            }
        }

        public List<ProductSearchDTO> SearchProducts(string query)
        {
            using var context = new AppDbContext();

            var searchResults = context.Products
                .Where(p => p.ProductName.Contains(query, StringComparison.OrdinalIgnoreCase))
              .Select(p => new ProductSearchDTO
              {
                  Id = p.Id,
                  ProductName = p.ProductName,
                  Price = p.Price,
                  Discount = p.Discount,
                  Description = p.Description,
                  Quantity = p.Quantity,
                  IsFeatured = p.IsFeatured,
                  Status = p.Status,
                  CategoryId = p.CategoryId,
                  PhotoUrl = p.PhotoUrl,
                  CreatedDate = p.CreatedDate,
                  CategoryName = p.Category.CategoryName, // Eğer Category ilişkili bir özellikse

              }).ToList();

            return searchResults;




        }
    }
}
