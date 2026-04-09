using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class Customer : AEntityFormCommand<t_wms_customer>
    {
        #region ++INSTANCE STATIC++
        public static Customer Instance
        {
            get
            {
                using (Customer _Instance = new Customer())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        protected WMSEntities _Model { get; set; }

        public Customer()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_customer; };
        }

        public override bool ValidateSaveNew(t_wms_customer ent, ref string msg_validate)
        {
            if (_Model.t_wms_customer.Any(x => x.owner_id == ent.owner_id && x.customer_code == ent.customer_code))
            {
                msg_validate = "! Customer Code and Owner has in system.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_customer
                         where rows.t_wms_owner.t_wms_owner_user.Any(a=>a.user_id == _SessionVals.UserName && a.is_active == "YES")
                         select new
                         {
                             KeyId = rows.customer_id,
                             rows.owner_id,
                             rows.t_wms_owner.owner_code,
                             rows.customer_code,
                             rows.customer_name,
                             rows.description,
                             rows.is_active,
                             rows.create_date,
                             rows.create_by,
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
                             //-----------------
                             rows.ref_customer_code,
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
            var result = from rows in _Model.t_wms_customer
                         where rows.is_active == "YES"
                         orderby rows.customer_code ascending
                         select new Property
                         {
                             guid_member = rows.customer_id,
                             display_member = rows.customer_code + " : " + rows.customer_name
                         };

            return result;
        }

        public IQueryable<Property> GetQuery(Guid _owner_id)
        {
            var result = from rows in _Model.t_wms_customer
                         where rows.is_active == "YES"
                         && rows.owner_id == _owner_id
                         orderby rows.customer_code ascending
                         select new Property
                         {
                             guid_member = rows.customer_id,
                             display_member = rows.customer_code + " : " + rows.customer_name
                         };

            return result;
        }

        public IQueryable<Property> GetQueryCode()
        {
            var result = from rows in _Model.t_wms_customer
                         where rows.is_active == "YES"
                         orderby rows.customer_code ascending
                         select new Property
                         {
                             value_member = rows.customer_code,
                             display_member = rows.customer_code + " : " + rows.customer_name
                         };

            return result;
        }

        public IQueryable<Property> GetQueryModernTrade(Guid _owner_id)
        {
            var result = from rows in _Model.t_wms_customer
                         where rows.is_active == "YES"
                         && rows.owner_id == _owner_id
                         && (rows.item_shelf_life == "YES")
                         orderby rows.customer_code ascending
                         select new Property
                         {
                             guid_member = rows.customer_id,
                             display_member = rows.customer_code + " : " + rows.customer_name
                         };

            return result;
        }

        #endregion
    }
}
