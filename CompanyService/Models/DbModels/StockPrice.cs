using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyService.Models
{
    public class StockPrice
    {
        public int companyStockExchangecompanyId { get; set; }
        public int companyStockExchangestockExchangeId { get; set; }
        public CompanyStockExchange companyStockExchange { get; set; }

        public decimal price { get; set; }

        public DateTime dateTime { get; set; }
    }
}
