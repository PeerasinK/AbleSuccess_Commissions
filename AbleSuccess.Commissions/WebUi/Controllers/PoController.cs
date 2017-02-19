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
    public class PoController : BaseController
    {
        public ActionResult Index()
        {
            PoManager manager = new PoManager();
            PoCollectionViewModel model = new PoCollectionViewModel
            {
                SearchDateFrom = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1),
                SearchDateTo = DateTime.UtcNow,
                SearchCustomer = ""
            };

            model = manager.GetPoList(model);

            return View(model);
        }

        public ActionResult List(DateTime dateFrom, DateTime dateTo, string customer)
        {
            PoManager manager = new PoManager();

            PoCollectionViewModel model = new PoCollectionViewModel
            {
                SearchDateFrom = dateFrom,
                SearchDateTo = dateTo,
                SearchCustomer = customer
            };

            model = manager.GetPoList(model);

            return PartialView("List", model);
        }

        public ActionResult Detail(int detailId = 0)
        {
            PoDetailViewModel model = new PoDetailViewModel();
            PoManager manager = new PoManager();
            EmployeeManager empManager = new EmployeeManager();
            CommissionManager comManager = new CommissionManager();


            if (detailId > 0)
            {
                ViewBag.Action = "Edit";

                int currentProfilerId = 0;
                // if role is not "Able" then has to get profilerId
                if (Helper.UserRole != "1")
                {
                    currentProfilerId = empManager.GetEmployeeDetail(0).ProfileId;
                }
                model = manager.GetPoDetail(detailId);

                if (currentProfilerId.Equals(model.SalesProfileId1)
                    || currentProfilerId.Equals(model.SalesProfileId2)
                    || currentProfilerId.Equals(model.SalesProfileId3))
                    model.IsOwner = true;
                else model.IsOwner = false;
            }
            else
            {
                ViewBag.Action = "New";
                model = manager.MappingCommissionDetail(model);
                model.IsOwner = true;
                model.PoDate = DateTime.UtcNow;
                model.CommissionRate = comManager.GetCommissionRate(false);
            }

            // Set config
            model.CommissionPayRate = Helper.CommissionPayRate;
            model.CommissionForSale = Helper.CommissionForSale;

            // Set Lookup
            List<LookupModel> lookupEmployee = new List<LookupModel> { new LookupModel { Key = "-1", Value = "Please select employee" } };
            lookupEmployee.AddRange(empManager.GetLookup());
            model.LookupEmployee = lookupEmployee;
            model.LookupProductType = Helper.LookupProductType;
            model.LookupProductBrand = Helper.LookupProductBrand;
            model.LookupTransportLocation = Helper.LookupTransportationProvince;
            model.LookupPosition = Helper.LookupPosition;

            return View(model);
        }

        [HttpPost]
        public ActionResult New(PoDetailViewModel model)
        {
            try
            {
                PoManager manager = new PoManager();
                manager.CreatePo(model);
            }
            catch (Exception e)
            {
                return Json(new { Result = false, Message = e.Message });
            }

            return Json(new { Result = true, Url = Url.Action("Index", "Po") });
        }

        [HttpPost]
        public ActionResult Edit(PoDetailViewModel model)
        {
            try
            {
                PoManager manager = new PoManager();
                manager.UpdatePo(model);
            }
            catch (Exception e)
            {
                return Json(new { Result = false, Message = e.Message });
            }

            return Json(new { Result = true, Url = Url.Action("Detail", "Po") + "?detailId=" + model.PoDetailId });
        }

        [HttpPost]
        public ActionResult Remove(int detailId)
        {
            try
            {
                PoManager manager = new PoManager();
                manager.DeletePo(detailId);
            }
            catch (Exception e)
            {
                return Json(new { Result = false, Message = e.Message });
            }

            return Json(new { Result = true, Url = Url.Action("Index", "Po") });
        }
    }
}