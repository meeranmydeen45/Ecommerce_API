using Ecommerce_NetCore_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Services
{
    public class CustomerNameService
    {
        private readonly Context context;
      public CustomerNameService(Context _context)
        {
            this.context = _context;
        }

        public string GetCustomerName(int CustomerId)
        {
          string Name = context.customers.Single(x => x.CustomerId == CustomerId.ToString()).CustomerName;
          return Name;
        }
    }
}
