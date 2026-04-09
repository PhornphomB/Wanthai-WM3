using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

using SecurityM.Source;
using Prototype.Providers;

namespace SecurityM.Access.PropertyCollection
{
    public class Menu : IDisposable
    {

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion


        public SecurityM_Entities _Model { get; set; }

        public Menu()
        {
            this._Model = new SecurityM_Entities();
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

        public IQueryable<Property> GetQueryByPlatform(string _app_id, string _platform)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = from rows in this._Model.t_com_menu
                             where rows.is_active == "YES" && rows.app_id == _app_id && rows.platform == _platform && string.IsNullOrEmpty(rows.process)
                             orderby rows.menu_name
                             select new Property
                             {
                                 value_member = rows.menu_id,
                                 display_member = rows.menu_name
                             };

                return result;
            });
        }

        public IQueryable<Property> GetQueryMenuGroup(string _app_id, string _platform)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_com_menu
                              where rows.is_active == "YES" && rows.app_id == _app_id && rows.platform == _platform
                              orderby rows.menu_group
                              select new Property
                              {
                                  value_member = rows.menu_group,
                                  display_member = rows.menu_group
                              }).Distinct();

                return result;
            });
        }

    }
}
