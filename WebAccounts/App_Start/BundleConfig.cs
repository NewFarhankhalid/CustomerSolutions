using System.Web;
using System.Web.Optimization;

namespace Installments
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquerylib").Include(
                        "~/Scripts/jquery-3.3.1.js",
                        "~/bower_components/jquery-ui/jquery-ui.min.js",
                        "~/bower_components/bootstrap/dist/js/bootstrap.min.js",
                       "~/bower_components/raphael/raphael.min.js",
                        "~/bower_components/morris.js/morris.min.js", //for charts
                     //   "~/bower_components/jquery-sparkline/dist/jquery.sparkline.min.js",
                        "~/Scripts/bootbox.js",
                        "~/Scripts/Respond.js",
                        //"~/bower_components/jquery-knob/dist/jquery.knob.min.js",
                        "~/bower_components/moment/min/moment.min.js",
                        "~/bower_components/bootstrap-daterangepicker/daterangepicker.js",
                        "~/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",
                        "~/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                        "~/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                        "~/bower_components/fastclick/lib/fastclick.js",
                        "~/dist/js/adminlte.min.js",
                        "~/Vendor/datatables/jquery.dataTables.js",
                        "~/Vendor/datatables/dataTables.bootstrap4.js",
                        "~/bower_components/select2/dist/js/select2.full.min.js",
                        "~/plugins/input-mask/jquery.inputmask.js",
                        "~/plugins/input-mask/jquery.inputmask.date.extensions.js",
                        "~/plugins/input-mask/jquery.inputmask.extensions.js",
                        //"~/bower_components/bootstrap-colorpicker/dist/js/bootstrap-colorpicker.min.js",
                        "~/plugins/timepicker/bootstrap-timepicker.min.js", "~/Scripts/Selectize.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                               "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new StyleBundle("~/Content/css").Include(

                      "~/Content/site.css", "~/Vendor/datatables/dataTables.bootstrap4.css"));
        }
    }
}
