using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyService.Models
{
    public class AddCompanyModel
    {
        public int id { get; set; }
        public string company { get; set; }

        public string turnover { get; set; }

        public string ceo { get; set; }

        public string boardOfDirectors { get; set; }

        public int[] stockExchanges { get; set; }

        public int sector { get; set; }

        public string[] stockCodes { get; set; }
    }
}
