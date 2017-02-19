using AbleSuccess;
using AbleSuccess.Commissions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using AbleSuccess.Commissions.WebUi.Common;
using AbleSuccess.Commissions.WebUi.Manager;
using AbleSuccess.Commissions.WebUi.Models;

namespace AbleSuccess.Commissions.WebUi.Controllers
{
    public class EmployeeController : BaseController
    {
        public ActionResult Index()
        {
            EmployeeManager manager = new EmployeeManager();
            EmployeeCollectionViewModel model = manager.GetEmployeeList();

            return View(model);
        }

        public ActionResult List(string name, int position, int devision)
        {
            EmployeeManager manager = new EmployeeManager();

            EmployeeCollectionViewModel model = new EmployeeCollectionViewModel
            {
                SearchEmployeeName = name,
                SearchPosition = position,
                SearchDevision = devision
            };
            model = manager.GetEmployeeList(model);

            return PartialView("List", model);
        }

        public ActionResult Detail(int id = 0)
        {
            EmployeeDetailViewModel model = new EmployeeDetailViewModel();

            EmployeeManager manager = new EmployeeManager();
            model = manager.GetEmployeeDetail(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeDetailViewModel model)
        {
            try
            {
                EmployeeManager manager = new EmployeeManager();
                manager.UpdateEmployee(model);
            }
            catch (Exception e)
            {
                return Json(new { Result = false, Message = e.Message });
            }

            return Json(new { Result = true, Url = Url.Action("Detail", "Employee") + "?id=" + model.ProfileId });
        }

        public ActionResult ChangeImage(byte[] image)
        {
            EmployeeManager manager = new EmployeeManager();
            manager.ChangeImage(Int32.Parse(Helper.UserId), image);

            return Content("Change image successful");
        }

        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                EmployeeManager manager = new EmployeeManager();
                manager.ChangePassword(Int32.Parse(Helper.UserId), oldPassword, newPassword);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    Result = false,
                    Message = "Old password invalid"
                });
            }

            return Json(new { Result = true });
        }
    }
}