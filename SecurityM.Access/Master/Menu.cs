using Prototype.Providers;
using SecurityM.Source;
using System;
using System.Data;
using System.Linq;

namespace SecurityM.Access.Master
{
    public class Menu : AEntityFormCommand<t_com_menu>
    {
        public SecurityM_Entities _Model { get; set; }

        public Menu()
        {
            this._Model = new SecurityM_Entities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_com_menu; };
        }


        #region ++INSTANCE STATIC++
        public static Menu Instance
        {
            get
            {
                using (Menu _Instance = new Menu())
                {
                    return _Instance;
                }
            }
        }

        #endregion


        public override void SetOptionalSaveNew(t_com_menu ent)
        {
            ent.menu_id = Guid.NewGuid().ToString();
            ent.resource_master_id = ent.menu_id;

            if (!string.IsNullOrEmpty(ent.parent_menu_id))
                ent.menu_group = _Model.t_com_menu.Select(se => new { se.menu_id, se.menu_name }).Single(x => x.menu_id == ent.parent_menu_id).menu_name;
            else
                ent.menu_group = ent.menu_name;


            var entMas = new t_com_resource_master();
            entMas.resource_master_id = ent.resource_master_id;
            entMas.app_id = ent.app_id;
            entMas.resource_name = ent.menu_name;
            entMas.description = ent.menu_name;
            entMas.default_value = ent.menu_name;
            entMas.create_date = ent.create_date;
            entMas.create_by = ent.create_by;

            this._Model.t_com_resource_master.Add(entMas);
        }

        public override void SetOptionalSaveUpdate(t_com_menu ent)
        {
            if (!string.IsNullOrEmpty(ent.parent_menu_id))
                ent.menu_group = _Model.t_com_menu.Select(se => new { se.menu_id, se.menu_name }).Single(x => x.menu_id == ent.parent_menu_id).menu_name;
            else
                ent.menu_group = ent.menu_name;

            var entMas = _Model.t_com_resource_master.SingleOrDefault(x => x.resource_master_id == ent.resource_master_id);
            if (entMas != null)
            {
                entMas.resource_name = ent.menu_name;
                entMas.description = ent.menu_name;
                entMas.default_value = ent.menu_name;
            }
        }

        public override bool DeleteById(object Id)
        {
            return this._Model.Delete(this, delegate ()
            {
                var _id = Id.ToString();

                var ent = this._Model.t_com_menu.FirstOrDefault(x => x.menu_id == _id);
                if (ent != null)
                {
                    var res_ms = this._Model.t_com_resource_master.FirstOrDefault(x => x.resource_master_id == ent.resource_master_id);
                    if (res_ms != null)
                    {
                        var res_det = this._Model.t_com_resource_detail.Where(x => x.resource_master_id == ent.resource_master_id);
                        this._Model.t_com_resource_detail.RemoveRange(res_det);

                        this._Model.Entry(res_ms).State = System.Data.Entity.EntityState.Deleted;
                    }

                    var menu_grbs = this._Model.t_com_user_group_menu.Where(x => x.menu_id == ent.menu_id);
                    this._Model.t_com_user_group_menu.RemoveRange(menu_grbs);

                    this._Model.Entry(ent).State = System.Data.Entity.EntityState.Deleted;
                }

                return this._Model.SaveChanges();
            });
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _is_admin = (bool)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_is_admin").Value;
            var _app_id = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_app_id").Value;

            var result = from rows in this._Model.t_com_menu

                         join app in this._Model.t_com_application on rows.app_id equals app.app_id
                         join parent in this._Model.t_com_menu on rows.parent_menu_id equals parent.menu_id into joinParent
                         from parent in joinParent.DefaultIfEmpty()
                         where rows.app_id == _app_id
                         select new
                         {
                             KeyId = rows.menu_id,
                             app.app_id,
                             app.application_name,
                             rows.menu_id,
                             rows.menu_name,
                             rows.platform,
                             //rows.menu_group,
                             parent_menu_name = parent.menu_name,
                             rows.menu_group_sequence,
                             rows.menu_sequence,
                             rows.process,
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

        #endregion
    }

}
