using AbleSuccess.Commissions.WebUi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbleSuccess.Commissions.WebUi.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            if (String.IsNullOrEmpty(Helper.UserId)) throw new UnauthorizedAccessException();

            string action = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
            string controller = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");

            // Admin
            if (Helper.UserRole == "0") 
            {
                if(controller != "User" && controller != "Home")
                {
                    throw new HttpRequestValidationException();
                }
            }
            // Able
            else if (Helper.UserRole == "1")
            {
                if (controller == "User")
                {
                    throw new HttpRequestValidationException();
                }
            }
            // User
            else if (Helper.UserRole == "2")
            {
                if (controller == "User")
                {
                    throw new HttpRequestValidationException();
                }
                if(controller =="Commission" && action == "Rate")
                {
                    throw new HttpRequestValidationException();
                }
            }
        }
    }
}