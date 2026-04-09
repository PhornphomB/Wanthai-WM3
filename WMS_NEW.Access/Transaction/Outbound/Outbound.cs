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
    [Serializable()]
    public class OutboundMasterDto
    {
        public OutboundMasterDto()
        {
            sum_qty_plan = 0;
            sum_qty_pick = 0;
            sum_qty_stage = 0;
            sum_qty_load = 0;
            sum_qty_ship = 0;
        }

        public Guid outbound_order_master_id { get; set; }
        public Guid wh_master_id { get; set; }
        public string wh_id { get; set; }
        public string outbound_order_number { get; set; }
        public string customer_name { get; set; }
        public Guid owner_id { get; set; }
        public string owner_code { get; set; }
        public string order_status { get; set; }

        public double? sum_qty_plan { get; set; }
        public double? sum_qty_pick { get; set; }
        public double? sum_qty_stage { get; set; }
        public double? sum_qty_load { get; set; }
        public double? sum_qty_ship { get; set; }

        public Guid? carrier_id { get; set; }

        public DateTime? ship_date_actual { get; set; }
    }


    public class Outbound : AEntityFormCommand<t_wms_outbound_master>
    {
        #region ++INSTANCE STATIC++
        public static Outbound Instance
        {
            get
            {
                using (Outbound _Instance = new Outbound())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public WMSEntities _Model { get; set; }

        public Outbound()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_outbound_master; };
            this._Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
        }


        public override bool ValidateSaveNew(t_wms_outbound_master ent, ref string msg_validate)
        {
            if (_Model.t_wms_outbound_master.Any(qry => qry.wh_master_id == ent.wh_master_id && qry.outbound_order_number == ent.outbound_order_number))
            {
                msg_validate = "! Warehouse ID and Outbound Order No. has existed key.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _userID = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userID").Value;
            var _isFirstLoad = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_isFirstLoad").Value;

            var _cateID = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_cateID").Value;
            var _itemNumber = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_itemNumber").Value;


            var query = from master in this._Model.t_wms_outbound_master
                        where master.t_wms_wh.t_wms_wh_user.Any(qry => qry.user_id == _userID)
                        && master.t_wms_owner.t_wms_owner_user.Any(an => an.user_id == _userID && an.is_active == "YES")
                        select master;

            var exclude_stat = new[] { "CLOSE" };

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

            if (!string.IsNullOrEmpty(_itemNumber))
            {
                query = from master in query
                        where master.t_wms_outbound_detail.Any(qry => qry.t_wms_wh_item.t_wms_item.item_number.Contains(_itemNumber))
                        select master;
            }


            var result = from rows in query
                         select new
                         {
                             KeyId = rows.outbound_order_master_id,
                             rows.owner_code,
                             rows.owner_id,
                             rows.wh_id,
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
                             rows.t_wms_customer.customer_code,
                             rows.t_wms_customer.customer_name,
                             rows.create_by,
                             rows.create_date,
                             rows.customer_purchase_order,
                             total_quantity = (double?)rows.t_wms_outbound_detail.Sum(s => s.quantity_order) ?? 0,
                             rows.t_wms_customer.province,
                             rows.customer_order_number,
                             rows.priority
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion

        public override bool DeleteById(object _Id)
        {
            return this._Model.Delete(this, delegate ()
            {
                Guid outbound_order_master_id = (Guid)_Id;
                var ent = this._Model.t_wms_outbound_master.Where(w => w.outbound_order_master_id == outbound_order_master_id).FirstOrDefault();
                if (ent != null && ent.order_status == "OPEN")
                {
                    var entDet = this._Model.t_wms_outbound_detail.Where(w => w.outbound_order_master_id == ent.outbound_order_master_id);
                    this._Model.t_wms_outbound_detail.RemoveRange(entDet);

                    this._Model.t_wms_outbound_master.Remove(ent);
                }

                return this._Model.SaveChanges();
            });
        }

        #region Customize

        public bool CancelOrder(Guid _wh_master_id, string _wh_id, Guid _outbound_order_ms_id, string _remark_desc)
        {

            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_outbound_cancel_order(_SessionVals.AppID, _wh_id, _SessionVals.DeviceID, _SessionVals.UserName, _outbound_order_ms_id, _remark_desc, errCode, errMsg);

            var inverse = this._Model.Engaged(this, delegate ()
            {
                if (errCode.Value.ToString() == "0") return false;
                else return true;
            }, errMsg.Value.ToString());

            return !inverse;
        }

        public DataTable CheckItemNotInWarehouseOwner(List<Guid> _listKey, Guid _wh_master_id, Guid _owner_id)
        {
            return this._Model.GetDataTable(this, delegate ()
            {
                var resultItems = (from rows in this._Model.t_wms_outbound_detail
                                   where _listKey.Contains(rows.outbound_order_master_id)
                                   select rows.t_wms_wh_item.item_master_id).Distinct();

                var result = from rows in this._Model.t_wms_item
                             where resultItems.Contains(rows.item_master_id)
                                  && ((!rows.t_wms_wh_item.Any(ext => ext.wh_master_id == _wh_master_id)) || (rows.owner_id != _owner_id))

                             orderby rows.item_number
                             select new
                             {
                                 rows.item_number,
                                 has_wh = rows.t_wms_wh_item.Any(ext => ext.wh_master_id == _wh_master_id) ? "YES" : "NO",
                                 has_ow = (rows.owner_id == _owner_id) ? "YES" : "NO"
                             };

                return result;
            });

        }

        public bool CheckSameOwnerGroup(List<Guid> _listKey)
        {
            return this._Model.Engaged(this, delegate ()
            {
                var groupCount = from rows in this._Model.t_wms_outbound_master
                                 where _listKey.Contains(rows.outbound_order_master_id)
                                 group rows by new
                                 {
                                     rows.wh_master_id,
                                     rows.owner_id
                                 } into grb
                                 select new
                                 {
                                     grb.Key.wh_master_id,
                                     grb.Key.owner_id
                                 };

                var check = this._Model.Engaged(this, delegate ()
                {
                    if (groupCount.Count() == 1) return false;
                    else return true;
                }, "! Outbound Order Number must in same group Warehouse and Owner");


                return check;
            });
        }

        public bool CheckLoadOverStepOpen(Guid _wh_master_id, string _load_id)
        {
            return this._Model.Engaged(this, delegate ()
            {
                var check = this._Model.Engaged(this, delegate ()
                {
                    if (this._Model.t_wms_outbound_master.Where(wh => wh.wh_master_id == _wh_master_id && wh.load_id == _load_id).All(qry => qry.order_status == "OPEN"))
                        return false;
                    else
                        return true;
                }, "! Cannot use this Load ID, Because some orders used this Load ID has over status OPEN.");


                return check;
            });
        }

        public OutboundMasterDto GetDtoByKeyId(Guid _order_master_id)
        {
            return this._Model.GetDataEntityBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_outbound_master
                              where rows.outbound_order_master_id == _order_master_id

                              let detail_count = rows.t_wms_outbound_detail.Count()

                              select new OutboundMasterDto
                              {
                                  outbound_order_master_id = rows.outbound_order_master_id,
                                  outbound_order_number = rows.outbound_order_number,
                                  wh_master_id = rows.wh_master_id,
                                  wh_id = rows.t_wms_wh.wh_id,
                                  customer_name = rows.customer_code + " : " + rows.customer_name,
                                  owner_id = rows.owner_id,
                                  owner_code = rows.t_wms_owner.owner_code,
                                  order_status = rows.order_status,


                                  sum_qty_plan = detail_count > 0 ? rows.t_wms_outbound_detail.Sum(sum => sum.quantity_order) : 0,
                                  sum_qty_pick = detail_count > 0 ? rows.t_wms_outbound_detail.Sum(sum => sum.quantity_pick) : 0,

                                  carrier_id = rows.carrier_id

                              }).FirstOrDefault();

                var pick = (from rows in this._Model.t_wms_outbound_pick_master
                            where rows.outbound_order_master_id == result.outbound_order_master_id

                            let detail_count = rows.t_wms_outbound_pick_detail.Count()

                            select new
                            {
                                sum_qty_stage = detail_count > 0 ? rows.t_wms_outbound_pick_detail.Sum(sum => sum.quantity_stage) : 0,
                                sum_qty_load = detail_count > 0 ? rows.t_wms_outbound_pick_detail.Sum(sum => sum.quantity_load) : 0,
                                sum_qty_ship = detail_count > 0 ? rows.t_wms_outbound_pick_detail.Sum(sum => sum.quantity_ship) : 0,

                            }).FirstOrDefault();

                if (pick != null)
                {
                    result.sum_qty_stage = pick.sum_qty_stage;
                    result.sum_qty_load = pick.sum_qty_load;
                    result.sum_qty_ship = pick.sum_qty_ship;
                }

                return result;
            });
        }

        public string GetOrderNumberByKeyID(Guid Id)
        {
            return this._Model.GetDataEntityBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_outbound_master
                              where rows.outbound_order_master_id == Id
                              select rows.outbound_order_number).FirstOrDefault();

                return result;
            });
        }

        public object GetByOrderNumber(string order_number)
        {
            return this._Model.GetDataEntityBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_outbound_master
                              where rows.outbound_order_number == order_number
                              select rows).FirstOrDefault();

                return result;
            });
        }
        public void UpdateWarehouse(
        string appID
        , string deviceID
        , Guid whMasterId
        , string outboundOrderNumber
        , Guid toWhMasterId
        , Guid outboundOrderMasterId
        , string userID
        , ref string _errCode
        , ref string _errMsg)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_outbound_update_warehouse(appID
                , deviceID
                , whMasterId
                , outboundOrderNumber
                , toWhMasterId
                , outboundOrderMasterId
                , userID
                , errCode
                , errMsg);

            _errCode = errCode.Value.ToString();
            _errMsg = errMsg.Value.ToString();
        }
        #endregion
    }
}
