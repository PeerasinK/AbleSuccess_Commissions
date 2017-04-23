using AbleSuccess;
using AbleSuccess.Commissions.WebUi.Common;
using AbleSuccess.Commissions.WebUi.Manager;
using AbleSuccess.Commissions.WebUi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbleSuccess.Commissions.WebUi.Controllers
{
    public class CommissionController : BaseController
    {
        public ActionResult Index(int userId = -1)
        {
            CommissionManager manager = new CommissionManager();
            EmployeeManager empManager = new EmployeeManager();

            
            CommissionCollectionViewModel model = new CommissionCollectionViewModel
            {
                SearchDateFrom = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1),
                SearchDateTo = DateTime.UtcNow,
                SearchProfileId = -1,
                SearchRoleId = -1,
                CommissionViewCollection = new List<CommissionViewModel>()
            };

            if(userId > 0) model = manager.GetCommissionList(model, userId);

            List<LookupModel> lookupEmployee = new List<LookupModel> { new LookupModel { Key = "-1", Value = "---- Employee ----" } };
            lookupEmployee.AddRange(empManager.GetLookup());
            model.LookupEmployee = lookupEmployee;
            model.LookupPosition = Helper.LookupPosition;

            return View(model);
        }

        public ActionResult List(DateTime dateFrom, DateTime dateTo, int profileId = -1, int roleId = -1)
        {
            CommissionManager manager = new CommissionManager();
            EmployeeManager empManager = new EmployeeManager();

            CommissionCollectionViewModel model = new CommissionCollectionViewModel
            {
                SearchDateFrom = dateFrom,
                SearchDateTo = dateTo,
                SearchProfileId = profileId,
                SearchRoleId = roleId
            };

            model = profileId > 0 ? manager.GetCommissionList(model) : manager.GetCommissionList(model, int.Parse(Helper.UserId));

            List<LookupModel> lookupEmployee = new List<LookupModel> { new LookupModel { Key = "-1", Value = "---- Employee ----" } };
            lookupEmployee.AddRange(empManager.GetLookup());
            model.LookupEmployee = lookupEmployee;
            model.LookupPosition = Helper.LookupPosition;

            return PartialView("List", model);
        }

        public ActionResult Rate()
        {
            return View(new CommissionRateViewModel { SearchYear = DateTime.Now.Year.ToString() });
        }

        public ActionResult RateNew()
        {
            return View(new CommissionRateDetailViewModel());
        }

        public ActionResult RateDetail(int year)
        {
            if (year == 0) year = DateTime.Now.Year;

            CommissionManager manager = new CommissionManager();
            CommissionRateDetailViewModel model = manager.GetCommissionRateDetail(year);

            return PartialView("RateDetail", model);
        }

        [HttpPost]
        public JsonResult GetRateDetail(int year)
        {
            if (year == 0) year = DateTime.Now.Year;

            CommissionManager manager = new CommissionManager();
            CommissionRateDetailViewModel model = manager.GetCommissionRateDetail(year);

            return Json(new { Result = model });
        }

        [HttpPost]
        public ActionResult NewRate(CommissionRateDetailViewModel model)
        {
            try
            {
                CommissionManager manager = new CommissionManager();
                manager.CreateCommissionRate(model);
            }
            catch (Exception e)
            {
                return Json(new { Result = false, Message = e.Message });
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public ActionResult EditRate(CommissionRateDetailViewModel model)
        {
            try
            {
                CommissionManager manager = new CommissionManager();
                manager.UpdateCommissionRate(model);
            }
            catch (Exception e)
            {
                return Json(new { Result = false, Message = e.Message });
            }

            return Json(new { Result = true });
        }

        [HttpGet]
        public ActionResult PrintReport(DateTime dateFrom, DateTime dateTo, int profileId = -1, int roleId = -1)
        {
            try
            {
                FastReportManager FastReportManager = new FastReportManager();
                CommissionManager manager = new CommissionManager();
                EmployeeManager empManager = new EmployeeManager();
                CommissionCollectionViewModel model = new CommissionCollectionViewModel
                {
                    SearchDateFrom = dateFrom,
                    SearchDateTo = dateTo,
                    SearchProfileId = profileId,
                    SearchRoleId = roleId
                };
                model = profileId > 0 ? manager.GetCommissionList(model) : manager.GetCommissionList(model, int.Parse(Helper.UserId));

                List<LookupModel> lookupEmployee = new List<LookupModel> { new LookupModel { Key = "-1", Value = "---- Employee ----" } };
                lookupEmployee.AddRange(empManager.GetLookup());
                model.LookupEmployee = lookupEmployee;
                model.LookupPosition = Helper.LookupPosition;

                byte[] strm = FastReportManager.GenPDFFile(Request.PhysicalApplicationPath
                                                            + "Reports/ReportCommission.frx", model).ToArray();
                return File(strm, "application/pdf", "Report.pdf");
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}