using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData.Mapping
{
    public class WarehouseItem : AEntityFormCommand<t_wms_wh_item>
    {
        #region ++INSTANCE STATIC++
        public static WarehouseItem Instance
        {
            get
            {
                using (WarehouseItem _Instance = new WarehouseItem())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        public WMSEntities _Model { get; set; }

        public WarehouseItem()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_wh_item; };
        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model. v_wms_mapping_warehouse_item
                         select new
                         {
                             KeyId = rows.key_id,
                             rows.wh_id,
                             rows.wh_master_id,
                             rows.item_number,
                             rows.owner_id,
                           //  rows.owner_name,
                             rows.owner_code,
                            // rows.item_master_id,
                             rows.is_active,
                             map_status = rows.is_active.Value ? "YES" : "NO",
                             rows.create_date,
                             rows.create_by
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion

        #region Function

        public bool SaveMapping(List<ConfigGlobal.DTO._Global.KeySelect> _listKey, string _create_by)
        {
            try
            {
                string select_warehouse_id = string.Empty;
                if (_listKey.Count > 0)
                {
                    select_warehouse_id = _listKey.FirstOrDefault().KeyId.ToString().Split('_')[0];
                }
                var _queryHasInDB = this._Model.t_wms_wh_item.Where(rows => rows.t_wms_wh.wh_id == select_warehouse_id).Select(s => s.t_wms_wh.wh_id + "_" + s.t_wms_item.item_master_id.ToString());

                var listSelect = _listKey.Where(w => w.Active == true).Select(s => s.KeyId.ToString()).ToList();

                List<string> listUnselect = new List<string>();
                listUnselect = _listKey.Where(w => w.Active == false).Select(s => s.KeyId.ToString()).ToList();


                //Insert
                var listExcept = listSelect.Except(_queryHasInDB).AsEnumerable();
                var result = from rows in this._Model.v_wms_mapping_warehouse_item
                             where listExcept.Contains(rows.key_id)
                             select rows;

                //Insert
                t_wms_wh_item entIns;
                foreach (var item in result)
                {
                    entIns = new t_wms_wh_item();
                    entIns.wh_item_master_id = Guid.NewGuid();
                    entIns.wh_master_id = item.wh_master_id;
                    entIns.item_master_id = item.item_master_id;
                    entIns.interface_to_host = "YES";
                    entIns.is_active = "YES";
                    entIns.create_by = _create_by;
                    entIns.create_date = DateTime.Now;

                    this._Model.t_wms_wh_item.Add(entIns);
                }

                //Delete
                if (listUnselect.Count > 0)
                {
                    var entDel = from rows in this._Model.t_wms_wh_item
                                 where listUnselect.Contains(rows.t_wms_wh.wh_id + "_" + rows.t_wms_item.item_master_id.ToString())
                                 select rows;

                    this._Model.t_wms_wh_item.RemoveRange(entDel);
                }

                return this.GridObjContext.Save(this, delegate ()
                {
                    return _Model.SaveChanges();
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public IQueryable<Property> GetQuery(Guid _wh_master_id)
        {
            var result = from rows in _Model.t_wms_wh_item
                         where rows.is_active == "YES"
                         && rows.wh_master_id == _wh_master_id
                         orderby rows.t_wms_item.item_number ascending
                         select new Property
                         {
                             guid_member = rows.wh_item_master_id,
                             display_member = rows.t_wms_item.item_number + ":" + rows.t_wms_item.description
                         };

            return result;
        }

        public IQueryable<Property> GetQuery(Guid _wh_master_id, Guid _owner_id)
        {
            var result = from rows in _Model.t_wms_wh_item
                         where rows.is_active == "YES"
                         && rows.wh_master_id == _wh_master_id
                         && rows.t_wms_item.owner_id == _owner_id
                         && rows.t_wms_item.is_active == "YES"
                         orderby rows.t_wms_item.item_number ascending
                         select new Property
                         {
                             guid_member = rows.wh_item_master_id,
                             display_member = rows.t_wms_item.item_number + ":" + rows.t_wms_item.description
                         };

            return result;
        }


        public IQueryable<Property> GetQuery_Inbound(Guid _inbound_order_master_id)
        {

            var result = (from rows in _Model.t_wms_inbound_detail
                          where rows.inbound_order_master_id == _inbound_order_master_id
                          && ((rows.quantity_order - rows.quantity_receive) > 0 || rows.over_receipt_allowed == "YES")
                          orderby rows.t_wms_wh_item.t_wms_item.item_number ascending
                          select new Property
                          {
                              guid_member = rows.wh_item_master_id,
                              display_member = rows.t_wms_wh_item.t_wms_item.item_number + ":" + rows.t_wms_wh_item.t_wms_item.description
                          }).Distinct();

            return result;
        }
    }
}
