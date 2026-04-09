using Prototype.Providers;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData.Transportation
{
    public class Truck : AEntityFormCommand<t_wms_truck>
    {
        public WMSEntities _Model { get; set; }
        public Truck()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_truck; };

        }

        public override bool ValidateSaveNew(t_wms_truck ent, ref string msg_validate)
        {
            if (_Model.t_wms_truck.Any(x => x.truck_name == ent.truck_name))
            {
                msg_validate = "! Truck name has in system.";
                return false;
            }
            else
                return true;
        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_truck
                         select new
                         {
                             KeyId = rows.truck_id,
                             rows.truck_type_id,
                             rows.t_wms_truck_type.truck_type,
                             rows.carrier_id,
                             rows.t_wms_carrier.carrier_code,
                             rows.t_wms_carrier.carrier_name,
                             rows.truck_name,
                             rows.license_plate,
                             rows.driver_name1,
                             rows.id_card_driver1,
                             rows.driver_name2,
                             rows.id_card_driver2,
                             rows.driver_name3,
                             rows.id_card_driver3,
                             rows.brand,
                             rows.model,
                             rows.cost_per_km,
                             rows.create_by,
                             rows.create_date,
                             rows.update_by,
                             rows.update_date
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
            var result = from rows in _Model.t_wms_truck
                         orderby rows.truck_name ascending
                         select new Property
                         {
                             guid_member = rows.truck_id,
                             display_member = rows.truck_name
                         };

            return result;
        }

        #endregion
    }
}
