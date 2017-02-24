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

        public string Search(int page_index, int page_size, string salesman_card_id, string salesman_name)
        {
            return JsonConvert.SerializeObject(svr.Search(page_index, page_size, salesman_card_id, salesman_name));
        }

        public string ChangeReviewState(string ids, int state)
        {
            List<int> list = new List<int>();

            var str_arr = ids.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var id in str_arr)
            {
                list.Add(int.Parse(id));
            }
            
            result = svr.ChangeReviewState(list, state);

            return JsonConvert.SerializeObject(result);
        }
    }
}