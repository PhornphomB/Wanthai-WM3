using Prototype.Providers;
using System;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Inbound.Receipt
{
    public class PartialReceiptDetailSummary : AGridObjectSourceQuery
    {

        public WMSEntities _Model { get; set; }


        public PartialReceiptDetailSummary()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
        }

        #region ++INSTANCE STATIC++
       
        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            Guid? receipt_header_id = null;
            if (this.FilterCustom != null)
            {
                var entOrder = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_receipt_header_id");
                if (entOrder != null && entOrder.Value != null)
                {
                    receipt_header_id = (Guid)entOrder.Value;
                }
            }


            var result = from rows in this._Model.t_wms_receipt_detail
                         let detail = rows.t_wms_inbound_detail
                         let item = detail.t_wms_wh_item.t_wms_item

                         join uom in this._Model.t_wms_item_uom
                         on rows.item_uom_id equals uom.item_uom_id

                         where rows.receipt_header_id == receipt_header_id

                         group rows
                         by new
                         {
                             // rows.receipt_detail_id,
                             detail.line_number,
                             item.item_number,
                             item.description,
                             item.t_wms_category.item_category,
                             category_description = item.t_wms_category.description,
                             detail.grade,
                             detail.price,
                             detail.quantity_order,
                             uom.uom,
                         } into grp
                         select new
                         {
                             KeyID = "0",
                             grp.Key.line_number,
                             grp.Key.item_number,
                             grp.Key.description,
                             grp.Key.item_category,
                             grp.Key.category_description,
                             grp.Key.grade,
                             grp.Key.price,
                             grp.Key.quantity_order,
                             quantity_received = grp.Sum(qry => qry.quantity_received),
                             grp.Key.uom
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion

        public DataTable Get_ReceiptSummary(Guid _receipt_header_id)
        {
            return this._Model.GetDataTable(this, delegate ()
            {
                var result = from rows in this._Model.t_wms_receipt_detail
                             let detail = rows.t_wms_inbound_detail
                             let item = detail.t_wms_wh_item.t_wms_item

                             join uom in this._Model.t_wms_item_uom
                             on rows.item_uom_id equals uom.item_uom_id

                             where rows.receipt_header_id == _receipt_header_id

                             group rows
                             by new
                             {
                                 //KeyID = "0",
                                 // rows.receipt_detail_id,
                                 detail.line_number,
                                 item.item_number,
                                 item.description,
                                 rows.lpn,
                                 rows.parent_lpn,
                                 rows.lot_number,
                                 rows.expiry_date,
                                 rows.serial_number,
                                 item.t_wms_category.item_category,
                                 category_description = item.t_wms_category.description,
                                 detail.grade,
                                 detail.price,
                                 detail.quantity_order,
                                 //quantity_received = detail.quantity_receive,
                                 //rows.quantity_received,
                                 uom.uom,
                                 //isColor = false,
                                 rows.location_received
                             } into grp
                             select new
                             {
                                 KeyID = "0",
                                 grp.Key.line_number,
                                 grp.Key.item_number,
                                 grp.Key.description,
                                 grp.Key.lpn,
                                 grp.Key.parent_lpn,
                                 grp.Key.lot_number,
                                 grp.Key.expiry_date,
                                 grp.Key.serial_number,
                                 grp.Key.item_category,
                                 grp.Key.category_description,
                                 grp.Key.grade,
                                 grp.Key.price,
                                 grp.Key.quantity_order,
                                 quantity_received = grp.Sum(s => s.quantity_received),
                                 grp.Key.uom,
                                 isColor = false,
                                 grp.Key.location_received
                             };
                return result;
            });

        }
    }
}
