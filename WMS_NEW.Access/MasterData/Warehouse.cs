using Prototype.Providers;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class Warehouse : AEntityFormCommand<t_wms_wh>
    {
        #region ++INSTANCE STATIC++
        public static Warehouse Instance
        {
            get
            {
                using (Warehouse _Instance = new Warehouse())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public WMSEntities _Model { get; set; }

        public Warehouse()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_wh; };
        }

        public override bool ValidateSaveNew(t_wms_wh ent, ref string msg_validate)
        {
            if (_Model.t_wms_wh.Any(x => x.wh_id == ent.wh_id))
            {
                msg_validate = "! Wh Id has in system.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_wh
                         select new
                         {
                             KeyId = rows.wh_master_id,
                             rows.wh_id,
                             rows.wh_name,
                             rows.description,
                             rows.wh_group,
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
        //public IQueryable<Property> GetQuery()
        //{
        //    var result = from rows in _Model.t_wms_wh
        //                 where rows.is_active == "YES"
        //                 orderby rows.wh_id ascending
        //                 select new Property
        //                 {
        //                     guid_member = rows.wh_master_id,
        //                     display_member = rows.wh_id + ":" + rows.wh_name
        //                 };

        //    return result;
        //}

        public IQueryable<Property> GetQueryForDBoard_User(string _user)
        {
            var result = from rows in _Model.t_wms_wh_user
                         where rows.is_active == "YES"
                         && rows.user_id.ToUpper() == _user.ToUpper()
                         orderby rows.t_wms_wh.wh_id ascending
                         select new Property
                         {
                             guid_member = rows.wh_master_id,
                             display_member = "WAREHOUSE : " + rows.t_wms_wh.wh_name
                         };

            return result;
        }

        public IQueryable<Property> GetQuery_User(string _user)
        {
            var result = from rows in _Model.t_wms_wh_user
                         where rows.is_active == "YES"
                         && rows.user_id.ToUpper() == _user.ToUpper()
                         orderby rows.t_wms_wh.wh_id ascending
                         select new Property
                         {
                             guid_member = rows.wh_master_id,
                             display_member = rows.t_wms_wh.wh_id + ":" + rows.t_wms_wh.wh_name
                         };

            return result;
        }

        public IQueryable<Property> GetQuery_User()
        {
            return GetQuery_User(_SessionVals.UserName);
        }

        public IQueryable<Property> GetQueryCode()
        {
            var result = from rows in _Model.t_wms_wh
                         where rows.is_active == "YES"
                         orderby rows.wh_id ascending
                         select new Property
                         {
                             value_member = rows.wh_id,
                             display_member = rows.wh_id + ":" + rows.wh_name
                         };

            return result;
        }

        public IQueryable<Property> GetQueryCode(string _user)
        {
            var result = from rows in _Model.t_wms_wh_user
                         where rows.is_active == "YES"
                         && rows.user_id.ToUpper() == _user.ToUpper()
                         orderby rows.t_wms_wh.wh_id ascending
                         select new Property
                         {
                             value_member = rows.t_wms_wh.wh_id,
                             display_member = rows.t_wms_wh.wh_id + ":" + rows.t_wms_wh.wh_name
                         };

            return result;
        }

        #endregion

    }
}
