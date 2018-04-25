using System.Web;
using System.Web.Optimization;

namespace Wollo.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundle/js").Include(
                      //"~/js/modernizr-2.6.2.min.js",
                      //"~/js/jquery.min.js",
                      //"~/js/jquery-1.7.1.js",
                      //"~/js/bootstrap.min.js",
                      //"~/js/jquery.easing.1.3.js",
                      //"~/js/jquery.waypoints.min.js",
                      //"~/js/owl.carousel.min.js",
                      //"~/js/jquery.magnific-popup.min.js",
                      //"~/js/jquery.stellar.min.js",
                      //"~/js/jquery.countTo.js",
                      //"~/js/wow.min.js",
                      //"~/js/main.js",
                      //"~/js/jquery.js",
                      //"~/js/raphael.min.js",
                      //"~/js/morris.min.js",
                      //"~/js/morris-data.js",
                      //"~/js/bootstrap-datepicker.js"
            ));

            bundles.Add(new StyleBundle("~/bundle/css").Include(
                "~/css/animate.css",
                "~/css/icomoon.css",
                "~/css/magnific-popup.css",
                "~/css/owl.carousel.min.css",
                "~/css/owl.theme.default.min.css",
                "~/css/bootstrap.css",
                "~/css/cards.css",
                "~/css/bootstrap.min.css",
                "~/font-awesome/css/font-awesome.min.css",
                "~/css/plugins/morris.css"
            ));

            //BundleTable.EnableOptimizations = true;
        }
    }
}
