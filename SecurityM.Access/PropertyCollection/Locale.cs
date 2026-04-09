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
    public class Locale : IDisposable
    {

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion


        public SecurityM_Entities _Model { get; set; }

        public Locale()
        {
            this._Model = new SecurityM_Entities();
        }

        #region ++INSTANCE STATIC++
        public static Locale Instance
        {
            get
            {
                using (Locale _Instance = new Locale())
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

                var result = from rows in this._Model.t_com_locale
                             where rows.is_active == "YES"
                             orderby rows.locale_id
                             select new Property
                             {
                                 value_member = rows.locale_id,
                                 display_member = rows.name
                             };

                return result;
            });
        }
    }
}
