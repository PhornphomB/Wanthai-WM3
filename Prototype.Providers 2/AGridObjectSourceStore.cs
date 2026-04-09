using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
//using System.Linq.Dynamic;
//using System.Dynamic;

namespace Prototype.Providers
{
    public abstract class AGridObjectSourceStore : IGridKeyData, IGridViewData<DataTable>, IGridExportData, IStoreExtend, IDisposable
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


        protected DbContext GridObjContext { get; set; }
        private CustomExecuteStore StoreFunction { get; set; }

        public List<SqlParameter> StoreParameterQuery { get; set; }
        public List<SqlParameter> StoreParameterExport { get; set; }

        public string StoreNameQuery { get; set; }
        public string StoreNameExport { get; set; }

        private int _countAllDataRow = 0;
        public int CountAllDataRow
        {
            set { _countAllDataRow = value; }
            get { return _countAllDataRow; }
        }

        private void InitialStoreFunction()
        {
            if (StoreFunction == null)
                StoreFunction = new CustomExecuteStore(this.GridObjContext);
        }

        abstract public void InitialStoreView();
        abstract public void InitialStoreExport();

        public FilterCustom FilterCustom { get; set; }
        public FilterList FilterAuto { get; set; }

        public List<String> ExportHeaderName { get; set; }

        protected DataTable QueryView(int? iBeginRowIndex, int? iMaximumRows, string _sortCondition, FilterList _filterGrid, FilterCustom _filterCustom)
        {
            try
            {
                this.InitialStoreFunction();

                this.FilterAuto = _filterGrid;
                this.FilterCustom = _filterCustom;

                var _condition = DataFilterSQL.GetSQLCondition(_filterGrid);

                this.StoreParameterQuery = new List<SqlParameter>();
                this.StoreParameterQuery.Add(new SqlParameter("@in_startRowIndex", iBeginRowIndex));
                this.StoreParameterQuery.Add(new SqlParameter("@in_maximumRows", iMaximumRows));
                this.StoreParameterQuery.Add(new SqlParameter("@in_vchCondition", _condition));
                this.StoreParameterQuery.Add(new SqlParameter("@in_vchSort", _sortCondition));
                this.StoreParameterQuery.Add(new SqlParameter("@in_switchType", "_view"));
                this.StoreParameterQuery.Add(new SqlParameter("@out_rowCount", 0) { Direction = ParameterDirection.Output });

                this.InitialStoreView();

                if (this.StoreNameQuery == null)
                    throw new Exception("! AGridObjectSourceStore.StoreNameQuery is null value");

                DataTable dt = this.StoreFunction.ExecuteToTable(this.StoreNameQuery, this.StoreParameterQuery);

                this.CountAllDataRow = Convert.ToInt32(this.StoreFunction.OutParameters.First().Value);

                //string selectByColumns = string.Empty;
                //foreach (var column in dt.Columns.Cast<DataColumn>())
                //{
                //    selectByColumns += string.Format("it[\"{0}\"] as {0},", column.ColumnName);
                //}

                //var result = dt.AsEnumerable().AsQueryable().Select(String.Format("new({0})", selectByColumns.TrimEnd(',')));

                return dt;
            }
            catch (Exception exception)
            {
                //this.Logging = new Prototype.Providers.Logging(this, exception,
                //    System.Diagnostics.EventLogEntryType.Error,
                //    1060,
                //    global::Prototype.Providers.Message.Message_ListFailure,
                //    global::Prototype.Providers.Message.Message_TryOrContact);
                //this.RaiseLogging();

                throw exception;
            }
        }

