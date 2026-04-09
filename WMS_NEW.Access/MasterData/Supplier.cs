using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class Supplier : AEntityFormCommand<t_wms_supplier>
    {
        #region ++INSTANCE STATIC++
        public static Supplier Instance
        {
            get
            {
                using (Supplier _Instance = new Supplier())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        protected WMSEntities _Model { get; set; }

        public Supplier()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_supplier; };
        }

        public override bool ValidateSaveNew(t_wms_supplier ent, ref string msg_validate)
        {
            if (_Model.t_wms_supplier.Any(x => x.supplier_code == ent.supplier_code))
            {
                msg_validate = "! Supplier code has in system.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_supplier
                         where rows.t_wms_owner.t_wms_owner_user.Any(a => a.user_id == _SessionVals.UserName && a.is_active == "YES")
                         select new
                         {
                             KeyId = rows.supplier_id,
                             rows.t_wms_owner.owner_code,
                             rows.supplier_code,
                             rows.supplier_name,
                             rows.description,
                             rows.is_active,
                             rows.create_date,
                             rows.create_by,
                             rows.owner_id,

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
                             //-------------------
                             rows.user_def1,
                             rows.user_def2,
                             rows.user_def3,
                             rows.user_def4,
                             rows.user_def5,
                             rows.user_def6,
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
            var result = from rows in _Model.t_wms_supplier
                         where rows.is_active == "YES"
                         orderby rows.supplier_code ascending
                         select new Property
                         {
                             guid_member = rows.supplier_id,
                             display_member = rows.supplier_code + " : " + rows.supplier_name
                         };

            return result;
        }

        public IQueryable<Property> GetQuery(Guid _owner_id)
        {
            var result = from rows in _Model.t_wms_supplier
                         where rows.is_active == "YES"
                         && rows.owner_id == _owner_id
                         orderby rows.supplier_code ascending
                         select new Property
                         {
                             guid_member = rows.supplier_id,
                             display_member = rows.supplier_code + " : " + rows.supplier_name
                         };

            return result;
        }

        public IQueryable<Property> GetQueryCode()
        {
            var result = from rows in _Model.t_wms_supplier
                         where rows.is_active == "YES"
                         orderby rows.supplier_code ascending
                         select new Property
                         {
                             value_member = rows.supplier_code,
                             display_member = rows.supplier_code + " : " + rows.supplier_name
                         };

            return result;
        }

        #endregion


    }
}
