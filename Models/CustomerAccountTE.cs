using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class CustomerAccountTE
    {
        public int Id { get; set; }
        public string Customername { get; set; }
        public int Availableamount { get; set; }
        public string Customerid { get; set; }
    }
}
