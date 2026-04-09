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
    public class ManifestTspAssignDetail : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public ManifestTspAssignDetail()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _tspTruckID = Guid.Parse(this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_tspTruckID").Value.ToString());

            var result = from rows in this._Model.t_wms_outbound_manifest_tsp_order_de
                         where rows.tsp_truck_id == _tspTruckID
                         select new
                         {
                             KeyID = rows.tsp_order_detail_id,
                             province = rows.cus_province,
                             rows.outbound_order_number,
                             rows.line_number,
                             rows.item_number,
                             rows.item_quantity,
                             rows.volume_per,
                             rows.volume_total,
                             rows.uom,
                             rows.create_by,
                             rows.create_date
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
