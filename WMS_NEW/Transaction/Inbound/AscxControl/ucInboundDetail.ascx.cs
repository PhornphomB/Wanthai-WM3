using _UControls;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.SqlServer.Server;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Prototype.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Input;
using WMS_NEW.Access.Transaction.Inbound;
using WMS_NEW.MasterData;
using WMS_NEW.Report.AscxControls;
using WMS_NEW.Source;

namespace WMS_NEW.Transaction.Inbound.AscxControl
{
    public partial class ucInboundDetail : UControlCustom, IFormRelation
    {
        #region ++ DELEGATE ++
        delegate void dg_Search();
        event dg_Search eSearch;

        public Delegate dg_CallBackSearch;

        #endregion


        #region Message

        string msgOverReceipt = "! Please input Percent Over Receipt";
        string msgQtyMore = "! Quantity more than plan";
        string msgQtyLess = "! Quantity less than receive";
        string msgInputLot = "! Please input lot number";
        string msgLotDuplicate = "! Lot number was duplicate";
        string msgNotSave = "! Can not save data, Because order was closed";
        string msgNotPrimaryUOM = "! Could not save data, Because 'Item Primary UOM' not set";
        string msgNotFoundUOM = "! Could not found UOM";
        // string msgWaitBartender = "System have job in queue data, Please wait ...";
        string msgWaitBartenderTrigger = "System have job in queue data, Please wait ...";
        string msgNotPathData = "Path Data could not found !";
        // string msgNotPathTrigger = "Path Trigger could not found !";
        string msgNotSetPath = "Not set path !";
        string msgPrintSuccess = "Write data success";

        string msgFoundNotData = "Data not found !";

        string msgSerialWasUsed = "All serial was used !";


        #endregion

        #region Property ViewState

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
        public Guid owner_id
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

        public bool is_close
        {
            get
            {
                if (ViewState["is_close"] == null)
                {
                    return false;
                }
                return (bool)ViewState["is_close"];
            }
            set
            {
                ViewState["is_close"] = value;
            }
        }

        public bool is_serial
        {
            get
            {
                if (ViewState["is_serial"] == null)
                {
                    return false;
                }
                return (bool)ViewState["is_serial"];
            }
            set
            {
                ViewState["is_serial"] = value;
            }
        }
        public int? days_to_expire
        {
            get
            {
                return (int?)ViewState["days_to_expire"];
            }
            set
            {
                ViewState["days_to_expire"] = value;
            }
        }
        public bool is_can_delete
        {
            get
            {
                return (bool)ViewState["is_can_delete"];
            }
            set
            {
                ViewState["is_can_delete"] = value;
            }
        }
        #endregion



        public Action<dynamic> UpdateParent { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Deledate Call Back
                eSearch += new dg_Search(Receive_CallBack);
                ucInboundReceipt.dg_CallBackSearch = eSearch;
                #endregion

                #region Event
                GridExt1.GridRowCommandClick += GridExt1_GridRowCommandClick;
                GridExt1.GridDeleted += GridExt1_GridDeleted;
                GridExt1.GridRowCanDeleteValidate += GridExt1_GridRowCanDeleteValidate;
                GridExt1.GridRowCanEditValidate += GridExt1_GridRowCanEditValidate;
                GridExt1.GridRowAfterDataBound += GridExt1_GridRowAfterDataBound;
                

                ucReportViewer.BindingParameter += UcReportViewer_BindingParameter;
                ddlWhItem.PostValueChanged = ddlWhItem_PostValueChanged;
                
                ddlOverReceipt.PostValueChanged = ddlOverReceipt_PostValueChanged;
                txtSerial.TextEnterChanged = txtSerial_TextEnterChanged;

                chkItemBom.PostValueChanged += chkItemBom_PostValueChanged;

                popupBomMaster.CloseClick += PopupBomMaster_CloseClick;

                ddlUOM.PostValueChanged = ddlUOM_PostValueChanged;
                txtMFGDate.TextValueChanged += TxtMFGDate_TextValueChanged;
                ddlPalletUOM.PostValueChanged = ddlPalletUOM_PostValueChanged;
                txtMonthToExp.TextEnterChanged = txtMonthToExp_TextEnterChanged;
                #endregion

                #region Init PopupEntity 

                popInboundDetail.InitObjectsEvent += () => { popInboundDetail.ObjectDataAccess = new Access.Transaction.Inbound.InboundDetail(); };
                popInboundDetail.InitControlStatic();

                GridExt1.PopupEntitySource = popInboundDetail;

                popInboundDetail.AfterNewDataEvent += PopInboundDetail_AfterNewDataEvent;
                popInboundDetail.AfterSetEditDataEvent += PopInboundDetail_AfterSetEditDataEvent;
                popInboundDetail.PreSaveEntityEvent += PopInboundDetail_PreSaveEntityEvent;
                popInboundDetail.ValidateEntityEvent += PopInboundDetail_ValidateEntityEvent;
                popInboundDetail.RaiseEntitySaved += PopInboundDetail_RaiseEntitySaved;

                ddlLocation.PostValueChanged = ddlLocation_PostValueChanged;
                #endregion

                #region Binding DropDown Lazy

                #region Grid Column
                #endregion

                #region Grid Custom
                #endregion

                #region Form Page
                ddlWhItem.MethodQueryProperty = delegate () { return Access.MasterData.Mapping.WarehouseItem.Instance.GetQuery(this.wh_master_id, this.owner_id); };
                ddlItemBom.MethodQueryProperty = delegate () { return Access.MasterData.ItemBom.Instance.GetPropertyAll(this.wh_master_id, this.owner_id); };
                //ddlProductionLine.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("production_line"); };
                ddlLocation.MethodQueryProperty = delegate () { return WMS_NEW.Access.MasterData.Location.Instance.GetQuery_Receipt(this.wh_master_id); };
                ddRefItemNumber.MethodQueryProperty = delegate () { return WMS_NEW.Access.MasterData.Item.Instance.GetQuery(); };
                ddlProductionLine.MethodQueryProperty = delegate () { return Access.MasterData.ProductionLine.Instance.GetQuery(this.wh_master_id); };
                #endregion

                #endregion



                if (!Page.IsPostBack)
                {

                    #region Initial PopupEntity

                    //กำหนด PanelID หรือ ControlID ที่เป็นตัวคลุม Group InputData สำหรับการทำงานในหน้านี้
                    #endregion

                    #region Initial Panel Tap
                    #endregion

                    #region BindDataSource DropDrown
                    ddlDefaultItemStatus.MethodQueryProperty = delegate () { return Access.Transaction.Inventory.InventoryStatus.Instance.GetQueryCode_Inbound(); };
                    ddlDefaultItemStatus.BindDataSource();

                    ddlOverReceipt.MethodQueryProperty = delegate () { return new ConfigGlobal.DTO._Global.ActiveType().AsQueryable().Reverse(); };
                    ddlOverReceipt.BindDataSource();

                    #endregion


                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExt1_GridRowDelete(object _rowKeyValue)
        {
            using (var model = new Source.WMSEntities())
            {
                Guid KeyId = (Guid)_rowKeyValue;
                var Print = model.t_wms_print_label.Where(x => x.inbound_order_detail_id == KeyId);
                if (Print != null)
                {
                    model.t_wms_print_label.RemoveRange(Print);
                    model.SaveChanges();
                }
            }
        }

        private void ddlLocation_PostValueChanged(dynamic _val)
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
                            if (result.lpn_controlled.ToUpper() == "YES")
                            {
                                txtLPN.Enabled = true;
                                txtLPN.IsPrimary = true;
                            }
                            else
                            {
                                txtLPN.Enabled = false;
                                txtLPN.IsPrimary = false;
                            }
                        }
                    }
                    else
                    {
                        ddlLocation.Clear();
                    }

