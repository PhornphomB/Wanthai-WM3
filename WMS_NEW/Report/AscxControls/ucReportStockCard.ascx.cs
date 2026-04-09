using System;
using System.Collections.Generic;
using System.Web.UI;

namespace WMS_NEW.Report.AscxControls
{
    public partial class ucReportStockCard : UControlCustom
    {
        public Guid wh_master_id
        {
            get
            {
                return (Guid)ViewState["wh_master_id"];
            }
            set
            {
                ViewState["wh_master_id"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ddlWarehouse.PostValueChanged += ddlWarehouse_PostValueChanged;
                ucReportViewer.BindingParameter += UcReportViewer_BindingParameter;

                ddlWhItemMaster.MethodQueryProperty = delegate () { return Access.MasterData.Mapping.WarehouseItem.Instance.GetQuery(this.wh_master_id); };

                if (!Page.IsPostBack)
                {
                    ddlWarehouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(); };
                    ddlWarehouse.BindDataSource();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void ShowDialog()
        {
            popReport.ShowDialog();
            ddlWarehouse.Clear();
            ddlWhItemMaster.Clear();
            txtDateStart.Clear();
            txtDateEnd.Clear();
        }

        private void ddlWarehouse_PostValueChanged(dynamic obj)
        {
            try
            {
                if (obj == null)
                {
                    return;
                }

                this.wh_master_id = obj;

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                ucReportViewer.ViewReportNow("267e5401-d6db-459b-9625-9a282e1c1c97");
                popReport.HideDialog();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private List<ReportParameter> UcReportViewer_BindingParameter(string _report_id)
        {
            try
            {
                string formamt = "yyyyMMdd";
                var prms = new List<ReportParameter>();
                prms.Add(new ReportParameter { Name = "@in_vchWhItemMasterID", Value = ddlWhItemMaster.GetValue().ToString() });
                prms.Add(new ReportParameter { Name = "@in_vchStartDate", Value = txtDateStart.GetValue().Value.ToString(formamt) });
                prms.Add(new ReportParameter { Name = "@in_vchEndDate", Value = txtDateEnd.GetValue().Value.ToString(formamt) });

                return prms;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}