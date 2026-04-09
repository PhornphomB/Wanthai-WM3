using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WMS_NEW.Source;
using Prototype.Providers;

namespace WMS_NEW.Access.Administrator
{
    public class TransactionLog : AGridObjectSourceQuery
    {
        public WMSEntities _Model { get; set; }

        public TransactionLog()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
        }

        public static TransactionLog Instance
        {
            get
            {
                using (TransactionLog _Instance = new TransactionLog())
                {
                    return _Instance;
                }
            }
        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_com_tran_log

                         join reas in this._Model.t_wms_reason
                         on rows.reason_code equals reas.reason_code into _reason
                         from reason in _reason.DefaultIfEmpty()

                         select new
                         {
                             KeyID = rows.tran_id,
                             rows.app_id,
                             rows.tran_type,
                             rows.sub_tran_type,
                             rows.app_name,
                             rows.description,
                             rows.start_tran_datetime,
                             rows.end_tran_datetime,
                             rows.warehouse,
                             rows.to_warehouse,
                             rows.location,
                             rows.to_location,
                             rows.owner,
                             rows.to_owner,
                             rows.item_number,
                             rows.quantity,
                             rows.quantity_uom,
                             rows.after_quantity,
                             rows.after_quantity_uom,
                             rows.lot_number,
                             rows.after_lot_number,
                             rows.expiry_date,
                             rows.after_expiry_date,
                             rows.serial_number,
                             rows.user_id,
                             rows.reference_number,
                             rows.control_number,
                             rows.lpn,
                             rows.after_lpn,
                             rows.parent_lpn,
                             rows.after_parent_lpn,
                             rows.status,
                             rows.after_status,
                             rows.zone,
                             rows.to_zone,
                             rows.order_number,
                             rows.line_number,
                             rows.control_value_1,
                             rows.control_value_2,
                             reason.reason_code,
                             reason_desc = reason.short_description,
                             rows.udf_1,
                             rows.udf_2,
                             rows.udf_3,
                             rows.udf_4,
                             rows.udf_datetime_1,
                             rows.udf_datetime_2,
                             rows.udf_datetime_3,
                             rows.device,
                             rows.create_date,
                             rows.create_by,
                             rows.rowversion,
                             rows.attribute1,
                             rows.attribute2,
                             rows.attribute3,
                             rows.attribute4,
                             rows.attribute5,
                             rows.after_attribute1,
                             rows.after_attribute2,
                             rows.after_attribute3,
                             rows.after_attribute4,
                             rows.after_attribute5,
                             rows.receive_date
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion

        public t_com_tran_log GetLog_BySerial(string _serial, Guid _owner_id)
        {
            _serial = Convert.ToInt32(_serial).ToString();

            var FORMAT_SERIAL_RUNNING = _Model.t_wms_rule.Where(wh => wh.rule_code == "FORMAT_SERIAL_RUNNING").FirstOrDefault();
            if (FORMAT_SERIAL_RUNNING != null)
            {
                _serial = _serial.PadLeft(FORMAT_SERIAL_RUNNING.value.Length, '0');
            }

            string prefix_serial = string.Empty;
            var ent = this._Model.t_wms_owner.Where(wh => wh.owner_id == _owner_id).FirstOrDefault();
            if (ent != null)
            {
                prefix_serial = ent.owner_number;
            }
            _serial = prefix_serial + _serial;


            var result = (from rows in this._Model.t_com_tran_log
                          where rows.serial_number == _serial
                          && rows.sub_tran_type != "CLOSE_RECEIPT"
                          orderby rows.create_date descending
                          select rows).FirstOrDefault();

            return result;
        }
    }
}
