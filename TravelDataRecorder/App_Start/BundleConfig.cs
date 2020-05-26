using System.Web;
using System.Web.Optimization;

namespace TravelDataRecorder
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/assets/js/vendor.bundle.base.js",
                      "~/assets/js/vendor.bundle.addons.js",
                      "~/assets/js/off-canvas.js",
                      "~/assets/js/misc.js",
                      "~/assets/js/chart.js",
                       "~/Scripts/Travel.js",
                      "~/assets/js/dashboard.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/assets/css/style.css",
                      "~/assets/css/responsive.css",
                      "~/assets/fonts/iconfonts/mdi/css/materialdesignicons.min.css",
                      "~/assets/fonts/iconfonts/mdi/css/materialdesignicons.css"));

            bundles.Add(new ScriptBundle("~/bundles/fullCalendarjs").Include(
                    "~/Scripts/moment.min.js",
                     "~/Scripts/fullcalendar.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/DataTablejs").Include(
                 "~/Scripts/jquery.dataTables.min.js"));

            bundles.Add(new StyleBundle("~/Content/datetimepickercss").Include(
                    "~/assets/css/Datetimepicker/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/datetimepickerjs").Include(
           "~/assets/js/Datetimepicker/jquery-1.12.4.js",
           "~/assets/js/Datetimepicker/jquery-1.12.1-ui.js"));

            bundles.Add(new StyleBundle("~/Content/fullCalendarcss").Include(
                  "~/assets/css/fullcalendar.min.css"));

            bundles.Add(new StyleBundle("~/Content/DataTablecss").Include(
                 "~/assets/css/datatables.css"));


        }

    }
}
