using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Inventory
{
    public partial class InventoryChangeSerial : PageCustom
    {
        #region ViewState

        private Guid inventory_id
        {
            get
            {
                if (ViewState["inventory_id"] == null)
                    ViewState["inventory_id"] = Guid.Empty;

                return (Guid)ViewState["inventory_id"];
            }
            set
            {
                ViewState["inventory_id"] = value;
            }
        }

        private string serial_number
        {
            get
            {
                if (ViewState["serial_number"] == null)
                    ViewState["serial_number"] = string.Empty;

                return (string)ViewState["serial_number"];
            }
            set
            {
                ViewState["serial_number"] = value;
            }
        }

        Guid grid_wh_master_id
        {
            get
            {
                if (ViewState["grid_wh_master_id"] == null)
                    ViewState["grid_wh_master_id"] = Guid.Empty;

                return (Guid)ViewState["grid_wh_master_id"];
            }
            set
            {
                ViewState["grid_wh_master_id"] = value;
            }
        }

        string grid_zone
        {
            get
            {
                if (ViewState["grid_zone"] == null)
                    ViewState["grid_zone"] = string.Empty;

                return (string)ViewState["grid_zone"];
            }
            set
            {
                ViewState["grid_zone"] = value;
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Grid Column

                iColWarehouse.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(); };
                //iColWarehouse.DropDownAutoPostBack = true;
                //iColWarehouse.DropDownPostValueChanged = delegate (dynamic _value)
                //{
                //    this.grid_wh_master_id = _value ?? Guid.Empty;

                //    GridExt1.GridColumnRefreshFilter(iColZone);
                //};

                iColZone.DropDownQueryProperty = delegate () { return Access.MasterData.Zone.Instance.GetQueryCode_UserWarehouse(); };
                //iColZone.DropDownAutoPostBack = true;
                //iColZone.DropDownPostValueChanged = delegate (dynamic _value)
                //{
                //    this.grid_zone = _value;

                //    GridExt1.GridColumnRefreshFilter(iColLocation);
                //};

                //iColLocation.DropDownQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQuery_UserWarehouse(); };
                iColLocation.DropDownQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetCodeQueryUserWarehouseRuleLocNotMove(); };

                iColOwner.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery_User(); };

                iColItem.DropDownQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQuery(); };
                iColCategory.DropDownQueryProperty = delegate () { return Access.MasterData.ItemCategory.Instance.GetQuery(); };
                iColInvStatus.DropDownQueryProperty = delegate () { return Access.Transaction.Inventory.InventoryStatus.Instance.GetQuery(); };

                #endregion

                GridExt1.GridRowCommandClick += GridExt1_GridRowCommandClick;
                GridExt1.GridRowAfterDataBound += GridExt1_GridRowAfterDataBound;

                if (!Page.IsPostBack)
                {

                    ddlReasonCode.MethodQueryProperty = delegate () { return Access.MasterData.Reason.Instance.GetQuery("ChangeSerial"); };
                    ddlReasonCode.BindDataSource();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExt1_GridRowAfterDataBound(System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {
                var btnChange = (Button)e.Row.FindControl("change_serial");
                btnChange.CssClass = "btn btn-sm btn-warning btn-ingrid";

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExt1_GridRowCommandClick(System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                popupChange.ShowDialog();

                //Initial Control
                txtRemark.Clear();

                string key_id = (string)e.CommandArgument;
                using (var acc = new Access.Transaction.Inventory.InventoryChangeSerial())
                {
                   var ent =  acc._Model.v_wms_inventory_data_by_serial.Where(w => w.key_id == key_id).FirstOrDefault();
                    if (ent != null)
                    {
                        this.serial_number = ent.serial_number;
                        this.inventory_id = ent.inventory_id;
                        txtWarehouse.SetValue(ent.wh_id);
                        txtItem.SetValue(ent.item_number);
                        txtDescription.SetValue(ent.description);
                        txtSerial.SetValue(ent.serial_number);
                        txtSerialNew.Clear();

                        txtWarehouse.Update();
                        txtItem.Update();
                        txtDescription.Update();
                        txtSerial.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btnConfirmChange_Click(object sender, EventArgs e)
        {
            try
            {
                using (var acc = new Access.Transaction.Inventory.InventoryChangeSerial())
                {
                    PlugEventResult(acc);
                    string serial_new = txtSerialNew.GetValue();
                    Guid reason_id = ddlReasonCode.GetValue();
                    string remark = txtRemark.GetValue();


                    bool isSuccess = acc.Serial_Change(this.inventory_id, this.serial_number, serial_new, reason_id, remark);
                    if (isSuccess)
                    {
                        popupChange.HideDialog();
                        //GridExt1.DataBind();
                        GridExt1.Search();
                    }

                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btnChangeLocation_Click()
        {

        }
    }
}