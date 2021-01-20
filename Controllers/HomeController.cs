using System;
using System.Collections.Generic;
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
        public ActionResult<String> AddNewCategory([FromForm]ProdAddCategoryTE fromData)
        {
            if (fromData.CategoryName != null)
            {
            ProdAddCategoryTE categoryName =  _context.categories.SingleOrDefault(x => x.CategoryName.ToLower() == fromData.CategoryName.ToLower());
            if(categoryName == null)
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
        public ActionResult<List<ProductWithCategoryIdsTE>> GetProductById([FromForm]ProductWithCategoryIdsTE data)
        {
            List<ProductWithCategoryIdsTE> products = _context.productWithCategoryIds.Where(x => x.CategoryId == data.Id).ToList<ProductWithCategoryIdsTE>();
            return Ok(products);
        }

        [HttpPost("addnewproduct")]
        public ActionResult<String> AddNewProduct([FromForm]ProductWithCategoryIdsTE productWithCategoryId)
        {
           ProductWithCategoryIdsTE product =  _context.productWithCategoryIds.SingleOrDefault(x => x.ProductName == productWithCategoryId.ProductName.ToLower());
           if(product == null)
            {
                _context.Add(productWithCategoryId);
                _context.SaveChanges();
            }
            else
            {
                return Ok("Name Exist Already");
            }
            return Ok("Saved Successfully!");
        }

        [HttpPost("register")]
        public ActionResult<string> ProductRegistration([FromForm] ProductEntryData formData)
        { 
            var prod = _context.products.SingleOrDefault(x => x.ProductName.ToLower() == formData.Name.ToLower() && x.CategoryId == Convert.ToInt16(formData.Catergory));
            if(prod != null)
            {
                ProdAddHistoryTE addprod = new ProdAddHistoryTE();
                addprod.Quantity = formData.Quantity;
                addprod.Cost = formData.Cost;
                addprod.Size = formData.Size;
                addprod.ProductId = prod.Id;

                _context.Add(addprod);
                _context.SaveChanges();


            }
            else
            {
                string UploadFolder = Path.Combine(_hostEnvironemnt.WebRootPath, "Images");
                string UniqueFileName = Guid.NewGuid().ToString() + "_" + formData.ImageFile.FileName;
                string FilePath = Path.Combine(UploadFolder, UniqueFileName);
                formData.ImageFile.CopyTo(new FileStream(FilePath, FileMode.Create));

                ProductTE product = new ProductTE();
                product.ProductName = formData.Name;
                product.ImagePath = UniqueFileName;

                _context.Add(product);
                _context.SaveChanges();

                var ProductObj = _context.products.Single(x => x.ImagePath == UniqueFileName);

                ProdAddHistoryTE prodAddHistory = new ProdAddHistoryTE();
                prodAddHistory.Quantity = formData.Quantity;
                prodAddHistory.Cost = formData.Cost;
                prodAddHistory.Size = formData.Size;
                prodAddHistory.ProductId = ProductObj.Id;

                _context.Add(prodAddHistory);
                _context.SaveChanges();

            }

            
            var prodDataToGByQtySize = _context.prodAddHistoryData.ToList();
            var prodGByQtySize =   prodDataToGByQtySize.GroupBy(x => new { x.ProductId, x.Size }).Select(x => new
            {
                ProductID = x.Key.ProductId,
                Size = x.Key.Size,
                TotalQuantity = x.Sum(x => x.Quantity),
                AvgCost = x.Sum(x => x.Cost) / x.Count()

            }).ToList();
         //   _context.Dispose();

            foreach (var product in prodGByQtySize)
            {

             var ExistingProduct = _context.stocks.SingleOrDefault(x => x.ProductId == product.ProductID && x.Size == product.Size);
                if (ExistingProduct != null)
                {
                    
                    ExistingProduct.Quantity = product.TotalQuantity;
                    ExistingProduct.Cost = product.AvgCost;
                    _context.Entry(ExistingProduct).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();
                }
                else
                {
                    StockTE stock = new StockTE();
                    stock.Quantity = product.TotalQuantity;
                    stock.Cost = product.AvgCost;
                    stock.Size = product.Size;
                    stock.ProductId = product.ProductID;
                    _context.Add(stock);
                    _context.SaveChanges();

                }


            }
        

            return Ok("Data Posted Successfully");

        }


         //Get List of Available Products to Display in the AddProduct Page
        [HttpGet("getstocks")]
        public ActionResult<List<ProductsInStock>> GetProdctsInStock()
        {
            List<ProductsInStock> productsInStocks = new List<ProductsInStock>();
            var products = _context.products.ToList();
            foreach(ProductTE prod in products)
            {
                ProductsInStock productsInStock = new ProductsInStock();
                productsInStock.Id = prod.Id;
                productsInStock.ProductName = prod.ProductName;
                productsInStock.ProductImage = prod.ImagePath;
                var stockBySize = _context.stocks.Where(x => x.ProductId == prod.Id).ToList();
                List<StockTE> stockTEs = new List<StockTE>();

                foreach(StockTE stock in stockBySize)
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

            return Ok(productsInStocks);
        }

        [HttpPost("pruchase")]
        public ActionResult<string> PurchaseStock(List<CartItems> cartItems) 
        {
            foreach(CartItems item in cartItems)
            {
             StockTE stockTE  = _context.stocks.Single(x => x.ProductId == item.id && x.Size == item.size);
             stockTE.Quantity = stockTE.Quantity - item.Quantity;
              _context.Entry(stockTE).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
              _context.SaveChanges();
            }

            return Ok("Successfulley finished tx");
        }

        [HttpPost("pdfdata")]
        public ActionResult<string> StorePDF([FromForm]PDFData formData)
        {
            string Base64 = formData.base64;
            byte[] byteArray = Convert.FromBase64String(Base64);
            BillDataTE billDataTE = new BillDataTE();
           //  billDataTE.Id = 1;
           billDataTE.BillNumber = 102;
            billDataTE.BillAmount = 145000;
            billDataTE.BillDate = DateTime.Now;
            billDataTE.BillByteArray = byteArray;
            billDataTE.CustomerId = 001;
            _context.Add(billDataTE);
            _context.SaveChanges();


          byte[] byteArray2  = _context.bills.Single(x => x.Id == 10).BillByteArray;
          string Base642 = Convert.ToBase64String(byteArray2);
            string test = "Git Test02";
            return Ok(Base642);
        }

    }
}
