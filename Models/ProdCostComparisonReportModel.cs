using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class ProdCostComparisonReportModel
    {
        public string Productname { get; set; }
        public string Size { get; set; }

        public int Latestpurchaseprice { get; set; }

        public int Saleprice { get; set; }
    }
}
