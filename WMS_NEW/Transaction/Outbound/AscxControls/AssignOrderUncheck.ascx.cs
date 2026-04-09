using _UControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Outbound.AscxControls {
    public partial class AssignOrderUncheck : UControlCustom, IFormRelation {

        public Action<dynamic> UpdateParent { get; set; }
        protected void Page_Load(object sender, EventArgs e) {
            try {
                GridExt1.GridRowCommandClick += GridExt1_GridRowCommandClick;
                GridExt1.GridRowAfterDataBound += GridExt1_GridRowAfterDataBound;
                if (!Page.IsPostBack) {

                }
            } catch (Exception ex) {
                this.Logging = new Prototype.Providers.Logging(this, ex);
                this.RaiseLogging();
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

        private void GridExt1_GridRowAfterDataBound(GridViewRowEventArgs e) {
            try {                
                var btUncheck = (Button)e.Row.FindControl("Uncheck");                
                //ปิดปุ่ม UncheckAll
                if (hidOrderStatus.GetValue() == "SHIP" || hidOrderStatus.GetValue() == "CANCEL" || hidOrderStatus.GetValue() == "CLOSE") {
                    btUncheck.Enabled = false;
                }
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        // ยกเลิก
        protected void btUncheckAll_Click(object sender, EventArgs e) {
            try {
                ;
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void GridExt1_GridRowCommandClick(GridViewCommandEventArgs e) {
            try {
                string _errCode = "", _errMsg = "";
                string[] KeyId = e.CommandArgument.ToString().Split('|');
                //KeyId = 0 rows.outbound_pick_master_id + "|" + 1 rows.outbound_pick_detail_id + "|" + 2 rows.location_id + "|" + 3 rows.door + "|" + 4 rows.lpn                
                //
                if (e.CommandName == "Uncheck") {
                    if (KeyId[2] != string.Empty) {
                        Nullable<System.Guid> in_vchWhMasterID = hidWhMasterId.GetValue();
                        string in_vchWarehouse = hidWId.GetValue();
                        Nullable<System.Guid> in_vchOwnerID = hidOwnerID.GetValue();
                        string in_vchOwnerCode = hidOwnerCode.GetValue();
                        Nullable<System.Guid> in_vchDoorLocationID = Guid.Parse(KeyId[2]);
                        string in_vchDoor = KeyId[3];
                        string in_vchOutboundOrderNumber = hidOrderOrderNumber.GetValue();
                        Nullable<System.Guid> in_vchOutboundMasterID = hidOrderMasterId.GetValue();
                        Nullable<System.Guid> in_vchOutboundPickMasterID = Guid.Parse(KeyId[0]);
                        string in_vchLPN = KeyId[4];
                        using (var _ass = new Access.Transaction.Outbound.AssignOrderUncheck()) {
                            _ass.UncheckByLPN(
                                in_vchWhMasterID
                                , in_vchWarehouse
                                , in_vchOwnerID
                                , in_vchOwnerCode
                                , in_vchDoorLocationID
                                , in_vchDoor
                                , in_vchOutboundOrderNumber
                                , in_vchOutboundMasterID
                                , in_vchOutboundPickMasterID
                                , in_vchLPN
                                , out _errCode, out _errMsg);
                            if (_errCode == "0") {
                                Page.MessageSuccess(_errMsg);
                                GridExt1.Search();
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