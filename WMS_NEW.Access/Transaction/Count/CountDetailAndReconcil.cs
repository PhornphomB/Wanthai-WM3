using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Count
{
    public class CountDetailAndReconcil : AGridObjectSourceStore
    {
        public WMSEntities _Model { get; set; }

        public CountDetailAndReconcil()
        {
            this._Model = new WMSEntities();
            //AGridObjectSourceQuery Map Model
            this._Model.Database.CommandTimeout = 1200;

            base.GridObjContext = _Model;
        }

        #region ++INSTANCE STATIC++
        public static CountDetailAndReconcil Instance
        {
            get
            {
                using (CountDetailAndReconcil _Instance = new CountDetailAndReconcil())
                {
                    return _Instance;
                }
            }
        }
        #endregion
        #region Inherit AGridObjectSourceQuery

        public override void InitialStoreView()
        {
            var _count_master_id = (Guid?)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_count_master_id")?.Value;
            var _active_load = this.FilterCustom.FirstOrDefault(wh => wh.DataFieldValue == "_active_load");
            if (_active_load != null)
            {
                if (_active_load.Value == null && _count_master_id != null)
                {
                    this.StoreNameQuery = "usp_wms_count_reconcile_merge_by_master";
                    this.StoreParameterQuery.Add(new System.Data.SqlClient.SqlParameter("@in_uniqCountMasterId", _count_master_id));
                }
            }
        }

        public override void InitialStoreExport()
        {
            var _count_master_id = (Guid?)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_count_master_id")?.Value;
            var _active_load = this.FilterCustom.FirstOrDefault(wh => wh.DataFieldValue == "_active_load");
            if (_active_load != null)
            {
                if (_active_load.Value == null && _count_master_id != null)
                {
                    this.StoreNameExport = "usp_wms_count_reconcile_merge_by_master";
                    this.StoreParameterExport.Add(new System.Data.SqlClient.SqlParameter("@in_uniqCountMasterId", _count_master_id));
                }
            }
        }

        #endregion
    }
}
