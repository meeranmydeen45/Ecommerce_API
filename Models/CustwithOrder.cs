using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class CustwithOrder
    {
        public List<CartItems> cartItems { get; set; }
        public CustomerTE customer { get; set; }
        public int Totalcost { get; set; }
    }
}
