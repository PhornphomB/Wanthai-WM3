using Prototype.Providers;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData.Transportation
{
    public class Carrier : AEntityFormCommand<t_wms_carrier>
    {
        public WMSEntities _Model { get; set; }
        public Carrier()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_carrier; };

        }

        public override bool ValidateSaveNew(t_wms_carrier ent, ref string msg_validate)
        {
            if (_Model.t_wms_carrier.Any(x => x. carrier_code == ent.carrier_code))
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
            var result = from rows in this._Model.t_wms_carrier
                         select new
                         {
                             KeyId = rows.carrier_id,
                             rows.carrier_code,
                             rows.carrier_name,
                             rows.description,
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



        #region GetQuery Property

        public IQueryable<Property> GetQuery()
        {
            var result = from rows in _Model.t_wms_carrier
                         where rows.is_active == "YES"
                         orderby rows.carrier_code ascending
                         select new Property
                         {
                             guid_member = rows.carrier_id,
                             display_member = rows.carrier_code + " : " + rows.carrier_name
                         };

            return result;
        }

        #endregion
    }
}
