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
    public class Application : IDisposable
    {

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion


        public SecurityM_Entities _Model { get; set; }

        public Application()
        {
            this._Model = new SecurityM_Entities();
        }

        #region ++INSTANCE STATIC++
        public static Application Instance
        {
            get
            {
                using (Application _Instance = new Application())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        public IQueryable<Property> GetQueryProperty(string _app_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---

                var result = from rows in this._Model.t_com_application
                             where rows.is_active == "YES" && rows.app_id == _app_id
                             orderby rows.application_name
                             select new Property
                             {
                                 value_member = rows.app_id,
                                 display_member = rows.application_name
                             };

                return result;
            });
        }

        public IQueryable<Property> GetQueryProperty_GetName(string _app_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---

                var result = from rows in this._Model.t_com_application
                             where rows.is_active == "YES" && rows.app_id == _app_id
                             orderby rows.application_name
                             select new Property
                             {
                                 value_member = rows.application_name,
                                 display_member = rows.application_name
                             };

                return result;
            });
        }

    }
}
