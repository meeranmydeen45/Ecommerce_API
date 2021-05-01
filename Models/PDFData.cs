using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class PDFData
    {
        public int Id { get; set; }

        public int Billnumber{ get; set; }

        public int Billamount { get; set; }

       public int Deduction { get; set; }

       public int Payableamount { get; set; }

       public int Billprofit { get; set; }

       public string Base64 { get; set; }

       public int Customerid { get; set; }
       


    }
}
