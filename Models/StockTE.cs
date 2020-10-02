using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class StockTE
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public int Cost { get; set; }
        public int ProductId { get; set; }
    }
}
