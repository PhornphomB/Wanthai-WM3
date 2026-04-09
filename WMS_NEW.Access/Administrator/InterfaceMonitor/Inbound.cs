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
    public class Inbound : AGridObjectSourceQuery
    {

        public WMSEntities _Model { get; set; }

        public Inbound()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static Inbound Instance
        {
            get
            {
                using (Inbound _Instance = new Inbound())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public t_host_wms_inbound_order GetByKeyID(object Id)
        {
            var realId = Id.ToString();

            //Inbound Order
            return this._Model.GetDataEntityBy<t_host_wms_inbound_order>(this, delegate ()
            {
                var result = (from rows in this._Model.t_host_wms_inbound_order
                              where rows.host_record_id == realId
                              select rows).FirstOrDefault();

                return result;
            });

        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            //var _active_load = this.FilterCustom.FirstOrDefault(wh => wh.DataFieldValue == "_active_load");

            var result = from rows in this._Model.t_host_wms_inbound_order
                         select new
                         {
                             KeyID = rows.host_record_id,
                             rows.host_record_id,
                             rows.record_type,
                             rows.processing_code,
                             rows.processing_status,
                             rows.wh_id,
                             rows.owner_id,
                             rows.order_number,
                             rows.type,
                             rows.supplier_code,
                             rows.customer_code,
                             rows.order_create_date,
                             rows.order_status,
                             rows.expected_delivery_date,
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
                             rows.insert_date,
                             rows.create_date,
                             rows.create_by,
                             rows.host_source
                         };

            //if (_active_load == null)
            //{
            //    result = result.Where(wh => false);
            //}

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion

    }
}
