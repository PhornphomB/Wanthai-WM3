using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace WMS_NEW.Transaction.Inventory
{
    public partial class InventoryChangeExpDate : PageCustom
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
                    ddlReasonCode.MethodQueryProperty = delegate () { return Access.MasterData.Reason.Instance.GetQuery("ChangeExpiryDate"); };
                    ddlReasonCode.BindDataSource();

                    hid_control_exp_date.SetValue(true);
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
                List<Guid> listKey = GridExt1.GetListKey().Where(w => w.Active == true).Select(s => Guid.Parse(s.KeyId.ToString())).ToList();
                if (listKey == null || listKey.Count == 0)
                {
                    Page.MessageWarning("Please select item.");
                    return;
                }

                //Initial Control
                txtExpDate.Clear();
                txtMfgDate.Clear();
                txtMfgDate.IsPrimary = false;

                txtQty.Clear();
                ddlReasonCode.Clear();
                txtRemark.Clear();
                txtQty.Enabled = false;
                txtQty.IsPrimary = txtQty.Enabled;

                this.is_multiple_item = listKey.Count > 1 ? true : false;

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

                if (listKey.Count > 1)
                {
                    txtQty.Enabled = false;
                    txtQty.IsPrimary = txtQty.Enabled;

                    lblQty.InnerText = "Select multiple item, Can not enter quantity.";
                }
                else if (listKey.Count == 1)
                {
                    using (var acc = new Access.MasterData.Item())
                    {
                        txtQty.Enabled = true;
                        txtQty.IsPrimary = txtQty.Enabled;


                        Guid inventory_id = listKey[0];
                        var ent = acc._Model.v_wms_inventory_data.Where(w => w.inventory_id == inventory_id).FirstOrDefault();
                        if (ent != null)
                        {
                            txtExpDate.SetValue(ent.exp_date);
                            txtQty.SetValue(ent.quantity);
                            Guid item_master_id = ent.item_master_id;
                            this.control_serial = acc.IsControlSerial(item_master_id);
                            if (this.control_serial)
                            {
                                //ถ้า Item control serial ห้ามใส่จำนวน
                                txtQty.Enabled = false;
                                txtQty.IsPrimary = txtQty.Enabled;

                                lblQty.InnerText = "Item control serial, Can not enter quantity.";
                            }
                        }
                    }
                }

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
                using (var acc = new Access.Transaction.Inventory.InventoryChange())
                {
                    PlugEventResult(acc);
                    List<Guid> listKey = GridExt1.GetListKey().Where(w => w.Active == true).Select(s => Guid.Parse(s.KeyId.ToString())).ToList();

                    string exp_date = txtExpDate.GetValue() != null ? txtExpDate.GetValue().Value.ToString("yyyyMMdd") : string.Empty;
                    DateTime? mfg_date = txtMfgDate.GetValue();
                    double? qty = txtQty.GetValue();
                    Guid reason_id = ddlReasonCode.GetValue();
                    string remark = txtRemark.GetValue();

                    bool isSuccess = acc.Change_ExpiryDate(listKey, mfg_date, exp_date, qty, reason_id, remark);
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