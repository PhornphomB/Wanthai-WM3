using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

using WMS_NEW.Source;
using Prototype.Providers;

namespace WMS_NEW.Access.Transaction.OutbndManifest
{
    public class ManifestTspOrderSrc : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public ManifestTspOrderSrc()
        {
            this._Model = new  WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _userID = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userID").Value;

            var exclude_stat = new[] { "CLOSE" };

            var result = from rows in this._Model.t_wms_outbound_master
                         where _Model.t_wms_wh_user.Any(qry => qry.wh_master_id == rows.wh_master_id && qry.user_id == _userID)
                               && !exclude_stat.Contains(rows.order_status)

                         let cus = rows.t_wms_customer

                         select new
                         {
                             KeyID = rows.outbound_order_master_id,
                             cmd_item = "",
                             rows.owner_code,
                             rows.owner_id,
                             rows.wh_id,
                             rows.outbound_order_number,
                             rows.order_type,
                             rows.order_date,
                             rows.order_status,
                             rows.customer_id,
                             cus.route_code,
                             cus.route_name,
                             cus.customer_code,
                             cus.customer_name,
                             rows.create_by,
                             rows.create_date,
                             rows.customer_purchase_order,
                             total_quantity = (double?)rows.t_wms_outbound_detail.Sum(s => s.quantity_order) ?? 0,
                             item_count = rows.t_wms_outbound_detail.Count(),
                             cus.province,
                             rows.customer_order_number
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
