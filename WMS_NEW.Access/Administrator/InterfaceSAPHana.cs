using ConfigGlobal.DTO._Global;
using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Administrator
{
    public class InterfaceSAPHana : AEntityFormCommand<t_host_export_interface_sap_hana>
    {
        #region ++INSTANCE STATIC++
        public static InterfaceSAPHana Instance
        {
            get
            {
                using (InterfaceSAPHana _Instance = new InterfaceSAPHana())
                {
                    return _Instance;
                }
            }
        }
        #endregion
        protected WMSEntities _Model { get; set; }

        public InterfaceSAPHana()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_host_export_interface_sap_hana; };
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_host_export_interface_sap_hana
                         select new
                         {
                             KeyID = rows.interface_id,
                             rows.unloading_point,
                             rows.process_type,
                             rows.processing_status,
                             rows.posting_date,
                             rows.document_date,
                             rows.document_header_text,
                             rows.item_number,
                             rows.plant,
                             rows.storage_location,
                             rows.batch,
                             rows.supplier_batch,
                             rows.movement_type,
                             rows.entry_quantity,
                             rows.entry_unit,
                             rows.purchase_order_number,
                             rows.purchase_order_item,
                             rows.good_recipient,
                             rows.cost_center,
                             rows.destination_item_number,
                             rows.destination_plant,
                             rows.destination_storage_location,
                             rows.destination_batch,
                             rows.expiry_date,
                             rows.production_date,
                             rows.gl_account,
                             rows.reference_no,
                             rows.wh_id,
                             rows.create_by,
                             rows.create_date,
                             rows.export_date,
                             rows.export_file_name,
                             rows.export_error_msg,
                             rows.lpn,
                             rows.tran_type,

                             //rows.item_text,
                             //rows.delivery_note,
                             //rows.bill_of_lading,
                             //rows.user_name,
                             //rows.movement_indicator,
                             //rows.reason_for_movement,
                             //rows.ref_export_id,
                             //rows.wh_master_id,
                             //rows.ref_interface_id,
                             //rows.rowversion,
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
