using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeService.Models
{
    public class AddStockExchangeModel
    {
        public string stockExchange { get; set; }

        public string brief { get; set; }

        public string contactAddress { get; set; }

        public string remarks { get; set; }
    }
}
