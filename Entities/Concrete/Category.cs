using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Category : BaseEntity, IEntity
    {
        public string CategoryName { get; set; }
        public string PhotoUrl { get; set; }
        public List<Product> Products { get; set; }
    }
}
