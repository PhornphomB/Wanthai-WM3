using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace WMS_NEW.MasterData
{
    public partial class Item : PageCustom
    {
        private Guid category_id
        {
            get
            {
                if (ViewState["category_id"] == null)
                    ViewState["category_id"] = Guid.Empty;

                return (Guid)ViewState["category_id"];
            }
            set
            {
                ViewState["category_id"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                popupItem.AfterNewDataEvent += PopupItem_AfterNewDataEvent;
                popupItem.AfterSetEditDataEvent += PopupItem_AfterSetEditDataEvent;

                #region Binging DropDown Property Column Grid

                iColOwner.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                GridColumnExt6.DropDownQueryProperty = delegate () { return new global::ConfigGlobal.DTO._Global.ActiveType().AsQueryable(); };

                iColLot.DropDownQueryProperty = delegate ()
                {
                    var list = new List<Prototype.Providers.Property>();
                    list.Add(new Prototype.Providers.Property() { value_member = "Full", display_member = "Full" });
                    list.Add(new Prototype.Providers.Property() { value_member = "None", display_member = "None" });

                    return list.AsQueryable();
                };
                iColExp.DropDownQueryProperty = delegate ()
                {
                    var list = new List<Prototype.Providers.Property>();
                    list.Add(new Prototype.Providers.Property() { value_member = "Full", display_member = "Full" });
                    list.Add(new Prototype.Providers.Property() { value_member = "None", display_member = "None" });

                    return list.AsQueryable();
                };
                iColSerial.DropDownQueryProperty = delegate ()
                {
                    var list = new List<Prototype.Providers.Property>();
                    list.Add(new Prototype.Providers.Property() { value_member = "Full", display_member = "Full" });
                    list.Add(new Prototype.Providers.Property() { value_member = "None", display_member = "None" });

                    return list.AsQueryable();
                };

                #endregion


                #region Binging DropDown Property Input Data

                if (!Page.IsPostBack)
                {
                    Func<IQueryable<Prototype.Providers.Property>> FuncQueryAttr = () =>
                    {
                        return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("mst_item_attribute_ctrl"); ;
                    };
                    Func<IQueryable<Prototype.Providers.Property>> FuncQueryAttr1 = () =>
                    {
                        return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("mst_item_attribute1_ctrl"); ;
                    };

                    ddOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                    ddBatch.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("mst_item_lot_ctrl"); };
                    ddSerial.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("mst_item_sn_ctrl"); };
                    ddExpiryDate.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("mst_item_exp_ctrl"); };
                    ddKit.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("mst_item_kit_ctrl"); };

                    ddAttribute1.MethodQueryProperty = delegate () { return FuncQueryAttr1(); };
                    ddAttribute2.MethodQueryProperty = delegate () { return FuncQueryAttr(); };
                    ddAttribute3.MethodQueryProperty = delegate () { return FuncQueryAttr(); };
                    ddAttribute4.MethodQueryProperty = delegate () { return FuncQueryAttr(); };
                    ddAttribute5.MethodQueryProperty = delegate () { return FuncQueryAttr(); };

                    ddlRFID.MethodQueryProperty = delegate () { return new global::ConfigGlobal.DTO._Global.ActiveType().AsQueryable(); };
                    ddlPrecut.MethodQueryProperty = delegate () { return new global::ConfigGlobal.DTO._Global.ActiveType().AsQueryable(); };
                    ddlBarcode.MethodQueryProperty = delegate () { return new global::ConfigGlobal.DTO._Global.ActiveType().AsQueryable(); };
                    ddIsBom.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("mst_item_bom_ctrl"); };
                }

                ddCategory.MethodQueryProperty = delegate () { return Access.MasterData.ItemCategory.Instance.GetQuery(); };
                ddSubCategory.MethodQueryProperty = delegate () { return Access.MasterData.SubCategory.Instance.GetQueryProperty(this.category_id); };

                #endregion


                ddCategory.PostValueChanged = DdCategory_PostValueChanged;

                popupItem.InitObjectsEvent += () => { popupItem.ObjectDataAccess = new Access.MasterData.Item(); };
                popupItem.InitControlStatic();

                GridExt1.PopupEntitySource = popupItem;
                GridExt1.GridExportTemplate += GridExt1_GridExportTemplate;

                if (!Page.IsPostBack)
                {
                    ddSerial.DefaultValue = "Full";
                    ddExpiryDate.DefaultValue = "Full";
                    ddSerial.DefaultValue = "Full";
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
                HeaderCol.Add("Warehouse");
                HeaderCol.Add("Owner");
                HeaderCol.Add("Category");
                HeaderCol.Add("Sub Category");
                HeaderCol.Add("Item Number");
                HeaderCol.Add("Description");
                HeaderCol.Add("Lot Control");
                HeaderCol.Add("Exp Control");
                HeaderCol.Add("Inventory Type");
                HeaderCol.Add("Day To Expire");
                HeaderCol.Add("UOM");
                HeaderCol.Add("Attribute 1 Control");
                HeaderCol.Add("Attribute 2 Control");
                HeaderCol.Add("Attribute 3 Control");
                HeaderCol.Add("Attribute 4 Control");
                HeaderCol.Add("Attribute 5 Control");

                using (var _model = new Source.WMSEntities())
                {
                    ExportExcel_EPPlus _excel = new ExportExcel_EPPlus();
                    // Insert Empty 
                    var List = new List<dynamic>();

                    _excel.ToExcel(List.ToDataTable(), "ItemTemplate.xlsx", HeaderCol.ToArray());
                }
                #endregion

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void PopupItem_AfterNewDataEvent()
        {
            ddBatch.Enabled = true;
            ddExpiryDate.Enabled = true;
            ddSerial.Enabled = true;
            ddAttribute1.Enabled = true;
            ddAttribute2.Enabled = true;
            ddAttribute3.Enabled = true;
            ddAttribute4.Enabled = true;
            ddAttribute5.Enabled = true;

            PanelTab1.VisiblePanel(6, false);
        }

        private void PopupItem_AfterSetEditDataEvent()
        {
            try
            {
                var id = (Guid)popupItem.KeyFieldValue;
                var has_inven = false;
                var has_inbound = false;
                var has_outbound = false;
                var has_system = false;
                var wh_item_master_id = Guid.Empty;


                using (var _model = new Source.WMSEntities())
                {
                    has_inven = (from rows in _model.t_wms_inventory
                                 join whit in _model.t_wms_wh_item on rows.wh_item_master_id equals whit.wh_item_master_id
                                 where whit.item_master_id == id
                                 select rows).Any();

                    has_inbound = (from rows in _model.t_wms_inbound_detail
                                   join whit in _model.t_wms_wh_item on rows.wh_item_master_id equals whit.wh_item_master_id
                                   where whit.item_master_id == id && (rows.t_wms_inbound_master.order_status.ToUpper() == "OPEN" || rows.t_wms_inbound_master.order_status.ToUpper() == "RECEIVING") && !string.IsNullOrEmpty(rows.lot_number)
                                   select rows).Any();

                    has_outbound = (from rows in _model.t_wms_outbound_detail
                                    join whit in _model.t_wms_wh_item on rows.wh_item_master_id equals whit.wh_item_master_id
                                    where whit.item_master_id == id && rows.t_wms_outbound_master.order_status.ToUpper() == "OPEN" && !string.IsNullOrEmpty(rows.lot_number)
                                    select rows).Any();

                    // พี่หนุ่มให้แก้  06/07/2022
                    //จาก Control Lot = full ไปเป็น Control Lot = None
                    //1.ถ้ามีของใน Inventroy >> จะต้องแก้ไขไม่ได้
                    //2.ถ้ามีการระบุ lot ที่ inbound plan >> จะต้องแก้ไขไม่ได้(สนใจแค่ order ที่ status = open กับ receiving)
                    //3.ถ้ามีการระบุ lot ที่ outbound plan >> จะต้องแก้ไขไม่ได้(สนใจแค่ order ที่ status = open)
                    //จาก Control Lot = None ไปเป็น control lot = full
                    //1.ถ้ามีของใน Inventroy >> จะต้องแก้ไขไม่ได้
                    var LotControl = ddBatch.GetValue();

                    if (LotControl == "FULL")
                    {
                        if (has_inven || has_inbound || has_outbound)
                            has_system = true;
                    }
                    else // NONE
                    {
                        if (has_inven)
                            has_system = true;
                    }

                    wh_item_master_id = _model.t_wms_wh_item.Where(w => w.item_master_id == id).FirstOrDefault() != null ? _model.t_wms_wh_item.Where(w => w.item_master_id == id).FirstOrDefault().wh_item_master_id : Guid.Empty;
                }

                var invert = !has_system; //!has_inven;

                ddBatch.Enabled = invert;
                ddExpiryDate.Enabled = invert;
                ddSerial.Enabled = invert;
                ddAttribute1.Enabled = invert;
                ddAttribute2.Enabled = invert;
                ddAttribute3.Enabled = invert;
                ddAttribute4.Enabled = invert;
                ddAttribute5.Enabled = invert;


                PanelTab1.VisiblePanel(6, true);
                var item_number = txtItemNumber.GetValue();
                uploadFileEx1.BindingData(item_number);
                //ucItemPickface.InitForm(wh_item_master_id);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void DdCategory_PostValueChanged(dynamic _value)
        {
            try
            {
                this.category_id = _value ?? Guid.Empty;
                ddSubCategory.BindDataSource();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                pnlImportFile.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
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

                if (dt.Columns["Warehouse"].IsEmpty() || dt.Columns["Owner"].IsEmpty() || dt.Columns["Category"].IsEmpty() || dt.Columns["Sub Category"].IsEmpty() || dt.Columns["Item Number"].IsEmpty() || dt.Columns["Description"].IsEmpty() || dt.Columns["Lot Control"].IsEmpty() || dt.Columns["Exp Control"].IsEmpty() || dt.Columns["Inventory Type"].IsEmpty() || dt.Columns["Day To Expire"].IsEmpty() || dt.Columns["UOM"].IsEmpty() || dt.Columns["Attribute 1 Control"].IsEmpty() || dt.Columns["Attribute 2 Control"].IsEmpty() || dt.Columns["Attribute 3 Control"].IsEmpty() || dt.Columns["Attribute 4 Control"].IsEmpty() || dt.Columns["Attribute 5 Control"].IsEmpty())
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
                            foreach (DataRow dr in dt.Rows)
                            {
                                try
                                {
                                    if (!dr.Equals(null))
                                    {



                                        string _wh = dr[0].ToString().Trim();
                                        string _owner = dr[1].ToString().Trim();
                                        string _item_cate = dr[2].ToString().Trim();
                                        string _sub_cate = dr[3].ToString().Trim();
                                        string _item_number = dr[4].ToString().Trim();
                                        string _description = dr[5].ToString().Trim();
                                        string _lot_control = dr[6].ToString().Trim() == "FULL" ? "FULL" : "NONE";
                                        string _exp_control = dr[7].ToString().Trim() == "FULL" ? "FULL" : "NONE";
                                        string _inv_type = dr[8].ToString().Trim() == "YES" ? "YES" : "NO";
                                        string _expiry_date = dr[9].ToString().Trim();
                                        string _uom = dr[10].ToString().Trim();
                                        string _attribute1 = dr[11].ToString().Trim() == "FULL" ? "FULL" : "NONE";
                                        string _attribute2 = dr[12].ToString().Trim() == "FULL" ? "FULL" : "NONE";
                                        string _attribute3 = dr[13].ToString().Trim() == "FULL" ? "FULL" : "NONE";
                                        string _attribute4 = dr[14].ToString().Trim() == "FULL" ? "FULL" : "NONE";
                                        string _attribute5 = dr[15].ToString().Trim() == "FULL" ? "FULL" : "NONE";

                                        Guid _category_id = Guid.Empty;
                                        Guid _item_master_id = Guid.Empty;

                                        // Check Item cate
                                        var _category = _model.t_wms_category.Where(w => w.item_category == _item_cate).FirstOrDefault();
                                        if (_category == null)
                                        {
                                            Source.t_wms_category _newCate = new Source.t_wms_category();
                                            _newCate.category_id = Guid.NewGuid();
                                            _newCate.item_category = _item_cate;
                                            _newCate.description = _item_cate;
                                            _newCate.is_active = "YES";
                                            _newCate.create_by = _SessionVals.UserName;
                                            _newCate.create_date = DateTime.Now;
                                            _model.t_wms_category.Add(_newCate);
                                            _model.SaveChanges();

                                            _category_id = _newCate.category_id;
                                        }
                                        else
                                        {
                                            _category_id = _category.category_id;
                                        }

                                        var _sub_category = _model.t_wms_sub_category.Where(w => w.sub_category == _sub_cate).FirstOrDefault();

                                        // Check Warehouse 
                                        var _warehouse = _model.t_wms_wh.Where(w => w.wh_id == _wh).FirstOrDefault();
                                        if (!string.IsNullOrEmpty(_wh))
                                        {
                                            if (_warehouse == null)
                                            {
                                                string errItemCode = "ไม่พบข้อมูล Warehouse"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                continue;
                                            }
                                        }

                                        // Check Owner
                                        var _onwer = _model.t_wms_owner.Where(w => w.owner_code == _owner).FirstOrDefault();
                                        if (_onwer == null)
                                        {
                                            string errItemCode = "ไม่พบข้อมูล Onwer"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                            _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                            continue;
                                        }

                                        // Check Item CASE INESRT OR UPDATE
                                        var _item = _model.t_wms_item.Where(w => w.item_number == _item_number).FirstOrDefault();
                                        if (_item == null) // INSERT
                                        {
                                            Source.t_wms_item _newItem = new Source.t_wms_item();
                                            _newItem.item_master_id = Guid.NewGuid();
                                            _newItem.item_number = _item_number;
                                            _newItem.description = _description;
                                            _newItem.owner_id = _onwer.owner_id;
                                            _newItem.category_id = _category_id;

                                            if (_sub_category != null)
                                                _newItem.sub_category_id = _sub_category.sub_category_id;

                                            _newItem.lot_control = _lot_control;
                                            _newItem.expiry_date_control = _exp_control;
                                            _newItem.inventory_type = _inv_type;

                                            int _dayToExp = 0;
                                            if (int.TryParse(_expiry_date, out _dayToExp))
                                            {
                                                _newItem.days_to_expire = Convert.ToInt32(_expiry_date);
                                            } 

                                            _newItem.is_kit = "NO";
                                            _newItem.is_bom = "NO";
                                            _newItem.sn_control = "NONE";
                                            _newItem.attribute1_control = _attribute1;
                                            _newItem.attribute2_control = _attribute2;
                                            _newItem.attribute3_control = _attribute3;
                                            _newItem.attribute4_control = _attribute4;
                                            _newItem.attribute5_control = _attribute5;
                                            _newItem.is_active = "YES";
                                            _newItem.create_by = _SessionVals.UserName;
                                            _newItem.create_date = DateTime.Now;
                                            _model.t_wms_item.Add(_newItem);
                                            _model.SaveChanges();

                                            _item_master_id = _newItem.item_master_id;

                                        }
                                        else
                                        {
                                            _item.item_number = _item_number;
                                            _item.description = _description;
                                            _item.owner_id = _onwer.owner_id;
                                            _item.category_id = _category_id;

                                            if (_sub_category != null)
                                                _item.sub_category_id = _sub_category.sub_category_id;

                                            _item.lot_control = _lot_control;
                                            _item.expiry_date_control = _exp_control;
                                            _item.inventory_type = _inv_type;

                                            int _dayToExp = 0;
                                            if (int.TryParse(_expiry_date, out _dayToExp))
                                            {
                                                _item.days_to_expire = _dayToExp;
                                            }

                                            _item.attribute1_control = _attribute1;
                                            _item.attribute2_control = _attribute2;
                                            _item.attribute3_control = _attribute3;
                                            _item.attribute4_control = _attribute4;
                                            _item.attribute5_control = _attribute5;

                                            _item.update_by = _SessionVals.UserName;
                                            _item.update_date = DateTime.Now;

                                            _item_master_id = _item.item_master_id;

                                        }

                                        var _uomUpdate = _model.t_wms_item_uom.Where(w => w.uom == _uom && w.item_master_id == _item_master_id && w.primary_uom == "YES").FirstOrDefault();
                                        if (_uomUpdate != null)
                                        {
                                            _uomUpdate.uom = _uom;
                                            _uomUpdate.uom_prompt = _uom;
                                        }
                                        else
                                        {
                                            //Insert Uom 
                                            Source.t_wms_item_uom _newUom = new Source.t_wms_item_uom();
                                            _newUom.item_uom_id = Guid.NewGuid();
                                            _newUom.item_master_id = _item_master_id;
                                            _newUom.uom = _uom;
                                            _newUom.primary_uom = "YES";
                                            _newUom.conversion_factor = 1;
                                            _newUom.sequence = 1;
                                            _newUom.picking_uom = "YES";
                                            _newUom.shipping_uom = "YES";
                                            _newUom.uom_prompt = _uom;
                                            _newUom.picking_class = _model.t_wms_rule.Where(w => w.rule_code == "RULE_PICK_BY_FIFO" && w.type == "PICK_RULE").FirstOrDefault().rule_id.ToString();
                                            _newUom.putaway_class = _model.t_wms_rule.Where(w => w.rule_code == "RULE_PUTAWAY" && w.type == "PUTAWAY_RULE").FirstOrDefault().rule_id.ToString();
                                            _newUom.display_ti_hi = "NO";
                                            _newUom.length = 1;
                                            _newUom.width = 1;
                                            _newUom.height = 1;
                                            _newUom.weight = 1;
                                            _newUom.is_active = "YES";
                                            _newUom.create_by = _SessionVals.UserName;
                                            _newUom.create_date = DateTime.Now;

                                            _model.t_wms_item_uom.Add(_newUom);
                                            _model.SaveChanges();

                                        }

                                        //Map Item WH และเช็คว่าถ้าไม่มี WH ให้แมพทุก WH
                                        if (!string.IsNullOrEmpty(_wh))
                                        {
                                            if (!_model.t_wms_wh_item.Any(w => w.item_master_id == _item_master_id && w.wh_master_id == _warehouse.wh_master_id))
                                            {
                                                Source.t_wms_wh_item _whItem = new Source.t_wms_wh_item();
                                                _whItem.wh_item_master_id = Guid.NewGuid();
                                                _whItem.item_master_id = _item_master_id;
                                                _whItem.wh_master_id = _warehouse.wh_master_id;
                                                _whItem.interface_to_host = "YES";
                                                _whItem.is_active = "YES";
                                                _whItem.create_by = _SessionVals.UserName;
                                                _whItem.create_date = DateTime.Now;
                                                _model.t_wms_wh_item.Add(_whItem);
                                                _model.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            var ListWh = _model.t_wms_wh.ToList();
                                            foreach (var item in ListWh)
                                            {
                                                if (!_model.t_wms_wh_item.Any(w => w.item_master_id == _item_master_id && w.wh_master_id == item.wh_master_id))
                                                {
                                                    Source.t_wms_wh_item _whItem = new Source.t_wms_wh_item();
                                                    _whItem.wh_item_master_id = Guid.NewGuid();
                                                    _whItem.item_master_id = _item_master_id;
                                                    _whItem.wh_master_id = item.wh_master_id;
                                                    _whItem.interface_to_host = "YES";
                                                    _whItem.is_active = "YES";
                                                    _whItem.create_by = _SessionVals.UserName;
                                                    _whItem.create_date = DateTime.Now;
                                                    _model.t_wms_wh_item.Add(_whItem);
                                                    _model.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string errItemCode = "รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง กรุณาตรวจสอบข้อมูล"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                    _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                    continue;
                                }
                            }
                        }
                        _dvError.RowFilter = "Error <> ''";
                        if (_dvError.Count > 0)
                        {
                            ExportExcel_EPPlus _excel = new ExportExcel_EPPlus();
                            _excel.ToExcel(_dvError.ToTable(), "ItemError.xlsx", columnNames);
                        }
                        GridExt1.DataBind();
                        Page.MessageSuccess("Success");
                        pnlImportFile.HideDialog();
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