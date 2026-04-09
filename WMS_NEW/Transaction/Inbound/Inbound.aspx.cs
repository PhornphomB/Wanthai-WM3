using _UControls;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Web.UI;
using System.Windows.Input;
using System.Xml.Linq;
using WMS_NEW._ExtensionClass;
using WMS_NEW.Access.Administrator.InterfaceMonitor;
using WMS_NEW.Access.MasterData;
using WMS_NEW.MasterData;
using WMS_NEW.Report.AscxControls;
using WMS_NEW.Source;


namespace WMS_NEW.Transaction.Inbound
{
    public partial class Inbound : PageCustom
    {
        #region ++ DELEGATE ++
        delegate void dg_Search();
        event dg_Search eSearch;
        #endregion


        private Guid owner_id
        {
            get
            {
                if (ViewState["owner_id"] == null)
                    ViewState["owner_id"] = Guid.Empty;

                return (Guid)ViewState["owner_id"];
            }
            set
            {
                ViewState["owner_id"] = value;
            }
        }
        private Guid ref_lpn
        {
            get
            {
                if (ViewState["ref_lpn"] == null)
                    ViewState["ref_lpn"] = Guid.Empty;

                return (Guid)ViewState["ref_lpn"];
            }
            set
            {
                ViewState["ref_lpn"] = value;
            }
        }
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
        private Guid col_wh_master_id
        {
            get
            {
                if (ViewState["col_wh_master_id"] == null)
                    ViewState["col_wh_master_id"] = Guid.Empty;

                return (Guid)ViewState["col_wh_master_id"];
            }
            set
            {
                ViewState["col_wh_master_id"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Deledate Call Back
                eSearch += new dg_Search(Receive_CallBack);
                ucInboundDetail.dg_CallBackSearch = eSearch;
                #endregion
                ucReportViewer.BindingParameter += UcReportViewer_BindingParameter;
                #region Binding DropDown Lazy

                #region Grid Column
                iColWarehouse.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
                iColOwner.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                iColOrderType.DropDownQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("inbound_order_type"); };
                iColOrderStatus.DropDownQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("inbound_order_status"); };
                iColSupplier.DropDownQueryProperty = delegate () { return Access.MasterData.Supplier.Instance.GetQuery(); };
                iColCustomer.DropDownQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQuery(); };


                #endregion

                #region Grid Custom
                ddlCategory.MethodQueryProperty = delegate () { return new Access.MasterData.ItemCategory().GetQuery(); };
                ddOrderType.MethodQueryProperty = delegate () { return Access.Transaction.Inbound.InboundMaster.Instance.GetQueryCode("inbound_order_type", Request.QueryString["pagetype"]); };
                #endregion

