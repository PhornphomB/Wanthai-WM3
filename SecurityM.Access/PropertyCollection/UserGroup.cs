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
    public class UserGroup : IDisposable
    {

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion


        public SecurityM_Entities _Model { get; set; }

        public UserGroup()
        {
            this._Model = new SecurityM_Entities();
        }

        #region ++INSTANCE STATIC++
        public static UserGroup Instance
        {
            get
            {
                using (UserGroup _Instance = new UserGroup())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        public IQueryable<Property> GetQueryProperty()
        {
            return this._Model.GetDataBy(this, delegate()
            {
                //Delegate Statemant ---

                var result = from rows in this._Model.t_com_user_group
                             where rows.is_active == "YES"
                             orderby rows.name
                             select new Property
                             {
                                 value_member = rows.user_group_id,
                                 display_member = rows.name
                             };

                return result;
            });
        }

        public IQueryable<Property> GetQueryProperty(string _app_id)
        {
            return this._Model.GetDataBy(this, delegate()
            {
                //Delegate Statemant ---

                var result = from rows in this._Model.t_com_user_group
                             where rows.is_active == "YES" && rows.app_id == _app_id
                             orderby rows.name
                             select new Property
                             {
                                 value_member = rows.user_group_id,
                                 display_member = rows.name
                             };

                return result;
            });
        }
    }
}
