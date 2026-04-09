using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Inventory
{
    public partial class InventoryChangeLocation : PageCustom
    {
        #region ViewState
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

        private bool control_lpn
        {
            get
            {
                if (ViewState["control_lpn"] == null)
                    ViewState["control_lpn"] = false;

                return (bool)ViewState["control_lpn"];
            }
            set
            {
                ViewState["control_lpn"] = value;
            }
        }

        private bool control_parent_lpn
        {
            get
            {
                if (ViewState["control_parent_lpn"] == null)
                    ViewState["control_parent_lpn"] = false;

                return (bool)ViewState["control_parent_lpn"];
            }
            set
            {
                ViewState["control_parent_lpn"] = value;
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

                ddlWarehouse.PostValueChanged = ddlWarehouse_PostValueChanged;
                ddlLocation.PostValueChanged = ddlLocation_PostValueChanged;
                chkParentLPN.PostValueChanged = chkParentLPN_PostValueChanged;
                chkLPN.PostValueChanged = chkLPN_PostValueChanged;

                ddlLocation.MethodQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQuery_Change(this.wh_master_id); };

                if (!Page.IsPostBack)
                {
                    ddlWarehouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
                    ddlWarehouse.BindDataSource();

                    ddlReasonCode.MethodQueryProperty = delegate () { return Access.MasterData.Reason.Instance.GetQuery("ChangeLocation"); };
                    ddlReasonCode.BindDataSource();

                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void ddlWarehouse_PostValueChanged(dynamic obj)
        {
            try
            {
                if (obj == null)
                {
                    return;
                }
                ddlLocation.Clear();
                this.wh_master_id = obj;

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void ddlLocation_PostValueChanged(dynamic obj)
        {
            try
            {
                if (obj == null)
                {
                    return;
                }

                //chkLPN.Checked = false;
                txtLPN.Clear();
                lblLpn.InnerText = string.Empty;
                using (var acc = new Access.MasterData.Location())
                {
                    this.control_lpn = acc.Control_LPN(obj);
                    if (this.control_lpn)
                    {
                        chkLPN_PostValueChanged(true);
                        chkLPN.Enabled = true;
                        chkLPN.Checked = true;
                        lblLpn.InnerText = "Loction control lpn.";
                    }
                    else
                    {
                        chkLPN_PostValueChanged(false);
                        chkLPN.Checked = false;
                        chkLPN.Enabled = false;
                        txtLPN.IsPrimary = false;
                        txtLPN.Enabled = false;
                    }
                }

                popupChange.UpdateContent();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void chkParentLPN_PostValueChanged(dynamic obj)
        {
            try
            {
                if (obj)
                {
                    txtParentLPN.Enabled = false;
                }
                else
                {
                    txtParentLPN.Enabled = true;
                }

                if (this.control_parent_lpn && obj == false)
                {
                    txtParentLPN.IsPrimary = true;
                }

                popupChange.UpdateContent();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void chkLPN_PostValueChanged(dynamic obj)
        {
            try
            {
                if (obj)
                {
                    txtLPN.Enabled = false;
                    txtQty.Enabled = false;
                    txtQty.IsPrimary = false;
                }
                else
                {
                    txtLPN.Enabled = true;
                    txtQty.Enabled = true;
                    txtQty.IsPrimary = true;
                }

                if (this.control_lpn && obj == false)
                {
                    txtLPN.IsPrimary = true;
                }
                else if (this.control_lpn && obj == true)
                {
                    txtLPN.IsPrimary = false;
                }

                if (this.control_serial || this.is_multiple_item)
                {
                    txtQty.Enabled = false;
                    txtQty.IsPrimary = false;

                }

                popupChange.UpdateContent();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }



        protected void btnChangeLocation_Click(object sender, EventArgs e)
        {
            try
            {
                List<Guid> listKey = GridExt1.GetListKey().Where(w => w.Active == true).Select(s => Guid.Parse(s.KeyId.ToString())).ToList();
                if (listKey == null || listKey.Count == 0)
                {
                    Page.MessageWarning("Please select item.");
                    return;
                }

                this.is_multiple_item = listKey.Count > 1 ? true : false;

                //Initial Control
                chkParentLPN.Enabled = false;
                txtParentLPN.Enabled = false;


                chkLPN.Enabled = false;
                txtLPN.Enabled = false;
                lblLpn.InnerText = string.Empty;

                txtQty.Enabled = true;
                txtQty.IsPrimary = txtQty.Enabled;
                lblQty.InnerText = string.Empty;

                ddlWarehouse.Clear();
                ddlLocation.Clear();
                ddlReasonCode.Clear();
                txtParentLPN.Clear();
                txtLPN.Clear();
                txtQty.Clear();
                txtRemark.Clear();

                //=====================

                using (var acc = new Access.Configuration.Rule())
                {
                    bool isControlParent = acc.Is_Rule("RULE_SCAN_PARENT_LPN", "YES");
                    if (isControlParent)
                    {
                        chkParentLPN.Enabled = true;
                        txtParentLPN.Enabled = true;
                        txtParentLPN.IsPrimary = true;
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
                            this.wh_master_id = ent.wh_master_id;
                            ddlWarehouse.SetValue(ent.wh_master_id);
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
                    if (listKey == null || listKey.Count <= 0)
                    {
                        this.Page.MessageWarning("Please select item again !");
                        popupChange.HideDialog();
                        return;
                    }


                    Guid wh_master_id = ddlWarehouse.GetValue();
                    Guid location_id = ddlLocation.GetValue();
                    string parent_lpn = txtParentLPN.GetValue();
                    string lpn = txtLPN.GetValue();
                    double? qty = txtQty.GetValue();
                    Guid reason_id = ddlReasonCode.GetValue();
                    string remark = txtRemark.GetValue();

                    bool stay_parent_lpn = chkParentLPN.GetValue();
                    bool stay_lpn = chkLPN.GetValue();

                    bool isSuccess = acc.Change_Location(listKey, wh_master_id, location_id, parent_lpn, lpn, qty, reason_id, remark, stay_parent_lpn, stay_lpn);
                    if (isSuccess)
                    {
                        popupChange.HideDialog();
                        // GridExt1.DataBind();
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

    }

}