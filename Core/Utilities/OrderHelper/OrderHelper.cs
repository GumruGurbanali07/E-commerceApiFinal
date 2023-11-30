using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Utilities.OrderHelper
{
    public class OrderHelper
    {
        public static decimal ShippingFee { get; } = 5;
        public static Dictionary<string, string> PaymentMethods { get; } = new()
        {
            {"Cash","Cache on delivery" },
            {"Paypal","Paypal" },
            { "Credit Card","Credit Card"}
        };
        public static List<string> PaymentStatus { get; } = new()
        {
            "Pending","Accepted","Canceled"
        };
        public static List<string> OrderStatus { get; } = new()
        {
            "Created","Accepted","Canceled","Shipped","Delivered","Returned"

        };
      
        public static Dictionary<int, int> GetProductDictionary(string productId)
        {
            var productDictionary = new Dictionary<int, int>();
            if (!string.IsNullOrEmpty(productId))
            {
                string[] productIdArray = productId.Split('-');
                foreach (var productid in productIdArray)
                {
                    try
                    {
                        int id = int.Parse(productid);
                        if (productDictionary.ContainsKey(id))
                        {
                            productDictionary[id] += 1;
                        }
                        else
                        {
                            productDictionary.Add(id, 1);
                        }
                    }
                    catch (Exception)
                    {
                       
                    }
                }
            }
            return productDictionary;
        }

    }
}