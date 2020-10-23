using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class PDFData
    {
        public string FileName { get; set; }
        public string base64 { get; set; }
    }
}
