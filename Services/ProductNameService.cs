using Ecommerce_NetCore_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Services
{
    public class ProductNameService
    {
        private readonly Context context;
        public ProductNameService(Context _context)
        {
            context = _context;

        }
        public string GetProductName(int ProductId)
        {
           return  context.products.Single(x => x.Id == ProductId).ProductName;
        }

    }
}
