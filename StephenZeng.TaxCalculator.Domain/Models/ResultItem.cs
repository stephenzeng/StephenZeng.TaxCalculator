using System.ComponentModel.DataAnnotations;

namespace StephenZeng.TaxCalculator.Domain.Models
{
    public class ResultItem
    {
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}