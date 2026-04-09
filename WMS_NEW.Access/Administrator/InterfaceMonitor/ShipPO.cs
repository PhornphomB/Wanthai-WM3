using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Administrator.InterfaceMonitor
{
    public class ShipPO : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public ShipPO()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static ShipPO Instance
        {
            get
            {
                using (ShipPO _Instance = new ShipPO())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public v_host_m3_ship_po_header GetByKeyID(object Id)
        {
            var realId = Id.ToString();

            //Inbound Order
            return this._Model.GetDataEntityBy<v_host_m3_ship_po_header>(this, delegate ()
            {
                var result = (from rows in this._Model.v_host_m3_ship_po_header
                                  where rows.WMSORN == realId
                              select rows).FirstOrDefault();

                return result;
            });

        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            bool isFirstLoad = this.FilterCustom
                                   .FirstOrDefault(qry => qry.DataFieldValue == "_is_first_load")?.Value?.ToString().ToUpper() == "TRUE";

            var query = from rows in this._Model.v_host_m3_ship_po_header
                         select new
                         {
                             KeyID = rows.WMSORN,
                             rows.WMSORN,
                             rows.customer_code,
                             rows.facility_code,
                             rows.warehouse,
                             rows.order_type,
                             rows.request_del_date,
                             rows.customer_po_no,
                             rows.customer_po_date,
                             rows.order_date,
                             rows.ship_date,
                             rows.ship_time,
                             rows.wms_create_date,
                             rows.wms_create_time,
                             rows.m3_get_status,
                             rows.m3_reference_no,
                             rows.m3_interface_date,
                             rows.m3_interface_time,
                             rows.m3_interface_status,
                             rows.Sync_UnsuccessNo,
                             rows.sync_flag,
                             rows.sync_date,
                             rows.error_msg
                         };

            if (isFirstLoad)
            {
                string currentDate = DateTime.Now.ToString("yyyyMMdd");
                query = query.Where(wh => wh.wms_create_date == currentDate);
            }

            return query;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion
    }
    public class ShipPODetail : AGridObjectSourceQuery
    {

        public WMSEntities _Model { get; set; }

        public ShipPODetail()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var wmsorn = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "wmsorn").Value;

            var result = from rows in this._Model.v_host_m3_ship_po_line
                         where rows.WMSORN == wmsorn
                         select new
                         {
                             KeyID = rows.WMSORN,
                             rows.item_number,
                             rows.item_description,
                             rows.request_del_date,
                             rows.quantity,
                             rows.uom,
                             rows.sales_price,
                             rows.sales_price_uom,
                             rows.line_wms_create_date,
                             rows.line_wms_create_time,
                             rows.line_m3_get_status,
                             rows.line_m3_reference_no,
                             rows.line_m3_interface_date,
                             rows.line_m3_interface_time,
                             rows.line_m3_interface_status,
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
