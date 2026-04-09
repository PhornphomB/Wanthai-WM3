using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Administrator.InterfaceMonitor
{
    public class Item : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public Item()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static Item Instance
        {
            get
            {
                using (Item _Instance = new Item())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public v_host_wms_item_and_uom GetByKeyID(object Id)
        {
            var realId = Id.ToString();

            //Inbound Order
            return this._Model.GetDataEntityBy<v_host_wms_item_and_uom>(this, delegate ()
            {
                var result = (from rows in this._Model.v_host_wms_item_and_uom
                              where rows.host_record_id == realId
                              select rows).FirstOrDefault();

                return result;
            });

        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            //var _active_load = this.FilterCustom.FirstOrDefault(wh => wh.DataFieldValue == "_active_load");

            var result = from rows in this._Model.v_host_wms_item_and_uom
                         select new
                         {
                             KeyID = rows.host_record_id,
                             rows.host_record_id,
                             rows.item_number,
                             rows.description,
                             rows.item_category,
                             rows.lot_control,
                             rows.expiry_date_control,
                             rows.sn_control,
                             rows.attribute1_control,
                             rows.attribute2_control,
                             rows.attribute3_control,
                             rows.attribute4_control,
                             rows.attribute5_control,
                             rows.item_is_active,
                             rows.uom,
                             rows.uom_is_active,
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
