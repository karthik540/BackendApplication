using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyService.Models
{
    public class CompanyList : Company
    {
        public string sectorName { get; set; }

        public List<StockExchange> stockExchanges { get; set; }
        public List<string> stockCodes { get; set; }
    }
}
