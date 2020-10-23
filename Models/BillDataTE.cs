using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class BillDataTE
    {
        public int Id { get; set; }
        public int BillNumber { get; set; }
        public int BillAmount { get; set; }
        public DateTime BillDate { get; set; }
        public Byte[] BillByteArray { get; set; }
        public int CustomerId { get; set; }
    }
}
