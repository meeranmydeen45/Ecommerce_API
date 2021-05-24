using Ecommerce_NetCore_API.Models;
using Ecommerce_NetCore_API.Services;
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
    public class ViewtemplateoneController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly Context context;
        public ViewtemplateoneController(IWebHostEnvironment _webHostEnvironment, Context _context)
        {
            webHostEnvironment = _webHostEnvironment;
            context = _context;
        }

        [HttpGet("gethistory")]
        public ActionResult<string> GetAllData()
        {
            return Ok("test");
        }

        [HttpPost("prodaddhistory")]
        public ActionResult<List<ProdAddHistoryTE>> GetProductRegisteredHistory([FromForm]ViewTemplateModelOne data)
        {
            bool IsFromDateValid = false;
            bool IsEndDateValid = false;
            DateTime FromDate;
            string CompleteFromDateString ="";
            DateTime EndDate;
            string CompleteEndDateString ="";
            DateTime dt;
            List<ProdAddHistoryTE> products = new List<ProdAddHistoryTE>();
            string Result = "";
            bool NotFound = false;

            if (data.FromDate != null && data.EndDate != null)
            {
                
                DateService DateService1 = new DateService();
                CompleteFromDateString = DateService1.GetCompleteDate(data.FromDate);
                IsFromDateValid = DateTime.TryParse(CompleteFromDateString, out dt);

                DateService DateService2 = new DateService();
                CompleteEndDateString = DateService2.GetCompleteDate(data.EndDate);
                IsEndDateValid = DateTime.TryParse(CompleteEndDateString, out dt);
            }
            if(IsFromDateValid && IsEndDateValid)
            {
                FromDate = Convert.ToDateTime(CompleteFromDateString);
                EndDate =  Convert.ToDateTime(CompleteEndDateString);

                if(data.ProductValue != null && data.SizeValue == null)
                {
                 var ProductsRegistered = context
                        .prodAddHistoryData
                        .Where(x => x.Date >= FromDate && x.Date <= EndDate && x.ProductId.ToString() == data.ProductValue).ToList();
                    if (ProductsRegistered.Count() != 0)
                    {
                        products = ProductsRegistered.ToList();
                    }
                    else
                    {
                        Result = "Not Found.. Change Your Search Criteria!";
                        NotFound = true;
                    }

                }
                
                if(data.ProductValue !=null && data.SizeValue != null)
                {

                }

            }


            
            if(NotFound)
            {
                return Ok(Result);
            }
            return Ok(products);

        }

    }
}
