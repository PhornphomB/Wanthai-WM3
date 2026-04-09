using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Data.Entity;
using System.Data.Entity.SqlServer;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Viewer
{

    [Serializable()]
    public class OutboundDetailExport
    {
        public OutboundDetailExport()
        {
        }

        public Guid KeyID { get; set; }
        public string Warehouse { get; set; }
        public string Owner { get; set; }
        public string OutboundOrderNo { get; set; }
        public string OrderType { get; set; }
        public string OrderStatus { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Department { get; set; }
        public DateTime?
            Date
        { get; set; }

        public string ItemNumber { get; set; }
        public string ItemDesc { get; set; }


        public string Lot { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string SerialNumber { get; set; }


        public string StageItemStatus { get; set; }
        public string ItemGrade { get; set; }
        public double? Price { get; set; }
        public double? QtyOrder { get; set; }
        public double? QtyQhip { get; set; }
        public string UOM { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }

        public DateTime? ShipDate { get; set; }
    }


    public class OutboundDetail : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public OutboundDetail()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static OutboundDetail Instance
        {
            get
            {
                using (OutboundDetail _Instance = new OutboundDetail())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {

            DateTime min_date = new DateTime(1753, 1, 7);

            var _userID = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userID").Value;

            var ruleOrderTypes = this._Model.t_wms_rule
            .Where(rule => rule.rule_code == "RULE_ORDER_TYPE_INSERT_INBOUND" && rule.is_active == "YES")
            .Select(rule => rule.value)
            .ToList();

            var result = from pick_det in _Model.t_wms_outbound_pick_detail.AsNoTracking()

                         let rows = pick_det.t_wms_outbound_detail
                         let master = rows.t_wms_outbound_master
                         join wh in this._Model.t_wms_wh on master.department equals wh.wh_master_id.ToString() into whJoin
                         from wh in whJoin.DefaultIfEmpty()
                         let item = rows.t_wms_wh_item.t_wms_item

                         let _minDate = DbFunctions.CreateDateTime(0001, 1, 1, 0, 0, 0)
                         let _year = SqlFunctions.DatePart("year", (DbFunctions.Left(pick_det.expiry_date, 4).Trim()))
                         let _month = SqlFunctions.DatePart("year", "00" + pick_det.expiry_date.Substring(4, 2).Trim())
                         let _day = SqlFunctions.DatePart("year", "00" + pick_det.expiry_date.Substring(6, 2).Trim())
                         let expiry_date = !string.IsNullOrEmpty(pick_det.expiry_date)
                         ? SqlFunctions.DateAdd("day", _day - 1,
                               SqlFunctions.DateAdd("month", _month - 1,
                                   SqlFunctions.DateAdd("year", _year - 1, _minDate)))
                         : min_date

                         where master.t_wms_wh.t_wms_wh_user.Any(qry => qry.user_id == _userID)
                         select new
                         {
                             KeyID = rows.outbound_order_master_id,

                             master.t_wms_wh.wh_id,
                             master.t_wms_owner.owner_code,
                             master.outbound_order_number,
                             master.order_type,
                             master.order_status,
                             master.customer_code,
                             master.customer_name,
                             department = ruleOrderTypes.Contains(master.order_type) ? wh.wh_id : master.department,
                             master.ship_date_actual,
                             master.user_def1,

                             item.item_number,
                             rows.item_description,

                             pick_det.lot_number,
                             pick_det.mfg_date,
                             exp_date = expiry_date == min_date ? null : expiry_date,
                             pick_det.serial_number,
                             pick_det.lpn,

                             rows.staging_item_status,
                             item.grade,
                             rows.price,
                             rows.quantity_order,
                             pick_det.quantity_ship,
                             alter_quantity_order = rows.quantity_order/ rows.pack_size_conversion_factor ,
                             alter_quantity_ship = pick_det.quantity_ship / rows.pack_size_conversion_factor,
                             rows.pack_size_uom,
                             item.t_wms_item_uom.FirstOrDefault(qry => qry.primary_uom == "YES").uom,
                             rows.create_by,
                             rows.update_by,
                             rows.attribute1,
                             master.load_id
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
