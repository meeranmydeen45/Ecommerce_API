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

        public int CalculatingProductProfitForReversalProduct(int ProductId, string Size, int BillNumber, int ReverseQuantity, int SoldPrice)
        {
            var BillObject = context.billscollections.Single(x => x.Billnumber == BillNumber);
            
            var ProdSoldList = GetAllSoldProducts(ProductId, Size);
            var ProdPurchasedList = GetAllPurchasedProducts(ProductId, Size);
            int TotalSaleCost = 0;
            //int TotalNoOfSoldItemsPeroid = 0;
            int TotalPurchaseCost = 0;
            int TotalNoOfPurchaseItemsInTx = 0;

            int ProdSoldFromInitialtoReversalProdDate = ProdSoldList.Where(x => x.Purchasedate <= BillObject.Billdate).Sum(x => x.Quantity);

            TotalSaleCost = ReverseQuantity * SoldPrice;

            /*foreach (var prodSold in ProdSoldFromInitialtoReversalProdDate)
            {
                TotalNoOfSoldItemsPeroid += prodSold.Quantity;
                TotalSaleCost = TotalSaleCost + (prodSold.Quantity * prodSold.Unitprice);

            }*/


            int currentQuantity = 0;
            int NeedsToSkipItems = ProdSoldFromInitialtoReversalProdDate - ReverseQuantity;
            int count = 0;
            foreach (var prodPurchase in ProdPurchasedList)
            {
                TotalNoOfPurchaseItemsInTx += prodPurchase.Quantity;


                if (TotalNoOfPurchaseItemsInTx > NeedsToSkipItems)
                {
                    if (count == 0)
                    {
                        // int skipProdValue = SkipItems - (TotalNoOfPurchaseItemsInTx - prodPurchase.Quantity);
                        currentQuantity = TotalNoOfPurchaseItemsInTx - NeedsToSkipItems;

                        if (currentQuantity < ReverseQuantity)
                        {
                            TotalPurchaseCost += currentQuantity * prodPurchase.Cost;
                            //currentQuantity += (prodPurchase.Quantity - skipProdValue);
                            count = 1;
                        }
                        else
                        {
                            return TotalPurchaseCost = ReverseQuantity * prodPurchase.Cost;

                        }
                    }
                    else
                    {
                        currentQuantity = currentQuantity + prodPurchase.Quantity;
                        if (currentQuantity < ReverseQuantity)
                        {
                            TotalPurchaseCost += prodPurchase.Quantity * prodPurchase.Cost;
                        }
                        else if (currentQuantity >= ReverseQuantity)
                        {
                            currentQuantity = ReverseQuantity - (currentQuantity - prodPurchase.Quantity);

                            return TotalPurchaseCost = currentQuantity * prodPurchase.Cost;
                        }

                    }
                }

            }


            return TotalSaleCost - TotalPurchaseCost;
        }

        public int CalcuatingProductProfitBasedDateValue(int ProductId, string Size, DateTime FromDate, DateTime EndDate)
        {
            var ProdSoldList = GetAllSoldProducts(ProductId, Size);
            var ProdPurchasedList = GetAllPurchasedProducts(ProductId, Size);
            int TotalSaleCost = 0;
            int TotalNoOfSoldItemsPeroid = 0;
            int TotalPurchaseCost = 0;
            int TotalNoOfPurchaseItemsInTx = 0;

            int ProdSoldFromInitialtoChoosenEndDate = ProdSoldList.Where(x => x.Purchasedate <= EndDate).Sum(x => x.Quantity);
            var ProdSoldChoosenFromDateToEndDate = ProdSoldList.Where(x => x.Purchasedate >= FromDate && x.Purchasedate <= EndDate).ToList();


            foreach (var prodSold in ProdSoldChoosenFromDateToEndDate)
            {
                TotalNoOfSoldItemsPeroid += prodSold.Quantity;
                TotalSaleCost = TotalSaleCost + (prodSold.Quantity * prodSold.Unitprice);

            }
            int currentQuantity = 0;
            int NeedsToSkipItems = ProdSoldFromInitialtoChoosenEndDate - TotalNoOfSoldItemsPeroid;
            int count = 0;
            foreach (var prodPurchase in ProdPurchasedList)
            {
                TotalNoOfPurchaseItemsInTx += prodPurchase.Quantity;

                
                if (TotalNoOfPurchaseItemsInTx > NeedsToSkipItems)
                { 
                    if(count == 0) { 
                   // int skipProdValue = SkipItems - (TotalNoOfPurchaseItemsInTx - prodPurchase.Quantity);
                    currentQuantity = TotalNoOfPurchaseItemsInTx - NeedsToSkipItems;
                     
                    if(currentQuantity < TotalNoOfSoldItemsPeroid)
                    { 
                    TotalPurchaseCost += currentQuantity * prodPurchase.Cost;
                    //currentQuantity += (prodPurchase.Quantity - skipProdValue);
                    count = 1;
                    }
                    else
                    {
                    return TotalPurchaseCost = TotalNoOfSoldItemsPeroid * prodPurchase.Cost;

                    }
                    }
                    else
                    {
                        currentQuantity = currentQuantity + prodPurchase.Quantity;
                        if (currentQuantity < TotalNoOfSoldItemsPeroid)
                        {
                            TotalPurchaseCost += prodPurchase.Quantity * prodPurchase.Cost;
                        }
                        else if(currentQuantity >= TotalNoOfSoldItemsPeroid)
                        {
                            currentQuantity = TotalNoOfSoldItemsPeroid - (currentQuantity - prodPurchase.Quantity);
                             
                           return TotalPurchaseCost = currentQuantity * prodPurchase.Cost;
                        }

                    }
                }
                
            }


            return TotalSaleCost - TotalPurchaseCost;


            
        }

    }
}
