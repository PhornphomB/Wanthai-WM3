using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

using WMS_NEW.Source;
using Prototype.Providers;

namespace WMS_NEW.Access.Viewer
{
    public class InventoryAdjust : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public InventoryAdjust()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static InventoryAdjust Instance
        {
            get
            {
                using (InventoryAdjust _Instance = new InventoryAdjust())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.v_wms_adjust_export.AsNoTracking()

                         join items in this._Model.t_wms_item
                         on rows.item_number equals items.item_number into _item
                         from item in _item.DefaultIfEmpty()

                         select new
                         {
                             KeyID = "0",
                             rows.owner_code,
                             rows.location,
                             rows.adjust_number,
                             wh_id = rows.wh_code,
                             rows.adjust_date_export,
                             rows.item_number,
                             item_desc = item.description,
                             rows.adjust_by,
                             rows.reason_code,
                             rows.reason_desc,
                             rows.adjust_date_stock,
                             rows.lot_number,
                             rows.serial_number,
                             rows.expiry_date,
                             rows.quantity,
                             rows.after_quantity,
                             rows.quantity_uom
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion
    }
}
