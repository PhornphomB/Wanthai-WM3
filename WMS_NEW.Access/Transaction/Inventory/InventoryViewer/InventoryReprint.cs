using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Inventory.InventoryViewer
{
    public class InventoryReprint : AGridObjectSourceQuery, IDisposable
    {
        public WMSEntities _Model { get; set; }

        public InventoryReprint()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static InventoryReprint Instance
        {
            get
            {
                using (InventoryReprint _Instance = new InventoryReprint())
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

            var result = from rows in this._Model.v_wms_inventory_data_reprint
                         where listWh.Contains(rows.wh_master_id)
                         && listOwn.Contains(rows.owner_id)
                         select new
                         {
                             KeyId = rows.inventory_id,
                             rows.attribute_group_id,
                             rows.inv_type,
                             rows.wh_id,
                             rows.wh_master_id,
                             rows.owner_id,
                             rows.owner_code,
                             rows.zone_id,
                             rows.zone,
                             rows.location_id,
                             rows.location,
                             rows.parent_lpn,
                             rows.lpn,
                             rows.category_id,
                             rows.item_category,
                             rows.cate_description,
                             rows.wh_item_master_id,
                             rows.item_master_id,
                             rows.item_number,
                             rows.description,
                             rows.item_uom_id,
                             rows.uom_prompt,
                             rows.uom,
                             rows.quantity,
                             rows.quantity_allocated,
                             quantity_avalible = rows.quantity - rows.quantity_allocated,
                             rows.inv_status,
                             rows.inventory_status_id,
                             rows.receive_date,
                             rows.receive_date_filter,
                             rows.lot_number,
                             rows.mfg_date,
                             rows.expiry_date,
                             rows.exp_date,
                             rows.attribute1,
                             rows.qr_format,
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
