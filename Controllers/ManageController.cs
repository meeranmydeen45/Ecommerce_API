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
            CustwithOrder CustWithBillAmount = new CustwithOrder();
            var BillData = _context.billspending.SingleOrDefault(x => x.Billnumber == billsPending.Billnumber);
            if (BillData != null)
            {
                var CustData = _context.customers.SingleOrDefault(x => x.CustomerId == BillData.Customerid.ToString());

                CustWithBillAmount.customer = CustData;
                CustWithBillAmount.Totalcost = BillData.Pendingamount;
            }
            else
            {
                CustWithBillAmount.customer = null;
                CustWithBillAmount.Totalcost = 0;
            }
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
                if (bills.Paymentmode == "ACCOUNT")
                {
                    var CustAccountData = _context.customeraccounts.Single(x => Convert.ToInt32(x.Customerid) == bills.Customerid);
                    if (CustAccountData != null)
                    {
                        CustAccountData.Availableamount -= bills.Paidamount;
                        _context.Entry(CustAccountData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();

                    }
                    else
                    {
                        result = "Customer Account Data Not Found - Server Side";
                    }

                }
                else if (bills.Paymentmode == "CASH")
                {
                    bool isDataAvailable = _context.cashposition.Any();
                    if (isDataAvailable)
                    {
                        int Id = _context.cashposition.Max(x => x.Id);
                        var CashPositionData = _context.cashposition.Single(x => x.Id == Id);
                        CashPositionData.Globalcash += bills.Paidamount;
                        _context.Entry(CashPositionData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();

                    }
                    else
                    {
                        CashPositionTE cashPosition = new CashPositionTE()
                        { Globalcash = bills.Paidamount };
                        _context.Add(cashPosition);
                        _context.SaveChanges();

                    }

                }
                BillData.Pendingamount = BillData.Pendingamount - bills.Paidamount;

                if (BillData.Pendingamount == 0)
                    BillData.Iscompleted = true;

                _context.Entry(BillData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

                CustomerTxHistoryTE customer = new CustomerTxHistoryTE()
                {
                    Billnumber = bills.Billnumber,
                    Paidamount = bills.Paidamount,
                    Paiddate = DateTime.Now.Date,
                    Paymentmode = bills.Paymentmode,
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

        [HttpPost("addsizes")]
        public ActionResult<string> AddSizes([FromForm] SizesTE data)
        {
            string result = "";
            data.Size = data.Size.Trim().ToUpper();
            bool isAny = _context.Sizes.Any(x => x.Size == data.Size);
            if (!isAny)
            {
                _context.Add(data);
                _context.SaveChanges();
                result = "Done!";
            }
            else
            {
                result = "Available Already!";
            }


            return Ok(result);
        }

        [HttpGet("getsizes")]
        public ActionResult<List<SizesTE>> GetSizes()
        {
            var Sizes = _context.Sizes.ToList();
            return Ok(Sizes);

        }

        [HttpPost("getcustomerbyid")]
        public ActionResult<CustomerTE> GetCustomerById([FromForm] CustomerTE data)
        {
            string result = "";
            var Customer = _context.customers.SingleOrDefault(x => x.CustomerId == data.CustomerId);
            if (Customer != null)
            {
                return Ok(Customer);
            }
            else
            {
                result = "Customer Not Found!";
                return Ok(result);
            }

        }

        [HttpPost("createcustomeraccount")]
        public ActionResult<String> CreateCustomerAccount([FromForm] CustomerAccountTE data)
        {
            string result = "";
            //Fitst Time - Needs to check Any Data Available in CustomerAccount Table
            bool isDataAvailable = _context.customeraccounts.Any();
            if (isDataAvailable)
            {
                var isCustAvailable = _context.customeraccounts.SingleOrDefault(x => x.Customerid == data.Customerid);
                if (isCustAvailable == null)
                {
                    _context.Add(data);
                    _context.SaveChanges();
                    result = "Account Created Successfully";

                }
                else
                {
                    result = "Account Available Already!";
                }

            }
            else
            {
                //It will Run Just ontime for creating Table
                _context.Add(data);
                _context.SaveChanges();
                result = "Account Created Successfully";

            }
            return Ok(result);
        }

        [HttpPost("getcustomeraccount")]
        public ActionResult<CustomerAccountTE> GetCustomerAccount([FromForm] CustomerAccountTE data)
        {
            var CustomerAccountData = _context.customeraccounts.SingleOrDefault(x => x.Customerid == data.Customerid);
            return Ok(CustomerAccountData);
        }

        [HttpGet("getallpendingbills")]
        public ActionResult<List<PendingBills>> GetAllPendingBills()
        {
            List<PendingBills> PendingBillswithName = new List<PendingBills>();
            var isAnyData = _context.billspending.Any();
            if (isAnyData)
            {
                var PendingBills = _context.billspending.Where(x => x.Iscompleted == false).ToList();
                foreach (BillsPendingTE bill in PendingBills)
                {
                    var Customer = _context.customers.Single(x => x.CustomerId == bill.Customerid.ToString());
                    PendingBills PendingBill = new PendingBills()
                    {
                        Billnumber = bill.Billnumber.ToString(),
                        Pendingamount = bill.Pendingamount,
                        Customerid = Customer.CustomerId,
                        Customermobile = Customer.customermobile,
                        Customername = Customer.CustomerName
                    };
                    PendingBillswithName.Add(PendingBill);
                }
            }
            return Ok(PendingBillswithName);

        }

        [HttpPost("getpendingbillsbycustomer")]
        public ActionResult<List<PendingBills>> GetPendingBillByCustomerValue([FromForm] PendingBills data)
        {

            string TextBoxValue = data.TextBoxValue.Trim();
            string SearchCriteria = data.SearchCriteria;
            string Result = "";
            List<PendingBills> PendingBillsWithName = new List<PendingBills>();
            CustomerTE Customer = new CustomerTE();
            List<BillsPendingTE> PendingBills = new List<BillsPendingTE>();
            var isAnyData = _context.billspending.Any();
            if (isAnyData)
            {

                if (SearchCriteria != "CUSTOMERID")
                {
                    if (SearchCriteria == "CUSTOMERMOBILE")
                    {
                        Customer = _context.customers.SingleOrDefault(x => x.customermobile == TextBoxValue);
                        if (Customer != null)
                        {
                            PendingBills = _context.billspending.
                                   Where(x => x.Customerid.ToString() == Customer.CustomerId && x.Iscompleted == false).ToList();
                            if (PendingBills == null)
                            {
                                Result = "No Pending Bill Available for" + Customer.CustomerName;
                            }
                        }
                        else
                        {
                            Result = "Match Not Found with Mobile";
                        }
                    }
                    else if (SearchCriteria == "CUSTOMERNAME")
                    {

                        Customer = _context.customers.SingleOrDefault(x => x.CustomerName.ToUpper() == TextBoxValue.ToUpper());
                        if (Customer != null)
                        {
                            PendingBills = _context.billspending.
                                   Where(x => x.Customerid.ToString() == Customer.CustomerId && x.Iscompleted == false).ToList();
                            if (PendingBills == null)
                            {
                                Result = "No Pending Bill Available for" + Customer.CustomerName;
                            }
                        }
                        else
                        {
                            Result = "Not Found with Name";
                        }

                    }
                }
                else if (SearchCriteria == "CUSTOMERID")
                {
                    Customer = _context.customers.SingleOrDefault(x => x.CustomerId == TextBoxValue);
                    if (Customer != null)
                    {
                        PendingBills = _context.billspending.Where(x => x.Customerid.ToString() == TextBoxValue && x.Iscompleted == false).ToList();
                        if (PendingBills == null)
                        {
                            Result = "No Pending Bill Available for" + Customer.CustomerName;
                        }
                    }
                    else
                    {
                        Result = "Not Found with ID";
                    }


                }
            }
            // Iterating throug all Pending Bills and adding Customer Name and Mobile
            if (PendingBills.Count > 0)
            {
                foreach (var billData in PendingBills)
                {
                    PendingBills bill = new PendingBills()
                    {
                        Billnumber = billData.Billnumber.ToString(),
                        Pendingamount = billData.Pendingamount,
                        Customerid = billData.Customerid.ToString(),
                        Customermobile = Customer.customermobile.ToString(),
                        Customername = Customer.CustomerName
                    };
                    PendingBillsWithName.Add(bill);
                }
                return Ok(PendingBillsWithName);
            }
            else
            {
                PendingBillsWithName = null;
                return Ok(Result);
            }


        }

        [HttpPost("getpdfbillbyno")]
        public ActionResult<string> GetPDFByBillNumber([FromForm] PendingBills data)
        {
            var BillData = _context.billscollections.SingleOrDefault(x => x.Billnumber.ToString() == data.Billnumber);
            if (BillData != null)
            {
                byte[] ByteArray = BillData.Billbytearray;
                string Base64 = Convert.ToBase64String(ByteArray);
                return Ok(Base64);
            }
            else
            {
                string result = string.Empty;
                return Ok(result);
            }
        }

        [HttpGet("getglobalcash")]
        public ActionResult<int> GetGlobalCash()
        {
            int Cashposition = 0;
            bool isDataAvailable = _context.cashposition.Any();
            if (isDataAvailable)
            {
                int Id = _context.cashposition.Max(x => x.Id);
                var CashPositionData = _context.cashposition.Single(x => x.Id == Id);
                Cashposition = CashPositionData.Globalcash;

            }
            else
            {
                //It will Run Just One Time in Entire Application
                CashPositionTE cashPosition = new CashPositionTE()
                { Globalcash = 0 };
                _context.Add(cashPosition);
                _context.SaveChanges();
                Cashposition = 0;

            }
            return Ok(Cashposition);
        }

        [HttpGet("getaccountdetails")]
        public ActionResult<CustomerAccountTE> GetCustomerAccount(string SearchValue)
        {
            string Searchvalue = SearchValue.Trim();
            string Result = "";
            CustomerAccountTE CustomerAccountInfo = new CustomerAccountTE();
            CustomerAccountInfo = _context.customeraccounts.SingleOrDefault(x => x.Customerid == Searchvalue);
            if (CustomerAccountInfo == null)
            {
                var CustomerData = _context.customers.SingleOrDefault(x => x.customermobile == Searchvalue);
                if (CustomerData != null)
                {
                    CustomerAccountInfo = _context.customeraccounts.SingleOrDefault(x => x.Customerid == CustomerData.CustomerId);
                    if (CustomerAccountInfo != null)
                    {
                        return Ok(CustomerAccountInfo);
                    }
                    else
                    {
                        Result = "Customer Dont Have Account Credit";
                        return Ok(Result);
                    }

                }
                else
                {

                    Result = "Mobile or CustID doesn't match with any Customer";
                    return Ok(Result);
                }
            }
            else
            {
                return Ok(CustomerAccountInfo);
            }

        }

        [HttpPost("credittocustaccount")]
        public ActionResult<String> SetCreditToCustomerAccount([FromForm] CustomerAccountTE data)
        {
            string Result = "";
            var CustomerAccount = _context.customeraccounts.SingleOrDefault(x => x.Customerid == data.Customerid);
            if (CustomerAccount != null)
            {
                CustomerAccount.Availableamount += data.Availableamount;
                _context.Entry(CustomerAccount).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

                int MaxId = _context.cashposition.Max(x => x.Id);
                var CashPosition = _context.cashposition.Single(x => x.Id == MaxId);
                CashPosition.Globalcash += data.Availableamount;
                _context.Entry(CashPosition).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                Result = "Credit Successfully";

            }
            else
            {
                Result = "Failed";

            }
            return Ok(Result);
        }


        [HttpPost("globalcashtx")]
        public ActionResult<string>  GlobalCashCreditandDebitHandling(int amount, string type)
        {
            bool isAnyData = _context.cashposition.Any();
            string Result = "";
            if (isAnyData)
            {
               int id = _context.cashposition.Max(x => x.Id);
               var CashData = _context.cashposition.Single(x => x.Id == id);
               if(type == "CREDIT")
                {
                    CashData.Globalcash += amount;
                    _context.Entry(CashData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                     Result = "Credit Sucessfully";
                }
               else if(type == "DEBIT")
                {
                    CashData.Globalcash -= amount;
                    _context.Entry(CashData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    Result = "Debit Sucessfully";
                }
            }
            else
            {
                Result = "Failed";
            }
            return Ok(Result);
        }

        [HttpGet("modifiedbills")]
        public ActionResult<List<BillsDatasTE>> ModifiedBills()
        {
            string Result = "";
           var ModifiedBillsData = _context.billscollections.Where(x => x.Isbillmodified == true);
            List<BillsDatasTE> ModifiedBills = new List<BillsDatasTE>();
           if(ModifiedBillsData != null)
            {
                foreach(var BillData in ModifiedBillsData)
                {
                    BillsDatasTE bill = new BillsDatasTE()
                    {
                        Billnumber = BillData.Billnumber,
                        Customerid = BillData.Customerid
                    };
                    ModifiedBills.Add(bill);
                }
                return Ok(ModifiedBills);
            }
            else
            {
                Result = "No Pending Bills Availble yet!";
                return Ok(Result);

            }
        }
        
    }
}
