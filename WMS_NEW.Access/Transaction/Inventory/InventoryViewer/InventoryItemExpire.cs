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
    public class InventoryItemExpire : AGridObjectSourceQuery, IDisposable
    {
        public WMSEntities _Model { get; set; }

        public InventoryItemExpire()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static InventoryBatchSummary Instance
        {
            get
            {
                using (InventoryBatchSummary _Instance = new InventoryBatchSummary())
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

            DateTime min_date = new DateTime(1753, 1, 7);
            var listWh = this._Model.t_wms_wh_user.Where(w => w.user_id == _SessionVals.UserName).Select(s => s.wh_master_id);
            var listOwn = this._Model.t_wms_owner_user.Where(w => w.user_id == _SessionVals.UserName).Select(s => s.owner_id);

            var result = from rows in this._Model.v_wms_inventory_data.AsNoTracking()
                         where rows.days_to_expire <= 0
                         && listWh.Contains(rows.wh_master_id)
                         && listOwn.Contains(rows.owner_id)
                         

                         select new
                         {
                             KeyId = rows.inventory_id,
                             rows.wh_master_id,
                             rows.wh_id,
                             rows.owner_code,
                             rows.zone,
                             rows.location,
                             rows.item_number,
                             rows.description,
                             rows.lpn,
                             rows.parent_lpn,
                             rows.inv_status,
                             rows.lot_number,
                             expiry_date = rows.exp_date,
                             rows.quantity,
                             rows.quantity_allocated,
                             rows.uom,
                             rows.dg_code,
                             rows.serial_number,
                             rows.grade,
                             rows.item_category,
                             rows.price,
                             rows.receive_date,
                             rows.item_master_id,
                             rows.category_id,
                             rows.cate_description,
                             rows.owner_id,
                             rows.days_to_expire,
                             //#ทำเป็น Temp หลอก เพื่อให้ทุกหน้าจอ ค้นหาได้
                             delivery_date = min_date,
                             delivery_number = "",
                             receipt_date = DbFunctions.TruncateTime(rows.receive_date),
                             //#Custom
                             //rows.alternate_item_number
                             rows.mfg_date,

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
