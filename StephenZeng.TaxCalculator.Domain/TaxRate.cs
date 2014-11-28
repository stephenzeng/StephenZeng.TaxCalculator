using System.Collections.Generic;
using System.Linq;

namespace StephenZeng.TaxCalculator.Domain
{
    public class TaxRate
    {
        public int Year { get; set; }
        public string Description { get; set; }
        public ICollection<TaxRateItem> Items { get; set; }

        public void ValidateItems()
        {
            if (Items.IsNullOrEmpty())
                throw new InvalidTaxRateException("Tax rate item must not be empty");

            foreach (var item in Items)
            {
                if (item.Thresholds.IsNullOrEmpty())
                    throw new InvalidTaxRateException("Tax rate item threshold must not be empty");

                var orderedList = item.Thresholds.OrderBy(t => t.Start);

                if (orderedList.First().Start != 0)
                    throw new InvalidTaxRateException("Tax rate threshold must start from 0");

                if (orderedList.Last().End.HasValue)
                    throw new InvalidTaxRateException("The last tax rate threshold must end in empty");
            }
        }
    }
}
