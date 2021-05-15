﻿using Ecommerce_NetCore_API.Models;
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
        public ActionResult<StockTE> GetStockPrice([FromForm] ProductWithCategoryIdsTE prod)
        {
            int ProdId = _context.products.Single(x => x.ProductName == prod.ProductName && x.CategoryId == prod.CategoryId).Id;
            StockTE stock = _context.stocks.SingleOrDefault(x => x.ProductId == ProdId && x.Size == prod.ProductSize);

            return Ok(stock);
        }

        [HttpPost("setstockprice")]
        public ActionResult<String> SetStockPrice([FromForm] ProductWithCategoryIdsTE prod)
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

        [HttpPost("getbilldata")]
        public ActionResult<CustwithOrder> GetBillData([FromForm] BillsPendingTE billsPending)
        {
            var BillData = _context.billspending.SingleOrDefault(x => x.Billnumber == billsPending.Billnumber);
            var CustData = _context.customers.SingleOrDefault(x => x.CustomerId == BillData.Customerid.ToString());

            CustwithOrder CustWithBillAmount = new CustwithOrder()
            {
                customer = CustData,
                Totalcost = BillData.Pendingamount
            };
            return Ok(CustWithBillAmount);
        }

        [HttpPost("storepayment")]
        public ActionResult<string> StorePayment([FromForm] CustomerTxHistoryTE bills)
        {
            string result = "";

            BillsPendingTE BillData = _context.billspending
                 .Single(x => x.Billnumber == bills.Billnumber && x.Customerid == bills.Customerid);
            if (BillData != null)
            {
                BillData.Pendingamount = BillData.Pendingamount - bills.Paidamount;

                if (BillData.Pendingamount == 0)
                    BillData.Iscompleted = true;

                _context.Entry(BillData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                string paymode = Convert.ToInt32(bills.Paymentmode) == 1 ? "Cash" : "Account";
                CustomerTxHistoryTE customer = new CustomerTxHistoryTE()
                {
                    Billnumber = bills.Billnumber,
                    Paidamount = bills.Paidamount,
                    Paiddate = DateTime.Now.Date,
                    Paymentmode = paymode,
                    Customerid = bills.Customerid
                };
                _context.Add(customer);
                _context.SaveChanges();
                result = "Done!";
            }
            else
            {
                result = "Please check your Bill Number!";
            }
            return Ok(result);
        }
           
           
        
    }
}
