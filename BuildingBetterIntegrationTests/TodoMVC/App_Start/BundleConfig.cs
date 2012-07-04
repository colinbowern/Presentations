using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace TodoMVC.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Site")
                .Include("~/Scripts/jquery-1.*")
                .Include("~/Scripts/kendo/2012.1.322/kendo.core.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Styles/Site").Include("~/Styles/Site.css"));
        }
    }
}