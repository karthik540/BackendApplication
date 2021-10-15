using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeService.Data
{
    public class DbInit
    {
        public static void InitializeWithFakeData(AppDbContext context)
        {
            context.Database.EnsureCreated();

            context.SaveChanges();
        }
    }
}
