using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NSubstitute;
using NUnit.Framework;
using Raven.Client;
using StephenZeng.TaxCalculator.Domain.Models;
using StephenZeng.TaxCalculator.Domain.Services.Interfaces;
using StephenZeng.TaxCalculator.Web.Controllers;
using StephenZeng.TaxCalculator.Web.Models;

namespace StephenZeng.TaxCalculator.Web.Test
{
    [TestFixture]
    public class the_calculator_controller
    {
        [Test]
        public void index_action_should_query_taxrates()
        {
            //arrange
            var calculateService = Substitute.For<ICalculateService>();
            var documentSession = Substitute.For<IDocumentSession>();
            documentSession.Query<TaxRate>().Returns(new FakeRavenQueryable<TaxRate>(Enumerable.Empty<TaxRate>().AsQueryable()));

            var controller = new CalculatorController(calculateService) {DocumentSession = documentSession};

            //act
            controller.Index();

            //assert
            documentSession.Received().Query<TaxRate>();
            Assert.IsInstanceOf<IEnumerable<SelectListItem>>(controller.ViewBag.TaxRatesList);
        }

        [Test]
        public void calculate_should_call_calculator_service()
        {
            //arrange
            var result = new Result();

            var calculateService = Substitute.For<ICalculateService>();
            calculateService.Calculate(null, 0).ReturnsForAnyArgs(c => result);

            var documentSession = Substitute.For<IDocumentSession>();
            documentSession.Query<TaxRate>().Returns(new FakeRavenQueryable<TaxRate>(Enumerable.Empty<TaxRate>().AsQueryable()));

            var controller = new CalculatorController(calculateService) { DocumentSession = documentSession };

            var model = new TaxCalculateViewModel
            {
                SelectedYear = 1,
                TaxableIncome = 1,
            };

            //act
            controller.Index(model);

            //assert
            calculateService.ReceivedWithAnyArgs().Calculate(null, 0);
            Assert.AreEqual(result, model.Result);
        }
    }
}
