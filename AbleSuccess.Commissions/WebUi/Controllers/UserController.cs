using AbleSuccess;
using AbleSuccess.Commissions;
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
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            UserManager manager = new UserManager();
            UserCollectionViewModel model = manager.GetUserList(new UserCollectionViewModel
            {
                SearchUserStatus = 1
            });
            model.SearchUserStatus = 1;

            return View(model);
        }

        public ActionResult List(string username, int role, int status)
        {
            UserManager manager = new UserManager();

            UserCollectionViewModel model = new UserCollectionViewModel
            {
                SearchUsername = username,
                SearchUserRole = role,
                SearchUserStatus = status
            };
            model = manager.GetUserList(model);

            return PartialView("List", model);
        }

        public ActionResult Detail(int id)
        {
            UserDetailViewModel model = new UserDetailViewModel();

            if (id > 0)
            {
                ViewBag.Action = "Edit";
                UserManager manager = new UserManager();
                model = manager.GetUserDetail(id);
            }
            else
            {
                ViewBag.Action = "New";
                // Set Lookup
                model.Status = -1;
                model.Role = -1;
                model.Position = -1;
                model.Devision = -1;
                model.LookupUserStatus = Helper.LookupUserStatus;
                model.LookupRole = Helper.LookupRole;
                model.LookupPosition = Helper.LookupPosition;
                model.LookupDevision = Helper.LookupDevision;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult New(UserDetailViewModel model)
        {
            try
            {
                UserManager manager = new UserManager();
                manager.CreateUser(model);
            }
            catch (Exception e)
            {
                return Json(new { Result = false, Message = e.Message });
            }

            return Json(new { Result = true, Url = Url.Action("Detail", "User") + "?id=" + model.UserId });
        }

        [HttpPost]
        public ActionResult Edit(UserDetailViewModel model)
        {
            try
            {
                UserManager manager = new UserManager();
                manager.UpdateUser(model);
            }
            catch (Exception e)
            {
                return Json(new { Result = false, Message = e.Message });
            }

            return Json(new { Result = true, Url = Url.Action("Detail", "User") + "?id=" + model.UserId });
        }

        public ActionResult ResetPassword(int id)
        {
            UserManager manager = new UserManager();
            manager.ResetPassword(id);

            return Content("Reset password successful");
        }

        public ActionResult Add(int id, string username, int role, int status)
        {
            UserManager manager = new UserManager();
            manager.UpdateUserStatus(id, 1);

            return RedirectToAction("List", "User", new { username, role, status });
        }

        public ActionResult Delete(int id, string username, int role, int status)
        {
            UserManager manager = new UserManager();
            manager.UpdateUserStatus(id, 0);

            return RedirectToAction("List", "User", new { username, role, status });
        }
    }
}