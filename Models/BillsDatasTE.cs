using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class BillsDatasTE
    {
        public int Id { get; set; }

        [Required]
        public int Billnumber { get; set; }
        public int Billamount { get; set; }

        public int Deduction { get; set; }

        public int Payableamount { get; set; }

        public int Billprofit { get; set; }

        public Byte[] Billbytearray { get; set; }
        
        public DateTime Billdate { get; set; }

        public bool Ispaid { get; set; }

        public bool Isbillmodified { get; set; }

        public int Customerid { get; set; }
    }
}
