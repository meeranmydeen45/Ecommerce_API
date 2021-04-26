using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class CartItems
    {
        public int id { get; set; }
        public string productName { get; set; }
        public string productImage { get; set; }
        public string size { get; set; }
        public int Quantity { get; set; }
       
        public int totalQuantity { get; set; }
        public int cost { get; set; }

        public int totalCost { get; set; }
    }
}

