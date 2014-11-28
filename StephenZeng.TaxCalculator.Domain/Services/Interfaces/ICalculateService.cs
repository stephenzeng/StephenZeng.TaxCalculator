using StephenZeng.TaxCalculator.Domain.Models;

namespace StephenZeng.TaxCalculator.Domain.Services.Interfaces
{
    public interface ICalculateService
    {
        Result Calculate(TaxRate taxRate, decimal income);
    }
}