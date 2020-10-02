using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class ProdAddHistoryTE
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Cost { get; set; }
        public string Size { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int ProductId { get; set; }


    }
}
