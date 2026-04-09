using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;

using System.Linq.Dynamic;
using thiscode.Tools.DynamicSelectExtensions;

namespace Prototype.Providers
{
    public enum ExportSourceType
    {
        Database = 0,
        Object = 1
    }

    public interface IEntityCommandForm
    {
        bool Save(object _objectEntity);
        bool Update(object _objectEntity);

        object GetByKeyID(object Id);
        object GetEditKeyID(object Id);
    }

    public interface IQueryExtend
    {
        IQueryable<dynamic> QueryViewRaw();
    }

    public interface IStoreExtend
    {
        DataTable QueryViewRaw(FilterCustom _filterCustom);
    }

    public interface IGridCommand
    {
        bool DeleteById(object Id);
    }


    public interface IGridViewData<TResult>
    {
        TResult GetViewData(int iBeginRowIndex, int iMaximumRows, string iSortField, string _sortDefault, FilterList _filterGrid, FilterCustom _filterCustom);
        int GetCountData(string _sortDefault, FilterList _filterGrid, FilterCustom _filterCustom);
    }

    public interface IGridKeyData
    {
        IQueryable<TCast> GetQueryAllKey<TCast>(FilterList _filterGrid, FilterCustom _filterCustom, string _keyFieldName);
        List<TCast> GetAllKey<TCast>(FilterList _filterGrid, FilterCustom _filterCustom, string _keyFieldName);
    }

    public interface IGridExportData
    {
        event global::Prototype.Providers.EventHandler EventResulted;
        global::Prototype.Providers.Logging Logging { get; set; }

        void RaiseLogging();

        DataTable GetExportData(FilterList _filterGrid, FilterCustom _filterCustom, ExportSourceType _exportSourceType);
        DataTable GetExportData(string iSortField, string _sortDefault, FilterList _filterGrid, FilterCustom _filterCustom, ExportSourceType _exportSourceType);
    }
}