        protected DataTable QueryExport(FilterList _filterGrid, FilterCustom _filterCustom, string _sortCondition)
        {
            return this.GetDataTable(this, delegate ()
            {
                this.InitialStoreFunction();

                this.FilterAuto = _filterGrid;
                this.FilterCustom = _filterCustom;

                var _condition = DataFilterSQL.GetSQLCondition(_filterGrid);

                this.StoreParameterExport = new List<SqlParameter>();
                this.StoreParameterExport.Add(new SqlParameter("@in_startRowIndex", DBNull.Value));
                this.StoreParameterExport.Add(new SqlParameter("@in_maximumRows", DBNull.Value));
                this.StoreParameterExport.Add(new SqlParameter("@in_vchCondition", _condition));
                this.StoreParameterExport.Add(new SqlParameter("@in_vchSort", _sortCondition));
                this.StoreParameterExport.Add(new SqlParameter("@in_switchType", "_export"));
                this.StoreParameterExport.Add(new SqlParameter("@out_rowCount", 0) { Direction = ParameterDirection.Output });

                this.InitialStoreExport();

                if (this.StoreNameExport == null)
                    throw new Exception("! AGridObjectSourceStore.StoreNameExport is null value");

                DataTable dt = this.StoreFunction.ExecuteToTable(this.StoreNameExport, this.StoreParameterExport, this.ExportHeaderName);
                return dt;
            });
        }

        public DataTable QueryViewRaw(FilterCustom _filterCustom)
        {
            return this.GetDataTable(this, delegate ()
            {
                InitialStoreFunction();

                this.FilterCustom = _filterCustom;

                StoreParameterQuery = new List<SqlParameter>();
                StoreParameterQuery.Add(new SqlParameter("@in_startRowIndex", DBNull.Value));
                StoreParameterQuery.Add(new SqlParameter("@in_maximumRows", DBNull.Value));
                StoreParameterQuery.Add(new SqlParameter("@in_vchCondition", ""));
                StoreParameterQuery.Add(new SqlParameter("@in_vchSort", DBNull.Value));
                StoreParameterQuery.Add(new SqlParameter("@in_switchType", "_schema"));
                StoreParameterQuery.Add(new SqlParameter("@out_rowCount", 0) { Direction = ParameterDirection.Output });

                this.InitialStoreView();

                if (StoreNameQuery == null)
                    throw new Exception("! AGridObjectSourceStore.StoreNameQuery is null value");

                DataTable dt = StoreFunction.ExecuteToTable(StoreNameQuery, StoreParameterQuery);
                return dt;
            });
        }



        #region Implement Interface IGridViewData

        public DataTable GetViewData(int iBeginRowIndex, int iMaximumRows, string iSortField, string _sortDefault, FilterList _filterGrid, FilterCustom _filterCustom)
        {
            //return this.GetDataTable(this, delegate ()
            //{
            var sortFields = string.Empty;
            if (!string.IsNullOrEmpty(iSortField))
                sortFields = iSortField;
            else
                sortFields = _sortDefault;

            var query = QueryView(iBeginRowIndex, iMaximumRows, sortFields, _filterGrid, _filterCustom);

            return query;
            //});
        }

        public int GetCountData(string _sortDefault, FilterList _filterGrid, FilterCustom _filterCustom)
        {
            return this.CountAllDataRow;
        }

        #endregion

        #region Implement Interface IGridKeyData

        public IQueryable<TCast> GetQueryAllKey<TCast>(FilterList _filterGrid, FilterCustom _filterCustom, string _keyFieldName)
        {
            var dt = QueryExport(_filterGrid, _filterCustom, string.Empty);

            var result = from rows in dt.AsEnumerable()
                         select rows.Field<TCast>(_keyFieldName);

            return result.AsQueryable();
        }
        public List<TCast> GetAllKey<TCast>(FilterList _filterGrid, FilterCustom _filterCustom, string _keyFieldName)
        {
            return GetQueryAllKey<TCast>(_filterGrid, _filterCustom, _keyFieldName).ToList();
        }

        #endregion

        #region Implement Interface IGridExportData

        public DataTable GetExportData(FilterList _filterGrid, FilterCustom _filterCustom, ExportSourceType _exportSourceType)
        {
            return this.GetDataTable(this, delegate ()
            {
                var table = QueryExport(_filterGrid, _filterCustom, string.Empty);
                return table;
            });
        }

        public DataTable GetExportData(string iSortField, string _sortDefault, FilterList _filterGrid, FilterCustom _filterCustom, ExportSourceType _exportSourceType)
        {
            return this.GetDataTable(this, delegate ()
            {
                var sortFields = string.Empty;
                if (!string.IsNullOrEmpty(iSortField))
                    sortFields = iSortField;
                else
                    sortFields = _sortDefault;

                var table = QueryExport(_filterGrid, _filterCustom, sortFields);
                return table;
            });
        }

        #endregion
    }
}
