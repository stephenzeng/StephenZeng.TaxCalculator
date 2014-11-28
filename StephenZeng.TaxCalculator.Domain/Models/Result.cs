using System.Collections.Generic;
using System.Linq;

namespace StephenZeng.TaxCalculator.Domain.Models
{
    public class Result
    {
        public string Description { get; set; }
        public decimal TaxableIncome { get; set; }
        public ICollection<ResultItem> Items { get; set; }

        public decimal TotalAmount
        {
            get { return Items.Sum(i => i.Amount); }
        }
    }
}