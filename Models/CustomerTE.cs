using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class CustomerTE
    {
        public int Id { get; set; }
        [Required]
        public string customermobile { get; set; }
        [Required]
        public string CustomerName { get; set; }
        public string Customeraddress { get; set; }

        public string CustomerId { get; set; }
    }
}
