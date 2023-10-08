using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Product : BaseEntity, IEntity
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsFeatured { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Specification> Specifications { get; set; }
    }
}
