using Prototype.Providers;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Authentication
{
    public class User : AEntityFormCommand<v_wms_user>
    {
        public WMSEntities _Model { get; set; }
        public User()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.v_wms_user; };

        }

        public override bool ValidateSaveNew(v_wms_user ent, ref string msg_validate)
        {
            if (_Model.v_wms_user.Any(x => x.user_id == ent.user_id))
            {
                msg_validate = "! User name has in system.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.v_wms_user
                         select new
                         {
                             KeyId = "",
                             rows.user_id,
                             rows.name,
                             rows.first_name,
                             rows.last_name
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion



        #region GetQuery Property

        public IQueryable<Property> GetQuery()
        {
            var result = from rows in _Model.v_wms_user
                         where rows.app_id == _SessionVals.AppID
                         orderby rows.user_id ascending
                         select new Property
                         {
                             value_member = rows.user_id,
                             display_member = rows.user_id + ":" + rows.name
                         };

            return result;
        }

        #endregion
    }
}
