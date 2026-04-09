using System;
using System.Linq;
using System.Web.UI;

namespace WMS_NEW.Transaction.Inventory
{
    public partial class InventoryAdjustNotStock : PageCustom
    {
        #region ViewState

        public int days_to_expire
        {
            get
            {
                return (int)ViewState["days_to_expire"];
            }
            set
            {
                ViewState["days_to_expire"] = value;
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
                #region Event

                ddlWarehouse.PostValueChanged = ddlWarehouse_PostValueChanged;
                ddlOwner.PostValueChanged = ddlOwner_PostValueChanged;
                ddlWhItem.PostValueChanged = ddlWhItem_PostValueChanged;
                ddlLocation.PostValueChanged = ddlLocation_PostValueChanged;
                txtMfgDate.PostValueChanged = txtMfgDate_PostValueChanged;

                #endregion

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

                ddlWhItem.MethodQueryProperty = delegate () { return Access.MasterData.Mapping.WarehouseItem.Instance.GetQuery(ddlWarehouse.GetValue() ?? Guid.Empty, ddlOwner.GetValue() ?? Guid.Empty); };
                ddlLocation.MethodQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQuery_Change(ddlWarehouse.GetValue() ?? Guid.Empty); };

                if (!Page.IsPostBack)
                {
                    ddlAdjustFunction.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("adjust_function"); };
                    ddlAdjustFunction.BindDataSource();

                    ddlWarehouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(); };
                    ddlWarehouse.BindDataSource();

                    ddlWarehouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(); };
                    ddlWarehouse.BindDataSource();

