using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class Profitmodel
    {

        public int Productid { get; set; }
        public string Size  { get; set; }

        public int Quantity { get; set; }
        public int  Profit { get; set; }
    }
}