                #region Form Page
                ddlWarehouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
                ddlOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                ddlOrderType.MethodQueryProperty = delegate () { return Access.Transaction.Inbound.InboundMaster.Instance.GetQueryCode("inbound_order_type", Request.QueryString["pagetype"]); };
                ddlSupplier.MethodQueryProperty = delegate () { return Access.MasterData.Supplier.Instance.GetQuery(this.owner_id); };
                ddlCustomer.MethodQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQuery(this.owner_id); };
                ddlMaketo.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("inbound_make_to"); };
                //ddlProductionLine.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("production_line"); };
                if (ddlWarehouse.MethodQueryProperty().ToList().Count == 1)
                {
                    this.wh_master_id = ddlWarehouse.MethodQueryProperty().Select(w => w.guid_member).FirstOrDefault();
                }
                ddlProductionLine.MethodQueryProperty = delegate () { return Access.MasterData.ProductionLine.Instance.GetQuery(this.wh_master_id); };
                if (iColWarehouse.DropDownQueryProperty().ToList().Count == 1)
                {
                    this.col_wh_master_id = iColWarehouse.DropDownQueryProperty().Select(w => w.guid_member).FirstOrDefault();
                }
                iColProductionLine.DropDownQueryProperty = delegate () { return Access.MasterData.ProductionLine.Instance.GetQuery(this.col_wh_master_id); };

                BindDdlReference();

                iColWarehouse.DropDownPostValueChanged = iColWarehouse_DropDownPostValueChanged;
                ddlWarehouse.PostValueChanged = ddlWarehouse_PostValueChanged;
                ddlOrderType.PostValueChanged = ddlOrderType_PostValueChanged;


                

                #endregion

                #endregion

                #region Init PopupEntity 

                popupEx.InitObjectsEvent += () => { popupEx.ObjectDataAccess = new Access.Transaction.Inbound.InboundMaster(); };
                popupEx.InitControlStatic();

                GridExt1.PopupEntitySource = popupEx;

                #endregion

                #region Event 
                PanelTab1.TabIndexChanged += PanelTab1_TabIndexChanged;
                chkGenOrderNo.PostValueChanged = chkGenOrderNo_PostValueChanged;
                popupEx.AfterNewDataEvent += PopupEx_AfterNewDataEvent;
                popupEx.AfterSetEditDataEvent += PopupEx_AfterSetEditDataEvent;
                popupEx.PreSaveEntityEvent += PopupEx_PreSaveEntityEvent;
                popupEx.ValidateEntityEvent += PopupEx_ValidateEntityEvent;
                popupEx.RaiseEntitySaved += PopupEx_RaiseEntitySaved;
                
                GridExt1.GridRowCanDeleteValidate += GridExt1_GridRowCanDeleteValidate;
                ddlOwner.PostValueChanged = ddlOwner_PostValueChanged;
                ddlMaketo.PostValueChanged = ddlMaketo_PostValueChanged;
                GridExt1.GridExportTemplate += GridExt1_GridExportTemplate;
                GridExt1.GridPreExportData += GridExt1_GridPreExportData;
                #endregion



                if (!Page.IsPostBack)
                {
                    #region Set Initial Filter Grid

                    var ucFirstLoad = new _UControls.InputHidden() { DataFieldValue = "_is_first_load" };
                    ucFirstLoad.SetValue(true);
                    GridExt1.AddFilterCustomInputInclude(ucFirstLoad);
                    txtPageType.SetValue(Request.QueryString["pagetype"]);

                    #endregion
                    //iColWarehouse.DropDownSelectedValue(Guid.Parse("DE233E82-1F9A-47B9-A85B-F5541F9F28EA"));

                    //txtInboundNo.IsPrimary = true;
                }


            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        private void GridExt1_GridPreExportData(ref System.Data.DataTable dtExport)
        {
            if (dtExport.Rows.Count > 0)
            {
                foreach (DataRow row in dtExport.Rows)
                {
                    row["item_number"] = row["item_number"].ToString().Replace("<br>", "\r\n");
                }
            }
        }

        private bool PopupEx_ValidateEntityEvent()
        {
            ref_lpn = ddlRefLPN.GetValue() ?? Guid.Empty;
            if (ref_lpn != Guid.Empty)
            {
                using (var model = new Source.WMSEntities())
                {
                    var print = (from m in model.t_wms_outbound_master
                                 join d in model.t_wms_outbound_detail on m.outbound_order_master_id equals d.outbound_order_master_id
                                 join p in model.t_wms_outbound_pick_detail on d.outbound_order_detail_id equals p.outbound_order_detail_id
                                 join l in model.t_wms_print_label on p.lpn equals l.lpn
                                 where m.outbound_order_master_id == ref_lpn
                                 select l).ToList();
                    if (print.Count == 0)
                    {
                        Page.MessageWarning(clsResource.GetResource("inbound", "validate_no_item_available_for_repack"));
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }  
            }
            else
            {
                return true;
            }


        }

        private void Receive_CallBack()
        {
            try
            {
                GridExt1.Search();
                Get_Summary();
                UpdateOrderStatus();
                ucReceivePartial.Refresh_Data();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void PanelTab1_TabIndexChanged(int _index)
        {
            if (popupEx.FormState == _UControls.FormState.Edit && _index == 5)
            {
                ucReceiptDetail.Search(popupEx.KeyFieldValue);
            }
        }

        #region Report
        protected void btReport_Click(object sender, EventArgs e)
        {
            try
            {
                ucReportViewer.ShowDialog();
                ucReportViewer.InitialForm("InboundHeader");

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private List<Report.AscxControls.ReportParameter> UcReportViewer_BindingParameter(string _report_id)
        {
            var prms = new List<ReportParameter>();

            GridExt1.SetGridFilterForSearch();
            var filters = GridExt1.FilterGridOptions;

            GridExt1.SetCustomFilterForSearch();
            var entCate = GridExt1.FilterCtrlOptions.Where(w => w.DataFieldValue == ddlCategory.DataFieldValue).FirstOrDefault();
            if (entCate != null && entCate.Value != null)
            {
                filters.Add(new Prototype.Providers.Filter("category_id", Prototype.Providers.FilterAt.Equal, entCate.Value.ToString()));
            }

            var entItem = GridExt1.FilterCtrlOptions.Where(w => w.DataFieldValue == txtItemNumber.DataFieldValue).FirstOrDefault();
            if (entItem != null && entItem.Value != null)
            {
                filters.Add(new Prototype.Providers.Filter("item_number", Prototype.Providers.FilterAt.Contains, entItem.Value.ToString()));
            }

            var entOrderType = GridExt1.FilterCtrlOptions.Where(w => w.DataFieldValue == ddlOrderType.DataFieldValue).FirstOrDefault();
            if (entOrderType != null && entOrderType.Value != null)
            {
                filters.Add(new Prototype.Providers.Filter("order_type", Prototype.Providers.FilterAt.Equal, entOrderType.Value.ToString()));
            }

            //var _condition = " AND " + Prototype.Providers.DataFilterSQL.GetSQLCondition(filters).Replace("'", "*").Replace("=", "$").Replace("%", "[tnecrep]");
            //var _condition = Prototype.Providers.DataFilterSQL.GetSQLCondition(filters).Replace("'", "*").Replace("=", "$").Replace("%", "[tnecrep]");
            var _condition = Prototype.Providers.DataFilterSQL.GetSQLCondition(filters).Replace("'", "*").Replace("=", "$").Replace("%", "[tnecrep]").Replace("#", "%23");
            if (!string.IsNullOrEmpty(_condition))
            {
                _condition = " AND " + _condition;
            }
            prms.Add(new ReportParameter() { Name = "@prmCondition", Value = _condition });

            return prms;
        }
        #endregion

        #region Popup Event

        private void PopupEx_PreSaveEntityEvent()
        {
            try
            {
                var _objectEntity = (popupEx.ObjectDataAccess as Access.Transaction.Inbound.InboundMaster).Entity;

                _objectEntity.wh_id = ddlWarehouse.GetText().Split(':')[0].Trim();
                _objectEntity.owner_code = ddlOwner.GetText().Split(':')[0].Trim();
                using (var model = new Source.WMSEntities())
                {
                    if (popupEx.FormState == _UControls.FormState.Edit)
                    {
                        Guid inbound_order_master_id = (Guid)popupEx.KeyFieldValue;
                        var ent = model.t_wms_inbound_master.Where(w => w.inbound_order_master_id == inbound_order_master_id).FirstOrDefault();
                        if (ent != null)
                        {
                            _objectEntity.order_status = ent.order_status;
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

        private void PopupEx_AfterNewDataEvent()
        {
            try
            {

                txtOrderStatus.SetValue(_ExtensionClass.InboundOrderStatus.OPEN);
                txtCreateBy.SetValue(_SessionVals.UserName);

                lblSumPlanQTY.InnerText = "0.00";
                lblSumReceiveQTY.InnerText = "0.00";
                lblSumPrintQTY.InnerText = "0.00";


                btCloseOrder.Enabled = false;
                popupEx.EnableSave = true;
                txtRefInboundOrderNumber.Enabled = true;
                ddlRefLPN.Enabled = true;

                //Rule Running
                using (var model = new Source.WMSEntities())
                {
                    var entRule = model.t_wms_rule.Where(w => w.rule_code.ToUpper() == "INBOUND_ORDER_RUNNING" && w.is_active == "YES").FirstOrDefault();
                    if (entRule != null)
                    {
                        bool isRule = entRule.value.ToUpper() == "YES" ? true : false;
                        chkGenOrderNo.Checked = isRule;
                        chkGenOrderNo_PostValueChanged(isRule);

                    }
                }
                chkGenOrderNo.Enabled = true;
                ddlMaketo.Enabled = true;

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void PopupEx_AfterSetEditDataEvent()
        {
            try
            {
                var _objectEntity = (popupEx.ObjectDataAccess as Access.Transaction.Inbound.InboundMaster).Entity;
                hidRefInboundOrderNumber.SetValue(_objectEntity.ref_inbound_order_number);
                chkGenOrderNo.Enabled = false;
                ddlMaketo.Enabled = false;
                btCloseOrder.Enabled = true;
                txtRefInboundOrderNumber.Enabled = false;
                ddlRefLPN.Enabled = false;
                Get_Summary();
                Control_Close();
                using(var _Model = new WMSEntities())
                {
                    var detail = _Model.t_wms_inbound_detail.FirstOrDefault(x => x.ref_lpn != null && x.inbound_order_master_id == _objectEntity.inbound_order_master_id);
                    if(detail != null)
                    {
                        var pick = _Model.t_wms_outbound_pick_detail.FirstOrDefault(x => x.lpn == detail.ref_lpn);
                        if(pick != null)
                        {
                            var outDetail = _Model.t_wms_outbound_detail.FirstOrDefault(x => x.outbound_order_detail_id == pick.outbound_order_detail_id);
                            if(outDetail != null)
                            {
                                ddlRefLPN.SetValue(outDetail.outbound_order_master_id);
                                ddlRefLPN.Update();
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
        }
        private void PopupEx_RaiseEntitySaved(bool _saveStatus)
        {
            if (_saveStatus)
            {
                var _objectEntity = (popupEx.ObjectDataAccess as Access.Transaction.Inbound.InboundMaster).Entity;
                if (_objectEntity != null)
                {
                    using (var model = new Source.WMSEntities())
                    {
                        if (hidRefInboundOrderNumber.GetValue() != _objectEntity.ref_inbound_order_number)
                        {
                            var entDetail = model.t_wms_inbound_detail.Where(w => w.inbound_order_master_id == _objectEntity.inbound_order_master_id).ToList();
                            if (entDetail.Count > 0)
                            {
                                entDetail.ForEach(w => { w.attribute1 = _objectEntity.ref_inbound_order_number; });
                            }
                            var entPrintLabel = model.t_wms_print_label.Where(w => w.inbound_order_master_id == _objectEntity.inbound_order_master_id).ToList();
                            if (entPrintLabel.Count > 0)
                            {
                                entPrintLabel.ForEach(w => { w.attribute1 = _objectEntity.ref_inbound_order_number; });
                            }
                            
                        }
                        hidRefInboundOrderNumber.SetValue(_objectEntity.ref_inbound_order_number);

                        if (ref_lpn != Guid.Empty && !model.t_wms_inbound_detail.Any(a => a.inbound_order_master_id == _objectEntity.inbound_order_master_id))
                        {
                            InsertRepackDetail(model, _objectEntity);
                        }
                        if(ref_lpn != Guid.Empty)
                        {
                            ddlRefLPN.SetValue(ref_lpn);
                            ddlRefLPN.Update();
                        }
                        ucInboundDetail.RefreshGrid();
                    }
                }
            }
        }

        #endregion
        void InsertRepackDetail(WMSEntities _model, t_wms_inbound_master master)
        {
            List<t_wms_inbound_detail> _Inbound_Details = new List<t_wms_inbound_detail>();
            List<t_wms_print_label> _Print_Labels = new List<t_wms_print_label>();
            var entOutboundDetail = _model.t_wms_outbound_detail.Where(w => w.outbound_order_master_id == ref_lpn).ToList();
            foreach (var detail in entOutboundDetail)
            {
                var entPickDetail = _model.t_wms_outbound_pick_detail.Where(w => w.outbound_order_detail_id == detail.outbound_order_detail_id && w.quantity_ship > 0).GroupBy(g => g.lpn).Select(group => new DTOPickDetail
                {
                    lpn = group.Key,
                    quantity_ship = group.Sum(g => g.quantity_ship),
                    wh_item_master_id = group.Select(g => g.wh_item_master_id).FirstOrDefault(),
                    mfg_date = group.Select(g => g.mfg_date).FirstOrDefault(),
                    expiry_date = group.Select(g => g.expiry_date).FirstOrDefault(),
                    serial_number = group.Select(g => g.serial_number).FirstOrDefault(),
                    attribute1 = group.Select(g => g.attribute1).FirstOrDefault(),
                    attribute2 = group.Select(g => g.attribute2).FirstOrDefault(),
                    attribute3 = group.Select(g => g.attribute3).FirstOrDefault(),
                    attribute4 = group.Select(g => g.attribute4).FirstOrDefault(),
                    attribute5 = group.Select(g => g.attribute5).FirstOrDefault(),

                });
                foreach(var pick_detail in entPickDetail)
                {
                    var conversion = (decimal)(detail.pack_size_conversion_factor);
                    var qty_order = Math.Floor((decimal)pick_detail.quantity_ship / conversion);
                    var remain = (decimal)pick_detail.quantity_ship - (conversion * qty_order);
                    setInboundDetail(_model, master, detail, pick_detail, qty_order * conversion, true, ref _Inbound_Details, ref _Print_Labels);
                    if(remain > 0)
                    {
                        setInboundDetail(_model, master, detail, pick_detail, remain, false, ref _Inbound_Details, ref _Print_Labels);
                    }
                }
            }
            _model.t_wms_inbound_detail.AddRange(_Inbound_Details);
            _model.t_wms_print_label.AddRange(_Print_Labels);
            try
            {
                _model.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        void setInboundDetail(WMSEntities _model, t_wms_inbound_master master, t_wms_outbound_detail outbound_detail, DTOPickDetail pickDetail, decimal quantity_order, bool is_pack, ref List<t_wms_inbound_detail> lstInbound, ref List<t_wms_print_label> lstPrint)
        {
            t_wms_print_label print = _model.t_wms_print_label.FirstOrDefault(x => x.lpn == pickDetail.lpn);
            var uomBase = _model.t_wms_item_uom.Where(w => w.item_master_id == outbound_detail.item_master_id && w.primary_uom == "YES").FirstOrDefault();
            var pack_size = _model.t_wms_item_uom.Where(w => w.item_master_id == outbound_detail.item_master_id && w.uom == outbound_detail.pack_size_uom && w.conversion_factor == outbound_detail.pack_size_conversion_factor).FirstOrDefault();
            var pallet_size = _model.t_wms_item_uom.Where(w => w.item_master_id == outbound_detail.item_master_id && w.uom == print.pallet_size_uom && w.conversion_factor == print.pallet_size_conversion_factor).FirstOrDefault();

            t_wms_inbound_detail _Inbound_Detail = new t_wms_inbound_detail
            {
                inbound_order_detail_id = Guid.NewGuid(),
                inbound_order_master_id = master.inbound_order_master_id,
                wh_item_master_id = pickDetail.wh_item_master_id,
                item_master_id = outbound_detail.item_master_id,
                item_uom_id = outbound_detail.item_uom_id,
                item_number = outbound_detail.item_number,
                line_number = lstInbound.Count == 0 ? "000001" : (lstInbound.Count + 1).ToString("000000"),
                quantity_order = Convert.ToDouble(quantity_order),
                quantity_receive = 0,
                uom = outbound_detail.uom,
                item_status = "NEW",
                default_item_status = "Quarantine",
                price = outbound_detail.price,
                grade = outbound_detail.grade,
                over_receipt_allowed = "NO",
                over_receipt_percentage = null,
                source_of_record = null,
                lot_number = Convert.ToDateTime(pickDetail.mfg_date).ToString("dd-MM-yyyy"),
                mfg_date = pickDetail.mfg_date,
                expiry_date = pickDetail.expiry_date,
                serial_number = pickDetail.serial_number,
                create_by = _SessionVals.UserName,
                create_date = DateTime.Now,
                production_line = master.production_line,
                ref_outbound_order_number = master.ref_outbound_order_number,
                pack_size_uom_id = is_pack ? pack_size.item_uom_id : uomBase.item_uom_id,
                pack_size_uom = is_pack ? outbound_detail.pack_size_uom : uomBase.uom,
                pack_size_conversion_factor = is_pack ? outbound_detail.pack_size_conversion_factor : uomBase.conversion_factor,
                pallet_size_uom_id = pallet_size.item_uom_id,
                pallet_size_uom = pallet_size.uom,
                pallet_size_conversion_factor = pallet_size.conversion_factor,
                attribute1 = is_pack ? (master.make_to.ToUpper() == "STOCK" ? "MS-" + pack_size.uom : pickDetail.attribute1) : (master.make_to.ToUpper() == "STOCK" ? "MS-" + uomBase.uom : pickDetail.attribute1),
                attribute2 = pickDetail.attribute2,
                attribute3 = pickDetail.attribute3,
                attribute4 = pickDetail.attribute4,
                attribute5 = pickDetail.attribute5,
                ref_lpn = pickDetail.lpn,
            };
            lstInbound.Add(_Inbound_Detail);

            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));
            var errMsg_pallet = new ObjectParameter("out_ErrorMessage", typeof(string));

            var _lpn = string.Empty;
            var generate = _model.usp_wms_generate_lpn(master.inbound_order_number, _Inbound_Detail.production_line, errMsg);
            if (generate != null)
            {
                _lpn = generate.FirstOrDefault();
            }

            var _pallet_seq = 0;
            var generate_pallet_seq = _model.usp_wms_generate_pallet_seq(master.wh_master_id, _Inbound_Detail.production_line, _Inbound_Detail.mfg_date, _Inbound_Detail.item_number, errMsg);
            if (generate_pallet_seq != null)
            {
                _pallet_seq = generate_pallet_seq.FirstOrDefault() ?? 0;
            }

            t_wms_print_label _Print_Label = new t_wms_print_label();
            _Print_Label.print_label_id = Guid.NewGuid();
            _Print_Label.wh_id = master.wh_id;
            _Print_Label.inbound_order_master_id = master.inbound_order_master_id;
            _Print_Label.inbound_order_number = master.inbound_order_number;
            _Print_Label.inbound_order_detail_id = _Inbound_Detail.inbound_order_detail_id;
            _Print_Label.line_number = _Inbound_Detail.line_number;
            _Print_Label.row_print = lstInbound.Count;
            _Print_Label.lpn = _lpn;
            _Print_Label.wh_item_master_id = _Inbound_Detail.wh_item_master_id;
            _Print_Label.item_number = _Inbound_Detail.item_number;
            _Print_Label.item_description = _model.t_wms_item.FirstOrDefault(x => x.item_master_id == outbound_detail.item_master_id).description;
            _Print_Label.quantity_order = _Inbound_Detail.quantity_order;
            _Print_Label.item_uom_id = _Inbound_Detail.item_uom_id;
            _Print_Label.uom = _Inbound_Detail.uom;
            _Print_Label.lot_number = Convert.ToDateTime(_Inbound_Detail.mfg_date).ToString("dd-MM-yyyy");
            _Print_Label.mfg_date = _Inbound_Detail.mfg_date;

            DateTime result;
            try
            {
                result = DateTime.ParseExact(_Inbound_Detail.expiry_date, "yyyyMMdd", null);
                _Print_Label.expiry_date = result;
            }
            catch (FormatException)
            {
                Page.MessageWarning("expiry_date Invalid date format");
            }

            _Print_Label.attribute1 = _Inbound_Detail.attribute1;
            _Print_Label.production_line = _Inbound_Detail.production_line;
            _Print_Label.pack_size_per_pallet = (_Inbound_Detail.quantity_order / _Inbound_Detail.pack_size_conversion_factor);
            _Print_Label.pack_size_uom = _Inbound_Detail.pack_size_uom;
            _Print_Label.pack_size_conversion_factor = _Inbound_Detail.pack_size_conversion_factor;
            _Print_Label.pallet_size_uom = _Inbound_Detail.pallet_size_uom;
            _Print_Label.pallet_size_conversion_factor = _Inbound_Detail.pallet_size_conversion_factor;
            _Print_Label.is_received = "NO";
            _Print_Label.is_print = "NO";
            _Print_Label.create_by = _Inbound_Detail.create_by;
            _Print_Label.create_date = _Inbound_Detail.create_date;
            _Print_Label.ref_lpn = _Inbound_Detail.ref_lpn;
            _Print_Label.pallet_seq = _pallet_seq;
            _Print_Label.is_cancelled = "NO";
            lstPrint.Add(_Print_Label);
        }
        private class DTOPickDetail
        {
            public string lpn { get; set; }
            public double? quantity_ship { get; set; }
            public Guid wh_item_master_id { get; set; }
            public DateTime? mfg_date { get; set; }
            public string expiry_date { get; set; }
            public string serial_number { get; set; }
            public string attribute1 { get; set; }
            public string attribute2 { get; set; }
            public string attribute3 { get; set; }
            public string attribute4 { get; set; }
            public string attribute5 { get; set; }

        }
        #region Control Event
        void chkGenOrderNo_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == true)
                {
                    var _no = string.Empty;

                    using (var _acc = new WMS_NEW.Access.FunctionGenerate())
                    {

                        _no = _acc.GetGenerateRunning("IN");
                    }
                    txtInboundNo.SetValue(_no);
                    txtInboundNo.Enabled = false;
                }
                else
                {
                    txtInboundNo.Clear();
                    txtInboundNo.Enabled = true;
                    txtInboundNo.MaxLength = 10;
                }

                txtInboundNo.Update();
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
                this.owner_id = _value ?? Guid.Empty;
                ddlCustomer.BindDataSource();
                ddlSupplier.BindDataSource();

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        void ddlMaketo_PostValueChanged(dynamic _value)
        {
            try
            {
                string value = (Convert.ToString(_value ?? ""));
                if (value.ToUpper() == "STOCK")
                {
                    txtRefInboundOrderNumber.Enabled = false;
                    txtRefInboundOrderNumber.SetValue("MS");
                }
                else
                {
                    txtRefInboundOrderNumber.Enabled = true;
                    if(popupEx.FormState == FormState.New)
                    {
                        txtRefInboundOrderNumber.Clear();
                    }
                }

                if (ddlOrderType.GetValue() != null)
                {
                    ddlOrderType_PostValueChanged(ddlOrderType.GetValue());
                }
                //else
                //{
                //    if (value.ToUpper() == "STOCK")
                //    {
                //        ddlCustomer.IsPrimary = false;
                //    }
                //    else if(value.ToUpper() == "ORDER")
                //    {
                //        ddlCustomer.IsPrimary = true;
                //    }

                //    ddlCustomer.Update();
                //}

                txtRefInboundOrderNumber.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        protected void btCloseOrder_Click(object sender, EventArgs e)
        {
            try
            {
                popupOrderClose.ShowDialog();
                txtCloseRemark.SetValue("");
                txtInboundOrderNoClose.SetValue(txtInboundNo.GetValue());
                
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        protected void btSendLims_Click(object sender, EventArgs e)
        {
            try
            {
                if (SendLIMS())
                {
                    Page.MessageSuccess("Send LIMS Success");
                }
                else
                {
                    Page.MessageWarning("Send LIMS Fail");
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private bool SendLIMS()
        {
            try
            {
                using (var model = new WMSEntities())
                {
                    model.usp_inf_lims_inbound_order();
                }
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                // สร้างข้อความแสดง Validation Error
                var validationErrors = ex.EntityValidationErrors
                    .SelectMany(eve => eve.ValidationErrors)
                    .Select(ve => $"Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                var errorMessage = string.Join("; ", validationErrors);
                this.Logging = new Prototype.Providers.Logging(this, new Exception(errorMessage));
                this.RaiseLogging();
                return false;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
                return false;
            }
        }
        protected void btComfirmClose_Click(object sender, EventArgs e)
        {
            try
            {
                Validate_Close_Order();
            }
            catch (SqlException ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btCopy_Click(object sender, EventArgs e)
        {
            try
            {
                if (popupEx.KeyFieldValue != null)
                {
                    var ref_outbound_order_number = txtRefOutboundOrderNumber.GetValue();
                    Guid inbound_order_master_id = (Guid)popupEx.KeyFieldValue;
                    int detailCount = 0;
                    using (var model = new Source.WMSEntities())
                    {
                        var ent = model.t_wms_inbound_master.Where(w => w.inbound_order_master_id == inbound_order_master_id).FirstOrDefault();
                        if (ent != null)
                        {
                            ent.ref_outbound_order_number = ref_outbound_order_number;
                        }
                        var entDetail = model.t_wms_inbound_detail.Where(w => w.inbound_order_master_id == inbound_order_master_id).ToList();
                        if (entDetail.Count > 0)
                        {
                            entDetail.ForEach(w => { w.ref_outbound_order_number = ref_outbound_order_number; });
                        }
                        detailCount = entDetail.Count;
                        model.SaveChanges();
                    }
                    if(detailCount > 0)
                    {
                        GridExt1.Search();
                    }
                    else
                    {
                        Page.MessageWarning("There is no data to copy.");
                    }
                }
                else
                {
                    Page.MessageWarning("There is no data to copy.");
                }
                
            }
            catch (SqlException ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private bool GridExt1_GridRowCanDeleteValidate(System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {
                var order_status = (string)DataBinder.Eval(e.Row.DataItem, "order_status");
                if ((order_status != _ExtensionClass.InboundOrderStatus.OPEN))
                {
                    return false;
                }
                var KeyId = (Guid)DataBinder.Eval(e.Row.DataItem, "KeyId");
                using (var model = new Source.WMSEntities())
                {
                    var ent = model.t_wms_print_label.Where(x => x.inbound_order_master_id == KeyId && ((x.is_cancelled == "NO" && x.is_interface_hana == "YES") || x.is_print == "YES"));
                    if (ent.Count() > 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();

                return false;
            }
        }

        void BindDdlReference()
        {
            if (popupEx.FormState == FormState.New)
            {
                ddlRefLPN.MethodQueryProperty = delegate () { return Access.Transaction.Inbound.InboundMaster.Instance.GetOutboundRef(); };
            }
            if (popupEx.FormState == FormState.Edit)
            {
                ddlRefLPN.MethodQueryProperty = delegate () { return Access.Transaction.Inbound.InboundMaster.Instance.GetOutboundRefAll(); };
            }
            ddlRefLPN.Update();
        }

        #endregion

        #region Function
        void Validate_Close_Order()
        {
            using (var _acc = new WMS_NEW.Access.Transaction.Inbound.InboundMaster())
            {
                PlugEventResult(_acc);

                string _appID = _SessionVals.AppID;
                Guid _wh_master_id = ddlWarehouse.GetValue();
                Guid _inbound_order_master_id = (Guid)popupEx.KeyFieldValue;
                string _deviceID = _SessionVals.DeviceID;
                string _userID = _SessionVals.UserName;
                string _errIsConfirm = string.Empty;
                string _errCode = string.Empty;
                string _errMsg = string.Empty;

                _acc.ValidateCloseOrder(_appID, _wh_master_id, _inbound_order_master_id, _userID, out _errIsConfirm, out _errCode, out _errMsg);
                
                if (_errCode == "0")
                {
                    hidIsConfirm.SetValue(_errIsConfirm);
                    hidIsConfirm.Update();

                    if (_errIsConfirm == "YES")
                    {
                        lblValidateCloseOrder.Text = _SessionVals.LocaleID == "1033" ? "Duplicate Customer Order in outbound, Do you need to add items to the Customer Order?" : "Customer Order ซ้ำในเอกสารจ่ายสินค้า, ต้องการเพิ่มรายการสินค้าใน Customer Order หรือไม่";
                        popupConfirmCloseOrder.ShowDialog();
                    }
                    else
                    {
                        Close_Order();
                    }
                }
                else
                {
                    Page.MessageWarning(_errMsg);
                }


            }
        }
        protected void btnComfirmCloseOrder_Click(object sender, EventArgs e)
        {
            try
            {
                Close_Order();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        void Close_Order()
        {
            using (var _acc = new WMS_NEW.Access.Transaction.Inbound.InboundMaster())
            {
                PlugEventResult(_acc);

                string _appID = _SessionVals.AppID;
                Guid _wh_master_id = ddlWarehouse.GetValue();
                Guid _inbound_order_master_id = (Guid)popupEx.KeyFieldValue;
                string _order_number = txtInboundNo.GetValue();
                string _close_remark = txtCloseRemark.GetValue();
                string _confirm_meke_to_order = hidIsConfirm.GetValue();
                string _deviceID = _SessionVals.DeviceID;
                string _userID = _SessionVals.UserName;
                string _errCode = string.Empty;
                string _errMsg = string.Empty;

                _acc.CloseOrder(_appID, _wh_master_id, _inbound_order_master_id, _close_remark, _confirm_meke_to_order, _deviceID, _userID, out _errCode, out _errMsg);

                if (_errCode == "0")
                {
                    popupOrderClose.HideDialog();
                    popupEx.HideDialog();
                    popupConfirmCloseOrder.HideDialog();
                    GridExt1.DataBind();
                    UpdateOrderStatus();
                    if (SendLIMS())
                    {
                        Page.MessageSuccess("Send LIMS Success");
                    }
                    else
                    {
                        Page.MessageWarning("Send LIMS Fail");
                    }
                }
                else
                {
                    Page.MessageWarning(_errMsg);
                }

            }
        }

        void Get_Summary()
        {
            using (var _acc = new WMS_NEW.Access.Transaction.Inbound.InboundDetail())
            {
                base.PlugEventResult(_acc);

                var order_result = _acc.Get_OrderSummary((Guid)popupEx.KeyFieldValue);
                var receive_result = _acc.Get_ReceiveSummary((Guid)popupEx.KeyFieldValue);
                var print_result = _acc.Get_PrintSummary((Guid)popupEx.KeyFieldValue);
                if (order_result != null)
                {
                    lblSumPlanQTY.InnerText = order_result.PlanQuantity == 0 ? "0" : order_result.PlanQuantity.ToString(Extensions.FormatDecimal);
                }
                else
                {
                    lblSumPlanQTY.InnerText = "0";
                }
                if (receive_result != null)
                {
                    lblSumReceiveQTY.InnerText = receive_result.ReceiveQuantity == 0 ? "0" : receive_result.ReceiveQuantity.ToString(Extensions.FormatDecimal);
                }
                else
                {
                    lblSumReceiveQTY.InnerText = "0";
                }
                if (print_result != null)
                {
                    lblSumPrintQTY.InnerText = print_result.PrintQuantity == 0 ? "0" : print_result.PrintQuantity.ToString(Extensions.FormatDecimal);
                }
                else
                {
                    lblSumPrintQTY.InnerText = "0";
                }
                update_summary.Update();
            }
        }

        void UpdateOrderStatus()
        {
            if (popupEx.FormState == _UControls.FormState.Edit)
            {
                using (var model = new Source.WMSEntities())
                {
                    var inbound_order_master_id = (Guid)popupEx.KeyFieldValue;
                    var ent = model.t_wms_inbound_master.Where(w => w.inbound_order_master_id == inbound_order_master_id).FirstOrDefault();
                    if (ent != null)
                    {
                        txtOrderStatus.SetValue(ent.order_status);
                        txtOrderStatus.Update();
                    }
                }
            }
        }

        void Control_Close()
        {

            if (txtOrderStatus.GetValue().ToUpper() == "CLOSE")
            {
                popupEx.EnableSave = false;
                btCloseOrder.Enabled = false;

            }
            else
            {
                popupEx.EnableSave = true;
                btCloseOrder.Enabled = true;
            }


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

                if (/*dt.Columns["Warehouse"].IsEmpty() || dt.Columns["Owner"].IsEmpty() ||*/ dt.Columns["Inbound Order Number"].IsEmpty() || dt.Columns["Order Type"].IsEmpty() || dt.Columns["Supplier Code"].IsEmpty() || dt.Columns["Customer Code"].IsEmpty() || dt.Columns["Make To"].IsEmpty() || dt.Columns["Reference Inbound Order Group"].IsEmpty() || dt.Columns["Order Date"].IsEmpty() || dt.Columns["Item Number"].IsEmpty() || /*dt.Columns["Item Description"].IsEmpty() ||*/ dt.Columns["MFG Date"].IsEmpty() || dt.Columns["Month To Expire"].IsEmpty() || dt.Columns["Order Qty"].IsEmpty() || dt.Columns["UOM"].IsEmpty() || dt.Columns["Pallet UOM"].IsEmpty() || dt.Columns["Production Line"].IsEmpty() || dt.Columns["Ref Outbound Order Number"].IsEmpty() || dt.Columns["Ref Item Number"].IsEmpty() || dt.Columns["Create By"].IsEmpty() /*|| dt.Columns["Attribute 1"].IsEmpty()*/ || dt.Columns["Attribute 2"].IsEmpty() || dt.Columns["Attribute 3"].IsEmpty() || dt.Columns["Attribute 4"].IsEmpty() || dt.Columns["Attribute 5"].IsEmpty())
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
                                List<Source.t_wms_inbound_master> _listMaster = new List<Source.t_wms_inbound_master>();
                                List<Source.t_wms_inbound_detail> _listInbound = new List<Source.t_wms_inbound_detail>();
                                List<Source.t_wms_print_label> _listPrintLabel = new List<Source.t_wms_print_label>();
                                List<Source.t_wms_inbound_master> inExcel = new List<Source.t_wms_inbound_master>();

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


                                            //string _warehouse = dr[0].ToString().Trim();
                                            //string _owner = dr[1].ToString().Trim();
                                            string _inbound_oreder_number = dr[0].ToString().Trim();
                                            string _order_type = dr[1].ToString().Trim();
                                            string _supplier = dr[2].ToString().Trim();
                                            string _customer = dr[3].ToString().Trim();
                                            string _make_to = dr[4].ToString().Trim();
                                            string _ref_inbound_order_group = dr[5].ToString().Trim();
                                            string _create_date = dr[6].ToString().Trim();

                                            string _item_number = dr[7].ToString().Trim();
                                            //string _description = dr[10].ToString().Trim();
                                            string _mfg_date = dr[8].ToString().Trim();
                                            string _month_to_exp = dr[9].ToString().Trim();
                                            string _quantity_order = dr[10].ToString().Trim();
                                            string _uom = dr[11].ToString().Trim();
                                            string _pallet_uom = dr[12].ToString().Trim();
                                            string _production_line = dr[13].ToString().Trim();
                                            string _ref_outbound_order_number = dr[14].ToString().Trim();
                                            string _ref_item_number = dr[15].ToString().Trim();
                                            string _create_by = dr[16].ToString().Trim();

                                            //string _attri1 = "";
                                            string _attri2 = dr[17].ToString().Trim();
                                            string _attri3 = dr[18].ToString().Trim();
                                            string _attri4 = dr[19].ToString().Trim();
                                            string _attri5 = dr[20].ToString().Trim();
                                            // Guid _inbound_master_id = Guid.Empty;

                                            var supplier = _model.t_wms_supplier.Where(w => w.supplier_code == _supplier || w.supplier_name == _supplier).FirstOrDefault();
                                            var customer = _model.t_wms_customer.Where(w => w.customer_code == _customer || w.customer_name == _customer).FirstOrDefault();
                                            var productionLine = _model.t_wms_production_line.FirstOrDefault(w => w.production_line == _production_line || w.description == _production_line);
                                            if(productionLine == null)
                                            {
                                                string errItemCode = "ไม่พบข้อมูล Production Line ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            var warehouse = _model.t_wms_wh.Where(w => w.wh_master_id == productionLine.wh_master_id).FirstOrDefault();
                                            var owner = _model.t_wms_owner.FirstOrDefault();
                                            var ordertype = _model.t_com_combobox_item.Where(w => w.is_active == "YES" && w.group_name == "inbound_order_type" && w.display_member == _order_type).FirstOrDefault();

                                            if (ordertype == null)
                                            {
                                                string errItemCode = "ไม่พบข้อมูล Order Type ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }

                                            string[] rules = _model.t_wms_rule.Where(x => x.rule_code == "RETURN_MODERN_TRADE" && x.is_active == "YES").Select(se => se.value).ToArray();
                                            if (rules.Contains(_order_type))
                                            {
                                                if(string.IsNullOrEmpty(_customer))
                                                {
                                                    string errItemCode = $"กรุณาระบุ Customer เมื่อทำการเลือก Order Type: {_order_type}"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                    _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                    continue;
                                                }
                                            }

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
                                            var palletUOM = _model.t_wms_item_uom.Where(w => w.item_master_id == item.item_master_id && w.is_pallet_uom == "YES").ToList();
                                            if (palletUOM.Count == 0)
                                            {
                                                string errItemCode = $"ไม่มีข้อมูล Pallet uom สำหรับ Item Number: {_item_number}"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            else if (palletUOM.Count == 1)
                                            {
                                                _pallet_uom = palletUOM.FirstOrDefault().uom;
                                            }
                                            else if(palletUOM.Count > 1 && string.IsNullOrEmpty(_pallet_uom))
                                            {
                                                string errItemCode = "ข้อมูล Pallet uom มีมากกว่า 1 กรุณาระบุข้อมูลที่ต้องการ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            var palletUOMBase = _model.t_wms_item_uom.Where(w => w.item_master_id == item.item_master_id && w.is_pallet_uom == "YES" && w.uom == _pallet_uom).FirstOrDefault();
                                            if (palletUOMBase == null)
                                            {
                                                string errItemCode = "ไม่พบข้อมูล Pallet uom Base ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            if (warehouse == null)
                                            {
                                                string errItemCode = "ไม่พบข้อมูล Warehouse ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
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
                                            

                                            if (supplier == null && !string.IsNullOrEmpty(_supplier))
                                            {
                                                string errItemCode = "ไม่พบข้อมูล Supplier ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            if (customer == null && !string.IsNullOrEmpty(_customer))
                                            {
                                                string errItemCode = "ไม่พบข้อมูล Customer ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            if (string.IsNullOrEmpty(_inbound_oreder_number))
                                            {
                                                string errItemCode = "กรุณาใส่ข้อมูล Inbound Order Number"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
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
                                            if (string.IsNullOrEmpty(_production_line))
                                            {
                                                string errItemCode = "กรุณาใส่ข้อมูล Production Line"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }

                                            if(_make_to.ToUpper() != "STOCK" && _make_to.ToUpper() != "ORDER")
                                            {
                                                string errItemCode = "กรุณาใส่ข้อมูล Make To ให้ถูกต้อง"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            if (_make_to.ToUpper() == "ORDER" && string.IsNullOrEmpty(_ref_inbound_order_group))
                                            {
                                                string errItemCode = $"หากทำการเลือก Make To '{_make_to.ToUpper()}' กรุณาใส่ข้อมูล Ref. Inbound Order Number"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                            if (string.IsNullOrEmpty(_mfg_date))
                                            {
                                                string errItemCode = "กรุณาใส่ข้อมูล MFG Date"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;

                                            }
                                            var master = _model.t_wms_inbound_master.FirstOrDefault(w => w.inbound_order_number == _inbound_oreder_number);
                                            if (master != null)
                                            {
                                                var print_label = _model.t_wms_print_label.Where(w => w.inbound_order_master_id == master.inbound_order_master_id);
                                                if (print_label.Where(w => w.is_print == "YES").Count() > 0)
                                                {
                                                    string errItemCode = "ไม่สามารถอัพโหลดข้อมูลได้ เนื่องจากมีบางรายการในเอกสารนี้ มีการพิมพ์เรียบร้อยแล้ว"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                    _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                    continue;
                                                }
                                                if (print_label.Where(w => w.is_cancelled == "NO" && w.is_interface_hana == "YES").Count() > 0)
                                                {
                                                    string errItemCode = "ไม่สามารถอัพโหลดข้อมูลได้ เนื่องจากมีบางรายการในเอกสารนี้ มีการส่ง GR ไปยัง SAP แล้ว"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                    _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                    continue;
                                                }
                                                else
                                                {
                                                    //28022024 พี่นัทให้แก้เป็นลบข้อมูลทั้งหมดแล้ว Insert ใหม่ ในเคส OPEN ถ้าไม่ใช่ ให้ไปแก้ในโปรแกรม
                                                    inExcel = _listMaster.Where(w => w.inbound_order_number == _inbound_oreder_number && w.order_type == _order_type).ToList();
                                                    if (inExcel.Count() == 0)
                                                    {
                                                        if (master != null && master.order_status == "OPEN")
                                                        {
                                                            _model.t_wms_inbound_detail.RemoveRange(_model.t_wms_inbound_detail.Where(w => w.inbound_order_master_id == master.inbound_order_master_id));
                                                            _model.t_wms_print_label.RemoveRange(_model.t_wms_print_label.Where(w => w.inbound_order_master_id == master.inbound_order_master_id));
                                                            _model.t_wms_receipt_header.RemoveRange(_model.t_wms_receipt_header.Where(w => w.inbound_order_master_id == master.inbound_order_master_id));
                                                            _model.t_wms_inbound_master.Remove(master);
                                                            _model.SaveChanges();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (inExcel.FirstOrDefault(w => w.inbound_order_number == _inbound_oreder_number).ref_inbound_order_number != _ref_inbound_order_group)
                                                        {
                                                            string errItemCode = "Inbound Order Number เดียวกัน ต้องมี Ref. Inbound Order Number เหมือนกัน"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                            _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                            continue;
                                                        }
                                                    }
                                                }
                                            }

                                            var inbound_master = _model.t_wms_inbound_master.Where(w => w.inbound_order_number == _inbound_oreder_number).FirstOrDefault();
                                            if (inbound_master == null) // Insert
                                            {

                                                Source.t_wms_inbound_master _newInboundMaster = new Source.t_wms_inbound_master();
                                                _newInboundMaster.inbound_order_master_id = Guid.NewGuid();
                                                _newInboundMaster.inbound_order_number = _inbound_oreder_number;
                                                _newInboundMaster.order_type = ordertype.display_member;
                                                _newInboundMaster.make_to = _make_to.Substring(0, 1).ToUpper() + _make_to.Substring(1);
                                                _newInboundMaster.ref_inbound_order_number = (_make_to.ToUpper() == "STOCK" ? "MS" : _ref_inbound_order_group);
                                                _newInboundMaster.order_status = "OPEN";
                                                _newInboundMaster.production_line = _production_line;
                                                _newInboundMaster.ref_outbound_order_number = _ref_outbound_order_number;
                                                if (supplier != null)
                                                    _newInboundMaster.supplier_id = supplier.supplier_id;
                                                if (customer != null)
                                                    _newInboundMaster.customer_id = customer.customer_id;

                                                _newInboundMaster.create_date = string.IsNullOrEmpty(_create_date) ? DateTime.Now : Convert.ToDateTime(_create_date);

                                                _newInboundMaster.wh_id = warehouse.wh_id;
                                                _newInboundMaster.wh_master_id = warehouse.wh_master_id;

                                                _newInboundMaster.owner_code = owner.owner_code;
                                                _newInboundMaster.owner_id = owner.owner_id;

                                                _newInboundMaster.create_by = _create_by;
                                                _model.t_wms_inbound_master.Add(_newInboundMaster);
                                                _listMaster.Add(_newInboundMaster);
                                                _model.SaveChanges();

                                                // INSERT Detail
                                                Source.t_wms_inbound_detail _newInboundDetail = new Source.t_wms_inbound_detail();
                                                _newInboundDetail.inbound_order_detail_id = Guid.NewGuid();
                                                _newInboundDetail.inbound_order_master_id = _newInboundMaster.inbound_order_master_id;
                                                _newInboundDetail.wh_item_master_id = whItem.wh_item_master_id;
                                                _newInboundDetail.item_master_id = itemowner.item_master_id;
                                                _newInboundDetail.item_uom_id = uomBase.item_uom_id;
                                                _newInboundDetail.item_number = itemowner.item_number;
                                                _newInboundDetail.line_number = _model.t_wms_inbound_detail.Where(w => w.inbound_order_master_id == _newInboundMaster.inbound_order_master_id).Count() == 0 ? "000001" : (_model.t_wms_inbound_detail.Where(w => w.inbound_order_master_id == _newInboundMaster.inbound_order_master_id).Count() + 1).ToString("000000");
                                                _newInboundDetail.pack_size_uom_id = uom.item_uom_id;
                                                _newInboundDetail.pack_size_uom = _uom;
                                                _newInboundDetail.pack_size_conversion_factor = _model.t_wms_item_uom.FirstOrDefault(x => x.item_master_id == itemowner.item_master_id && x.uom == _uom).conversion_factor;
                                                _newInboundDetail.pallet_size_uom_id = palletUOMBase.item_uom_id;
                                                _newInboundDetail.pallet_size_uom = _pallet_uom;
                                                _newInboundDetail.pallet_size_conversion_factor = _model.t_wms_item_uom.FirstOrDefault(x => x.item_master_id == itemowner.item_master_id && x.uom == _pallet_uom).conversion_factor;
                                                _newInboundDetail.production_line = _production_line;
                                                _newInboundDetail.ref_outbound_order_number = _ref_outbound_order_number;
                                                _newInboundDetail.ref_item_number = _ref_item_number;
                                                if (uom.primary_uom == "NO")
                                                {
                                                    _newInboundDetail.quantity_order = Convert.ToDouble(_quantity_order) * uom.conversion_factor;
                                                }
                                                else
                                                {
                                                    _newInboundDetail.quantity_order = Convert.ToDouble(_quantity_order);
                                                }

                                                _newInboundDetail.uom = uomBase.uom;
                                                _newInboundDetail.item_status = "NEW";
                                                //_newInboundDetail.default_item_status = "Available";
                                                _newInboundDetail.default_item_status = "Quarantine";
                                                _newInboundDetail.over_receipt_allowed = "NO";

                                                //if (itemowner.lot_control == "FULL")
                                                _newInboundDetail.lot_number = Convert.ToDateTime(_mfg_date).ToString("dd-MM-yyyy");
                                                _newInboundDetail.mfg_date = Convert.ToDateTime(_mfg_date);
                                                if (item.days_to_expire == null)
                                                {
                                                    if (string.IsNullOrEmpty(_month_to_exp))
                                                    {
                                                        string errItemCode = "Item Number ดังกล่าวไม่ได้ระบุวันที่ Day to expire กรุณาระบุ Month To Expire"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                        _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                        continue;
                                                    }
                                                }
                                                var days_to_expire = 0;
                                                if (item.days_to_expire == null)
                                                {
                                                    if (string.IsNullOrEmpty(_month_to_exp) || _month_to_exp == "0")
                                                    {
                                                        string errItemCode = "Item Number ดังกล่าวไม่ได้ระบุวันที่ Day to expire กรุณาระบุ Month To Expire"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                        _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        days_to_expire = Convert.ToInt32(_month_to_exp);
                                                    }
                                                }
                                                else
                                                {
                                                    days_to_expire = item.days_to_expire ?? 0;
                                                }

                                                // if (itemowner.expiry_date_control == "FULL")
                                                //if (!string.IsNullOrEmpty(_exp_date))
                                                //{

                                                //}
                                                _newInboundDetail.expiry_date = Convert.ToDateTime(_mfg_date).AddMonths(days_to_expire).ToString("yyyyMMdd");
                                                //if (itemowner.attribute1_control == "FULL")
                                                _newInboundDetail.attribute1 = (_make_to.ToUpper() == "STOCK" ? "MS-" + _uom : _ref_inbound_order_group);
                                                //if (itemowner.attribute2_control == "FULL")
                                                _newInboundDetail.attribute2 = _attri2;
                                                //if (itemowner.attribute3_control == "FULL")
                                                _newInboundDetail.attribute3 = _attri3;
                                                //if (itemowner.attribute4_control == "FULL")
                                                _newInboundDetail.attribute4 = _attri4;
                                                //if (itemowner.attribute5_control == "FULL")
                                                _newInboundDetail.attribute5 = _attri5;

                                                _newInboundDetail.create_by = _create_by;
                                                _newInboundDetail.create_date = string.IsNullOrEmpty(_create_date) ? DateTime.Now : Convert.ToDateTime(_create_date);
                                                _listInbound.Add(_newInboundDetail);
                                                _model.t_wms_inbound_detail.Add(_newInboundDetail);
                                                _model.SaveChanges();

                                                int quantity = (int)(_newInboundDetail.quantity_order);
                                                int palletSize = (int)(_newInboundDetail.pallet_size_conversion_factor ?? 1);

                                                int insertCount = (int)Math.Ceiling((double)quantity / palletSize);

                                                for (int i = 0; i < insertCount; i++)
                                                {
                                                    int amountToInsert = Math.Min(palletSize, quantity - i * palletSize);
                                                    var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                                                    var _lpn = string.Empty;
                                                    var generate = _model.usp_wms_generate_lpn(_newInboundMaster.inbound_order_number, _newInboundDetail.production_line, errMsg);
                                                    if (generate != null)
                                                    {
                                                        _lpn = generate.FirstOrDefault();
                                                    }

                                                    var _pallet_seq = 0;
                                                    var generate_pallet_seq = _model.usp_wms_generate_pallet_seq(_newInboundMaster.wh_master_id, _newInboundDetail.production_line, _newInboundDetail.mfg_date, _newInboundDetail.item_number, errMsg);
                                                    if (generate_pallet_seq != null)
                                                    {
                                                        _pallet_seq = generate_pallet_seq.FirstOrDefault() ?? 0;
                                                    }

                                                    t_wms_print_label entPrint = new t_wms_print_label();
                                                    entPrint.print_label_id = Guid.NewGuid();
                                                    entPrint.wh_id = _newInboundMaster.wh_id;
                                                    entPrint.inbound_order_master_id = _newInboundDetail.inbound_order_master_id;
                                                    entPrint.inbound_order_number = _newInboundMaster.inbound_order_number;
                                                    entPrint.inbound_order_detail_id = _newInboundDetail.inbound_order_detail_id;
                                                    entPrint.line_number = _newInboundDetail.line_number;
                                                    entPrint.row_print = i + 1;
                                                    entPrint.lpn = _lpn;
                                                    entPrint.wh_item_master_id = _newInboundDetail.wh_item_master_id;
                                                    entPrint.item_number = _newInboundDetail.item_number;
                                                    entPrint.item_description = item.description;
                                                    entPrint.quantity_order = _newInboundDetail.quantity_order;
                                                    entPrint.item_uom_id = _newInboundDetail.item_uom_id;
                                                    entPrint.uom = _newInboundDetail.uom;
                                                    entPrint.lot_number = _newInboundDetail.lot_number;
                                                    entPrint.mfg_date = _newInboundDetail.mfg_date;

                                                    DateTime result;
                                                    try
                                                    {
                                                        result = DateTime.ParseExact(_newInboundDetail.expiry_date, "yyyyMMdd", null);
                                                        entPrint.expiry_date = result;
                                                    }
                                                    catch (FormatException)
                                                    {
                                                        Page.MessageWarning("expiry_date Invalid date format");
                                                    }

                                                    entPrint.attribute1 = _newInboundDetail.attribute1;
                                                    entPrint.production_line = _newInboundDetail.production_line;
                                                    entPrint.pack_size_per_pallet = (amountToInsert / _newInboundDetail.pack_size_conversion_factor);
                                                    entPrint.pack_size_uom = _newInboundDetail.pack_size_uom;
                                                    entPrint.pack_size_conversion_factor = _newInboundDetail.pack_size_conversion_factor;
                                                    entPrint.pallet_size_uom = _newInboundDetail.pallet_size_uom;
                                                    entPrint.pallet_size_conversion_factor = _newInboundDetail.pallet_size_conversion_factor;
                                                    entPrint.is_received = "NO";
                                                    entPrint.is_print = "NO";
                                                    entPrint.pallet_seq = _pallet_seq;
                                                    entPrint.create_by = _newInboundDetail.create_by;
                                                    entPrint.create_date = _newInboundDetail.create_date;
                                                    entPrint.is_cancelled = "NO";
                                                    _listPrintLabel.Add(entPrint);
                                                }
                                            }
                                            else
                                            {
                                                if (inbound_master.order_status != "OPEN")
                                                {
                                                    string errItemCode = "ไม่สามารถอัพเดทข้อมูล Inbound ที่ไม่ใช่สถานะ OPEN ได้ เนื่องจากมีการใช้งานหมายเลข Inbound Order Number นี้แล้ว"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                    _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                    continue;
                                                }

                                                if (inbound_master.make_to.ToUpper() != _make_to.ToUpper())
                                                {
                                                    string errItemCode = "ภายใต้ Order Number เดียวกันไม่สามารถเพิ่มข้อมูลที่มี Make To ต่างกันได้"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                    _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                    continue;
                                                }
                                                if (inExcel.Count() > 0)
                                                {
                                                    if (inbound_master.customer_id != null)
                                                    {
                                                        if (!_listMaster.Any(a => a.customer_id == customer.customer_id))
                                                        {
                                                            string errItemCode = "ภายใต้ Order Number เดียวกันไม่สามารถเพิ่มข้อมูลที่มี Customer ต่างกันได้"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                            _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                            continue;
                                                        }
                                                    }

                                                }
                                                inbound_master.order_type = ordertype.display_member;
                                                if (supplier != null)
                                                    inbound_master.supplier_id = supplier.supplier_id;
                                                if (customer != null)
                                                    inbound_master.customer_id = customer.customer_id;
                                                inbound_master.create_date = string.IsNullOrEmpty(_create_date) ? DateTime.Now : Convert.ToDateTime(_create_date);

                                                inbound_master.update_by = _create_by;
                                                inbound_master.update_date = DateTime.Now;
                                                _model.SaveChanges();

                                                //19072023 พี่นัทให้แก้เป็นลบข้อมูลทั้งหมดแล้ว Insert ใหม่
                                                //_model.t_wms_inbound_detail.RemoveRange(_model.t_wms_inbound_detail.Where(w => w.inbound_order_master_id == inbound_master.inbound_order_master_id));
                                                //_model.t_wms_print_label.RemoveRange(_model.t_wms_print_label.Where(w => w.inbound_order_master_id == inbound_master.inbound_order_master_id));
                                                //_model.SaveChanges();

                                                // INSERT Detail
                                                Source.t_wms_inbound_detail _newInboundDetail = new Source.t_wms_inbound_detail();
                                                _newInboundDetail.inbound_order_detail_id = Guid.NewGuid();
                                                _newInboundDetail.inbound_order_master_id = inbound_master.inbound_order_master_id;
                                                _newInboundDetail.wh_item_master_id = whItem.wh_item_master_id;
                                                _newInboundDetail.item_master_id = itemowner.item_master_id;
                                                _newInboundDetail.item_uom_id = uomBase.item_uom_id;
                                                _newInboundDetail.item_number = itemowner.item_number;
                                                _newInboundDetail.line_number = _listInbound.Count() == 0 ? "000001" : (_listInbound.Count() + 1).ToString("000000");
                                                _newInboundDetail.pack_size_uom_id = uom.item_uom_id;
                                                _newInboundDetail.pack_size_uom = _uom;
                                                _newInboundDetail.pack_size_conversion_factor = _model.t_wms_item_uom.FirstOrDefault(x => x.item_master_id == itemowner.item_master_id && x.uom == _uom).conversion_factor;
                                                _newInboundDetail.pallet_size_uom_id = palletUOMBase.item_uom_id;
                                                _newInboundDetail.pallet_size_uom = _pallet_uom;
                                                _newInboundDetail.pallet_size_conversion_factor = _model.t_wms_item_uom.FirstOrDefault(x => x.item_master_id == itemowner.item_master_id && x.uom == _pallet_uom).conversion_factor;
                                                _newInboundDetail.production_line = _production_line;
                                                _newInboundDetail.ref_outbound_order_number = _ref_outbound_order_number;
                                                _newInboundDetail.ref_item_number = _ref_item_number;

                                                if (uom.primary_uom == "NO")
                                                    _newInboundDetail.quantity_order = Convert.ToDouble(_quantity_order) * uom.conversion_factor;
                                                else
                                                    _newInboundDetail.quantity_order = Convert.ToDouble(_quantity_order);

                                                _newInboundDetail.uom = uomBase.uom;
                                                _newInboundDetail.item_status = "NEW";
                                                //_newInboundDetail.default_item_status = "Available";
                                                _newInboundDetail.default_item_status = "Quarantine";
                                                _newInboundDetail.over_receipt_allowed = "NO";

                                                //if (itemowner.lot_control == "FULL")
                                                _newInboundDetail.lot_number = Convert.ToDateTime(_mfg_date).ToString("dd-MM-yyyy");
                                                _newInboundDetail.mfg_date = Convert.ToDateTime(_mfg_date);
                                                var days_to_expire = 0;
                                                if (item.days_to_expire == null)
                                                {
                                                    if (string.IsNullOrEmpty(_month_to_exp) || _month_to_exp == "0")
                                                    {
                                                        string errItemCode = "Item Number ดังกล่าวไม่ได้ระบุวันที่ Day to expire กรุณาระบุ Month To Expire"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                        _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        days_to_expire = Convert.ToInt32(_month_to_exp);
                                                    }
                                                }
                                                else
                                                {
                                                    days_to_expire = item.days_to_expire ?? 0;
                                                }

                                                //if (!string.IsNullOrEmpty(_exp_date))
                                                _newInboundDetail.expiry_date = Convert.ToDateTime(_mfg_date).AddMonths(days_to_expire).ToString("yyyyMMdd");

                                                //if (itemowner.attribute1_control == "FULL")
                                                _newInboundDetail.attribute1 = (_make_to.ToUpper() == "STOCK" ? "MS-" + _uom : _ref_inbound_order_group);
                                                //if (itemowner.attribute2_control == "FULL")
                                                _newInboundDetail.attribute2 = _attri2;
                                                //if (itemowner.attribute3_control == "FULL")
                                                _newInboundDetail.attribute3 = _attri3;
                                                //if (itemowner.attribute4_control == "FULL")
                                                _newInboundDetail.attribute4 = _attri4;
                                                //if (itemowner.attribute5_control == "FULL")
                                                _newInboundDetail.attribute5 = _attri5;

                                                _newInboundDetail.create_by = _create_by;
                                                _newInboundDetail.create_date = string.IsNullOrEmpty(_create_date) ? DateTime.Now : Convert.ToDateTime(_create_date);
                                                _listInbound.Add(_newInboundDetail);
                                                _model.t_wms_inbound_detail.Add(_newInboundDetail);
                                                _model.SaveChanges();

                                                int quantity = (int)(_newInboundDetail.quantity_order);
                                                int palletSize = (int)(_newInboundDetail.pallet_size_conversion_factor ?? 1);

                                                int insertCount = (int)Math.Ceiling((double)quantity / palletSize);

                                                for (int i = 0; i < insertCount; i++)
                                                {
                                                    int amountToInsert = Math.Min(palletSize, quantity - i * palletSize);
                                                    var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                                                    var _lpn = string.Empty;
                                                    var generate = _model.usp_wms_generate_lpn(inbound_master.inbound_order_number, _newInboundDetail.production_line, errMsg);
                                                    if (generate != null)
                                                    {
                                                        _lpn = generate.FirstOrDefault();
                                                    }

                                                    var _pallet_seq = 0;
                                                    var generate_pallet_seq = _model.usp_wms_generate_pallet_seq(inbound_master.wh_master_id, _newInboundDetail.production_line, _newInboundDetail.mfg_date, _newInboundDetail.item_number, errMsg);
                                                    if (generate_pallet_seq != null)
                                                    {
                                                        _pallet_seq = generate_pallet_seq.FirstOrDefault() ?? 0;
                                                    }
                                                    t_wms_print_label entPrint = new t_wms_print_label();
                                                    entPrint.print_label_id = Guid.NewGuid();
                                                    entPrint.wh_id = inbound_master.wh_id;
                                                    entPrint.inbound_order_master_id = _newInboundDetail.inbound_order_master_id;
                                                    entPrint.inbound_order_number = inbound_master.inbound_order_number;
                                                    entPrint.inbound_order_detail_id = _newInboundDetail.inbound_order_detail_id;
                                                    entPrint.line_number = _newInboundDetail.line_number;
                                                    entPrint.row_print = i + 1;
                                                    entPrint.lpn = _lpn;
                                                    entPrint.wh_item_master_id = _newInboundDetail.wh_item_master_id;
                                                    entPrint.item_number = _newInboundDetail.item_number;
                                                    entPrint.item_description = item.description;
                                                    entPrint.quantity_order = _newInboundDetail.quantity_order;
                                                    entPrint.item_uom_id = _newInboundDetail.item_uom_id;
                                                    entPrint.uom = _newInboundDetail.uom;
                                                    entPrint.lot_number = _newInboundDetail.lot_number;
                                                    entPrint.mfg_date = _newInboundDetail.mfg_date;

                                                    DateTime result;
                                                    try
                                                    {
                                                        result = DateTime.ParseExact(_newInboundDetail.expiry_date, "yyyyMMdd", null);
                                                        entPrint.expiry_date = result;
                                                    }
                                                    catch (FormatException)
                                                    {
                                                        Page.MessageWarning("expiry_date Invalid date format");
                                                    }

                                                    entPrint.attribute1 = _newInboundDetail.attribute1;
                                                    entPrint.production_line = _newInboundDetail.production_line;
                                                    entPrint.pack_size_per_pallet = (amountToInsert / _newInboundDetail.pack_size_conversion_factor);
                                                    entPrint.pack_size_uom = _newInboundDetail.pack_size_uom;
                                                    entPrint.pack_size_conversion_factor = _newInboundDetail.pack_size_conversion_factor;
                                                    entPrint.pallet_size_uom = _newInboundDetail.pallet_size_uom;
                                                    entPrint.pallet_size_conversion_factor = _newInboundDetail.pallet_size_conversion_factor;
                                                    entPrint.is_received = "NO";
                                                    entPrint.is_print = "NO";
                                                    entPrint.pallet_seq = _pallet_seq;
                                                    entPrint.create_by = _newInboundDetail.create_by;
                                                    entPrint.create_date = _newInboundDetail.create_date;
                                                    entPrint.is_cancelled = "NO";
                                                    _listPrintLabel.Add(entPrint);
                                                }
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
                                    ExportExcel_EPPlus _excel = new ExportExcel_EPPlus();
                                    _excel.ToExcel(_dvError.ToTable(), "InboundError.xlsx", columnNames);
                                }

                                try
                                {
                                    _model.t_wms_print_label.AddRange(_listPrintLabel);
                                    _model.SaveChanges();
                                }
                                catch (DbEntityValidationException ex)
                                {
                                    foreach (var error in ex.EntityValidationErrors.SelectMany(ev => ev.ValidationErrors))
                                    {
                                        Console.WriteLine($"Property: {error.PropertyName} Error: {error.ErrorMessage}");
                                    }
                                }
                                

                                dbContextTransaction.Commit();
                            }

                        }
                        GridExt1.DataBind();
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

        private void ddlOrderType_PostValueChanged(dynamic _value)
        {
            try
            {
                string value = _value != null ? _value.ToString() : string.Empty;
                using (var _model = new Source.WMSEntities())
                {
                    string[] rules = _model.t_wms_rule.Where(x => x.rule_code == "RETURN_MODERN_TRADE" && x.is_active == "YES").Select(se => se.value).ToArray();
                    
                    if (Array.Exists(rules, element => element == _value))
                    {
                        ddlCustomer.IsPrimary = true;
                    }
                    else
                    {
                        ddlCustomer.IsPrimary = false;
                    }
                    //else
                    //{
                    //    var make_to = ddlMaketo.GetValue();
                    //    if(make_to != null)
                    //    {
                    //        if (make_to == "Order")
                    //        {
                    //            ddlCustomer.IsPrimary = true;
                    //        }
                    //        else
                    //        {
                    //            ddlCustomer.IsPrimary = false;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        ddlCustomer.IsPrimary = false;
                    //    }
                    //}
                    ddlCustomer.Update();

                    string[] rules_def9 = _model.t_wms_rule
                        .Where(x => x.rule_code == "TYPE_REQUIRE_RECHD_UDF9" && x.is_active == "YES")
                        .Select(se => se.value)
                        .ToArray();

                    string[] rules_def9_validate_month = _model.t_wms_rule
                        .Where(x => x.rule_code == "TYPE_REQUIRE_RECHD_UDF9_VALIDATE_MONTH" && x.is_active == "YES")
                        .Select(se => se.value)
                        .ToArray();

                    bool isPrimaryUdf9 = Array.Exists(rules_def9, element => element == _value);
                    bool isPrimaryUdf9ValidateMonth = Array.Exists(rules_def9_validate_month, element => element == _value);
                    
                    if (isPrimaryUdf9) { ucReceivePartial.Is_Primary_Udf_9(isPrimaryUdf9, false); }
                    else if (isPrimaryUdf9ValidateMonth) { ucReceivePartial.Is_Primary_Udf_9(isPrimaryUdf9ValidateMonth, true); }
                    else { ucReceivePartial.Is_Primary_Udf_9(false, false); }


                    if (_model.t_wms_rule.Any(a => a.rule_code == "ORDER_TYPE_REF_OUTBOUND" && a.is_active == "YES" && a.value == value))
                    {
                        ddlRefLPN.IsPrimary = true;
                        ddlRefLPN.Enabled = true;
                        ddlProductionLine.IsPrimary = (popupEx.FormState == FormState.New);
                        BindDdlReference();
                        
                        if (string.IsNullOrEmpty(ddlMaketo.GetValue()))
                        {
                            ddlMaketo.SetValue("Stock");
                            ddlMaketo.Enabled = false;
                            ddlMaketo_PostValueChanged("Stock");
                        }
                    }
                    else
                    {
                        ddlRefLPN.Clear();
                        ddlRefLPN.IsPrimary = false;
                        ddlRefLPN.Enabled = false;
                        ddlProductionLine.IsPrimary = false;
                    }
                    popupEx.UpdateContent();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private void ddlWarehouse_PostValueChanged(dynamic _value)
        {
            if(_value != null)
            {
                this.wh_master_id = _value;
            }
        }
        private void iColWarehouse_DropDownPostValueChanged(dynamic _value)
        {
            if (_value != null)
            {
                this.col_wh_master_id = _value;
            }
        }
        private void GridExt1_GridExportTemplate()
        {
            try
            {
                #region template old
                List<string> HeaderCol = new List<string>();
                //HeaderCol.Add("Warehouse#");
                //HeaderCol.Add("Owner#");
                HeaderCol.Add("Inbound Order Number#");
                HeaderCol.Add("Order Type#");
                HeaderCol.Add("Supplier Code");
                HeaderCol.Add("Customer Code");
                HeaderCol.Add("Make To#");
                HeaderCol.Add("Reference Inbound Order Group#");
                HeaderCol.Add("Order Date");
                HeaderCol.Add("Item Number#");
                //HeaderCol.Add("Item Description");
                HeaderCol.Add("MFG Date#");
                HeaderCol.Add("Month To Expire");
                HeaderCol.Add("Order Qty#");
                HeaderCol.Add("UOM#");
                HeaderCol.Add("Pallet UOM#");
                HeaderCol.Add("Production Line#");
                HeaderCol.Add("Ref Outbound Order Number");
                HeaderCol.Add("Ref Item Number");
                HeaderCol.Add("Create By#");
                //HeaderCol.Add("Attribute 1");
                HeaderCol.Add("Attribute 2");
                HeaderCol.Add("Attribute 3");
                HeaderCol.Add("Attribute 4");
                HeaderCol.Add("Attribute 5");
                

                using (var _model = new Source.WMSEntities())
                {
                    ExportExcel_EPPlus _excel = new ExportExcel_EPPlus();
                    // Insert Empty 
                    var List = new List<Dto_Inbound_Template>();
                    var dtoInboundTemplate = new Dto_Inbound_Template();
                    List.Add(dtoInboundTemplate);
                    _excel.ToExcel(List.ToDataTable(), "InboundTemplate.xlsx", HeaderCol.ToArray());
                    
                    //using (ExcelPackage testFile = new ExcelPackage())
                    //{
                    //    ExcelWorksheet testSht = testFile.Workbook.Worksheets[1];
                    //    using(MemoryStream memoryStream = new MemoryStream())
                    //    {
                    //        testFile.SaveAs(memoryStream);
                    //        memoryStream.WriteTo(Response.OutputStream);
                    //    }
                    //}
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


    public class Dto_Inbound_Template
    {
        //public string Warehouse { get; set; }
        //public string Owner { get; set; }
        public string InboundOrderNumber { get; set; }
        public string OrderType { get; set; }
        public string SupplierCode { get; set; }
        public string CustomerCode { get; set; }
        public string MakeTo { get; set; }
        public string ReferenceInboundOrderGroup { get; set; }
        public DateTime OrderDate { get; set; }
        public string ItemNumber { get; set; }
        //public string ItemDescription { get; set; }
        public DateTime MFGDate { get; set; }
        public int MonthToExpire { get; set; }
        public int OrderQty { get; set; }
        public string UOM { get; set; }
        public string PalletUOM { get; set; }
        public string ProductionLine { get; set; }
        public string RefOutboundOrderNumber { get; set; }
        public string RefItemNumber { get; set; }
        public string CreateBy { get; set; }
    }
}