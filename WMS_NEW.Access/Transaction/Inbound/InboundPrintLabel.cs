using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;
using static WMS_NEW.Access.Transaction.Inbound.InboundPrintLabelDetail;

namespace WMS_NEW.Access.Transaction.Inbound
{
    public class InboundPrintLabel : AGridObjectSourceQuery, IDisposable
    {
        public WMSEntities _Model { get; set; }

        public InboundPrintLabel()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static InboundPrintLabel Instance
        {
            get
            {
                using (InboundPrintLabel _Instance = new InboundPrintLabel())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            string user_id = _SessionVals.UserName;
            string is_firstLoad = string.Empty;
            Guid? category_id = null;
            string item_number = string.Empty;
            DateTime? receive_date = null;
            DateTime? mfg_date = null;
            //string ref_inbound_order_number = string.Empty;
            string order_type = string.Empty;
            //string create_date = string.Empty;
            List<string> rules = new List<string>();
            List<string> NotWithinTheRules = new List<string>();

            if (this.FilterCustom != null)
            {

                var entCategory = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_category_id");
                if (entCategory != null && entCategory.Value != null)
                {
                    category_id = (Guid)entCategory.Value;
                }

                var entItem = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_item_number");
                if (entItem != null && entItem.Value != null)
                {
                    item_number = entItem.Value.ToString();
                }

                var entFirstLoad = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_is_first_load");
                if (entFirstLoad != null && entFirstLoad.Value != null)
                {
                    is_firstLoad = entFirstLoad.Value.ToString();
                }

                var entReceiveDate = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_receive_date");
                if (entReceiveDate != null && entReceiveDate.Value != null)
                {
                    receive_date = ((DateTime)entReceiveDate.Value).Date;
                }
                var entMFGDate = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_mfg_date");
                if (entMFGDate != null && entMFGDate.Value != null)
                {
                    mfg_date = ((DateTime)entMFGDate.Value).Date;
                }
                //var entCreateDate = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_create_date");
                //if (entCreateDate != null && entCreateDate.Value != null)
                //{
                //    create_date = entCreateDate.Value.ToString();
                //}
                //var entRefInboundOrderNumber = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_ref_inbound_order_number");
                //if (entRefInboundOrderNumber != null && entRefInboundOrderNumber.Value != null)
                //{
                //    ref_inbound_order_number = entRefInboundOrderNumber.Value.ToString();
                //}



                NotWithinTheRules = this._Model.t_wms_rule.Where(w => w.rule_code == "RULE_DISPLAY_NOT_IN_PRINT_LABEL").Select(s => s.value).ToList();



                var entOrderType = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_order_type");
                if (entOrderType != null && entOrderType.Value != null)
                {
                    order_type = entOrderType.Value.ToString();
                }
            }

            var query = (from rows in _Model.v_wms_print_label_viewer
                         join wh in _Model.t_wms_wh on rows.wh_master_id equals wh.wh_master_id into leftwh
                         from wh in leftwh.DefaultIfEmpty()
                         join owner in _Model.t_wms_owner on rows.owner_id equals owner.owner_id into leftowner
                         from owner in leftowner.DefaultIfEmpty()
                         where wh.t_wms_wh_user.Any(qry => qry.user_id == user_id)
                         && owner.t_wms_owner_user.Any(a => a.user_id == user_id && a.is_active == "YES")
                         select new
                         {
                             KeyId = rows.inbound_order_master_id,
                             rows.owner_code,
                             rows.owner_id,
                             rows.wh_master_id,
                             rows.wh_id,
                             rows.inbound_order_number,
                             rows.order_type,
                             rows.order_status,
                             rows.expected_delivery_date,
                             rows.supplier_id,
                             rows.supplier_code,
                             rows.supplier_name,
                             rows.customer_id,
                             rows.customer_code,
                             rows.customer_name,
                             rows.create_by,
                             rows.create_date,
                             rows.make_to,
                             rows.ref_inbound_order_number,
                             rows.ref_outbound_order_number,
                             line_number = rows.count_detail,
                             total_qty = rows.quantity_order,
                             rows.production_line,
                             rows.item_number,
                             rows.item_number_excel,

                         });

                        if (NotWithinTheRules.Count > 0)
            {
                query = from master in query
                        where !NotWithinTheRules.Contains(master.order_type)
                        select master;
            }
            if (!string.IsNullOrEmpty(is_firstLoad))
            {
                query = from master in query
                        where master.order_status != "CLOSE"
                        select master;
            }

            if (!string.IsNullOrEmpty(item_number))
            {
                query = from master in query
                        where master.item_number.Contains(item_number)
                        select master;
            }

            if (category_id != null)
            {
                var inbound_detail = _Model.t_wms_inbound_detail.Where(w => w.t_wms_wh_item.t_wms_item.category_id == category_id).Select(s => s.inbound_order_master_id).Distinct().ToList();
                query = from master in query
                        where inbound_detail.Contains(master.KeyId)
                        select master;
            }

            if (receive_date != null)
            {
                var inbound_detail = _Model.t_wms_inbound_detail.Where(w => DbFunctions.TruncateTime(w.receive_date) == receive_date).Select(s => s.inbound_order_master_id).Distinct().ToList();
                query = from master in query
                        where inbound_detail.Contains(master.KeyId)
                        select master;
            }

            if (mfg_date != null)
            {
                var inbound_detail = _Model.t_wms_inbound_detail.Where(w => DbFunctions.TruncateTime(w.mfg_date) == mfg_date).Select(s => s.inbound_order_master_id).Distinct().ToList();
                query = from master in query
                        where inbound_detail.Contains(master.KeyId)
                        select master;
            }
            return query;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion
    }
    public class InboundPrintLabelDetail : AEntityFormCommand<t_wms_print_label>
    {
        public WMSEntities _Model { get; set; }

        public InboundPrintLabelDetail()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_print_label; };
        }
        public override IQueryable<dynamic> InitialQueryView()
        {
            var inbound_order_master_id = (Guid)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_inbound_order_master_id").Value;
            var result = from rows in _Model.t_wms_print_label
                         where rows.inbound_order_master_id == inbound_order_master_id && (rows.is_cancelled == "NO" || rows.is_cancelled == null)
                         select new
                         {
                             KeyId = rows.print_label_id,
                             rows = rows.print_label_id,
                             rows.inbound_order_master_id,
                             rows.lpn,
                             rows.quantity_order,
                             rows.is_print,
                             rows.is_received,
                             rows.is_interface_hana,
                             rows.item_number,
                             rows.item_description,
                             rows.mfg_date,
                             rows.expiry_date,
                             rows.attribute1,
                             rows.pack_size_uom,
                             rows.pack_size_conversion_factor,
                             rows.pallet_size_uom,
                             rows.pallet_size_conversion_factor,
                             rows.pack_size_per_pallet,
                             rows.lot_number,
                             rows.row_print,
                             rows.line_number,
                             rows.production_line,
                             rows.print_date,
                             is_cancelled = rows.is_cancelled ?? "NO",
                         };

            //var entIsPrint = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_is_print");
            //if (entIsPrint != null && entIsPrint.Value != null)
            //{
            //    result = result.Where(x => x.is_print == entIsPrint.Value.ToString());
            //}

            var entIsFirstLoad = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_is_first_load");
            if (entIsFirstLoad != null && entIsFirstLoad.Value != null)
            {
                result = result.Where(x => x.is_print == "NO");
            }
            var entProductionLine = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_production_line");
            if (entProductionLine != null && entProductionLine.Value != null)
            {
                result = result.Where(x => x.production_line == entProductionLine.Value.ToString());
            }

            return result;

        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }
    }
}
