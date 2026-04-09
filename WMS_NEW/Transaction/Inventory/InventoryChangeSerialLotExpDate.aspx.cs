using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace WMS_NEW.Transaction.Inventory
{
    public partial class InventoryChangeSerialLotExpDate : PageCustom
    {
        #region ViewState

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
                    ddlReasonCode.MethodQueryProperty = delegate () { return Access.MasterData.Reason.Instance.GetQuery("ChangeExpiryDate"); };
                    ddlReasonCode.BindDataSource();

                    hid_control_lot_exp_date.SetValue(true);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btnChangeExpDate_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> listKey = GridExt1.GetListKey().Where(w => w.Active == true).Select(s => s.KeyId.ToString()).ToList();
                if (listKey == null || listKey.Count == 0)
                {
                    Page.MessageWarning("Please select item.");
                    return;
                }

                //Initial Control
                txtLot.Clear();
                txtExpDate.Clear();
                txtMfgDate.Clear();
                ddlReasonCode.Clear();
                txtRemark.Clear();

                txtLot.IsPrimary = false;
                txtMfgDate.IsPrimary = false;
                txtExpDate.IsPrimary = false;

                txtLot.Enabled = false;
                txtMfgDate.Enabled = false;
                txtExpDate.Enabled = false;


                //Validate
                #region Validate
                var ent = new Source.v_wms_inventory_data_by_serial();
                using (var acc = new Access.Transaction.Inventory.InventoryChangeSerial())
                {
                    bool is_MatchLotExp = acc.ValidateControlLotExpDate(listKey,out ent);
                    if (!is_MatchLotExp)
                    {
                        Page.MessageWarning("Lot Number and Expirydate not match !");
                        return;
                    }
                }

                if (ent.lot_control.ToUpper() == "FULL")
                {
                    txtLot.IsPrimary = true;
                    txtLot.Enabled = true;

                    txtLot.SetValue(ent.lot_number);
                }

                if (ent.expiry_date_control.ToUpper() == "FULL")
                {
                    using (var accRule = new Access.Configuration.Rule())
                    {
                        if (accRule.Is_Rule("RULE_REQUIRE_MFG_DATE", "YES"))
                        {
                            txtMfgDate.Enabled = true;
                            txtMfgDate.IsPrimary = true;
                        }
                        else
                        {
                            txtMfgDate.Enabled = false;
                            txtMfgDate.IsPrimary = false;
                        }
                    }
                    txtExpDate.IsPrimary = true;
                    txtExpDate.Enabled = true;

                    txtMfgDate.SetValue(ent.mfg_date);
                    txtExpDate.SetValue(ent.exp_date);
                }
                #endregion

                popupChange.ShowDialog();
                popupChange.UpdateContent();
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
                    List<string> listKey = GridExt1.GetListKey().Where(w => w.Active == true).Select(s =>s.KeyId.ToString()).ToList();

                    string exp_date = txtExpDate.GetValue() != null ? txtExpDate.GetValue().Value.ToString("yyyyMMdd") : string.Empty;
                    DateTime? mfg_date = txtMfgDate.GetValue();
                    string lot_number = txtLot.GetValue() != null ? txtLot.GetValue() : string.Empty;

                    Guid reason_id = ddlReasonCode.GetValue();
                    string remark = txtRemark.GetValue();

                    bool isSuccess = acc.Serial_Change_Lot_Exp(listKey, mfg_date, exp_date,lot_number,1, reason_id, remark);
                    if (isSuccess)
                    {
                        popupChange.HideDialog();
                        GridExt1.Search();
                       // GridExt1.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


    }

}