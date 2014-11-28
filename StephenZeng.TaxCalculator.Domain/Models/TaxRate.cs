using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using StephenZeng.TaxCalculator.Domain.Attributes;

namespace StephenZeng.TaxCalculator.Domain.Models
{
    public class TaxRate : IValidatableObject
    {
        public int Year { get; set; }

        [Required]
        public string Description { get; set; }

        [MustNotBeEmpty]
        public ICollection<TaxRateItem> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var item in Items)
            {
                var validatonContext = new ValidationContext(item, null, null);
                var validationResults = new Collection<ValidationResult>();
                if (!Validator.TryValidateObject(item, validatonContext, validationResults))
                {
                    foreach (var result in validationResults)
                    {
                        yield return result;
                    }
                }

                if (item.Thresholds.IsNullOrEmpty()) continue;
                
                var orderedList = item.Thresholds.OrderBy(t => t.Start);

                if (orderedList.First().Start != 0)
                    yield return new ValidationResult("Tax rate threshold must start from 0.");

                if (orderedList.Last().End.HasValue)
                    yield return new ValidationResult("The last tax rate threshold must end with empty.");
            }
        }
    }
}
