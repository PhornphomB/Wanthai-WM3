using Prototype.Providers;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Administrator
{
    public class InterfaceLog : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public InterfaceLog()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
        }
        public static InterfaceLog Instance
        {
            get
            {
                using (InterfaceLog _Instance = new InterfaceLog())
                {
                    return _Instance;
                }
            }
        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_interface_process_log_execute
                         select new
                         {
                             KeyID = rows.inf_log_id,
                             rows.process,
                             rows.host_record_id,
                             rows.data_number,
                             rows.error_code,
                             rows.error_msg,
                             rows.data_1,
                             rows.data_2,
                             rows.data_3,
                             rows.data_4,
                             rows.create_date,
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
