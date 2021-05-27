using Ecommerce_NetCore_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Services
{
    public class ProductProfitService
    {
        private readonly Context context;

        public ProductProfitService(Context _context)
        {
            context = _context;
        }
        public List<ProdAddHistoryTE> GetAllPurchasedProducts(int ProductId, string Size)
        {
         var ProductUniquePurchasedList = context.prodAddHistoryData.Where(x => x.ProductId == ProductId && x.Size == Size).ToList();
            return ProductUniquePurchasedList;
        }

        public List<SalewithCustIdTE> GetAllSoldProducts(int ProductId, string Size)
        {
            var ProductUniqueSoldList = context.saleswithCustomerIds.Where(x => x.Productid == ProductId && x.Prodsize == Size).ToList();
            return ProductUniqueSoldList;
        }

        public int GetProductProfit(int ProductId, string Size)
        {
          var ProdSoldList =  GetAllSoldProducts(ProductId, Size);
          var ProdPurchasedList = GetAllPurchasedProducts(ProductId, Size);
            int TotalSaleCost = 0;
            int TotalNoOfSoldItems = 0;
            int TotalPurchaseCost = 0;
            int TotalNoOfPurchaseItemsInTx = 0;

            foreach(var prodSold in ProdSoldList)
            {
                TotalNoOfSoldItems += prodSold.Quantity;
                TotalSaleCost = TotalSaleCost + (prodSold.Quantity * prodSold.Unitprice);

            }
            int prevQuantity = 0;
            foreach (var prodPurchase in ProdPurchasedList)
            {
                TotalNoOfPurchaseItemsInTx += prodPurchase.Quantity;
                
                if(TotalNoOfPurchaseItemsInTx < TotalNoOfSoldItems)
                {
                    TotalPurchaseCost += prodPurchase.Quantity * prodPurchase.Cost;
                    prevQuantity += prodPurchase.Quantity;
                }
                else if(TotalNoOfPurchaseItemsInTx > TotalNoOfSoldItems)
                {
                    int QuantityDiff = TotalNoOfSoldItems - prevQuantity;
                    TotalPurchaseCost += QuantityDiff * prodPurchase.Cost;
                }
            }




            return TotalSaleCost - TotalPurchaseCost;
        }
    }
}
