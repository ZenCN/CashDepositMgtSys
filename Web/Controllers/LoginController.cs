using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Service;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return File("~/client/index.html", "text/html");
        }

        public string QueryUserInfo(string user_code)
        {
            LoginSvr svr = new LoginSvr();

            return JsonConvert.SerializeObject(svr.QueryUserInfo(user_code));
        }

    }
}
