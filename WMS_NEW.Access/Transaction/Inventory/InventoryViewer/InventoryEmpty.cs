using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Data.Entity;
using System.Data.Entity.SqlServer;

using Prototype.Providers;
using WMS_NEW.Source;


namespace WMS_NEW.Access.Transaction.Inventory.InventoryViewer
{
    public class InventoryEmpty : AGridObjectSourceQuery,IDisposable
    {
        public WMSEntities _Model { get; set; }

        public InventoryEmpty()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static InventoryEmpty Instance
        {
            get
            {
                using (InventoryEmpty _Instance = new InventoryEmpty())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _active_load = this.FilterCustom.FirstOrDefault(wh => wh.DataFieldValue == "_active_load");
            var listWh = this._Model.t_wms_wh_user.Where(w => w.user_id == _SessionVals.UserName).Select(s => s.wh_master_id);
            var listOwn = this._Model.t_wms_owner_user.Where(w => w.user_id == _SessionVals.UserName).Select(s => s.owner_id);

            var result = from rows in this._Model.v_wms_inventory_empty
                         where listWh.Contains(rows.wh_master_id)
                         && listOwn.Contains(rows.owner_id)

                         select new
                         {
                             KeyId = rows.item_master_id,
                             rows.item_master_id,
                             rows.item_number,
                             rows.wh_master_id,
                             rows.wh_id,
                             rows.owner_id,
                             rows.owner_code,
                             rows.description,
                             rows.qty,
                             rows.category_id,
                             rows.item_category,
                             rows.item_uom_id,
                             rows.uom,
                             rows.is_active,
                             rows.create_by,
                             rows.create_date,
                             rows.update_by,
                             rows.update_date,

                         };


            if (_active_load == null)
            {
                result = result.Where(wh => false);
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
