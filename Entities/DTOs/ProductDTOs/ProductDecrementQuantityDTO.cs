using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    // Out Of Stock System
    public class ProductDecrementQuantityDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
