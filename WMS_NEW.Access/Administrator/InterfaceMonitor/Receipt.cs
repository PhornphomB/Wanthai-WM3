using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

using WMS_NEW.Source;
using Prototype.Providers;
using System.Globalization;

namespace WMS_NEW.Access.Administrator.InterfaceMonitor
{
    public class Receipt : AGridObjectSourceQuery
    {

        public WMSEntities _Model { get; set; }

        public Receipt()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static Receipt Instance
        {
            get
            {
                using (Receipt _Instance = new Receipt())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public t_host_wms_receipt_conf_header_export GetByKeyID(object Id)
        {
            var realId = Id.ToString();

            //Inbound Order
            return this._Model.GetDataEntityBy<t_host_wms_receipt_conf_header_export>(this, delegate ()
            {
                var result = (from rows in this._Model.t_host_wms_receipt_conf_header_export
                              where rows.receipt_id == realId
                              select rows).FirstOrDefault();

                return result;
            });

        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            bool isFirstLoad = this.FilterCustom
                                  .FirstOrDefault(qry => qry.DataFieldValue == "_is_first_load")?.Value?.ToString().ToUpper() == "TRUE";


            var result = from rows in this._Model.t_host_wms_receipt_conf_header_export
                         select new
                         {
                             KeyID = rows.receipt_id,
                             btnKey = rows.receipt_id,
                             rows.receipt_id,
                             rows.processing_code,
                             rows.processing_status,
                             rows.wh_id,
                             rows.owner_id,
                             rows.order_type,
                             rows.supplier_code,
                             rows.inbound_order_number,
                             rows.receipt_number,
                             rows.reference_number,
                             rows.order_status,
                             rows.order_date,
                             rows.user_def1,
                             rows.user_def2,
                             rows.user_def3,
                             rows.user_def4,
                             rows.user_def5,
                             rows.user_def6,
                             rows.user_def7,
                             rows.user_def8,
                             rows.user_def9,
                             rows.user_def10,
                             rows.create_by,
                             rows.create_date,
                             rows.interface_date
                         };

            if (isFirstLoad)
            {
                result = result.Where(wh => wh.processing_status.ToUpper() == "PENDING");
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
