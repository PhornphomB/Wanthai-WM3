using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Data.OleDb;
using System.IO;
using System.Configuration;


public class PageImportExcel : PageCustom
{
    private DataTable GetExcelData(string FilePath, string Extension)
    {


        try
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    // conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    conStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;TypeGuessRows=0;IMEX=1;ImportMixedTypes=Text;';", FilePath);
                    break;
                case ".xlsx": //Excel 07
                              //conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    conStr = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;TypeGuessRows=0;IMEX=1;ImportMixedTypes=Text;';", FilePath);
                    break;
            }

            conStr = String.Format(conStr, FilePath);

            var connExcel = new OleDbConnection(conStr);
            var cmdExcel = new OleDbCommand();
            var oda = new OleDbDataAdapter();
            var dt = new DataTable();

            cmdExcel.Connection = connExcel;
            connExcel.Open();

            var dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

            //Read Data from First Sheet
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);

            connExcel.Close();
            return dt;
        }
        catch (Exception ex)
        {
            Logging = new Prototype.Providers.Logging(this, ex);
            RaiseLogging();

            DataTable dt = null;
            return dt;
        }
    }

    public DataTable GetDataFromSheet(FileUpload thisFile)
    {
        DataTable _dtExcel = null;

        var Extension = Path.GetExtension(thisFile.PostedFile.FileName);
        var FileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + Extension;
        var FilePath = Server.MapPath("~/_temp/");

        if (!Directory.Exists(FilePath))
            Directory.CreateDirectory(FilePath);

        FilePath = Path.Combine(FilePath, FileName);

        try
        {
            if ((!thisFile.HasFile) || (thisFile.PostedFile.ContentLength == 0))
            {
                new Exception("กรุณาเลือกไฟล์ข้อมูล Excel");
            }
            else if ((Extension != ".xls") && (Extension != ".xlsx")) //Check Type Excel File
            {
                new Exception("ไฟล์ข้อมูล Excel ต้องเป็นนามสกุล .xls, .xlsx");
            }
            else
            {
                thisFile.SaveAs(FilePath);
                _dtExcel = GetExcelData(FilePath, Extension);
            }
        }
        catch (Exception ex)
        {
            Logging = new Prototype.Providers.Logging(this, ex);
            RaiseLogging();
            throw ex;
        }
        finally
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }

        return _dtExcel;
    }
}
