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
        public ActionResult<List<ProdAddHistoryReportModel>> GetProductRegisteredHistory([FromForm]ViewTemplateModelOne data, string groupvalue)
        {
            bool IsFromDateValid = false;
            bool IsEndDateValid = false;
            DateTime FromDate;
            string CompleteFromDateString ="";
            DateTime EndDate;
            string CompleteEndDateString ="";
            DateTime dt;
            bool SelectOne = false;
            bool SelectTwo = false;
            bool SelectThree = false;
           List<ProdAddHistoryTE> products = new List<ProdAddHistoryTE>();
           List<ProdAddHistoryReportModel> ReportModelList = new List<ProdAddHistoryReportModel>();
           //List<List<ProdAddHistoryTE>> TotalCollection = new List<List<ProdAddHistoryTE>>();
            string Result = "";
            bool NotFound = false;
            bool IsGroup = groupvalue == "GROUP" ? true : false;

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
                if(data.CategoryValue != null && data.ProductValue == null && data.SizeValue == null)
                {
                    SelectOne = true;
                    var ProductsByCategoryId =  context.products.Where(x => x.CategoryId.ToString() == data.CategoryValue).ToList();
                    if(ProductsByCategoryId.Count() != 0)
                    {
                        
                        foreach(var ProductsById in ProductsByCategoryId)
                        { 
                            var collection = context.prodAddHistoryData.Where(x => x.ProductId == ProductsById.Id && x.Date >= FromDate && x.Date <= EndDate).OrderBy(x => x.Size).ToList();
                            foreach(var prod in collection)
                            {
                                products.Add(prod);
                            }
                        }
                       // return Ok(products);
                    }
                    else
                    {
                        Result = "NotFound";
                        NotFound = true;
                    }
                }
                else if(data.ProductValue != null && data.SizeValue == null)
                {
                    SelectTwo = true;
                    var DataCollection = context
                           .prodAddHistoryData
                           .Where(x => x.ProductId.ToString() == data.ProductValue && x.Date >= FromDate && x.Date <= EndDate).OrderBy(x => x.Size).ToList();
                    if (DataCollection.Count() != 0)
                    {
                        
                        products = DataCollection;
                    }
                    else
                    {
                        Result = "NotFound";
                        NotFound = true;
                    }

                }
                else if(data.ProductValue !=null && data.SizeValue != null)
                {
                    SelectThree = true;
                    var  DataBySize = context.prodAddHistoryData
                        .Where(x => x.ProductId.ToString() == data.ProductValue && x.Size == data.SizeValue && x.Date >= FromDate && x.Date <= EndDate).ToList();
                    if(DataBySize.Count() > 0)
                    {
                        products = DataBySize;

                    }
                    else
                    {
                        Result = "NotFound";
                        NotFound = true;

                    }

                }
                if(products.Count() > 0 && SelectOne && IsGroup)
                {
                  var GroupBySizeSelectOne =  products.GroupBy(x => new { x.ProductId, x.Size }).Select(z => new ProdAddHistoryTE() { 
                      ProductId = z.Key.ProductId,
                      Size = z.Key.Size,
                      Quantity = z.Sum(x => x.Quantity),
                      Cost = z.Sum(x => x.Cost) / z.Count(),
                    });
                    products = GroupBySizeSelectOne.ToList();

                }
                else if(products.Count() > 0 && SelectTwo && IsGroup)
                {
                    var GroupBySelectTwo = products.GroupBy(x => x.Size).Select(z => new ProdAddHistoryTE() { 
                      ProductId = products[0].ProductId,
                      Size = z.Key,
                      Quantity = z.Sum(x => x.Quantity),
                      Cost = z.Sum(x => x.Cost) / z.Count(),
                    });
                    products = GroupBySelectTwo.ToList();
                }
                else if (products.Count() > 0 && SelectThree && IsGroup)
                {
                    ProdAddHistoryTE SelectThreeData = new ProdAddHistoryTE()
                    {
                        ProductId = products[0].ProductId,
                        Size = products[0].Size,
                        Quantity = products.Sum(x => x.Quantity),
                        Cost = products.Sum(x=> x.Cost) / products.Count()

                    };
                    products.Clear();
                    products.Add(SelectThreeData);
                }
                //Produt Name not avilable in products thats why converting into new Model with productName!!
                if(products.Count() > 0)
                {
                    foreach(var prod in products)
                    {
                        ProductNameService NameService = new ProductNameService(context);
                        ProdAddHistoryReportModel model = new ProdAddHistoryReportModel()
                        {
                            Productname = NameService.GetProductName(prod.ProductId),
                            Size = prod.Size,
                            Quantity = prod.Quantity,
                            Cost = prod.Cost,
                            Date = prod.Date,
                            Productid = prod.ProductId
                            
                        };
                        ReportModelList.Add(model);
                    }

                }
                else
                {
                    NotFound = true;
                }
            }
            if(NotFound)
            {
                return Ok(Result);
            }
            return Ok(ReportModelList);

        }

    }
}
