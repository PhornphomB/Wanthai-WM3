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
    public class User : IDisposable
    {

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion


        public SecurityM_Entities _Model { get; set; }

        public User()
        {
            this._Model = new SecurityM_Entities();
        }

        #region ++INSTANCE STATIC++
        public static User Instance
        {
            get
            {
                using (User _Instance = new User())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        public IQueryable<Property> GetQueryProperty()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---

                var result = from rows in this._Model.t_com_user
                             where rows.is_active == "YES"
                             orderby rows.user_id
                             select new Property
                             {
                                 value_member = rows.user_id,
                                 display_member = rows.name
                             };

                return result;
            });
        }

        public IQueryable<Property> GetQueryFullName()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---

                var result = from rows in this._Model.t_com_user
                                 //where rows.is_active == "YES"
                             orderby rows.user_id
                             select new Property
                             {
                                 value_member = rows.user_id,
                                 display_member = rows.user_id + " : " + rows.first_name + " " + rows.last_name
                             };

                return result;
            });
        }
    }
}
