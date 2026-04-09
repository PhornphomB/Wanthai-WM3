using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData.ModernTrade
{
    public class ItemShelfLife : AEntityFormCommand<t_wms_shelf_life>
    {
        #region ++INSTANCE STATIC++
        public static ItemShelfLife Instance
        {
            get
            {
                using (ItemShelfLife _Instance = new ItemShelfLife())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        protected WMSEntities _Model { get; set; }

        public ItemShelfLife()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_shelf_life; };
        }

        public override bool ValidateSaveNew(t_wms_shelf_life ent, ref string msg_validate)
        {
            if (_Model.t_wms_shelf_life.Any(x => x.customer_id == ent.customer_id && x.category_id == ent.category_id && x.item_master_id == ent.item_master_id))
            {
                msg_validate = "! This data has already exists";
                return false;
            }
            else
                return true;
        }

        public override bool ValidateSaveUpdate(t_wms_shelf_life ent, ref string msg_validate)
        {
            if (_Model.t_wms_shelf_life.Any(x => x.customer_id == ent.customer_id && 
                        x.category_id == ent.category_id && 
                        x.item_master_id == ent.item_master_id &&
                        x.shelf_life_id != ent.shelf_life_id
                        ))
            {
                msg_validate = "! This data has already exists";
                return false;
            }
            else
                return true;
        }



        #region Inherit IEntityCommandForm

        public override void SetOptionalSaveNew(t_wms_shelf_life ent)
        {
            if (Entity.category_id != null)
            {
                var items = _Model.t_wms_item.Where(x => x.category_id == ent.category_id).ToList();

                t_wms_shelf_life detail;

                foreach (var item in items)
                {
                    if (!_Model.t_wms_shelf_life.Any(x => x.customer_id == ent.customer_id && x.item_master_id == item.item_master_id))
                    {
                        var customer = _Model.t_wms_customer.Where(x => x.customer_id == ent.customer_id).FirstOrDefault();
                        var owner = _Model.t_wms_owner.Where(x => x.owner_id == ent.owner_id).FirstOrDefault();

                        detail = new t_wms_shelf_life();
                        detail.shelf_life_id = Guid.NewGuid();
                        detail.wh_master_id = ent.wh_master_id;
                        detail.owner_id = ent.owner_id;
                        detail.customer_id = ent.customer_id;
                        detail.customer_name = customer.customer_name; //
                        detail.category_id = ent.category_id;
                        detail.item_master_id = item.item_master_id;
                        detail.owner_code = owner.owner_code; //
                        detail.shelf_life_day_remaining = ent.shelf_life_day_remaining;

                        detail.create_by = ent.create_by;
                        detail.create_date = ent.create_date;

                        this._Model.t_wms_shelf_life.Add(detail);
                    }
                }
            }
        }

        public override bool Save()
        {
            var msg_validate = string.Empty;
            var is_valid = ValidateSaveNew(Entity, ref msg_validate);

            if (this.GridObjContext.Engaged(this, () => { return !is_valid; }, msg_validate))
                return false;

            return this.GridObjContext.Save(this, delegate ()
            {
                if (Entity.item_master_id == Guid.Empty)
                {
                    SetOptionalSaveNew(Entity);
                }
                else
                {
                    var keys = this.GridObjContext.GetEntityKeys(Entity.GetType());
                    foreach (var key in keys)
                    {
                        if (key.TypeName.ToLower() == "guid")
                        {
                            Entity.SetPropertyValue(key.Name, Guid.NewGuid());
                        }
                    }

                    var customer = _Model.t_wms_customer.Where(x => x.customer_id == Entity.customer_id).FirstOrDefault();
                    var owner = _Model.t_wms_owner.Where(x => x.owner_id == Entity.owner_id).FirstOrDefault();
                    Entity.customer_name = customer.customer_name;
                    Entity.owner_code = owner.owner_code;

                    this.GridObjContext.Entry(Entity).State = System.Data.Entity.EntityState.Added;
                }

                return this.GridObjContext.SaveChanges();
            });
        }
        #endregion


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

            var result = from rows in this._Model.t_wms_shelf_life
                         join item in this._Model.t_wms_item on rows.item_master_id equals item.item_master_id
                         join cate in this._Model.t_wms_category on rows.category_id equals cate.category_id into joined_cate
                         from category in joined_cate.DefaultIfEmpty()
                         where rows.t_wms_wh.t_wms_wh_user.Any(qry => qry.user_id.ToUpper().Trim() == user_id)
                         select new
                         {
                             KeyId = rows.shelf_life_id,
                             rows.wh_master_id,
                             rows.t_wms_wh.wh_id,
                             rows.customer_id,

                             rows.category_id,

                             rows.owner_id,
                             agency = rows.owner_code,
                             rows.customer_name,
                             //category.item_category,
                             category_description = category.description,
                             rows.item_master_id,
                             item.item_number,
                             day_remaining = rows.shelf_life_day_remaining,
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
            var result = from rows in _Model.t_wms_shelf_life
                             //where rows.is_active == "YES"
                         orderby rows.t_wms_customer.customer_code ascending
                         select new Property
                         {
                             guid_member = rows.customer_id,
                             display_member = rows.t_wms_customer.customer_code + " : " + rows.customer_name
                         };

            return result;
        }

        public IQueryable<Property> GetQuery(Guid _customer_id)
        {
            var result = from rows in _Model.t_wms_shelf_life
                         where rows.customer_id == _customer_id
                         orderby rows.t_wms_customer.customer_code ascending
                         select new Property
                         {
                             guid_member = rows.customer_id,
                             display_member = rows.t_wms_customer.customer_code + " : " + rows.customer_name
                         };

            return result;
        }

        public IQueryable<Property> GetQueryCode()
        {
            var result = from rows in _Model.t_wms_shelf_life
                             //where rows.is_active == "YES"
                         orderby rows.t_wms_customer.customer_code ascending
                         select new Property
                         {
                             value_member = rows.t_wms_customer.customer_code,
                             display_member = rows.t_wms_customer.customer_code + " : " + rows.customer_name
                         };

            return result;
        }

        #endregion
    }
}
