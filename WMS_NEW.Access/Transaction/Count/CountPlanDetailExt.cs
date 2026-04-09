using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Data.Entity;
using System.Data.Entity.SqlServer;

using Prototype.Providers;
using WMS_NEW.Source;
using System.Data.Entity.Core.Objects;

namespace WMS_NEW.Access.Transaction.Count
{

    public class CountPlanDetailExt : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public CountPlanDetailExt()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _count_master_id = (Guid)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_count_master_id").Value;
            DateTime min_date = new DateTime(1753, 1, 7);

            var result = from rows in this._Model.v_wms_count_plan_detail_external

                         let _minDate = DbFunctions.CreateDateTime(0001, 1, 1, 0, 0, 0)
                         let _year = SqlFunctions.DatePart("year", (DbFunctions.Left(rows.expiry_date, 4).Trim()))
                         let _month = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(4, 2).Trim())
                         let _day = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(6, 2).Trim())
                         let expiry_date = !string.IsNullOrEmpty(rows.expiry_date)
                         ? SqlFunctions.DateAdd("day", _day - 1,
                               SqlFunctions.DateAdd("month", _month - 1,
                                   SqlFunctions.DateAdd("year", _year - 1, _minDate)))
                         : min_date

                         where rows.count_master_id == _count_master_id
                         select new
                         {
                             KeyId = "",
                             rows.zone,
                             rows.location,
                             rows.item_category,
                             rows.cate_description,
                             rows.item_number,
                             rows.description,
                             rows.parent_lpn,
                             rows.owner_code,
                             lot = rows.lot_number,
                             expiry_date = expiry_date == min_date ? null : expiry_date,
                             rows.serial_number,
                             rows.attribute1,
                             rows.attribute2,
                             rows.attribute3,
                             rows.attribute4,
                             rows.attribute5,
                             rows.stock_qty,
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion
    }

    public class CountPlanReconcilExt : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public CountPlanReconcilExt()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _count_master_id = (Guid)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_count_master_id").Value;
            var _view_opt = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_view_opt").Value;

            var result = from rows in this._Model.v_wms_count_reconcile_external
                         where rows.count_master_id == _count_master_id
                         select new
                         {
                             KeyId = "",
                             rows.item_number,
                             rows.description,
                             rows.inv_status,
                             rows.grade,
                             rows.price,
                             rows.zone,
                             rows.location,
                             rows.parent_lpn,
                             rows.lpn,
                             rows.lot,
                             rows.expiry_date,
                             rows.stock_qty,
                             rows.count_qty,
                             rows.diff_qty,
                             rows.uom_prompt,
                             rows.create_by,
                             rows.create_date,
                             rows.item_category,
                             rows.cate_description,
                             rows.serial_number,
                             rows.attribute1,
                             rows.attribute2,
                             rows.attribute3,
                             rows.attribute4,
                             rows.attribute5
                         };

            if (_view_opt == "EQ")
            {
                result = result.Where(qry => qry.stock_qty == qry.count_qty);
            }
            else if (_view_opt == "NOT_EQ")
            {
                result = result.Where(qry => qry.stock_qty != qry.count_qty);
            }

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion
    }

    public class CountDetailAndReconcilExt : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public CountDetailAndReconcilExt()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _count_master_id = (Guid)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_count_master_id").Value;
            DateTime min_date = new DateTime(1753, 1, 7);

            var result = from rows in this._Model.v_wms_count_reconcile_merge_external

                         let _minDate = DbFunctions.CreateDateTime(0001, 1, 1, 0, 0, 0)
                         let _year = SqlFunctions.DatePart("year", (DbFunctions.Left(rows.expiry_date, 4).Trim()))
                         let _month = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(4, 2).Trim())
                         let _day = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(6, 2).Trim())
                         let expiry_date = !string.IsNullOrEmpty(rows.expiry_date)
                         ? SqlFunctions.DateAdd("day", _day - 1,
                               SqlFunctions.DateAdd("month", _month - 1,
                                   SqlFunctions.DateAdd("year", _year - 1, _minDate)))
                         : min_date

                         where rows.count_master_id == _count_master_id
                         select new
                         {
                             KeyId = "",
                             rows.owner_code,
                             rows.item_number,
                             rows.description,
                             rows.inv_status,
                             rows.grade,
                             rows.price,
                             rows.zone,
                             rows.location,
                             rows.parent_lpn,
                             rows.lpn,
                             lot = rows.lot_number,
                             expiry_date = expiry_date == min_date ? null : expiry_date,
                             rows.stock_qty,
                             rows.count_qty,
                             rows.diff_qty,
                             rows.uom_prompt,
                             rows.item_category,
                             rows.cate_description,
                             rows.category_id,
                             rows.serial_number,
                             rows.attribute1,
                             rows.attribute2,
                             rows.attribute3,
                             rows.attribute4,
                             rows.attribute5

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
