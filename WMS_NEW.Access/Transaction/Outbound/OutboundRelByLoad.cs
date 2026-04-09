using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Objects.SqlClient;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Outbound
{
    public class OutboundRelByLoad : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public OutboundRelByLoad()
        {
            this._Model = new WMSEntities();
            base.GridObjContext = _Model;

            this._Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
        }


        #region ++INSTANCE STATIC++
        public static OutboundRelByLoad Instance
        {
            get
            {
                using (OutboundRelByLoad _Instance = new OutboundRelByLoad())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Access Command Data

        public IEnumerable<usp_outbound_check_picklist_item_by_group_result> CheckItemQtyPreRelease(string _wh_id, string _load_id)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            var dto_wh = this._Model.t_wms_wh.Select(se => new { se.wh_master_id, se.wh_id }).First(x => x.wh_id == _wh_id);

            var result = this._Model.usp_outbound_check_picklist_item_by_group(_SessionVals.AppID, dto_wh.wh_master_id, _load_id, _SessionVals.UserName, errCode, errMsg).ToArray();

            return result;
        }

        public bool ReleaseLoadBySystem(string _wh_id, string _load_id)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_outbound_release_order_by_group(_SessionVals.AppID, _wh_id, _load_id, _SessionVals.DeviceID, _SessionVals.UserName, errCode, errMsg);

            var inverse = this._Model.Engaged(this, delegate ()
            {
                if (errCode.Value.ToString() == "0") return false;
                else return true;
            }, errMsg.Value.ToString());

            return !inverse;
        }

        public bool ReleaseLoadByUser(string _wh_id, string _owner_code, string _load_id, string _username)
        {
            var saved = this._Model.GetDataEntityBy(this, delegate ()
            {
                var dateNow = DateTime.Now;

                var result = _Model.t_wms_outbound_master.Where(x => x.wh_id == _wh_id && x.owner_code == _owner_code && x.load_id == _load_id);

                foreach (var ent in result)
                {
                    ent.order_status = "RELEASE";
                    ent.pick_type = "USER_GROUP";
                    ent.update_by = _username;
                    ent.update_date = dateNow;
                }

                return _Model.SaveChanges();
            });

            return saved > 0;
        }

        public bool UnreleaseLoadByUser(string _wh_id, string _owner_code, string _load_id)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            var dto_wh = this._Model.t_wms_wh.Select(se => new { se.wh_master_id, se.wh_id }).First(x => x.wh_id == _wh_id);

            this._Model.usp_outbound_unrelease_user_by_group(_SessionVals.AppID, dto_wh.wh_master_id, _SessionVals.DeviceID, _SessionVals.UserName, _load_id, errCode, errMsg);

            var inverse = this._Model.Engaged(this, delegate ()
            {
                if (errCode.Value.ToString() == "0") return false;
                else return true;
            }, errMsg.Value.ToString());

            return !inverse;
        }

        public bool UnreleaseLoadBySystem(string _wh_id, string _load_id)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            var dto_wh = this._Model.t_wms_wh.Select(se => new { se.wh_master_id, se.wh_id }).First(x => x.wh_id == _wh_id);

            this._Model.usp_outbound_unrelease_by_group(_SessionVals.AppID, dto_wh.wh_master_id, _SessionVals.DeviceID, _SessionVals.UserName, _load_id, errCode, errMsg);

            var inverse = this._Model.Engaged(this, delegate ()
            {
                if (errCode.Value.ToString() == "0") return false;
                else return true;
            }, errMsg.Value.ToString());

            return !inverse;
        }


        //public bool ReleaseLoadByUser(string _userId, string _wh_master_id, string _load_id)
        //{
        //    string[] status_rel = (from rows in this._Model.t_wms_rule
        //                           where rows.is_active == "YES" && rows.rule_code == "RULE_OUTBOUND_ORDER_STATUS_FOR_RELEASE"
        //                           select rows.value).ToArray();

        //    var chk = this._Model.Existed(this, delegate ()
        //    {
        //        if (this._Model.t_wms_outbound_master.Any(wh => wh.wh_master_id == _wh_master_id && wh.load_id == _load_id
        //            && status_rel.Contains(wh.order_status) && string.IsNullOrEmpty(wh.dock_door_id)))
        //            return true;
        //        else
        //            return false;
        //    }, "! Not allow release order, Because some order not yet assign dock door.");

        //    if (chk)
        //        return false;

        //    return this._Model.Update(this, delegate ()
        //    {
        //        var listEnt = this._Model.t_wms_outbound_master.Where(wh => wh.wh_master_id == _wh_master_id && wh.load_id == _load_id
        //            && status_rel.Contains(wh.order_status));
        //        foreach (var ent in listEnt)
        //        {
        //            ent.order_status = "RELEASE";
        //            ent.pick_type = "USER";
        //            ent.update_by = _userId;
        //            ent.update_date = DateTime.Now;
        //        }

        //        return this._Model.SaveChanges();
        //    }, "Release Order By Load Success.");
        //}

        //public bool ConfirmShipByLoad(string _appId, string deviceName, string _userId, string _wh_master_id, string _wh_id, string _load_id
        //   , string carrier_id, string truck_type_id, string truck_number, string driver_license)
        //{
        //    var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
        //    var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

        //    this._Model.usp_outbound_ship_confirm_by_load_id(_appId, _wh_id, deviceName, _userId, _load_id
        //        , carrier_id, truck_type_id, truck_number, driver_license, "", "", DateTime.Now, errCode, errMsg);

        //    var inverse = this._Model.Engaged(this, delegate ()
        //    {
        //        if (errCode.Value.ToString() == "0") return false;
        //        else return true;
        //    }, errMsg.Value.ToString());


        //    return !inverse;
        //}

        //public bool CancelOrderByLoad(string _appId, string _userId, string _deviceID, string _wh_master_id, string _load_id)
        //{
        //    var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
        //    var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

        //    var wh_id = this._Model.t_wms_wh.Where(wh => wh.wh_master_id == _wh_master_id).Select(se => se.wh_id).First();

        //    this._Model.Usp_OutboundCancelOrder_ByGroup(_appId, wh_id, _deviceID, _userId, _load_id, string.Empty, errCode, errMsg);

        //    var inverse = this._Model.Engaged(this, delegate ()
        //    {
        //        if (errCode.Value.ToString() == "0") return false;
        //        else return true;
        //    }, errMsg.Value.ToString());

        //    return !inverse;
        //}

        //public bool UnreleaseOrderByLoad(string _appId, string _userId, string _deviceID, string _wh_master_id, string _load_id)
        //{
        //    var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
        //    var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

        //    var wh_id = this._Model.t_wms_wh.Where(wh => wh.wh_master_id == _wh_master_id).Select(se => se.wh_id).First();

        //    this._Model.usp_outbound_unrelease_by_group(_appId, wh_id, _deviceID, _userId, _load_id, errCode, errMsg);

        //    var inverse = this._Model.Engaged(this, delegate ()
        //    {
        //        if (errCode.Value.ToString() == "0") return false;
        //        else return true;
        //    }, errMsg.Value.ToString());

        //    return !inverse;
        //}

        //public string GetTruckTypeByConfirmShipLoad(string _wh_master_id, string _load_id)
        //{
        //    var out_vchTruckTypeID = new ObjectParameter("out_vchTruckTypeID", typeof(string));
        //    var out_vchTruckType = new ObjectParameter("out_vchTruckType", typeof(string));

        //    this._Model.usp_outbound_suggest_truck_type(_load_id, _wh_master_id, out_vchTruckTypeID, out_vchTruckType);

        //    return out_vchTruckType.Value.ToString();
        //}

        #endregion

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _userID = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userID").Value;
            var _isFirstLoad = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_isFirstLoad").Value;
            var _userPickType = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userPickType").Value;
            var _orderNumber = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_orderNumber").Value;

            string[] status_rel = (from rows in this._Model.t_wms_rule
                                   where rows.is_active == "YES" && rows.rule_code == "RULE_OUTBOUND_ORDER_STATUS_FOR_RELEASE"
                                   select rows.value).ToArray();

            string[] status_ship = (from rows in this._Model.t_wms_rule
                                    where rows.is_active == "YES" && rows.rule_code == "RULE_OUTBOUND_ORDER_STATUS_FOR_SHIP"
                                    select rows.value).ToArray();

            string[] pick_type = new[] { "", _userPickType };

            var query = from rows in this._Model.v_wms_outbound_rel_by_load_header
                        where rows.user_id == _userID && pick_type.Contains(rows.pick_type) 
                        select rows;

            var exclude_stat = new[] { "CLOSE" };

            if (!string.IsNullOrEmpty(_isFirstLoad))
            {
                query = from rows in query
                        where !exclude_stat.Contains(rows.load_status)
                        select rows;
            }

            if (!string.IsNullOrEmpty(_orderNumber))
            {
                query = from rows in query
                        where this._Model.t_wms_outbound_master.Any(wh => wh.wh_master_id == rows.wh_master_id && wh.owner_id == rows.owner_id
                                                                       && wh.load_id == rows.load_id && wh.outbound_order_number.Contains(_orderNumber))
                        select rows;
            }

            var result = from rows in query
                         select new
                         {
                             KeyId = rows.wh_id + "|" + rows.owner_code + "|" + rows.load_id + "|" + rows.load_status + "|" + rows.priority  +"|" + rows.ship_date_actual,
                             release = "",
                             unrel = "",
                             //ship = "",
                             //cancel = "",
                             allow_release = status_rel.Contains(rows.load_status) ? true : false,
                             allow_unrel = rows.load_status == "RELEASE" ? true : false,
                             //allow_ship = status_ship.Contains(rows.load_status) ? true : false,
                             //allow_cancel = rows.cancel_status,
                             rows.wh_id,
                             rows.owner_code,
                             rows.owner_id,
                             rows.load_id,
                             //location_dock = rows.location,
                             rows.order_qty,
                             rows.load_status,
                             rows.create_date,
                             rows.priority,
                             rows.ship_date_actual,
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


        //public OutboundMasterLoadDTO GetDTOByUniqKey(string _wh_master_id, string _owner_id, string _load_id)
        //{
        //    return this._Model.GetDataEntityBy<OutboundMasterLoadDTO>(this, delegate ()
        //    {
        //        var result = (from rows in this._Model.v_wms_outbound_rel_by_load
        //                      where rows.wh_master_id == _wh_master_id && rows.owner_id == _owner_id && rows.load_id == _load_id
        //                      select new OutboundMasterLoadDTO
        //                      {
        //                          wh_master_id = rows.wh_master_id,
        //                          wh_id = rows.wh_id,
        //                          owner = rows.owner_code,
        //                          load_id = rows.load_id,
        //                          dock_door = rows.location
        //                      }).FirstOrDefault();

        //        return result;
        //    });
        //}
    }
}
