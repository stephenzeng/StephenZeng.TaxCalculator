using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using AutoMapper;
using StephenZeng.TaxCalculator.Domain.Models;

namespace StephenZeng.TaxCalculator.Web.Controllers.Api
{
    public class TaxRateController : BaseApiController
    {
        public TaxRateController()
        {
            Mapper.CreateMap<TaxRate, TaxRate>()
                .ForMember(s => s.Description, o => o.MapFrom(c => string.Format("Copy - {0}", c.Description)))
                .ForMember(s => s.Id, o => o.Ignore())
                .ForMember(s => s.CreateAt, o => o.MapFrom(c => DateTime.Now));
        }

        public IEnumerable<TaxRate> GetAll()
        {
            return DocumentSession.Query<TaxRate>()
                .OrderByDescending(r => r.Year);
        }

        public IHttpActionResult Get(int id)
        {
            var taxRate = DocumentSession.Load<TaxRate>(id);

            if (taxRate == null) return NotFound();

            return Ok(taxRate);
        }
    }
}