using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.TransLog
{
    public class TransactionType : IDisposable
    {
        #region ++INSTANCE STATIC++
        public static TransactionType Instance
        {
            get
            {
                using (TransactionType _Instance = new TransactionType())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public WMSEntities _Model { get; set; }

        public TransactionType()
        {
            this._Model = new WMSEntities();
        }


        public IQueryable<Property> GetQuery()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = (from rows in this._Model.t_com_transaction
                              where rows.is_active == "YES"
                              orderby rows.tran_type
                              select new Property
                              {
                                  value_member = rows.tran_type,
                                  display_member = rows.tran_type
                              }).Distinct();

                return result;
            });
        }

        public IQueryable<Property> GetQueryByWarehouse(string _wh_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = (from rows in this._Model.t_com_transaction
                              where rows.is_active == "YES" && rows.wh_id == _wh_id
                              orderby rows.tran_type
                              select new Property
                              {
                                  value_member = rows.tran_type,
                                  display_member = rows.tran_type
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
