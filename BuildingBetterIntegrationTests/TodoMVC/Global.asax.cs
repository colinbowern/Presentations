using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NHibernate;
using TodoMVC.App_Start;
using TodoMVC.Persistence;

namespace TodoMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DatabaseConfig.CheckDatabaseConnectivity();
            DatabaseConfig.MigrateSchema();
        }

        #region IoC-less NHibernate
        protected static SessionProvider provider = new SessionProvider();

        protected void Application_BeginRequest()
        {
            HttpContext.Current.Items["Session"] = provider.CreateInstance();
        }

        protected void Application_EndRequest()
        {
            SessionProvider.DisposeInstance((ISession)HttpContext.Current.Items["Session"]);
        }
        #endregion
    }
}