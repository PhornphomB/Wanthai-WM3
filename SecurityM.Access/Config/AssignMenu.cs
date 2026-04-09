using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

using SecurityM.Source;
using Prototype.Providers;

namespace SecurityM.Access.Config
{
    [Serializable()]
    public class MenuViewRole
    {
        public bool MenuUse { get; set; }
        public int MenuGroupIndex { get; set; }
        public int MenuIndex { get; set; }
        public string MenuKey { get; set; }
        public string MenuName { get; set; }
        public string MenuParentKey { get; set; }
    }

    [Serializable()]
    public class MenuKey
    {
        public bool IsSelected { get; set; }
        public string MenuId { get; set; }
    }

    public class AssignMenu : AGridObjectSourceQuery
    {
        public SecurityM_Entities _Model { get; set; }

        public AssignMenu()
        {
            this._Model = new SecurityM_Entities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static AssignMenu Instance
        {
            get
            {
                using (AssignMenu _Instance = new AssignMenu())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public bool Save(List<MenuKey> _listMenu, string _user_group_id, string _create_by)
        {
            return this._Model.Update(this, delegate ()
            {

                var listUnselect = _listMenu.Where(wh => wh.IsSelected == false).Select(se => se.MenuId).ToList();

                var entUnSelect = this._Model.t_com_user_group_menu
                                             .Where(wh => wh.user_group_id == _user_group_id && listUnselect.Contains(wh.menu_id));

                foreach (var ent in entUnSelect)
                {
                    this._Model.t_com_user_group_menu.Remove(ent);
                }

                t_com_user_group_menu entSave = null;
                var dateNow = DateTime.Now;

                var listNewMenu = _listMenu.Where(wh => wh.IsSelected == true);
                foreach (var menu in listNewMenu)
                {
                    entSave = new t_com_user_group_menu();
                    entSave.menu_id = menu.MenuId;
                    entSave.user_group_id = _user_group_id;
                    entSave.is_active = "YES";
                    entSave.create_by = _create_by;
                    entSave.create_date = dateNow;

                    this._Model.t_com_user_group_menu.Add(entSave);
                }

                return this._Model.SaveChanges();
            });
        }

        public List<MenuViewRole> GetListMenuByRole(string _app_id, string user_group_id, string _platform, string _lang)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = (from menu in this._Model.t_com_menu
                              where menu.is_active == "YES" && menu.app_id == _app_id && menu.platform == _platform
                              select new MenuViewRole
                              {
                                  MenuUse = menu.t_com_user_group_menu.Any(wh => wh.user_group_id == user_group_id) ? true : false,
                                  MenuKey = menu.menu_id,
                                  MenuGroupIndex = menu.menu_group_sequence.Value,
                                  MenuIndex = menu.menu_sequence,
                                  MenuName = menu.menu_name,
                                  MenuParentKey = menu.parent_menu_id
                              }).ToList();

                return result;
            });
            //return this._Model.GetDataBy(this, delegate ()
            //{
            //    var result1 = (from menu in this._Model.t_com_menu
            //                   where menu.is_active == "YES" && menu.app_id == _app_id && menu.platform == _platform
            //                   select new MenuViewRole
            //                   {
            //                       MenuUse = menu.t_com_user_group_menu.Any(wh => wh.user_group_id == user_group_id) ? true : false,
            //                       MenuKey = menu.menu_id,
            //                       MenuGroupIndex = menu.menu_group_sequence.Value,
            //                       MenuIndex = menu.menu_sequence,
            //                       MenuName = menu.menu_name,
            //                       MenuParentKey = menu.parent_menu_id
            //                   }).ToList();

            //    var result = (from menu in this._Model.t_com_menu
            //                  join rm in this._Model.t_com_resource_master on menu.resource_master_id equals rm.resource_master_id
            //                  join rd in this._Model.t_com_resource_detail on new { rm.resource_master_id } equals new { rd.resource_master_id }
            //                  where menu.is_active == "YES" && menu.app_id == _app_id && menu.platform == _platform && rd.locale_id == _lang
            //                  select new MenuViewRole
            //                  {
            //                      MenuUse = menu.t_com_user_group_menu.Any(wh => wh.user_group_id == user_group_id) ? true : false,
            //                      MenuKey = menu.menu_id,
            //                      MenuGroupIndex = menu.menu_group_sequence.Value,
            //                      MenuIndex = menu.menu_sequence,
            //                      MenuName = rd.value, //menu.menu_name,
            //                      MenuParentKey = menu.parent_menu_id
            //                  }).ToList();
            //    //select new
            //    //             {
            //    //                 m.Menu_id,
            //    //                 m.Menu_group_sequence,
            //    //                 m.Menu_sequence,
            //    //                 resource_value = rd.Value,
            //    //                 m.Parent_menu_id,
            //    //                 rd.Locale_id
            //    //             });

            //    return result;
            //});
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            return null;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion



    }
}
