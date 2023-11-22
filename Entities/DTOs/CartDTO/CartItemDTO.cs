using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Entities.DTOs.CartDTO
{
    public class CartItemDTO
    {
        public Product Product { get; set; }=new Product();
        public int Quantity { get; set; }   
    }
}
