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
    public class InventoryBatchSummary : AGridObjectSourceQuery, IDisposable
    {
        public WMSEntities _Model { get; set; }

        public InventoryBatchSummary()
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

            var qry = from rows in this._Model.v_wms_inventory_data
                      where listWh.Contains(rows.wh_master_id)
                      && listOwn.Contains(rows.owner_id)

                      let _minDate = DbFunctions.CreateDateTime(0001, 1, 1, 0, 0, 0)
                      let _year = SqlFunctions.DatePart("year", (DbFunctions.Left(rows.expiry_date, 4).Trim()))
                      let _month = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(4, 2).Trim())
                      let _day = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(6, 2).Trim())
                      let expiry_date = !string.IsNullOrEmpty(rows.expiry_date)
                      ? SqlFunctions.DateAdd("day", _day - 1,
                            SqlFunctions.DateAdd("month", _month - 1,
                                SqlFunctions.DateAdd("year", _year - 1, _minDate)))
                      : min_date

                      select new
                      {
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
                          expiry_date = expiry_date == min_date ? null : expiry_date,
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
                          receipt_date = min_date,
                          rows.mfg_date,

                      };

            if (_active_load == null)
            {
                qry = qry.Where(wh => false);
            }

            qry = DataFilter.GetQuery(qry, this.FilterAuto);

            foreach (var fil in this.FilterAuto)
            {
                fil.IsFilter = false;
            }

            var result = from rows in qry
                         group rows
                         by new
                         {
                             rows.wh_id,
                             rows.owner_code,
                             rows.cate_description,
                             rows.item_number,
                             rows.description,
                             rows.lot_number,
                             rows.inv_status,
                             rows.uom
                         } into grp
                         select new
                         {
                             KeyId = "0",
                             grp.Key.wh_id,
                             grp.Key.owner_code,
                             grp.Key.cate_description,
                             grp.Key.item_number,
                             grp.Key.description,
                             grp.Key.lot_number,
                             grp.Key.inv_status,
                             quantity = grp.Sum(su => su.quantity),
                             quantity_allocated = grp.Sum(su => su.quantity_allocated),
                             grp.Key.uom

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
