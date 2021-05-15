using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class BillsPendingTE
    {
        public int Id { get; set; }
        public int Pendingamount { get; set; }
        public int Billnumber { get; set; }
        public bool Iscompleted { get; set; }    
        public int Customerid { get; set; }
    }
}