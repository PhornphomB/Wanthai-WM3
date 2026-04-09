using Prototype.Providers;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class Owner : AEntityFormCommand<t_wms_owner>
    {
        #region ++INSTANCE STATIC++
        public static Owner Instance
        {
            get
            {
                using (Owner _Instance = new Owner())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        protected WMSEntities _Model { get; set; }

        public Owner()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_owner; };
        }

        public override bool ValidateSaveNew(t_wms_owner ent, ref string msg_validate)
        {
            if (_Model.t_wms_owner.Any(x => x.owner_code == ent.owner_code))
            {
                msg_validate = "! Owner code has in system.";
                return false;
            } else if (_Model.t_wms_owner.Any(x => x.owner_name == ent.owner_name)) {
                msg_validate = "! Owner Name has in system.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_owner
                         select new
                         {
                             KeyId = rows.owner_id,
                             rows.owner_code,
                             rows.owner_name,
                             rows.owner_number,
                             rows.description,
                             rows.is_active,
                             rows.create_by,
                             rows.create_date,
                             //--------------
                             rows.addr_line_1,
                             rows.addr_line_2,
                             rows.addr_line_3,
                             rows.city,
                             rows.province,
                             rows.postal_code,
                             rows.country_code,
                             rows.country_name,
                             rows.phone,
                             rows.fax,
                             rows.email,
                             rows.contact,
                         };
            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion

        #region Qurey Propperty
        public IQueryable<Property> GetQuery()
        {
            var result = from rows in _Model.t_wms_owner
                         where rows.is_active == "YES"
                         orderby rows.owner_code ascending
                         select new Property
                         {
                             guid_member = rows.owner_id,
                             display_member = rows.owner_code + " : " + rows.owner_name
                         };

            return result;
        }

        public IQueryable<Property> GetQuery(string _user)
        {
            var result = from rows in _Model.t_wms_owner_user
                         where rows.is_active == "YES"
                         && rows.user_id.ToUpper() == _user.ToUpper()
                         orderby rows.t_wms_owner.owner_code ascending
                         select new Property
                         {
                             guid_member = rows.owner_id,
                             display_member = rows.t_wms_owner.owner_code + " : " + rows.t_wms_owner.owner_name
                         };

            return result;
        }

        public IQueryable<Property> GetQuery_User()
        {
            return GetQuery(_SessionVals.UserName);
        }

        public IQueryable<Property> GetQueryCode()
        {
            var result = from rows in _Model.t_wms_owner
                         where rows.is_active == "YES"
                         orderby rows.owner_code ascending
                         select new Property
                         {
                             value_member = rows.owner_code,
                             display_member = rows.owner_code + " : " + rows.owner_name
                         };

            return result;
        }

        public IQueryable<Property> GetQueryCode(string _user)
        {
            var result = from rows in _Model.t_wms_owner_user
                         where rows.is_active == "YES"
                         && rows.user_id.ToUpper() == _user.ToUpper()
                         orderby rows.t_wms_owner.owner_code ascending
                         select new Property
                         {
                             value_member = rows.t_wms_owner.owner_code,
                             display_member = rows.t_wms_owner.owner_code + " : " + rows.t_wms_owner.owner_name
                         };

            return result;
        }

        #endregion
    }
}
