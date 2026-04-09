using Prototype.Providers;
using System;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Inbound.Receipt
{
    public class PartialReceiptDetail : AEntityFormCommand<t_wms_receipt_detail> 
    {
        public WMSEntities _Model { get; set; }


        public PartialReceiptDetail()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_receipt_detail; };
        }

        #region ++INSTANCE STATIC++
        public static PartialReceiptDetail Instance
        {
            get
            {
                using (PartialReceiptDetail _Instance = new PartialReceiptDetail())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Inherit IEntityCommandForm



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
                             rows.receipt_item_status,
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
                             grp.Key.receipt_item_status,
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
    }
}
