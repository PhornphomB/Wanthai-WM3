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
    public class ManifestTspAssDTO
    {
        public string manifest_code { get; set; }
        public string license_plate { get; set; }
        public string truck_name { get; set; }
    }


    public class ManifestTspAssign : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        #region ++INSTANCE STATIC++
        public static ManifestTspAssign Instance
        {
            get
            {
                using (ManifestTspAssign _Instance = new ManifestTspAssign())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public ManifestTspAssign()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        public ManifestTspAssDTO GetEntityDto(Guid _tsp_truck_id)
        {
            return this._Model.GetDataEntityBy(this, delegate ()
            {
                var dto = (from rows in this._Model.t_wms_outbound_manifest_tsp_truck
                           join truck in _Model.t_wms_truck on rows.truck_id equals truck.truck_id
                           where rows.tsp_truck_id == _tsp_truck_id
                           group rows by new
                           {
                               rows.manifest_code,
                               truck.license_plate,
                               truck.truck_name
                           } into grb
                           let rows = grb.Key
                           select new ManifestTspAssDTO
                           {
                               manifest_code = rows.manifest_code,
                               license_plate = rows.license_plate,
                               truck_name = rows.truck_name
                           }).FirstOrDefault();

                return dto;
            });
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_outbound_manifest_tsp_order_de
                         join truck in _Model.t_wms_truck on rows.t_wms_outbound_manifest_tsp_truck.truck_id equals truck.truck_id
                         where rows.session_id == string.Empty
                         group rows by new
                         {
                             rows.tsp_truck_id,
                             rows.owner_code,
                             rows.wh_id,
                             rows.manifest_code,
                             truck.license_plate,
                             truck.truck_name
                         } into grb
                         let rows = grb.Key
                         select new
                         {
                             KeyID = rows.tsp_truck_id,
                             cmd_report = "",
                             rows.owner_code,
                             rows.wh_id,
                             rows.manifest_code,
                             rows.license_plate,
                             rows.truck_name,
                             volume_total = grb.Sum(sm => sm.volume_total)
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
