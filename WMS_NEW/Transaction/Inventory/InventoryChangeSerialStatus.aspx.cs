using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace WMS_NEW.Transaction.Inventory
{
    public partial class InventoryChangeSerialStatus : PageCustom
    {
        #region ViewState

        private bool control_serial
        {
            get
            {
                if (ViewState["control_serial"] == null)
                    ViewState["control_serial"] = false;

                return (bool)ViewState["control_serial"];
            }
            set
            {
                ViewState["control_serial"] = value;
            }
        }
        private bool is_multiple_item
        {
            get
            {
                if (ViewState["is_multiple_item"] == null)
                    ViewState["is_multiple_item"] = false;

                return (bool)ViewState["is_multiple_item"];
            }
            set
            {
                ViewState["is_multiple_item"] = value;
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

                if (!Page.IsPostBack)
                {
                    ddlInvStatus.MethodQueryProperty = delegate () { return Access.Transaction.Inventory.InventoryStatus.Instance.GetQuery(); };
                    ddlInvStatus.BindDataSource();

                    ddlReasonCode.MethodQueryProperty = delegate () { return Access.MasterData.Reason.Instance.GetQuery("ChangeStatus"); };
                    ddlReasonCode.BindDataSource();
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
                    List<string> listKey = GridExt1.GetListKey().Where(w => w.Active == true).Select(s => s.KeyId.ToString()).ToList();
                    Guid inv_status_id = ddlInvStatus.GetValue();
                    Guid reason_id = ddlReasonCode.GetValue();
                    string remark = txtRemark.GetValue();

                    if (acc.Serial_Change_Status(listKey, inv_status_id, 1, reason_id, remark))
                        GridExt1.Search();
                        //GridExt1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> listKey = GridExt1.GetListKey().Where(w => w.Active == true).Select(s => s.KeyId.ToString()).ToList();
                if (listKey == null || listKey.Count == 0)
                {
                    Page.MessageWarning("Please select item.");
                    return;
                }


                ddlInvStatus.Clear();
                ddlReasonCode.Clear();
                txtRemark.Clear();
                popupChange.ShowDialog();

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }
}