﻿using System.Web.Mvc;

namespace StephenZeng.TaxCalculator.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}