using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WMS_NEW.Source;
using Prototype.Providers;

namespace WMS_NEW.Access.Administrator
{
    public class ProcessLog : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public ProcessLog()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
        }
        public static ProcessLog Instance
        {
            get
            {
                using (ProcessLog _Instance = new ProcessLog())
                {
                    return _Instance;
                }
            }
        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_com_process_log
                         select new
                         {
                             KeyID = rows.process_id,
                             rows.app_id,
                             rows.log_type,
                             rows.warehouse,
                             rows.device,
                             rows.process,
                             rows.process_datetime,
                             rows.data_1,
                             rows.data_2,
                             rows.data_3,
                             rows.data_4,
                             rows.message,
                             rows.create_date,
                             rows.create_by,
                             rows.rowversion,
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
















