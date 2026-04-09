using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class ItemCategory : AEntityFormCommand<t_wms_category>
    {
        #region ++INSTANCE STATIC++
        public static ItemCategory Instance
        {
            get
            {
                using (ItemCategory _Instance = new ItemCategory())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public WMSEntities _Model { get; set; }

        public ItemCategory()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_category; };

        }

        public override bool ValidateSaveNew(t_wms_category ent, ref string msg_validate)
        {
            if (_Model.t_wms_category.Any(x => x.item_category == ent.item_category))
            {
                msg_validate = "! Code has in system.";
                return false;
            }
            else
                return true;
        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_category
                         select new
                         {
                             KeyId = rows.category_id,
                             rows.item_category,
                             rows.description,
                             rows.is_active,
                             rows.create_by,
                             rows.create_date
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        public override bool DeleteById(object _Id)
        {
            return this._Model.Delete(this, delegate ()
            {
                Guid category_id = (Guid)_Id;
                var ent = this._Model.t_wms_category.Where(w => w.category_id == category_id).FirstOrDefault();
                if (ent != null)
                {
                    var entDet = this._Model.t_wms_sub_category.Where(w => w.category_id == ent.category_id);
                    this._Model.t_wms_sub_category.RemoveRange(entDet);

                    this._Model.t_wms_category.Remove(ent);
                }

                return this._Model.SaveChanges();
            });
        }

        #endregion

        #region Query Property
        public IQueryable<Property> GetQuery()
        {
            var result = from rows in _Model.t_wms_category
                         where rows.is_active == "YES"
                         orderby rows.item_category ascending
                         select new Property
                         {
                             guid_member = rows.category_id,
                             display_member = rows.item_category + ":" + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQuery_Owner(List<Guid> _list_owner_id)
        {
            var result = from rows in _Model.t_wms_category
                         where rows.is_active == "YES"
                         && rows.t_wms_item.Any(a => _list_owner_id.Contains(a.owner_id))
                         orderby rows.item_category ascending
                         select new Property
                         {
                             guid_member = rows.category_id,
                             display_member = rows.item_category + ":" + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQuery_UserOwner()
        {
            string user_id = _SessionVals.UserName;
            List<Guid> listOwner = this._Model.t_wms_owner_user.Where(w => w.user_id == user_id).Select(s => s.owner_id).ToList();

            return GetQuery_Owner(listOwner);
        }

        public IQueryable<Property> GetQuery(Guid _item_master_id)
        {
            var result = from rows in _Model.t_wms_category
                         where rows.is_active == "YES"
                         && rows.t_wms_item.Any(a => a.item_master_id == _item_master_id)
                         orderby rows.item_category ascending
                         select new Property
                         {
                             guid_member = rows.category_id,
                             display_member = rows.item_category + ":" + rows.description
                         };

            return result;
        }

        public IQueryable<Property> GetQueryByOwner(Guid _owner_id)
        {
            var result = from rows in _Model.t_wms_category
                         where rows.is_active == "YES"
                         && rows.t_wms_item.Any(a => a.owner_id == _owner_id)
                         orderby rows.item_category ascending
                         select new Property
                         {
                             guid_member = rows.category_id,
                             display_member = rows.item_category + ":" + rows.description
                         };

            return result;
        }
        #endregion
    }
}
