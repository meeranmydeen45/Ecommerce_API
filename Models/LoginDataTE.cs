using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class LoginDataTE
    {
        public int Id { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public string  password { get; set; }

        public bool isLocked { get; set; }

    }
}