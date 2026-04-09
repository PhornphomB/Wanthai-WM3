using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Report.AscxControls;

namespace WMS_NEW.Export
{
    public partial class LabelPrinted : PageCustom
    {
        private Guid wh_master_id
        {
            get
            {
                if (ViewState["wh_master_id"] == null)
                    ViewState["wh_master_id"] = Guid.Empty;

                return (Guid)ViewState["wh_master_id"];
            }
            set
            {
                ViewState["wh_master_id"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Initial Peoperty Column Grid

            GridColumnExt00.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
            GridColumnExt06.DropDownQueryProperty = delegate () { return Access.MasterData.ProductionLine.Instance.GetQuery(this.wh_master_id); };
            GridColumnExt07.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty_Display("inbound_order_type"); };
            GridColumnExt14.DropDownQueryProperty = delegate () { return Access.Transaction.Inventory.InventoryStatus.Instance.GetQueryCode_Inbound(); };

            if (GridColumnExt00.DropDownQueryProperty().ToList().Count == 1)
            {
                this.wh_master_id = GridColumnExt00.DropDownQueryProperty().Select(w => w.guid_member).FirstOrDefault();
            }

            GridColumnExt00.DropDownPostValueChanged = GridColumnExt00_DropDownPostValueChanged;

            ucReportViewer.BindingParameter += UcReportViewer_BindingParameter;

            #endregion

            if (!Page.IsPostBack)
            {
            }

        }
        private void GridColumnExt00_DropDownPostValueChanged(dynamic _value)
        {
            if (_value != null)
            {
                this.wh_master_id = _value;
            }
        }
        #region Report
        protected void btReport_Click(object sender, EventArgs e)
        {
            try
            {
                ucReportViewer.ShowDialog();
                ucReportViewer.InitialForm("LabelPrinted");

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private List<Report.AscxControls.ReportParameter> UcReportViewer_BindingParameter(string _report_id)
        {
            var prms = new List<ReportParameter>();

            GridExt1.SetGridFilterForSearch();
            var filters = GridExt1.FilterGridOptions;
            GridExt1.SetCustomFilterForSearch();

            var _condition = Prototype.Providers.DataFilterSQL.GetSQLCondition(filters).Replace("'", "*").Replace("=", "$").Replace("%", "[tnecrep]").Replace("#", "%23");
            if (!string.IsNullOrEmpty(_condition))
            {
                _condition = " AND " + _condition;
            }
            prms.Add(new ReportParameter() { Name = "@prmCondition", Value = _condition });
            prms.Add(new ReportParameter() { Name = "@user_id", Value = _SessionVals.UserName });
            return prms;
        }
        #endregion
    }
}