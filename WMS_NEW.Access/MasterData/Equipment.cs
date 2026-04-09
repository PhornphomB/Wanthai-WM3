using Prototype.Providers;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class Equipment : AEntityFormCommand<t_wms_equipment>
    {
        public WMSEntities _Model { get; set; }
        public Equipment()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_equipment; };

        }

        public override bool ValidateSaveNew(t_wms_equipment ent, ref string msg_validate)
        {
            if (_Model.t_wms_equipment.Any(x => x.equipment_code == ent.equipment_code))
            {
                msg_validate = "! Code has in system.";
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

            var result = from rows in this._Model.t_wms_equipment

                         join eType in this._Model.t_com_combobox_item
                         on rows.equipment_type equals eType.value_member into EType
                         from leftType in EType.DefaultIfEmpty()

                         where leftType.group_name == "mst_equipment_type"
                         && rows.t_wms_wh.t_wms_wh_user.Any(qry => qry.user_id.ToUpper().Trim() == user_id)
                         select new
                         {
                             KeyId = rows.equipment_type_id,
                             rows.wh_master_id,
                             rows.t_wms_wh.wh_id,
                             rows.equipment_code,
                             rows.description,
                             rows.equipment_type,
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



        #region GetQuery Property

        public IQueryable<Property> GetQuery()
        {
            var result = from rows in _Model.t_wms_equipment
                         where rows.is_active == "YES"
                         orderby rows.equipment_code ascending
                         select new Property
                         {
                             guid_member = rows.equipment_type_id,
                             display_member = rows.equipment_code + " : " + rows.description
                         };

            return result;
        }

        #endregion
    }
}
