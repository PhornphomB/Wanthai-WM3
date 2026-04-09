using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using WMS_NEW.Source;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace WMS_NEW.Access.Transaction.Inbound
{
    public class InboundMaster : AEntityFormCommand<t_wms_inbound_master>
    {
        public WMSEntities _Model { get; set; }

        public InboundMaster()
        {
            _Model = new WMSEntities();
            _Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_inbound_master; };
        }


        #region ++INSTANCE STATIC++
        public static InboundMaster Instance
        {
            get
            {
                using (InboundMaster _Instance = new InboundMaster())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        #region Inherit IEntityCommandForm
        public override bool ValidateSaveNew(t_wms_inbound_master ent, ref string msg_validate)
        {
            if (_Model.t_wms_inbound_master.Any(qry => qry.wh_master_id == ent.wh_master_id && qry.inbound_order_number == ent.inbound_order_number))
            {
                msg_validate = "! Warehouse ID  and Inbound Order No. has existed key";
                return false;
            }
            else
            {
                return true;
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
            DateTime? mfg_date_to = null;
            string ref_inbound_order_number = string.Empty;
            string page_type = string.Empty;
            string order_type = string.Empty;
            string create_date = string.Empty;
            List<string> rules = new List<string>();
            List<string> NotWithinTheRules = new List<string>();
            FilterCustomSchema entMfgDate = new FilterCustomSchema();
            if (this.FilterCustom != null)
            {
                entMfgDate = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_mfg_date");

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
                var entCreateDate = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_create_date");
                if (entCreateDate != null && entCreateDate.Value != null)
                {
                    create_date = entCreateDate.Value.ToString();
                }
                var entRefInboundOrderNumber = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_ref_inbound_order_number");
                if (entRefInboundOrderNumber != null && entRefInboundOrderNumber.Value != null)
                {
                    ref_inbound_order_number = entRefInboundOrderNumber.Value.ToString();
                }

                var entPageType = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_page_type");
                if (entPageType != null && entPageType.Value != null)
                {
                    page_type = "pagetype=" + entPageType.Value.ToString();
                    rules = this._Model.t_wms_rule.Where(w => w.rule_code == "RULE_DISPLAY_INBOUND_PLAN" /*&& w.is_active == "YES"*/ && w.type == page_type).Select(s => s.value).ToList();
                }

                var entOrderType = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_order_type");
                if (entOrderType != null && entOrderType.Value != null)
                {
                    order_type = entOrderType.Value.ToString();
                }
            }

            var query = (from rows in _Model.v_wms_inbound_master_viewer
                         join wh in _Model.t_wms_wh on rows.wh_master_id equals wh.wh_master_id into leftwh
                         from wh in leftwh.DefaultIfEmpty()
                         join owner in _Model.t_wms_owner on rows.owner_id equals owner.owner_id into leftowner
                         from owner in leftowner.DefaultIfEmpty()
                         where rules.Contains(rows.order_type)
                         && wh.t_wms_wh_user.Any(qry => qry.user_id == user_id)
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
                             total_qty = rows.pack_size_per_pallet,
                             rows.production_line,
                             rows.item_number,
                             rows.add_production
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


            if (category_id != null)
            {
                var inbound_detail = _Model.t_wms_inbound_detail.Where(w => w.t_wms_wh_item.t_wms_item.category_id == category_id).Select(s => s.inbound_order_master_id).Distinct().ToList();
                query = (from master in query
                         where inbound_detail.Contains(master.KeyId)
                         select master).Distinct();

            }

            if (!string.IsNullOrEmpty(item_number))
            {
                query = from master in query
                        where master.item_number.Contains(item_number)
                        select master;
            }

            if (receive_date != null)
            {
                var inbound_detail = _Model.t_wms_inbound_detail.Where(w => DbFunctions.TruncateTime(w.receive_date) == receive_date).Select(s => s.inbound_order_master_id).Distinct().ToList();
                query = (from master in query
                         where inbound_detail.Contains(master.KeyId)
                         select master).Distinct();
            }

            if (entMfgDate.Value != null)
            {
                IQueryable<t_wms_inbound_detail> inboundDetailQuery = _Model.t_wms_inbound_detail.AsQueryable();

                switch (entMfgDate.FilterAt)
                {
                    case FilterAt.Between:
                        if (entMfgDate.ValueTo != null)
                        {
                            inboundDetailQuery = inboundDetailQuery.Where($"mfg_date >= @0 AND mfg_date <= @1", entMfgDate.Value, entMfgDate.ValueTo);
                        }
                        break;
                    case FilterAt.Equal:
                        inboundDetailQuery = inboundDetailQuery.Where($"mfg_date == @0", entMfgDate.Value);
                        break;
                    case FilterAt.MoreThan:
                        inboundDetailQuery = inboundDetailQuery.Where($"mfg_date > @0", entMfgDate.Value);
                        break;
                    case FilterAt.LessThan:
                        inboundDetailQuery = inboundDetailQuery.Where($"mfg_date < @0", entMfgDate.Value);
                        break;
                    case FilterAt.MoreThanEqual:
                        inboundDetailQuery = inboundDetailQuery.Where($"mfg_date >= @0", entMfgDate.Value);
                        break;
                    case FilterAt.LessThanEqual:
                        inboundDetailQuery = inboundDetailQuery.Where($"mfg_date <= @0", entMfgDate.Value);
                        break;
                    case FilterAt.NotEqual:
                        inboundDetailQuery = inboundDetailQuery.Where($"mfg_date != @0", entMfgDate.Value);
                        break;
                    case FilterAt.Contains:
                        inboundDetailQuery = inboundDetailQuery.Where($"mfg_date.Contains(@0)", entMfgDate.Value);
                        break;
                    case FilterAt.Empty:
                        inboundDetailQuery = inboundDetailQuery.Where($"String.IsNullOrEmpty(mfg_date)");
                        break;
                    default:
                        break;
                }

                var inboundDetailSet = new HashSet<Guid>(inboundDetailQuery.Select(s => s.inbound_order_master_id).Distinct());

                query = query.Where(master => inboundDetailSet.Contains(master.KeyId)).Distinct();

            }

            if (!string.IsNullOrEmpty(create_date))
            {
                query = from master in query
                        where DbFunctions.TruncateTime(master.create_date) == DateTime.Now
                        select master;
            }

            if (!string.IsNullOrEmpty(ref_inbound_order_number))
            {
                query = from master in query
                        where master.ref_inbound_order_number.Contains(ref_inbound_order_number)
                        select master;
            }
            if (!string.IsNullOrEmpty(order_type))
            {
                query = from master in query
                        where master.order_type == order_type
                        select master;
            }
            //251023 พี่นัทให้แก้เป็น &&
            //if (!string.IsNullOrEmpty(item_number) && receive_date != null)
            //{
            //    query = from master in query
            //            where master.t_wms_inbound_detail.Any(qry => qry.t_wms_receipt_detail.Any(w => DbFunctions.TruncateTime(w.receive_date) == receive_date && qry.t_wms_wh_item.t_wms_item.item_number.Contains(item_number)))
            //            select master;
            //}
            return query;
        }
        //public override IQueryable<dynamic> InitialQueryView()
        //{
        //    string user_id = _SessionVals.UserName;
        //    string is_firstLoad = string.Empty;
        //    Guid? category_id = null;
        //    string item_number = string.Empty;
        //    DateTime? receive_date = null;
        //    string ref_inbound_order_number = string.Empty;
        //    string page_type = string.Empty;
        //    string order_type = string.Empty;
        //    string create_date = string.Empty;

        //    List<string> rules = new List<string>();
        //    List<string> rulesNotIn = new List<string>();
        //    if (this.FilterCustom != null)
        //    {

        //        var entCategory = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_category_id");
        //        if (entCategory != null && entCategory.Value != null)
        //        {
        //            category_id = (Guid)entCategory.Value;
        //        }

        //        var entItem = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_item_number");
        //        if (entItem != null && entItem.Value != null)
        //        {
        //            item_number = entItem.Value.ToString();
        //        }

        //        var entFirstLoad = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_is_first_load");
        //        if (entFirstLoad != null && entFirstLoad.Value != null)
        //        {
        //            is_firstLoad = entFirstLoad.Value.ToString();
        //        }

        //        var entReceiveDate = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_receive_date");
        //        if (entReceiveDate != null && entReceiveDate.Value != null)
        //        {
        //            receive_date = ((DateTime)entReceiveDate.Value).Date;
        //        }
        //        var entCreateDate = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_create_date");
        //        if (entCreateDate != null && entCreateDate.Value != null)
        //        {
        //            create_date = entCreateDate.Value.ToString();
        //        }
        //        var entRefInboundOrderNumber = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_ref_inbound_order_number");
        //        if (entRefInboundOrderNumber != null && entRefInboundOrderNumber.Value != null)
        //        {
        //            ref_inbound_order_number = entRefInboundOrderNumber.Value.ToString();
        //        }

        //        var entPageType = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_page_type");
        //        if (entPageType != null && entPageType.Value != null)
        //        {
        //            page_type = "pagetype=" + entPageType.Value.ToString();
        //            rules = this._Model.t_wms_rule.Where(w => w.rule_code == "RULE_DISPLAY_INBOUND_PLAN" /*&& w.is_active == "YES"*/ && w.type == page_type).Select(s => s.value).ToList();
        //        }
        //        else
        //        {
        //            rules = this._Model.t_wms_rule.Where(w => w.rule_code == "RULE_DISPLAY_INBOUND_PLAN").Select(s => s.value).ToList();
        //            rulesNotIn = this._Model.t_wms_rule.Where(w => w.rule_code == "RULE_DISPLAY_NOT_IN_PRINT_LABEL").Select(s => s.value).ToList();
        //        }

        //        var entOrderType = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_order_type");
        //        if (entOrderType != null && entOrderType.Value != null)
        //        {
        //            order_type = entOrderType.Value.ToString();
        //        }
        //        //if((!this.FilterAuto.Any(x => x.DataPropertyName == "create_date")) && is_firstLoad == "True")
        //        //{
        //        //    this.FilterAuto.Add("create_date", FilterAt.Equal, DateTime.Now);
        //        //}
        //    }

        //    //Inbound Order


        //    var query = from master in this._Model.t_wms_inbound_master
        //                where master.t_wms_wh.t_wms_wh_user.Any(qry => qry.user_id == user_id) && master.t_wms_owner.t_wms_owner_user.Any(a => a.user_id == user_id && a.is_active == "YES")
        //                && rules.Contains(master.order_type)
        //                select master;


        //    if(rulesNotIn.Count > 0)
        //    {
        //        query = from master in query
        //                where !rulesNotIn.Contains(master.order_type)
        //                select master;
        //    }

        //    if (!string.IsNullOrEmpty(is_firstLoad))
        //    {
        //        query = from master in query
        //                where master.order_status != "CLOSE"
        //                select master;
        //    }


        //    if (category_id != null)
        //    {
        //        query = from master in query
        //                where master.t_wms_inbound_detail.Any(qry => qry.t_wms_wh_item.t_wms_item.category_id == category_id)
        //                select master;

        //    }

        //    if (!string.IsNullOrEmpty(item_number))
        //    {
        //        query = from master in query
        //                where master.t_wms_inbound_detail.Any(qry => qry.t_wms_wh_item.t_wms_item.item_number.Contains(item_number))
        //                select master;
        //    }

        //    if (receive_date != null)
        //    { 
        //        query = from master in query
        //                where master.t_wms_inbound_detail.Any(qry => qry.t_wms_receipt_detail.Any(w=> DbFunctions.TruncateTime(w.receive_date) == receive_date))
        //                select master;
        //    }

        //    if(!string.IsNullOrEmpty(create_date))
        //    {
        //        query = from master in query
        //                where DbFunctions.TruncateTime(master.create_date) == DateTime.Now
        //                select master;
        //    }

        //    if (!string.IsNullOrEmpty(ref_inbound_order_number))
        //    {
        //        query = from master in query
        //                where master.t_wms_inbound_detail.Any(qry => qry.attribute1.Contains(ref_inbound_order_number))
        //                select master;
        //    }

        //    //251023 พี่นัทให้แก้เป็น &&
        //    if (!string.IsNullOrEmpty(item_number) && receive_date != null)
        //    {
        //        query = from master in query
        //                where master.t_wms_inbound_detail.Any(qry => qry.t_wms_receipt_detail.Any(w => DbFunctions.TruncateTime(w.receive_date) == receive_date && qry.t_wms_wh_item.t_wms_item.item_number.Contains(item_number)))
        //                select master;
        //    }


        //    var queryResult = (from rows in query
        //                 let expected_delivery_date = DbFunctions.TruncateTime(rows.expected_delivery_date)
        //                 let customer = rows.t_wms_customer
        //                 select new
        //                 {
        //                     KeyId = rows.inbound_order_master_id,
        //                     rows.owner_code,
        //                     rows.owner_id,
        //                     rows.wh_master_id,
        //                     rows.wh_id,
        //                     rows.inbound_order_number,
        //                     rows.order_type,
        //                     rows.order_status,
        //                     expected_delivery_date,
        //                     rows.supplier_id,
        //                     rows.t_wms_supplier.supplier_code,
        //                     rows.t_wms_supplier.supplier_name,
        //                     rows.customer_id,
        //                     customer.customer_code,
        //                     customer.customer_name,
        //                     rows.create_by,
        //                     rows.create_date,
        //                     rows.make_to,
        //                     rows.ref_inbound_order_number,
        //                     rows.ref_outbound_order_number,
        //                     line_number = rows.t_wms_inbound_detail.Count(),
        //                     total_qty = (double?)rows.t_wms_inbound_detail.Sum(s => s.quantity_order) ?? 0,
        //                 }).ToList();

        //    var result = queryResult.Select(row => new
        //    {
        //        row.KeyId,
        //        row.owner_code,
        //        row.owner_id,
        //        row.wh_master_id,
        //        row.wh_id,
        //        row.inbound_order_number,
        //        row.order_type,
        //        row.order_status,
        //        row.expected_delivery_date,
        //        row.supplier_id,
        //        row.supplier_code,
        //        row.supplier_name,
        //        row.customer_id,
        //        row.customer_code,
        //        row.customer_name,
        //        row.create_by,
        //        row.create_date,
        //        row.make_to,
        //        row.ref_inbound_order_number,
        //        row.ref_outbound_order_number,
        //        production_line = string.Join(", ", (_Model.t_wms_inbound_detail.Where(x => x.inbound_order_master_id == row.KeyId).Select(x => x.production_line).Distinct())),
        //        row.line_number,
        //        row.total_qty
        //    });
        //    if (!string.IsNullOrEmpty(order_type))
        //    {
        //        result = result.Where(x => x.order_type == order_type);
        //    }

        //    return result.AsQueryable();

        //}
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }
        public IQueryable<Property> GetQueryCode(string _group_name, string _page_type)
        {
            List<string> rules = new List<string>();

            if (string.IsNullOrEmpty(_page_type))
            {
                rules = this._Model.t_wms_rule.Where(w => w.rule_code == "RULE_DISPLAY_INBOUND_PLAN").Select(s => s.value).ToList();
            }
            else
            {
                string page_type = "pagetype=" + _page_type;
                rules = this._Model.t_wms_rule.Where(w => w.rule_code == "RULE_DISPLAY_INBOUND_PLAN" /*&& w.is_active == "YES"*/ && w.type == page_type).Select(s => s.value).ToList();
            }


            var result = from rows in _Model.t_com_combobox_item
                         where rows.is_active == "YES"
                         && rows.group_name == _group_name
                         && rules.Contains(rows.value_member)
                         orderby rows.display_sequence, rows.group_name, rows.value_member ascending
                         select new Property
                         {
                             value_member = rows.value_member,
                             display_member = rows.display_member
                         };

            return result;
        }
        public IQueryable<Property> GetOutboundRef()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var detail = _Model.t_wms_inbound_detail.Where(x => x.ref_lpn != null).Select(x => x.ref_lpn).ToList();
                var pick = _Model.t_wms_outbound_pick_detail.Where(x => detail.Contains(x.lpn)).Select(x => x.outbound_order_detail_id).Distinct().ToList();
                var outdetail = _Model.t_wms_outbound_detail.Where(x => pick.Contains(x.outbound_order_detail_id)).Select(x => x.outbound_order_master_id).ToList();

                var result = from rows in _Model.t_wms_outbound_master
                             where rows.order_type.ToUpper() == "REPACK" && rows.order_status.ToUpper() == "SHIP"
                             //&& !outdetail.Contains(rows.outbound_order_master_id)
                             orderby rows.outbound_order_number ascending
                             select new Property
                             {
                                 guid_member = rows.outbound_order_master_id,
                                 display_member = rows.outbound_order_number
                             };

                return result;
            });
        }
        public IQueryable<Property> GetOutboundRefAll()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var detail = _Model.t_wms_inbound_detail.Where(x => x.ref_lpn != null).Select(x => x.ref_lpn).ToList();
                var pick = _Model.t_wms_outbound_pick_detail.Where(x => detail.Contains(x.lpn)).Select(x => x.outbound_order_detail_id).Distinct().ToList();
                var outdetail = _Model.t_wms_outbound_detail.Where(x => pick.Contains(x.outbound_order_detail_id)).Select(x => x.outbound_order_master_id).ToList();

                var result = from rows in _Model.t_wms_outbound_master
                             where outdetail.Contains(rows.outbound_order_master_id)
                             orderby rows.outbound_order_number ascending
                             select new Property
                             {
                                 guid_member = rows.outbound_order_master_id,
                                 display_member = rows.outbound_order_number
                             };

                return result;
            });

        }
        #endregion

        #region Function
        //public override bool DeleteById(object _Id)
        //{
        //    return this._Model.Delete(this, delegate ()
        //    {
        //        Guid inbound_order_master_id = (Guid)_Id;
        //        var ent = this._Model.t_wms_inbound_master.Where(w => w.inbound_order_master_id == inbound_order_master_id).FirstOrDefault();
        //        if (ent != null && ent.order_status == "OPEN")
        //        {
        //            var inbound_order_number = ent.inbound_order_number;
        //            var whId = ent.wh_id;
        //            var ownerCode = ent.owner_code;

        //            var entDet = this._Model.t_wms_inbound_detail.Where(w => w.inbound_order_master_id == ent.inbound_order_master_id);
        //            this._Model.t_wms_inbound_detail.RemoveRange(entDet);

        //            var entRec = this._Model.t_wms_receipt_header.Where(w => w.inbound_order_master_id == inbound_order_master_id);
        //            this._Model.t_wms_receipt_header.RemoveRange(entRec);

        //            this._Model.t_wms_inbound_master.Remove(ent);

        //            var entPrint = this._Model.t_wms_print_label.Where(w => w.inbound_order_master_id == inbound_order_master_id);
        //            this._Model.t_wms_print_label.RemoveRange(entPrint);

        //        }

        //        return this._Model.SaveChanges();
        //    });
        //}

        public override bool DeleteById(object _Id)
        {
            string message = string.Empty;

            return this._Model.Delete(this, delegate ()
            {
                Guid inbound_order_master_id = (Guid)_Id;

                var ent = this._Model.t_wms_inbound_master
                    .FirstOrDefault(w => w.inbound_order_master_id == inbound_order_master_id);

                if (ent == null)
                {
                    message = "Inbound order not found.";
                    return -1;
                }

                if (ent.order_status != "OPEN")
                {
                    message = $"Cannot delete because order status is '{ent.order_status}'.";
                    return -1;
                }

                // เก็บค่าที่ต้องใช้เรียก Stored ก่อนลบ
                var inbound_order_number = ent.inbound_order_number;
                var whId = ent.wh_id;
                var ownerCode = ent.owner_code;

                // ลบรายการลูก
                var entDet = this._Model.t_wms_inbound_detail
                    .Where(w => w.inbound_order_master_id == inbound_order_master_id);
                this._Model.t_wms_inbound_detail.RemoveRange(entDet);

                var entRec = this._Model.t_wms_receipt_header
                    .Where(w => w.inbound_order_master_id == inbound_order_master_id);
                this._Model.t_wms_receipt_header.RemoveRange(entRec);

                var entPrint = this._Model.t_wms_print_label
                    .Where(w => w.inbound_order_master_id == inbound_order_master_id);
                this._Model.t_wms_print_label.RemoveRange(entPrint);

                this._Model.t_wms_inbound_master.Remove(ent);

                // 1️⃣ SaveChanges ก่อน
                var affected = this._Model.SaveChanges();

                if (affected > 0)
                {
                    // 2️⃣ ถ้า Save สำเร็จ → ค่อยเรียก Stored
                    var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
                    var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                    this._Model.usp_inf_lims_delete_inbound_order(
                        _SessionVals.AppID,
                        inbound_order_number,
                        whId,
                        ownerCode,
                        _SessionVals.DeviceID,
                        _SessionVals.UserName,
                        errCode,
                        errMsg
                    );

                    if (errCode.Value?.ToString() != "0")
                    {
                        message = errMsg.Value?.ToString() ?? "Unknown error from stored procedure.";
                    }
                }

                return affected;
            }, message);
            }


        public void ValidateCloseOrder(string _appID, Guid _wh_master_id, Guid _inbound_order_master_id, string _userID, out string _errIsConfirm, out string _errCode, out string _errMsg)
        {
            var errIsConfirm = new ObjectParameter("out_vchIsConfirm", typeof(string));
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_inbound_validate_close_order(_appID, _wh_master_id, _inbound_order_master_id, _userID, errIsConfirm, errCode, errMsg);
            _errCode = errCode.Value.ToString();
            _errMsg = errMsg.Value.ToString();
            _errIsConfirm = errIsConfirm.Value.ToString();

        }

        //public void CloseOrder(string _appID, Guid _wh_master_id, Guid _inbound_order_master_id, string _close_remark, string _confirm_meke_to_order, string _deviceID, string _userID, out string _errCode, out string _errMsg)
        //{
        //    var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
        //    var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

        //    this._Model.usp_close_inbound_order(_appID, _wh_master_id, _inbound_order_master_id, _close_remark, _confirm_meke_to_order, _deviceID, _userID, errCode, errMsg);

        //    _errCode = errCode.Value.ToString();
        //    _errMsg = errMsg.Value.ToString();
        //}
        public void CloseOrder(string _appID, Guid _wh_master_id, Guid _inbound_order_master_id, string _close_remark, string _confirm_meke_to_order, string _deviceID, string _userID, out string _errCode, out string _errMsg)
        {
            #region NativeClientStored

            using (WMSEntities context = new WMSEntities())
            {
                string connectionString = context.Database.Connection.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("usp_close_inbound_order", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter ApplicationID = new SqlParameter
                        {
                            ParameterName = "@in_vchApplicationID",
                            SqlDbType = SqlDbType.VarChar,
                            Size = 40,
                            Value = _appID
                        };

                        SqlParameter WhMasterID = new SqlParameter
                        {
                            ParameterName = "@in_vchWhMasterID",
                            SqlDbType = SqlDbType.UniqueIdentifier,
                            Value = _wh_master_id
                        };

                        SqlParameter InboundMasterID = new SqlParameter
                        {
                            ParameterName = "@in_vchInboundMasterID",
                            SqlDbType = SqlDbType.UniqueIdentifier,
                            Value = _inbound_order_master_id
                        };

                        SqlParameter CloseRemark = new SqlParameter
                        {
                            ParameterName = "@in_vchCloseRemark",
                            SqlDbType = SqlDbType.NVarChar,
                            Size = 200,
                            Value = _close_remark ?? ""
                        };

                        SqlParameter IsConfirmMakeToOrder = new SqlParameter
                        {
                            ParameterName = "@in_vchIsConfirmMakeToOrder",
                            SqlDbType = SqlDbType.VarChar,
                            Size = 3,
                            Value = _confirm_meke_to_order
                        };

                        SqlParameter DeviceID = new SqlParameter
                        {
                            ParameterName = "@in_vchDeviceID",
                            SqlDbType = SqlDbType.NVarChar,
                            Size = 50,
                            Value = _deviceID
                        };

                        SqlParameter UserID = new SqlParameter
                        {
                            ParameterName = "@in_vchUserID",
                            SqlDbType = SqlDbType.NVarChar,
                            Size = 25,
                            Value = _userID
                        };

                        SqlParameter ErrorCode = new SqlParameter
                        {
                            ParameterName = "@out_ErrorCode",
                            SqlDbType = SqlDbType.VarChar,
                            Direction = ParameterDirection.Output,
                            Size = 50 // Set the size if needed
                        };

                        SqlParameter ErrorMessage = new SqlParameter
                        {
                            ParameterName = "@out_ErrorMessage",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Output,
                            Size = 500 // Set the size if needed
                        };


                        command.Parameters.Add(ApplicationID);
                        command.Parameters.Add(WhMasterID);
                        command.Parameters.Add(InboundMasterID);
                        command.Parameters.Add(CloseRemark);
                        command.Parameters.Add(IsConfirmMakeToOrder);
                        command.Parameters.Add(DeviceID);
                        command.Parameters.Add(UserID);
                        command.Parameters.Add(ErrorCode);
                        command.Parameters.Add(ErrorMessage);
                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Access output parameter value

                        _errCode = ErrorCode.Value.ToString();
                        _errMsg = ErrorMessage.Value.ToString();
                        // Now 'result' contains the output parameter value
                    }
                }
            }
            #endregion
        }
        public string RunningInboundMaster(Guid _wh_master_id, string _reference_number)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                string result = string.Empty;

                result = (from rows in this._Model.t_wms_inbound_master
                          where rows.wh_master_id == _wh_master_id
                          && rows.reference_number == _reference_number
                          select rows.inbound_order_number).Max();

                int running = 0;
                string inbound_running = string.Empty;

                if (result != null)
                {
                    //running
                    string[] arr = result.Split('-');

                    try
                    {
                        // check ว่ามี '-' หรือป่าว
                        if (arr.Length > 0)
                        {
                            running = Convert.ToInt32(arr.Last());
                            running += 1;

                            for (int i = 0; i < arr.Length; i++)
                            {
                                if (i != arr.Length - 1)
                                {
                                    inbound_running += arr[i];
                                    inbound_running += "-";
                                }
                                else
                                {
                                    inbound_running += running.ToString().PadLeft(arr.Last().Length, '0');
                                }
                            }

                            //inbound_running = _reference_number + "-" + running.ToString().PadLeft(3, '0');

                            return inbound_running;
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                    catch (Exception)
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    inbound_running = _reference_number + "-001";
                    return inbound_running;
                }
            });

        }
        #endregion
    }
}
