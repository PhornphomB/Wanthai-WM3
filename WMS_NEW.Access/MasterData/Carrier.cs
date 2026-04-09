using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class Carrier : AEntityFormCommand<t_wms_carrier>
    {
        #region ++INSTANCE STATIC++
        public static Carrier Instance
        {
            get
            {
                using (Carrier _Instance = new Carrier())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        protected WMSEntities _Model { get; set; }

        public Carrier()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_carrier; };
        }

        public override bool ValidateSaveNew(t_wms_carrier ent, ref string msg_validate)
        {
            if (_Model.t_wms_carrier.Any(x => x.carrier_code == ent.carrier_code))
            {
                msg_validate = "! Carrier Code has in system.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_carrier
                         select rows;


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
            var result = from rows in _Model.t_wms_carrier
                         where rows.is_active == "YES"
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
