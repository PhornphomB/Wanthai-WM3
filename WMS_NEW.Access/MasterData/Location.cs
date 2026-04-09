using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class Location : AEntityFormCommand<t_wms_location>
    {
        #region ++INSTANCE STATIC++
        public static Location Instance
        {
            get
            {
                using (Location _Instance = new Location())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        protected WMSEntities _Model { get; set; }

        public Location()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_location; };
        }

        public override bool ValidateSaveNew(t_wms_location ent, ref string msg_validate)
        {
            if (_Model.t_wms_location.Any(x => x.location == ent.location && x.wh_master_id == ent.wh_master_id))
            {
                msg_validate = "! Location and warehouse has in system.";
                return false;
            }
            else
                return true;
        }

        public override bool Save()
        {
            this.Entity.override_receive_date = "NO";
            return base.Save();
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            string user_id = string.Empty;
            if (this.FilterCustom != null)
            {
                var entU = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "user_id");
                if (entU != null)
                {
                    user_id = entU.Value.ToString().ToUpper().Trim();
                }
            }

            var result = from rows in this._Model.t_wms_location

                         join cboitem in this._Model.t_com_combobox_item
                         on rows.loc_type equals cboitem.value_member into CBOITEM
                         from leftCboItem in CBOITEM.DefaultIfEmpty()

                         where leftCboItem.group_name == "mst_location_type"
                         && rows.t_wms_wh.t_wms_wh_user.Any(qry => qry.user_id.ToUpper().Trim() == user_id)
                         select new
                         {
                             KeyId = rows.location_id,
                             rows.t_wms_wh.wh_id,
                             rows.wh_master_id,
                             rows.location,
                             rows.loc_type,
                             loc_type_name = leftCboItem.display_member,
                             rows.description,
                             rows.capacity_qty,
                             rows.is_active,
                             rows.create_by,
                             rows.create_date,
                             rows.x_cordinate,
                             rows.y_cordinate,
                             rows.color_code,
                             rows.is_full_pallet
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion

        #region Query Property

        public IQueryable<Property> GetQuery(Guid _wh_master_id)
        {
            var result = from rows in _Model.t_wms_location
                         where rows.is_active == "YES" && rows.wh_master_id == _wh_master_id
                         orderby rows.location ascending
                         select new Property
                         {
                             guid_member = rows.location_id,
                             display_member = rows.location + " : " + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQueryCode()
        {
            var result = from rows in _Model.t_wms_location
                         where rows.is_active == "YES"
                         orderby rows.location ascending
                         select new Property
                         {
                             value_member = rows.location,
                             display_member = rows.location + " : " + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQuery_Warehouse(Guid _wh_master_id)
        {
            var result = from rows in _Model.t_wms_location
                         where rows.is_active == "YES"
                         && rows.wh_master_id == _wh_master_id
                         orderby rows.location ascending
                         select new Property
                         {
                             guid_member = rows.location_id,
                             display_member = rows.location + " : " + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQuery_Warehouse(Guid[] _list_wh_master_id)
        {
            var result = from rows in _Model.t_wms_location
                         where rows.is_active == "YES"
                         && _list_wh_master_id.Contains(rows.wh_master_id)
                         orderby rows.location ascending
                         select new Property
                         {
                             guid_member = rows.location_id,
                             display_member = rows.location + " : " + rows.description
                         };

            return result.Distinct();
        }

        public IQueryable<Property> GetCodeQueryWarehouse(Guid[] _list_wh_master_id)
        {
            var result = from rows in _Model.t_wms_location
                         where rows.is_active == "YES"
                         && _list_wh_master_id.Contains(rows.wh_master_id)
                         orderby rows.location ascending
                         select new Property
                         {
                             value_member = rows.location,
                             display_member = rows.location + " : " + rows.description
                         };

            return result.Distinct();
        }

        public IQueryable<Property> GetCodeQueryWarehouseLocNotMove(Guid[] _list_wh_master_id)
        {
            var listLoc = this._Model.t_wms_rule.Where(w => w.rule_code == "LOCATION_TYPE_NOT_MOVE_ITEM" && w.is_active == "YES").Select(s => s.value).ToList();

            var result = from rows in _Model.t_wms_location
                         where rows.is_active == "YES"
                         && _list_wh_master_id.Contains(rows.wh_master_id)
                         && !listLoc.Contains(rows.loc_type)
                         orderby rows.location ascending
                         select new Property
                         {
                             value_member = rows.location,
                             display_member = rows.location + " : " + rows.description
                         };

            return result.Distinct();
        }

        public IQueryable<Property> GetQuery_Change(Guid _wh_master_id)
        {
            List<string> listLocType = new List<string>();
            listLocType = this._Model.t_wms_rule.Where(w => w.rule_code == "LOCATION_TYPE_NOT_MOVE_TO_DESTINATION" && w.is_active == "YES").Select(s => s.value).ToList();


            var result = from rows in _Model.t_wms_location
                         where rows.is_active == "YES"
                         && rows.wh_master_id == _wh_master_id
                         && !listLocType.Contains(rows.loc_type)
                         orderby rows.location ascending
                         select new Property
                         {
                             guid_member = rows.location_id,
                             display_member = rows.location + " : " + rows.description
                         };

            return result;
        }




        public IQueryable<Property> GetQuery_UserWarehouse()
        {
            string user_id = _SessionVals.UserName;
            Guid[] listWh = this._Model.t_wms_wh_user.Where(w => w.user_id == user_id).Select(s => s.wh_master_id).ToArray();

            return GetQuery_Warehouse(listWh);
        }

        public IQueryable<Property> GetCodeQueryUserWarehouse()
        {
            string user_id = _SessionVals.UserName;
            Guid[] listWh = this._Model.t_wms_wh_user.Where(w => w.user_id == user_id).Select(s => s.wh_master_id).ToArray();

            return GetCodeQueryWarehouse(listWh);
        }

        public IQueryable<Property> GetCodeQueryUserWarehouseRuleLocNotMove()
        {
            string user_id = _SessionVals.UserName;
            Guid[] listWh = this._Model.t_wms_wh_user.Where(w => w.user_id == user_id).Select(s => s.wh_master_id).ToArray();

            return GetCodeQueryWarehouseLocNotMove(listWh);
        }


        public IQueryable<Property> GetQuery_Receipt(Guid _wh_master_id)
        {
            return GetQuery_Rule(_wh_master_id, "RULE_GET_INBOUND_RECEIPT_LOCATION");
        }

        public IQueryable<Property> GetQuery_Rule(Guid _wh_master_id, string _rule_code)
        {
            List<string> listRule = this._Model.t_wms_rule.Where(w => w.rule_code == _rule_code && w.is_active == "YES").Select(s => s.value.ToString()).ToList();

            var result = from rows in _Model.t_wms_location
                         where rows.is_active == "YES"
                         && rows.wh_master_id == _wh_master_id
                         && listRule.Contains(rows.loc_type)
                         orderby rows.putaway_sequence ascending, rows.location ascending
                         select new Property
                         {
                             guid_member = rows.location_id,
                             display_member = rows.location + " : " + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQuery(Guid _wh_master_id, string _loc_type)
        {
            var result = from rows in _Model.t_wms_location
                         where rows.is_active == "YES"
                         && rows.wh_master_id == _wh_master_id
                         && rows.loc_type == _loc_type
                         orderby rows.location ascending
                         select new Property
                         {
                             guid_member = rows.location_id,
                             display_member = rows.location + " : " + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetLocationNameByLocType(Guid _wh_master_id, List<string> _listLocType)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_location
                             where rows.is_active == "YES" && rows.wh_master_id == _wh_master_id && _listLocType.Contains(rows.loc_type)
                             orderby rows.location
                             select new Property
                             {
                                 value_member = rows.location,
                                 display_member = rows.location
                             };

                return result;
            });
        }

        public IQueryable<Property> GetLocationNameByLocType_Customer(Guid _wh_master_id, List<string> _listLocType, Guid _customer_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_customer_location
                             let loc = rows.t_wms_location
                             where rows.is_active == "YES" && loc.wh_master_id == _wh_master_id && _listLocType.Contains(loc.loc_type) && rows.customer_id == _customer_id
                             orderby loc.location
                             select new Property
                             {
                                 value_member = loc.location,
                                 display_member = loc.location
                             };

                return result;
            });
        }

        public IQueryable<Property> GetQueryPropertyLocationLevel()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = (from rows in this._Model.t_wms_location
                              where rows.is_active == "YES" && !string.IsNullOrEmpty(rows.location_level)
                              orderby rows.location
                              select new Property
                              {
                                  Code = rows.location_level,
                                  Name = rows.location_level
                              }).Distinct().OrderBy(q => q.Code);
                return result;
            });
        }

        public IQueryable<Property> GetQuery_WarehouseZone(Guid _wh_master_id, string _zone)
        {
            var result = from rows in _Model.t_wms_zone_location
                         let loc = rows.t_wms_location
                         where rows.is_active == "YES"
                         && loc.wh_master_id == _wh_master_id && rows.t_wms_zone.zone == _zone
                         orderby loc.location ascending
                         select new Property
                         {
                             guid_member = rows.location_id,
                             display_member = loc.location + " : " + loc.description
                         };

            return result;
        }


        public IQueryable<Property> GetQuery_OutboundDetailRule(Guid _wh_master_id, Guid? _item_master_id, string _rule_code)
        {
            List<string> listRule = this._Model.t_wms_rule.Where(w => w.rule_code == _rule_code && w.is_active == "YES").Select(s => s.value.ToString()).ToList();

            var result = (from rows in _Model.v_wms_inventory_data
                          where rows.wh_master_id == _wh_master_id
                          && listRule.Contains(rows.loc_type)
                          && rows.item_master_id == _item_master_id
                          //&& _Model.t_wms_location_item.Any(a=>a.item_master_id == _item_master_id)
                          orderby rows.location ascending
                          select new Property
                          {
                              guid_member = rows.location_id,
                              display_member = rows.location + " : " + rows.description
                          }).Distinct();

            return result;
        }

        #endregion

        #region Function

        public bool CheckLocation_Inventory(Guid _location_id)
        {
            return this._Model.t_wms_inventory.Any(a => a.location_id == _location_id);
        }

        public bool Control_LPN(Guid _location_id)
        {
            return this._Model.t_wms_location.Any(a => a.location_id == _location_id && a.lpn_controlled == "YES");
        }

        #endregion
    }
}
