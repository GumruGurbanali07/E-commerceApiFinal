using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Entities.DTOs.CartDTO
{
    public class CartDTO
    {
        public List<CartItemDTO> CartItems { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
