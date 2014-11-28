namespace StephenZeng.TaxCalculator.Domain.Interfaces
{
    public interface ICalculator
    {
        Result Calculate(TaxRate taxRate, decimal income);
    }
}