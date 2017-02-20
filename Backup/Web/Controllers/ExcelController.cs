using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Operation;
using Infrastructure.Exception;

namespace Web.Controllers
{
    public class ExcelController : Controller
    {
        

        public Result Upload(HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    return null;
                }
                else
                {
                    return new Result(ResultType.Error,"找不到上传的文件，name = file");
                }
            }
            catch (Exception ex)
            {
                return new Result(ResultType.Error, new Message(ex).ErrorDetails);
            }
        }

    }
}
