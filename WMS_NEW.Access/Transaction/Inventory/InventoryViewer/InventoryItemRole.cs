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
    public class InventoryItemRole : AGridObjectSourceQuery
    {

        public WMSEntities _Model { get; set; }

        public InventoryItemRole()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _active_load = this.FilterCustom.FirstOrDefault(wh => wh.DataFieldValue == "_active_load");
            DateTime? min_date = null;// new DateTime(1753, 1, 7);
            var listWh = this._Model.t_wms_wh_user.Where(w => w.user_id == _SessionVals.UserName).Select(s => s.wh_master_id);
            var listOwn = this._Model.t_wms_owner_user.Where(w => w.user_id == _SessionVals.UserName).Select(s => s.owner_id);

            var result = from rows in this._Model.v_wms_inventory_data
                         where rows.inv_status == "Available"
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
                             //rows.quantity,
                             quantity = rows.quantity - rows.quantity_allocated,
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
                             rows.alternate_item_number,
                             rows.mfg_date,

                         };


            if (_active_load == null)
            {
                result = result.Where(wh => false);
            }

            result = DataFilter.GetQuery(result, this.FilterAuto);
            foreach (var fil in this.FilterAuto)
            {
                fil.IsFilter = false;
            }

            var qry = from rows in result
                      group rows by new
                      {
                          rows.wh_master_id,
                          rows.wh_id,
                          rows.owner_id,
                          rows.owner_code,
                          rows.item_master_id,
                          rows.item_number,
                          rows.description,
                          rows.uom,
                          rows.inv_status
                      } into g
                      select new
                      {
                          KeyId = "0",
                          g.Key.wh_id,
                          g.Key.owner_code,
                          g.Key.item_master_id,
                          g.Key.item_number,
                          g.Key.description,
                          quantity = g.Sum(s => s.quantity),
                          g.Key.uom,
                          g.Key.inv_status
                      };

            qry = from rows in qry
                  where this._Model.t_wms_item.Any(x => x.item_master_id == rows.item_master_id && rows.quantity <= (x.min_qty ?? 0))
                  select rows;


            return qry;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


    }
}
