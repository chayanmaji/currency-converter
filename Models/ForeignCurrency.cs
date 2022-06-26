using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.Models
{
    public class ForeignCurrencyRate
    {
        public string CurrencyCode { get; set; }
        public double Rate { get; set; }
        public DateTime ExchangeDate { get; set; }
    }
}
