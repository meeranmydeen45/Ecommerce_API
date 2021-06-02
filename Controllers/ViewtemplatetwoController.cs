using Ecommerce_NetCore_API.Models;
using Ecommerce_NetCore_API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewtemplatetwoController : Controller
    {

        private readonly Context context;

        public ViewtemplatetwoController(Context _context)
        {
            this.context = _context;
        }

        [HttpGet("getcustomerbymobile")]
        public ActionResult<CustomerTE> GetCustomerByMobileNumber(string MobileNumber)
        {

            var customer = context.customers.SingleOrDefault(x => x.customermobile == MobileNumber);
            if (customer != null)
            {
                return Ok(customer);

            }
            else
            {
                return Ok("Not Found with Mobile Number");
            }
        }

        [HttpPost("getmonthprofitandcustomer")]
        public ActionResult<List<ProdMonthCustomerProfitReportModel>> GetStoreProfit([FromForm] ViewTemplateModelTwo data)
        {
            bool IsFromDateValid = false;
            bool IsEndDateValid = false;
            DateTime FromDate;
            string CompleteFromDateString = "";
            DateTime EndDate;
            string CompleteEndDateString = "";
            DateTime dt;


            List<ProdMonthCustomerProfitReportModel> ReportModelList = new List<ProdMonthCustomerProfitReportModel>();

            string Result = "";
            bool NotFound = false;


            if (data.FromDate != null && data.EndDate != null)
            {

                DateService DateService1 = new DateService();
                CompleteFromDateString = DateService1.GetCompleteFromDate(data.FromDate);
                IsFromDateValid = DateTime.TryParse(CompleteFromDateString, out dt);

                DateService DateService2 = new DateService();
                CompleteEndDateString = DateService2.GetCompleteEndDate(data.EndDate);
                IsEndDateValid = DateTime.TryParse(CompleteEndDateString, out dt);

                if (IsFromDateValid && IsEndDateValid)
                {
                    FromDate = Convert.ToDateTime(CompleteFromDateString);
                    EndDate = Convert.ToDateTime(CompleteEndDateString);
                    CustomerNameService nameService = new CustomerNameService(context);
                    if (data.MobileNumber == null)
                    {

                        var BillCollectionData = context.billscollections.Where(x => x.Billdate >= FromDate && x.Billdate <= EndDate)
                          .Select(x => new ProdMonthCustomerProfitReportModel()
                          {
                              Billnumber = x.Billnumber.ToString(),
                              Customername = nameService.GetCustomerName(x.Customerid),
                              Profit = x.Billprofit,
                              Date = x.Billdate.ToString()
                          }).ToList<ProdMonthCustomerProfitReportModel>();
                        if (BillCollectionData.Count() > 0)
                        {
                            ReportModelList = BillCollectionData;
                        }
                        else
                        {
                            NotFound = true;
                            Result = "Not Found";
                        }
                    }
                    else
                    {
                        var BillCollectionData = context.billscollections.Where(x => x.Customerid.ToString() == data.Customerid
                         && x.Billdate >= FromDate && x.Billdate <= EndDate)
                             .Select(x => new ProdMonthCustomerProfitReportModel()
                             {
                                 Billnumber = x.Billnumber.ToString(),
                                 Customername = nameService.GetCustomerName(x.Customerid),
                                 Profit = x.Billprofit,
                                 Date = x.Billdate.ToString()

                             }).ToList<ProdMonthCustomerProfitReportModel>();

                        if (BillCollectionData.Count() > 0)
                        {
                            ReportModelList = BillCollectionData;
                        }
                        else
                        {
                            NotFound = true;
                            Result = "Not Found";
                        }

                    }
                }
            }
            if (NotFound)
            {
                return Ok(Result);
            }
            else
            {
                return Ok(ReportModelList);
            }

        }

        [HttpGet("customeraccountdetails")]
        public ActionResult<List<CustomerAccountTE>> GetCustomerAccountDetails(string CustomerId)
        {
            string Result = "";
            bool NotFound = false;
            List<CustomerAccountTE> AccountDataList = new List<CustomerAccountTE>();
            if (CustomerId != null)
            {
                var AccountData = context.customeraccounts.SingleOrDefault(x => x.Customerid == CustomerId);
                if (AccountData != null)
                {
                    AccountDataList.Add(AccountData);
                }
                else
                {
                    NotFound = true;
                    Result = "No Records Found";
                }

            }
            else
            {
                var AccountData = context.customeraccounts;

                if (AccountData != null)
                {
                    AccountDataList = AccountData.ToList();
                }
                else
                {
                    NotFound = true;
                    Result = "No Records Found";
                }
            }
            if (NotFound)
            {
                return Ok(Result);
            }
            else
            {
                return Ok(AccountDataList);
            }
        }

        [HttpGet("customertxhistory")]
        public ActionResult<List<CustomerTxHistoryReportModel>> GetCustomerTransactionHistory(string CustomerId)
        {

            List<CustomerTxHistoryTE> TxHistoryDataList = new List<CustomerTxHistoryTE>();
            List<CustomerTxHistoryReportModel> modelList = new List<CustomerTxHistoryReportModel>();

            TxHistoryDataList = context.customertxhistory.Where(x => x.Customerid.ToString() == CustomerId).ToList();

            if (TxHistoryDataList.Count() > 0)
            {
                CustomerNameService nameService = new CustomerNameService(context);
                foreach(var data in TxHistoryDataList)
                {
                    CustomerTxHistoryReportModel model = new CustomerTxHistoryReportModel();
                    model.Customername = nameService.GetCustomerName(data.Customerid);
                    model.Billnumber = data.Billnumber;
                    model.Paidamount = data.Paidamount;
                    model.Paymentmode = data.Paymentmode;
                    model.Paiddate = data.Paiddate;
                    modelList.Add(model);
                }
                
                return Ok(modelList);
            }
            else
            {
                return Ok("No Records Found");
            }
        }


        [HttpGet("getreversehistoryforbill")]
        public ActionResult<List<ReverseHistoryReportModel>> GetReverseHistoryofBill(string BillNo)
        {
          var ReverseData =  context.reverseentrydata.Where(x => x.Billnumber == BillNo);
          List<ReverseHistoryReportModel> modelList = new List<ReverseHistoryReportModel>();
          if(ReverseData != null)
            {
                ProductNameService nameService = new ProductNameService(context);
                foreach(var data in ReverseData)
                {
                    ReverseHistoryReportModel model = new ReverseHistoryReportModel();
                    model.Productname = nameService.GetProductName(data.Productid);
                    model.Size = data.Size;
                    model.Quantity = data.Quantity;
                    model.Saleprice = data.Saleprice;
                    model.Date = data.Date;
                    modelList.Add(model);
                }
                
                return Ok(modelList);
            }
            else
            {
                return Ok("No Records Found");
            }


        }




    }
}
