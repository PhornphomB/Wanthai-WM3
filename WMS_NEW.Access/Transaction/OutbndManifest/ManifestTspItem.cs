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
    public class ManifestTspItem : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public ManifestTspItem()
        {
            this._Model = new  WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static ManifestTspItem Instance
        {
            get
            {
                using (ManifestTspItem _Instance = new ManifestTspItem())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _sessionID = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_sessionID").Value;

            var result = from rows in this._Model.t_wms_outbound_manifest_tsp_order_de
                         where rows.session_id == _sessionID
                         select new
                         {
                             cmd_split = "",
                             KeyID = rows.tsp_order_detail_id,
                             rows.owner_code,
                             rows.wh_id,
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


        public double? GetVolumeTotal(string _session_id, Guid[] _keys)
        {
            return this._Model.GetDataEntityBy<double?>(this, delegate ()
            {
                double? result = 0;

                if (_keys.Count() > 0)
                {
                    result = _Model.t_wms_outbound_manifest_tsp_order_de.Where(x => x.session_id == _session_id && _keys.Contains(x.tsp_order_detail_id)).Sum(sm => sm.volume_total);
                }
                else
                {
                    result = _Model.t_wms_outbound_manifest_tsp_order_de.Where(x => x.session_id == _session_id).Sum(sm => sm.volume_total);
                }

                return result;
            });
        }

    }
}
