
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_NetCore_API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Ecommerce_NetCore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnvironemnt;
        private readonly Context _context;

        public HomeController(IWebHostEnvironment hostEnvironment, Context context)
        {
            _hostEnvironemnt = hostEnvironment;
            _context = context;
        }

        [HttpGet("Get")]
        public ActionResult<String> GetAll()
        {

            return Ok("Debugger Checking");


        }

        [HttpGet("getcategory")]
        public ActionResult<List<ProdAddCategoryTE>> GetCategories()
        {
            List<ProdAddCategoryTE> categories = _context.categories.ToList();
            return Ok(categories);
        }

        [HttpPost("addcategory")]
        public ActionResult<String> AddNewCategory([FromForm] ProdAddCategoryTE fromData)
        {
            if (fromData.CategoryName != null)
            {
                ProdAddCategoryTE categoryName = _context.categories.SingleOrDefault(x => x.CategoryName.ToLower() == fromData.CategoryName.ToLower());
                if (categoryName == null)
                {
                    _context.Add(fromData);
                    _context.SaveChanges();
                }
                else
                {
                    return Ok("Exist Already!");
                }
            }
            else
            {
                return Ok("Please provide Valid Data!");
            }
            return Ok("Data Saved Successfully!");

        }

        [HttpPost("getproductbyid")]
        public ActionResult<List<ProductTE>> GetProductById([FromForm] ProductWithCategoryIdsTE data)
        {
            List<ProductTE> products = _context.products.Where(x => x.CategoryId == data.Id).ToList<ProductTE>();
            return Ok(products);
        }

        [HttpPost("addnewproduct")]
        public ActionResult<String> AddNewProduct([FromForm] ProductWithCategoryIdsTE productData)
        {
            ProductTE product = _context.products.SingleOrDefault(x => x.ProductName.ToLower() == productData.ProductName.ToLower());
            if (product == null)
            {
                string UploadFolder = Path.Combine(_hostEnvironemnt.WebRootPath, "Images");
                string UniqueName = Guid.NewGuid().ToString() + "_" + productData.Imagefile.FileName;
                string FilePath = Path.Combine(UploadFolder, UniqueName);
                productData.Imagefile.CopyTo(new FileStream(FilePath, FileMode.Create));

                ProductTE prod = new ProductTE() {
                    ProductName = productData.ProductName,
                    ImagePath = UniqueName,  
                    CategoryId = productData.CategoryId
                };
                _context.Add(prod);
                _context.SaveChanges();
            }
            else
            {
                string UploadFolder = Path.Combine(_hostEnvironemnt.WebRootPath, "Images");
                string UniqueName = Guid.NewGuid().ToString() + "_" + productData.Imagefile;
                string FilePath = Path.Combine(UploadFolder, UniqueName);
                productData.Imagefile.CopyTo(new FileStream(FilePath, FileMode.Create));

                product.ImagePath = UniqueName;
                _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            }
            return Ok("Saved Successfully!");
        }

        [HttpPost("newuser-registration")]
        public ActionResult<string> UserRegistration([FromForm] LoginDataTE loginData)
        {
            if (loginData.username != null & loginData.password != null)
            {
                _context.Add(loginData);
                _context.SaveChanges();
                return Ok("Registration Done successfully");

            }
            else
            {
                return Ok("Please check the details!!");
            }


        }

        [HttpPost("uservalidation")]
        public ActionResult<LoginDataTE> UserValidation([FromForm] LoginDataTE loginData)
        {
            var userObject = _context.loginDatas.SingleOrDefault(x => x.username.ToLower() == loginData.username.ToLower().Trim() && x.password == loginData.password.ToLower().Trim());
            if (userObject != null)
            {
                return Ok(userObject);
            }
            else
            {
                return Ok("Incorrect Details!!");
            }
        }


        [HttpPost("register")]
        public ActionResult<string> ProductRegistration([FromForm] ProductEntryData formData)
        {
                int PurchaseCost = 0;
                 
                var ProductObj = _context.products.Single(x => x.ProductName == formData.Name);

                ProdAddHistoryTE prodAddHistory = new ProdAddHistoryTE();
                prodAddHistory.Quantity = formData.Quantity;
                prodAddHistory.Cost = formData.Cost;
                prodAddHistory.Size = formData.Size;
                prodAddHistory.Totalcost = formData.Quantity * formData.Cost;
                prodAddHistory.Date = DateTime.Now.Date;
                prodAddHistory.ProductId = ProductObj.Id;

                _context.Add(prodAddHistory);
                _context.SaveChanges();

            //Make Purchase impacting the Global Cash
            PurchaseCost = formData.Quantity * formData.Cost;
            bool isDataAvailable = _context.cashposition.Any();
            if (isDataAvailable)
            {
                int Id = _context.cashposition.Max(x => x.Id);
                var CashPositionData = _context.cashposition.Single(x => x.Id == Id);
                CashPositionData.Globalcash -= PurchaseCost;
                _context.Entry(CashPositionData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

            }
            else
            {
                CashPositionTE cashPosition = new CashPositionTE()
                { Globalcash = PurchaseCost };
                _context.Add(cashPosition);
                _context.SaveChanges();

            }

            //Make Effect in Stock Table once Product Registered

            var IsAnyStock = _context.stocks.Any();
            if(IsAnyStock)
            {
              var StockObj =  _context.stocks.SingleOrDefault(x => x.ProductId == prodAddHistory.ProductId && x.Size == prodAddHistory.Size);
              if(StockObj != null)
                {
                    StockObj.Cost = formData.Cost + 500;
                    StockObj.Quantity += formData.Quantity;
                    _context.Entry(StockObj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                }
              else
                {
                    StockTE stock = new StockTE()
                    {
                        Quantity = formData.Quantity,
                        Size = formData.Size,
                        Cost = formData.Cost,
                        ProductId = prodAddHistory.ProductId
                    };
                    _context.Add(stock);
                    _context.SaveChanges();
                }
              
            }
            else
            {
                //One Time Execution to Create Stock Table with data
                StockTE stock = new StockTE()
                {
                    Quantity = formData.Quantity,
                    Size = formData.Size,
                    Cost = formData.Cost,
                    ProductId = prodAddHistory.ProductId
                };
                _context.Add(stock);
                _context.SaveChanges();

            }
         return Ok("Data Posted Successfully");

        }


        //Get List of Available Products to Display in the AddProduct Page
        [HttpGet("getstocks")]
        public ActionResult<List<ProductsInStock>> GetProdctsInStock()
        {
            List<ProductsInStock> productsInStocks = new List<ProductsInStock>();
            var products = _context.products.ToList();
            foreach (ProductTE prod in products)
            {
                ProductsInStock productsInStock = new ProductsInStock();
                productsInStock.Id = prod.Id;
                productsInStock.ProductName = prod.ProductName;
                productsInStock.ProductImage = prod.ImagePath;
                var stockBySize = _context.stocks.Where(x => x.ProductId == prod.Id).ToList();
                if(stockBySize.Count > 0) {

                    List<StockTE> stockTEs = new List<StockTE>();

                    foreach (StockTE stock in stockBySize)
                    {
                        StockTE stockTE = new StockTE();
                        stockTE.Id = stock.Id;
                        stockTE.Quantity = stock.Quantity;
                        stockTE.Size = stock.Size;
                        stockTE.Cost = stock.Cost;
                        stockTE.ProductId = stock.ProductId;

                        stockTEs.Add(stockTE);
                    }

                    productsInStock.listOfstocksBySize = stockTEs;
                    productsInStocks.Add(productsInStock);

                }
               
            }

            return Ok(productsInStocks);
        }


        [HttpPost("is-cutomer-available")]
        public ActionResult<string> IsCustomerAvailabe([FromForm] CustomerTE customer)
        {
            var isExistingCustomer = _context.customers.SingleOrDefault(x => x.customermobile == customer.customermobile);
            if (isExistingCustomer != null)
            {
                return Ok(isExistingCustomer);
            }
            else
            {
                return Ok("Mobile No.Not Registered");
            }

        }

        [HttpPost("customer-registration")]
        public ActionResult<string> CustomerRegistration([FromForm] CustomerTE customer, bool update)
        {
            string Result = "";
            //If Want to Update Customer Mobile and Address
            if (update)
            {
                var CustomerDataForUpdate = _context.customers.Single(x => x.CustomerId == customer.CustomerId);
                if (CustomerDataForUpdate != null)
                {
                    CustomerDataForUpdate.customermobile = customer.customermobile;
                    CustomerDataForUpdate.Customeraddress = customer.Customeraddress;
                    _context.Entry(CustomerDataForUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                    Result = "Update Successfully";
                }
            }
            else
            {
                bool isAnyData = _context.customers.Any();
                if (isAnyData)
                {
                    int MaxIdValue = _context.customers.Max(x => x.Id);
                    CustomerTE CustwithMaxId = _context.customers.Single(x => x.Id == MaxIdValue);
                    CustomerTE IsExistingCust = _context.customers.SingleOrDefault(x => x.customermobile == customer.customermobile);
                    if(IsExistingCust == null)
                    {
                        //If will get Last row CustId and Increase it By 1 and update CustId with New Customer
                        customer.CustomerId = (Convert.ToInt32(CustwithMaxId.CustomerId) + 1).ToString();
                        _context.Add(customer);
                        _context.SaveChanges();
                        Result = "Customer Registered Successfully";

                    }
                    else
                    {
                        Result = "Mobile No Registered with Mr. " + IsExistingCust.CustomerName;
                    }
                }
                else
                {
                    //First time for creation Customer Table
                    int custId = 10100001;
                    customer.CustomerId = custId.ToString();
                    _context.Add(customer);
                    _context.SaveChanges();
                    Result = "Congratulation 1 Customer Registered Successfully";

                }
            }
            return Ok(Result);
        }

        [HttpPost("pruchase")]
        public  ActionResult<string> PurchaseStock(CustwithOrder custwithOrder) 
        {
            foreach (CartItems item in custwithOrder.cartItems)
            {
                SalewithCustIdTE salewithCustId = new SalewithCustIdTE
                {
                    Productname = item.productName,
                    Prodsize = item.size,
                    Quantity = item.Quantity,
                    Unitprice = item.cost,
                    TotalCost = item.Quantity * item.cost,
                    Purchasedate = DateTime.Now.Date,
                    Productid = item.id,
                    Custid = custwithOrder.customer.CustomerId

                };
                _context.Add(salewithCustId);
                _context.SaveChanges();
            }

            foreach (CartItems item in custwithOrder.cartItems)
            {
                


                StockTE stockTE = _context.stocks.Single(x => x.ProductId == item.id && x.Size == item.size);
                stockTE.Quantity = stockTE.Quantity - item.Quantity;
                _context.Entry(stockTE).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            
             }

            //To Calculate Bill profit
            List<Profitmodel> Datas = new List<Profitmodel>();
            int Totalprofit = 0;
            foreach (CartItems item in custwithOrder.cartItems)
            {
                List<Profitmodel> data = new List<Profitmodel>();
                data = _context.prodAddHistoryData.Where(x => x.ProductId == item.id && x.Size == item.size)
                    .GroupBy(x => new { x.ProductId, x.Size })
                        .Select(s => new Profitmodel()
                        {
                            Productid = s.Key.ProductId,
                            Size = s.Key.Size,
                            Profit = item.cost - s.Sum(x => x.Cost) / s.Count(),
                            Quantity = item.Quantity

                        }).ToList<Profitmodel>();
                Datas.Add(data[0]);
            }
            foreach(var data in Datas)
            {
                Totalprofit = Totalprofit +  (data.Profit * data.Quantity);

            }
            

            
            //Generate Bill No and Passing Complete Purchased Object back to FrontEnd for PDF generation
           int Billno;
           bool Notempty = _context.billscollections.Any();
           if(Notempty)
           {
               int MaxIdValue = _context.billscollections.Max(x => x.Id);
                int LastBillNo = _context.billscollections.Single(x => x.Id == MaxIdValue).Billnumber;
                Billno = LastBillNo + 1;

            }
            else
            {
                Billno = 801000;
            }
            CustObjwithBillNo custObjwithBillNo = new CustObjwithBillNo
            {
                Custwithorder = custwithOrder,
                Billnumber = Billno,
                Billprofit = Totalprofit
            };


           
            
            return Ok(custObjwithBillNo);
        }

        [HttpPost("pdfdata")]
        public ActionResult<string> StorePDF([FromForm]PDFData formData)
        {
            string Base64 = formData.Base64;    
            byte[] byteArray = Convert.FromBase64String(Base64);

            //Calculating Bill profit after dedcution of Discount %

            int Accurateprofit = formData.Billprofit - formData.Deduction;

            BillsDatasTE Billdata = new BillsDatasTE()
            {
                Billnumber = formData.Billnumber,
                Billamount = formData.Billamount,
                Deduction = formData.Deduction,
                Payableamount = formData.Payableamount,
                Billprofit = Accurateprofit,
                Billbytearray = byteArray,
                Billdate = DateTime.Now,
                Ispaid = false,
                Customerid = formData.Customerid
            };
            _context.Add(Billdata);
            _context.SaveChanges();

            BillsPendingTE billPending = new BillsPendingTE()
            {
                Pendingamount = formData.Payableamount,
                Billnumber = formData.Billnumber,
                Iscompleted = false,
                Customerid = formData.Customerid,
            };
            _context.Add(billPending);
            _context.SaveChanges();

           int maxID = _context.billscollections.Max(x => x.Id);
           byte[] byteArray2  = _context.billscollections.Single(x => x.Id == maxID).Billbytearray;
           string Base642 = Convert.ToBase64String(byteArray);

            return Ok(Base642);

        }

    }
}
