using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.Models.CanadaBank
{
    public class CurrencyRate
    {
        public Terms Terms { get; set; }
        public List<Observation> Observations { get; set; }
    }

    public class Observation {
        public string d { get; set; }
        public Currency Currency { get; set; }
    }

    public class Currency {
        public decimal v { get; set; }
    }



}
