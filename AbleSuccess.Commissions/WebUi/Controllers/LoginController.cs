using AbleSuccess.Commissions.WebUi.Common;
using AbleSuccess.Commissions.WebUi.Manager;
using AbleSuccess.Commissions.WebUi.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AbleSuccess.Commissions.WebUi.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View(new LoginModel());
        }

        public ActionResult Authentication(LoginModel model)
        {
            LoginManager manager = new LoginManager();
            model = manager.Authenticate(model.Username, model.Password);

            if (model == null)
            {
                return View("Index", new LoginModel { ResultMessage = "Username/Passoword invalid" });
            }

            // Set session
            Helper.Username = model.Username;
            Helper.UserId = model.UserId.ToString();
            Helper.UserRole = model.Role.ToString();
            Helper.ProfileId = model.ProfileId.ToString();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Helper.KillSession();
            return RedirectToAction("Index", "Login");
        }
    }
}