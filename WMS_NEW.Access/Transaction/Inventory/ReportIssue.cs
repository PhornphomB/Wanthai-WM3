using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Access.MasterData;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Inventory
{
    public class ReportIssue : AEntityFormCommand<t_wms_issue_record>
    {
        #region ++INSTANCE STATIC++
        public static ReportIssue Instance
        {
            get
            {
                using (ReportIssue _Instance = new ReportIssue())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        protected WMSEntities _Model { get; set; }

        public ReportIssue()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_issue_record; };
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_issue_record
                         join wh in this._Model.t_wms_wh on rows.wh_master_id equals wh.wh_master_id into wh_join
                         from wh in wh_join.DefaultIfEmpty()
                         select new
                         {
                             KeyId = rows.issue_record_id,
                             rows.wh_master_id,
                             wh.wh_id,
                             rows.tran_type,
                             rows.location,
                             rows.issue_record,
                             rows.lpn,
                             rows.remark,
                             rows.create_by,
                             rows.create_date,
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
