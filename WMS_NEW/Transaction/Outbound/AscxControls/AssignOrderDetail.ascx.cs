using _UControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Outbound.AscxControls {
    public partial class AssignOrderDetail : UControlCustom, IFormRelation {
        public Action<dynamic> UpdateParent { get; set; }

        protected void Page_Load(object sender, EventArgs e) {
            try {
                GridExt1.GridRowCanSelectValidate += GridExt1_GridRowCanSelectValidate;
                GridExt1.GridRowCommandClick += GridExt1_GridRowCommandClick;
                GridExt1.GridRowAfterDataBound += GridExt1_GridRowAfterDataBound;               

                if (!Page.IsPostBack) {
                   
                }
            } catch (Exception ex) {
                this.Logging = new Prototype.Providers.Logging(this, ex);
                this.RaiseLogging();
            }
        }

        private bool GridExt1_GridRowCanSelectValidate(GridViewRowEventArgs e) {
            var allow_check = (int)DataBinder.Eval(e.Row.DataItem, "is_allow_check");
            if (allow_check == 1) {
                return true;
            } else {
                return false;
            }            
        }

        private void GridExt1_GridRowAfterDataBound(GridViewRowEventArgs e) {
            try {
                var is_checked = (int)DataBinder.Eval(e.Row.DataItem, "is_checked");
                var btUncheckAll = (Button)e.Row.FindControl("Uncheck All");
                btUncheckAll.CssClass = "btn btn-sm btn-warning btn-ingrid";
                if (is_checked == 1) {
                    btUncheckAll.Enabled = true;  
                } else {
                    btUncheckAll.Enabled = false;
                }
                //ปิดปุ่ม UncheckAll
                if (hidOrderStatus.GetValue() == "SHIP" || hidOrderStatus.GetValue() == "CANCEL" || hidOrderStatus.GetValue() == "CLOSE") {
                    btUncheckAll.Enabled = false;
                } 
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void InitForm(dynamic _obj) {
            try {
                hidWhMasterId.SetValue(_obj.wh_master_id);
                hidWId.SetValue(_obj.wh_id);
                hidOwnerID.SetValue(_obj.owner_id);
                hidOwnerCode.SetValue(_obj.owner_code);
                hidOrderMasterId.SetValue(_obj.outbound_order_master_id);
                hidOrderOrderNumber.SetValue(_obj.outbound_order_number);
                hidOrderStatus.SetValue(_obj.order_status);
                //
                GridExt1.Search();
                GridExt1.GridAllowShowSelectBoxAll = false;
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void SaveAssignOrder() {
            try {
                if (GridExt1 == null || GridExt1.Rows.Count <= 0) {
                    Page.MessageWarning("Please select door!");
                    return;
                }
                using (var _ass = new Access.Transaction.Outbound.AssignOrderDetailSourceStored()) {
                    string order_status = _ass.GetOrderStatus(hidOrderMasterId.GetValue());
                    if (order_status == "" || order_status == "CLOSE" || order_status == "CANCEL") {
                        Page.MessageWarning("Not allow Order status [" + order_status  + "] to save");
                        GridExt1.Search();
                        GridExt1.GridAllowShowSelectBoxAll = false;
                        return;
                    }
                    int listUnselect = GridExt1.GetListKey().Where(w => w.Active == false).Select(s => s.KeyId).ToList().Count;
                    if (order_status != "OPEN") {
                        int CountAssignOrder = _ass.GetCountAssignOrder(hidOrderMasterId.GetValue());  
                        if ((CountAssignOrder - listUnselect) == 0) {
                            Page.MessageWarning("Order status [" + order_status + "] not allow all un-assign");
                            GridExt1.Search();
                            GridExt1.GridAllowShowSelectBoxAll = false;
                            return;
                        }
                        string door_already_checker = string.Empty;
                        int CountChecker = _ass.GetCountAlreadyCheckPickDetail(GridExt1.GetListKey(), hidOrderMasterId.GetValue(), ref door_already_checker);
                        if (CountChecker > 0) {
                            Page.MessageWarning("Not allow un-assign, door [" + door_already_checker + "] already checked");
                            GridExt1.Search();
                            GridExt1.GridAllowShowSelectBoxAll = false;
                            return;
                        }
                    }
                    //
                    if (_ass.SaveMapping(GridExt1.GetListKey(), _SessionVals.UserName, hidOrderMasterId.GetValue(), hidOrderOrderNumber.GetValue())) {
                        GridExt1.Search();
                        GridExt1.GridAllowShowSelectBoxAll = false;
                        Page.MessageSuccess("Save Success");
                    } else {
                        //ถ้าขึ้น Error เป็นไปได้ว่าเกี่ยวกับ The INSERT statement conflicted with the FOREIGN KEY constraint "FK_t_tms_manifest_order_mapping_t_tms_truck_manifest".
                        //The conflict occurred in database "WM3", table "dbo.t_tms_truck_manifest", column 'truck_manifest_id'.
                        Page.MessageWarning("Save Fail!");
                    }
                }
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void GridExt1_GridRowCommandClick(GridViewCommandEventArgs e) {
            try {
                string _errCode = "", _errMsg = "";
                string[] KeyId = e.CommandArgument.ToString().Split('|');
                //0 cast(isnull(m.truck_manifest_id, newid()) as varchar(36)) + ''|'' + 1 cast(isnull(l.location_id,''00000000-0000-0000-0000-000000000000'') as varchar(36))
                //+ ''|'' + 2 isnull(l.[location],''00000000-0000-0000-0000-000000000000'') + ''|'' + 3 cast(isnull(op.outbound_pick_master_id,''00000000-0000-0000-0000-000000000000'') as varchar(36)) as KeyId          
                //
                if (e.CommandName == "Uncheck All") {                    
                    if (KeyId[2] != string.Empty) {                        
                        Nullable<System.Guid> in_vchWhMasterID = hidWhMasterId.GetValue();
                        string in_vchWarehouse = hidWId.GetValue();
                        Nullable<System.Guid> in_vchOwnerID = hidOwnerID.GetValue();
                        string in_vchOwnerCode = hidOwnerCode.GetValue();
                        Nullable<System.Guid> in_vchDoorLocationID = Guid.Parse(KeyId[1]);
                        string in_vchDoor = KeyId[2];
                        string in_vchOutboundOrderNumber = hidOrderOrderNumber.GetValue();
                        Nullable<System.Guid> in_vchOutboundMasterID = hidOrderMasterId.GetValue();
                        Nullable<System.Guid> in_vchOutboundPickMasterID = Guid.Parse(KeyId[3]); ;
                        using (var _ass = new Access.Transaction.Outbound.AssignOrderUncheck()) {
                            _ass.UncheckAll(
                                in_vchWhMasterID
                                , in_vchWarehouse
                                , in_vchOwnerID
                                , in_vchOwnerCode
                                , in_vchDoorLocationID
                                , in_vchDoor
                                , in_vchOutboundOrderNumber
                                , in_vchOutboundMasterID
                                , in_vchOutboundPickMasterID
                                , out _errCode, out _errMsg);
                            if (_errCode == "0") {
                                Page.MessageSuccess(_errMsg);
                                GridExt1.Search();
                                GridExt1.GridAllowShowSelectBoxAll = false;
                            } else {
                                Page.MessageWarning(_errMsg);
                            }
                        }
                    } else {
                        Page.MessageWarning("Not found door!");
                    }
                }
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

    }
}