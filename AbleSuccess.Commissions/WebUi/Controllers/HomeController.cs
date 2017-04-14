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
            List<LookupModel> lookupSales = manager.GetLookupSales();
            ReportViewModel model = new ReportViewModel
            {
                LookupQuarter = Helper.LookupQuarter,
                LookupYear = manager.GetLookupYear(),
                LookupSales = lookupSales,
                Quarter = 1,
                Year = DateTime.UtcNow.Year,
                IsSales = lookupSales.Exists(x => x.Key == Helper.ProfileId),
            };

            return View(model);
        }

        public ActionResult Report(int type, int subType, int year, int salesProfileId)
        {
            ReportManager manager = new ReportManager();
            List<ChartData> dataList = manager.YearlyReport(type, subType, year, salesProfileId);

            return Json(new { data = dataList });
        }
    }
}