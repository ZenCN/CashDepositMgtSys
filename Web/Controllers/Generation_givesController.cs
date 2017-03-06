using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Exception;
using Infrastructure.Operation;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Service;
using Service.Model;

namespace Web.Controllers
{
    public class Generation_givesController : Controller
    {
        private Result result = null;
        private Generation_givesSvr svr = new Generation_givesSvr();

        public string Delete(string ids)
        {
            List<int> list = new List<int>();

            var str_arr = ids.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var id in str_arr)
            {
                list.Add(int.Parse(id));
            }

            return JsonConvert.SerializeObject(svr.Delete(list));
        }

        public string Save(string generation_gives, string deducted_items)
        {
            result = svr.Save(generation_gives, deducted_items, int.Parse(Request.Cookies["user_level"].Value));

            return JsonConvert.SerializeObject(result);
        }

        public string GetDeducteds(int id)
        {
            return JsonConvert.SerializeObject(svr.GetDeducteds(id));
        }

        public void Export(int page_index, int page_size, string salesman_card_id, string salesman_name)
        {
            svr.Export(page_index, page_size, salesman_card_id, salesman_name,
                Request.Cookies["user_code"].Value, Request.Cookies["agency_code"].Value, int.Parse(Request.Cookies["user_level"].Value));
        }

        public string Search(int page_index, int page_size, string salesman_card_id, string salesman_name)
        {
            return JsonConvert.SerializeObject(svr.Search(page_index, page_size, salesman_card_id, salesman_name,
                Request.Cookies["user_code"].Value, Request.Cookies["agency_code"].Value, int.Parse(Request.Cookies["user_level"].Value)));
        }

        public string ChangeReviewState(string ids, int state)
        {
            List<int> list = new List<int>();

            var str_arr = ids.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var id in str_arr)
            {
                list.Add(int.Parse(id));
            }

            return JsonConvert.SerializeObject(svr.ChangeReviewState(list, state));
        }

        public string Import(HttpPostedFileBase file)
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
                        ICell cell = sheet.GetRow(0).GetCell(0);
                        if (cell != null && cell.StringCellValue == "管理机构")
                        {
                            int startRow = 2;
                            int rowCount = sheet.LastRowNum;
                            int cellCount = sheet.GetRow(0).LastCellNum; //一行最后一个cell的编号 即总的列数

                            Dictionary<int, string> field = GetFieldDic();
                            List<Generation_gives> list = new List<Generation_gives>();
                            Generation_gives gives = new Generation_gives();

                            Type type = gives.GetType();
                            System.Reflection.PropertyInfo property = null;

                            for (int r = startRow; r <= rowCount; r++)
                            {
                                gives = new Generation_gives();
                                gives.agency_code = Request.Cookies["agency_code"].Value;
                                gives.recorder_code = Request.Cookies["user_code"].Value;
                                gives.record_date = DateTime.Now;
                                gives.review_state = 0;
                                for (int c = 0; c < cellCount; c++)
                                {
                                    cell = sheet.GetRow(r).GetCell(c);
                                    property = type.GetProperty(field[c]);

                                    if (cell.CellType == CellType.Numeric)
                                    {
                                        if (field[c] == "salesman_hiredate")
                                        {
                                            property.SetValue(gives, cell.DateCellValue, null);
                                        }
                                        else
                                        {
                                            property.SetValue(gives, Convert.ToDecimal(cell.NumericCellValue), null);
                                        }
                                    }
                                    else
                                    {
                                        property.SetValue(gives, cell.StringCellValue, null);
                                    }
                                }
                                list.Add(gives);
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
            dic.Add(13, "salesman_refunds");
            dic.Add(14, "remark");

            return dic;
        }
    }
}