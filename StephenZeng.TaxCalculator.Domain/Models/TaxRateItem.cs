using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StephenZeng.TaxCalculator.Domain.Attributes;

namespace StephenZeng.TaxCalculator.Domain.Models
{
    public class TaxRateItem
    {
        [Required]
        public string Description { get; set; }

        [MustNotBeEmpty]
        public List<TaxThreshold> Thresholds { get; set; } 
    }
}