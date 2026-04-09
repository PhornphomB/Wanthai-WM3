using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class Item : AEntityFormCommand<t_wms_item>
    {
        #region ++INSTANCE STATIC++
        public static Item Instance
        {
            get
            {
                using (Item _Instance = new Item())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        public WMSEntities _Model { get; set; }

        public Item()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_item; };

        }

        public override bool ValidateSaveNew(t_wms_item ent, ref string msg_validate)
        {
            if (_Model.t_wms_item.Any(x => x.item_number == ent.item_number && x.owner_id == ent.owner_id))
            {
                msg_validate = "! Item Number and Owner ID has in system.";
                return false;
            }
            if (ent.days_to_expire == 0)
            {
                msg_validate = "! [days_to_expire] must be entered with a value greater than 0.";
                return false;
            }
            else
                return true;
        }
        public override bool ValidateSaveUpdate(t_wms_item ent, ref string msg_validate)
        {
            if (ent.days_to_expire == 0)
            {
                msg_validate = "! [days_to_expire] must be entered with a value greater than 0.";
                return false;
            }
            else
                return true;
        }
        public bool IsControlSerial(Guid _item_master_id)
        {
            return this._Model.t_wms_item.Any(a => a.item_master_id == _item_master_id && a.sn_control == "FULL");
        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_item
                         where rows.t_wms_owner.t_wms_owner_user.Any(a => a.user_id == _SessionVals.UserName && a.is_active == "YES")
                         select new
                         {
                             KeyId = rows.item_master_id,
                             rows.owner_id,
                             rows.t_wms_owner.owner_code,
                             rows.item_number,
                             rows.description,
                             rows.is_bom,
                             rows.is_active,
                             rows.create_by,
                             rows.create_date,
                             rows.t_wms_category.item_category,
                             category_description = rows.t_wms_category.description,
                             rows.t_wms_category.category_id,
                             rows.lot_control,
                             rows.expiry_date_control,
                             rows.sn_control
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


        #region Query Property

        public IQueryable<Property> GetQuery()
        {
            var listWh = _Model.t_wms_wh_user.Where(w => w.user_id == _SessionVals.UserName).Select(s => s.wh_master_id);
            var listItem = _Model.t_wms_wh_item.Where(w => listWh.Contains(w.wh_master_id)).Select(s => s.item_master_id);

            var result = from rows in _Model.t_wms_item
                         where rows.is_active == "YES"
                         && listItem.Contains(rows.item_master_id)
                         orderby rows.item_number ascending
                         select new Property
                         {
                             //guid_member = rows.item_master_id,
                             value_member = rows.item_number,
                             display_member = rows.item_number + ":" + rows.description
                         };

            return result.Distinct();
        }

        public IQueryable<Property> GetQuery_Guid()
        {
            var listWh = _Model.t_wms_wh_user.Where(w => w.user_id == _SessionVals.UserName).Select(s => s.wh_master_id);
            var listItem = _Model.t_wms_wh_item.Where(w => listWh.Contains(w.wh_master_id)).Select(s => s.item_master_id);

            var result = from rows in _Model.t_wms_item
                         where rows.is_active == "YES"
                         && listItem.Contains(rows.item_master_id)
                         orderby rows.item_number ascending
                         select new Property
                         {
                             guid_member = rows.item_master_id,
                             //value_member = rows.item_master_id,
                             display_member = rows.item_number + ":" + rows.description
                         };

            return result.Distinct();
        }

        //public IQueryable<Property> GetQueryCode()
        //{
        //    var result = from rows in _Model.t_wms_item
        //                 where rows.is_active == "YES"
        //                 orderby rows.item_number ascending
        //                 select new Property
        //                 {
        //                     value_member = rows.item_number,
        //                     display_member = rows.item_number + ":" + rows.description
        //                 };

        //    return result;
        //}

        public IQueryable<Property> GetQuery_Warehouse(Guid _wh_master_id)
        {
            var result = from rows in _Model.t_wms_item
                         where rows.is_active == "YES"
                         && rows.t_wms_wh_item.Any(a => a.wh_master_id == _wh_master_id)
                         orderby rows.item_number ascending
                         select new Property
                         {
                             guid_member = rows.item_master_id,
                             display_member = rows.item_number + ":" + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQuery_Warehouse(List<Guid> _list_wh_master_id)
        {
            var result = from rows in _Model.t_wms_item
                         where rows.is_active == "YES"
                         && rows.t_wms_wh_item.Any(a => _list_wh_master_id.Contains(a.wh_master_id))
                         orderby rows.item_number ascending
                         select new Property
                         {
                             guid_member = rows.item_master_id,
                             display_member = rows.item_number + ":" + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQuery_Owner(List<Guid> _list_owner_id)
        {
            var result = from rows in _Model.t_wms_item
                         where rows.is_active == "YES"
                         && _list_owner_id.Contains(rows.owner_id)
                         orderby rows.item_number ascending
                         select new Property
                         {
                             guid_member = rows.item_master_id,
                             display_member = rows.item_number + ":" + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQuery_UserWarehouse()
        {
            string user_id = _SessionVals.UserName;
            List<Guid> listWh = this._Model.t_wms_wh_user.Where(w => w.user_id == user_id).Select(s => s.wh_master_id).ToList();

            return GetQuery_Warehouse(listWh);
        }

        public IQueryable<Property> GetQuery_UserOwner()
        {
            string user_id = _SessionVals.UserName;
            List<Guid> listOwn = this._Model.t_wms_owner_user.Where(w => w.user_id == user_id).Select(s => s.owner_id).ToList();

            return GetQuery_Owner(listOwn);
        }

        public IQueryable<Property> GetQuery(Guid _owner_id)
        {
            var result = from rows in _Model.t_wms_item
                         where rows.is_active == "YES"
                         && rows.owner_id == _owner_id
                         orderby rows.item_number ascending
                         select new Property
                         {
                             guid_member = rows.item_master_id,
                             display_member = rows.item_number + ":" + rows.description
                         };

            return result;
        }
        public IQueryable<Property> GetQuery(Guid _owner_id, Guid _category_id)
        {
            var result = from rows in _Model.t_wms_item
                         where rows.is_active == "YES"
                         && rows.owner_id == _owner_id
                         && rows.category_id == _category_id
                         orderby rows.item_number ascending
                         select new Property
                         {
                             guid_member = rows.item_master_id,
                             display_member = rows.item_number + ":" + rows.description
                         };

            return result;
        }


        public IQueryable<Property> GetQueryCode(Guid _owner_id)
        {
            var result = from rows in _Model.t_wms_item
                         where rows.is_active == "YES"
                         && rows.owner_id == _owner_id
                         orderby rows.item_number ascending
                         select new Property
                         {
                             value_member = rows.item_number,
                             display_member = rows.item_number + ":" + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQueryBom()
        {
            var result = from rows in _Model.t_wms_item
                         where rows.is_active == "YES"
                         && rows.is_bom == "YES"
                         orderby rows.item_number ascending
                         select new Property
                         {
                             guid_member = rows.item_master_id,
                             display_member = rows.item_number + ":" + rows.description
                         };

            return result;
        }
        public IQueryable<Property> GetQueryBom(Guid _owner_id)
        {
            var result = from rows in _Model.t_wms_item
                         where rows.is_active == "YES"
                         && rows.is_bom == "YES"
                         && rows.owner_id == _owner_id
                         orderby rows.item_number ascending
                         select new Property
                         {
                             guid_member = rows.item_master_id,
                             display_member = rows.item_number + ":" + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQueryPropertyWarehouse(Guid _wh_master_id, Guid owner_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = from rows in this._Model.t_wms_wh_item
                             let item = rows.t_wms_item
                             where rows.is_active == "YES"
                             && rows.wh_master_id == _wh_master_id
                             && item.owner_id == owner_id
                             orderby item.item_number
                             select new Property
                             {
                                 guid_member = rows.wh_item_master_id,
                                 display_member = item.item_number + ":" + item.description
                             };

                return result;
            });
        }

        public t_wms_wh_item GetWarehouseItem(Guid _wh_item_master_id)
        {
            return this._Model.GetDataEntityBy(this, delegate ()
            {
                var result = (from tb in _Model.t_wms_wh_item
                              where tb.wh_item_master_id == _wh_item_master_id
                              select tb).FirstOrDefault();

                //if (result != null)
                //{
                //    result.t_wms_itemReference.Load();
                //    result.t_wms_item.t_wms_item_crossref.Load();
                //}

                return result;
            });
        }

        public string GetDescriptionByWHItem(Guid _wh_item_master_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                return (from rows in this._Model.t_wms_wh_item
                        join item in _Model.t_wms_item on rows.item_master_id equals item.item_master_id
                        where rows.wh_item_master_id == _wh_item_master_id
                        select item.description).FirstOrDefault();
            });
        }

        public IQueryable<Property> GetQueryPropertyByItemBom(Guid _wh_master_id, Guid owner_id, string _is_bom = "YES")
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = from rows in this._Model.t_wms_wh_item
                             let item = rows.t_wms_item
                             where rows.is_active == "YES"
                             && item.is_bom == _is_bom
                             && rows.wh_master_id == _wh_master_id && item.owner_id == owner_id  
                             orderby item.item_number
                             select new Property
                             {
                                 guid_member = rows.wh_item_master_id,
                                 display_member = item.item_number
                             };

                return result;
            });
        }

        public string GetQtyUnitByItemMasterIDAndUom(Guid item_master_id,string uom)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                return (from rows in this._Model.t_wms_item_uom
                        where rows.item_master_id == item_master_id && rows.uom == uom
                        select rows.conversion_factor).FirstOrDefault().ToString();
            });
        }

        public string GetItemMasterIDByItemNumber(string item_name)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                return (from rows in this._Model.t_wms_item
                        where rows.item_number == item_name
                        select rows.item_master_id).FirstOrDefault().ToString();
            });
        }
        public string GetBaseUnitByItemMasterID(Guid item_master_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                return (from rows in this._Model.t_wms_item_uom
                        where rows.item_master_id == item_master_id && rows.primary_uom == "YES"
                        select rows.uom).FirstOrDefault().ToString();
            });
        }
        #endregion
    }
}
