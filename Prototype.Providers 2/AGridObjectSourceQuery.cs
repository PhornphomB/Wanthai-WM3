using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Entity;

using System.Linq.Dynamic;
using thiscode.Tools.DynamicSelectExtensions;

namespace Prototype.Providers
{
    abstract public class AGridObjectSourceQuery : IGridKeyData, IGridViewData<IQueryable>, IGridExportData, IQueryExtend, IDisposable
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

        public DbContext GridObjContext { get; set; }

        abstract public IQueryable<dynamic> InitialQueryView();
        abstract public IQueryable<dynamic> InitialQueryExport();

        public FilterCustom FilterCustom { get; set; }
        public FilterList FilterAuto { get; set; }

        public List<String> ExportHeaderName { get; set; }

        protected IQueryable<dynamic> QueryView(FilterList _filterGrid, FilterCustom _filterCustom)
        {
            //return this.GetDataBy(this, delegate ()
            //{

            try
            {
                FilterAuto = _filterGrid;
                FilterCustom = _filterCustom;

                foreach (var fil in FilterAuto.Where(qry => qry.IsFilter == false))
                {
                    fil.IsFilter = true;
                }

                var result = InitialQueryView();
                result = DataFilter.GetQuery(result, FilterAuto);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //});
        }
        protected IQueryable<dynamic> QueryExport(FilterList _filterGrid, FilterCustom _filterCustom)
        {
            return this.GetDataBy(this, delegate ()
            {
                FilterAuto = _filterGrid;
                FilterCustom = _filterCustom;

                foreach (var fil in FilterAuto.Where(qry => qry.IsFilter == false))
                {
                    fil.IsFilter = true;
                }

                var result = InitialQueryExport();
                result = DataFilter.GetQuery(result, FilterAuto);

                return result;
            });
        }

        public IQueryable<dynamic> QueryViewRaw()
        {
            var result = InitialQueryView();
            return result;
        }


        #region Implement Interface IGridViewData

        public IQueryable GetViewData(int iBeginRowIndex, int iMaximumRows, string iSortField, string _sortDefault
            , FilterList _filterGrid, FilterCustom _filterCustom)
        {
            //return this.GetDataBy(this, delegate ()
            //{
            var query = QueryView(_filterGrid, _filterCustom);

            if (!string.IsNullOrEmpty(iSortField))
                query = query.OrderBy(iSortField);
            else
                query = query.OrderBy(_sortDefault);

            return query.Skip(iBeginRowIndex).Take(iMaximumRows);
            //});
        }

        public int GetCountData(string _sortDefault, FilterList _filterGrid, FilterCustom _filterCustom)
        {
            //return this.GetDataBy(this, delegate ()
            //{
            var query = QueryView(_filterGrid, _filterCustom);
            return query.Count();
            //});
        }

        #endregion

        #region Implement Interface IGridKeyData

        public IQueryable<TCast> GetQueryAllKey<TCast>(FilterList _filterGrid, FilterCustom _filterCustom, string _keyFieldName)
        {
            return this.GetDataBy(this, delegate ()
            {
                var query = QueryView(_filterGrid, _filterCustom);
                var result = query.Select(_keyFieldName, null);

                return result.Cast<TCast>();
            });
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
                var query = QueryExport(_filterGrid, _filterCustom);

                return query.ToDataTable(this.ExportHeaderName);
            });
        }

        public DataTable GetExportData(string iSortField, string _sortDefault, FilterList _filterGrid, FilterCustom _filterCustom, ExportSourceType _exportSourceType)
        {
            return this.GetDataTable(this, delegate ()
            {
                var query = QueryExport(_filterGrid, _filterCustom);

                if (!string.IsNullOrEmpty(iSortField))
                    query = query.OrderBy(iSortField);
                else
                    query = query.OrderBy(_sortDefault);

                return query.ToDataTable(this.ExportHeaderName);
            });
        }

        #endregion
    }
}
