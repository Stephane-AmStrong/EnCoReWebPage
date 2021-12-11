using System.Web;
using System.Web.Optimization;

namespace EnCoReWebPage
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //blog
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/EnCoRe/jquery.js",
                "~/Scripts/EnCoRe/popper.min.js",
                "~/Scripts/EnCoRe/bootstrap.min.js",
                "~/Scripts/EnCoRe/jquery-ui.js",
                "~/Scripts/EnCoRe/jquery.fancybox.js",
                "~/Scripts/EnCoRe/validate.js",
                "~/Scripts/EnCoRe/isotope.js",
                "~/Scripts/EnCoRe/owl.js",
                "~/Scripts/EnCoRe/mixitup.js",
                "~/Scripts/EnCoRe/mixitup-loadmore.js",
                "~/Scripts/EnCoRe/scrollbar.js",
                "~/Scripts/EnCoRe/appear.js",
                "~/Scripts/EnCoRe/wow.js",
                "~/Scripts/EnCoRe/custom-script.js",
                "~/Scripts/EnCoRe/map-script.js"));

            //Admin LTE
            bundles.Add(new ScriptBundle ("~/jQuery/js").Include(
                "~/Scripts/AdminLTE/jquery/jquery.min.js",
                "~/Scripts/AdminLTE/jquery-ui/jquery-ui.min.js",

                // Bootstrap 4
                "~/Scripts/AdminLTE/bootstrap/js/bootstrap.bundle.min.js",
                // DataTables
                "~/Scripts/AdminLTE/datatables/jquery.dataTables.min.js",
                "~/Scripts/AdminLTE/datatables-bs4/js/dataTables.bootstrap4.min.js",
                "~/Scripts/AdminLTE/datatables-responsive/js/dataTables.responsive.min.js",
                "~/Scripts/AdminLTE/datatables-responsive/js/responsive.bootstrap4.min.js",
                // ChartJS
                "~/Scripts/AdminLTE/chart.js/Chart.min.js",
                // Sparkline
                "~/Scripts/AdminLTE/sparklines/sparkline.js",
                // jQuery Knob Chart
                "~/Scripts/AdminLTE/jquery-knob/jquery.knob.min.js",
                // daterangepicker
                "~/Scripts/AdminLTE/moment/moment.min.js",
                "~/Scripts/AdminLTE/daterangepicker/daterangepicker.js",
                // Tempusdominus Bootstrap 4
                "~/Scripts/AdminLTE/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",
                // Summernote
                "~/Scripts/AdminLTE/summernote/summernote-bs4.min.js",
                // overlayScrollbars
                "~/Scripts/AdminLTE/overlayScrollbars/js/jquery.overlayScrollbars.min.js",
                // AdminLTE App
                "~/Scripts/AdminLTE/dist/js/adminlte.js",
                // AdminLTE dashboard demo (This is only for demo purposes)
                "~/Scripts/AdminLTE/dist/js/pages/dashboard.js",
                // AdminLTE for demo purposes
                "~/Scripts/AdminLTE/dist/js/demo.js"
                //,

                //"~/Scripts/jquery.unobtrusive-ajax.js"
                ));

            bundles.Add(new ScriptBundle("~/AdminLTE/jquery").Include(
                "~/Scripts/AdminLTE/jquery/jquery.min.js",
                "~/Scripts/AdminLTE/jquery-ui/jquery-ui.min.js"));

            
            bundles.Add(new ScriptBundle("~/AdminLTE/Bootstrap4").Include(
                "~/Scripts/AdminLTE/bootstrap/js/bootstrap.bundle.min.js",
                "~/Scripts/AdminLTE/datatables/jquery.dataTables.min.js",
                "~/Scripts/AdminLTE/datatables-bs4/js/dataTables.bootstrap4.min.js",
                "~/Scripts/AdminLTE/datatables-responsive/js/dataTables.responsive.min.js",
                "~/Scripts/AdminLTE/datatables-responsive/js/responsive.bootstrap4.min.js"));


            
            bundles.Add(new ScriptBundle("~/AdminLTE/ChartJS").Include(
                "~/Scripts/AdminLTE/jquery/jquery.min.js",
                "~/Scripts/AdminLTE/jquery-ui/jquery-ui.min.js"));
            
            bundles.Add(new ScriptBundle("~/AdminLTE/jquery").Include(
                "~/Scripts/AdminLTE/jquery/jquery.min.js",
                "~/Scripts/AdminLTE/jquery-ui/jquery-ui.min.js"));

            //
            bundles.Add(new ScriptBundle("~/AdminLTE/ChartJS").Include(
                "~/Scripts/AdminLTE/chart.js/Chart.min.js"));

            
            bundles.Add(new ScriptBundle("~/AdminLTE/jquery").Include(
                "~/Scripts/AdminLTE/jquery/jquery.min.js",
                "~/Scripts/AdminLTE/jquery-ui/jquery-ui.min.js"));

            
            bundles.Add(new ScriptBundle("~/AdminLTE/jquery").Include(
                "~/Scripts/AdminLTE/jquery/jquery.min.js",
                "~/Scripts/AdminLTE/jquery-ui/jquery-ui.min.js"));

            
            bundles.Add(new ScriptBundle("~/AdminLTE/jquery").Include(
                "~/Scripts/AdminLTE/jquery/jquery.min.js",
                "~/Scripts/AdminLTE/jquery-ui/jquery-ui.min.js"));


            bundles.Add(new ScriptBundle ("~/adminBundles/js").Include(
                
                // ChartJS
                
                // Sparkline
                "~/Scripts/AdminLTE/sparklines/sparkline.js",
                // jQuery Knob Chart
                "~/Scripts/AdminLTE/jquery-knob/jquery.knob.min.js",
                // daterangepicker
                "~/Scripts/AdminLTE/moment/moment.min.js",
                "~/Scripts/AdminLTE/daterangepicker/daterangepicker.js",
                // Tempusdominus Bootstrap 4
                "~/Scripts/AdminLTE/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",
                // Summernote
                "~/Scripts/AdminLTE/summernote/summernote-bs4.min.js",
                // overlayScrollbars
                "~/Scripts/AdminLTE/overlayScrollbars/js/jquery.overlayScrollbars.min.js",
                // AdminLTE App
                "~/Scripts/AdminLTE/dist/js/adminlte.js",
                // AdminLTE dashboard demo (This is only for demo purposes)
                "~/Scripts/AdminLTE/dist/js/pages/dashboard.js",
                // AdminLTE for demo purposes
                "~/Scripts/AdminLTE/dist/js/demo.js",

                "~/Scripts/jquery.unobtrusive-ajax.js"
                ));

            // style bundle

            bundles.Add(new StyleBundle("~/styleBundle/css").Include(
                      "~/css/bootstrap.css",
                      "~/css/style.css"));

            /*
            
            <!-- Stylesheets -->
    @*<link href="~/css/bootstrap.css" rel="stylesheet">
    <link href="~/css/style.css" rel="stylesheet">*@
    <!-- Responsive File -->
    @*<link href="~/css/responsive.css" rel="stylesheet">*@

             */


            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
