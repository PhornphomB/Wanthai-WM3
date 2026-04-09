using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Inventory
{
    public class InventoryStatus : IDisposable
    {
        public WMSEntities _Model { get; set; }

        public InventoryStatus()
        {
            this._Model = new WMSEntities();
        }


        #region ++INSTANCE STATIC++
        public static InventoryStatus Instance
        {
            get
            {
                using (InventoryStatus _Instance = new InventoryStatus())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        public IQueryable<Property> GetQuery()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_inventory_status
                             where rows.is_active == "YES"
                             orderby rows.sequence ascending
                             select new Property
                             {
                                 guid_member = rows.inventory_status_id,
                                 display_member = rows.inv_status
                             };

                return result;
            });
        }

        public IQueryable<Property> GetQueryCode()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_inventory_status
                             where rows.is_active == "YES"
                             orderby rows.sequence ascending
                             select new Property
                             {
                                 Code = rows.inv_status,
                                 Name = rows.inv_status
                             };

                return result;
            });
        }

        public IQueryable<Property> GetQuery_Inbound()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_inventory_status

                             join rule in this._Model.t_wms_rule
                             on new { rows.inv_status, rule_code = "RULE_STATUS_INBOUND_DEFAULT" }
                             equals new { inv_status = rule.value, rule.rule_code } into tmp_rule
                             from _rule in tmp_rule.DefaultIfEmpty()

                             where rows.is_active == "YES"

                             orderby _rule.sequence ascending
                             select new Property
                             {
                                 guid_member = rows.inventory_status_id,
                                 display_member = rows.inv_status
                             };

                return result;
            });
        }


        public IQueryable<Property> GetQueryCode_Inbound()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_inventory_status

                             join rule in this._Model.t_wms_rule
                             on new { rows.inv_status, rule_code = "RULE_STATUS_INBOUND_DEFAULT" }
                             equals new { inv_status = rule.value, rule.rule_code } into tmp_rule
                             from _rule in tmp_rule.DefaultIfEmpty()

                             where rows.is_active == "YES"

                             orderby _rule.sequence ?? 99 ascending
                             select new Property
                             {
                                 Code = rows.inv_status,
                                 Name = rows.inv_status
                             };

                return result;
            });
        }


        public IQueryable<Property> GetQueryCode_Outbound()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_inventory_status

                             join rule in this._Model.t_wms_rule
                             on new { rows.inv_status, rule_code = "RULE_STATUS_OUTBOUND_DEFAULT" }
                             equals new { inv_status = rule.value, rule.rule_code } into tmp_rule
                             from _rule in tmp_rule.DefaultIfEmpty()

                             where rows.is_active == "YES"

                             orderby _rule.sequence ?? 99 ascending
                             select new Property
                             {
                                 Code = rows.inv_status,
                                 Name = rows.inv_status
                             };

                return result;
            });
        }

        public IQueryable<Property> GetQueryProperty_Rule()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var invStatus = from rows in this._Model.t_wms_rule
                                where rows.is_active == "YES" && rows.rule_code == "INVENTORY_CHANGE_NOT_IN_STATUS"
                                orderby rows.value
                                select rows.value;

                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_inventory_status

                             join rule in this._Model.t_wms_rule
                             on new { rows.inv_status, rule_code = "RULE_STATUS_INBOUND_DEFULT" }
                             equals new { inv_status = rule.value, rule.rule_code } into tmp_rule
                             from _rule in tmp_rule.DefaultIfEmpty()

                             where !invStatus.Contains(rows.inv_status)
                             && rows.is_active == "YES"

                             orderby rows.sequence ascending
                             select new Property
                             {
                                 Code = rows.inv_status,
                                 Name = rows.inv_status
                             };

                return result;
            });
        }

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
