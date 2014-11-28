using System.Collections.Generic;

namespace StephenZeng.TaxCalculator.Domain
{
    public class TaxRateItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<TaxThreshold> Thresholds { get; set; } 
    }
}