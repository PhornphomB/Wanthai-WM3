using _UControls;
using System;
using System.Linq;
using System.Web.UI;

namespace WMS_NEW.Transaction.Inbound.AscxControl
{

    public partial class ucInboundReceipt : UControlCustom
    {
        public Delegate dg_CallBackSearch;

        #region View State
        public Guid inbound_order_master_id
        {
            get
            {
                return (Guid)ViewState["inbound_order_master_id"];
            }
            set
            {
                ViewState["inbound_order_master_id"] = value;
            }
        }
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
        public Guid receipt_header_id
        {
            get
            {
                return (Guid)ViewState["receipt_header_id"];
            }
            set
            {
                ViewState["receipt_header_id"] = value;
            }
        }
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

        public bool is_control_serial
        {
            get
            {
                if (ViewState["is_control_serial"] == null)
                {
                    return false;
                }

                return (bool)ViewState["is_control_serial"];
            }
            set
            {
                ViewState["is_control_serial"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                popReceipt.InitObjectsEvent += () => { popReceipt.ObjectDataAccess = new Access.Transaction.Inbound.Receipt.ReceiptDetail(); };
                popReceipt.InitControlStatic();

                popReceipt.ValidateEntityEvent += PopReceipt_ValidateEntityEvent;
                popReceipt.PreSaveEntityEvent += PopReceipt_PreSaveEntityEvent;
                popReceipt.RaiseEntitySaved += PopReceipt_RaiseEntitySaved;

                ddlWhItem.PostValueChanged = ddlItem_PostValueChanged;
                ddlLocation.PostValueChanged = ddlLocation_PostValueChanged;
                ddlModeReceive.PostValueChanged = ddlModeReceive_PostValueChanged;
                txtMfgDate.PostValueChanged = TxtMfgDate_PostValueChanged;

                txtSerial.TextEnterChanged = txtSerial_TextEnterChanged;

                ddlLocation.MethodQueryProperty = delegate () { return WMS_NEW.Access.MasterData.Location.Instance.GetQuery_Receipt(this.wh_master_id); };
                ddlWhItem.MethodQueryProperty = delegate () { return WMS_NEW.Access.MasterData.Mapping.WarehouseItem.Instance.GetQuery_Inbound(this.inbound_order_master_id); };
                ddlReason.MethodQueryProperty = delegate () { return Access.MasterData.Reason.Instance.GetQuery("OverReceive"); };


                if (!Page.IsPostBack)
                {
                    ddlItemStatusReceive.MethodQueryProperty = delegate () { return WMS_NEW.Access.Transaction.Inventory.InventoryStatus.Instance.GetQuery_Inbound(); };
                    ddlItemStatusReceive.BindDataSource();

                    ddlModeReceive.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("receipt_mode"); };
                    ddlModeReceive.BindDataSource();

                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void txtSerial_TextEnterChanged(_IInputText obj)
        {
            try
            {
                popReceipt.Save();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private bool PopReceipt_ValidateEntityEvent()
        {
            if (ddlReceiveNo.GetValue() == null)
            {
                Page.MessageWarning("Please select receipt number !");
                return false;
            }

            return true;
        }

        private void PopReceipt_RaiseEntitySaved(bool _saveStatus)
        {
            try
            {
                if (_saveStatus)
                {


                    //InitForm(this.inbound_order_master_id);

                    if (dg_CallBackSearch != null)
                    {
                        dg_CallBackSearch.DynamicInvoke();
                    }

                    //Repeat
                    if (this.is_control_serial)
                    {
                        txtSerial.Focus();
                    }
                    else
                    {
                        ddlWhItem.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        #region Popup_Event
        private void PopReceipt_PreSaveEntityEvent()
        {
            try
            {
                var _objectEntity = (popReceipt.ObjectDataAccess as Access.Transaction.Inbound.Receipt.ReceiptDetail).Entity;
                _objectEntity.inbound_order_master_id = this.inbound_order_master_id;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        #endregion

        #region Control Event
        private void TxtMfgDate_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == null)
                {
                    return;
                }

                using (var accRule = new Access.Configuration.Rule())
                {
                    if (accRule.Is_Rule("RULE_MFG_CAL_EXPDATE", "YES"))
                    {

                        DateTime mfg_date = (DateTime)_value;
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

        public void ddlLocation_PostValueChanged(dynamic _val)
        {
            try
            {
                if (_val == null)
                {
                    return;
                }

                using (var acc = new WMS_NEW.Access.MasterData.Location())
                {
                    this.PlugEventResult(acc);
                    var result = (WMS_NEW.Source.t_wms_location)acc.GetByKeyID(_val);
                    if (result != null)
                    {
                        //Validate Rule
                        using (var accRule = new WMS_NEW.Access.Configuration.Rule())
                        {
                            //Check Rule Item Status
                            bool isItemStatus = accRule.Is_Rule("RULE_SELECT_ITEM_STATUS_BEFORE_RECEIPT", "YES");
                            ddlItemStatusReceive.Enabled = isItemStatus;

                            //Check Rule Receive Mode
                            bool isReceiveMode = accRule.Is_Rule("RULE_USER_CAN_SELECT_RECEIPT_MODE", "YES");
                            ddlModeReceive.Enabled = isReceiveMode;

                            bool isReason = accRule.Is_Rule("RULE_REASON_FOR_OVER_RECEIVE", "YES");
                            ddlReason.VisibleExt = isReason;
                            ddlReason.IsPrimary = isReason;

                            //Location_id = result.location_id;
                            if (result.lpn_controlled.ToUpper() == "YES")
                            {
                                bool isRule = accRule.Is_Rule("RULE_SCAN_PARENT_LPN", "YES");
                                txtParentLPN.Enabled = isRule;
                                txtLPN.Enabled = true;
                                txtLPN.IsPrimary = true;
                            }
                            else
                            {
                                txtParentLPN.Enabled = false;
                                txtLPN.Enabled = false;
                                txtLPN.IsPrimary = false;
                                ddlWhItem.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        ddlLocation.Clear();
                    }

                    txtParentLPN.Update();
                    txtLPN.Update();
                    ddlLocation.Update();
                    ddlWhItem.Update();
                    ddlReceiveNo.Update();
                    ddlItemStatusReceive.Update();
                    ddlReason.Update();
                }

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        public void ddlItem_PostValueChanged(dynamic _val)
        {
            try
            {
                if (_val == null)
                {
                    return;
                }

                txtExpDate.Clear();
                txtSerial.Clear();
                txtLot.Clear();
                ddlItemStatusReceive.Clear();
                Guid wh_item_master_id = (Guid)_val;
                ddlUOM.MethodQueryProperty = delegate () { return WMS_NEW.Access.MasterData.ItemUom.Instance.GetQuery_WhItem(wh_item_master_id); };
                ddlUOM.BindDataSource();

                using (var acc = new Access.MasterData.Mapping.WarehouseItem())
                {
                    var entDet = acc._Model.t_wms_inbound_detail.Where(w => w.inbound_order_master_id == this.inbound_order_master_id && w.wh_item_master_id == wh_item_master_id).FirstOrDefault();
                    if (entDet != null)
                    {
                        var inv_status_id = acc._Model.t_wms_inventory_status.Where(w => w.inv_status == entDet.default_item_status).FirstOrDefault().inventory_status_id;
                        ddlItemStatusReceive.SetValue(inv_status_id);
                        ddlItemStatusReceive.Update();
                    }
                    var ent = acc.GetByKeyID(wh_item_master_id);
                    if (ent != null)
                    {
                        if (ent.t_wms_item.expiry_date_control.ToUpper() == "FULL")
                        {
                            txtExpDate.Enabled = true;
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

                            txtExpDate.Enabled = false;
                            txtMfgDate.Enabled = false;
                        }

                        this.is_control_serial = ent.t_wms_item.sn_control.ToUpper() == "FULL" ? true : false;
                        if (this.is_control_serial)
                        {
                            using (var model = new Source.WMSEntities())
                            {
                                var entUom = model.t_wms_item_uom.Where(w => w.t_wms_item.t_wms_wh_item.Any(a => a.wh_item_master_id == wh_item_master_id) && w.primary_uom == "YES").FirstOrDefault();
                                if (entUom != null)
                                {
                                    ddlUOM.SetValue(entUom.item_uom_id);
                                    ddlUOM.Update();
                                }
                            }
                        }

                        txtLot.Enabled = ent.t_wms_item.lot_control.ToUpper() == "FULL" ? true : false;
                        txtSerial.Enabled = ent.t_wms_item.sn_control.ToUpper() == "FULL" ? true : false;
                        txtAttribute1.Enabled = ent.t_wms_item.attribute1_control.ToUpper() == "FULL" ? true : false;
                        txtAttribute2.Enabled = ent.t_wms_item.attribute2_control.ToUpper() == "FULL" ? true : false;
                        txtAttribute3.Enabled = ent.t_wms_item.attribute3_control.ToUpper() == "FULL" ? true : false;
                        txtAttribute4.Enabled = ent.t_wms_item.attribute4_control.ToUpper() == "FULL" ? true : false;
                        txtAttribute5.Enabled = ent.t_wms_item.attribute5_control.ToUpper() == "FULL" ? true : false;

                        txtExpDate.IsPrimary = txtExpDate.Enabled;
                        txtLot.IsPrimary = txtLot.Enabled;
                        txtSerial.IsPrimary = txtSerial.Enabled;
                        txtAttribute1.IsPrimary = txtAttribute1.Enabled;
                        txtAttribute2.IsPrimary = txtAttribute2.Enabled;
                        txtAttribute3.IsPrimary = txtAttribute3.Enabled;
                        txtAttribute4.IsPrimary = txtAttribute4.Enabled;
                        txtAttribute5.IsPrimary = txtAttribute5.Enabled;

                        txtExpDate.Update();
                        txtMfgDate.Update();
                        txtLot.Update();
                        txtSerial.Update();
                        txtAttribute1.Update();
                        txtAttribute2.Update();
                        txtAttribute3.Update();
                        txtAttribute4.Update();
                        txtAttribute5.Update();

                        ddlModeReceive_PostValueChanged("ENTER QTY");
                    }
                }

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        public void ddlModeReceive_PostValueChanged(dynamic _val)
        {
            try
            {
                if (_val == null)
                {
                    return;
                }

                var ent = ((string)_val).ToUpper();
                //1. BY EACH 
                // --> Qty = 1 no edit
                if (ent == "BY EACH" || this.is_control_serial)
                {
                    txtQty.Enabled = false;
                    txtQty.SetValue(1);

                    ddlUOM.Enabled = false;

                }

                else {
                    txtQty.Clear(); 
                    txtQty.Enabled = true;

                    ddlUOM.Enabled = true;
                }



                //if (((string)_val).ToUpper() == "ENTER QTY" && !this.is_control_serial)
                //{
                //    txtQty.Enabled = true;
                //    ddlUOM.Enabled = true;
                //    txtQty.IsPrimary = true;
                //    txtQty.Clear();

                //}
                //else
                //{
                //    txtQty.Enabled = false;
                //    ddlUOM.Enabled = false;
                //    txtQty.SetValue(1);
                //    txtQty.IsPrimary = false;
                //}
                txtQty.Update();
                ddlUOM.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }



        protected void btGenerateOrder_Click(object sender, EventArgs e)
        {
            try
            {
                using (var acc = new WMS_NEW.Access.Transaction.Inbound.Receipt.ReceiptDetail())
                {
                    this.PlugEventResult(acc);
                    string receipt_number = acc.Generate_Receipt_Number(this.inbound_order_master_id, this.wh_master_id, _SessionVals.UserName);

                    ddlReceiveNo.MethodQueryProperty = delegate () { return WMS_NEW.Access.Transaction.Inbound.Receipt.ReceiptHeader.Instance.GetQuery_NotClose(this.inbound_order_master_id); };
                    ddlReceiveNo.BindDataSource();
                    ddlReceiveNo.SetText(receipt_number);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        #endregion

        #region Function
        public void InitForm(Guid _inbound_order_master_id)
        {
            try
            {
                using (var acc = new Access.Transaction.Inbound.InboundMaster())
                {
                    var ent = acc.GetByKeyID(_inbound_order_master_id);
                    if (ent != null)
                    {
                        this.inbound_order_master_id = ent.inbound_order_master_id;
                        this.wh_master_id = ent.wh_master_id;

                        txtParentLPN.Enabled = false;
                        txtLPN.Enabled = false;
                        txtLot.Enabled = false;
                        txtSerial.Enabled = false;
                        txtExpDate.Enabled = false;
                        txtMfgDate.Enabled = false;
                        txtSerial.Enabled = false;
                        txtAttribute1.Enabled = false;
                        txtAttribute2.Enabled = false;
                        txtAttribute3.Enabled = false;
                        txtAttribute4.Enabled = false;
                        txtAttribute5.Enabled = false;

                        popReceipt.New();
                        ddlModeReceive.Clear();
                        ddlModeReceive.Update();

                        txtInboundOrder.SetValue(ent.inbound_order_number);
                        txtInboundOrder.Update();                   

                        ddlReceiveNo.MethodQueryProperty = delegate () { return Access.Transaction.Inbound.Receipt.ReceiptHeader.Instance.GetQuery_NotClose(this.inbound_order_master_id); };
                        ddlReceiveNo.BindDataSource();
                    }
                }

                if (ddlLocation.MethodQueryProperty().ToList().Count == 1)
                {
                    Guid LocationId = Guid.Parse(ddlLocation.MethodQueryProperty().ToList().FirstOrDefault().ToString());
                    ddlLocation.SetValue(LocationId);
                    ddlLocation.Update();
                    ddlLocation_PostValueChanged(LocationId);
                }

                #region Initial Grid Filter
                #endregion
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void ShowDialog()
        {
            popReceipt.ShowDialog();
        }

        public void HideDialog()
        {
            popReceipt.HideDialog();
        }
        #endregion

    }
}