using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Count
{
    public class CountReconcildPallet : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public CountReconcildPallet()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            Guid _count_master_id = Guid.Empty;
            var count_master_id = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_count_master_id");
            if(count_master_id != null && count_master_id.Value != null)
            {
                _count_master_id = (Guid)count_master_id.Value;
            }
            DateTime min_date = new DateTime(1753, 1, 7);

            var result = from rows in this._Model.v_wms_count_reconcile_pallet
                         where rows.count_master_id == _count_master_id
                         select new
                         {
                             KeyId = rows.count_master_id,
                             rows.count_master_id,
                             rows.location_id,
                             rows.location,
                             rows.zone,
                             rows.zone_id,
                             rows.stock_pallet_qty,
                             rows.count_pallet_qty,
                             rows.different,
                             rows.item_number,
                             rows.description,
                             rows.remark1,
                             rows.remark2,
                             rows.create_date
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
