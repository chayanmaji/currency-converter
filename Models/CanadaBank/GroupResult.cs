using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.Models.CanadaBank
{
    public class GroupResult
    {
        public Terms Terms { get; set; }
        public GroupDetails GroupDetails { get; set; }
    }



    public class GroupDetails
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public Dictionary<string, Group> GroupSeries { get; set; }
    }

    public class Group
    {
        public string Label { get; set; }
        public string Link { get; set; }
    }
}
