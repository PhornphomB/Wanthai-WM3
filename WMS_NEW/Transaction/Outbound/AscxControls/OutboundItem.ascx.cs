using _UControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Source;

namespace WMS_NEW.Transaction.Outbound.AscxControls
{
    public partial class OutboundItem : UControlCustom, IFormRelation
    {
        private string item_status
        {
            get
            {
                return (string)ViewState["item_status"];
            }
            set
            {
                ViewState["item_status"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Binding Event
                GridExt1.GridRowCanEditValidate += GridExt1_GridRowCanEditValidate;
                GridExt1.GridRowCanDeleteValidate += GridExt1_GridRowCanDeleteValidate;
                GridExt1.GridRowCommandClick += GridExt1_GridRowCommandClick;
                GridExt1.GridRowAfterDataBound += GridExt1_GridRowAfterDataBound;

                popupEntity1.AfterNewDataEvent += PanelPopup1_AfterNewDataEvent;
                popupEntity1.AfterSetEditDataEvent += popupInbound_AfterSetEditDataEvent;
                popupEntity1.ValidateEntityEvent += PanelPopup1_ValidateEntityEvent;
                popupEntity1.PreSaveEntityEvent += PanelPopup1_PreSaveEntityEvent;

                ddlItemNo.PostValueChanged += ddlItemNo_PostValueChanged;
                ddlUOM.PostValueChanged += ddlUOM_PostValueChanged;
                ddlDefaultItemStatus.PostValueChanged += ddlDefaultItemStatus_PostValueChanged;
                chkItemBom.PostValueChanged += chkItemBom_PostValueChanged;
                #endregion

                #region Initial Input Data

                ddlItemNo.MethodQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQueryPropertyWarehouse(wh_master_id, owner_id); };
                ddlItemBom.MethodQueryProperty = delegate () { return Access.MasterData.ItemBom.Instance.GetPropertyAll(this.wh_master_id, this.owner_id); };

                ddlDefaultItemStatus.MethodQueryProperty = delegate () { return Access.Transaction.Inventory.InventoryStatus.Instance.GetQueryCode_Outbound(); };
                ddl_UDF5.MethodQueryProperty = delegate () { return new global::ConfigGlobal.DTO._Global.ActiveType().AsQueryable(); };

                //เพิ่ม Fuction หยิบของตาม Location ที่เลือก 27/03/2023 พี่นัทให้ดึง Location จาก Inventory ที่มีของอยู่
                ddlLocation.MethodQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQuery_OutboundDetailRule(this.wh_master_id,this.item_master_id, "RULE_LOCATION_TYPE_FOR_PICK"); };
                #endregion


                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = new Access.Transaction.Outbound.OutboundDetail(); };
                popupEntity1.InitControlStatic();

                GridExt1.PopupEntitySource = popupEntity1;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }






        #region Properties

        const string order_type_other = "OTHER";

        Guid wh_master_id
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

        Guid owner_id
        {
            get
            {
                return (Guid)ViewState["owner_id"];
            }
            set
            {
                ViewState["owner_id"] = value;
            }
        }

        Guid? item_master_id
        {
            get
            {
                return (Guid?)ViewState["item_master_id"];
            }
            set
            {
                ViewState["item_master_id"] = value;
            }
        }

        string order_type
        {
            get
            {
                return (string)ViewState["order_type"];
            }
            set
            {
                ViewState["order_type"] = value;
            }
        }

        string order_status
        {
            get
            {
                return (string)ViewState["order_status"];
            }
            set
            {
                ViewState["order_status"] = value;
            }
        }

        Guid whitem_masterid_edit
        {
            get
            {
                if (ViewState["whitem_masterid_edit"] == null)
                    ViewState["whitem_masterid_edit"] = Guid.Empty;

                return (Guid)ViewState["whitem_masterid_edit"];
            }
            set
            {
                ViewState["whitem_masterid_edit"] = value;
            }
        }

        #endregion


        public Action<dynamic> UpdateParent { get; set; }

