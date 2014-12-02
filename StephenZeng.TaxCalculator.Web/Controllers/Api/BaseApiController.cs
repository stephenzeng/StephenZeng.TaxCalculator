using System.Web.Http;
using System.Web.Http.Controllers;
using Raven.Client;

namespace StephenZeng.TaxCalculator.Web.Controllers.Api
{
    public abstract class BaseApiController : ApiController
    {
        protected IDocumentSession DocumentSession { get; private set; }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            if (DocumentSession == null) DocumentSession = MvcApplication.DocumentStore.OpenSession();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            using (DocumentSession)
            {
                if (DocumentSession != null)
                    DocumentSession.SaveChanges();
            }
        }
    }
}