using AbleSuccess.Commissions.WebUi.Common;
using AbleSuccess.Commissions.WebUi.Manager;
using AbleSuccess.Commissions.WebUi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbleSuccess.Commissions.WebUi.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ReportManager manager = new ReportManager();
            ReportViewModel model = new ReportViewModel
            {
                LookupQuarter = Helper.LookupQuarter,
                LookupYear =  manager.GetLookupYear(),
                Quarter = 1,
                Year = DateTime.UtcNow.Year,
            };

            return View(model);
        }

        public ActionResult Report(int type, int subType, int year)
        {
            ReportManager manager = new ReportManager();
            List<ChartData> dataList = manager.YearlyReport(type, subType, year);

            return Json(new { data = dataList });
        }
    }
}