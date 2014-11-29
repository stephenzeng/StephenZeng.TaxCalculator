using System.Collections.Generic;
using Raven.Abstractions.Extensions;
using Raven.Client;
using StephenZeng.TaxCalculator.Domain.Models;

namespace StephenZeng.TaxCalculator.Web
{
    public class DatabaseConfig
    {
        public static void Seed(IDocumentStore documentStore)
        {
            var list = new[]
            {
                GetTaxRate2011(),
                GetTaxRate2014()
            };

            using (var session = documentStore.OpenSession())
            {
                list.ForEach(session.Store);
                session.SaveChanges();
            }
        }

        private static TaxRate GetTaxRate2014()
        {
            const int year = 2014;

            var taxRate = new TaxRate
            {
                Id = year,
                Year = year,
                Description = string.Format("Tax rates in year {0} - {1}", year, year + 1)
            };

            taxRate.Items = new List<TaxRateItem>
            {
                new TaxRateItem
                {
                    Description = "Personal income tax rates",
                    Thresholds = new List<TaxThreshold>
                    {
                        new TaxThreshold {Start = 0m, End = 18200m, Rate = 0m},
                        new TaxThreshold {Start = 18200m, End = 37000m, Rate = 0.19m},
                        new TaxThreshold {Start = 37000m, End = 80000m, Rate = 0.325m},
                        new TaxThreshold {Start = 80000m, End = 180000m, Rate = 0.37m},
                        new TaxThreshold {Start = 180000m, End = null, Rate = 0.45m},
                    }
                },
                new TaxRateItem
                {
                    Description = "Medicare levy rates",
                    Thresholds = new List<TaxThreshold>
                    {
                        new TaxThreshold {Start = 0m, End = 20542m, Rate = 0m},
                        new TaxThreshold {Start = 20542m, End = 24167m, Rate = 0.1m},
                        new TaxThreshold {Start = 24167m, Rate = 0.02m},
                    }
                },
                new TaxRateItem
                {
                    Description = "Budget repair levy rates",
                    Thresholds = new List<TaxThreshold>
                    {
                        new TaxThreshold {Start = 0m, End = 180000m, Rate = 0m},
                        new TaxThreshold {Start = 180000m, Rate = 0.02m},
                    },
                }
            };

            return taxRate;
        }

        private static TaxRate GetTaxRate2011()
        {
            const int year = 2011;

            var taxRate = new TaxRate
            {
                Id = year,
                Year = year,
                Description = string.Format("Tax rates in year {0} - {1}", year, year + 1)
            };

            taxRate.Items = new List<TaxRateItem>
            {
                new TaxRateItem
                {
                    Description = "Personal income tax rates",
                    Thresholds = new List<TaxThreshold>
                    {
                        new TaxThreshold {Start = 0m, End = 6000m, Rate = 0m},
                        new TaxThreshold {Start = 6000m, End = 37000m, Rate = 0.15m},
                        new TaxThreshold {Start = 37000m, End = 80000m, Rate = 0.3m},
                        new TaxThreshold {Start = 80000m, End = 180000m, Rate = 0.37m},
                        new TaxThreshold {Start = 180000m, Rate = 0.45m},
                    }
                },
                new TaxRateItem
                {
                    Description = "Medicare levy rates",
                    Thresholds = new List<TaxThreshold>
                    {
                        new TaxThreshold {Start = 0m, End = 19404m, Rate = 0m},
                        new TaxThreshold {Start = 19404m, End = 22828m, Rate = 0.1m},
                        new TaxThreshold {Start = 22828m, Rate = 0.015m},
                    }
                },
                new TaxRateItem
                {
                    Description = "Flood levy rates",
                    Thresholds = new List<TaxThreshold>
                    {
                        new TaxThreshold {Start = 0m,End = 50000m, Rate = 0m},
                        new TaxThreshold {Start = 50000m, End = 100000m, Rate = 0.005m},
                        new TaxThreshold {Start = 100000m, Rate = 0.01m},
                    }
                }
            };

            return taxRate;
        }
    }
}