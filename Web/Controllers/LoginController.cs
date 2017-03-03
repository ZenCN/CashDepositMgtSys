using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            int s = int.Parse("s");
            return File("~/client/index.html", "text/html");
        }

    }
}
