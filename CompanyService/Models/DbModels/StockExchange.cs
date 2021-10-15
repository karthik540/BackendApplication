using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CompanyService.Models
{
    public class StockExchange
    {
        public int Id { get; set; }

        public string stockExchange { get; set; }

        public string brief { get; set; }

        public string contactAddress { get; set; }

        public string remarks { get; set; }

        [JsonIgnore]
        public List<CompanyStockExchange> CompanyList { get; set; }
    }
}
