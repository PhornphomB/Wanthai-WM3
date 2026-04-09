using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace WMS_NEW.Report.AscxControls
{
    public partial class ucReportViewer : UControlCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                GridExt1.GridRowClick += GridExt1_GridRowClick;

                popReportViewer.CloseClick += popReport_CloseClick;

                if (!Page.IsPostBack)
                {

                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void InitialForm(string _form_name)
        {
            hid_form_name.SetValue(_form_name);
            GridExt1.Search();

            ShowDialog();
        }

        public void ShowDialog()
        {
            popReport.ShowDialog();
        }

        public void HideDialog()
        {
            popReport.HideDialog();
        }

        void popReport_CloseClick(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        public event BindingParamsReport BindingParameter;

        public void ViewReportNow(string _report_id)
        {
            var prmUrl = "&";
            if (BindingParameter != null)
            {
                var prms = BindingParameter(_report_id.ToUpper());
                foreach (var prm in prms)
                {
                    prmUrl += prm.Name + "=" + prm.Value + "&";
                }
            }
            prmUrl = prmUrl.TrimEnd('&');

            Guid report_id = Guid.Parse(_report_id);
            using (var acc = new Access.Report.ReportViewer())
            {
                var entRpt = acc._Model.t_com_report_manager.Where(w => w.report_id == report_id).FirstOrDefault();
                if (entRpt != null)
                {
                    popReportViewer.HeaderText = entRpt.report_name;
                    popReportViewer.ShowDialog();

                    if (entRpt.report_type.ToUpper() == "RPT")
                    {
                        ifrmReport.Attributes.Add("src", ConfigurationManager.AppSettings["ReportUrl"]
                            + "&_app_Reporttitle=" + entRpt.report_name + "(" + entRpt.report_file_name + ")"
                            + "&_app_Reportpath=" + entRpt.report_file_name
                            + "&" + ConfigurationManager.AppSettings["ReportConnection"] + prmUrl);
                    }
                    else //RDL
                    {
                        ifrmReport.Attributes.Add("src", ConfigurationManager.AppSettings["ReportUrlRDL"]
                         + "&_app_Reporttitle=" + entRpt.report_name + "(" + entRpt.report_file_name + ")"
                         + "&_app_Reportpath=" + entRpt.report_file_name
                         + "&" + ConfigurationManager.AppSettings["ReportConnection"] + prmUrl);
                    }
                }
            }
        }

        void GridExt1_GridRowClick(object _rowKeyValue)
        {
            try
            {
                this.ViewReportNow(_rowKeyValue.ToString());
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        #region Properties for Parent Page

        #endregion
    }

    public class ReportParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public delegate List<ReportParameter> BindingParamsReport(string _report_id);

}