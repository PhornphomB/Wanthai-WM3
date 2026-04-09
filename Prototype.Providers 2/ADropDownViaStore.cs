using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Data.Entity;

namespace Prototype.Providers
{
    public abstract class ADropDownViaStore : IDisposable
    {
        public event global::Prototype.Providers.EventHandler EventResulted;
        public global::Prototype.Providers.Logging Logging { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Action Logging

        public void RaiseLogging()
        {
            Logging.Raise(EventResulted);
        }

        #endregion


        public ADropDownViaStore()
        {
            Parameters = new List<SqlParameter>();
        }

        protected DbContext DropDownObjContext { get; set; }

        private CustomExecuteStore StoreFunction { get; set; }
        private List<SqlParameter> Parameters { get; set; }


        private void InitialStoreFunction()
        {
            if (StoreFunction == null)
                StoreFunction = new CustomExecuteStore(DropDownObjContext);
        }

        protected IEnumerable<Property> GetDropDownData(string _storeName, string _condition, int _limit, params SqlParameter[] _includePrms)
        {
            Parameters.Clear();
            Parameters.Add(new SqlParameter("@in_vchLimit", _limit));
            Parameters.Add(new SqlParameter("@in_vchCondition", _condition));
            Parameters.AddRange(_includePrms);

            InitialStoreFunction();

            var result = StoreFunction.ExecuteToList<Property>(_storeName, Parameters);

            StoreFunction = null;

            return result;
        }
    }
}
