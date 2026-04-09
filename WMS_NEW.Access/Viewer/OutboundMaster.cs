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
    public class OutboundMasterExport
    {
        public OutboundMasterExport()
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

        public string ShipToCode { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAddress { get; set; }
        public DateTime? ShipDate { get; set; }

        public double? TotalQtyOrder { get; set; }
        public double? TotalQtyShip { get; set; }
    }

    public class OutboundMaster : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public OutboundMaster()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static OutboundMaster Instance
        {
            get
            {
                using (OutboundMaster _Instance = new OutboundMaster())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {

            var _userID = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userID").Value;

            var ruleOrderTypes = this._Model.t_wms_rule
            .Where(rule => rule.rule_code == "RULE_ORDER_TYPE_INSERT_INBOUND" && rule.is_active == "YES")
            .Select(rule => rule.value)
            .ToList();

            var result = from rows in this._Model.t_wms_outbound_master.AsNoTracking()
                         join wh in this._Model.t_wms_wh on rows.department equals wh.wh_master_id.ToString() into whJoin
                         from wh in whJoin.DefaultIfEmpty()
                         where rows.t_wms_wh.t_wms_wh_user.Any(qry => qry.user_id == _userID)
                         select new
                         {
                             KeyID = rows.outbound_order_master_id,
                             rows.t_wms_wh.wh_id,
                             rows.t_wms_owner.owner_code,
                             rows.outbound_order_number,
                             rows.order_type,
                             rows.order_status,
                             rows.customer_code,
                             rows.customer_name,
                             //rows.department,
                             department = ruleOrderTypes.Contains(rows.order_type) ? wh.wh_id : rows.department,
                             rows.ship_to_code,
                             rows.ship_to_name,
                             rows.ship_to_addr_line_1,
                             rows.ship_date_actual,
                             qty_order = rows.t_wms_outbound_detail.Any() ? rows.t_wms_outbound_detail.Sum(sm => sm.quantity_order) : 0d,
                             qty_ship = rows.t_wms_outbound_detail.Any() ? rows.t_wms_outbound_detail.Sum(sm => sm.quantity_ship) : 0d
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
