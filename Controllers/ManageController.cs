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
    public class ManageController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly Context _context;

        public ManageController(IWebHostEnvironment hostEnvironment, Context context)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
        }

        [HttpPost("getstockprice")]
        public ActionResult<StockTE> GetStockPrice([FromForm]ProductWithCategoryIdsTE prod)
        {
           int ProdId = _context.products.Single(x => x.ProductName == prod.ProductName && x.CategoryId == prod.CategoryId).Id;
           StockTE stock = _context.stocks.SingleOrDefault(x => x.ProductId == ProdId && x.Size == prod.ProductSize);
            
            return Ok(stock);
        }

        [HttpPost("setstockprice")]
        public ActionResult<String> SetStockPrice([FromForm]ProductWithCategoryIdsTE prod)
        {
            int ProdId = _context.products.Single(x => x.ProductName == prod.ProductName && x.CategoryId == prod.CategoryId).Id;
            StockTE stock = _context.stocks.SingleOrDefault(x => x.ProductId == ProdId && x.Size == prod.ProductSize);
            if (stock != null)
            {
                stock.Cost = prod.Cost;
                _context.Entry(stock).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();            
            }
            else
            {
                return Ok("Product Not Found.. Please check!");

            }
            return Ok("Success Fully Done!");
        }
    }
}
