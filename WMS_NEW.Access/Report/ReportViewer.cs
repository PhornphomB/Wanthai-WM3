using Prototype.Providers;
using System;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Report
{
    [Serializable]
    public class DTO_Report {
        public string DataFieldValue { get; set; }
        public object Value { get; set; }
    }
    public class ReportViewer : AGridObjectSourceQuery
    {
        #region ++INSTANCE STATIC++
        public static ReportViewer Instance
        {
            get
            {
                using (ReportViewer _Instance = new ReportViewer())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public WMSEntities _Model { get; set; }

        public ReportViewer()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            string form_name = string.Empty;
            if (this.FilterCustom != null)
            {
                var ent = this.FilterCustom.Where(w => w.DataFieldValue == "form_name").FirstOrDefault();
                if (ent != null && ent.Value != null)
                {
                    form_name = ent.Value.ToString().ToUpper();
                }
            }

            var result = from rows in this._Model.t_com_report_manager
                         where rows.is_active == "YES"
                         && rows.form_name.ToUpper() == form_name 
                         select new
                         {
                             KeyId = rows.report_id,
                             rows.app_id,
                             rows.form_name,
                             rows.report_name,
                             rows.report_seq,
                             rows.report_file_name,
                             rows.report_type,
                             rows.is_active,
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
