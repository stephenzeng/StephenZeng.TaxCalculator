using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using AutoMapper;
using StephenZeng.TaxCalculator.Domain.Models;

namespace StephenZeng.TaxCalculator.Web.Controllers
{
    public class TaxRateController : BaseController
    {
        public TaxRateController()
        {
            Mapper.CreateMap<TaxRate, TaxRate>()
                .ForMember(s => s.Description, o => o.MapFrom(c => string.Format("Copy - {0}", c.Description)))
                .ForMember(s => s.Id, o => o.Ignore())
                .ForMember(s => s.CreateAt, o => o.MapFrom(c => DateTime.Now));
        }

        public ActionResult Index()
        {
            var list = DocumentSession.Query<TaxRate>()
                .OrderByDescending(r => r.Year)
                .ToArray();

            return View(list);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var taxRate = new TaxRate
            {
                Items = new List<TaxRateItem>
                {
                    new TaxRateItem()
                    {
                        Thresholds = new List<TaxThreshold>
                        {
                            new TaxThreshold(),
                            new TaxThreshold(),
                            new TaxThreshold(),
                        }
                    }
                }
            };

            return View(taxRate);
        }

        [HttpPost]
        public ActionResult Add(TaxRate taxRate)
        {
            try
            {
                DocumentSession.Store(taxRate);
                taxRate.CreateAt = DateTime.Now;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var taxRate = DocumentSession.Load<TaxRate>(id);
            return View(taxRate);
        }

        [HttpPost]
        public ActionResult Edit(TaxRate taxRate)
        {
            try
            {
                DocumentSession.Store(taxRate);
                ShowInfoMessage("Tax rate saved successfully");
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }

            return View(taxRate);
        }

        [HttpPost]
        public HttpResponseMessage Delete(int id)
        {
            var config = DocumentSession.Load<TaxRate>(id);
            DocumentSession.Delete(config);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public int Copy(int id)
        {
            var taxRate = DocumentSession.Load<TaxRate>(id);
            var copy = Mapper.Map<TaxRate>(taxRate);
            copy.CreateAt = DateTime.Now;

            DocumentSession.Store(copy);

            return copy.Id;
        }
    }
}
