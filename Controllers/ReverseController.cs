using Ecommerce_NetCore_API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReverseController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly Context _context;
        public ReverseController(IWebHostEnvironment hostEnvironment, Context context)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
        }

        [HttpPost("reverseentry")]
        public ActionResult<string> ReverseEntry([FromForm]ReverseEntryDataTE data)
        {
            string result = "";
            return Ok(result);
        }
    }
}
