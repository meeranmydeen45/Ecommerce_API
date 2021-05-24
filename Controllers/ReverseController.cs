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
        public ActionResult<string> ReverseEntry([FromForm] ReverseEntryDataTE data)
        {
            string result = "";
            int UnitPrice = 0;
            int ProfitDeduction = 0;
            int BillAmountDeduction = 0;
            List<ProdAddHistoryTE> products = _context.prodAddHistoryData
                 .Where(x => x.ProductId == data.Productid && x.Size == data.Size).ToList();
            if (products != null)
            { 
                int Total = 0;
            foreach (var prod in products)
            {
                Total += prod.Cost;
            }
            UnitPrice = Total / products.Count;
            ProfitDeduction = data.Quantity * data.Saleprice - data.Quantity * UnitPrice;
            BillAmountDeduction = data.Quantity * data.Saleprice;
              var stock = _context.stocks
                          .SingleOrDefault(x => x.ProductId == data.Productid && x.Size.ToUpper() == data.Size.ToUpper());
                if(stock != null)
                {
                    stock.Quantity += data.Quantity;
                    _context.Entry(stock).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();

                    //Modifying Bill Table
                   var BillData = _context.billscollections.Single(x => x.Billnumber == Convert.ToInt32(data.Billnumber));
                   if(BillData != null)
                    {
                        BillData.Billamount -= BillAmountDeduction;
                        BillData.Payableamount -= BillAmountDeduction;
                        BillData.Billprofit -= ProfitDeduction;
                        BillData.Isbillmodified = true;
                        _context.Entry(BillData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();

                    // Modifiying - Pending BillData Table
                       var PendingBillData = _context.billspending.Single(x => x.Billnumber == Convert.ToInt32(data.Billnumber));
                        int CustPreviousPendingBalance = 0;
                        if(PendingBillData.Pendingamount <= BillAmountDeduction)
                        {
                            CustPreviousPendingBalance = PendingBillData.Pendingamount;
                            PendingBillData.Pendingamount = 0;
                            PendingBillData.Iscompleted = true;
                            _context.Entry(PendingBillData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            _context.SaveChanges();
                        }
                        else
                        {
                            PendingBillData.Pendingamount -= BillAmountDeduction;

                            if (PendingBillData.Pendingamount <= 0)
                                PendingBillData.Iscompleted = true;

                            _context.Entry(PendingBillData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            _context.SaveChanges();
                        }
                        

                        //Adding Reverse History in DB
                         data.Date = DateTime.Now;
                        _context.Add(data);
                        _context.SaveChanges();

                        //Modifying Global Cash
                        bool isDataAvailable = _context.cashposition.Any();
                        if (isDataAvailable && PendingBillData.Pendingamount < BillAmountDeduction)
                        {
                            int Id = _context.cashposition.Max(x => x.Id);
                            var CashPositionData = _context.cashposition.Single(x => x.Id == Id);
                            int TotalDeduction = BillAmountDeduction - CustPreviousPendingBalance;
                            CashPositionData.Globalcash = CashPositionData.Globalcash - TotalDeduction; 
                            _context.Entry(CashPositionData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            _context.SaveChanges();

                        }
       
                        result = "Reverse Done!";
                    }
                    else
                    {
                        result = "Bill Not Found";
                    }
                }
                else
                {
                    result = "Stock Not Found";
                }
            }
            else
            {
                result = "Product Not Found";
            }

            return Ok(result);
        }
    }
}
