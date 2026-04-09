using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Microsoft.Reporting.WebForms;
using Microsoft.SqlServer.Server;
using Neodynamic.SDK.Web;
using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.WebPrint
{
    /// <summary>
    /// Summary description for PrintRPTHandler
    /// </summary>
    public class PrintRPTHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (WebClientPrint.ProcessPrintJob(context.Request.Url.Query))
            {
                try
                {
                    string strRptName = "";
                    if (context.Request["reportName"] != null)
                        strRptName = ConfigurationManager.AppSettings["FolderRDL"] + context.Request["reportName"].ToString();

                    List<DTOPrintLabel> _printLabels = new List<DTOPrintLabel>();

                    if (context.Request["plid"] != "undefined" && context.Request["plid"] != null)
                    {
                        string[] rows = context.Request["plid"].Split(new string[] { "][" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string row in rows)
                        {
                            string rowWithoutBrackets = row.Trim('[', ']');
                            string[] values = rowWithoutBrackets.Split(',');
                            using(var _model = new WMSEntities())
                            {
                                if (context.Request["reportName"].ToString() == "rptPrintLabel.rdl")
                                {
                                    string item_number = values[4];
                                    DTOPrintLabel dTOPrintLabel = new DTOPrintLabel
                                    {
                                        print_label_id = values[0],
                                        qr_format = values[1],
                                        lpn = values[2],
                                        item_description = _model.t_wms_item.FirstOrDefault(x => x.item_number == item_number)?.description ?? item_number,
                                        item_number = values[4],
                                        attribute1 = values[5],
                                        mfg_date = ConvertStringToDatetime(values[6]),
                                        expiry_date = ConvertStringToDatetime(values[7]),
                                        print_date = ConvertStringToDatetime(values[8]),
                                        pack_size_per_pallet = values[9],
                                        pack_size_uom = values[10],
                                        row_print = values[11],
                                        line_number = values[12],
                                        pallet_seq = values[13],
                                        production_line = values[14],
                                        model = values[15]
                                    };
                                    _printLabels.Add(dTOPrintLabel);
                                }
                                else if (context.Request["reportName"].ToString() == "rptRePrintLabel.rdl")
                                {
                                    string item_number = values[2];
                                    DTOPrintLabel dTOPrintLabel = new DTOPrintLabel
                                    {
                                        qr_format = values[0],
                                        lpn = values[1],
                                        item_number = values[2],
                                        item_description = _model.t_wms_item.FirstOrDefault(x => x.item_number == item_number)?.description ?? item_number,
                                        attribute1 = values[4],
                                        mfg_date = ConvertStringToDatetime(values[5]),
                                        expiry_date = ConvertStringToDatetime(values[6]),
                                        print_date = ConvertStringToDatetime(values[7]),
                                        pack_size_per_pallet = values[8],
                                        pack_size_uom = values[9],
                                        pallet_seq = values[10],
                                        production_line = values[11],
                                        model = values[12]
                                    };
                                    _printLabels.Add(dTOPrintLabel);
                                }
                            }
                        }
                    }

                    bool useDefaultPrinter = (context.Request["useDefaultPrinter"] == "checked");
                    string printerName = context.Server.UrlDecode(context.Request["printerName"]);

                    // Setup the report viewer object and get the array of bytes   

                    ReportViewer viewer = new ReportViewer();
                    viewer.ProcessingMode = ProcessingMode.Local;
                    viewer.SizeToReportContent = true;
                    viewer.LocalReport.ReportPath = context.Server.MapPath(strRptName);
                    if (context.Request["reportName"].ToString() == "rptPrintLabel.rdl")
                        viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", _printLabels.OrderBy(o => o.row_print).Select(s => new { s.print_label_id, s.qr_format, s.lpn, s.item_description, s.item_number, s.attribute1, s.mfg_date, s.expiry_date, s.print_date, s.pack_size_per_pallet, s.pack_size_uom, s.row_print, s.line_number, s.pallet_seq, s.production_line, s.model }).ToList().ToDataTable())); // Add datasource here
                    if (context.Request["reportName"].ToString() == "rptRePrintLabel.rdl")
                        viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", _printLabels.Select(s => new { s.qr_format, s.lpn, s.item_number, s.item_description, s.attribute1, s.mfg_date, s.expiry_date, s.print_date, s.pack_size_per_pallet, s.pack_size_uom, s.pallet_seq, s.production_line, s.model }).ToList().ToDataTable())); // Add datasource here

                    // Variables   
                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;

                    // Setup the report viewer object and get the array of bytes   
                    byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                    //create a temp file name for our PDF file...
                    string fileName = "LabelSticker-" + Guid.NewGuid().ToString("N");

                    //Create a PrintFilePDF object with the PDF file
                    PrintFilePDF file = new PrintFilePDF(bytes, fileName);
                    //Create a ClientPrintJob and send it back to the client! 

                    //Set license info...
                    WebClientPrint.LicenseOwner = "OGA International Co Ltd - 1 WebApp Lic - 1 WebServer Lic";
                    WebClientPrint.LicenseKey = "DE19B2D8963F5B1825CFD643AADF6932B1A4CA74";

                    ClientPrintJob cpj = new ClientPrintJob();
                    //set file to print...
                    cpj.PrintFile = file;

                    //set client printer...
                    if (useDefaultPrinter || printerName == "null")
                        cpj.ClientPrinter = new DefaultPrinter();
                    else
                        cpj.ClientPrinter = new InstalledPrinter(printerName);

                    cpj.PrinterCommandsCopies = 1;

                    //send it...            
                    context.Response.ContentType = "application/octet-stream";
                    var byteContent = cpj.GetContent();
                    context.Response.BinaryWrite(byteContent);
                    //context.Response.End();
                }
                catch (Exception ex)
                {
                    LoggingFile.WriteTextLogging(ex.Message, LoggingFile.LogFileName(this.GetType().ToString()));
                    if(ex.InnerException != null)
                    {
                        LoggingFile.WriteTextLogging(ex.InnerException.Message, LoggingFile.LogFileName(this.GetType().ToString()));
                    }
                }
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public class DTOPrintLabel
        {
            public string MyProperty { get; set; }
            public string print_label_id { get; set; }
            public string qr_format { get; set; }
            public string lpn { get; set; }
            public string item_description { get; set; }
            public string item_number { get; set; }
            public string attribute1 { get; set; }
            public DateTime mfg_date { get; set; }
            public DateTime expiry_date { get; set; }
            public DateTime print_date { get; set; }
            public string pack_size_per_pallet { get; set; }
            public string pack_size_uom { get; set; }
            public string row_print { get; set; }
            public string line_number { get; set; }
            public string pallet_seq { get; set; }
            public string production_line { get; set; }
            public string model { get; set; }
        }
        public DateTime ConvertStringToDatetime(string DateString)
        {
            try
            {
                return Convert.ToDateTime(DateString);
            }
            catch (FormatException)
            {
                try
                {
                    string format = ConfigurationManager.AppSettings["LocalDatetimeFormat"];
                    DateTime dateTime = DateTime.ParseExact(DateString, format, CultureInfo.InvariantCulture);
                    return dateTime;
                }
                catch(FormatException)
                {
                    return DateTime.MinValue;
                }
            }
            
        }
    }
}