using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

using WMS_NEW.Source;
using Prototype.Providers;
using System.Globalization;

namespace WMS_NEW.Access.Administrator.InterfaceMonitors
{
    public class OutboundDetail : AGridObjectSourceQuery
    {

        public WMSEntities _Model { get; set; }

        public OutboundDetail()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var host_record_id = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "host_record_id").Value;

            var result = from rows in this._Model.t_host_wms_outbound_order_detail
                         where rows.host_record_id == host_record_id
                         select new
                         {
                             KeyID = rows.outbound_order_detail_id,
                             rows.host_record_id,
                             rows.record_type,
                             rows.processing_code,
                             rows.processing_status,
                             rows.wh_id,
                             rows.owner_id,
                             rows.order_number,
                             rows.line_number,
                             rows.item_number,
                             rows.order_quantity,
                             rows.uom,
                             rows.lot_number,
                             rows.expiry_date,
                             rows.serial_number,
                             rows.item_description,
                             rows.customer_item_code,
                             rows.dangerous_good_flag,
                             rows.lpn,
                             rows.grade,
                             rows.default_item_status,
                             rows.is_used,
                             rows.price,
                             rows.user_def1,
                             rows.user_def2,
                             rows.user_def3,
                             rows.user_def4,
                             rows.user_def5,
                             rows.user_def6,
                             rows.user_def7,
                             rows.user_def8,
                             rows.user_def9,
                             rows.user_def10,
                             rows.insert_date,
                             rows.create_date,
                             rows.create_by,
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