                    ddlOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery_User(); };
                    ddlOwner.BindDataSource();

                    ddlItemStatus.MethodQueryProperty = delegate () { return Access.Transaction.Inventory.InventoryStatus.Instance.GetQuery(); };
                    ddlItemStatus.BindDataSource();

                    ddlReasonCode.MethodQueryProperty = delegate () { return Access.MasterData.Reason.Instance.GetQuery("adjustment"); };
                    ddlReasonCode.BindDataSource();


                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void txtMfgDate_PostValueChanged(dynamic obj)
        {
            try
            {
                if (obj == null)
                {
                    return;
                }

                using (var accRule = new Access.Configuration.Rule())
                {
                    if (accRule.Is_Rule("RULE_MFG_CAL_EXPDATE", "YES"))
                    {

                        DateTime mfg_date = (DateTime)obj;
                        DateTime exp_date = mfg_date.AddDays(this.days_to_expire);
                        txtExpDate.SetValue(exp_date);
                        txtExpDate.Update();
                    }
                }
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
                txtLPN.Clear();
                using (var acc = new Access.MasterData.Location())
                {
                    var ent = acc.GetByKeyID((Guid)obj);
                    if (ent != null)
                    {
                        bool control_lpn = ent.lpn_controlled == "YES" ? true : false;
                        txtLPN.Enabled = control_lpn;
                        txtLPN.IsPrimary = control_lpn;
                    }
                }
                txtLPN.Update();

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
                ddlLocation.BindDataSource();
                ddlWhItem.BindDataSource();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void ddlOwner_PostValueChanged(dynamic obj)
        {
            try
            {
                ddlWhItem.BindDataSource();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void ddlWhItem_PostValueChanged(dynamic obj)
        {
            try
            {
                if (obj == null)
                {
                    return;
                }

                object[] listObj = { txtItemDesc, txtDgCode, txtItemCrossRef, txtLot, txtExpDate, txtSerial, txtAttribute1, txtAttribute2, txtAttribute3, txtAttribute4, txtAttribute5, ddlUOM };
                Clear(listObj);

                using (var acc = new Access.MasterData.Mapping.WarehouseItem())
                {
                    var ent = acc.GetByKeyID((Guid)obj);
                    if (ent != null)
                    {
                        txtItemDesc.SetValue(ent.t_wms_item.description);
                        txtDgCode.SetValue(ent.t_wms_item.dg_code);
                        var entCros = ent.t_wms_item.t_wms_item_crossref.FirstOrDefault();
                        if (entCros != null)
                        {
                            txtItemCrossRef.SetValue(entCros.alternate_item_number);
                        }

                        if (ent.t_wms_item.expiry_date_control.ToUpper() == "FULL")
                        {
                            txtExpDate.Enabled = true;
                            txtExpDate.IsPrimary = true;

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

                                this.days_to_expire = ent.t_wms_item.days_to_expire ?? 0;
                            }
                        }
                        else
                        {
                            txtMfgDate.IsPrimary = false;
                            txtMfgDate.Enabled = false;
                            txtExpDate.Enabled = false;
                            txtExpDate.IsPrimary = false;

                        }

                        bool control_lot = ent.t_wms_item.lot_control == "FULL" ? true : false;
                        txtLot.Enabled = control_lot;
                        txtLot.IsPrimary = control_lot;
                        //bool control_exp_date = ent.t_wms_item.expiry_date_control == "FULL" ? true : false;
                        //txtExpDate.Enabled = control_exp_date;
                        //txtExpDate.IsPrimary = control_exp_date;
                        bool control_serial = ent.t_wms_item.sn_control == "FULL" ? true : false;
                        txtSerial.Enabled = control_serial;
                        txtSerial.IsPrimary = control_serial;
                        txtOrderQTY.Enabled = !control_serial;
                        if (control_serial)
                        {
                            txtOrderQTY.SetValue(1);
                        }

                        bool control_attribute1 = ent.t_wms_item.attribute1_control == "FULL" ? true : false;
                        txtAttribute1.Enabled = control_attribute1;
                        txtAttribute1.IsPrimary = control_attribute1;
                        bool control_attribute2 = ent.t_wms_item.attribute2_control == "FULL" ? true : false;
                        txtAttribute2.Enabled = control_attribute2;
                        txtAttribute2.IsPrimary = control_attribute2;
                        bool control_attribute3 = ent.t_wms_item.attribute3_control == "FULL" ? true : false;
                        txtAttribute3.Enabled = control_attribute3;
                        txtAttribute3.IsPrimary = control_attribute3;
                        bool control_attribute4 = ent.t_wms_item.attribute4_control == "FULL" ? true : false;
                        txtAttribute4.Enabled = control_attribute4;
                        txtAttribute4.IsPrimary = control_attribute4;
                        bool control_attribute5 = ent.t_wms_item.attribute5_control == "FULL" ? true : false;
                        txtAttribute5.Enabled = control_attribute5;
                        txtAttribute5.IsPrimary = control_attribute5;



                    }
                }

                ddlUOM.MethodQueryProperty = delegate () { return Access.MasterData.ItemUom.Instance.GetQuery_WhItem(obj); };
                ddlUOM.BindDataSource();

                popupAdjust.UpdateContent();


            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btnAdjustIn_Click(object sender, EventArgs e)
        {
            try
            {
                popupAdjust.ShowDialog();
                Clear();

                //Initial Control
                txtMfgDate.Enabled = false;
                txtExpDate.Enabled = false;
                txtLot.Enabled = false;
                txtSerial.Enabled = false;
                txtReceiveDate.SetValue(DateTime.Now);

                popupAdjust.UpdateContent();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btnConfirmAdjust_Click(object sender, EventArgs e)
        {
            try
            {
                string in_vchSubTranType = ddlAdjustFunction.GetValue();
                Nullable<System.Guid> in_vchWhMasterID = ddlWarehouse.GetValue();
                Nullable<System.Guid> in_vchOwnerID = ddlOwner.GetValue();
                Nullable<System.Guid> in_vchInvStatusID = ddlItemStatus.GetValue();
                Nullable<System.Guid> in_vchLocationID = ddlLocation.GetValue();
                Nullable<System.Guid> in_vchWhItemMasterID = ddlWhItem.GetValue();
                string in_vchParentLPN = txtParentLPN.GetValue();
                string in_vchLPN = txtLPN.GetValue();
                string in_vchLotNumber = txtLot.GetValue(); ;
                string in_vchExpiryDate = txtExpDate.GetValue() != null ? txtExpDate.GetValue().Value.ToString("yyyyMMdd") : string.Empty;
                string in_vchAttribute1 = txtAttribute1.GetValue();
                string in_vchAttribute2 = txtAttribute2.GetValue();
                string in_vchAttribute3 = txtAttribute3.GetValue();
                string in_vchAttribute4 = txtAttribute4.GetValue();
                string in_vchAttribute5 = txtAttribute5.GetValue();
                string in_vchSerialNumber = txtSerial.GetValue();
                Nullable<double> in_fltQty = txtOrderQTY.GetValue();
                Nullable<System.Guid> in_vchItemUomID = ddlUOM.GetValue();
                Nullable<System.Guid> in_vchReasonID = ddlReasonCode.GetValue();
                Nullable<System.DateTime> in_dtReceiveDate = txtReceiveDate.GetValue();
                Nullable<System.DateTime> in_dtMfgDate = txtMfgDate.GetValue();

                using (var acc = new Access.Transaction.Inventory.InventoryAdjust())
                {
                    PlugEventResult(acc);
                    acc.Adjust_NotStock(in_vchSubTranType, in_vchWhMasterID, in_vchOwnerID, in_vchInvStatusID, in_vchLocationID, in_vchWhItemMasterID, in_vchParentLPN, in_vchLPN, in_vchLotNumber, in_vchExpiryDate, in_vchAttribute1, in_vchAttribute2, in_vchAttribute3, in_vchAttribute4, in_vchAttribute5, in_vchSerialNumber, in_fltQty, in_vchItemUomID, in_vchReasonID, in_dtReceiveDate, in_dtMfgDate);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void Clear()
        {
            foreach (var control in panelData.Controls)
            {
                if (control is _UControls._IInputControl)
                {
                    ((_UControls._IInputControl)control).Clear();
                }
            }
        }

        void Clear(object[] _obj)
        {
            foreach (var control in _obj)
            {
                if (control is _UControls._IInputControl)
                {
                    ((_UControls._IInputControl)control).Clear();
                }
            }
        }
    }
}