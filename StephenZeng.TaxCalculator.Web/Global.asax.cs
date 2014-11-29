﻿using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Raven.Client;
using Raven.Client.Document;
using StephenZeng.TaxCalculator.Web.Controllers;

namespace StephenZeng.TaxCalculator.Web
{
    public class MvcApplication : System.Web.HttpApplication
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
        }

        private static void InitializeDocumentStore()
        {
            if (DocumentStore != null) return;

            DocumentStore = new DocumentStore()
            {
                ConnectionStringName = "RavenDB"
            }.Initialize();
        }
    }
}
