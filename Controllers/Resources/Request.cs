using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.Controllers.Resources
{
    public class Request
    {
        [Required]
        public string SourceCurrency { get; set; }
        [Required]
        public string TargetCurrency { get; set; }
        public string Date { get; set; }
    }
}
