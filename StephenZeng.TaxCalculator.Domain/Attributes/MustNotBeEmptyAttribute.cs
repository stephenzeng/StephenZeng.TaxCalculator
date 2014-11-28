using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace StephenZeng.TaxCalculator.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MustNotBeEmptyAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IEnumerable;
            return list != null && list.GetEnumerator().MoveNext();
        }
    }
}