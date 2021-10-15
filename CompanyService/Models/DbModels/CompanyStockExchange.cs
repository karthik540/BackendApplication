using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyService.Models
{
    public class CompanyStockExchange
    {
        public int companyId { get; set; }
        public Company company { get; set; }

        public int stockExchangeId { get; set; }
        public StockExchange stockExchange { get; set; }

        public string stockCode { get; set; }
    }
}