                    txtLPN.Update();
                    ddlLocation.Update();
                }

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private bool GridExt1_GridRowCanEditValidate(GridViewRowEventArgs e)
        {
            var bom_id = (int?)DataBinder.Eval(e.Row.DataItem, "bom_id");

            return (bom_id == null);
        }

        private void txtSerial_TextEnterChanged(_IInputText obj)
        {
            try
            {
                if (this.is_serial && !string.IsNullOrEmpty(txtSerial.GetValue()))
                {
                    txtOrderQTY.SetValue(1);
                    txtOrderQTY.Enabled = false;
                }
                else
                {
                    txtOrderQTY.Clear();
                    txtOrderQTY.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private bool GridExt1_GridRowCanDeleteValidate(System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            var bom_id = (int?)DataBinder.Eval(e.Row.DataItem, "bom_id");
            if (bom_id != null)
            {
                return false;
            }

            if (is_close)
            {
                return false;
            }

            if(!is_can_delete)
            {
                return false;
            }

            var KeyId = (Guid)DataBinder.Eval(e.Row.DataItem, "KeyId");
            using(var model = new Source.WMSEntities())
            {
                var ent = model.t_wms_print_label.Where(x => x.inbound_order_detail_id == KeyId && ((x.is_cancelled == "NO" && x.is_interface_hana == "YES") || x.is_print == "YES"));
                if (ent.Count() > 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void GridExt1_GridRowAfterDataBound(System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {
                var btEditBOM = (Button)e.Row.FindControl("EditBOM");

                if ((int?)DataBinder.Eval(e.Row.DataItem, "bom_id") != null)
                {
                    var bom_master = (string)DataBinder.Eval(e.Row.DataItem, "bom_master");

                    btEditBOM.Text = bom_master;
                    btEditBOM.CssClass = "btn btn-info btn-ingrid";
                    btEditBOM.Enabled = (status_order.ToUpper() == "OPEN");
                    btEditBOM.Visible = true;


                }
                else
                {
                    btEditBOM.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        public void InitForm(dynamic _obj)
        {
            try
            {
                var ent = (Source.t_wms_inbound_master)_obj;
                this.wh_master_id = ent.wh_master_id;
                this.owner_id = ent.owner_id;
                this.inbound_order_master_id = ent.inbound_order_master_id;
                txtOwnerCode.SetValue(ent.t_wms_owner.owner_code);
                hdf_inbound_order_master_id.SetValue(ent.inbound_order_master_id);

                Control_Receipt(this.inbound_order_master_id);


                ddlWhItem.Enabled = ent.order_status == "OPEN" ? true : false;

                status_order = ent.order_status;

                ControlClose(ent.order_status.ToUpper() != "CLOSE");

                //Rule Check New Receive ALL 180723
                using (var _model = new Source.WMSEntities())
                {
                    btReceiveAll.Visible = true;

                    if (!_model.t_wms_rule.Any(a => a.rule_code == "RULE_RECEIVE_ALL" && a.value == "YES"))
                        btReceiveAll.Visible = false;

                    string[] rules = _model.t_wms_rule.Where(x => x.rule_code == "ORDER_TYPE_FOR_INSERT_INBOUND" && x.is_active == "YES").Select(se => se.value).ToArray();
                    if (rules.Contains(ent.order_type))
                    {
                        is_can_delete = false;
                        ControlRule(false);
                    } // disabel save and disable new
                    else
                    {
                        is_can_delete = true;
                    }
                }
                #region Initial Grid Filter
                #endregion

                GridExt1.Search();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        #region Popup Entity

        private void PopInboundDetail_RaiseEntitySaved(bool _saveStatus)
        {
            try
            {
                if (_saveStatus)
                {
                    Control_Receipt(this.inbound_order_master_id);
                    var _objectEntity = (popInboundDetail.ObjectDataAccess as Access.Transaction.Inbound.InboundDetail).Entity;
                    InsertPrintLabel(this.inbound_order_master_id, _objectEntity.inbound_order_detail_id);
                    
                    if(popInboundDetail.FormState == _UControls.FormState.New)
                    {
                        popInboundDetail.New();
                    }
                    Receive_CallBack();
                }

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private bool PopInboundDetail_ValidateEntityEvent()
        {
            try
            {

                if (popInboundDetail.FormState == FormState.Edit)
                {
                    using (var model = new Source.WMSEntities())
                    {
                        Guid keyId = Guid.Parse(popInboundDetail.KeyFieldValue.ToString());
                        var _objectEntity = model.t_wms_inbound_detail.Where(w => w.inbound_order_detail_id == keyId).FirstOrDefault();
                        if (_objectEntity != null)
                        {
                            //Control Serial
                            if (_objectEntity.t_wms_wh_item.t_wms_item.sn_control.ToUpper().Trim() == "FULL")
                            {
                                if (!string.IsNullOrEmpty(txtSerial.GetValue()) && txtOrderQTY.GetValue() != 1)
                                {
                                    Page.MessageWarning("Can not save data ,Because qty_order <> 1 !");
                                    return false;
                                }
                            }

                            Guid item_uom_id = ddlUOM.GetValue();
                            var entUom = model.t_wms_item_uom.Where(w => w.item_uom_id == item_uom_id).FirstOrDefault();
                            if (entUom != null)
                            {
                                double qty = txtOrderQTY.GetValue();
                                _objectEntity.quantity_order = qty * entUom.conversion_factor;

                                if (_objectEntity.quantity_receive != 0 && _objectEntity.quantity_order < _objectEntity.quantity_receive)
                                {
                                    Page.MessageWarning("Can not save data ,Because qty_order < qty_receive !");
                                    return false;
                                }
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
            return true;

        }

        private void PopInboundDetail_PreSaveEntityEvent()
        {
            try
            {
                var _objectEntity = (popInboundDetail.ObjectDataAccess as Access.Transaction.Inbound.InboundDetail).Entity;
                
                using (var acc = new Access.MasterData.Mapping.WarehouseItem())
                {
                    if (chkItemBom.Checked == false)
                    {
                        var ent = (Source.t_wms_wh_item)acc.GetByKeyID(ddlWhItem.GetValue());
                        if (ent != null)
                        {
                            _objectEntity.item_master_id = ent.item_master_id;
                            _objectEntity.item_number = ent.t_wms_item.item_number;

                            var entUomPrimary = ent.t_wms_item.t_wms_item_uom.Where(w => w.primary_uom == "YES").FirstOrDefault();
                            if (entUomPrimary != null)
                            {
                                _objectEntity.item_uom_id = entUomPrimary.item_uom_id;
                                _objectEntity.uom = entUomPrimary.uom;
                            }
                            else
                            {
                                _objectEntity.uom = ent.t_wms_item.t_wms_item_uom.Where(w => w.conversion_factor == 1).FirstOrDefault().uom;
                            }

                                Guid item_uom_id = ddlUOM.GetValue();
                            var entUom = acc._Model.t_wms_item_uom.Where(w => w.item_uom_id == item_uom_id).FirstOrDefault();
                            if (entUom != null)
                            {
                                double qty = txtOrderQTY.GetValue();
                                _objectEntity.quantity_order = qty * entUom.conversion_factor;

                            }

                            //var entPrint = acc._Model.t_wms_print_label.Where(x => x.inbound_order_master_id == _objectEntity.inbound_order_master_id && x.inbound_order_detail_id == _objectEntity.inbound_order_detail_id).ToList();
                            //if (entPrint.Count != 0)
                            //{
                            //    if (entPrint.FirstOrDefault().quantity_order != _objectEntity.quantity_order)
                            //    {
                            //        _objectEntity.pack_size_uom = entUomPrimary != null ? entUomPrimary.uom : null;
                            //        _objectEntity.pack_size_conversion_factor = entUomPrimary != null ? entUomPrimary.conversion_factor : 1;
                            //    }
                            //}
                        }
                    }
                    
                    //else
                    //{
                    //    _objectEntity.wh_item_master_id = null;
                    //    _objectEntity.item_uom_id = null;
                    //    _objectEntity.uom = string.Empty;
                    //}
                }

                _objectEntity.expiry_date = txtExpDate.GetValue() != null ? txtExpDate.GetValue().Value.ToString("yyyyMMdd") : null;

                if (popInboundDetail.FormState == FormState.New)
                {
                    _objectEntity.inbound_order_master_id = this.inbound_order_master_id;
                    _objectEntity.item_status = "NEW";
                }

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void PopInboundDetail_AfterSetEditDataEvent()
        {
            try
            {
                var _objectEntity = (popInboundDetail.ObjectDataAccess as Access.Transaction.Inbound.InboundDetail).Entity;
                int print_row = 0;
                txtExpDate.Clear();
                if (!string.IsNullOrEmpty(_objectEntity.expiry_date))
                {
                    DateTime dt = DateTime.ParseExact(_objectEntity.expiry_date, "yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
                    txtExpDate.SetValue(dt);
                }
                if (days_to_expire != null)
                {
                    txtMonthToExp.SetValue(days_to_expire);
                    txtMonthToExp.Update();
                }
                using (var _model = new WMSEntities())
                {
                    var base_uom = _model.t_wms_item_uom.Where(x => x.item_master_id == _objectEntity.item_master_id && x.primary_uom == "YES").Select(x => x.uom).FirstOrDefault();
                    txtBaseUnit.SetValue(base_uom);
                    txtBaseUnit.Update();
                    ddlUOM.SetValue(_objectEntity.pack_size_uom_id);
                    ddlUOM.Update();
                    print_row = (_model.t_wms_print_label.Where(x => x.inbound_order_detail_id == _objectEntity.inbound_order_detail_id && x.is_print == "YES").ToList()).Count();

                    make_to = _model.t_wms_inbound_master.Find(_objectEntity.inbound_order_master_id).make_to;
                }
                
                txtOrderQTY.SetValue(_objectEntity.quantity_order / (_objectEntity.pack_size_conversion_factor ?? 1));
                txtOrderQTY.Enabled = !(_objectEntity.quantity_order == _objectEntity.quantity_receive || print_row > 0);
                txtOrderQTY.Update();


                chkItemBom.Checked = false;
                chkItemBom.Enabled = false;
                chkItemBom_PostValueChanged(chkItemBom.GetValue());

                bool is_not_receive = (_objectEntity.quantity_receive == 0 && print_row == 0);
                #region enable data
                txtMFGDate.Enabled = is_not_receive;
                //txtMFGDate.Enabled = false;
                ddlDefaultItemStatus.Enabled = is_not_receive;
                ddlUOM.Enabled = is_not_receive;
                ddlPalletUOM.Enabled = is_not_receive;
                ddlProductionLine.Enabled = false;
                txtRefOutboundOrderNumber.Enabled = is_not_receive;
                ddRefItemNumber.Enabled = is_not_receive;
                ddlOverReceipt.Enabled = is_not_receive;
                txtPrice.Enabled = is_not_receive;
                txtMonthToExp.Enabled = is_not_receive && (days_to_expire == null || days_to_expire == 0);
                txtMonthToExp.IsPrimary = false;
                txtExpDate.Enabled = false;

                txtMFGDate.Update();
                txtExpDate.Update();
                ddlDefaultItemStatus.Update();
                ddlUOM.Update();
                ddlPalletUOM.Update();
                ddlProductionLine.Update();
                txtRefOutboundOrderNumber.Update();
                ddRefItemNumber.Update();
                ddlOverReceipt.Update();
                txtPrice.Update();
                txtMonthToExp.Update();

                #endregion

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void PopInboundDetail_AfterNewDataEvent()
        {
            try
            {
                ddlWhItem.Enabled = true;
                ddlWhItem.VisibleExt = true;
                ddlItemBom.IsPrimary = false;
                ddlItemBom.VisibleExt = false;
                chkItemBom.Enabled = true;
                chkItemBom.Checked = false;

                txtlot.Enabled = false;
                txtExpDate.Enabled = false;
                txtAttribute1.Enabled = false;
                txtAttribute2.Enabled = false;
                txtAttribute3.Enabled = false;
                txtAttribute4.Enabled = false;
                txtAttribute5.Enabled = false;

                txtExpDate.Update();
                txtAttribute1.Update();
                txtAttribute2.Update();
                txtAttribute3.Update();
                txtAttribute4.Update();
                txtAttribute5.Update();

                ddlDefaultItemStatus.Enabled = true;
                ddlPalletUOM.Enabled = true;
                ddlProductionLine.Enabled = true;
                txtRefOutboundOrderNumber.Enabled = true;
                ddRefItemNumber.Enabled = true;
                ddlOverReceipt.Enabled = true;
                txtPrice.Enabled = true;

                ddlDefaultItemStatus.Update();
                ddlPalletUOM.Update();
                ddlProductionLine.Update();
                txtRefOutboundOrderNumber.Update();
                ddRefItemNumber.Update();
                ddlOverReceipt.Update();
                txtPrice.Update();

                //GET Line Number
                using (var _acc = new WMS_NEW.Access.Transaction.Inbound.InboundDetail())
                {
                    this.PlugEventResult(_acc);
                    string _max_line_number = _acc.GetLineNumber(this.inbound_order_master_id);
                    txtLineNumber.SetValue(_max_line_number);
                    txtLineNumber.Update();

                }

                using (var model = new Source.WMSEntities())
                {
                    txtOverPercent.Clear();
                    ddlOverReceipt.Clear();
                    var entRule = model.t_wms_rule.Where(w => w.rule_code.ToUpper() == "RULE_OVER_RECEIVE_INBOUND" && w.is_active == "YES").FirstOrDefault();
                    if (entRule != null)
                    {
                        ddlOverReceipt.SetValue(entRule.value);
                        ddlOverReceipt_PostValueChanged(entRule.value);
                        if (entRule.value == "YES")
                        {
                            double over_rec = !string.IsNullOrEmpty(entRule.process_name) ? Convert.ToDouble(entRule.process_name) : 0;
                            txtOverPercent.SetValue(over_rec);
                        }
                    }
                    ddlOverReceipt.Update();
                    txtOverPercent.Update();

                    var master = model.t_wms_inbound_master.FirstOrDefault(x => x.inbound_order_master_id == this.inbound_order_master_id);
                    if(master != null)
                    {
                        ddlProductionLine.SetValue(master.production_line);
                        ddlProductionLine.Update();
                        txtRefOutboundOrderNumber.SetValue(master.ref_outbound_order_number);
                        txtRefOutboundOrderNumber.Update();

                        make_to = master.make_to;

                        if (master.make_to != "Stock")
                        {
                            txtAttribute1.SetValue(master.ref_inbound_order_number);
                        }
                    }
                    

                }
                
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        #endregion

        #region Event Control
        protected void btnSaveReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                //ucOrderReceipt.Receipt_Item();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        protected void btnReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                ucInboundReceipt.ShowDialog();
                ucInboundReceipt.InitForm(this.inbound_order_master_id);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btReceiveAll_Click(object sender, EventArgs e)
        {
            try
            {
                pnlPopReceiveAll.ShowDialog();
                txtLPN.Enabled = false;
                txtLPN.Clear();
                txtLPN.Update();
                ddlLocation.BindDataSource();
                
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btConfReceiveAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlLocation.GetValue() == null)
                {
                    Page.MessageWarning("Please Select Location For Receive Item !");
                    return;
                }

                if (txtLPN.GetValue() == null && txtLPN.IsPrimary == true)
                {
                    Page.MessageWarning("Please Enter LPN !");
                    return;
                }

                using (var accRec = new Access.Transaction.Inbound.Receipt.ReceiptDetail())
                {
                    string _errCode = "", _errMsg = "";
                    accRec.InboundReceiptAll(hdf_inbound_order_master_id.GetValue(), ddlLocation.GetValue(), txtLPN.GetValue(), out _errCode, out _errMsg);

                    if (_errCode == "0")
                    {
                        Page.MessageSuccess(_errMsg);
                        pnlPopReceiveAll.HideDialog();
                        Receive_CallBack();
                    }
                    else
                    {
                        Page.MessageWarning(_errMsg); 
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void ddlWhItem_PostValueChanged(dynamic _value)
        {
            try
            { 

                if (popInboundDetail.FormState == FormState.New)
                {
                    txtItemDesc.Clear();
                    txtPrice.Clear();
                    txtItemCrossRef.Clear();

                    //txtlot.Clear();
                    txtExpDate.Clear();
                    //txtAttribute1.Clear();
                    txtAttribute2.Clear();
                    txtAttribute3.Clear();
                    txtAttribute4.Clear();
                    txtAttribute5.Clear();

                    txtSerial.Clear();

                    ddlUOM.ClearItems();
                    ddlUOM.Clear();
                }

                if (_value == null)
                {
                    txtlot.Enabled = false;
                    txtExpDate.Enabled = false;
                    txtAttribute1.Enabled = false;
                    txtAttribute2.Enabled = false;
                    txtAttribute3.Enabled = false;
                    txtAttribute4.Enabled = false;
                    txtAttribute5.Enabled = false;
                    txtSerial.Enabled = false;


                    return;
                }

                Guid wh_item_master_id = _value;
                Guid item_master_id = Guid.Empty;

                using (var acc = new Access.MasterData.Item())
                {
                    var entWhItem = acc._Model.t_wms_wh_item.Where(w => w.wh_item_master_id == wh_item_master_id).FirstOrDefault();
                    if (entWhItem != null)
                    {
                        item_master_id = entWhItem.item_master_id;
                        var ent = (Source.t_wms_item)acc.GetByKeyID(item_master_id);
                        days_to_expire = ent.days_to_expire;

                        if ((ent.days_to_expire == null || ent.days_to_expire == 0) && popInboundDetail.FormState == FormState.New)
                        {
                            txtMonthToExp.Enabled = true;
                            txtMonthToExp.IsPrimary = true;
                        }
                        else
                        {
                            txtMonthToExp.SetValue(ent.days_to_expire);
                            txtMonthToExp.Enabled = false;
                            txtMonthToExp.IsPrimary = false;
                        }
                        if (txtMFGDate.GetValue() != null)
                        {
                            AddMonthToExpire();
                        }

                        txtItemDesc.SetValue(ent.description);
                        txtPrice.SetValue(ent.price);
                        var entCross = ent.t_wms_item_crossref.FirstOrDefault();
                        if (entCross != null)
                        {
                            txtItemCrossRef.SetValue(entCross.alternate_item_number);
                        }
                        this.is_serial = ent.sn_control.ToUpper().Trim() == "FULL" ? true : false;
                        //txtlot.Enabled = ent.lot_control.ToUpper().Trim() == "FULL" ? true : false;
                        txtlot.Enabled = false;
                        //txtExpDate.Enabled = ent.expiry_date_control.ToUpper().Trim() == "FULL" ? true : false;
                        txtExpDate.Enabled = false;
                        //txtExpDate.IsPrimary = txtExpDate.Enabled;
                        txtSerial.Enabled = is_serial;
                        //txtAttribute1.Enabled = ent.attribute1_control.ToUpper().Trim() == "FULL" ? true : false;
                        txtAttribute1.Enabled = false;
                        txtAttribute2.Enabled = ent.attribute2_control.ToUpper().Trim() == "FULL" ? true : false;
                        txtAttribute3.Enabled = ent.attribute3_control.ToUpper().Trim() == "FULL" ? true : false;
                        txtAttribute4.Enabled = ent.attribute4_control.ToUpper().Trim() == "FULL" ? true : false;
                        txtAttribute5.Enabled = ent.attribute5_control.ToUpper().Trim() == "FULL" ? true : false;


                        if (this.is_serial && !string.IsNullOrEmpty(txtSerial.GetValue()))
                        {
                            txtOrderQTY.SetValue(1);
                            txtOrderQTY.Enabled = false;
                        }
                        else
                        {
                            txtOrderQTY.Clear();
                            txtOrderQTY.Enabled = true;
                        }

                        ddlUOM.MethodQueryProperty = delegate () { return new WMS_NEW.Access.MasterData.ItemUom().GetQuery_WhItem(wh_item_master_id); };
                        ddlUOM.BindDataSource();
                        

                        if (this.is_serial)
                        {
                            //Request maii 2020/10/28
                            //ddlUOM.Enabled = false;
                            var entUom = acc._Model.t_wms_item_uom.Where(w => w.t_wms_item.t_wms_wh_item.All(a => a.wh_item_master_id == wh_item_master_id) && w.primary_uom == "YES").FirstOrDefault();
                            if (entUom != null)
                            {
                                ddlUOM.SetValue(entUom.item_uom_id);
                            }
                        }
                        else
                        {
                            ddlUOM.Clear();
                            ddlUOM.Enabled = true;
                        }

                        ddlPalletUOM.MethodQueryProperty = delegate () { return new WMS_NEW.Access.MasterData.ItemUom().GetQuery_WhPalletItem(wh_item_master_id); };
                        ddlPalletUOM.BindDataSource();

                        #region Set Default Value

                        if (ddlUOM.MethodQueryProperty().ToList().Count == 1)
                        {
                            Guid KeyId = Guid.Parse(ddlUOM.MethodQueryProperty().ToList().FirstOrDefault().ToString());
                            ddlUOM.SetValue(KeyId);
                            ddlUOM_PostValueChanged(KeyId);
                        }
                        else
                        {
                            hidPackSizeUOMId.Clear();
                            hidPackSizeUOM.Clear();
                            txtPackSizeQty.Clear();
                            txtBaseUnit.Clear();

                            hidPackSizeUOMId.Update();
                            hidPackSizeUOM.Update();
                            txtPackSizeQty.Update();
                            txtBaseUnit.Update();

                        }
                        if (ddlPalletUOM.MethodQueryProperty().ToList().Count == 1)
                        {
                            Guid palletKeyId = Guid.Parse(ddlPalletUOM.MethodQueryProperty().ToList().FirstOrDefault().ToString());
                            ddlPalletUOM.SetValue(palletKeyId);
                            ddlPalletUOM_PostValueChanged(palletKeyId);
                        }
                        else
                        {
                            txtPalletSizeUom.Clear();
                            txtPalletSizeQty.Clear();

                            txtPalletSizeUom.Update();
                            txtPalletSizeQty.Update();
                        }
                        #endregion

                        //if (this.is_serial)
                        //{
                        //    var entUom = acc._Model.t_wms_item_uom.Where(w => w.t_wms_item.t_wms_wh_item.All(a => a.wh_item_master_id == wh_item_master_id) && w.primary_uom == "YES").FirstOrDefault();
                        //    if (entUom != null)
                        //    {
                        //        ddlUOM.SetValue(entUom.item_uom_id);
                        //    }
                        //}
                        //else
                        //{
                        //    ddlUOM.Clear();
                        //    ddlUOM.Enabled = true;
                        //}
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
                txtItemCrossRef.Update();
                txtItemDesc.Update();
                txtPrice.Update();
                txtOrderQTY.Update();
                ddlUOM.Update();

                txtSerial.Update();
                txtlot.Update();
                txtExpDate.Update();
                txtAttribute1.Update();
                txtAttribute2.Update();
                txtAttribute3.Update();
                txtAttribute4.Update();
                txtAttribute5.Update();

                txtMonthToExp.Update();
                popInboundDetail.UpdateContent();

            }
        }

        void ddlOverReceipt_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == null)
                {
                    return;
                }

                if (_value == "YES")
                {
                    txtOverPercent.Readonly = false;
                    txtOverPercent.IsPrimary = true;
                }
                else
                {
                    txtOverPercent.Readonly = true;
                    txtOverPercent.SetValue(null);
                    txtOverPercent.IsPrimary = false;
                }

                txtOverPercent.Update();

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExt1_GridDeleted(object sender, EventArgs e)
        {
            try
            {
                Control_Receipt(this.inbound_order_master_id);
                Receive_CallBack();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        #endregion

        #region Function
        private void Receive_CallBack()
        {
            GridExt1.Search();
            if (dg_CallBackSearch != null)
            {
                dg_CallBackSearch.DynamicInvoke();
            }
        }

        void Control_Receipt(Guid inbound_order_master_id)
        {
            using (var model = new Source.WMSEntities())
            {
                var cDet = model.t_wms_inbound_detail.Where(w => w.inbound_order_master_id == inbound_order_master_id).Count();
                btnReceipt.Enabled = cDet > 0 ? true : false;
                btReceiveAll.Enabled = cDet > 0 ? true : false;
            }

        }
        public bool InsertPrintLabel(Guid inbound_order_master_id, Guid inbound_order_detail_id)
        {
            using (var model = new Source.WMSEntities())
            {
                bool resultReturn = false;

                var entPrintLabel = model.t_wms_print_label.Where(x => x.inbound_order_master_id == inbound_order_master_id && x.inbound_order_detail_id == inbound_order_detail_id).OrderBy(x => x.row_print).ToList();
                if(entPrintLabel.Count == 0)
                {
                    List<t_wms_print_label> labels = new List<t_wms_print_label>();
                    var Inbound = (from rows in model.t_wms_inbound_detail
                                   join mas in model.t_wms_inbound_master on rows.inbound_order_master_id equals mas.inbound_order_master_id into lmas
                                   from mas in lmas.DefaultIfEmpty()
                                   join item in model.t_wms_item on rows.item_master_id equals item.item_master_id into litem
                                   from item in litem.DefaultIfEmpty()
                                   where rows.inbound_order_master_id == inbound_order_master_id && rows.inbound_order_detail_id == inbound_order_detail_id
                                   select new
                                   {
                                       mas.wh_id,
                                       mas.inbound_order_number,
                                       rows.line_number,
                                       rows.wh_item_master_id,
                                       rows.item_number,
                                       item.description,
                                       rows.quantity_order,
                                       rows.item_uom_id,
                                       rows.uom,
                                       rows.lot_number,
                                       rows.mfg_date,
                                       rows.expiry_date,
                                       rows.attribute1,
                                       rows.production_line,
                                       rows.pack_size_uom,
                                       rows.pack_size_conversion_factor,
                                       rows.pallet_size_uom,
                                       rows.pallet_size_conversion_factor
                                   }).FirstOrDefault();
                    if(Inbound != null)
                    {
                        int quantity = (int)(Inbound.quantity_order);
                        int palletSize = (int)(Inbound.pallet_size_conversion_factor ?? 1);

                        int insertCount = (int)Math.Ceiling((double)quantity / palletSize);

                        for (int i = 0; i < insertCount; i++)
                        {
                            int amountToInsert = Math.Min(palletSize, quantity - i * palletSize);
                            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                            var _lpn = string.Empty;
                            var generate = model.usp_wms_generate_lpn(Inbound.inbound_order_number, Inbound.production_line, errMsg);
                            if (generate != null)
                            {
                                _lpn = generate.FirstOrDefault();
                            }

                            var _pallet_seq = 0;
                            var generate_pallet_seq = model.usp_wms_generate_pallet_seq(this.wh_master_id, Inbound.production_line, Inbound.mfg_date, Inbound.item_number, errMsg);
                            if (generate_pallet_seq != null)
                            {
                                _pallet_seq = generate_pallet_seq.FirstOrDefault() ?? 0;
                            }

                            t_wms_print_label entPrint = new t_wms_print_label();
                            entPrint.print_label_id = Guid.NewGuid();
                            entPrint.wh_id = Inbound.wh_id;
                            entPrint.inbound_order_master_id = inbound_order_master_id;
                            entPrint.inbound_order_number = Inbound.inbound_order_number;
                            entPrint.inbound_order_detail_id = inbound_order_detail_id;
                            entPrint.line_number = Inbound.line_number;
                            entPrint.row_print = i + 1;
                            entPrint.lpn = _lpn;
                            entPrint.wh_item_master_id = Inbound.wh_item_master_id;
                            entPrint.item_number = Inbound.item_number;
                            entPrint.item_description = Inbound.description;
                            entPrint.quantity_order = Inbound.quantity_order;
                            entPrint.item_uom_id = Inbound.item_uom_id;
                            entPrint.uom = Inbound.uom;
                            entPrint.lot_number = Inbound.lot_number;
                            entPrint.mfg_date = Inbound.mfg_date;

                            DateTime result;
                            try
                            {
                                result = DateTime.ParseExact(Inbound.expiry_date, "yyyyMMdd", null);
                                entPrint.expiry_date = result;
                            }
                            catch (FormatException)
                            {
                                Page.MessageWarning("expiry_date Invalid date format");
                                resultReturn = false;
                            }

                            entPrint.attribute1 = Inbound.attribute1;
                            entPrint.production_line = Inbound.production_line;
                            entPrint.pack_size_per_pallet = (amountToInsert / Inbound.pack_size_conversion_factor);
                            entPrint.pack_size_uom = Inbound.pack_size_uom;
                            entPrint.pack_size_conversion_factor = Inbound.pack_size_conversion_factor;
                            entPrint.pallet_size_uom = Inbound.pallet_size_uom;
                            entPrint.pallet_size_conversion_factor = Inbound.pallet_size_conversion_factor;
                            entPrint.is_received = "NO";
                            entPrint.is_print = "NO";
                            entPrint.pallet_seq = _pallet_seq;
                            entPrint.create_by = _SessionVals.UserName;
                            entPrint.create_date = DateTime.Now;
                            entPrint.is_cancelled = "NO";
                            labels.Add(entPrint);

                        }
                        model.t_wms_print_label.AddRange(labels);
                    }
                    
                }
                else
                {
                    List<t_wms_print_label> adjust = new List<t_wms_print_label>();
                    var Inbound = model.t_wms_inbound_detail.FirstOrDefault(x => x.inbound_order_master_id == inbound_order_master_id && x.inbound_order_detail_id == inbound_order_detail_id);
                    var master = model.t_wms_inbound_master.FirstOrDefault(x => x.inbound_order_master_id == inbound_order_master_id);
                    t_wms_print_label last_label = entPrintLabel.OrderByDescending(x => x.row_print).FirstOrDefault();

                    int quantity = (int)(Inbound.quantity_order);
                    int palletSize = (int)(Inbound.pallet_size_conversion_factor ?? 1);

                    for (int i = 0; i < entPrintLabel.Count; i++)
                    {
                        entPrintLabel[i].mfg_date = Inbound.mfg_date;

                        DateTime result;
                        try
                        {
                            result = DateTime.ParseExact(Inbound.expiry_date, "yyyyMMdd", null);
                            entPrintLabel[i].expiry_date = result;
                        }
                        catch (FormatException)
                        {
                            Page.MessageWarning("expiry_date Invalid date format");
                            resultReturn = false;
                        }
                        entPrintLabel[i].attribute1 = Inbound.attribute1;
                        entPrintLabel[i].lot_number = Inbound.lot_number;
                        entPrintLabel[i].quantity_order = Inbound.quantity_order;
                        entPrintLabel[i].pack_size_uom = Inbound.pack_size_uom;
                        entPrintLabel[i].pack_size_conversion_factor = Inbound.pack_size_conversion_factor;
                        entPrintLabel[i].pallet_size_uom = Inbound.pallet_size_uom;
                        entPrintLabel[i].pallet_size_conversion_factor = Inbound.pallet_size_conversion_factor;
                        entPrintLabel[i].update_by = _SessionVals.UserName;
                        entPrintLabel[i].update_date = DateTime.Now;
                        if (quantity > 0)
                        {
                            entPrintLabel[i].pack_size_per_pallet = ((palletSize > quantity ? quantity : palletSize) / entPrintLabel[i].pack_size_conversion_factor);
                        }
                        else
                        {
                            entPrintLabel[i].pack_size_per_pallet = quantity;
                        }
                        quantity -= palletSize;
                    }

                    List<int> takeout = new List<int>();
                    if (quantity > 0)
                    {
                        while (quantity > 0)
                        {
                            takeout.Add(palletSize > quantity ? quantity : palletSize);
                            quantity -= palletSize;
                        }
                    }
                    int insertCount = (entPrintLabel.Where(x => x.pack_size_per_pallet > 0)).Count() + takeout.Count;

                    int rowCountDifference = insertCount - entPrintLabel.Count;
                    if (rowCountDifference > 0)
                    {
                        // Add rows to Table1
                        for (int i = 0; i < rowCountDifference; i++)
                        {
                            int amountToInsert = Math.Min(palletSize, quantity - i * palletSize);
                            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                            var _lpn = string.Empty;
                            var generate = model.usp_wms_generate_lpn(master.inbound_order_number, Inbound.production_line, errMsg);
                            if (generate != null)
                            {
                                _lpn = generate.FirstOrDefault();
                            }

                            var _pallet_seq = 0;
                            var generate_pallet_seq = model.usp_wms_generate_pallet_seq(this.wh_master_id, Inbound.production_line, Inbound.mfg_date, Inbound.item_number, errMsg);
                            if (generate_pallet_seq != null)
                            {
                                _pallet_seq = generate_pallet_seq.FirstOrDefault() ?? 0;
                            }

                            t_wms_print_label entPrint = new t_wms_print_label();
                            entPrint.print_label_id = Guid.NewGuid();
                            entPrint.wh_id = last_label.wh_id;
                            entPrint.inbound_order_master_id = inbound_order_master_id;
                            entPrint.inbound_order_number = last_label.inbound_order_number;
                            entPrint.inbound_order_detail_id = inbound_order_detail_id;
                            entPrint.line_number = Inbound.line_number;
                            entPrint.row_print = (i + 1) + entPrintLabel.Count;
                            entPrint.lpn = _lpn;
                            entPrint.wh_item_master_id = Inbound.wh_item_master_id;
                            entPrint.item_number = Inbound.item_number;
                            entPrint.item_description = last_label.item_description;
                            entPrint.quantity_order = Inbound.quantity_order;
                            entPrint.item_uom_id = Inbound.item_uom_id;
                            entPrint.uom = Inbound.uom;
                            entPrint.lot_number = Inbound.lot_number;
                            entPrint.mfg_date = Inbound.mfg_date;
                            entPrint.expiry_date = last_label.expiry_date;
                            entPrint.attribute1 = Inbound.attribute1;
                            entPrint.production_line = Inbound.production_line;
                            entPrint.pack_size_per_pallet = takeout[i] / Inbound.pack_size_conversion_factor;
                            entPrint.pack_size_uom = Inbound.pack_size_uom;
                            entPrint.pack_size_conversion_factor = Inbound.pack_size_conversion_factor;
                            entPrint.pallet_size_uom = Inbound.pallet_size_uom;
                            entPrint.pallet_size_conversion_factor = Inbound.pallet_size_conversion_factor;
                            entPrint.is_received = "NO";
                            entPrint.is_print = "NO";
                            entPrint.pallet_seq = _pallet_seq;
                            entPrint.create_by = _SessionVals.UserName;
                            entPrint.create_date = DateTime.Now;
                            entPrint.is_cancelled = "NO";
                            adjust.Add(entPrint);
                        }

                        model.t_wms_print_label.AddRange(adjust);
                    }
                    else if (rowCountDifference < 0)
                    {
                        List<t_wms_print_label> labelLastList = entPrintLabel.OrderByDescending(x => x.row_print).ToList();
                        // Remove rows from Table1
                        for (int i = 0; i < Math.Abs(rowCountDifference); i++)
                        {
                            if (labelLastList[i] != null && labelLastList[i].is_received != "YES")
                            {
                                adjust.Add(labelLastList[i]);
                            }
                           
                        }
                        model.t_wms_print_label.RemoveRange(adjust);
                        

                    }
                }
                try
                {
                    model.SaveChanges();
                    resultReturn = true;
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var error in ex.EntityValidationErrors.SelectMany(ev => ev.ValidationErrors))
                    {
                        Page.MessageWarning($"Property: {error.PropertyName} Error: {error.ErrorMessage}");
                        resultReturn = false;
                    }
                }
                return resultReturn;
            }

        }
        public void ControlClose(bool value)
        {
            GridExt1.NewVisible = value;
            btnReceipt.Enabled = value;
            btReceiveAll.Enabled = value;
            popInboundDetail.EnableSave = value;

            this.is_close = !value;

            GridExt1.DataBind();
        }
        public void ControlRule(bool value)
        {
            if (!value)
            {
                popInboundDetail.EnableSave = false;
                GridExt1.NewVisible = false;
            } // disabel save and disable new
            else
            {
                if (is_close)
                {
                    popInboundDetail.EnableSave = !is_close;
                    GridExt1.NewVisible = !is_close;
                }
                else
                {
                    popInboundDetail.EnableSave = true;
                    GridExt1.NewVisible = true;
                }

            }
            GridExt1.DataBind();
        }
        #endregion

        #region Report
        private List<Report.AscxControls.ReportParameter> UcReportViewer_BindingParameter(string _report_id)
        {
            _report_id = _report_id.ToUpper();
            var prms = new List<ReportParameter>();

            string inbound_order_master_id = this.inbound_order_master_id.ToString();
            string inbound_order_number = string.Empty;
            string owner_code = string.Empty;
            string wh_id = string.Empty;
            using (var acc = new Source.WMSEntities())
            {
                var ent = acc.t_wms_inbound_master.Where(w => w.inbound_order_master_id == this.inbound_order_master_id).FirstOrDefault();
                if (ent != null)
                {
                    inbound_order_number = ent.inbound_order_number;
                    owner_code = ent.owner_code;
                    wh_id = ent.wh_id;
                }
            }

            if (_report_id == "D34AF5E3-047E-4939-804D-09EC2B4C2896")
            {
                //prms.Add(new ReportParameter { Name = "@_order_number", Value = txtInboundNo.GetValue() });
                prms.Add(new ReportParameter { Name = "@inbound_order_master_id", Value = inbound_order_master_id });
                prms.Add(new ReportParameter { Name = "@wh_master_id", Value = this.wh_master_id.ToString() });
            }
            else if (_report_id == "B1301C43-0D55-4D60-9201-D2AC91DF488F")
            {
                prms.Add(new ReportParameter { Name = "@inbound_order_master_id", Value = inbound_order_master_id });
            }
            else if (_report_id == "2EB5BDA1-C794-45A8-AB52-249149C8FACA")
            {
                prms.Add(new ReportParameter { Name = "@inbound_order_master_id", Value = inbound_order_master_id });
            }


            else if (_report_id == "48D2953E-5BD5-4211-8865-2706D27BBF4E")
            {
                prms.Add(new ReportParameter { Name = "@order_number", Value = inbound_order_number });
            }
            else if (_report_id == "6008A339-5D0C-448B-86BF-2FA8BC64DF12")
            {
                prms.Add(new ReportParameter { Name = "@inbound_order_number", Value = inbound_order_number });
                prms.Add(new ReportParameter { Name = "@owner_code", Value = owner_code });
                prms.Add(new ReportParameter { Name = "@wh_id", Value = wh_id });
            }
            else if (_report_id == "AA2118BC-CAB3-4C7C-AD85-B9F926556EB7")
            {
                string order_master_id = inbound_order_master_id;
                string receipt_number = string.Empty;

                using (var _model = new Source.WMSEntities())
                {
                    var receipt_hdr = _model.t_wms_receipt_header.Select(se => new { se.inbound_order_master_id, se.receipt_number }).FirstOrDefault(x => x.inbound_order_master_id == this.inbound_order_master_id);
                    if (receipt_hdr != null)
                    {
                        receipt_number = receipt_hdr.receipt_number;
                    }
                }

                prms.Add(new ReportParameter { Name = "@wh_id", Value = wh_id });
                prms.Add(new ReportParameter { Name = "@owner_code", Value = owner_code });
                prms.Add(new ReportParameter { Name = "@receipt_number", Value = receipt_number });
            }

            return prms;
        }
        protected void btReport_Click(object sender, EventArgs e)
        {
            try
            {
                ucReportViewer.InitialForm("Inbound");
                ucReportViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        #endregion

        #region Print
        protected void btPrintAll_Click(object sender, EventArgs e)
        {
            try
            {
                //ucInboundPrint.InitialForm(this.inbound_order_master_id, null);
                //ucInboundPrint.ShowDialog();
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
                //if (e.CommandName == "PRINT")
                //{
                //    using (var acc = new Access.Transaction.Inbound.InboundDetail())
                //    {
                //        Guid keyID = Guid.Parse(e.CommandArgument.ToString());
                //        var ent = acc.GetByKeyID(keyID);
                //        if (ent != null)
                //        {
                //            //ucInboundPrint.InitialForm(this.inbound_order_master_id, keyID);
                //            //ucInboundPrint.ShowDialog();

                //            ucInboundPrint144.InitialForm(keyID);
                //            ucInboundPrint144.ShowDialog();
                //        }
                //    }
                //}
                //else 
                if (e.CommandName == "EDITBOM")
                {
                    var _inbound_order_detail_id = new Guid(e.CommandArgument.ToString());
                    ViewBomMaster(_inbound_order_detail_id);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        #endregion

        #region BOM
        private void chkItemBom_PostValueChanged(dynamic _value)
        {
            try
            {

                if (_value)
                {
                    ddlWhItem.IsPrimary = ddlUOM.IsPrimary = false;
                    ddlItemBom.IsPrimary = true;

                    ddlWhItem.VisibleExt = false;
                    ddlItemBom.VisibleExt = true;
                    //panelWhItem.Visible = false;

                    txtItemDesc.Clear();

                    //pantlUOM.Visible = false;
                    ddlUOM.Visible = false;

                    ddlItemBom.BindDataSource();
                }
                else
                {
                    ddlWhItem.IsPrimary = ddlUOM.IsPrimary = true;
                    ddlItemBom.IsPrimary = false;

                    ddlWhItem.VisibleExt = true;
                    ddlItemBom.VisibleExt = false;
                    //panelWhItem.Visible = true;

                    //pantlUOM.Visible = true;
                    ddlUOM.Visible = true;

                    if (popInboundDetail.FormState == FormState.New)
                    {
                        ddlWhItem.BindDataSource();
                        ddlUOM.BindDataSource();
                    }
                }

                popInboundDetail.UpdateContent();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        Guid _inbound_order_bom_id
        {
            get
            {
                return (Guid)ViewState["inbound_order_bom_id"];
            }
            set
            {
                ViewState["inbound_order_bom_id"] = value;
            }
        }

        public string status_order
        {
            get
            {
                return (string)ViewState["status_order"];
            }
            set
            {
                ViewState["status_order"] = value;
            }
        }

        public string make_to
        {
            get
            {
                return (string)ViewState["make_to"];
            }
            set
            {
                ViewState["make_to"] = value;
            }
        }
        
        private List<string> GridSorts
        {
            get
            {
                if (ViewState["GridSorts"] == null)
                    ViewState["GridSorts"] = new string[0];
                return new List<string>((string[])ViewState["GridSorts"]);
            }
            set
            {
                ViewState["GridSorts"] = value.ToArray();
            }
        }

        void ViewBomMaster(Guid _inbound_order_detail_id)
        {
            using (var _acc = new Access.Transaction.Inbound.Bom.BomDetail())
            {

                var bom = _acc.GetBomDetailData(_inbound_order_detail_id);
                //var bom = (from rows in _model.t_wms_inbound_detail
                //           join grb_bom in _model.t_wms_inbound_group_bom on rows.inbound_order_bom_id equals grb_bom.inbound_order_bom_id
                //           where rows.inbound_order_detail_id == _inbound_order_detail_id
                //           select new
                //           {
                //               rows.inbound_order_bom_id,
                //               rows.bom_item_number,
                //               grb_bom.quantity
                //           }).Single();

                //var result_items = (from rows in _model.t_wms_inbound_detail
                //                    where rows.inbound_order_bom_id == bom.inbound_order_bom_id
                //                    select new BomDetailDto
                //                    {
                //                        KeyId = rows.inbound_order_detail_id,
                //                        item_number = rows.item_number,
                //                        uom = rows.uom,
                //                        bom_detail_quantity = rows.bom_detail_quantity
                //                    }).ToList();


                _inbound_order_bom_id = (Guid)bom.inbound_order_bom_id;

                txtBomMsCode.SetValue(bom.bom_item_number);
                txtBomMsQty.SetValue(bom.quantity);

                txtBomMsCode.Enabled = false;


                hdf_bom_inbound_order_master_id.SetValue(_inbound_order_detail_id);
                //gridBomDetail.DataSource = result_items;
                gridBomDetail.Search();

                var allow_cmd = (status_order.ToUpper() == "OPEN");

                txtBomMsQty.Readonly = !allow_cmd;
                btBomMsQtyUpdate.Enabled = allow_cmd;
                btBomMsQtyDelete.Enabled = allow_cmd;

                popupBomMaster.ShowDialog();
            }
        }

        private void PopupBomMaster_CloseClick(object sender, EventArgs e)
        {
            //gridBomDetail.DataSource = null;
            //gridBomDetail.DataBind();
        }

        protected void btBomMsQtyUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (var _model = new Source.WMSEntities())
                {
                    var ent_bom = _model.t_wms_inbound_group_bom.Single(x => x.inbound_order_bom_id == _inbound_order_bom_id);
                    ent_bom.quantity = txtBomMsQty.GetValue();
                    ent_bom.update_date = DateTime.Now;
                    ent_bom.update_by = Session["UserID"].ToString().Trim();

                    var items = _model.t_wms_inbound_detail.Where(qry => qry.inbound_order_bom_id == _inbound_order_bom_id);
                    foreach (var item in items)
                    {
                        item.quantity_order = (ent_bom.quantity * item.bom_detail_quantity.Value);
                        item.update_date = ent_bom.update_date;
                        item.update_by = ent_bom.update_by;
                    }

                    if (_model.SaveChanges() > 0)
                    {
                        Page.ScriptJqueryMessage("Update Success.", JMessageType.Accept, true);

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
                    var items = _model.t_wms_inbound_detail.Where(qry => qry.inbound_order_bom_id == _inbound_order_bom_id);
                    foreach (var item in items)
                    {
                        _model.t_wms_inbound_detail.Remove(item);
                    }

                    var ent_bom = _model.t_wms_inbound_group_bom.Single(x => x.inbound_order_bom_id == _inbound_order_bom_id);
                    _model.t_wms_inbound_group_bom.Remove(ent_bom);

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
        private void ddlUOM_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == null)
                {
                    return;
                }

                using (var _model = new Source.WMSEntities())
                {
                    t_wms_item_uom uom = _model.t_wms_item_uom.Find(_value);
                    if (uom != null)
                    {
                        if (make_to == "Stock")
                        {
                            txtAttribute1.SetValue("MS-" + uom.uom); ;
                        }
                        hidPackSizeUOMId.SetValue(_value);
                        hidPackSizeUOM.SetValue(uom.uom);
                        txtPackSizeQty.SetValue(uom.conversion_factor);
                        t_wms_item_uom base_uom = _model.t_wms_item_uom.FirstOrDefault(x => x.item_master_id == uom.item_master_id && x.primary_uom == "YES");
                        if (base_uom != null)
                        {
                            txtBaseUnit.SetValue(base_uom.uom);
                        }
                    }
                }
                hidPackSizeUOM.Update();
                txtAttribute1.Update();
                txtPackSizeQty.Update();
                txtBaseUnit.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void TxtMFGDate_TextValueChanged(string _value)
        {
            try
            {
                txtlot.SetValue(txtMFGDate.GetValue() != null ? txtMFGDate.GetValue().Value.ToString("dd-MM-yyyy") : null);
                txtlot.Update();

                if (string.IsNullOrEmpty(_value) || ddlWhItem.GetValue() == null)
                {
                    return;
                }
                
                AddMonthToExpire();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private void ddlPalletUOM_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == null)
                {
                    return;
                }

                using (var _model = new Source.WMSEntities())
                {
                    t_wms_item_uom uom = _model.t_wms_item_uom.Find(_value);
                    if (uom != null)
                    {
                        txtPalletSizeUom.SetValue(uom.uom);
                        txtPalletSizeQty.SetValue(uom.conversion_factor);
                    }
                }
                txtPalletSizeQty.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private void txtMonthToExp_TextEnterChanged(_IInputText obj)
        {
            try
            {
                days_to_expire = Convert.ToInt32(txtMonthToExp.GetValue() ?? 0);
                if (txtMFGDate.GetValue() == null)
                {
                    return ;
                }
                AddMonthToExpire();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private void AddMonthToExpire()
        {
            var mfg_date = txtMFGDate.GetValue();
            var item_days_to_expire = this.days_to_expire;
            var month_to_expire = txtMonthToExp.GetValue();
            if(mfg_date != null)
            {
                if (item_days_to_expire != null)
                {
                    txtExpDate.SetValue(Convert.ToDateTime(mfg_date).AddMonths(item_days_to_expire ?? 0));
                }
                else
                {
                    if (month_to_expire != null)
                    {
                        try
                        {
                            txtExpDate.SetValue(Convert.ToDateTime(mfg_date).AddMonths((int)month_to_expire));
                            this.days_to_expire = (int)month_to_expire;
                        }
                        catch (RuntimeBinderException)
                        {
                            Console.WriteLine("Conversion failed. The dynamic value cannot be converted to int.");
                        }
                    }
                }
                txtExpDate.Update();
            }
        }
        public void RefreshGrid()
        {
            GridExt1.Search();
        }
        #endregion


    }
}