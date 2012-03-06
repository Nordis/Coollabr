using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CoolabrThird.Abstract;
using CoolabrThird.Models;

namespace CoolabrThird.Controllers
{
    public class AdminController : RavenDbController
    {
        //
        // GET: /Admin/

        public ActionResult Index(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                RedirectFromLoginPage(returnUrl);
            }

            return View("Login", new LogOnModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public ActionResult Login(LogOnModel model)
        {
            var user = RavenSession.Query<User>().FirstOrDefault(u => u.Email == model.Login);

            if (user == null || user.ValidatePassword(model.Password) == false)
            {
                ModelState.AddModelError("UserNotExistOrPasswordNotMatch",
                                         "Email and password do not match to any known user.");
            }
            else if (user.Enabled == false)
            {
                ModelState.AddModelError("UserNotEnabled",
                                         "User is not enabled");
            }

            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(model.Login, true);
                return RedirectFromLoginPage(model.ReturnUrl);
            }

            return View(new LogOnModel { Login = model.Login, ReturnUrl = model.ReturnUrl });
        }

        private ActionResult RedirectFromLoginPage(string retrunUrl = null)
        {
            if (string.IsNullOrEmpty(retrunUrl))
                return RedirectToRoute("homepage");
            return Redirect(retrunUrl);
        }

    }
}
