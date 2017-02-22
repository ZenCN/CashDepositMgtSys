using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Operation;
using Newtonsoft.Json;
using Service;

namespace Web.Controllers
{
    public class Generation_givesController : Controller
    {
        private Result result = null;
        private Generation_givesSvr svr = new Generation_givesSvr();

        public string Save(string generation_gives, string deducted_items)
        {
            result = svr.Save(generation_gives, deducted_items);

            return JsonConvert.SerializeObject(result);
        }
    }
}