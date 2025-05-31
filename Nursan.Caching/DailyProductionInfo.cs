using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nursan.Caching
{
    public class DailyProductionInfo
    {
        public string HarnessModelName { get; set; }
        public int? TargetQuantity { get; set; }
        public int? CurrentQuantity { get; set; }
        public string StationName { get; set; }
        public string FamilyName { get; set; }
        public DateTime Date { get; set; }
    }
}
