using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class ReverseEntryDataTE
    {
        public int Id { get; set; }
        public string Billnumber { get; set; }
        public int Productid  { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public int Saleprice { get; set; }
        public DateTime Date { get; set; }
    }
}
