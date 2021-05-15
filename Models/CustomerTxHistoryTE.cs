using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class CustomerTxHistoryTE
    {
        public int Id { get; set; }
        public int Paidamount { get; set; }
        public int Billnumber { get; set; }
        public DateTime Paiddate { get; set; }
        public string Paymentmode { get; set; }
        public int Customerid { get; set; }
    }
}
