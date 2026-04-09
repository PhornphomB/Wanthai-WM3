using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Core.Objects;

using WMS_NEW.Source;
using Prototype.Providers;

namespace WMS_NEW.Access.Transaction.OutbndManifest
{
    public class ManifestTspOrderItemSrc : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public ManifestTspOrderItemSrc()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _outbound_order_master_id = (Guid)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_outbound_order_master_id").Value;

            DateTime min_date = new DateTime(1753, 1, 7);
            var result = from rows in this._Model.v_wms_outbound_detail_linenumber
                         where rows.outbound_order_master_id == _outbound_order_master_id

                         let _minDate = EntityFunctions.CreateDateTime(0001, 1, 1, 0, 0, 0)
                         let _year = SqlFunctions.DatePart("year", (EntityFunctions.Left(rows.expiry_date, 4).Trim()))
                         let _month = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(4, 2).Trim())
                         let _day = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(6, 2).Trim())
                         let expiry_date = !string.IsNullOrEmpty(rows.expiry_date)
                         ? SqlFunctions.DateAdd("day", _day - 1,
                               SqlFunctions.DateAdd("month", _month - 1,
                                   SqlFunctions.DateAdd("year", _year - 1, _minDate)))
                         : min_date

                         let quantity_tsp = _Model.t_wms_outbound_manifest_tsp_order_de.Where(x => x.outbound_order_detail_id == rows.outbound_order_detail_id).Sum(sm => sm.item_quantity) ?? 0

                         select new
                         {
                             KeyID = rows.outbound_order_detail_id,
                             rows.line_number,
                             rows.line_number_int,
                             rows.default_item_status,
                             rows.item_number,
                             rows.item_description,
                             rows.bom_id,
                             rows.bom_master,
                             rows.item_category,
                             rows.category_description,
                             rows.lot_number,
                             rows.lpn,
                             expiry_date = expiry_date == min_date ? null : expiry_date,
                             rows.grade,
                             rows.price,
                             rows.quantity_order,
                             quantity_tsp,
                             quantity_avalible = (rows.quantity_order - quantity_tsp),
                             rows.uom,
                             rows.create_by,
                             rows.create_date,
                             rows.attribute1,
                             rows.attribute2,
                             rows.attribute3,
                             rows.attribute4,
                             rows.attribute5,
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
