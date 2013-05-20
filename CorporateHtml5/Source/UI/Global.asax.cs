using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace CorporateHtml5
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapHubs();
        }
    }
}