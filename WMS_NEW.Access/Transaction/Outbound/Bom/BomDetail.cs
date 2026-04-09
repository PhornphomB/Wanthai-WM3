using Prototype.Providers;
using System;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Outbound.Bom
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
    public class OutboundBomDetailDto
    {
        public Guid? outbound_order_bom_id { get; set; }
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
            Guid? outbound_order_bom_id = null;
            if (this.FilterCustom != null)
            {
                var entOrder = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_outbound_order_detail_id");
                if (entOrder != null && entOrder.Value != null)
                {
                    outbound_order_bom_id = (Guid)entOrder.Value;
                }
            }

            var bom = GetBomDetailData((Guid)outbound_order_bom_id);

            var result = (from rows in _Model.t_wms_outbound_detail
                          where rows.outbound_order_bom_id == bom.outbound_order_bom_id
                          select new BomDetailDto
                          {
                              KeyId = rows.outbound_order_detail_id,
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

        public override bool DeleteById(object Id)
        {
            return this._Model.Delete(this, delegate ()
            {
                var _id = new Guid(Id.ToString());

                var del = _Model.t_wms_outbound_detail.Single(x => x.outbound_order_detail_id == _id);
                _Model.t_wms_outbound_detail.Remove(del);

                return this._Model.SaveChanges();
            });
        }
        #endregion


        #region Function
        public OutboundBomDetailDto GetBomDetailData(Guid _outbound_order_master_id)
        {

            return this._Model.GetDataBy(this, delegate ()
            {
                var bom = (from rows in _Model.t_wms_outbound_detail
                           join grb_bom in _Model.t_wms_outbound_group_bom on rows.outbound_order_bom_id equals grb_bom.outbound_order_bom_id
                           where rows.outbound_order_detail_id == _outbound_order_master_id
                           select new OutboundBomDetailDto
                           {
                               outbound_order_bom_id = rows.outbound_order_bom_id,
                               bom_item_number = rows.bom_item_number,
                               quantity = grb_bom.quantity
                           }).Single();
                return bom;
            });
        }

        #endregion

    }
}
