using System.Web;
using System.Web.Optimization;

namespace TasksApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/modernizr").Include(
            "~/Scripts/modernizr-2.6.2-dev.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Layout").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/kendo/2013.1.319/kendo.web.js",
                        "~/Scripts/kendo/kendo.bindings.custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/Styles/Layout").Include("~/Styles/Layout.css"));

            bundles.Add(new StyleBundle("~/Styles/LayoutIE6").Include("~/Styles/Layout.IE6.css"));

            bundles.Add(new StyleBundle("~/Styles/jQueryUI").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}