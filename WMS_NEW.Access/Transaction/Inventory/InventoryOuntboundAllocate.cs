using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Inventory
{
    public class InventoryOuntboundAllocate : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public InventoryOuntboundAllocate()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        public override IQueryable<dynamic> InitialQueryExport()
        {
            return InitialQueryView();
        }

        public override IQueryable<dynamic> InitialQueryView()
        {
            var listWh = this._Model.t_wms_wh_user.Where(w => w.user_id == _SessionVals.UserName).Select(s => s.wh_master_id);
            var listOwn = this._Model.t_wms_owner_user.Where(w => w.user_id == _SessionVals.UserName).Select(s => s.owner_id);

            var _result = from _row in this._Model.v_wms_outbound_allocate
                          where listWh.Contains(_row.wh_master_id) && listOwn.Contains(_row.owner_id)
                          select new
                          {                    
                          KeyId = _row.inventory_id
                                 ,_row.attribute_group_id
                                 ,_row.inv_type
                                 ,_row.wh_id
                                 ,_row.wh_master_id
                                 ,_row.owner_id
                                 ,_row.owner_code
                                 ,_row.location_id
                                 ,_row.parent_lpn
                                 ,_row.lpn
                                 ,_row.wh_item_master_id
                                 ,_row.item_number
                                 ,_row.description
                                 ,_row.uom_prompt
                                 ,_row.uom
                                 ,_row.serial_number
                                 ,_row.sn_control
                                 ,_row.quantity
                                 ,_row.quantity_allocated
                                 ,_row.quantity_incoming
                                 ,_row.receive_date
                                 ,_row.inv_status
                                 ,_row.inventory_status_id
                                 ,_row.location
                                 ,_row.loc_type
                                 ,_row.pick_sequence
                                 ,_row.pick_area_id
                                 ,_row.alternate_item_number
                                 ,_row.lot_number
                                 ,_row.expiry_date
                                 ,_row.zone
                                 ,_row.category_id
                                 ,_row.item_category
                                 ,_row.cate_description
                                 ,_row.receive_date_filter
                                 ,_row.dg_code
                                 ,_row.grade
                                 ,_row.price
                                 ,_row.Checked
                                 ,_row.adjust_qty
                                 ,_row.item_master_id
                                 ,_row.lpn_controlled
                                 ,_row.cost
                                 ,_row.attribute1
                                 ,_row.attribute2
                                 ,_row.attribute3
                                 ,_row.attribute4
                                 ,_row.attribute5
                                 ,_row.days_to_expire
                                 ,_row.create_date
                                 ,_row.create_by
                                 ,_row.order_number_desc
                          };

                return _result;
        }
    }
}
