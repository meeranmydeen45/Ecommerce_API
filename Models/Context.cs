using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Models
{
    public class Context : DbContext
    {

        public Context(DbContextOptions<Context> opt) : base(opt)
        {

        }

        public DbSet<LoginDataTE> loginDatas { get; set; }
        public DbSet<ProductTE> products { get; set; }
        public DbSet<ProdAddCategoryTE> categories { get; set; }
        public DbSet<ProductWithCategoryIdsTE> productWithCategoryIds { get; set; }
        public DbSet<ProdAddHistoryTE> prodAddHistoryData { get; set; }
        public DbSet<CustomerTE> customers { get; set; }
        public DbSet<SalewithCustIdTE> saleswithCustomerIds { get; set; }
        public DbSet<StockTE> stocks { get; set; }
        public DbSet<BillDataTE> bills { get; set; }

        


    }
}
