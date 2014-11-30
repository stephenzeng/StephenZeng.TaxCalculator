using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Raven.Client;
using Raven.Client.Embedded;
using StephenZeng.TaxCalculator.Domain.Services.Interfaces;
using StephenZeng.TaxCalculator.Web.Controllers;

namespace StephenZeng.TaxCalculator.Web
{
    public class MvcApplication : HttpApplication
    {
        public static IDocumentStore DocumentStore { get; private set; }

        public MvcApplication()
        {
            BeginRequest += (sender, args) =>
            {
                HttpContext.Current.Items["CurrentRequestRavenSession"] = BaseController.DocumentStore.OpenSession();
            };

            EndRequest += (sender, args) =>
            {
                using (var session = (IDocumentSession)HttpContext.Current.Items["CurrentRequestRavenSession"])
                {
                    if (session == null)
                        return;

                    if (Server.GetLastError() != null)
                        return;

                    session.SaveChanges();
                }
            };
        }

        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitializeDocumentStore();
            BaseController.DocumentStore = DocumentStore;

            DatabaseConfig.Seed(DocumentStore);
            SetupDependencyInjection();
        }

        private static void InitializeDocumentStore()
        {
            if (DocumentStore != null) return;

            DocumentStore = new EmbeddableDocumentStore()
            {
                RunInMemory = true
            }.Initialize();
        }

        private static void SetupDependencyInjection()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyTypes(typeof (ICalculateService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerRequest();
            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
