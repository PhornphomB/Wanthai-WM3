using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Outbound
{
    public class OutboundRelease : AGridObjectSourceQuery, IDisposable
    {
        public WMSEntities _Model { get; set; }

        public OutboundRelease()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;
            this._Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

            base.GridObjContext = _Model;
        }


        #region Access Command Data

        public IEnumerable<usp_outbound_check_picklist_item_result> CheckItemQtyPreRelease(Guid _outbound_order_master_id)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            var result = this._Model.usp_outbound_check_picklist_item(_SessionVals.AppID, _outbound_order_master_id, _SessionVals.UserName, errCode, errMsg).ToArray();

            return result;
        }

        public bool ReleaseOrderBySystem(Guid _outbound_order_master_id)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_outbound_release_order(_SessionVals.AppID, _outbound_order_master_id, _SessionVals.DeviceID, _SessionVals.UserName, errCode, errMsg);

            var inverse = this._Model.Engaged(this, delegate ()
            {
                if (errCode.Value.ToString() == "0") return false;
                else return true;
            }, errMsg.Value.ToString());

            return !inverse;
        }

        public bool ReleaseOrderByUser(Guid _outbound_order_master_id)
        {
            return this._Model.Update(this, delegate ()
            {
                var result = 0;

                var ent = this._Model.t_wms_outbound_master.FirstOrDefault(x => x.outbound_order_master_id == _outbound_order_master_id && x.order_status == "OPEN");
                if (ent != null)
                {
                    ent.order_status = "RELEASE";
                    ent.pick_type = "User";
                    ent.user_def10 = DateTime.Now;
                    ent.update_by = _SessionVals.UserName;
                    ent.update_date = ent.user_def10;
                    ent.release_by = _SessionVals.UserName;
                    ent.release_date = DateTime.Now;

                    result = this._Model.SaveChanges();
                }

                return result;
            }, "Release Order Success.");
        }

        public bool UnreleaseOrder(Guid _outbound_order_master_id)
        {

            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_outbound_unrelease(_SessionVals.AppID, _SessionVals.DeviceID, _SessionVals.UserName, _outbound_order_master_id, errCode, errMsg);

            var inverse = this._Model.Engaged(this, delegate ()
            {
                if (errCode.Value.ToString() == "0") return false;
                else return true;
            }, errMsg.Value.ToString());

            return !inverse;
        }

        public bool CloseOrder(string _wh_id, Guid _outbound_order_master_id, string _closeRemark, DateTime _closeDate)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_close_outbound_order(_SessionVals.AppID, _wh_id, _SessionVals.DeviceID, _SessionVals.UserName, _outbound_order_master_id, _closeRemark, _closeDate, errCode, errMsg);

            var is_success = errCode.Value.ToString() == "0";
            var msg = errMsg.Value.ToString();

            if (is_success)
                this.MessageSuccess(this, msg);
            else
                this.MessageWarning(this, msg);

            return is_success;
        }

        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _userID = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userID").Value;
            var _isFirstLoad = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_isFirstLoad").Value;
            var _userPickType = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userPickType").Value;

            var _cateID = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_cateID").Value;
            var _itemNumber = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_itemNumber").Value;

            var entLot = this.FilterCustom.Where(w => w.DataFieldValue == "_lot_number").FirstOrDefault();
            var entExp = this.FilterCustom.Where(w => w.DataFieldValue == "_exp_date").FirstOrDefault();

            string[] status = (from rows in this._Model.t_wms_rule
                               where rows.is_active == "YES" && rows.rule_code == "RULE_OUTBOUND_ORDER_STATUS_FOR_RELEASE"
                               select rows.value).ToArray();

            string[] pick_type = new[] { null, "BLIND PICK", _userPickType };

            if(_userPickType == "SYSTEM")
            {
                pick_type = new[] { null, "BLIND PICK", _userPickType, "SYSTEM_GROUP" };
            }

            var query = from master in this._Model.t_wms_outbound_master
                        where pick_type.Contains(master.pick_type)
                          && master.t_wms_wh.t_wms_wh_user.Any(qry => qry.user_id == _userID) && master.t_wms_outbound_detail.Any()
                          && master.t_wms_owner.t_wms_owner_user.Any(an => an.user_id == _userID && an.is_active == "YES")
                        select master;

            string[] exclude_stat = new[] { "CLOSE", "SHIP" };

            if (!string.IsNullOrEmpty(_isFirstLoad))
            {
                query = from rows in query
                        where !exclude_stat.Contains(rows.order_status)
                        select rows;
            }

            if (_cateID != null)
            {
                var _cateIDFilter = (Guid)_cateID;

                query = from master in query
                        where master.t_wms_outbound_detail.Any(qry => qry.t_wms_wh_item.t_wms_item.category_id == _cateIDFilter)
                        select master;
            }



            //พี่นัท Require 17/03/2021
            if (!string.IsNullOrEmpty(_itemNumber))
            {
                //query = from master in query
                //        where master.t_wms_outbound_detail.Any(qry => qry.t_wms_wh_item.t_wms_item.item_number.Contains(_itemNumber))
                //        select master;
                query = query.Where(w => w.t_wms_outbound_pick_master.Any(a => a.t_wms_outbound_pick_detail.Any(a2 => a2.t_wms_wh_item.t_wms_item.item_number.Contains(_itemNumber))));
            }

            if (entLot != null && entLot.Value != null)
            {
                string lot_number = entLot.Value.ToString();
                query = query.Where(w => w.t_wms_outbound_pick_master.Any(a => a.t_wms_outbound_pick_detail.Any(a2 => a2.lot_number.Contains(lot_number))));
            }

            if (entExp != null && entExp.Value != null)
            {
                string exp = ((DateTime)entExp.Value).ToString("yyyyMMdd");
                query = query.Where(w => w.t_wms_outbound_pick_master.Any(a => a.t_wms_outbound_pick_detail.Any(a2 => a2.expiry_date == exp)));
            }

            //Kritsada : WanThai : 2024-03-04 : ถ้า Order type เป็นตาม Rule RULE_CHECKER_ORDER_TYPE ให้เช็คว่ามีการ Assgin order หรือยัง ถ้ายังจะยังไม่ให้กดปุ่ม Release ได้
            string[] statusChecker = (from rows in this._Model.t_wms_rule
                                      where rows.is_active == "YES" && rows.rule_code == "RULE_CHECKER_ORDER_TYPE"
                                      select rows.value).ToArray();

            var ruleOrderTypes = this._Model.t_wms_rule
            .Where(rule => rule.rule_code == "RULE_ORDER_TYPE_INSERT_INBOUND" && rule.is_active == "YES")
            .Select(rule => rule.value)
            .ToList();

            var result = from rows in query
                         join wh in this._Model.t_wms_wh on rows.department equals wh.wh_master_id.ToString() into whJoin
                         from wh in whJoin.DefaultIfEmpty()
                         let qty_order = rows.t_wms_outbound_detail.Sum(sm => sm.quantity_order)
                         let qty_pick = rows.t_wms_outbound_detail.Sum(sm => sm.quantity_pick)
                         //
                         let assign_order_count = rows.t_tms_manifest_order_mapping.Count(count_r => count_r.outbound_order_master_id == rows.outbound_order_master_id)
                         select new
                         {
                             KeyId = rows.outbound_order_master_id,
                             release = "",
                             unrel = "",
                             allow_release = status.Contains(rows.order_status) ? true : false,
                             allow_unrel = rows.order_status == "RELEASE" ? true : false,
                             rows.t_wms_owner.owner_code,
                             rows.t_wms_owner.owner_id,
                             rows.t_wms_wh.wh_id,
                             rows.outbound_order_number,
                             rows.load_id,
                             rows.order_type,
                             rows.order_date,
                             rows.delivery_date_plan,
                             rows.loading_date_plan,
                             rows.ship_date_plan,
                             rows.ship_date_actual,
                             rows.order_status,
                             rows.customer_id,
                             rows.customer_code,
                             rows.t_wms_customer.customer_name,
                             qty_order = qty_order,
                             qty_pick = qty_pick,
                             rows.create_by,
                             rows.create_date,
                             rows.update_by,
                             rows.update_date,
                             rows.customer_purchase_order,
                             rows.t_wms_customer.province,
                             rows.customer_order_number,
                             //
                             allow_release_order_type_checker = statusChecker.Contains(rows.order_type) ? true : false,
                             assign_order_count = assign_order_count,
                             department = ruleOrderTypes.Contains(rows.order_type) ? wh.wh_id : rows.department,
                             rows.priority,
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion
    }
}
