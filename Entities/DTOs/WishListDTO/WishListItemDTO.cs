﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.WishListDTO
{
    public class WishListItemDTO
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
    }
}
