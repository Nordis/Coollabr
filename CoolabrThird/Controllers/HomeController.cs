using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoolabrThird.Abstract;

namespace CoolabrThird.Controllers
{
    public class HomeController : RavenDbController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
