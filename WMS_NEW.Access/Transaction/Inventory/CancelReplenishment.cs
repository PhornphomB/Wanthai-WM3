using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Inventory
{
    public class CancelReplenishment : AEntityFormCommand<t_wms_replenishment_task>
    {
        #region ++INSTANCE STATIC++
        public static CancelReplenishment Instance
        {
            get
            {
                using (CancelReplenishment _Instance = new CancelReplenishment())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        protected WMSEntities _Model { get; set; }

        public CancelReplenishment()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_replenishment_task; };
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_replenishment_task
                         select new
                         {
                             KeyId = rows.replenishment_task_id,
                             rows.wh_master_id,
                             rows.wh_id,
                             rows.owner_id,
                             rows.owner_code,
                             rows.source_location_id,
                             rows.source_location,
                             rows.destination_location_id,
                             rows.destination_location,
                             rows.lpn,
                             rows.wh_item_master_id,
                             rows.item_number,
                             rows.item_description,
                             rows.lot_number,
                             rows.mfg_date,
                             rows.expiry_date,
                             rows.attribute1,
                             rows.attribute2,
                             rows.attribute3,
                             rows.attribute4,
                             rows.attribute5,
                             rows.inventory_status_id,
                             rows.inventory_status,
                             rows.receive_date,
                             rows.is_replenish,
                             rows.inventory_id,
                             rows.attribute_group_id,
                             rows.quantity,
                             rows.uom,
                             rows.source_zone_id,
                             rows.source_zone,
                             rows.destination_zone_id,
                             rows.destination_zone,
                             rows.stg_location_id,
                             rows.stg_location,
                             rows.create_by,
                             rows.create_date
                         };

            var entFirstLoad = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_is_first_load");
            if (entFirstLoad != null && entFirstLoad.Value != null)
            {
                result = result.Where(x => x.is_replenish == "NO");
            }

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }
        

        #endregion
    }
}
