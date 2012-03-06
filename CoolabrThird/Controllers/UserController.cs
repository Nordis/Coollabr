using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoolabrThird.Abstract;
using CoolabrThird.Models;

namespace CoolabrThird.Controllers
{
    public class UserController : RavenDbController
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(User user)
        {

            user = user.SetPassword("nordis");
            RavenSession.Store(user);


            return View(user);
        }

    }
}
