using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData.Mapping
{
    public class ZoneLocation : AEntityFormCommand<t_wms_zone_location>
    {
        protected WMSEntities _Model { get; set; }

        public ZoneLocation()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_zone_location; };
        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.v_wms_mapping_zone_location
                         select new
                         {
                             KeyId = rows.key_id,
                             rows.wh_master_id,
                             rows.wh_id,
                             rows.zone,
                             rows.zone_id,
                             rows.location,
                             rows.location_id,
                             rows.is_active,
                             map_status = rows.is_active.Value ? "YES" : "NO",
                             rows.create_date,
                             rows.create_by
                         };

            if (this.FilterAuto != null && this.FilterAuto.Count > 0)
            {
                var entZone = this.FilterAuto.Where(w => w.DataPropertyName == "zone_id").FirstOrDefault();
                if (entZone != null)
                {
                    var zone_id = (Guid)entZone.Value; 
                    result = result.Where(w => !this._Model.t_wms_zone_location.Any(a => a.zone_id != zone_id && a.location_id == w.location_id));
                    return result;

                }
            }

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion

        #region Function

       // public bool SaveMapping(List<ConfigGlobal.DTO._Global.KeySelect> _listKey, string _create_by, Guid _wh_master_id)
        public bool SaveMapping(List<ConfigGlobal.DTO._Global.KeySelect> _listKey, string _create_by)
        {
            try
            {
                Guid select_zone = Guid.Empty;
                if (_listKey.Count > 0)
                {
                    select_zone = Guid.Parse(_listKey.FirstOrDefault().KeyId.ToString().Split('_')[0]);
                }
                var _queryHasInDB = this._Model.t_wms_zone_location.Where(rows => rows.t_wms_zone.zone_id == select_zone).Select(s => s.t_wms_zone.zone_id + "_" + s.t_wms_location.location_id);

                var listSelect = _listKey.Where(w => w.Active == true).Select(s => s.KeyId.ToString()).ToList();

                List<string> listUnselect = new List<string>();
                listUnselect = _listKey.Where(w => w.Active == false).Select(s => s.KeyId.ToString()).ToList();


                //Insert
                var listExcept = listSelect.Except(_queryHasInDB).AsEnumerable();
                var result = from rows in this._Model.v_wms_mapping_zone_location
                             where listExcept.Contains(rows.key_id)
                             //&& rows.wh_master_id == _wh_master_id
                             select rows;

                //Insert
                t_wms_zone_location entIns;
                foreach (var item in result)
                {
                    entIns = new t_wms_zone_location();
                    entIns.zone_location_id = Guid.NewGuid();
                    entIns.zone_id = item.zone_id.Value;
                    entIns.location_id = item.location_id;
                    entIns.is_active = "YES";
                    entIns.create_by = _create_by;
                    entIns.create_date = DateTime.Now;

                    this._Model.t_wms_zone_location.Add(entIns);
                }

                //Delete
                if (listUnselect.Count > 0)
                {
                    var entDel = from rows in this._Model.t_wms_zone_location
                                 where listUnselect.Contains(rows.t_wms_zone.zone_id + "_" + rows.t_wms_location.location_id)
                                 select rows;

                    this._Model.t_wms_zone_location.RemoveRange(entDel);
                }

                return this.GridObjContext.Save(this, delegate ()
                {
                    return _Model.SaveChanges();
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
