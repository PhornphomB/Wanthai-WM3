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
    public class InboundDetail : AGridObjectSourceQuery
    {

        public WMSEntities _Model { get; set; }

        public InboundDetail()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        public object GetByKeyID(object Id)
        {
            var realId = Id.ToString();

            return this._Model.GetDataEntityBy<t_host_wms_inbound_order_detail>(this, delegate()
            {
                var result = (from rows in this._Model.t_host_wms_inbound_order_detail
                              where rows.host_record_id == realId
                              select rows).FirstOrDefault();

                return result;
            });

        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var host_record_id = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "host_record_id").Value;

            var result = from rows in this._Model.t_host_wms_inbound_order_detail
                         where rows.host_record_id == host_record_id
                         select new
                         {
                             KeyID = rows.inbound_order_detail_id,
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
                             rows.default_item_status,
                             rows.over_receipt_percentage,
                             rows.supplier_item_number,
                             rows.lpn,
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
