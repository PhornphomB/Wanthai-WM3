using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class Zone : AEntityFormCommand<t_wms_zone>
    {
        #region ++INSTANCE STATIC++
        public static Zone Instance
        {
            get
            {
                using (Zone _Instance = new Zone())
                {
                    return _Instance;
                }
            }
        }
        #endregion
        protected WMSEntities _Model { get; set; }

        public Zone()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_zone; };
        }

        public override bool ValidateSaveNew(t_wms_zone ent, ref string msg_validate)
        {
            if (_Model.t_wms_zone.Any(x => x.zone == ent.zone && x.wh_master_id == ent.wh_master_id))
            {
                msg_validate = "! Zone and warehouse has in system.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            string user_id = string.Empty;
            if (this.FilterCustom != null)
            {
                var entU = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "user_id");
                if (entU != null)
                {
                    user_id = entU.Value.ToString().ToUpper().Trim();
                }
            }


            var result = from rows in this._Model.t_wms_zone
                         where rows.t_wms_wh.t_wms_wh_user.Any(a => a.user_id.ToUpper().Trim() == user_id)
                         select new
                         {
                             KeyId = rows.zone_id,
                             rows.t_wms_wh.wh_id,
                             rows.wh_master_id,
                             rows.zone,
                             rows.description,
                             rows.zone_type,
                             rows.is_active,
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


        #region Query Property
        public IQueryable<Property> GetQuery()
        {
            var result = from rows in _Model.t_wms_zone
                         where rows.is_active == "YES"
                         orderby rows.zone ascending
                         select new Property
                         {
                             guid_member = rows.zone_id,
                             display_member = rows.zone + ":" + rows.description
                         };

            return result;
        }
        public IQueryable<Property> GetQuery_Warehouse(Guid _wh_master_id)
        {
            var result = from rows in _Model.t_wms_zone
                         where rows.is_active == "YES"
                         && rows.wh_master_id == _wh_master_id
                         orderby rows.zone ascending
                         select new Property
                         {
                             guid_member = rows.zone_id,
                             display_member = rows.zone + ":" + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQuery_Warehouse(Guid[] _list_wh_master_id)
        {
            var result = from rows in _Model.t_wms_zone
                         where rows.is_active == "YES"
                         && _list_wh_master_id.Contains(rows.wh_master_id)
                         orderby rows.zone ascending
                         select new Property
                         {
                             guid_member = rows.zone_id,
                             display_member = rows.zone + ":" + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQueryCode_Warehouse(Guid[] _list_wh_master_id)
        {
            var result = from rows in _Model.t_wms_zone
                         where rows.is_active == "YES"
                         && _list_wh_master_id.Contains(rows.wh_master_id)
                         orderby rows.zone ascending
                         select new Property
                         {
                             value_member = rows.zone,
                             display_member = rows.zone + ":" + rows.description
                         };

            return result.Distinct();
        }

        public IQueryable<Property> GetQuery_UserWarehouse()
        {
            string user_id = _SessionVals.UserName;
            Guid[] listWh = this._Model.t_wms_wh_user.Where(w => w.user_id == user_id).Select(s => s.wh_master_id).ToArray();

            return GetQuery_Warehouse(listWh);
        }

        public IQueryable<Property> GetQueryCode_UserWarehouse()
        {
            string user_id = _SessionVals.UserName;
            Guid[] listWh = this._Model.t_wms_wh_user.Where(w => w.user_id == user_id).Select(s => s.wh_master_id).ToArray();

            return GetQueryCode_Warehouse(listWh);
        }


        public IQueryable<Property> GetQueryCode()
        {
            var result = from rows in _Model.t_wms_zone
                         where rows.is_active == "YES"
                         orderby rows.zone ascending
                         select new Property
                         {
                             value_member = rows.zone,
                             display_member = rows.zone + ":" + rows.description
                         };

            return result;
        }


        public IQueryable<Property> GetQueryCode(Guid _wh_master_id)
        {
            var result = from rows in _Model.t_wms_zone
                         where rows.is_active == "YES"
                         && rows.wh_master_id == _wh_master_id
                         orderby rows.zone ascending
                         select new Property
                         {
                             value_member = rows.zone,
                             display_member = rows.zone + ":" + rows.description
                         };

            return result;
        }


        #endregion

    }
}
