using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbleSuccess.Commissions.WebUi.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult _404()
        {
            return View();
        }

        public ActionResult _000()
        {
            return View();
        }
    }
}