using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class Reason : AEntityFormCommand<t_wms_reason>
    {
        #region ++INSTANCE STATIC++
        public static Reason Instance
        {
            get
            {
                using (Reason _Instance = new Reason())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        protected WMSEntities _Model { get; set; }

        public Reason()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_reason; };
        }

        public override bool ValidateSaveNew(t_wms_reason ent, ref string msg_validate)
        {
            if (_Model.t_wms_reason.Any(x => x.reason_code == ent.reason_code))
            {
                msg_validate = "! Reason has in system.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_reason

                         join rType in this._Model.t_com_combobox_item
                         on rows.reason_type equals rType.value_member into RTYPE
                         from leftType in RTYPE.DefaultIfEmpty()

                         where leftType.group_name == "mst_reason_type"
                         select new
                         {
                             KeyId = rows.reason_id,
                             rows.reason_code,
                             rows.short_description,
                             rows.description,
                             rows.reason_type,
                             leftType.display_member,
                             rows.is_active,
                             rows.create_date,
                             rows.create_by,
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
            var result = from rows in _Model.t_wms_reason
                         where rows.is_active == "YES"
                         orderby rows.reason_code ascending
                         select new Property
                         {
                             guid_member = rows.reason_id,
                             display_member = rows.reason_code + ":" + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQuery(string _reason_type)
        {
            var result = from rows in _Model.t_wms_reason
                         where rows.is_active == "YES"
                         && rows.reason_type.ToUpper() == _reason_type.ToUpper()
                         orderby rows.reason_code ascending
                         select new Property
                         {
                             guid_member = rows.reason_id,
                             display_member = rows.reason_code + ":" + rows.description
                         };

            return result;
        }


        public IQueryable<Property> GetQueryCode()
        {
            var result = from rows in _Model.t_wms_reason
                         where rows.is_active == "YES"
                         orderby rows.reason_code ascending
                         select new Property
                         {
                             value_member = rows.reason_code,
                             display_member = rows.reason_code + ":" + rows.description
                         };

            return result;
        }
        #endregion

    }
}
