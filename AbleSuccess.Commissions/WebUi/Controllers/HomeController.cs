using AbleSuccess.Commissions.WebUi.Common;
using AbleSuccess.Commissions.WebUi.Manager;
using AbleSuccess.Commissions.WebUi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        public ActionResult PrintReport(int type, int subType, int year, int salesProfileId)
        {
            try
            {
                FastReportManager FastReportManager = new FastReportManager();
                ReportManager manager = new ReportManager();
                List<ChartData> dataList = manager.YearlyReport(type, subType, year, salesProfileId);

                DashBoardMasterReportModel model = new DashBoardMasterReportModel
                {
                    ChartDataModels = dataList.Select(x => new ChartDataModel()
                                                        {
                                                            label = x.label,
                                                            y = x.y
                                                        }).ToList(),
                    Name = salesProfileId.ToString(),
                    ReportType = type.ToString(),
                    ReportSubType = subType.ToString(),
                    Year = year.ToString()
                };
                byte[] strm = FastReportManager.GenPDFFile(Request.PhysicalApplicationPath
                                                            + "Reports/DashBoardChart.frx", model).ToArray();
                return File(strm, "application/pdf", "Report.pdf");
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}