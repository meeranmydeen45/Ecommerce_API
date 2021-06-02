using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class CustomerTxHistoryReportModel
    {
        public string Customername { get; set; }

        public int Billnumber { get; set; }
        public int Paidamount { get; set; }
        public string Paymentmode { get; set; }
        public DateTime Paiddate { get; set; }
       
    }
}
