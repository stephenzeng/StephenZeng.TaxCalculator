using System;

namespace StephenZeng.TaxCalculator.Domain
{
    public class InvalidTaxRateException : Exception
    {
        public InvalidTaxRateException(string message) : base(message)
        {
        }
    }
}