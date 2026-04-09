using Prototype.Providers;
using System;
using System.Data;
using System.Linq;
using WMS_NEW.Source;


namespace WMS_NEW.Access.Transaction.Inbound.Receipt
{
    public class ReceiptHeader : AEntityFormCommand<t_wms_receipt_header>
    {
        public WMSEntities _Model { get; set; }


        public ReceiptHeader()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_receipt_header; };
        }

        #region ++INSTANCE STATIC++
        public static ReceiptHeader Instance
        {
            get
            {
                using (ReceiptHeader _Instance = new ReceiptHeader())
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

                         //join uom in this._Model.t_wms_item_uom
                         //on rows.item_uom_id equals uom.item_uom_id

                         where rows.receipt_header_id == receipt_header_id
                         select new
                         {
                             KeyID = rows.receipt_detail_id,
                             detail.line_number,
                             item.item_number,
                             item.description,
                             item.t_wms_category.item_category,
                             detail.grade,
                             item.price,
                             rows.receipt_item_status,
                             rows.quantity_received,
                             uom = item.t_wms_item_uom.Where(w => w.primary_uom == "YES").FirstOrDefault().uom,
                             //uom.uom,
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


        public bool IsHeaderStatus(Guid _inbound_order_master_id, string _receipt_status)
        {

            var result = (from rows in this._Model.t_wms_receipt_header
                          where rows.inbound_order_master_id == _inbound_order_master_id
                          select rows);

            int countAll = result.Count();
            int countStatus = result.Where(qry => qry.receipt_status == _receipt_status).Count();

            return countAll == countStatus ? true : false;
        }

        public override bool Update()
        {
            return base.Update();
        }

        #region Get Query
        public IQueryable<Property> GetQuery(Guid _inbound_order_master_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_receipt_header
                             where rows.inbound_order_master_id == _inbound_order_master_id
                             orderby rows.receipt_number
                             select new Property
                             {
                                 guid_member = rows.receipt_header_id,
                                 display_member = rows.receipt_number
                             };

                return result;
            });
        }
        public IQueryable<Property> GetQuery_NotClose(Guid _inbound_order_master_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_receipt_header
                             where rows.inbound_order_master_id == _inbound_order_master_id
                             && rows.receipt_status != "CLOSE"
                             orderby rows.receipt_number
                             select new Property
                             {
                                 guid_member = rows.receipt_header_id,
                                 display_member = rows.receipt_number
                             };

                return result;
            });
        }
        public void UpdateUserDef9(Guid _receipt_header_id, DateTime? _user_def9)
        {
            var result = (from rows in this._Model.t_wms_receipt_header
                          where rows.receipt_header_id == _receipt_header_id
                          select rows).FirstOrDefault();

            if (result != null)
            {
                result.user_def9 = _user_def9;
                this._Model.SaveChanges();
            }
        }
        #endregion

    }
}
