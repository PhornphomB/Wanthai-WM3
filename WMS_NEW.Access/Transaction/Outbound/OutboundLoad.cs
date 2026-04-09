using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Objects.SqlClient;

using Prototype.Providers;
using WMS_NEW.Source;


namespace WMS_NEW.Access.Transaction.Outbound
{
    public class OutboundLoad : AGridObjectSourceQuery
    {
        public class OrderTypeCustomer
        {
            public string order_type { get; set; }
            public Guid? customer_id { get; set; }
        }


        #region ++INSTANCE STATIC++
        public static OutboundLoad Instance
        {
            get
            {
                using (OutboundLoad _Instance = new OutboundLoad())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public WMSEntities _Model { get; set; }

        public OutboundLoad()
        {
            this._Model = new WMSEntities();
            base.GridObjContext = _Model;
        }


        public bool UpdateOrderLoad(string _wh_id, string _owner_code, string _load_id, Guid _order_master_id, bool _is_add)
        {
            //var chk = this._Model.Existed(this, delegate ()
            //{
            //    if (this._Model.t_wms_outbound_master.Any(qry => qry.wh_id == _wh_id && qry.owner_code == _owner_code && qry.load_id == _load_id && qry.order_status != "OPEN"))
            //        return true;
            //    else
            //        return false;
            //}, "! Cannot update load order, bacause some order in group load not Status [Open]");

            //if (chk)
            //    return false;


            if (_is_add)
            {
                var dto_ord = this._Model.t_wms_outbound_master.Select(se => new { se.outbound_order_master_id, se.outbound_order_number, se.order_type, se.customer_id })
                                                .Single(x => x.outbound_order_master_id == _order_master_id);


                var rule_not_mix = this._Model.t_wms_rule.Any(wh => wh.rule_code == "CREATE_OUTBOUND_GROUP_MIX_ORDER_TYPE" && wh.value == "NO");
                if (rule_not_mix)
                {
                    var chk_mix_order_type = this._Model.Existed(this, delegate ()
                    {
                        var has_mix_ord_type = this._Model.t_wms_outbound_master.Any(qry => qry.wh_id == _wh_id && qry.owner_code == _owner_code
                                                                                         && qry.load_id == _load_id && qry.order_type != dto_ord.order_type);
                        if (has_mix_ord_type)
                            return true;
                        else
                            return false;
                    }, "! Cannot mix type Order [" + dto_ord.outbound_order_number + "], Type [" + dto_ord.order_type + "] in load order.");

                    if (chk_mix_order_type)
                        return false;
                }


                var chk_picklist_mix_customer = this._Model.Existed(this, delegate ()
                {
                    var listMix = this._Model.t_wms_outbound_master.Where(qry => qry.wh_id == _wh_id && qry.owner_code == _owner_code && qry.load_id == _load_id)
                                                                        .Select(se => new OrderTypeCustomer
                                                                        {
                                                                            order_type = se.order_type,
                                                                            customer_id = se.customer_id
                                                                        }).ToList();

                    listMix.Add(new OrderTypeCustomer { customer_id = dto_ord.customer_id, order_type = dto_ord.order_type });

                    var has_type_picklist = listMix.Any(x => x.order_type.ToUpper().Contains("PICKING LIST"));
                    var enum_customers = listMix.Select(se => se.customer_id).Distinct();

                    if (has_type_picklist && enum_customers.Count() > 1)
                        return true;
                    else
                        return false;
                }, "! Load order has Order Type [PO-Picking List] not allow mix customers.");

                if (chk_picklist_mix_customer)
                    return false;
            }


            var saved = this._Model.Update(this, delegate ()
            {
                var ent = this._Model.t_wms_outbound_master.FirstOrDefault(qry => qry.outbound_order_master_id == _order_master_id && (qry.order_status == "OPEN" || qry.order_status == "RELEASE"));
                if (ent != null)
                {
                    if (_is_add)
                    {
                        ent.load_id = _load_id;
                    }
                    else
                    {
                        ent.load_id = null;
                        ent.pick_type = ent.order_status == "RELEASE" ? "SYSTEM" : ent.pick_type;
                    }
                        
                }

                return this._Model.SaveChanges();
            });

            return saved;
        }


        public IQueryable<Property> GetQuery(string _wh_id, string _owner_code)
        {
            string[] order_type = (from rows in this._Model.t_wms_rule
                                   where rows.is_active == "YES" && rows.rule_code == "RULE_CHECKER_ORDER_TYPE"
                                   select rows.value).ToArray();

            var result = from rows in _Model.t_wms_outbound_master
                         where rows.wh_id == _wh_id && rows.owner_code == _owner_code && string.IsNullOrEmpty(rows.load_id) && rows.order_status == "OPEN"
                         && order_type.Contains(rows.order_type)
                         orderby rows.outbound_order_number
                         select new Property
                         {
                             guid_member = rows.outbound_order_master_id,
                             display_member = rows.outbound_order_number
                         };

            return result;
        }

        public string GetLoadStatus(string _wh_id, string _owner_code, string _load_id)
        {
            var result = (from rows in _Model.v_wms_outbound_rel_by_load
                          where rows.wh_id == _wh_id && rows.owner_code == _owner_code && rows.load_id == _load_id
                          select rows.load_status).FirstOrDefault();

            return result;
        }
        public List<string> GetOrderStatusList(string _wh_id, string _owner_code, string _load_id)
        {
            var result = (from rows in _Model.t_wms_outbound_master
                          where rows.wh_id == _wh_id && rows.owner_code == _owner_code && rows.load_id == _load_id
                          select rows.order_status).ToList();

            return result;
        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_outbound_master
                         select new
                         {
                             KeyId = rows.outbound_order_master_id,
                             unrel = "",
                             allow_unrel = rows.order_status == "RELEASE" ? true : false,
                             allow_update = (rows.order_status == "OPEN" || rows.order_status == "RELEASE") ? true : false,
                             rows.t_wms_wh.wh_id,
                             rows.t_wms_owner.owner_code,
                             rows.outbound_order_number,
                             rows.load_id,
                             rows.order_type,
                             rows.order_date,
                             rows.order_status,
                             rows.customer_id,
                             rows.t_wms_customer.customer_name
                         };


            if (this.FilterCustom == null)
                return result.Where(x => false);

            var entWh = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_wh_id");
            if (entWh != null)
            {

                var wh_id = (string)entWh.Value;
                var owner_code = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_owner_code").Value;
                var loadId = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_loadId").Value;

                result = result.Where(x => x.wh_id == wh_id && x.owner_code == owner_code && x.load_id == loadId);
            }

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion
    }
}
