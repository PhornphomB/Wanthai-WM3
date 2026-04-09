using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Outbound {
    public partial class AssignOrder : PageCustom {

        Access.Transaction.Outbound.AssignOrder _assignOrder = null;

        protected void Page_Load(object sender, EventArgs e) {
            try {
                _assignOrder = new Access.Transaction.Outbound.AssignOrder();
                #region Binging DropDown Property Column Grid

                iColWarehouse.DropDownQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };
                iColOwner.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                iColOrderStatus.DropDownQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("outbound_order_status"); };
                iColOrderType.DropDownQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryRule("RULE_CHECKER_ORDER_TYPE"); };

                #endregion Binging DropDown Property Column Grid                    

                GridExt1.GridRowEdit += GridExt1_GridRowEdit;

                #region Binging DropDown Property Input Data

                if (!Page.IsPostBack) {
                   
                }

                #endregion Binging DropDown Property Input Data

                popupAssignOrder.InitObjectsEvent += () => { popupAssignOrder.ObjectDataAccess = _assignOrder; };
                popupAssignOrder.InitControlStatic();

                GridExt1.PopupEntitySource = popupAssignOrder;
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExt1_GridRowEdit(object _rowKeyValue) {
            try {
                var id = Guid.Parse(_rowKeyValue.ToString());
                var result = this._assignOrder.GetOrderHeader(id);
                InputTextBox_wh_id.SetValue(result.wh_id);
                InputTextBox_outbound_order_number.SetValue(result.outbound_order_number);
                InputTextBox_customer_code.SetValue(result.customer_code);
                InputTextBox_owner_code.SetValue(result.owner_code);
                InputTextBox_order_status.SetValue(result.order_status);
                InputTextBox_sum_quantity_order.SetValue(result.sum_quantity_order.ToString());
                InputTextBox_sum_quantity_pick.SetValue(result.sum_quantity_pick.ToString());
                InputTextBox_sum_quantity_stage.SetValue(result.sum_quantity_stage.ToString());
                InputTextBox_sum_quantity_load.SetValue(result.sum_quantity_load.ToString());
                InputTextBox_sum_quantity_ship.SetValue(result.sum_quantity_ship.ToString());
                //ปิดปุ่ม Save
                if (result.order_status == "SHIP" || result.order_status == "CANCEL" || result.order_status == "CLOSE") {
                    btSaveAssginOrder.Enabled = false;
                } else {
                    btSaveAssginOrder.Enabled = true;
                }             
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btSaveAssginOrder_Click(object sender, EventArgs e) {
            try {
                AssignOrderDetail1.SaveAssignOrder();
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

    }
}