using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Viewer
{
    public class LabelPrinted : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public LabelPrinted()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static LabelPrinted Instance
        {
            get
            {
                using (LabelPrinted _Instance = new LabelPrinted())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            string user_id = _SessionVals.UserName;
            
            var result = from rows in this._Model.v_wms_label_printed.AsNoTracking()
                         join wh in _Model.t_wms_wh on rows.wh_master_id equals wh.wh_master_id into leftwh
                         from wh in leftwh.DefaultIfEmpty()
                         join owner in _Model.t_wms_owner on rows.owner_id equals owner.owner_id into leftowner
                         from owner in leftowner.DefaultIfEmpty()
                         where wh.t_wms_wh_user.Any(qry => qry.user_id == user_id)
                         && owner.t_wms_owner_user.Any(a => a.user_id == user_id && a.is_active == "YES")
                         select rows;
            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


    }
}
