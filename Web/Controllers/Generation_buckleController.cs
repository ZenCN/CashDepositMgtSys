using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Operation;
using Infrastructure.Exception;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Service;
using Service.Model;

namespace Web.Controllers
{
    public class Generation_buckleController : Controller
    {
        private Result result = null;
        private Generation_buckleSvr svr = new Generation_buckleSvr();

        public string Delete(string ids)
        {
            List<int> list = new List<int>();

            var str_arr = ids.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var id in str_arr)
            {
                list.Add(int.Parse(id));
            }

            return JsonConvert.SerializeObject(svr.Delete(list));
        }

        public string SumDetails(int page_index, int page_size, string agency_code, string channel, string apply_start,
            string apply_end)
        {
            return
                JsonConvert.SerializeObject(svr.SumDetails(page_index, page_size, agency_code, channel,
                    DateTime.Parse(apply_start),
                    DateTime.Parse(apply_end), HttpUtility.UrlDecode(Request.Cookies["user_jurisdiction"].Value)));
        }

        public string QuerySchedule(int page_index, int page_size, string agency_code, string channel,
            string apply_start,
            string apply_end)
        {
            return
                JsonConvert.SerializeObject(svr.QuerySchedule(page_index, page_size, agency_code, channel,
                    DateTime.Parse(apply_start),
                    DateTime.Parse(apply_end), HttpUtility.UrlDecode(Request.Cookies["user_jurisdiction"].Value)));
        }

