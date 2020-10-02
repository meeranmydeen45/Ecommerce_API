using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class ProductTE
    {

        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }
    }

}
