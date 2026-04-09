using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Data.Entity;
using System.Data.Entity.SqlServer;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Outbound
{
    public class OutboundPickSerialDetail : AGridObjectSourceQuery, IDisposable
    {
        public WMSEntities _Model { get; set; }

        public OutboundPickSerialDetail()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;
            this._Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            DateTime min_date = new DateTime(1753, 1, 7);

            var result = from rows in this._Model.v_wms_outbound_pick_detail_mini

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
                             KeyId = "",
                             rows.item_number,
                             rows.description,
                             rows.location,
                             rows.pick_location,
                             rows.lot_number,
                             expiry_date = expiry_date == min_date ? null : expiry_date,
                             rows.serial_number,
                             rows.lpn,
                             rows.pick_to_lpn,
                             rows.parent_lpn,
                             rows.pick_to_parent_lpn,
                             rows.quantity_plan,
                             rows.quantity_pick,
                             rows.quantity_stage,
                             rows.quantity_load,
                             rows.quantity_ship,
                             rows.outbound_order_master_id
                         };


            if (FilterCustom == null)
            {
                result = result.Where(x => false);
            }
            else
            {
                var _outbound_order_master_id = (Guid)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_outbound_order_master_id").Value;

                result = result.Where(x => x.outbound_order_master_id == _outbound_order_master_id);
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