        public void ExportSchedule(int page_index, int page_size, string agency_code, string channel,
            DateTime apply_start, DateTime apply_end)
        {
            svr.ExportSchedule(page_index, page_size, agency_code, channel, apply_start,
                apply_end, HttpUtility.UrlDecode(Request.Cookies["user_jurisdiction"].Value));
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

        public string Save(string buckle)
        {
            Generation_buckle generation_buckle = JsonConvert.DeserializeObject<Generation_buckle>(buckle);

            if (generation_buckle.id == 0)
            {
                generation_buckle.agency_code = Request.Cookies["agency_code"].Value;
                generation_buckle.recorder_code = Request.Cookies["user_code"].Value;
                generation_buckle.record_date = DateTime.Now;

                int level = int.Parse(Request.Cookies["user_level"].Value);
                switch (level)
                {
                    case 2:
                        generation_buckle.review_state = 2;
                        break;
                    case 3:
                        generation_buckle.review_state = 1;
                        break;
                    case 4:
                        generation_buckle.review_state = 0;
                        break;
                }
            }

            return JsonConvert.SerializeObject(svr.Save(generation_buckle));
        }

        public string Search(int page_index, int page_size, string salesman_card_id, string salesman_name,
            string review_state, string apply_start, string apply_end)
        {
            return
                JsonConvert.SerializeObject(svr.Search(page_index, page_size, salesman_card_id, salesman_name,
                    review_state, DateTime.Parse(apply_start), DateTime.Parse(apply_end),
                    Request.Cookies["user_code"].Value, Request.Cookies["agency_code"].Value,
                    int.Parse(Request.Cookies["user_level"].Value)));
        }

        public void ExportSumDetails(int page_index, int page_size, string agency_code, string channel,
            DateTime apply_start, DateTime apply_end)
        {
            svr.ExportSumDetails(page_index, page_size, agency_code, channel, apply_start,
                apply_end, HttpUtility.UrlDecode(Request.Cookies["user_jurisdiction"].Value));
        }

        public void Export(int page_index, int page_size, string salesman_card_id, string salesman_name,
            string review_state, string apply_start, string apply_end)
        {
            svr.Export(page_index, page_size, salesman_card_id, salesman_name, review_state, DateTime.Parse(apply_start), DateTime.Parse(apply_end),
                Request.Cookies["user_code"].Value, Request.Cookies["agency_code"].Value,
                int.Parse(Request.Cookies["user_level"].Value));
        }

        public string Import(HttpPostedFileBase file, string channel)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            FileStream fs = null;
            string filePath = null;

            try
            {
                if (file != null)
                {
                    filePath = Server.MapPath("~/Excel/") + DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss") + " " +
                               file.FileName;
                    file.SaveAs(filePath);

                    fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    if (filePath.IndexOf(".xlsx") > 0) // 2007版本
                        workbook = new XSSFWorkbook(fs);
                    else if (filePath.IndexOf(".xls") > 0) // 2003版本
                        workbook = new HSSFWorkbook(fs);

                    sheet = workbook.GetSheetAt(0); //获取第一个sheet

                    if (sheet != null)
                    {
                        IRow row = null;
                        ICell cell = sheet.GetRow(0).GetCell(0);
                        if (cell != null && cell.StringCellValue == "管理机构")
                        {
                            int startRow = 2;
                            int rowCount = sheet.PhysicalNumberOfRows;
                            int cellCount = sheet.GetRow(0).LastCellNum; //一行最后一个cell的编号 即总的列数

                            Dictionary<int, string> field = GetFieldDic();
                            List<Generation_buckle> list = new List<Generation_buckle>();
                            Generation_buckle buckle = new Generation_buckle();
                            Type type = buckle.GetType();
                            System.Reflection.PropertyInfo property = null;

                            for (int r = startRow; r < rowCount; r++)
                            {
                                row = sheet.GetRow(r);
                                if (row != null)
                                {
                                    buckle = new Generation_buckle();
                                    for (int c = 0; c < cellCount; c++)
                                    {
                                        cell = row.GetCell(c);

                                        if (cell != null)
                                        {
                                            property = type.GetProperty(field[c]);

                                            if (cell.CellType == CellType.Numeric)
                                            {
                                                if (field[c] == "salesman_hiredate")
                                                {
                                                    property.SetValue(buckle, cell.DateCellValue, null);
                                                }
                                                else
                                                {
                                                    property.SetValue(buckle, Convert.ToDecimal(cell.NumericCellValue),
                                                        null);
                                                }
                                            }
                                            else
                                            {
                                                property.SetValue(buckle, cell.StringCellValue, null);
                                            }
                                        }
                                    }
                                    buckle.agency_code = Request.Cookies["agency_code"].Value;
                                    buckle.recorder_code = Request.Cookies["user_code"].Value;
                                    buckle.record_date = DateTime.Now;
                                    buckle.channel = channel;
                                    int level = int.Parse(Request.Cookies["user_level"].Value);
                                    switch (level)
                                    {
                                        case 2:
                                            buckle.review_state = 2;
                                            break;
                                        case 3:
                                            buckle.review_state = 1;
                                            break;
                                        case 4:
                                            buckle.review_state = 0;
                                            break;
                                    }
                                    list.Add(buckle);
                                }
                            }

                            if (list.Any())
                            {
                                result = svr.Import(list);
                            }
                            else
                            {
                                result = new Result(ResultType.error, file.FileName + " 读取不到Excel中的数据！");
                            }
                        }
                        else
                        {
                            result = new Result(ResultType.error, file.FileName + "格式不正确！");
                        }
                    }
                    else
                    {
                        result = new Result(ResultType.error, file.FileName + " 第一个Sheet为空！");
                    }
                }
                else
                {
                    result = new Result(ResultType.error, "找不到上传的文件，name = file");
                }
            }
            catch (Exception ex)
            {
                result = new Result(ResultType.error, new Message(ex).ErrorDetails);
            }
            finally
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            return JsonConvert.SerializeObject(result);
        }

        private Dictionary<int, string> GetFieldDic()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "agency_code");
            dic.Add(1, "salesman_name");
            dic.Add(2, "salesman_sex");
            dic.Add(3, "salesman_card_type");
            dic.Add(4, "salesman_card_id");
            dic.Add(5, "salesman_phone");
            dic.Add(6, "salesman_hiredate");
            dic.Add(7, "salesman_bank_account_name");
            dic.Add(8, "salesman_bank_account_number");
            dic.Add(9, "salesman_bank_name");
            dic.Add(10, "salesman_bank_province");
            dic.Add(11, "salesman_bank_city");
            dic.Add(12, "salesman_cash_deposit");
            dic.Add(13, "remark");

            return dic;
        }
    }
}