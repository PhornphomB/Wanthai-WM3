using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData.Transportation
{
    public class TransportBy : IDisposable
    {
        #region ++INSTANCE STATIC++
        public static TransportBy Instance
        {
            get
            {
                using (TransportBy _Instance = new TransportBy())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public WMSEntities _Model { get; set; }

        public TransportBy()
        {
            this._Model = new WMSEntities();
        }


        public IQueryable<Property> GetQuery()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = (from rows in this._Model.t_wms_transport_by
                              where rows.is_active == "YES"
                              orderby rows.transport_by_code
                              select new Property
                              {
                                  value_member = rows.transport_by_code,
                                  display_member = rows.transport_by_name
                              }).Distinct();

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
