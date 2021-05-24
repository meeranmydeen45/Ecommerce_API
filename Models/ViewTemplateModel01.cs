using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class ViewTemplateModel
    {
        public int CategoryValue { get; set; }

        public int ProductValue { get; set; }

        public int SizeValue { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
