using _UControls;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Source;

namespace WMS_NEW.Transaction.Inbound
{
    public partial class InboundPrintLabel : PageCustom
    {
        #region ++ DELEGATE ++
        delegate void dg_Search();
        event dg_Search eSearch;
        #endregion
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
                #region Deledate Call Back
                eSearch += new dg_Search(Get_Summary);
                ucInboundPrintLabelDetail.dg_CallBackSearch = eSearch;
                #endregion
                #region Form Page
                ddlWarehouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
                
                ddlOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                ddlOrderType.MethodQueryProperty = delegate () { return Access.Transaction.Inbound.InboundMaster.Instance.GetQueryCode("inbound_order_type", null); };
                #endregion
                #region Grid Column
                iColWarehouse.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
                //iColOwner.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                iColOrderType.DropDownQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("inbound_order_type"); };
                iColOrderStatus.DropDownQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("inbound_order_status"); };
                //iColSupplier.DropDownQueryProperty = delegate () { return Access.MasterData.Supplier.Instance.GetQuery(); };
                iColCustomer.DropDownQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQuery(); };
                //iColProductionLine.DropDownQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("production_line"); };

                if (ddlWarehouse.MethodQueryProperty().ToList().Count == 1)
                {
                    this.wh_master_id = ddlWarehouse.MethodQueryProperty().Select(w => w.guid_member).FirstOrDefault();
                }

                iColProductionLine.DropDownQueryProperty = delegate () { return Access.MasterData.ProductionLine.Instance.GetQuery(this.wh_master_id); };
                #endregion

                #region Grid Custom
                ddlCategory.MethodQueryProperty = delegate () { return new Access.MasterData.ItemCategory().GetQuery(); };
                //ddOrderType.MethodQueryProperty = delegate () { return Access.Transaction.Inbound.InboundMaster.Instance.GetQueryCode("inbound_order_type", Request.QueryString["pagetype"]); };
                #endregion

                #region Init PopupEntity 

                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = new Access.Transaction.Inbound.InboundMaster(); };
                popupEntity1.InitControlStatic();

                GridExt1.PopupEntitySource = popupEntity1;
                GridExt1.GridPreExportData += GridExt1_GridPreExportData;
                popupEntity1.AfterSetEditDataEvent += PopupEntity1_AfterSetEditDataEvent;
                iColWarehouse.DropDownPostValueChanged = iColWarehouse_DropDownPostValueChanged;
                #endregion

                if (!Page.IsPostBack)
                {
                    #region Set Initial Filter Grid

                    var ucFirstLoad = new _UControls.InputHidden() { DataFieldValue = "_is_first_load" };
                    ucFirstLoad.SetValue(true);
                    GridExt1.AddFilterCustomInputInclude(ucFirstLoad);
                    txtMFGDate.SetValue(DateTime.Now);
                    //GridColumnExtLite lite = new GridColumnExtLite() { DataField = "create_date" };
                    //GridExt1.GridFilterSetValue(lite, DateTime.Now);
                    txtPageType.SetValue(Request.QueryString["pagetype"]);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void PopupEntity1_AfterSetEditDataEvent()
        {
            Get_Summary();
        }

        private void GridExt1_GridPreExportData(ref System.Data.DataTable dtExport)
        {
            
            if (dtExport.Rows.Count > 0)
            {
                foreach (DataRow row in dtExport.Rows)
                {
                    row["item_number_excel"] = row["item_number_excel"].ToString().Replace("<br>", "\r\n");
                }
            }
        }

        private void iColWarehouse_DropDownPostValueChanged(dynamic _value)
        {
            if (_value != null)
            {
                this.wh_master_id = _value;
            }
        }
        void Get_Summary()
        {
            using (var _acc = new WMS_NEW.Access.Transaction.Inbound.InboundDetail())
            {
                base.PlugEventResult(_acc);

                //var result = _acc.Get_InboundDetailSummary((Guid)popupEntity1.KeyFieldValue);
                var order_result = _acc.Get_OrderSummary((Guid)popupEntity1.KeyFieldValue);
                var receive_result = _acc.Get_ReceiveSummary((Guid)popupEntity1.KeyFieldValue);
                var print_result = _acc.Get_PrintSummary((Guid)popupEntity1.KeyFieldValue);
                if (order_result != null)
                {
                    lblSumPlanQTY.InnerText = order_result.PlanQuantity == 0 ? "0" : order_result.PlanQuantity.ToString(Extensions.FormatDecimal);
                }
                else
                {
                    lblSumPlanQTY.InnerText = "0";
                }
                if (receive_result != null)
                {
                    lblSumReceiveQTY.InnerText = receive_result.ReceiveQuantity == 0 ? "0" : receive_result.ReceiveQuantity.ToString(Extensions.FormatDecimal);
                }
                else
                {
                    lblSumReceiveQTY.InnerText = "0";
                }
                if (print_result != null)
                {
                    lblSumPrintQTY.InnerText = print_result.PrintQuantity == 0 ? "0" : print_result.PrintQuantity.ToString(Extensions.FormatDecimal);
                }
                else
                {
                    lblSumPrintQTY.InnerText = "0";
                }
                update_summary.Update();
            }
        }
    }
}