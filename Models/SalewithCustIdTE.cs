using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class SalewithCustIdTE
    {
        public int Id { get; set; }
        public string Productname { get; set; }
        public string Prodsize { get; set; }
        public int Quantity { get; set; }
        public int Unitprice { get; set; }
        public int TotalCost { get; set; }
        public DateTime Purchasedate { get; set; }
        public int Productid { get; set; }
        public string Custid { get; set; }

        
    }
}
