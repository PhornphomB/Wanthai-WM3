using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Outbound
{
    public class InventoryItemOrder : AGridObjectSourceQuery
    {
        #region ++INSTANCE STATIC++
        public static InventoryItemOrder Instance
        {
            get
            {
                using (InventoryItemOrder _Instance = new InventoryItemOrder())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public WMSEntities _Model { get; set; }

        public InventoryItemOrder()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _userID = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userID").Value;
            var _delivery_date = this.FilterAuto.FirstOrDefault(qry => qry.DataPropertyName == "delivery_date_plan");      
            if (_delivery_date == null)
            {
                var wh_master_ids = (from rows in this._Model.t_wms_wh
                                     where rows.t_wms_wh_user.Any(qry => qry.user_id == _userID)
                                     select rows.wh_master_id).ToList();

                var _list_wh_master_id = wh_master_ids.Select(s => new Guid?(s)).ToList();

                var result = from rows in this._Model.v_outbound_viewer_sum_qty_by_item_no_delivery_date
                             where _list_wh_master_id.Contains(rows.wh_master_id) && _Model.t_wms_owner_user.Any(a => a.user_id == _SessionVals.UserName && a.is_active == "YES" && a.owner_id == rows.owner_id)
                             select new
                             {
                                 KeyID = rows.wh_item_master_id,
                                 rows.category_id,
                                 delivery_date_plan = "",
                                 rows.item_category,
                                 rows.item_description,
                                 rows.item_number,
                                 rows.owner_code,
                                 rows.owner_id,
                                 rows.quantity_order,
                                 rows.uom,
                                 rows.wh_master_id,
                             };
 

                return result;
            }
            else
            {

                var wh_master_ids = (from rows in this._Model.t_wms_wh
                                     where rows.t_wms_wh_user.Any(qry => qry.user_id == _userID)
                                     select rows.wh_master_id).ToList();

                var _list_wh_master_id = wh_master_ids.Select(s => new Guid?(s)).ToList();

                var result = from rows in this._Model.v_outbound_viewer_sum_qty_by_item
                             where _list_wh_master_id.Contains(rows.wh_master_id) 
                             select new
                             {
                                 KeyID = rows.wh_item_master_id,
                                 rows.category_id,
                                 rows.delivery_date_plan,
                                 rows.item_category,
                                 rows.item_description,
                                 rows.item_number,
                                 rows.owner_code,
                                 rows.owner_id,
                                 rows.quantity_order,
                                 rows.uom,
                                 rows.wh_master_id,
                             };

                return result;
            }
         
        }

        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }
    }

    public class InventoryItemInvent : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public InventoryItemInvent()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _userID = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userID").Value;

            var wh_master_ids = (from rows in this._Model.t_wms_wh
                                 where rows.t_wms_wh_user.Any(qry => qry.user_id == _userID)
                                 select rows.wh_master_id).ToList();

            var _list_wh_master_id = wh_master_ids.Select(s => new Guid?(s)).ToList();

            var result = from rows in this._Model.v_outbound_inv_viewer_sum_qty_item_diff
                       where _list_wh_master_id.Contains(rows.wh_master_id) && _Model.t_wms_owner_user.Any(a => a.user_id == _SessionVals.UserName && a.is_active == "YES" && a.owner_id == rows.owner_id)
                         select new
                         {
                             KeyID = rows.wh_item_master_id,
                             rows.item_number,
                             rows.item_description,
                             rows.item_category,
                             rows.category_id,
                             rows.owner_id,
                             rows.owner_code,
                             rows.uom,
                             rows.sumInv,
                             rows.quantity_diff,
                             rows.wh_master_id
                         };

            //return result.GroupBy(w => new {
            //    w.KeyID,
            //    w.category_id,
            //    //w.delivery_date_plan,
            //    w.item_category,
            //    w.item_description,
            //    w.item_number,
            //    w.owner_code,
            //    w.owner_id,
            //    //w.quantity_order,
            //    w.uom,
            //    w.wh_master_id
            //}).Select(g => new {
            //    g.Key.KeyID,
            //    g.Key.category_id,            
            //    g.Key.item_category,
            //    g.Key.item_description,
            //    g.Key.item_number,
            //    g.Key.owner_code,
            //    g.Key.owner_id,
            //    sumInv = g.Sum(c => c.sumInv),
            //    quantity_diff = g.Sum(c => c.quantity_diff),
            //    g.Key.uom,
            //    g.Key.wh_master_id
            //});

           return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion
    }

    public class InventoryItemOrderDetail : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public InventoryItemOrderDetail()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _wh_item_master_id = Guid.Parse((string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_wh_item_master_id").Value);
            
            var result = from rows in this._Model.t_wms_outbound_detail

                         join uom in this._Model.t_wms_item_uom on rows.item_uom_id equals uom.item_uom_id

                         let master = rows.t_wms_outbound_master

                         where master.order_status == "OPEN" && rows.wh_item_master_id == _wh_item_master_id
                         select new
                         {
                             KeyID = "0",
                             master.outbound_order_number,
                             rows.line_number,
                             rows.t_wms_wh_item.t_wms_item.item_number,
                             rows.quantity_order,
                             uom.uom,
                             rows.t_wms_wh_item.t_wms_item.category_id,
                             master.customer_purchase_order,
                             master.customer_code,
                             master.customer_name,
                             master.t_wms_owner.owner_code,
                             master.order_type,
                             ship_to = master.ship_to_code + " : " + master.ship_to_name,
                             master.delivery_date_plan
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion
    }

    public class InventoryItemInventDetail : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public InventoryItemInventDetail()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _wh_item_master_id = Guid.Parse((string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_wh_item_master_id").Value);

            var result = from rows in this._Model.v_wms_inventory_data_for_inv_compare
                         where rows.wh_item_master_id == _wh_item_master_id
                         select new
                         {
                             KeyID = "0",
                             rows.item_number,
                             rows.description,
                             rows.location,
                             rows.lpn,
                             rows.lot_number,
                             rows.expiry_date,
                             rows.quantity,
                             rows.uom,
                             rows.category_id
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
