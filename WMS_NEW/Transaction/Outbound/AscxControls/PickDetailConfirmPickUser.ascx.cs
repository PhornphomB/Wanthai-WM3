using System;
using System.Linq;
using System.Web.UI;

namespace WMS_NEW.Transaction.Outbound.AscxControls
{
    public partial class PickDetailConfirmPickUser : UControlCustom
    {
        #region ++ DELEGATE ++
        public Delegate dg_CallBackSearch;


        #endregion
        public string IsSerial
        {
            get
            {
                if (ViewState["IsSerial"] == null)
                    return "NONE";
                else
                    return (string)ViewState["IsSerial"];
            }
            set
            {
                ViewState["IsSerial"] = value;
            }
        }
        public string IsLot
        {
            get
            {
                if (ViewState["IsLot"] == null)
                    return "NONE";
                else
                    return (string)ViewState["IsLot"];
            }
            set
            {
                ViewState["IsLot"] = value;
            }
        }
        public string IsExp
        {
            get
            {
                if (ViewState["IsExp"] == null)
                    return "NONE";
                else
                    return (string)ViewState["IsExp"];
            }
            set
            {
                ViewState["IsExp"] = value;
            }
        }


        protected Access.Transaction.Outbound.OutboundMasterDto OutboundMasterDTO
        {
            get
            {
                if (ViewState["OutboundMasterDTO"] == null)
                    return new Access.Transaction.Outbound.OutboundMasterDto();
                else
                    return (Access.Transaction.Outbound.OutboundMasterDto)ViewState["OutboundMasterDTO"];
            }
            set
            {
                ViewState["OutboundMasterDTO"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ddlLocation.PostValueChanged += DdlLocation_PostValueChanged;
                ddlItem.PostValueChanged += DdlItem_PostValueChanged;
                ddlLot.PostValueChanged += DdlLot_PostValueChanged;
                ddlExpiryDate.PostValueChanged += DdlExpiryDate_PostValueChanged;
                ddlItemStatus.PostValueChanged += DdlItemStatus_PostValueChanged;

                ddlLpn.PostValueChanged = ddlLpn_PostValueChanged;

                if (!Page.IsPostBack)
                {
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }

        }
        public void InitForm(Access.Transaction.Outbound.OutboundMasterDto _ent)
        {
            try
            {

                this.OutboundMasterDTO = _ent;

                ClearData();

                Binding_DropDown(ddlLocation, "LOCATION");
                Binding_DropDown(ddlLocationStaging, "STAGING_LOCATION");


                txtOrderNo.SetValue(_ent.outbound_order_number);
                using (var acc = new Access.MasterData.Rule())
                {
                    string isPick = acc.GetRuleValue("RULE_PICK_PACK");
                    if (isPick == "YES")
                    {
                        txtBoxNumber.Enabled = true;
                        txtBoxNumber.IsPrimary = true;
                    }
                    else
                    {
                        txtBoxNumber.Enabled = false;
                        txtBoxNumber.IsPrimary = false;
                        txtBoxNumber.SetValue(_ent.outbound_order_number);
                    }
                    txtBoxNumber.Update();
                }

                popPickDetail.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        #region Event control

        private void DdlLocation_PostValueChanged(dynamic _value)
        {
            try
            {
                ddlLpn.Enabled = false;
                ddlLpn.IsPrimary = false;
                ddlItem.Enabled = false;
                ddlItemStatus.Enabled = false;
                // Location Control
                if (_value != null)
                {
                    ddlItem.Enabled = true;
                    ddlItemStatus.Enabled = true;



                    using (var model = new Source.WMSEntities())
                    {
                        Guid location_id = _value;
                        var entLocation = model.t_wms_location.Where(w => w.location_id == location_id).FirstOrDefault();
                        if (entLocation != null)
                        {
                            Binding_DropDown(ddlItem, "ITEM_NUMBER");
                            Binding_DropDown(ddlLpn, "LPN");

                            if (entLocation.lpn_controlled == "YES")
                            {
                                ddlLpn.Enabled = true;
                                ddlLpn.IsPrimary = true;
                            }
                        }
                    }
                }
                ddlItem.Update();
                ddlItemStatus.Update();
                ddlLpn.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        private void ddlLpn_PostValueChanged(dynamic obj)
        {
            try
            {
                Binding_DropDown(ddlItem, "ITEM_NUMBER");

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void DdlItemStatus_PostValueChanged(dynamic _value)
        {
            try
            {

                if (IsLot == "FULL")
                {
                    Binding_DropDown(ddlLot, "LOT_NUMBER");
                }

                if (IsExp == "FULL")
                {
                    Binding_DropDown(ddlExpiryDate, "EXPIRY_DATE");
                }

                Binding_DropDown(ddlSerial, "SERIAL_NUMBER");

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void DdlExpiryDate_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value != null)
                {
                    Binding_DropDown(ddlSerial, "SERIAL_NUMBER");
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void DdlLot_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value != null)
                {
                    if (IsExp == "FULL")
                    {
                        Binding_DropDown(ddlExpiryDate, "EXPIRY_DATE");
                    }
                    Binding_DropDown(ddlSerial, "SERIAL_NUMBER");

                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void DdlItem_PostValueChanged(dynamic _value)
        {
            try
            {
                Guid wh_item_master_id = Guid.Empty;
                string location = ddlLocation.GetText();
                string lpn = ddlLpn.GetValue();
                string default_item_status = ddlItemStatus.GetText();
                string lot_number = ddlLot.GetText();

                //Item Control
                ddlLot.Enabled = false;
                ddlExpiryDate.Enabled = false;
                ddlSerial.Enabled = false;
                txtAttribute1.Enabled = false;
                txtAttribute2.Enabled = false;
                txtAttribute3.Enabled = false;
                txtAttribute4.Enabled = false;
                txtAttribute5.Enabled = false;

                ddlLot.IsPrimary = false;
                ddlExpiryDate.IsPrimary = false;
                ddlSerial.IsPrimary = false;
                txtAttribute1.IsPrimary = false;
                txtAttribute2.IsPrimary = false;
                txtAttribute3.IsPrimary = false;
                txtAttribute4.IsPrimary = false;
                txtAttribute5.IsPrimary = false;
                ddlUOM.Enabled = false;


                if (_value != null)
                {
                    ddlUOM.Enabled = true;
                    txtQty.Enabled = true;
                    txtQty.SetValue(1);
                    ddlUOM.MethodQueryProperty = delegate () { return Access.MasterData.ItemUom.Instance.GetQuery_Item(_value); };
                    ddlUOM.BindDataSource();
                    using (var model = new Source.WMSEntities())
                    {
                        Guid item_master_id = _value;
                        var entItem = model.t_wms_item.Where(w => w.item_master_id == item_master_id).FirstOrDefault();
                        if (entItem != null)
                        {
                            IsSerial = entItem.sn_control;
                            IsLot = entItem.lot_control;
                            IsExp = entItem.expiry_date_control;

                            //Set wh_item_master_id
                            var entWhItem = entItem.t_wms_wh_item.Where(w => w.wh_master_id == OutboundMasterDTO.wh_master_id && w.item_master_id == entItem.item_master_id).FirstOrDefault();
                            if (entWhItem != null)
                            {
                                wh_item_master_id = entWhItem.wh_item_master_id;

                                //Set Default Item Status
                                Binding_DropDown(ddlItemStatus, "ITEM_STATUS");
                            }

                            //Set Item Control
                            if (entItem.lot_control == "FULL")
                            {
                                Binding_DropDown(ddlLot, "LOT_NUMBER");
                                ddlLot.Enabled = true;
                                ddlLot.IsPrimary = true;
                            }

                            if (entItem.expiry_date_control == "FULL")
                            {
                                Binding_DropDown(ddlExpiryDate, "EXPIRY_DATE");
                                ddlExpiryDate.Enabled = true;
                                ddlExpiryDate.IsPrimary = true;
                            }

                            if (entItem.sn_control == "FULL")
                            {
                                Binding_DropDown(ddlSerial, "SERIAL_NUMBER");
                                ddlSerial.Enabled = true;
                                txtQty.Enabled = false;
                                txtQty.SetValue(1);
                                var item_uom_id = model.t_wms_item_uom.Where(w => w.primary_uom == "YES" && w.item_master_id == item_master_id).FirstOrDefault().item_uom_id;
                                ddlUOM.SetValue(item_uom_id);
                                //ddlSerial.IsPrimary = true;
                            }

                            if (entItem.attribute1_control == "FULL")
                            {
                                txtAttribute1.Enabled = true;
                                txtAttribute1.IsPrimary = true;
                            }
                            if (entItem.attribute2_control == "FULL")
                            {
                                txtAttribute2.Enabled = true;
                                txtAttribute2.IsPrimary = true;
                            }
                            if (entItem.attribute3_control == "FULL")
                            {
                                txtAttribute3.Enabled = true;
                                txtAttribute3.IsPrimary = true;
                            }
                            if (entItem.attribute4_control == "FULL")
                            {
                                txtAttribute4.Enabled = true;
                                txtAttribute4.IsPrimary = true;
                            }
                            if (entItem.attribute5_control == "FULL")
                            {
                                txtAttribute5.Enabled = true;
                                txtAttribute5.IsPrimary = true;
                            }

                        }
                    }
                }

                ddlLot.Update();
                ddlExpiryDate.Update();
                ddlSerial.Update();
                txtAttribute1.Update();
                txtAttribute2.Update();
                txtAttribute3.Update();
                txtAttribute4.Update();
                txtAttribute5.Update();

                ddlUOM.Update();
                txtQty.Update();

                updateControl.Update();

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        #endregion


        void Binding_DropDown(_UControls.InputDropDown _ddl, string _control_name)
        {
            try
            {

                Nullable<System.Guid> location_id = ddlLocation.GetValue();
                Nullable<System.Guid> item_master_id = ddlItem.GetValue();
                string item_status = ddlItemStatus.GetText();
                string lot_number = ddlLot.GetText();
                string expiry_date = ddlExpiryDate.GetText();
                string serial_number = ddlSerial.GetText();
                string lpn = ddlLpn.GetValue();


                _ddl.MethodQueryProperty = delegate () { return new Access.Transaction.Outbound.OutboundPickDetail().GetQuery(_control_name, this.OutboundMasterDTO, location_id, item_master_id, item_status, lot_number, expiry_date, serial_number, lpn); };
                _ddl.BindDataSource();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        void ClearData()
        {
            foreach (Control item in pnData.Controls)
            {
                if (item is _UControls._IInputControl)
                {
                    if (!((_UControls._IInputControl)item).IsKey)
                    {
                        ((_UControls._IInputControl)item).Clear();
                    }
                    ((_UControls._IInputControl)item).Enabled = false;
                    ((_UControls._IInputControl)item).Update();

                }
            }

            ddlLocation.Enabled = true;
            ddlLocationStaging.Enabled = true;

            ddlLocation.Update();
            ddlLocationStaging.Update();
        }

        protected void btConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                using (var acc = new Access.Transaction.Outbound.OutboundPickDetail())
                {
                    PlugEventResult(acc);
                    Guid in_vchForkLocationID = ddlLocationStaging.GetValue();
                    string in_vchWhID = OutboundMasterDTO.wh_id;
                    Guid in_vchWhMasterID = OutboundMasterDTO.wh_master_id;
                    string in_vchOrderNumber = OutboundMasterDTO.outbound_order_number;
                    string in_vchBoxNumber = txtBoxNumber.GetValue();
                    string in_vchOwner = OutboundMasterDTO.owner_code;
                    Guid in_vchOwnerID = OutboundMasterDTO.owner_id;
                    string in_vchItemNumber = ddlItem.GetText();
                    string in_vchLotNumber = ddlLot.GetText();
                    string in_vchExpiryDate = ddlExpiryDate.GetText();
                    Guid in_vchLocationID = ddlLocation.GetValue();
                    string in_vchLocation = ddlLocation.GetText();
                    string in_vchParentLPN = string.Empty;
                    string in_vchLPN = ddlLpn.GetValue();
                    string in_vchSerialNumber = string.Empty;
                    if (IsSerial == "FULL")
                    {
                        in_vchSerialNumber = ddlSerial.GetValue() == null ? "ALL" : ddlSerial.GetText();
                    }
                    double in_fltQuantity = txtQty.GetValue();
                    Guid in_vchItemUOMID = ddlUOM.GetValue();
                    string in_vchDefaultItemStatus = ddlItemStatus.GetValue();
                    string in_vchAttribute1 = txtAttribute1.GetValue();
                    string in_vchAttribute2 = txtAttribute2.GetValue();
                    string in_vchAttribute3 = txtAttribute3.GetValue();
                    string in_vchAttribute4 = txtAttribute4.GetValue();
                    string in_vchAttribute5 = txtAttribute5.GetValue();

                    Guid item_master_id = ddlItem.GetValue();
                    var entWhItem = acc._Model.t_wms_wh_item.Where(w => w.wh_master_id == OutboundMasterDTO.wh_master_id && w.item_master_id == item_master_id).FirstOrDefault();
                    Nullable<System.Guid> wh_item_master_id = null;
                    if (entWhItem != null)
                    {
                        wh_item_master_id = entWhItem.wh_item_master_id;
                    }

                    acc.UserPickOrder(in_vchForkLocationID,
                        in_vchWhMasterID,
                        this.OutboundMasterDTO.outbound_order_master_id,
                        in_vchOwnerID,
                        in_vchBoxNumber,
                        in_vchLocationID,
                        in_vchParentLPN,
                        in_vchLPN,
                        wh_item_master_id,
                        in_vchDefaultItemStatus,
                        in_fltQuantity,
                        in_vchItemUOMID,
                        in_vchLotNumber,
                        in_vchExpiryDate,
                        in_vchSerialNumber,
                        in_vchAttribute1,
                        in_vchAttribute2,
                        in_vchAttribute3,
                        in_vchAttribute4,
                        in_vchAttribute5);

                    ClearData();

                    if (dg_CallBackSearch != null)
                    {
                        dg_CallBackSearch.DynamicInvoke();
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