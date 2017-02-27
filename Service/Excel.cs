using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using Service.Model;
using System.Web;

namespace Service
{
    public class Excel
    {
        public void Export(List<Generation_gives> list)
        {
            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/Excel/Generation_gives.xls", FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            int strat_row = 2;
            ISheet sheet = hssfworkbook.GetSheet("数据");
            sheet = CreateCell(sheet, list.Count, 15, strat_row);

            ICell cell = null;
            ICellStyle cell_style = SetCellStyle(sheet);
            for (int i = 0; i < list.Count; i++)
            {
                cell = sheet.GetRow(strat_row + i).GetCell(0);
                cell.SetCellValue(list[i].agency_code);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(1);
                cell.SetCellValue(list[i].salesman_name);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(2);
                cell.SetCellValue(list[i].salesman_sex);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(3);
                cell.SetCellValue(list[i].salesman_card_type);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(4);
                cell.SetCellValue(list[i].salesman_card_id);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(5);
                cell.SetCellValue(list[i].salesman_phone);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(6);
                cell.SetCellValue(list[i].salesman_hiredate == null
                    ? null
                    : list[i].salesman_hiredate.Value.ToString("yyyy年M月d日"));
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(7);
                cell.SetCellValue(list[i].salesman_bank_account_name);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(8);
                cell.SetCellValue(list[i].salesman_bank_account_number);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(9);
                cell.SetCellValue(list[i].salesman_bank_name);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(10);
                cell.SetCellValue(list[i].salesman_bank_province);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(11);
                cell.SetCellValue(list[i].salesman_bank_city);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(12);
                cell.SetCellValue(list[i].salesman_cash_deposit == null ? "" : list[i].salesman_cash_deposit.ToString());
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(13);
                cell.SetCellValue(list[i].salesman_refunds == null ? "" : list[i].salesman_refunds.ToString());
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(14);
                cell.SetCellValue(list[i].remark);
                cell.CellStyle = cell_style;
            }

            ResponseExcel(hssfworkbook);
        }

        public void Export(List<Generation_buckle> list)
        {
            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/Excel/Generation_buckle.xls", FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            int strat_row = 2;
            ISheet sheet = hssfworkbook.GetSheet("数据");
            sheet = CreateCell(sheet, list.Count, 14, strat_row);

            ICell cell = null;
            ICellStyle cell_style = SetCellStyle(sheet);
            for (int i = 0; i < list.Count; i++)
            {
                cell = sheet.GetRow(strat_row + i).GetCell(0);
                cell.SetCellValue(list[i].agency_code);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(1);
                cell.SetCellValue(list[i].salesman_name);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(2);
                cell.SetCellValue(list[i].salesman_sex);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(3);
                cell.SetCellValue(list[i].salesman_card_type);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(4);
                cell.SetCellValue(list[i].salesman_card_id);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(5);
                cell.SetCellValue(list[i].salesman_phone);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(6);
                cell.SetCellValue(list[i].salesman_hiredate == null
                    ? null
                    : list[i].salesman_hiredate.Value.ToString("yyyy年M月d日"));
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(7);
                cell.SetCellValue(list[i].salesman_bank_account_name);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(8);
                cell.SetCellValue(list[i].salesman_bank_account_number);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(9);
                cell.SetCellValue(list[i].salesman_bank_name);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(10);
                cell.SetCellValue(list[i].salesman_bank_province);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(11);
                cell.SetCellValue(list[i].salesman_bank_city);
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(12);
                cell.SetCellValue(list[i].salesman_cash_deposit == null ? "" : list[i].salesman_cash_deposit.ToString());
                cell.CellStyle = cell_style;

                cell = sheet.GetRow(strat_row + i).GetCell(13);
                cell.SetCellValue(list[i].remark);
                cell.CellStyle = cell_style;
            }

            ResponseExcel(hssfworkbook);
        }

        private ISheet CreateCell(ISheet sheet, int row_count, int col_count, int start_row)
        {
            for (int i = 0; i < row_count; i++)
            {
                IRow row = null;
                if (sheet.GetRow(start_row + i) == null)
                {
                    row = sheet.CreateRow(start_row + i);
                }
                else
                {
                    row = sheet.GetRow(start_row + i);
                }
                for (int j = 0; j < col_count; j++)
                {
                    if (row.GetCell(j) == null)
                    {
                        row.CreateCell(j);
                    }
                }
            }

            return sheet;
        }

        private ICellStyle SetCellStyle(ISheet sheet)
        {
            ICellStyle style = sheet.Workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;//水平对齐居中
            style.BorderBottom = BorderStyle.Thin; //边框是黑色的
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            return style;
        }

        private void ResponseExcel(HSSFWorkbook hssfworkbook)
        {
            // 设置响应头（文件名和文件格式）
            //设置响应的类型为Excel
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            //设置下载的Excel文件名
            HttpContext.Current.Response.AddHeader("Content-Disposition",
                string.Format("attachment; filename={0}", DateTime.Now.ToString("yyyy年MM月dd日 HH mm ss") + ".xls"));
            //Clear方法删除所有缓存中的HTML输出。但此方法只删除Response显示输入信息，不删除Response头信息。以免影响导出数据的完整性。
            HttpContext.Current.Response.Clear();

            //写入到客户端
            using (MemoryStream ms = new MemoryStream())
            {
                //将工作簿的内容放到内存流中
                hssfworkbook.Write(ms);
                //将内存流转换成字节数组发送到客户端
                HttpContext.Current.Response.BinaryWrite(ms.GetBuffer());
                //HttpContext.Current.ApplicationInstance.CompleteRequest();//为了解决Response.End()由于代码已经过优化或者本机框架位于调用堆栈之上，无法计算表达式的值 的异常
                HttpContext.Current.Response.End();
            }
        }
    }
}
