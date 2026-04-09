using _UControls;
using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Report.AscxControls;

namespace WMS_NEW.Transaction.Outbound
{
    public partial class Outbound : PageCustom
    {
        public Guid _wh_master_id
        {
            get
            {
                if (ViewState["_wh_master_id"] == null)
                {
                    return Guid.Empty;
                }
                else
                {
                    return (Guid)ViewState["_wh_master_id"];
                }
            }
            set
            {
                ViewState["_wh_master_id"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Binding Event

                GridExt1.GridRowCanDeleteValidate += GridExt1_GridRowCanDeleteValidate;
                GridExt1.GridSearching += GridExt1_GridSearching;
                GridExt1.GridExportTemplate += GridExt1_GridExportTemplate;

                popupOutbound.AfterNewDataEvent += PanelPopup1_AfterNewDataEvent;
                popupOutbound.AfterSetEditDataEvent += popupInbound_AfterSetEditDataEvent;
                popupOutbound.ValidateEntityEvent += PanelPopup1_ValidateEntityEvent;
                popupOutbound.PreSaveEntityEvent += PanelPopup1_PreSaveEntityEvent;

                chkGenOrderNo.PostValueChanged += chkGenOrderNo_PostValueChanged;

                ddlWarehouse.PostValueChanged += ddlWarehouse_PostValueChanged;
                ddlOwner.PostValueChanged += ddlOwner_PostValueChanged;
                ddlCustomer.PostValueChanged += ddlCustomer_PostValueChanged;

                ddlOrderType.PostValueChanged += ddlOrderType_PostValueChanged;

                reportReportPack.BindingParameter += ReportViewer_BindingParameter;

                #endregion

                #region Initial Peoperty Column Grid

                GridColumnExt1.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQueryCode(_SessionVals.UserName); };
                GridColumnExt2.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName.Trim()); };
                GridColumnExt3.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty_Display("outbound_order_type"); };
                GridColumnExt5.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty_Display("outbound_order_status"); };
                GridColumnExt9.DropDownQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQuery(); };

                #endregion

                #region Initial Input Data

                //Search
                ddlCategory.MethodQueryProperty = delegate () { return Access.MasterData.ItemCategory.Instance.GetQuery(); };

                //Save
                ddlWarehouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
                ddlOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                ddlOrderType.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryPropertyAll("outbound_order_type"); };

                ddlCustomer.MethodQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQuery(ddlOwner.GetValue()); };
                ddDockDoor.MethodQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQuery(ddlWarehouse.GetValue(), "DOOR"); };

                ddCarrier.MethodQueryProperty = delegate () { return Access.MasterData.Carrier.Instance.GetQuery(); };

                ddBackFlag.MethodQueryProperty = delegate () { return new global::ConfigGlobal.DTO._Global.ActiveType().AsQueryable(); };
                ddTransportBy.MethodQueryProperty = delegate () { return Access.MasterData.Transportation.TransportBy.Instance.GetQuery(); };
                #endregion

                if (!Page.IsPostBack)
                {
                    ddWarehouseByRule.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(); };
                    ddTruckType.MethodQueryProperty = delegate () { return Access.MasterData.Transportation.TruckType.Instance.GetQuery(); };

                    //Search
                    hidSessionUser.SetValue(_SessionVals.UserName);
                    hidIsFirstLoad.SetValue("YES");
                }


                popupOutbound.InitObjectsEvent += () => { popupOutbound.ObjectDataAccess = new Access.Transaction.Outbound.Outbound(); };
                popupOutbound.InitControlStatic();

                GridExt1.PopupEntitySource = popupOutbound;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExt1_GridSearching(object sender, EventArgs e)
        {
            if (hidIsFirstLoad.GetValue() == "YES")
                hidIsFirstLoad.SetValue(string.Empty);
        }
        bool order_type_has_rule = false;

        void ddlOrderType_PostValueChanged(dynamic _value)
        {
            try
            {
                var data_field = "department";
                order_type_has_rule = false;

                if (_value != null)
                {
                    order_type_has_rule = Access.MasterData.Rule.Instance.Any_Rule("RULE_ORDER_TYPE_INSERT_INBOUND", (string)_value);
                }

                if (order_type_has_rule)
                {
                    ddWarehouseByRule.DataFieldValue = data_field;
                    ddWarehouseByRule.ComboType = ComboType.String;
                    txtWarehouseByRule.DataFieldValue = string.Empty;

                    ddWarehouseByRule.VisibleExt = true;
                    txtWarehouseByRule.VisibleExt = false;
                }
                else
                {
                    ddWarehouseByRule.DataFieldValue = string.Empty;
                    txtWarehouseByRule.DataFieldValue = data_field.ToString();

                    ddWarehouseByRule.VisibleExt = false;
                    txtWarehouseByRule.VisibleExt = true;
                }
                if (_value != null)
                {
                    if (Access.MasterData.Rule.Instance.Any_Rule("ORDER_TYPE_NOT_REQUIRE_CUST_PO", (string)_value))
                    {
                        txtCustomerPerchaseOrder.IsPrimary = false;
                        InputTextDate7.IsPrimary = false;
                    }
                    else
                    {
                        txtCustomerPerchaseOrder.IsPrimary = true;
                        InputTextDate7.IsPrimary = true;
                    }
                    txtCustomerPerchaseOrder.Update();
                    InputTextDate7.Update();
                }
                    
                //txtUdf1.Enabled = true;
                //txtUdf2.IsPrimary = false;

                //if (_value != null && (_value as string).ToUpper() == ORD_TYPE_OTHER)
                //{
                //    txtUdf1.Enabled = false;
                //    txtUdf2.IsPrimary = true;
                //}

                //txtUdf1.Update();
                //txtUdf2.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        #region Popup Entity Event

        void PanelPopup1_AfterNewDataEvent()
        {
            ddlOrderType.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("outbound_order_type"); };
            ddlOrderType.BindDataSource();

            txtOutboundNo.BaseContentCss = "col-sm-2";

            dtpDate.SetValue(DateTime.Now);

            if (txtOrderStatus.GetValue() == null)
            {
                txtOrderStatus.SetValue("OPEN");
            }

            txtShipToCodeName.SetValue(null);
            txtShipToCodeName.Enabled = true;

            btCancelOrder.Visible = false;

            //Rule Running
            using (var model = new Source.WMSEntities())
            {
                var entRule = model.t_wms_rule.Where(w => w.rule_code.ToUpper() == "OUTBOUND_ORDER_RUNNING" && w.is_active == "YES").FirstOrDefault();
                if (entRule != null)
                {
                    bool isRule = entRule.value.ToUpper() == "YES" ? true : false;

                    chkGenOrderNo.Checked = isRule;

                    chkGenOrderNo_PostValueChanged(isRule);
                }
            }
            chkGenOrderNo.Enabled = true;
            updateGenOrderNo.Visible = true;

        }

        void popupInbound_AfterSetEditDataEvent()
        {
            chkGenOrderNo.Checked = false;
            updateGenOrderNo.Visible = false;

            txtOutboundNo.BaseContentCss = "col-sm-3";

            var _objectEntity = (popupOutbound.ObjectDataAccess as Access.Transaction.Outbound.Outbound).Entity;

            if (order_type_has_rule)
            {
                ddWarehouseByRule.SetValue(Guid.Parse(_objectEntity.department));
            }
            else
            {
                txtWarehouseByRule.SetValue(_objectEntity.department);
            }


            if (_objectEntity.order_status == "OPEN")
            {
                txtLoadId.Enabled = true;
                ddDockDoor.Enabled = true;
                ddlWarehouse.Enabled = true;
                ddWarehouseByRule.Enabled = true;
                dtpDate.Enabled = true;
            }
            else
            {
                txtLoadId.Enabled = false;
                ddDockDoor.Enabled = false;
                ddlWarehouse.Enabled = false;
                ddWarehouseByRule.Enabled = false;
                dtpDate.Enabled = false;
            }

            if (_objectEntity.order_status == "SHIP")
            {
                txtCustomerPerchaseOrder.Enabled = false;
            }
            else
            {
                txtCustomerPerchaseOrder.Enabled = true;
            }

            if (!string.IsNullOrEmpty(hidCustomerCode.GetValue()) && hidCustomerCode.GetValue() == hidShipToCode.GetValue())
            {
                txtShipToCodeName.SetValue(ddlCustomer.GetText());
            }
            else
            {
                txtShipToCodeName.SetValue(null);
            }

            txtShipToCodeName.Enabled = true;

            this._wh_master_id = _objectEntity.wh_master_id;

            btCancelOrder.Visible = true;

            if (Access.MasterData.Rule.Instance.Any_Rule("RULE_OUTBOUND_ORDER_STATUS_FOR_CANCEL", _objectEntity.order_status))
                btCancelOrder.Enabled = true;
            else
                btCancelOrder.Enabled = false;
        }

        bool PanelPopup1_ValidateEntityEvent()
        {
            //กำหนดเงื่อนไข Validate ที่เป็น Business Logic ให้ InputData บ่างส่วน ถ้าไม่มีให้ return true

            if (popupOutbound.FormState == _UControls.FormState.Edit)
            {
                if (txtOrderStatus.GetValue() == "OPEN" && !string.IsNullOrEmpty(txtLoadId.GetValue()))
                {
                    using (var _acc = new Access.Transaction.Outbound.Outbound())
                    {
                        this.PlugEventResult(_acc);

                        if (_acc.CheckLoadOverStepOpen(ddlWarehouse.GetValue(), txtLoadId.GetValue()))
                        {
                            return false;
                        }
                    }
                }
                if(_wh_master_id != null && _wh_master_id != (Guid)ddlWarehouse.GetValue())
                {
                    using (var _acc = new Access.Transaction.Outbound.Outbound())
                    {
                        this.PlugEventResult(_acc);

                        string appID = _SessionVals.AppID;
                        string deviceName = _SessionVals.DeviceID;
                        string userID = _SessionVals.UserName;

                        Guid wh_master_id = (Guid)ddlWarehouse.GetValue();
                        Guid outbound_order_master_id = (Guid)popupOutbound.KeyFieldValue;
                        string outbound_order_number = txtOutboundNo.GetValue();
                        string errCode = string.Empty;
                        string errMsg = string.Empty;

                        _acc.UpdateWarehouse(appID, deviceName,this._wh_master_id, outbound_order_number, wh_master_id, outbound_order_master_id, userID, ref errCode, ref errMsg);
                        if (errCode != "0")
                        {
                            Page.MessageWarning(errMsg);
                            return false; 
                        }
                        this._wh_master_id = wh_master_id;
                    }
                        
                }
            }

            return true;
        }

        void PanelPopup1_PreSaveEntityEvent()
        {
            //กำหนดค่าให้ Property บางส่วนที่ไม่ได้อยู่ในส่วนของ InputData ก่อนที่จะบันทึก

            var _objectEntity = (popupOutbound.ObjectDataAccess as Access.Transaction.Outbound.Outbound).Entity;

            _objectEntity.wh_id = ddlWarehouse.GetText().Split(':')[0].Trim();
            _objectEntity.owner_code = ddlOwner.GetText().Split(':')[0].Trim();

            if (popupOutbound.FormState == _UControls.FormState.Edit)
            {
                using (var _acc = new Access.Transaction.Outbound.Outbound())
                {
                    Guid outbound_order_master_id = (Guid)popupOutbound.KeyFieldValue;
                    var ent = _acc._Model.t_wms_outbound_master.Where(w => w.outbound_order_master_id == outbound_order_master_id).FirstOrDefault();
                    if (ent != null)
                    {
                        _objectEntity.order_status = ent.order_status;
                    }
                }
            }
        }



        #endregion


        bool GridExt1_GridRowCanDeleteValidate(GridViewRowEventArgs e)
        {
            var order_status = (string)DataBinder.Eval(e.Row.DataItem, "order_status");
            //if ((order_status == "OPEN") || (order_status == "CANCEL"))
            if (order_status == "OPEN") // Comment by p'whan
                return true;
            else
                return false;
        }


        void chkGenOrderNo_PostValueChanged(dynamic _value)
        {
            try
            {
                if (popupOutbound.FormState == _UControls.FormState.Edit)
                    return;

                if (_value == true)
                {
                    var _no = Access.FunctionGenerate.Instance.GetGenerateRunning("OUT");

                    txtOutboundNo.SetValue(_no);
                    txtOutboundNo.Enabled = false;
                }
                else
                {
                    txtOutboundNo.Clear();
                    txtOutboundNo.Enabled = true;
                    txtOutboundNo.MaxLength = 15;
                }

                txtOutboundNo.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void ddlWarehouse_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value != null)
                {
                    ddDockDoor.BindDataSource();
                    ddDockDoor.Update();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void ddlOwner_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value != null)
                {
                    ddlCustomer.BindDataSource();
                    ddlCustomer.Update();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void ddlCustomer_PostValueChanged(dynamic _value)
        {
            if (!IsPostBack || _value == null)
                return;

            try
            {
                using (var _accCus = new Access.MasterData.Customer())
                {
                    PlugEventResult(_accCus);

                    var entityCus = _accCus.GetByKeyID((Guid)_value);
                    if (entityCus == null)
                        entityCus = new Source.t_wms_customer();

                    var iCustomer = updateCustomer.Controls.OfType<Control>().First().Controls.OfType<_UControls._IInputControl>();

                    foreach (var ictrl in iCustomer.Where(qry => !string.IsNullOrEmpty(qry.DataFieldTempValue)))
                    {
                        object obj = entityCus.GetPropertyValue(ictrl.DataFieldTempValue);
                        ictrl.SetObjectValue(obj);
                    }
                }

                updateCustomer.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btCopyShip_Click(object sender, EventArgs e)
        {
            if (!IsPostBack)
                return;

            try
            {
                var iCustomer = updateCustomer.Controls.OfType<Control>().First().Controls.OfType<_UControls._IInputControl>();

                var iShipTo = updateShipTo.Controls.OfType<Control>().First().Controls.OfType<_UControls._IInputControl>();

                txtShipToCodeName.SetObjectValue(ddlCustomer.GetText());

                foreach (var ictrl in iShipTo.Where(qry => !string.IsNullOrEmpty(qry.DataFieldTempValue)))
                {
                    var entityCus = iCustomer.First(qry => qry.DataFieldValue == ictrl.DataFieldTempValue);
                    if (entityCus != null)
                        ictrl.SetObjectValue(entityCus.GetObjectValue());
                }

                // updateShipTo.Update();

                PanelTab1.ChangeActivePanel(2);
                PanelTab1.UpdateContent();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btCancelOrder_Click(object sender, EventArgs e)
        {
            try
            {
                popupCancelOrder.ShowDialog();
                txtCancelRemark.SetValue(null);
                txtOutboundCancelOrder.SetValue(txtOutboundNo.GetValue());
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btCancelComfirm_Click(object sender, EventArgs e)
        {
            try
            {
                using (var _acc = new Access.Transaction.Outbound.Outbound())
                {
                    PlugEventResult(_acc);

                    if (_acc.CancelOrder((Guid)ddlWarehouse.GetValue(), ddlWarehouse.GetText().Split(':')[0].Trim(), (Guid)popupOutbound.KeyFieldValue, txtCancelRemark.GetValue()))
                    {
                        GridExt1.DataBind();

                        popupCancelOrder.HideDialog();
                        popupOutbound.HideDialog();

                        Page.MessageSuccess("Cancel Order Success.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        #region ReportViewer

        protected void btReportPacking_Click(object sender, EventArgs e)
        {
            try
            {
                txtPackShipDate.Clear();
                popupReportPack.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btReportPackConf_Click(object sender, EventArgs e)
        {
            try
            {
                reportReportPack.ViewReportNow("9B26F97D-9389-4256-871F-194EB85A90E5");
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        List<ReportParameter> ReportViewer_BindingParameter(string _report_id)
        {
            var prms = new List<ReportParameter>();

            prms.Add(new ReportParameter() { Name = "@ship_date_actual", Value = txtPackShipDate.GetValue().Value.ToString("yyyy-MM-dd", FieldsStatic.CultureInfo) });

            return prms;
        }

        #endregion

        protected void btnImport_Click(object sender, EventArgs e)
        {
            pnlImportFile.ShowDialog();
        }

        protected void btUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if ((!fuExcel.HasFile) || (fuExcel.PostedFile.ContentLength == 0))
                {
                    Page.MessageWarning("Please Select Excel File.");
                    return;
                }
                pnlImportFile.HideDialog();
                //Import To DB.
                PageImportExcel obj = new PageImportExcel();
                DataTable dt = obj.GetDataFromSheet(fuExcel);
                if (dt.Columns.Contains("Error"))
                {
                    dt.Columns.Remove("Error");
                }
                dt.Columns.Add("Error");
                DataView _dvError = new DataView(dt);

                string[] columnNames = dt.Columns.Cast<DataColumn>()
                                  .Select(x => x.ColumnName)
                                  .ToArray();

                if (dt.Columns["Warehouse"].IsEmpty() || dt.Columns["Owner"].IsEmpty() || dt.Columns["Order Number"].IsEmpty() || dt.Columns["Order Type"].IsEmpty() || dt.Columns["Customer Code"].IsEmpty() || dt.Columns["Cust PO"].IsEmpty() || dt.Columns["Ship Date Plan"].IsEmpty() || dt.Columns["Ship To"].IsEmpty() || dt.Columns["Item Number"].IsEmpty() || dt.Columns["Item Description"].IsEmpty() || dt.Columns["Order Qty"].IsEmpty() || dt.Columns["Lot"].IsEmpty() || dt.Columns["Expiry Date"].IsEmpty() || dt.Columns["UOM"].IsEmpty() || dt.Columns["Price"].IsEmpty() || dt.Columns["Create By"].IsEmpty() || dt.Columns["Attribute 1"].IsEmpty() || dt.Columns["Attribute 2"].IsEmpty() || dt.Columns["Attribute 3"].IsEmpty() || dt.Columns["Attribute 4"].IsEmpty() || dt.Columns["Attribute 5"].IsEmpty())
                {
                    Page.MessageWarning("Excel File Format Incorrect.");
                    return;
                }

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        using (Source.WMSEntities _model = new Source.WMSEntities())
                        {
                            using (var dbContextTransaction = _model.Database.BeginTransaction())
                            {
                                List<Source.t_wms_outbound_detail> _listOutboundDetail = new List<Source.t_wms_outbound_detail>();

                                foreach (DataRow dr in dt.Rows)
                                {
                                    try
                                    {
                                        if (!dr.Equals(null))
                                        { 
                                            if (dr[0].ToString().Trim() == "" && dr[1].ToString().Trim() == "" && dr[2].ToString().Trim() == "" && dr[3].ToString().Trim() == "" && dr[4].ToString().Trim() == "" && dr[5].ToString().Trim() == "" && dr[6].ToString().Trim() == "" && dr[7].ToString().Trim() == "" && dr[8].ToString().Trim() == "" && dr[9].ToString().Trim() == "" && dr[10].ToString().Trim() == "" && dr[11].ToString().Trim() == "" && dr[12].ToString().Trim() == "" && dr[13].ToString().Trim() == "" && dr[14].ToString().Trim() == "" && dr[15].ToString().Trim() == "")
                                            {
                                                continue;
                                            }

                                            string _warehouse = dr[0].ToString().Trim();
                                            string _owner = dr[1].ToString().Trim();
                                            string _outbound_order_number = dr[2].ToString().Trim();
                                            string _order_type = dr[3].ToString().Trim();
                                            string _customer = dr[4].ToString().Trim();
                                            string _customer_po = dr[5].ToString().Trim();
                                            string _ship_date_plan = dr[6].ToString().Trim();
                                            string _ship_to = dr[7].ToString().Trim();
                                            string _des_warehouse = dr[8].ToString().Trim();
                                            string _item_number = dr[9].ToString().Trim();
                                            string _item_description = dr[10].ToString().Trim();
                                            string _quantity = dr[11].ToString().Trim();
                                            string _lot_number = dr[12].ToString().Trim();
                                            string _expiry_date = dr[13].ToString().Trim();
                                            string _uom = dr[14].ToString().Trim();
                                            string _price = dr[15].ToString().Trim();
                                            string _create_by = dr[16].ToString().Trim();
                                            string _attri1 = dr[17].ToString().Trim();
                                            string _attri2 = dr[18].ToString().Trim();
                                            string _attri3 = dr[19].ToString().Trim();
                                            string _attri4 = dr[20].ToString().Trim();
                                            string _attri5 = dr[21].ToString().Trim();
                                            string _item_default_status = dr[22].ToString().Trim();
                                            if (_attri1==string.Empty)
                                            {
                                                string errItemCode = "Attribute1 not data";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            Guid _outbound_master_id = Guid.Empty;

                                            var customer = _model.t_wms_customer.Where(w => w.customer_code == _customer || w.customer_name == _customer).FirstOrDefault();
                                            var warehouse = _model.t_wms_wh.Where(w => w.wh_id == _warehouse || w.wh_name == _warehouse).FirstOrDefault();
                                            var owner = _model.t_wms_owner.Where(w => w.owner_code == _owner || w.owner_name == _owner).FirstOrDefault();
                                            var ordertype = _model.t_com_combobox_item.Where(w => w.is_active == "YES" && w.group_name == "outbound_order_type" && w.display_member == _order_type).FirstOrDefault();
                                            var desWarehouse = _model.t_wms_wh.Where(w => w.wh_id == _des_warehouse || w.wh_name == _des_warehouse).FirstOrDefault();
                                            var itemDefault = _model.t_wms_inventory_status.Where(w => w.is_active == "YES" && w.inv_status == _item_default_status).FirstOrDefault();
                                            var item = _model.t_wms_item.Where(w => w.item_number == _item_number).FirstOrDefault();
                                            if (item == null)
                                            {
                                                string errItemCode = "ไม่พบข้อมูล Item ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            var uom = _model.t_wms_item_uom.Where(w => w.uom == _uom && w.item_master_id == item.item_master_id).FirstOrDefault();
                                            if (uom == null)
                                            {
                                                string errItemCode = "ไม่พบข้อมูล UOM ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            var uomBase = _model.t_wms_item_uom.Where(w => w.item_master_id == item.item_master_id && w.primary_uom == "YES").FirstOrDefault();
                                            if (uomBase == null)
                                            {
                                                string errItemCode = "ไม่พบข้อมูล Uom Base ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            if (warehouse == null)
                                            {
                                                string errItemCode = "ไม่พบข้อมูล Warehouse ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            
                                            if (_order_type== "Transfer Out")
                                            {
                                                //if (desWarehouse == null)
                                                //{
                                                //    string errItemCode = "ไม่พบข้อมูล Destination Warehouse ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                //    _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                //    continue;
                                                //}
                                                //_des_warehouse = desWarehouse.wh_master_id.ToString();

                                                string errItemCode = $"ไม่สามารถใช้งาน Order Type ประเภท {_order_type} ได้"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }

                                            if(itemDefault==null)
                                            {
                                                string errItemCode = "ไม่พบข้อมูล Item Default Status ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }

                                            if (owner == null)
                                            {
                                                string errItemCode = "ไม่พบข้อมูล Owner ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            var UserWh = _model.t_wms_wh_user.Where(w => w.user_id == _SessionVals.UserName && w.wh_master_id == warehouse.wh_master_id).FirstOrDefault();
                                            if (UserWh == null)
                                            {
                                                string errItemCode = "ไม่พบข้อมูล User Mapping Warehouse ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            var whItem = _model.t_wms_wh_item.Where(w => w.wh_master_id == warehouse.wh_master_id && w.item_master_id == item.item_master_id).FirstOrDefault();
                                            if (whItem == null)
                                            {
                                                string errItemCode = "ไม่พบข้อมูล Warehouse Item ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            if (customer == null)
                                            {
                                                string errItemCode = "ไม่พบข้อมูล Customer ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            if (string.IsNullOrEmpty(_create_by))
                                            {
                                                string errItemCode = "กรุณาใส่ข้อมูล CreateBy"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;

                                            }
                                            var itemowner = _model.t_wms_item.Where(w => w.item_number == _item_number && w.owner_id == owner.owner_id).FirstOrDefault();
                                            if (itemowner == null)
                                            {
                                                string errItemCode = "Owner กับ Item ไม่ Match กัน"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }

                                            var outbound_master = _model.t_wms_outbound_master.Where(w => w.outbound_order_number == _outbound_order_number).FirstOrDefault();
                                            if (outbound_master == null) // Insert
                                            {
                                                Source.t_wms_outbound_master _newOutboundMaster = new Source.t_wms_outbound_master();
                                                _newOutboundMaster.outbound_order_master_id = Guid.NewGuid();
                                                _newOutboundMaster.outbound_order_number = _outbound_order_number;
                                                _newOutboundMaster.order_type = ordertype.display_member;

                                                if (customer != null)
                                                {
                                                    _newOutboundMaster.customer_id = customer.customer_id;
                                                    _newOutboundMaster.customer_code = customer.customer_code;
                                                    _newOutboundMaster.customer_name = customer.customer_name;
                                                    _newOutboundMaster.cus_description = customer.description;
                                                }

                                                _newOutboundMaster.customer_purchase_order = _customer_po;
                                                _newOutboundMaster.create_date = DateTime.Now;
                                                _newOutboundMaster.order_status = "OPEN";
                                                _newOutboundMaster.order_date = DateTime.Now;
                                                _newOutboundMaster.ship_date_plan = Convert.ToDateTime(_ship_date_plan);


                                                _newOutboundMaster.wh_id = warehouse.wh_id;
                                                _newOutboundMaster.wh_master_id = warehouse.wh_master_id;

                                                _newOutboundMaster.owner_code = owner.owner_code;
                                                _newOutboundMaster.owner_id = owner.owner_id;

                                                _newOutboundMaster.ship_to_code = _ship_to;
                                                _newOutboundMaster.department = _des_warehouse;
                                                _newOutboundMaster.create_by = _create_by;
                                                _model.t_wms_outbound_master.Add(_newOutboundMaster);
                                                _model.SaveChanges();

                                                Source.t_wms_outbound_detail _newOutboundDetail = new Source.t_wms_outbound_detail();
                                                _newOutboundDetail.outbound_order_detail_id = Guid.NewGuid();
                                                _newOutboundDetail.outbound_order_master_id = _newOutboundMaster.outbound_order_master_id;
                                                _newOutboundDetail.wh_item_master_id = whItem.wh_item_master_id;
                                                _newOutboundDetail.item_master_id = itemowner.item_master_id;
                                                _newOutboundDetail.item_uom_id = uomBase.item_uom_id;
                                                _newOutboundDetail.item_number = itemowner.item_number;
                                                _newOutboundDetail.line_number = _model.t_wms_outbound_detail.Where(w => w.outbound_order_master_id == _outbound_master_id).Count() == 0 ? "000001" : (_model.t_wms_outbound_detail.Where(w => w.outbound_order_master_id == _outbound_master_id).Count() + 1).ToString("000000");
                                                if (uom.primary_uom == "NO")
                                                    _newOutboundDetail.quantity_order = Convert.ToDouble(_quantity) * uom.conversion_factor;
                                                else
                                                    _newOutboundDetail.quantity_order = Convert.ToDouble(_quantity);

                                                _newOutboundDetail.uom = uomBase.uom;
                                                _newOutboundDetail.item_status = "NEW";
                                                _newOutboundDetail.default_item_status = "Available";
                                                _newOutboundDetail.lot_number = _lot_number;

                                                if (!string.IsNullOrEmpty(_expiry_date))
                                                    _newOutboundDetail.expiry_date = Convert.ToDateTime(_expiry_date).ToString("yyyyMMdd");

                                                _newOutboundDetail.attribute1 = _attri1;
                                                _newOutboundDetail.attribute2 = _attri2;
                                                _newOutboundDetail.attribute3 = _attri3;
                                                _newOutboundDetail.attribute4 = _attri4;
                                                _newOutboundDetail.attribute5 = _attri5;
                                                _newOutboundDetail.default_item_status = _item_default_status;
                                                if (!string.IsNullOrEmpty(_price))
                                                    _newOutboundDetail.price = Convert.ToDouble(_price);

                                                _newOutboundDetail.create_by = _create_by;
                                                _newOutboundDetail.create_date = DateTime.Now;


                                                ///เพิ่ม save ใน column pack_size_uom,pack_size_conversion_factor
                                                _newOutboundDetail.pack_size_uom = _uom;
                                                _newOutboundDetail.pack_size_conversion_factor = uom.conversion_factor;
                                                ///
                                                _listOutboundDetail.Add(_newOutboundDetail); 
                                            }
                                            else
                                            {
                                                if (outbound_master.order_status != "OPEN")
                                                {
                                                    string errItemCode = "ไม่สามารถอัพเดทข้อมูล Outbound ที่ไม่ใช่สถานะ OPEN ได้"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                    _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                    continue;
                                                }

                                                outbound_master.order_type = ordertype.display_member;

                                                if (customer != null)
                                                {
                                                    outbound_master.customer_id = customer.customer_id;
                                                    outbound_master.customer_code = customer.customer_code;
                                                    outbound_master.customer_name = customer.customer_name;
                                                    outbound_master.cus_description = customer.description;
                                                }

                                                outbound_master.customer_purchase_order = _customer_po;
                                                outbound_master.ship_to_code = _ship_to;
                                                outbound_master.department = _des_warehouse;
                                                //outbound_master.order_date = Convert.ToDateTime(_order_date);
                                                outbound_master.ship_date_plan = Convert.ToDateTime(_ship_date_plan);

                                                outbound_master.update_by = _SessionVals.UserName;
                                                outbound_master.update_date = DateTime.Now;
                                                _model.SaveChanges();

                                                //19072023 พี่นัทให้แก้เป็นลบข้อมูลทั้งหมดแล้ว Insert ใหม่
                                                _model.t_wms_outbound_detail.RemoveRange(_model.t_wms_outbound_detail.Where(w => w.outbound_order_master_id == outbound_master.outbound_order_master_id));
                                                _model.SaveChanges();

                                                //Insert Detail
                                                Source.t_wms_outbound_detail _newOutboundDetail = new Source.t_wms_outbound_detail();
                                                _newOutboundDetail.outbound_order_detail_id = Guid.NewGuid();
                                                _newOutboundDetail.outbound_order_master_id = outbound_master.outbound_order_master_id;
                                                _newOutboundDetail.wh_item_master_id = whItem.wh_item_master_id;
                                                _newOutboundDetail.item_master_id = itemowner.item_master_id;
                                                _newOutboundDetail.item_uom_id = uomBase.item_uom_id;
                                                _newOutboundDetail.item_number = itemowner.item_number;
                                                _newOutboundDetail.line_number = _listOutboundDetail.Count() == 0 ? "000001" : (_listOutboundDetail.Count() + 1).ToString("000000");
                                                if (uom.primary_uom == "NO")
                                                    _newOutboundDetail.quantity_order = Convert.ToDouble(_quantity) * uom.conversion_factor;
                                                else
                                                    _newOutboundDetail.quantity_order = Convert.ToDouble(_quantity);

                                                _newOutboundDetail.uom = uomBase.uom;
                                                _newOutboundDetail.item_status = "NEW";
                                                _newOutboundDetail.default_item_status = "Available";
                                                _newOutboundDetail.lot_number = _lot_number;

                                                if (!string.IsNullOrEmpty(_expiry_date))
                                                    _newOutboundDetail.expiry_date = Convert.ToDateTime(_expiry_date).ToString("yyyyMMdd");

                                                _newOutboundDetail.attribute1 = _attri1;
                                                _newOutboundDetail.attribute2 = _attri2;
                                                _newOutboundDetail.attribute3 = _attri3;
                                                _newOutboundDetail.attribute4 = _attri4;
                                                _newOutboundDetail.attribute5 = _attri5;
                                                _newOutboundDetail.default_item_status = _item_default_status;
                                                if (!string.IsNullOrEmpty(_price))
                                                    _newOutboundDetail.price = Convert.ToDouble(_price);

                                                _newOutboundDetail.create_by = _create_by;
                                                _newOutboundDetail.create_date = DateTime.Now;
                                                ///เพิ่ม save ใน column pack_size_uom,pack_size_conversion_factor
                                                _newOutboundDetail.pack_size_uom = _uom;
                                                _newOutboundDetail.pack_size_conversion_factor = uom.conversion_factor;
                                                ///
                                                _listOutboundDetail.Add(_newOutboundDetail);
                                            } 
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string errItemCode = "รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                        _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                        continue;
                                    }
                                }

                                _dvError.RowFilter = "Error <> ''";
                                if (_dvError.Count > 0)
                                {
                                    _dvError.RowFilter = ""; // เอาข้อมูลออกมาทั้งหมดรวมที่ไม่ได้ Error
                                    dbContextTransaction.Rollback();
                                    pnlImportFile.HideDialog();
                                    ExportExcel_EPPlus _excel = new ExportExcel_EPPlus();
                                    _excel.ToExcel(_dvError.ToTable(), "OutboundError.xlsx", columnNames);
                                }

                                _model.t_wms_outbound_detail.AddRange(_listOutboundDetail);
                                _model.SaveChanges();
                                dbContextTransaction.Commit();
                            }
                        }

                        GridExt1.DataBind();
                        pnlImportFile.HideDialog();
                        Page.MessageSuccess("Success");
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }

        }

        private void GridExt1_GridExportTemplate()
        {
            try
            {
                #region template old
                List<string> HeaderCol = new List<string>();
                HeaderCol.Add("Warehouse#");
                HeaderCol.Add("Owner#");
                HeaderCol.Add("Order Number#");
                HeaderCol.Add("Order Type#");
                HeaderCol.Add("Customer Code#");
                HeaderCol.Add("Cust PO");
                HeaderCol.Add("Ship Date Plan#");
                HeaderCol.Add("Ship To");
                HeaderCol.Add("Destination Warehouse");
                HeaderCol.Add("Item Number#");
                HeaderCol.Add("Item Description");
                HeaderCol.Add("Order Qty#");
                HeaderCol.Add("Lot");
                HeaderCol.Add("Expiry Date");
                HeaderCol.Add("UOM#");
                HeaderCol.Add("Price");
                HeaderCol.Add("Create By#");
                HeaderCol.Add("Attribute 1#");
                HeaderCol.Add("Attribute 2");
                HeaderCol.Add("Attribute 3");
                HeaderCol.Add("Attribute 4");
                HeaderCol.Add("Attribute 5");
                HeaderCol.Add("Item Default Status");

                using (var _model = new Source.WMSEntities())
                {
                    ExportExcel_EPPlus _excel = new ExportExcel_EPPlus();
                    // Insert Empty 
                    var List = new List<Dto_Outbound_Template>();
                    var dtoOutboundTemplate = new Dto_Outbound_Template();
                    List.Add(dtoOutboundTemplate);
                    _excel.ToExcel(List.ToDataTable(), "OutboundTemplate.xlsx", HeaderCol.ToArray());
                }
                #endregion

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }

    public class Dto_Outbound_Template
    {
        public string Warehouse { get; set; }
        public string Owner { get; set; }
        public string OrderNumber { get; set; }
        public string OrderType { get; set; }
        public string CustomerCode { get; set; }
        public string CustPO { get; set; }

        public DateTime ShipDatePlan { get; set; }
        public string ShipTo { get; set; }
        public string DesWarehouse { get; set; }
        public string ItemNumber { get; set; }
        public string ItemDescription { get; set; }

        public int OrderQty { get; set; }
        public string Lot { get; set; }
        public DateTime? Expiry_date { get; set; }
        public string UOM { get; set; }

        public double Price { get; set; }

        public string CreateBy { get; set; }
        public string ItemDefState { get; set; }
    }
}