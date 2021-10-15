using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CompanyService.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string company { get; set; }

        public string turnover { get; set; }

        public string ceo { get; set; }

        public string boardOfDirectors { get; set; }

        public int sectorId { get; set; }

        public Sector sector { get; set; }

        [JsonIgnore]
        public List<CompanyStockExchange> StockExchangeList { get; set; }
    }
}
