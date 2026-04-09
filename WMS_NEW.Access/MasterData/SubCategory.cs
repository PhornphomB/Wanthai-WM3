using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS_NEW.Source;
using Prototype.Providers;

namespace WMS_NEW.Access.MasterData
{
    public class SubCategory : AEntityFormCommand<t_wms_sub_category>
    {
        #region ++INSTANCE STATIC++
        public static SubCategory Instance
        {
            get
            {
                using (SubCategory _Instance = new SubCategory())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public WMSEntities _Model { get; set; }

        public SubCategory()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_sub_category; };
        }

        public override bool ValidateSaveNew(t_wms_sub_category ent, ref string msg_validate)
        {
            if (_Model.t_wms_sub_category.Any(x => x.sub_category == ent.sub_category && x.category_id == ent.category_id))
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
            var result = from rows in this._Model.t_wms_sub_category
                         select new
                         {
                             KeyId = rows.sub_category_id,
                             rows.category_id,
                             rows.sub_category,
                             rows.description,
                             rows.is_active,
                             rows.create_by,
                             rows.create_date,
                             rows.inf_storage,
                             rows.update_by,
                             rows.update_date
                         };

            if (this.FilterCustom == null)
                return result.Where(x => false);

            var entCate = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "category_id");
            if (entCate != null && entCate.Value != null)
            {
                Guid category_id = (Guid)entCate.Value;
                result = result.Where(w => w.category_id == category_id);
            }

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


        public IQueryable<Property> GetQueryProperty(Guid category_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_sub_category
                             where rows.is_active == "YES" && rows.category_id == category_id
                             && rows.t_wms_category.is_active == "YES"
                             orderby rows.sub_category
                             select new Property
                             {
                                 guid_member = rows.sub_category_id,
                                 display_member = rows.sub_category
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
