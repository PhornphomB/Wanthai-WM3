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
    public class ShipDetail : AGridObjectSourceQuery
    {

        public WMSEntities _Model { get; set; }

        public ShipDetail()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var host_shipment_id = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "host_record_id").Value;

            var result = from rows in this._Model.t_host_wms_ship_conf_detail_export
                         where rows.host_shipment_id == host_shipment_id
                         select new
                         {
                             KeyID = rows.host_shipment_detail_id,
                             rows.host_shipment_id,
                             rows.processing_code,
                             rows.processing_status,
                             rows.wh_id,
                             rows.owner_id,
                             rows.order_number,
                             rows.line_number,
                             rows.item_number,
                             rows.location,
                             rows.lot_number,
                             rows.expiry_date,
                             rows.serial_number,
                             rows.uom,
                             rows.order_quantity,
                             rows.quantity,
                             rows.short_quantity,
                             rows.status,
                             rows.reason_code,
                             rows.lpn,
                             rows.user_id,
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
                             rows.create_date,
                             rows.create_by,
                             rows.mfg_date,
                             rows.attribute1,
                             rows.receipt_date
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
