using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData.Mapping
{
    public class UserOwner : AEntityFormCommand<t_wms_owner_user>
    {
        protected WMSEntities _Model { get; set; }

        public UserOwner()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_owner_user; };
        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.v_wms_mapping_user_owner
                         select new
                         {
                             KeyId = rows.key_id,
                             rows.user_id,
                             rows.owner_code,
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
                string select_user_id = string.Empty;
                if (_listKey.Count > 0)
                {
                    select_user_id = _listKey.FirstOrDefault().KeyId.ToString().Split('_')[0];
                }
                var _queryHasInDB = this._Model.t_wms_owner_user.Where(rows => rows.user_id == select_user_id).Select(s => s.user_id + "_" + s.t_wms_owner.owner_code);

                var listSelect = _listKey.Where(w => w.Active == true).Select(s => s.KeyId.ToString()).ToList();

                List<string> listUnselect = new List<string>();
                listUnselect = _listKey.Where(w => w.Active == false).Select(s => s.KeyId.ToString()).ToList();


                //Insert
                var listExcept = listSelect.Except(_queryHasInDB).AsEnumerable();
                var result = from rows in this._Model.v_wms_mapping_user_owner
                             where listExcept.Contains(rows.key_id)
                             select rows;

                //Insert
                t_wms_owner_user entIns;
                foreach (var item in result)
                {
                    entIns = new t_wms_owner_user();
                    entIns.owner_user_id = Guid.NewGuid();
                    entIns.owner_id = item.owner_id;
                    entIns.user_id = item.user_id;
                    entIns.is_active = "YES";
                    entIns.create_by = _create_by;
                    entIns.create_date = DateTime.Now;

                    this._Model.t_wms_owner_user.Add(entIns);
                }

                //Delete
                if (listUnselect.Count > 0)
                {
                    var entDel = from rows in this._Model.t_wms_owner_user
                                 where listUnselect.Contains(rows.user_id + "_" + rows.t_wms_owner.owner_code)
                                 select rows;

                    this._Model.t_wms_owner_user.RemoveRange(entDel);
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
