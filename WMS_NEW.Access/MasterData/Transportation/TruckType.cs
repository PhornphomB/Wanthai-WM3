using Prototype.Providers;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData.Transportation
{
    public class TruckType : AEntityFormCommand<t_wms_truck_type>
    {
        #region ++INSTANCE STATIC++
        public static TruckType Instance
        {
            get
            {
                using (TruckType _Instance = new TruckType())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public WMSEntities _Model { get; set; }

        public TruckType()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_truck_type; };

        }

        public override bool ValidateSaveNew(t_wms_truck_type ent, ref string msg_validate)
        {
            if (_Model.t_wms_truck_type.Any(x => x.truck_type == ent.truck_type))
            {
                msg_validate = "! TruckType name has in system.";
                return false;
            }
            else
                return true;
        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_truck_type
                         select new
                         {
                             KeyId = rows.truck_type_id,
                             rows.truck_type,
                             rows.wheel,
                             rows.volume,
                             rows.weight,
                             rows.length,
                             rows.width,
                             rows.height,
                             rows.factor,
                             rows.is_active,
                             rows.create_by,
                             rows.create_date,
                             rows.update_by,
                             rows.update_date,
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
            var result = from rows in _Model.t_wms_truck_type
                         where rows.is_active == "YES"
                         orderby rows.truck_type ascending
                         select new Property
                         {
                             guid_member = rows.truck_type_id,
                             display_member = rows.truck_type
                         };

            return result;
        }

        #endregion
    }
}
