using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class ProdProfitReportModel
    {
        public string Productname { get; set; }
        public string Size { get; set; }
        public int Purchasecostaverage { get; set; }
        public int Salecostaverage { get; set; }
        public int Quantitysold { get; set; }
        public int Profit { get; set; }
    }
}
