using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData.Mapping
{
    public class LocationCategory : AEntityFormCommand<t_wms_location_category>
    {
        protected WMSEntities _Model { get; set; }

        public LocationCategory()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_location_category; };
        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.v_wms_mapping_location_category
                         select new
                         {
                             KeyId = rows.key_id,
                             rows.wh_id,
                             rows.wh_master_id,

                             //rows.category_id,
                             rows.item_category,
                             rows.location,
                             rows.location_id,
                             rows.is_active,
                             map_status = rows.is_active.Value ? "YES" : "NO",

                             rows.create_date,
                             rows.create_by
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion

        #region Function

        public bool SaveMapping(List<ConfigGlobal.DTO._Global.KeySelect> _listKey, string _create_by)
        {
            try
            {

                Guid select_location = Guid.Empty;
                if (_listKey.Count > 0)
                {
                    select_location = Guid.Parse(_listKey.FirstOrDefault().KeyId.ToString().Split('_')[0]);
                }
                var _queryHasInDB = this._Model.t_wms_location_category.Where(rows => rows.t_wms_location.location_id == select_location).Select(s => s. t_wms_location.location + "_" + s.t_wms_category.category_id);

                var listSelect = _listKey.Where(w => w.Active == true).Select(s => s.KeyId.ToString()).ToList();

                List<string> listUnselect = new List<string>();
                listUnselect = _listKey.Where(w => w.Active == false).Select(s => s.KeyId.ToString()).ToList();


                //Insert
                var listExcept = listSelect.Except(_queryHasInDB).AsEnumerable();
                var result = from rows in this._Model.v_wms_mapping_location_category
                             where listExcept.Contains(rows.key_id)
                             select rows; 

                //Insert
                t_wms_location_category entIns;
                foreach (var item in result)
                {
                    entIns = new t_wms_location_category();
                    entIns.category_location_id = Guid.NewGuid();
                    entIns.category_id = item.category_id;
                    entIns.location_id = item.location_id;
                    entIns.is_active = "YES";
                    entIns.create_by = _create_by;
                    entIns.create_date = DateTime.Now;

                    this._Model.t_wms_location_category.Add(entIns);
                }

                //Delete
                if (listUnselect.Count > 0)
                {
                    var entDel = from rows in this._Model.t_wms_location_category
                                 where listUnselect.Contains(rows.t_wms_location.location_id + "_" + rows.t_wms_category.category_id)
                                 select rows;

                    this._Model.t_wms_location_category.RemoveRange(entDel);
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
