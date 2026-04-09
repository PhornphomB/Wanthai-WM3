using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Outbound {
    public class AssignOrderUncheck : AEntityFormCommand<v_tms_assign_order_uncheck> {
        #region ++INSTANCE STATIC++
        public static AssignOrderUncheck Instance {
            get {
                using (AssignOrderUncheck _Instance = new AssignOrderUncheck()) {
                    return _Instance;
                }
            }
        }
        #endregion
        protected WMSEntities _Model { get; set; }

        public AssignOrderUncheck() {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_outbound_pick_detail; };
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView() {
            Guid _wh_master_id = new Guid();
            Guid _outbound_order_master_id = new Guid();

            if (this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_wh_master_id").Value != null) {
                _wh_master_id = (Guid)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_wh_master_id").Value;
            }
            if (this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_outbound_order_master_id").Value != null) {
                _outbound_order_master_id = (Guid)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_outbound_order_master_id").Value;
            }

            var result = from rows in this._Model.v_tms_assign_order_uncheck
                         where rows.outbound_order_master_id == _outbound_order_master_id
                         select new {
                             KeyId = rows.outbound_pick_master_id + "|" +rows.outbound_pick_detail_id + "|" + rows.location_id + "|" + rows.door + "|" + rows.lpn
                             , rows.outbound_pick_detail_id
                             , rows.outbound_pick_master_id
                             , rows.outbound_order_number
                             , rows.outbound_order_master_id
                             , rows.lpn
                             , rows.check_date
                             , rows.location_id
                             , rows.door
                             , rows.truck_type
                             , rows.license
                             , rows.head_tail
                             , rows.register_date
                         };
            return result;
        }

        public override IQueryable<dynamic> InitialQueryExport() {
            return this.InitialQueryView();
        }

        public void UncheckAll(Nullable<System.Guid> in_vchWhMasterID, string in_vchWarehouse, Nullable<System.Guid> in_vchOwnerID, string in_vchOwnerCode
            , Nullable<System.Guid> in_vchDoorLocationID, string in_vchDoor, string in_vchOutboundOrderNumber, Nullable<System.Guid> in_vchOutboundMasterID
            , Nullable<System.Guid> in_vchOutboundPickMasterID, out string _errCode, out string _errMsg) {

            string in_vchApplicationID = _SessionVals.AppID;
            string in_vchDeviceID = _SessionVals.DeviceID;
            string in_vchUserID = _SessionVals.UserName;
            //
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));
            //
            this._Model.usp_outbound_unchecker_all(
                in_vchApplicationID
                , in_vchDeviceID
                , in_vchUserID
                , in_vchWhMasterID
                , in_vchWarehouse
                , in_vchOwnerID
                , in_vchOwnerCode
                , in_vchDoorLocationID
                , in_vchDoor
                , in_vchOutboundOrderNumber
                , in_vchOutboundMasterID
                , in_vchOutboundPickMasterID
                , errCode, errMsg);
            //
            _errCode = errCode.Value.ToString();
            _errMsg = errMsg.Value.ToString();
        }

        public void UncheckByLPN(Nullable<System.Guid> in_vchWhMasterID, string in_vchWarehouse, Nullable<System.Guid> in_vchOwnerID, string in_vchOwnerCode
            , Nullable<System.Guid> in_vchDoorLocationID, string in_vchDoor, string in_vchOutboundOrderNumber, Nullable<System.Guid> in_vchOutboundMasterID
            , Nullable<System.Guid> in_vchOutboundPickMasterID, string in_vchLPN, out string _errCode, out string _errMsg) {

            string in_vchApplicationID = _SessionVals.AppID;
            string in_vchDeviceID = _SessionVals.DeviceID;
            string in_vchUserID = _SessionVals.UserName;
            //
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));
            //
            this._Model.usp_outbound_unchecker_by_lpn(
                in_vchApplicationID
                , in_vchDeviceID
                , in_vchUserID
                , in_vchWhMasterID
                , in_vchWarehouse
                , in_vchOwnerID
                , in_vchOwnerCode
                , in_vchDoorLocationID
                , in_vchDoor
                , in_vchOutboundOrderNumber
                , in_vchOutboundMasterID
                , in_vchOutboundPickMasterID
                , in_vchLPN
                , errCode, errMsg);
            //
            _errCode = errCode.Value.ToString();
            _errMsg = errMsg.Value.ToString();
        }


        #endregion
    }
}
