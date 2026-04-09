using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS_NEW.Access.Helper
{
    public class SQLDynamicSearch
    {
        public string GenerateFilter(FilterModel filter)
        {

            string fullFieldName = $"{filter.DataPropertyName}";

            switch (filter.FilterAt)
            {
                case FilterAt.Equal:
                    return $" AND {fullFieldName} = '{filter.Value}'";
                case FilterAt.Between:
                    return $" AND {fullFieldName} BETWEEN '{filter.Value}' AND '{filter.ValueTo}'";
                case FilterAt.NotEqual:
                    return $" AND {fullFieldName} <> '{filter.Value}'";
                case FilterAt.MoreThan:
                    return $" AND {fullFieldName} > '{filter.Value}'";
                case FilterAt.LessThan:
                    return $" AND {fullFieldName} < '{filter.Value}'";
                case FilterAt.Contains:
                    return $" AND {fullFieldName} LIKE '%{filter.Value}%'";
                case FilterAt.MoreThanEqual:
                    return $" AND {fullFieldName} >= '{filter.Value}'";
                case FilterAt.LessThanEqual:
                    return $" AND {fullFieldName} <= '{filter.Value}'";
                default:
                    return string.Empty;
            }
        }
    }
    public class FilterModel
    {
        public string DataPropertyName { get; set; }
        public FilterAt FilterAt { get; set; }
        public string Value { get; set; }
        public string ValueTo { get; set; } // เฉพาะกรณี Between
    }
}
