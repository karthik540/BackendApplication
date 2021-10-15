using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyService.Models
{
    public class IPODetail
    {
        public int Id { get; set; }

        public int companyId { get; set; }
        public Company company { get; set; }

        public int stockExchangeId { get; set; }
        public StockExchange stockExchange { get; set; }

        public decimal pricePerShare { get; set; }

        public int totalShares { get; set; }

        public DateTime openDate { get; set; }


    }
}
