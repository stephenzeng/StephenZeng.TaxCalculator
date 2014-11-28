using System.Collections.Generic;
using System.Linq;

namespace StephenZeng.TaxCalculator.Domain
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }
    }
}