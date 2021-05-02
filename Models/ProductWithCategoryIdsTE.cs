using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class ProductWithCategoryIdsTE
    {
        public int Id { get; set; }
        public string ProductName { get; set; }

        [NotMapped]
        public IFormFile Imagefile { get; set; }
        public int CategoryId { get; set; }
    }
}
