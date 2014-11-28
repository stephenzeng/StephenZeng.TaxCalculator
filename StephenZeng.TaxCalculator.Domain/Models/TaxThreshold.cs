namespace StephenZeng.TaxCalculator.Domain.Models
{
    public class TaxThreshold
    {
        public decimal Start { get; set; }
        public decimal? End { get; set; }
        public decimal Rate { get; set; }
    }
}