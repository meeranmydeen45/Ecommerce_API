using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class ProdSaleHistoryReportModel
    {
        public string Productname { get; set; }

        public int Quantity { get; set; }

        public int Cost { get; set; }
        public string Size { get; set; }

        public DateTime Date { get; set; }

        public int Productid { get; set; }
    }
}
