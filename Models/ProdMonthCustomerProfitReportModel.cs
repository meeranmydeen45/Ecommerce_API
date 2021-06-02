using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class ProdMonthCustomerProfitReportModel
    {
        public string Billnumber { get; set; }

        public string Customername { get; set; }

        public string Date { get; set; }

        public int Profit { get; set; }
    }
}
