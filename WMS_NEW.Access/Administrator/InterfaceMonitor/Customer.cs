using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Administrator.InterfaceMonitor
{
    public class Customer : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public Customer()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static Customer Instance
        {
            get
            {
                using (Customer _Instance = new Customer())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public v_host_wms_customer GetByKeyID(object Id)
        {
            var realId = Id.ToString();

            //Inbound Order
            return this._Model.GetDataEntityBy<v_host_wms_customer>(this, delegate ()
            {
                var result = (from rows in this._Model.v_host_wms_customer
                              where rows.host_record_id == realId
                              select rows).FirstOrDefault();

                return result;
            });

        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            //var _active_load = this.FilterCustom.FirstOrDefault(wh => wh.DataFieldValue == "_active_load");

            var result = from rows in this._Model.v_host_wms_customer
                         select new
                         {
                             KeyID = rows.host_record_id,
                             rows.host_record_id,
                             rows.processing_status,
                             rows.customer_code,
                             rows.name,
                             rows.phone,
                             rows.fax,
                             rows.email,
                             rows.contact,
                             rows.create_by,
                             rows.create_date,
                             rows.interface_msg
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
