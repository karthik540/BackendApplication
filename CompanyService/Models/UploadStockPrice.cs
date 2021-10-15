using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyService.Models
{
    public class UploadStockPrice
    {
        [JsonProperty("Company Code")]
        public string CompanyCode { get; set; }
        [JsonProperty("Stock Exchange")]
        public string StockExchange { get; set; }
        [JsonProperty("Price Per Share(in Rs)")]
        public string PricePerShare { get; set; }
        [JsonProperty("Date")]
        public string Date { get; set; }
        [JsonProperty("Time")]
        public string Time { get; set; }
    }
}
