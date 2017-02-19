using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebUi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Get the exception.
            Exception exception = Server.GetLastError();

            //clear the response.
            Response.Clear();

            // Clear the error on server.
            Server.ClearError();

            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;

            //Handle the error
            HandleError(exception, Context);
        }

        public void HandleError(Exception exception, HttpContext httpContext)
        {
            //create the route data to the error controller/
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");

            //check if we have a http exception
            var httpException = exception as System.Web.HttpException;

            if (httpException != null)
            {
                //add route information for the http errors.
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // Page not found.
                        Response.Redirect("~/Error/_404", false);
                        break;

                    // Handle other http errors.
                    default:
                        Response.Redirect("~/Error/_000", false);
                        break;
                }
            }
            else
            {
                Response.Redirect("/Login");
                return;
            }
        }

    }
}
