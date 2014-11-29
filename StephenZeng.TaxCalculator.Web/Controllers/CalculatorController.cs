using System.Linq;
using System.Web.Mvc;
using StephenZeng.TaxCalculator.Domain.Models;
using StephenZeng.TaxCalculator.Domain.Services.Interfaces;
using StephenZeng.TaxCalculator.Web.Models;

namespace StephenZeng.TaxCalculator.Web.Controllers
{
    public class CalculatorController : BaseController
    {
        private readonly ICalculateService _calculateService;

        public CalculatorController(ICalculateService calculateService)
        {
            _calculateService = calculateService;
        }

        public ActionResult Index()
        {
            var list = DocumentSession.Query<TaxRate>()
                .OrderByDescending(r => r.Year)
                .ToArray()
                .Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Id.ToString(),
                });

            ViewBag.TaxRatesList = list;

            return View();
        }

        [HttpPost]
        public ActionResult Index(TaxCalculateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taxRate = DocumentSession.Load<TaxRate>(model.SelectedYear);
                model.Result = _calculateService.Calculate(taxRate, model.TaxableIncome.Value);
            }
            else
            {
                ShowErrorMessage("Please input valid data");
            }

            var list = DocumentSession.Query<TaxRate>()
                .OrderByDescending(r => r.Year)
                .ToArray()
                .Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Id.ToString(),
                });

            ViewBag.TaxRatesList = list;

            return View(model);
        }
    }
}
