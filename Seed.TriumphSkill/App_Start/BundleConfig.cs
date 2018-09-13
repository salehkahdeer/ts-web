using System.Web;
using System.Web.Optimization;

namespace Seed.TriumphSkill
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
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

            bundles.Add(new ScriptBundle("~/Scripts/Bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Angular").Include(
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-sanitize.js",
                        "~/Scripts/Sortable.js",
                        "~/Scripts/angular-ui/ui-bootstrap.js",
                        "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                        "~/Scripts/angular-dashboard-framework.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/dashboard-widgets/adf-widget-clock.js",
                        "~/Scripts/dashboard-widgets/adf-widget-linklist.js",
                        "~/Scripts/dashboard-widgets/adf-widget-number.js",
                        "~/Scripts/dashboard-widgets/showdown.js",
                        "~/Scripts/dashboard-widgets/markdown.js",
                        "~/Scripts/dashboard-widgets/adf-widget-markdown.js"));

            bundles.Add(new ScriptBundle("~/Scripts/DataTables").Include(
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/dataTables.bootstrap.js",
                        "~/Scripts/DataTables/fnSetFilteringDelay.js"));

            bundles.Add(new StyleBundle("~/Content/Bootstrap").Include(
                "~/Content/bootstrap.css", 
                "~/Content/DataTables/dataTables.bootstrap.css",
                "~/Content/signin.css"));

            bundles.Add(new StyleBundle("~/Content/Dashboard").Include(
                "~/Content/angular-dashboard-framework.css",
                "~/Content/dashboard-widgets/adf-widget-clock.css",
                "~/Content/dashboard-widgets/adf-widget-number.css"));
        }
    }
}