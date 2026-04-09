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
    public class Ship : AGridObjectSourceQuery
    {

        public WMSEntities _Model { get; set; }

        public Ship()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++

        public static WMSEntities Instance
        {
            get
            {
                using (WMSEntities _Instance = new WMSEntities())
                {
                    return _Instance;
                }
            }
        }

        #endregion


        public t_host_wms_ship_conf_header_export GetByKeyID(object Id)
        {
            var realId = Id.ToString();

            //Inbound Order
            return this._Model.GetDataEntityBy<t_host_wms_ship_conf_header_export>(this, delegate ()
            {
                var result = (from rows in this._Model.t_host_wms_ship_conf_header_export
                              where rows.host_shipment_id == realId
                              select rows).FirstOrDefault();

                return result;
            });

        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            bool isFirstLoad = this.FilterCustom
                                  .FirstOrDefault(qry => qry.DataFieldValue == "_is_first_load")?.Value?.ToString().ToUpper() == "TRUE";


            var result = from rows in this._Model.t_host_wms_ship_conf_header_export
                         select new
                         {
                             KeyID = rows.host_shipment_id,
                             btnKey = rows.host_shipment_id,
                             rows.host_shipment_id,
                             rows.processing_code,
                             rows.processing_status,
                             rows.wh_id,
                             rows.customer_id,
                             rows.owner_id,
                             rows.order_number,
                             rows.order_status,
                             rows.order_type,
                             rows.order_date,
                             rows.tracking_number,
                             rows.ship_date_actual,
                             rows.delivery_date_plan,
                             rows.container_number,
                             rows.seal_number,
                             rows.trailer_number,
                             rows.carrier_code,
                             rows.equipment_type,
                             rows.cdl_license,
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
                             rows.customer_order_number,
                             rows.customer_purchase_order,
                             rows.create_date,
                             rows.create_by,
                             rows.interface_date,
                             rows.user_def9
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
