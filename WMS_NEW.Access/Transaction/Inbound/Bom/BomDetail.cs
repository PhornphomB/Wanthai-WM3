using Prototype.Providers;
using System;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Inbound.Bom
{
    [Serializable()]
    public class BomDetailDto
    {
        public Guid KeyId { get; set; }
        public string item_number { get; set; }
        public string uom { get; set; }
        public double? bom_detail_quantity { get; set; }
    }

    [Serializable()]
    public class InboundBomDetailDto
    {
        public Guid? inbound_order_bom_id { get; set; }
        public string bom_item_number { get; set; }
        public double quantity { get; set; }
    }


    public class BomDetail : AEntityFormCommand<BomDetailDto>
    {
        public WMSEntities _Model { get; set; }

        public BomDetail()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_inbound_detail; };
        }


        #region ++INSTANCE STATIC++
        public static BomDetail Instance
        {
            get
            {
                using (BomDetail _Instance = new BomDetail())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        #region Inherit IEntityCommandForm
        public override bool ValidateSaveNew(BomDetailDto ent, ref string msg_validate)
        {
            return true;
        }

        #endregion


        #region Inherit AGridObjectSourceQuery
        public override IQueryable<dynamic> InitialQueryView()
        {
            Guid? inbound_order_master_id = null;
            if (this.FilterCustom != null)
            {
                var entOrder = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_inbound_order_master_id");
                if (entOrder != null && entOrder.Value != null)
                {
                    inbound_order_master_id = (Guid)entOrder.Value;
                }
            }

            var bom = GetBomDetailData((Guid)inbound_order_master_id);

            var result = (from rows in _Model.t_wms_inbound_detail
                          where rows.inbound_order_bom_id == bom.inbound_order_bom_id
                          select new BomDetailDto
                          {
                              KeyId = rows.inbound_order_detail_id,
                              item_number = rows.item_number,
                              uom = rows.uom,
                              bom_detail_quantity = rows.bom_detail_quantity
                          }).ToList();

            return result.AsQueryable();
        }

        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


        #region Function
        public InboundBomDetailDto GetBomDetailData(Guid _inbound_order_master_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var bom = (from rows in _Model.t_wms_inbound_detail
                           join grb_bom in _Model.t_wms_inbound_group_bom on rows.inbound_order_bom_id equals grb_bom.inbound_order_bom_id
                           where rows.inbound_order_detail_id == _inbound_order_master_id
                           select new InboundBomDetailDto
                           {
                               inbound_order_bom_id = rows.inbound_order_bom_id,
                               bom_item_number = rows.bom_item_number,
                               quantity = grb_bom.quantity
                           }).Single();
                return bom;
            });
        }
        #endregion

    }
}
