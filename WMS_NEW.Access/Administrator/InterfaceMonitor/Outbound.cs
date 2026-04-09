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
    public class Outbound : AGridObjectSourceQuery
    {

        public WMSEntities _Model { get; set; }

        public Outbound()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static Outbound Instance
        {
            get
            {
                using (Outbound _Instance = new Outbound())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public t_host_wms_outbound_order GetByKeyID(object Id)
        {
            var realId = Id.ToString();

            //Inbound Order
            return this._Model.GetDataEntityBy<t_host_wms_outbound_order>(this, delegate ()
            {
                var result = (from rows in this._Model.t_host_wms_outbound_order
                              where rows.host_record_id == realId
                              select rows).FirstOrDefault();

                return result;
            });

        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            //var _active_load = this.FilterCustom.FirstOrDefault(wh => wh.DataFieldValue == "_active_load");

            var result = from rows in this._Model.t_host_wms_outbound_order
                         select new
                         {
                             KeyID = rows.host_record_id,
                             rows.outbound_order_id,
                             rows.record_type,
                             rows.processing_code,
                             rows.processing_status,
                             rows.wh_id,
                             rows.owner_id,
                             rows.order_number,
                             rows.type,
                             rows.order_status,
                             rows.customer_order_number,
                             rows.customer_code,
                             rows.customer_purchase_order,
                             rows.order_create_date,
                             rows.order_ship_date,
                             rows.department,
                             rows.load_id,
                             rows.load_sequence_number,
                             rows.tracking_number,
                             rows.rush_order,
                             rows.back_order_flag,
                             rows.ship_to_code,
                             rows.ship_to_name,
                             rows.ship_to_address_line_1,
                             rows.ship_to_address_line_2,
                             rows.ship_to_address_line_3,
                             rows.ship_to_city,
                             rows.ship_to_province,
                             rows.ship_to_postal_code,
                             rows.ship_to_country,
                             rows.ship_to_country_name,
                             rows.ship_to_contact_name,
                             rows.ship_to_phone_number,
                             rows.bill_to_code,
                             rows.bill_to_name,
                             rows.bill_to_address_line_1,
                             rows.bill_to_address_line_2,
                             rows.bill_to_address_line_3,
                             rows.bill_to_city,
                             rows.bill_to_province,
                             rows.bill_to_postal_code,
                             rows.bill_to_country,
                             rows.bill_to_country_name,
                             rows.bill_to_contact_name,
                             rows.bill_to_phone_number,
                             rows.requested_by,
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
                             rows.host_source,
                             rows.delivery_date_plan,
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
