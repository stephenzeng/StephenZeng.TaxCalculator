using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StephenZeng.TaxCalculator.Domain.Interfaces;

namespace StephenZeng.TaxCalculator.Domain
{
    public class Calculator : ICalculator
    {
        public Result Calculate(TaxRate taxRate, decimal income)
        {
            var result = new Result
            {
                Description = string.Format("Tax calculate result for income {0} in year {1} - {2}",
                    income.ToString("C"),
                    taxRate.Year,
                    taxRate.Year + 1),
                Items = new Collection<ResultItem>()
            };

            foreach (var item in taxRate.Items)
            {
                var taxItem = new ResultItem
                {
                    Name = item.Name,
                    Amount = CalculateTaxItem(item.Thresholds, income)
                };

                result.Items.Add(taxItem);
            }

            return result;
        }

        private static decimal CalculateTaxItem(IEnumerable<TaxThreshold> thresholds, decimal input)
        {
            var output = 0m;
            var temp = input;

            foreach (var rate in thresholds.Where(r => r.Start <= input).OrderByDescending(r => r.Start))
            {
                output += (temp - rate.Start) * rate.Rate;
                temp = rate.Start;
            }

            return output;
        }
    }
}