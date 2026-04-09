using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Data;

using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Threading;

namespace Prototype.Providers
{
    public enum EPExcelAlignment
    {
        Left = 0,
        Center = 1,
        Right = 2
    }

    public class EPExcelAttr
    {
        public string Format { get; set; }
        public EPExcelAlignment Alignment { get; set; }
    }
    public class ExportExcel_EPPlus : IDisposable
    {
        public virtual void ToExcel(DataTable dt)
        {
            ToExcel(dt, "Sheet1");
        }
        public virtual void ToExcel(DataTable dt, string Filename)
        {
            ToExcel(dt, Filename, null);
        }

        public virtual void ToExcel(DataTable dt, string Filename, string[] headName)
        {
            ToExcel(dt, Filename, headName, null);
        }

        public virtual void ToExcel(DataTable dt, string Filename, string[] headName, Func<string, EPExcelAttr> _statement)
        {
            try
            {
                MemoryStream ms = DataTableToExcelXlsx(dt, "Sheet1", headName, _statement);
                ms.WriteTo(HttpContext.Current.Response.OutputStream);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Filename);
                // HttpContext.Current.Response.StatusCode = 200;
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (ThreadAbortException ex)
            {

            }
        }

        private static MemoryStream DataTableToExcelXlsx(DataTable table, string sheetName, string[] headName, Func<string, EPExcelAttr> _statement)
        {
            MemoryStream Result = new MemoryStream();
            ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(sheetName);

            if (headName != null)
            {
                for (int i = 0; i < headName.Count(); i++)
                {
                    if (headName[i].Contains("#"))
                    {
                        ws.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, i + 1].Style.Font.Bold = true;
                        ws.Cells[1, i + 1].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                    }

                    ws.Cells[1, i + 1].Value = headName[i].Trim('#');

                    ws.Cells[1, i + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[1, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[1, i + 1].AutoFitColumns();
                }
            }
            else
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    ws.Cells[1, i + 1].Value = table.Columns[i].ColumnName;
                }
            }


            if (table.Rows.Count > 0)
            {
                ws.Cells["A2"].LoadFromDataTable(table, false);


                var countCol = 1;
                foreach (DataColumn tableCol in table.Columns)
                {
                    using (ExcelRange col = ws.Cells[2, countCol, 2 + table.Rows.Count, countCol])
                    {
                        if (_statement == null)
                        {
                            
                            //col.AutoFitColumns();
                            if (tableCol.DataType == typeof(DateTime))
                            {
                                
                                //col.Style.Numberformat.Format = "dd/MM/yyyy" + " " + "HH:mm:ss";
                                col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Column(countCol).Style.Numberformat.Format = "dd/MM/yyyy";
                            }
                            if (tableCol.DataType == typeof(string))
                            {
                                ws.Column(countCol).Style.Numberformat.Format = "@";
                            }
                            if (tableCol.DataType == typeof(int))
                            {
                                ws.Column(countCol).Style.Numberformat.Format = "0";
                            }
                        }
                        else
                        {
                            var attr = _statement(tableCol.ColumnName);

                            if (attr != null)
                            {
                                col.Style.Numberformat.Format = attr.Format;
                            }

                            switch (attr.Alignment)
                            {
                                case EPExcelAlignment.Left:
                                    col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    break;
                                case EPExcelAlignment.Center:
                                    col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    break;
                                case EPExcelAlignment.Right:
                                    col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    break;
                            }
                        }
                    }
                    countCol++;
                }
            }
            // ws = pack.Workbook.Worksheets.Add("TEST"); ADD NEW SHEET
            pack.SaveAs(Result);
            return Result;
        }


        public bool IsReusable
        {
            get { return false; }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
