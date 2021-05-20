using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class PendingBills
    {
        public string Billnumber { get; set; }
        public int Pendingamount { get; set; }
        public string Customername { get; set; }
        public string Customerid { get; set; }
        public string Customermobile { get; set; }
        public string TextBoxValue { get; set; }
        public string SearchCriteria { get; set; }
    }
}
