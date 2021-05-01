using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class CustObjwithBillNo
    {
        public CustwithOrder Custwithorder { get; set; }
        public int Billnumber { get; set; }

        public int Billprofit { get; set; }
    }
}
