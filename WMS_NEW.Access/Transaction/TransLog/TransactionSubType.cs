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
    public class TransactionSubType : IDisposable
    {
        #region ++INSTANCE STATIC++
        public static TransactionSubType Instance
        {
            get
            {
                using (TransactionSubType _Instance = new TransactionSubType())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public WMSEntities _Model { get; set; }

        public TransactionSubType()
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
                              orderby rows.sub_tran_type
                              select new Property
                              {
                                  value_member = rows.sub_tran_type,
                                  display_member = rows.sub_tran_type
                              }).Distinct();

                return result;
            });
        }

        public IQueryable<Property> GetQueryByWarehouse(string _tran_type, string _wh_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = (from rows in this._Model.t_com_transaction
                              where rows.is_active == "YES" && rows.tran_type == _tran_type && rows.wh_id == _wh_id
                              orderby rows.sub_tran_type
                              select new Property
                              {
                                  value_member = rows.sub_tran_type,
                                  display_member = rows.sub_tran_type
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
