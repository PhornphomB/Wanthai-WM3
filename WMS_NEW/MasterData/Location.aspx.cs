using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WMS_NEW.MasterData
{
    public partial class Location : PageCustom
    {
        _UControls.InputCheckBox chkLPN;
        _UControls.InputCheckBox chkOwner;
        _UControls.InputCheckBox chkItem;
        _UControls.InputCheckBox chkLot;
        _UControls.InputCheckBox chkExp;
        _UControls.InputCheckBox chkAtt1;
        _UControls.InputCheckBox chkAtt2;
        _UControls.InputCheckBox chkAtt3;
        _UControls.InputCheckBox chkAtt4;
        _UControls.InputCheckBox chkAtt5;
        _UControls.InputCheckBox chkStatus;
        _UControls.InputCheckBox chkFullPallet;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iColWarehouse.DropDownQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };
                iColLocType.DropDownQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("mst_location_type"); };

                #region Init PopupEntity 

                var access = new Access.MasterData.Location();
                var entity = access.Entity;

                #region Init Controls Entity

                var tabs_attr = new _UControls.EntityTab[]{
                    new _UControls.EntityTab { TabIndex = 1, TabName = "Location Capcity",ResourceGroup="location",ResourceName="tab_capacity" },
                    new _UControls.EntityTab { TabIndex = 2, TabName = "Location Mix" ,ResourceGroup="location",ResourceName="tab_mix" },
                };

                #region Binding
                _UControls.InputDropDown Warehouse = new _UControls.InputDropDown() { DataFieldValue = nameof(entity.wh_master_id), ComboType = _UControls.ComboType.Guid, DisplayDefault = "--Select--", IsKey = true, IsPrimary = true };
                Warehouse.MethodQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };
                Warehouse.ResourceGroup = "warehouse";
                Warehouse.ResourceName = "wh_id";

                _UControls.InputDropDown LocType = new _UControls.InputDropDown() { DataFieldValue = nameof(entity.loc_type), ComboType = _UControls.ComboType.String, DisplayDefault = "--Select--", IsKey = true };
                LocType.MethodQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("mst_location_type"); };

                chkLPN = new _UControls.InputCheckBox() { CheckBoxType = _UControls.CheckBoxType.String, DefaultValue = "YES", DataFieldValue = nameof(entity.lpn_controlled) };
                chkOwner = new _UControls.InputCheckBox() { CheckBoxType = _UControls.CheckBoxType.String, DefaultValue = "NO", DataFieldValue = nameof(entity.mix_owners) };
                chkItem = new _UControls.InputCheckBox() { CheckBoxType = _UControls.CheckBoxType.String, DefaultValue = "YES", DataFieldValue = nameof(entity.mix_items) };
                chkLot = new _UControls.InputCheckBox() { CheckBoxType = _UControls.CheckBoxType.String, DefaultValue = "YES", DataFieldValue = nameof(entity.mix_lot) };
                chkExp = new _UControls.InputCheckBox() { CheckBoxType = _UControls.CheckBoxType.String, DefaultValue = "YES", DataFieldValue = nameof(entity.mix_expiry_dates) };
                chkAtt1 = new _UControls.InputCheckBox() { CheckBoxType = _UControls.CheckBoxType.String, DefaultValue = "YES", DataFieldValue = nameof(entity.mix_attribute1) };
                chkAtt2 = new _UControls.InputCheckBox() { CheckBoxType = _UControls.CheckBoxType.String, DefaultValue = "NO", DataFieldValue = nameof(entity.mix_attribute2) };
                chkAtt3 = new _UControls.InputCheckBox() { CheckBoxType = _UControls.CheckBoxType.String, DefaultValue = "NO", DataFieldValue = nameof(entity.mix_attribute3) };
                chkAtt4 = new _UControls.InputCheckBox() { CheckBoxType = _UControls.CheckBoxType.String, DefaultValue = "NO", DataFieldValue = nameof(entity.mix_attribute4) };
                chkAtt5 = new _UControls.InputCheckBox() { CheckBoxType = _UControls.CheckBoxType.String, DefaultValue = "NO", DataFieldValue = nameof(entity.mix_attribute5) };
                chkStatus = new _UControls.InputCheckBox() { CheckBoxType = _UControls.CheckBoxType.String, DefaultValue = "YES", DataFieldValue = nameof(entity.mix_status) };
                chkFullPallet = new _UControls.InputCheckBox() { CheckBoxType = _UControls.CheckBoxType.String, DefaultValue = "YES", DataFieldValue = nameof(entity.is_full_pallet) };

                #endregion

                var override_controls = new List<_UControls.EntityCustom>();

                override_controls.Add(new _UControls.EntityCustom(Warehouse) { RefGlobalVar = (REF) => { Warehouse = REF; } });
                override_controls.Add(new _UControls.EntityCustom(LocType) { RefGlobalVar = (REF) => { LocType = REF; } });

                override_controls.Add(new _UControls.EntityCustom() { DataFieldValue = nameof(entity.location), IsKey = true });
                override_controls.Add(new _UControls.EntityCustom() { DataFieldValue = nameof(entity.description) });
                override_controls.Add(new _UControls.EntityCustom() { DataFieldValue = nameof(entity.putaway_sequence) });
                override_controls.Add(new _UControls.EntityCustom() { DataFieldValue = nameof(entity.pick_sequence) });
                override_controls.Add(new _UControls.EntityCustom() { DataFieldValue = nameof(entity.erp_location) });
                override_controls.Add(new _UControls.EntityCustom(chkFullPallet) { RefGlobalVar = (REF) => { chkFullPallet = REF; } });


                override_controls.Add(new _UControls.EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.length) });
                override_controls.Add(new _UControls.EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.width) });
                override_controls.Add(new _UControls.EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.height) });
                override_controls.Add(new _UControls.EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.capacity_qty) });
                override_controls.Add(new _UControls.EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.gross_weight) });
                override_controls.Add(new _UControls.EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.x_cordinate) });
                override_controls.Add(new _UControls.EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.y_cordinate) });
                override_controls.Add(new _UControls.EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.location_level) });
                override_controls.Add(new _UControls.EntityCustom { TabIndex = 1, DataFieldValue = nameof(entity.color_code) });


                override_controls.Add(new _UControls.EntityCustom(chkLPN) { RefGlobalVar = (REF) => { chkLPN = REF; }, TabIndex = 2 });
                override_controls.Add(new _UControls.EntityCustom(chkOwner) { RefGlobalVar = (REF) => { chkOwner = REF; }, TabIndex = 2 });
                override_controls.Add(new _UControls.EntityCustom(chkItem) { RefGlobalVar = (REF) => { chkItem = REF; }, TabIndex = 2 });
                override_controls.Add(new _UControls.EntityCustom(chkLot) { RefGlobalVar = (REF) => { chkLot = REF; }, TabIndex = 2 });
                override_controls.Add(new _UControls.EntityCustom(chkExp) { RefGlobalVar = (REF) => { chkExp = REF; }, TabIndex = 2 });
                override_controls.Add(new _UControls.EntityCustom(chkAtt1) { RefGlobalVar = (REF) => { chkAtt1 = REF; }, TabIndex = 2 });
                override_controls.Add(new _UControls.EntityCustom(chkAtt2) { RefGlobalVar = (REF) => { chkAtt2 = REF; }, TabIndex = 2 });
                override_controls.Add(new _UControls.EntityCustom(chkAtt3) { RefGlobalVar = (REF) => { chkAtt3 = REF; }, TabIndex = 2 });
                override_controls.Add(new _UControls.EntityCustom(chkAtt4) { RefGlobalVar = (REF) => { chkAtt4 = REF; }, TabIndex = 2 });
                override_controls.Add(new _UControls.EntityCustom(chkAtt5) { RefGlobalVar = (REF) => { chkAtt5 = REF; }, TabIndex = 2 });
                override_controls.Add(new _UControls.EntityCustom(chkStatus) { RefGlobalVar = (REF) => { chkStatus = REF; }, TabIndex = 2 });

                #endregion


                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = access; };
                popupEntity1.AutoCreateControlEntity(entity, override_controls, tabs_attr, nameof(entity.override_receive_date), nameof(entity.location_status), nameof(entity.total_weight), nameof(entity.total_volumn), nameof(entity.pick_area_id));
                popupEntity1.AfterSetEditDataEvent += PopupEntity1_AfterSetEditDataEvent;
                popupEntity1.AfterNewDataEvent += PopupEntity1_AfterNewDataEvent;

                GridExt1.PopupEntitySource = popupEntity1;

                var ucUser = new _UControls.InputHidden();
                ucUser.DataFieldValue = "user_id";
                ucUser.SetValue(_SessionVals.UserName);
                GridExt1.AddFilterCustomInputInclude(ucUser);

                chkStatus.PostValueChanged += chkStatus_PostValueChanged;
                GridExt1.GridExportTemplate += GridExt1_GridExportTemplate;
                #endregion


            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void PopupEntity1_AfterNewDataEvent()
        {
            try
            { 
                chkLPN.Enabled = true;
                chkOwner.Enabled = true;
                chkItem.Enabled = true;
                chkLot.Enabled = true;
                chkExp.Enabled = true;
                chkAtt1.Enabled = true;
                chkAtt2.Enabled = true;
                chkAtt3.Enabled = true;
                chkAtt4.Enabled = true;
                chkAtt5.Enabled = true;
                chkStatus.Enabled = true;

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void chkStatus_PostValueChanged(dynamic obj)
        {
            try
            {
                if (popupEntity1.FormState == _UControls.FormState.Edit)
                {
                    var id = (Guid)popupEntity1.KeyFieldValue;
                    using (var _model = new WMS_NEW.Source.WMSEntities())
                    {
                        if (_model.v_wms_inventory_data.Any(w => w.location_id == id))
                        {
                            if (obj.ToString() == "NO")
                            {
                                chkStatus.Checked = true;
                                Page.MessageWarning("Can not change mix status. Bacause item has inventory.");
                                return;
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

        private void PopupEntity1_AfterSetEditDataEvent()
        {
            try
            {
                var id = (Guid)popupEntity1.KeyFieldValue;

                bool isInv = IsLocationInventory(id);
                chkLPN.Enabled = !isInv;
                chkOwner.Enabled = chkOwner.Checked && isInv ? false : true;
                chkItem.Enabled = chkItem.Checked && isInv ? false : true;
                chkLot.Enabled = chkLot.Checked && isInv ? false : true;
                chkExp.Enabled = chkExp.Checked && isInv ? false : true;
                chkAtt1.Enabled = chkAtt1.Checked && isInv ? false : true;
                chkAtt2.Enabled = chkAtt2.Checked && isInv ? false : true;
                chkAtt3.Enabled = chkAtt3.Checked && isInv ? false : true;
                chkAtt4.Enabled = chkAtt4.Checked && isInv ? false : true;
                chkAtt5.Enabled = chkAtt5.Checked && isInv ? false : true;
                chkStatus.Enabled = chkStatus.Checked && isInv ? false : true;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        #region Event

        #endregion

        #region Function
        bool IsLocationInventory(Guid _location_id)
        {
            using (var acc = new Access.MasterData.Location())
            {
                return acc.CheckLocation_Inventory(_location_id);
            }
        }
        #endregion

        private void GridExt1_GridExportTemplate()
        {
            try
            {
                #region template old
                List<string> HeaderCol = new List<string>();
                HeaderCol.Add("wh_code#");
                HeaderCol.Add("location#");
                HeaderCol.Add("description#");
                HeaderCol.Add("loc_type#");
                HeaderCol.Add("capacity_qty");
                HeaderCol.Add("lpn_controlled#");
                HeaderCol.Add("mix_owners#");
                HeaderCol.Add("mix_status#");
                HeaderCol.Add("mix_items#");
                HeaderCol.Add("mix_lot#");
                HeaderCol.Add("mix_expiry_dates#");
                HeaderCol.Add("mix_attribute1#");
                HeaderCol.Add("mix_attribute2#");
                HeaderCol.Add("mix_attribute3#");
                HeaderCol.Add("mix_attribute4#");
                HeaderCol.Add("mix_attribute5#");
                HeaderCol.Add("putaway_sequence");
                HeaderCol.Add("pick_sequence");
                HeaderCol.Add("x_cordinate");
                HeaderCol.Add("y_cordinate");
                HeaderCol.Add("location_level");
                HeaderCol.Add("erp_location");
                HeaderCol.Add("full_pallet#");

                using (var _model = new Source.WMSEntities())
                {
                    ExportExcel_EPPlus _excel = new ExportExcel_EPPlus();
                    // Insert Empty 
                    var List = new List<dynamic>();
                    _excel.ToExcel(List.ToDataTable(), "LocationTemplate.xlsx", HeaderCol.ToArray());
                }
                #endregion

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

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

                if (dt.Columns["wh_code"].IsEmpty() || dt.Columns["location"].IsEmpty() || dt.Columns["description"].IsEmpty() || dt.Columns["loc_type"].IsEmpty() || dt.Columns["capacity_qty"].IsEmpty() || dt.Columns["lpn_controlled"].IsEmpty() || dt.Columns["mix_owners"].IsEmpty() || dt.Columns["mix_status"].IsEmpty() || dt.Columns["mix_items"].IsEmpty() || dt.Columns["mix_lot"].IsEmpty() || dt.Columns["mix_expiry_dates"].IsEmpty() || dt.Columns["mix_attribute1"].IsEmpty() || dt.Columns["mix_attribute2"].IsEmpty() || dt.Columns["mix_attribute3"].IsEmpty() || dt.Columns["mix_attribute4"].IsEmpty() || dt.Columns["mix_attribute5"].IsEmpty() || dt.Columns["putaway_sequence"].IsEmpty() || dt.Columns["pick_sequence"].IsEmpty() || dt.Columns["x_cordinate"].IsEmpty() || dt.Columns["y_cordinate"].IsEmpty() || dt.Columns["location_level"].IsEmpty() || dt.Columns["erp_location"].IsEmpty() || dt.Columns["full_pallet"].IsEmpty())
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

                                        if (dr[0].ToString().Trim() == "" && dr[1].ToString().Trim() == "" && dr[2].ToString().Trim() == "" && dr[3].ToString().Trim() == "" && dr[4].ToString().Trim() == "" && dr[5].ToString().Trim() == "" && dr[6].ToString().Trim() == "" && dr[7].ToString().Trim() == "" && dr[8].ToString().Trim() == "" && dr[9].ToString().Trim() == "" && dr[10].ToString().Trim() == "" && dr[11].ToString().Trim() == "" && dr[12].ToString().Trim() == "" && dr[13].ToString().Trim() == "" && dr[14].ToString().Trim() == "" && dr[15].ToString().Trim() == "" && dr[16].ToString().Trim() == "" && dr[17].ToString().Trim() == "" && dr[18].ToString().Trim() == "" && dr[19].ToString().Trim() == "" && dr[20].ToString().Trim() == "" && dr[21].ToString().Trim() == "" && dr[22].ToString().Trim() == "")
                                        {
                                            continue;
                                        }


                                        string _warehouse_code = dr[0].ToString().Trim();
                                        string _location = dr[1].ToString().Trim();
                                        string _location_desc = dr[2].ToString().Trim();
                                        string _loc_type = dr[3].ToString().Trim();
                                        string _capacity_qty = dr[4].ToString().Trim();
                                        string _lpn_controlled = dr[5].ToString().Trim();
                                        string _mix_owners = dr[6].ToString().Trim();
                                        string _mix_status = dr[7].ToString().Trim();
                                        string _mix_items = dr[8].ToString().Trim();
                                        string _mix_lot = dr[9].ToString().Trim();
                                        string _mix_expiry_dates = dr[10].ToString().Trim();
                                        string _mix_attribute1 = dr[11].ToString().Trim();
                                        string _mix_attribute2 = dr[12].ToString().Trim();
                                        string _mix_attribute3 = dr[13].ToString().Trim();
                                        string _mix_attribute4 = dr[14].ToString().Trim();
                                        string _mix_attribute5 = dr[15].ToString().Trim();
                                        string _putaway_sequence = dr[16].ToString().Trim();
                                        string _pick_sequence = dr[17].ToString().Trim();
                                        string _x_cordinate = dr[18].ToString().Trim();
                                        string _y_cordinate = dr[19].ToString().Trim();
                                        string _location_level = dr[20].ToString().Trim();
                                        string _erp_location = dr[21].ToString().Trim();
                                        string _full_pallet = dr[22].ToString().Trim();

                                        // Guid _inbound_master_id = Guid.Empty;

                                        var warehouse = _model.t_wms_wh.Where(w => w.wh_id == _warehouse_code || w.wh_name == _warehouse_code).FirstOrDefault();
                                        var loctype = _model.t_com_combobox_item.Where(w => w.is_active == "YES" && w.group_name == "mst_location_type" && (w.display_member == _loc_type && w.value_member == _loc_type)).FirstOrDefault();

                                        if (warehouse == null)
                                        {
                                            string errItemCode = "ไม่พบข้อมูล Warehouse ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                            _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                            continue;
                                        }

                                        if (loctype == null)
                                        {
                                            string errItemCode = "ไม่พบข้อมูล Location Type ในระบบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                            _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                            continue;
                                        }

                                        if (string.IsNullOrEmpty(_location) || string.IsNullOrEmpty(_location_desc) || string.IsNullOrEmpty(_lpn_controlled) || string.IsNullOrEmpty(_mix_owners) || string.IsNullOrEmpty(_mix_status) || string.IsNullOrEmpty(_mix_items) || string.IsNullOrEmpty(_mix_lot) || string.IsNullOrEmpty(_mix_expiry_dates) || string.IsNullOrEmpty(_mix_attribute1) || string.IsNullOrEmpty(_mix_attribute2) || string.IsNullOrEmpty(_mix_attribute3) || string.IsNullOrEmpty(_mix_attribute4) || string.IsNullOrEmpty(_mix_attribute5) || string.IsNullOrEmpty(_full_pallet))
                                        {
                                            string errItemCode = "กรุณาใส่ข้อมูลที่จำเป็นให้ครบ"; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                            _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                            continue;
                                        }

                                        var dtoLocation = _model.t_wms_location.Where(w => w.location == _location && w.wh_master_id == warehouse.wh_master_id).FirstOrDefault();
                                        if (dtoLocation == null) // Insert
                                        {
                                            Source.t_wms_location _newLocation = new Source.t_wms_location();
                                            _newLocation.location_id = Guid.NewGuid();
                                            _newLocation.location = _location;
                                            _newLocation.description = _location_desc;
                                            _newLocation.wh_master_id = warehouse.wh_master_id;
                                            _newLocation.loc_type = loctype.value_member;

                                            double _capacity_qty_out;
                                            if (!string.IsNullOrEmpty(_capacity_qty) && Double.TryParse(_capacity_qty, out _capacity_qty_out))
                                                _newLocation.capacity_qty = _capacity_qty_out;

                                            _newLocation.lpn_controlled = _lpn_controlled.ToUpper() == "YES" ? _lpn_controlled.ToUpper() : "NO";
                                            _newLocation.mix_owners = _mix_owners.ToUpper() == "YES" ? _mix_owners.ToUpper() : "NO";
                                            _newLocation.mix_status = _mix_status.ToUpper() == "YES" ? _mix_status.ToUpper() : "NO";
                                            _newLocation.mix_items = _mix_items.ToUpper() == "YES" ? _mix_items.ToUpper() : "NO";
                                            _newLocation.mix_lot = _mix_lot.ToUpper() == "YES" ? _mix_lot.ToUpper() : "NO";
                                            _newLocation.mix_expiry_dates = _mix_expiry_dates.ToUpper() == "YES" ? _mix_expiry_dates.ToUpper() : "NO";
                                            _newLocation.mix_attribute1 = _mix_attribute1.ToUpper() == "YES" ? _mix_attribute1.ToUpper() : "NO";
                                            _newLocation.mix_attribute2 = _mix_attribute2.ToUpper() == "YES" ? _mix_attribute2.ToUpper() : "NO";
                                            _newLocation.mix_attribute3 = _mix_attribute3.ToUpper() == "YES" ? _mix_attribute3.ToUpper() : "NO";
                                            _newLocation.mix_attribute4 = _mix_attribute4.ToUpper() == "YES" ? _mix_attribute4.ToUpper() : "NO";
                                            _newLocation.mix_attribute5 = _mix_attribute5.ToUpper() == "YES" ? _mix_attribute5.ToUpper() : "NO";

                                            int _putaway_sequence_out;
                                            int _pick_sequence_out;
                                            int _x_cordinate_out;
                                            int _y_cordinate_out;

                                            // ✅ แก้ไข: ใช้ตัวแปรที่ถูกต้องในการ Parse
                                            if (!string.IsNullOrEmpty(_putaway_sequence) && int.TryParse(_putaway_sequence, out _putaway_sequence_out))
                                                _newLocation.putaway_sequence = _putaway_sequence_out;
                                            if (!string.IsNullOrEmpty(_pick_sequence) && int.TryParse(_pick_sequence, out _pick_sequence_out))
                                                _newLocation.pick_sequence = _pick_sequence_out;
                                            if (!string.IsNullOrEmpty(_x_cordinate) && int.TryParse(_x_cordinate, out _x_cordinate_out))
                                                _newLocation.x_cordinate = _x_cordinate_out;
                                            if (!string.IsNullOrEmpty(_y_cordinate) && int.TryParse(_y_cordinate, out _y_cordinate_out))
                                                _newLocation.y_cordinate = _y_cordinate_out;

                                            if (!string.IsNullOrEmpty(_location_level))
                                                _newLocation.location_level = _location_level;
                                            if (!string.IsNullOrEmpty(_erp_location))
                                                _newLocation.erp_location = _erp_location;

                                            _newLocation.create_by = _SessionVals.UserName;
                                            _newLocation.create_date = DateTime.Now;
                                            _newLocation.is_active = "YES";
                                            _newLocation.override_receive_date = "NO";
                                            _newLocation.is_full_pallet = _full_pallet.ToUpper() == "YES" ? _full_pallet.ToUpper() : "NO";

                                            _model.t_wms_location.Add(_newLocation);
                                        }
                                        else
                                        {
                                            dtoLocation.description = _location_desc;
                                            //dtoLocation.loc_type = loctype.value_member;

                                            double _capacity_qty_out;
                                            if (!string.IsNullOrEmpty(_capacity_qty) && Double.TryParse(_capacity_qty, out _capacity_qty_out))
                                                dtoLocation.capacity_qty = _capacity_qty_out;

                                            dtoLocation.lpn_controlled = _lpn_controlled.ToUpper() == "YES" ? _lpn_controlled.ToUpper() : "NO";
                                            dtoLocation.mix_owners = _mix_owners.ToUpper() == "YES" ? _mix_owners.ToUpper() : "NO";

                                            string _checkStatus = _mix_status.ToUpper() == "YES" ? _mix_status.ToUpper() : "NO";

                                            if (_model.v_wms_inventory_data.Any(w => w.location_id == dtoLocation.location_id))
                                            {
                                                if (_checkStatus == "NO" && dtoLocation.mix_status == "YES")
                                                {
                                                    string errItemCode = "Can not change mix status. Bacause item has inventory."; //"รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                                    _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                                    continue;
                                                }
                                            }

                                            dtoLocation.mix_status = _checkStatus;
                                            dtoLocation.mix_items = _mix_items.ToUpper() == "YES" ? _mix_items.ToUpper() : "NO";
                                            dtoLocation.mix_lot = _mix_lot.ToUpper() == "YES" ? _mix_lot.ToUpper() : "NO";
                                            dtoLocation.mix_expiry_dates = _mix_expiry_dates.ToUpper() == "YES" ? _mix_expiry_dates.ToUpper() : "NO";
                                            dtoLocation.mix_attribute1 = _mix_attribute1.ToUpper() == "YES" ? _mix_attribute1.ToUpper() : "NO";
                                            dtoLocation.mix_attribute2 = _mix_attribute2.ToUpper() == "YES" ? _mix_attribute2.ToUpper() : "NO";
                                            dtoLocation.mix_attribute3 = _mix_attribute3.ToUpper() == "YES" ? _mix_attribute3.ToUpper() : "NO";
                                            dtoLocation.mix_attribute4 = _mix_attribute4.ToUpper() == "YES" ? _mix_attribute4.ToUpper() : "NO";
                                            dtoLocation.mix_attribute5 = _mix_attribute5.ToUpper() == "YES" ? _mix_attribute5.ToUpper() : "NO";

                                            int _putaway_sequence_out;
                                            int _pick_sequence_out;
                                            int _x_cordinate_out;
                                            int _y_cordinate_out;

                                            // ✅ แก้ไข: ใช้ตัวแปรที่ถูกต้องในการ Parse
                                            if (!string.IsNullOrEmpty(_putaway_sequence) && int.TryParse(_putaway_sequence, out _putaway_sequence_out))
                                                dtoLocation.putaway_sequence = _putaway_sequence_out;
                                            if (!string.IsNullOrEmpty(_pick_sequence) && int.TryParse(_pick_sequence, out _pick_sequence_out))
                                                dtoLocation.pick_sequence = _pick_sequence_out;
                                            if (!string.IsNullOrEmpty(_x_cordinate) && int.TryParse(_x_cordinate, out _x_cordinate_out))
                                                dtoLocation.x_cordinate = _x_cordinate_out;
                                            if (!string.IsNullOrEmpty(_y_cordinate) && int.TryParse(_y_cordinate, out _y_cordinate_out))
                                                dtoLocation.y_cordinate = _y_cordinate_out;

                                            if (!string.IsNullOrEmpty(_location_level))
                                                dtoLocation.location_level = _location_level;
                                            if (!string.IsNullOrEmpty(_erp_location))
                                                dtoLocation.erp_location = _erp_location;
                                            dtoLocation.is_full_pallet = _full_pallet.ToUpper() == "YES" ? _full_pallet.ToUpper() : "NO";

                                            dtoLocation.update_by = _SessionVals.UserName;
                                            dtoLocation.update_date = DateTime.Now;
                                        }
                                        _model.SaveChanges();
                                    }
                                }
                                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                                {
                                    // ✅ เพิ่ม: แสดง Validation Error Details
                                    var errorMessages = dbEx.EntityValidationErrors
                                        .SelectMany(x => x.ValidationErrors)
                                        .Select(x => x.PropertyName + ": " + x.ErrorMessage);
                                    string fullErrorMessage = string.Join("; ", errorMessages);
                                    string errItemCode = "Validation Error: " + fullErrorMessage;
                                    _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                    continue;
                                }
                                catch (Exception ex)
                                {
                                    string errItemCode = "รูปแบบของข้อมูลที่นำเข้าไม่ถูกต้อง";
                                    _dvError[dt.Rows.IndexOf(dr)]["error"] = errItemCode;
                                    continue;
                                }
                            }
                        }
                        _dvError.RowFilter = "Error <> ''";
                        if (_dvError.Count > 0)
                        {
                            ExportExcel_EPPlus _excel = new ExportExcel_EPPlus();
                            _excel.ToExcel(_dvError.ToTable(), "LocationError.xlsx", columnNames);
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
    }
}