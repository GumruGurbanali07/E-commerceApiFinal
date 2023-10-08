using Entities.DTOs.SpecificationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class ProductCreateDTO
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public bool IsFeatured { get; set; }
        public bool Status { get; set; }
        public int CategoryId { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<SpecificationAddDTO> SpecificationAddDTOs { get; set; }
    }
}