        public void InitForm(dynamic _obj)
        {
            try
            {
                var ent = (Source.t_wms_outbound_master)_obj;

                hidMasterId.SetValue(ent.outbound_order_master_id);
                wh_master_id = ent.wh_master_id;
                owner_id = ent.owner_id;

                txtOrderNumberShow.SetValue(ent.outbound_order_number);

                ddlItemNo.BindDataSource();

                order_status = ent.order_status;
                order_type = ent.order_type.ToUpper();

                if (order_type != order_type_other)
                {
                    txt_UDF5.Visible = true;
                    ddl_UDF5.VisibleExt = false;
                }
                else
                {
                    txt_UDF5.Visible = false;
                    ddl_UDF5.VisibleExt = true;
                }

                GridExt1.NewVisible = (GridExt1.NewVisible && (ent.order_type.ToUpper() != order_type_other));

                if (order_status == "OPEN")
                {
                    ddlItemNo.Enabled = true;
                    InputTextBox22.Readonly = InputTextBox23.Readonly = false;

                    GridExt1.NewVisible = true;
                    pnItem.Enabled = true;
                    pnUserDefine.Enabled = true;
                    popupEntity1.EnableSave = true;
                }
                else
                {
                    ddlItemNo.Enabled = false;
                    InputTextBox22.Readonly = InputTextBox23.Readonly = true;

                    GridExt1.NewVisible = false;
                    pnItem.Enabled = false;
                    pnUserDefine.Enabled = false;
                    popupEntity1.EnableSave = false;
                }

                GetRuleControl();

                hidOrderMasterId.SetValue(ent.outbound_order_master_id);
                GridExt1.Search();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        void ddlItemNo_PostValueChanged(dynamic _value)
        {
            try
            {

                if (popupEntity1.FormState == FormState.New)
                {
                    txtLot.Clear();
                    txtSerial.Clear();
                    dtpExpiry.Clear();
                    ddlUOM.ClearItems();
                    ddlUOM.Clear();
                    ddlLocation.Clear();
                    txtQtyUnit.Clear();
                    txtPackSizeUom.Clear();
                    txtAttribute1.Clear();
                    txtAttribute2.Clear();
                    txtAttribute3.Clear();
                    txtAttribute4.Clear();
                    txtAttribute5.Clear();
                }

                txtLot.Enabled = false;
                txtSerial.Enabled = false;
                dtpExpiry.Enabled = false;
                this.item_master_id = null;

                txtQtyUnit.Enabled = false;
                txtPackSizeUom.Enabled = false;
                txtAttribute1.Enabled = false;
                txtAttribute2.Enabled = false;
                txtAttribute3.Enabled = false;
                txtAttribute4.Enabled = false;
                txtAttribute5.Enabled = false;

                //var wh_item_master_id = _value != null ? (Guid)_value : Guid.Empty;
                if (_value == null)
                    return;

                var wh_item_master_id = (Guid)_value;
                var _entItem = Access.MasterData.Item.Instance.GetWarehouseItem(wh_item_master_id);

                if (_entItem != null)
                {
                    
                    txtdgCode.SetValue(_entItem.t_wms_item.dg_code);
                    
                    if ((popupEntity1.FormState == FormState.New) || (whitem_masterid_edit != wh_item_master_id))
                    {
                        txtPrice.SetValue(_entItem.t_wms_item.price);
                        txtItemDesc.SetValue(_entItem.t_wms_item.description);
                    }

                    var entCrossRef = _entItem.t_wms_item.t_wms_item_crossref.FirstOrDefault();
                    if (entCrossRef != null)
                    {
                        txtItemCrossRef.SetValue(entCrossRef.alternate_item_number);
                    }

                    txtItemDesc.Update();
                    txtPrice.Update();
                    txtdgCode.Update();
                    txtItemCrossRef.Update();

                    txtLot.Enabled = _entItem.t_wms_item.lot_control == "FULL";
                    txtSerial.Enabled = _entItem.t_wms_item.sn_control == "FULL";
                    dtpExpiry.Enabled = _entItem.t_wms_item.expiry_date_control == "FULL";

                    txtAttribute1.Enabled = _entItem.t_wms_item.attribute1_control == "FULL";
                    txtAttribute2.Enabled = _entItem.t_wms_item.attribute2_control == "FULL";
                    txtAttribute3.Enabled = _entItem.t_wms_item.attribute3_control == "FULL";
                    txtAttribute4.Enabled = _entItem.t_wms_item.attribute4_control == "FULL";
                    txtAttribute5.Enabled = _entItem.t_wms_item.attribute5_control == "FULL";
                    if(txtAttribute1.Enabled==false)
                    {
                        txtAttribute1.IsPrimary = false;
                    }
                    else
                    {
                        txtAttribute1.IsPrimary = true;
                    }
                    this.item_master_id = _entItem.item_master_id;

                    #region Inventory Remain Qty

                    using (var _model = new Source.WMSEntities())
                    {
                        //string[] rules = _model.t_wms_rule.Where(x => x.rule_code == "INVENTORY_STATUS_FOR_PICKING").Select(se => se.value).ToArray();
                        //var DefaultItemStatus = this.item_status;

                        var result = (from rows in _model.t_wms_inventory
                                      where rows.owner_id == owner_id && rows.wh_item_master_id == wh_item_master_id && rows.t_wms_location.loc_type != "STG"
                                      && rows.t_wms_inventory_status.inv_status == this.item_status
                                      select rows.quantity - rows.quantity_allocated).ToArray();

                        double remain_qty = 0;
                        if (result.Count() > 0)
                            remain_qty = result.Sum();

                        txtRemainQty.SetValue(remain_qty);
                        txtRemainQty.Update();

                        #endregion

                        ddlUOM.MethodQueryProperty = delegate () { return Access.MasterData.ItemUom.Instance.GetQuery_WhItem(_value); };
                        ddlUOM.BindDataSource();
                        ddlUOM.Update();
                    }

                  
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
            finally
            {
                txtLot.Update();
                txtSerial.Update();
                dtpExpiry.Update();
                ddlUOM.Update();
                txtAttribute1.Update();
                txtAttribute2.Update();
                txtAttribute3.Update();
                txtAttribute4.Update();
                txtAttribute5.Update();
            }
        }
        void ddlUOM_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == null)
                    return;

                string itemDisplay = ddlItemNo.GetText();
                var splitItemDisplay = itemDisplay.Split(':');
                string item_number = splitItemDisplay[0];
                string item_master_id = Access.MasterData.Item.Instance.GetItemMasterIDByItemNumber(item_number);

                if (item_master_id != null)
                {
                    var strUom = ddlUOM.GetText();
                    var qtyUnit = Access.MasterData.Item.Instance.GetQtyUnitByItemMasterIDAndUom(Guid.Parse(item_master_id), strUom);
                    txtQtyUnit.SetValue(qtyUnit);
                    var baseUnit = Access.MasterData.Item.Instance.GetBaseUnitByItemMasterID(Guid.Parse(item_master_id));
                    txtPackSizeUom.SetValue(baseUnit);
                }

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
            finally
            {
                txtQtyUnit.Update();
                txtPackSizeUom.Update();
            }
        }
        void ddlDefaultItemStatus_PostValueChanged(dynamic _value)
        {
            if(_value != null)
            {
                this.item_status = _value.ToString();
                ddlItemNo_PostValueChanged(ddlItemNo.GetValue());
            }
        }
        #region Popup Entity Event

        void PanelPopup1_AfterNewDataEvent()
        {
            if (popupEntity1.FormState == FormState.New)
            {
                string _lineNumber = Access.Transaction.Outbound.OutboundDetail.Instance.GetLineNumber((Guid)hidMasterId.GetValue());
                txtLineNumber.SetValue(_lineNumber);
            }
            this.item_status = "Approved";
            ddlDefaultItemStatus.SetValue("Approved");
            ddlDefaultItemStatus.Update();
            txtdgCode.SetValue(null);
            txtItemDesc.SetValue(null);
            txtPrice.SetValue(null);
            txtItemCrossRef.SetValue(null);

            ddlItemNo.Enabled = true;
            ddlItemNo.VisibleExt = true;
            ddlItemBom.VisibleExt = false;
            txtQty.Enabled = true;

            txt_UDF5.Clear();
            txt_UDF5.Update();

            ddl_UDF5.Clear();
            ddl_UDF5.Update();

            chkItemBom.Checked = false;
            chkItemBom.Enabled = true;
            chkItemBom_PostValueChanged(chkItemBom.GetValue());
            //var wh_item_master_id = (Guid)_value;
            //    var _entItem = Access.MasterData.Item.Instance.GetWarehouseItem(wh_item_master_id);
            //txtAttribute1.Enabled = false;
            //txtAttribute2.Enabled = false;
            //txtAttribute3.Enabled = false;
            //txtAttribute4.Enabled = false;
            //txtAttribute5.Enabled = false;
            //txtAttribute1.Enabled = _entItem.t_wms_item.attribute1_control == "FULL";
            //txtAttribute2.Enabled = _entItem.t_wms_item.attribute2_control == "FULL";
            //txtAttribute3.Enabled = _entItem.t_wms_item.attribute3_control == "FULL";
            //txtAttribute4.Enabled = _entItem.t_wms_item.attribute4_control == "FULL";
            //txtAttribute5.Enabled = _entItem.t_wms_item.attribute5_control == "FULL";
        }

        void popupInbound_AfterSetEditDataEvent()
        {

            var _objectEntity = (popupEntity1.ObjectDataAccess as Access.Transaction.Outbound.OutboundDetail).Entity;
            whitem_masterid_edit = _objectEntity.wh_item_master_id;
            this.item_status = _objectEntity.default_item_status;
            ddlItemNo_PostValueChanged(_objectEntity.wh_item_master_id);

            /// uom unit and pack size uom by pong //
            using (var accUOM = new Access.MasterData.ItemUom())
            {
                this.PlugEventResult(accUOM);
                var ent = (Access.MasterData.ItemUom.ItemUOMDto)accUOM.Get_MinimumUOM((Guid)ddlItemNo.GetValue(), (Guid)ddlUOM.GetValue());
                if (ent != null)
                {
                    ddlUOM.SetText(_objectEntity.pack_size_uom);
                    //ddlUOM.SetValue(_objectEntity.pack_size_uom);
                    ddlUOM_PostValueChanged(ent.uom_id);
                }
            }

            ///
            ddlUOM.Update();
            if (!string.IsNullOrEmpty(_objectEntity.expiry_date))
            {
                DateTime dt = DateTime.ParseExact(_objectEntity.expiry_date, "yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
                dtpExpiry.SetValue(dt);
            }

            ddlItemNo.Enabled = (_objectEntity.bom_detail_id == null);
            txtQty.Enabled = (_objectEntity.bom_detail_id == null);
            if(txtQty.Enabled==true&& _objectEntity.pack_size_uom!= _objectEntity.uom)
            {
                txtQty.SetValue(_objectEntity.quantity_order/ (_objectEntity.pack_size_conversion_factor ?? 1));
            }
            txt_UDF5.SetValue(_objectEntity.user_def5);
            txt_UDF5.Update();

            ddl_UDF5.SetValue(_objectEntity.user_def5);
            ddl_UDF5.Update();
      
            chkItemBom.Enabled = false;
            chkItemBom_PostValueChanged(chkItemBom.GetValue());
        }

        bool PanelPopup1_ValidateEntityEvent()
        {
            if (chkItemBom.GetValue() == true)
                return true;

            //ถ้าไม่มี หน่วยนับหลัก ห้ามสร้างแผน
            bool isPrimaryUOM = Access.MasterData.ItemUom.Instance.CheckHasPrimary_ByWareHouseItem((Guid)ddlItemNo.GetValue());
            if (!isPrimaryUOM)
            {
                Page.MessageWarning("! Cannot save, Please create item primary uom");
                return false;
            }

            return true;
        }

        void PanelPopup1_PreSaveEntityEvent()
        {
            var _objectEntity = (popupEntity1.ObjectDataAccess as Access.Transaction.Outbound.OutboundDetail).Entity;

            _objectEntity.user_def5 = order_type != order_type_other ? txt_UDF5.GetValue() : (string)ddl_UDF5.GetValue();

            if (chkItemBom.GetValue() == false)
            {

                _objectEntity.uom = ddlUOM.GetText();
                _objectEntity.wh_item_master_id = (Guid)ddlItemNo.GetValue();

                using (var accUOM = new Access.MasterData.ItemUom())
                {
                    this.PlugEventResult(accUOM);

                    var ent = (Access.MasterData.ItemUom.ItemUOMDto)accUOM.Get_MinimumUOM((Guid)ddlItemNo.GetValue(), (Guid)ddlUOM.GetValue());
                    if (ent != null)
                    {
                        _objectEntity.uom = ent.uom;
                        var entBase = (Access.MasterData.ItemUom.ItemUOMDto)accUOM.Get_MinimumUOM((Guid)ddlItemNo.GetValue());
                        if (entBase != null)
                            _objectEntity.item_uom_id = entBase.uom_id;
                        _objectEntity.quantity_order = _objectEntity.quantity_order * ent.conversion;
                    }
                }

                _objectEntity.bom_id = null;

            }
            if (dtpExpiry.GetValue() != null)
            {
                _objectEntity.expiry_date = dtpExpiry.GetValue().Value.ToString("yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
            }
            else
            {
                _objectEntity.expiry_date = string.Empty;
            }

            double qtyUnit = double.Parse(txtQtyUnit.GetValue());
            if (qtyUnit == 0)
            {
                using (var _model = new WMSEntities())
                {
                    var uomEntity = _model.t_wms_item_uom
                        .FirstOrDefault(x => x.item_master_id == _objectEntity.item_master_id && x.uom == _objectEntity.uom);

                    if (uomEntity != null)
                    {
                        qtyUnit = uomEntity.conversion_factor;
                    }
                }
            }

            _objectEntity.pack_size_conversion_factor = qtyUnit;
            _objectEntity.pack_size_uom = ddlUOM.GetText(); ;
            _objectEntity.attribute1 = txtAttribute1.GetValue();
            _objectEntity.attribute2 = txtAttribute2.GetValue();
            _objectEntity.attribute3 = txtAttribute3.GetValue();
            _objectEntity.attribute4 = txtAttribute4.GetValue();
            _objectEntity.attribute5 = txtAttribute5.GetValue();
        }

        #endregion


        private bool GridExt1_GridRowCanEditValidate(GridViewRowEventArgs e)
        {
            var bom_id = (int?)DataBinder.Eval(e.Row.DataItem, "bom_id");

            return (bom_id == null);
        }

        bool GridExt1_GridRowCanDeleteValidate(GridViewRowEventArgs e)
        {
            var bom_id = (int?)DataBinder.Eval(e.Row.DataItem, "bom_id");

            if ((order_status.ToUpper() == "OPEN") && (bom_id == null))
                return true;
            else
                return false;
        }
        private void GridExt1_GridRowCommandClick(GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EDITBOM")
                {
                    var _outbound_order_detail_id = new Guid(e.CommandArgument.ToString());
                    ViewBomMaster(_outbound_order_detail_id);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExt1_GridRowAfterDataBound(GridViewRowEventArgs e)
        {
            var btEditBOM = (Button)e.Row.FindControl("EditBOM");

            if ((int?)DataBinder.Eval(e.Row.DataItem, "bom_id") != null)
            {
                var bom_master = (string)DataBinder.Eval(e.Row.DataItem, "bom_master");

                btEditBOM.Text = bom_master;
                btEditBOM.CssClass = "btn btn-info btn-ingrid";
                btEditBOM.Enabled = (order_status.ToUpper() == "OPEN");
                btEditBOM.Visible = true;
            }
            else
            {
                btEditBOM.Visible = false;
            }
        }



        protected void btRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                GridExt1.DataBind();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GetRuleControl()
        {
            popupEntity1.VisibleSave = Access.MasterData.Rule.Instance.GetRuleValue("RULE_OUTBOUND_DETAIL_EDIT") == "YES";
        }


        #region BOM
        private void chkItemBom_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value)
                {
                    ddlItemNo.IsPrimary = ddlUOM.IsPrimary = false;
                    ddlItemBom.IsPrimary = true;

                    ddlItemNo.VisibleExt = false;
                    ddlItemBom.VisibleExt = true;

                    txtItemDesc.Clear();

                    //panelUOM.Visible = false;
                    txtRemainQty.Visible = false;
                    ddlUOM.Visible = false;
                }
                else
                {
                    ddlItemNo.IsPrimary = ddlUOM.IsPrimary = true;
                    ddlItemBom.IsPrimary = false;

                    ddlItemNo.VisibleExt = true;
                    ddlItemBom.VisibleExt = false;

                    // panelUOM.Visible = true;
                    txtRemainQty.Visible = true;
                    ddlUOM.Visible = true;
                }

                popupEntity1.UpdateContent();
                //popupReceiveItem.UpdateContent();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }



        Guid _outbound_order_bom_id
        {
            get
            {
                return (Guid)ViewState["outbound_order_bom_id"];
            }
            set
            {
                ViewState["outbound_order_bom_id"] = value;
            }
        }

        //protected List<Access.Transaction.Outbound.Bom.BomDetailDto> BomDetail
        //{
        //    get
        //    {
        //        if (ViewState["BomDetail"] == null)
        //            ViewState["BomDetail"] = new Access.Transaction.Outbound.Bom.BomDetailDto[0];
        //        return new List<Access.Transaction.Outbound.Bom.BomDetailDto>((Access.Transaction.Outbound.Bom.BomDetailDto[])ViewState["BomDetail"]);
        //    }
        //    set
        //    {
        //        ViewState["BomDetail"] = value.ToArray();
        //    }
        //}

        void ViewBomMaster(Guid _outbound_order_detail_id)
        {
            using (var _acc = new Access.Transaction.Outbound.Bom.BomDetail())
            {
                var bom = _acc.GetBomDetailData(_outbound_order_detail_id);

                _outbound_order_bom_id = (Guid)bom.outbound_order_bom_id;

                txtBomMsCode.SetValue(bom.bom_item_number);
                txtBomMsQty.SetValue(bom.quantity);

                hdf_bom_outbound_order_detail_id.SetValue(_outbound_order_detail_id);
                //gridBomDetail.DataSource = BomDetail;
                gridBomDetail.Search();

                var allow_cmd = (order_status.ToUpper() == "OPEN");

                txtBomMsQty.Readonly = !allow_cmd;
                btBomMsQtyUpdate.Enabled = allow_cmd;
                btBomMsQtyDelete.Enabled = allow_cmd;

                popupBomMaster.ShowDialog();
            }
        }

        private void PopupBomMaster_CloseClick(object sender, EventArgs e)
        {
            hdf_bom_outbound_order_detail_id.SetValue(null);
            //gridBomDetail.DataSource = null;
            gridBomDetail.DataBind();
        }

        protected void btBomMsQtyUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (var _model = new Source.WMSEntities())
                {
                    var ent_bom = _model.t_wms_outbound_group_bom.Single(x => x.outbound_order_bom_id == _outbound_order_bom_id);
                    ent_bom.quantity = txtBomMsQty.GetValue();
                    ent_bom.update_date = DateTime.Now;
                    ent_bom.update_by = Session["UserID"].ToString().Trim();

                    var items = _model.t_wms_outbound_detail.Where(qry => qry.outbound_order_bom_id == _outbound_order_bom_id);
                    foreach (var item in items)
                    {
                        item.quantity_order = (ent_bom.quantity * item.bom_detail_quantity.Value);
                        item.update_date = ent_bom.update_date;
                        item.update_by = ent_bom.update_by;
                    }

                    if (_model.SaveChanges() > 0)
                    {
                        Page.ScriptJqueryMessage("Update bom quantity success.", JMessageType.Accept, true);

                        popupBomMaster.HideDialog();
                        GridExt1.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btBomMsQtyDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (var _model = new Source.WMSEntities())
                {
                    var items = _model.t_wms_outbound_detail.Where(qry => qry.outbound_order_bom_id == _outbound_order_bom_id);
                    foreach (var item in items)
                    {
                        _model.t_wms_outbound_detail.Remove(item);
                    }

                    var ent_bom = _model.t_wms_outbound_group_bom.Single(x => x.outbound_order_bom_id == _outbound_order_bom_id);
                    _model.t_wms_outbound_group_bom.Remove(ent_bom);

                    if (_model.SaveChanges() > 0)
                    {
                        Page.ScriptJqueryMessage("Delete bom set success.", JMessageType.Accept, true);

                        popupBomMaster.HideDialog();
                        GridExt1.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        //private void GridBomDetail_GridRowDelete(object _rowKeyValue)
        //{
        //    try
        //    {
        //        var id = new Guid(_rowKeyValue.ToString());

        //        using (var _model = new Source.WMSEntities())
        //        {
        //            var del = _model.t_wms_outbound_detail.Single(x => x.outbound_order_detail_id == id);
        //            _model.t_wms_outbound_detail.Remove(del);

        //            if (_model.SaveChanges() > 0)
        //            {
        //                var list = BomDetail;

        //                var dto = list.FirstOrDefault(x => x.KeyId == id);
        //                if (dto != null)
        //                {
        //                    list.Remove(dto);

        //                    gridBomDetail.DataSource = list;
        //                    gridBomDetail.DataBind();

        //                    GridExt1.DataBind();

        //                    BomDetail = list;

        //                    popupBomMaster.UpdateContent();
        //                    Page.ScriptJqueryMessage("Delete item success.", JMessageType.Accept, true);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging = new Prototype.Providers.Logging(this, ex);
        //        RaiseLogging();
        //    }
        //}



        #endregion
    }
}