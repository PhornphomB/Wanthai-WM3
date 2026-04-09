using Prototype.Providers;
using System;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.DashBoard
{
    public class WarehouseVirtual : AGridObjectSourceQuery,IDisposable
    {
        public WMSEntities _Model { get; set; }

        public WarehouseVirtual()
        {
            _Model = new WMSEntities();
        }


        public override IQueryable<dynamic> InitialQueryView()
        {
            var wh_master_id = (Guid)this.FilterCustom.Where(w => w.DataFieldValue == "_wh_master_id").FirstOrDefault().Value;
            var location_level = FilterCustom.Where(w => w.DataFieldValue == "_location_level").FirstOrDefault()?.Value?.ToString() ?? string.Empty;

            var result = from rows in this._Model.v_wms_dashboard_warehouse_virtual
                         where rows.wh_master_id == wh_master_id
                         && rows.location_level == location_level
                         select new
                         {

                             KeyId = "",
                             rows.zone,
                             rows.zone_id,
                             rows.location_id,
                             rows.location,
                             rows.capacity_qty,
                             rows.current_qty,
                             rows.available_qty,
                             rows.usage_qty,
                             rows.wh_master_id,
                             rows.location_level
                         };
            return result;
        }

        public override IQueryable<dynamic> InitialQueryExport()
        {
            return InitialQueryView();
        }
    }

    public class WarehouseVirtualDetail : AGridObjectSourceQuery
    {
        protected WMSEntities _Model { get; set; }

        public WarehouseVirtualDetail()
        {
            _Model = new WMSEntities();
        }


        public override IQueryable<dynamic> InitialQueryView()
        {
            Guid location_id = Guid.Empty;
            var entLoc = this.FilterCustom.Where(w => w.DataFieldValue == "_location_id").FirstOrDefault();
            if (entLoc != null && entLoc.Value != null)
            {
                location_id = (Guid)entLoc.Value;
            }

            var result = from rows in this._Model.v_wms_inventory_data_by_serial_item
                         where rows.location_id == location_id
                         select new
                         {

                             KeyId = "",
                             rows.item_master_id,
                             rows.item_number,
                             rows.description,
                             rows.lot_number,
                             rows.expiry_date,
                             rows.serial_number,
                             rows.quantity,
                         };
            return result;
        }

        public override IQueryable<dynamic> InitialQueryExport()
        {
            return InitialQueryView();
        }
    }

}
