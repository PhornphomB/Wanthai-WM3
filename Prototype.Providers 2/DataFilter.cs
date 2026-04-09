using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Dynamic;
using System.Data.Objects;

using global::Prototype.Providers;
using System.Reflection;

namespace Prototype.Providers
{
    #region Filter Custom

    [Serializable()]
    public class FilterCustomSchema
    {
        public string DataFieldValue { get; set; }
        public object Value { get; set; }
        public object ValueTo { get; set; }
        public object FilterAt { get; set; }
    }
    [Serializable()]
    public class FilterCustom : List<FilterCustomSchema>
    {
        public void Add(string _dataFieldValue, object _value, object _filterAt)
        {
            Add(_dataFieldValue, _value, null, _filterAt);
        }
        public void Add(string _dataFieldValue, object _value, object _valueTo, object _filterAt)
        {
            var f = new FilterCustomSchema() { DataFieldValue = _dataFieldValue, Value = _value, ValueTo = _valueTo, FilterAt = _filterAt };
            this.Add(f);
        }
    }

    #endregion

    #region Filter Dynamic

    public enum FilterAt
    {
        [AttributeEntry("=", "Equal")]
        Equal,
        [AttributeEntry("&nbsp;", "None")]
        None,
        [AttributeEntry("Ø", "Empty")]
        Empty,
        [AttributeEntry(">", "More than")]
        MoreThan,
        [AttributeEntry("<", "Less than")]
        LessThan,
        [AttributeEntry("≥", "More than equal")]
        MoreThanEqual,
        [AttributeEntry("≤", "Less than equal")]
        LessThanEqual,
        [AttributeEntry("≠", "Not equal")]
        NotEqual,
        [AttributeEntry("%", "Contains")]
        Contains,
        [AttributeEntry("><", "Between")]
        Between
    }
    public interface IFilter
    {
        bool IsFilter { get; set; }
        string Symbol { get; set; }
        FilterAt FilterAt { get; set; }
        object[] Values { get; set; }
        object Value { get; set; }
        object ValueTo { get; set; }
        string DataPropertyName { get; set; }
        string GetCondition();
    }

    [Serializable()]
    public class Filter : IFilter
    {
        public Filter()
            : this(String.Empty, FilterAt.Equal, null)
        { }
        public Filter(String dataPropertyName, FilterAt filterAt, Object value)
        {
            DataPropertyName = dataPropertyName;
            FilterAt = filterAt;
            Value = value;
        }
        public String Symbol { get; set; }
        public Boolean IsFilter { get; set; }
        public Object[] Values { get; set; }
        public Object Value { get; set; }
        public Object ValueTo { get; set; }
        public String DataPropertyName { get; set; }
        private FilterAt filterAt;
        public FilterAt FilterAt
        {
            get
            {
                return filterAt;
            }
            set
            {
                filterAt = value;
                IsFilter = true;
                switch (filterAt)
                {
                    case FilterAt.Equal:
                        Symbol = "{0}=@0";
                        break;
                    case FilterAt.MoreThan:
                        Symbol = "{0}>@0";
                        break;
                    case FilterAt.LessThan:
                        Symbol = "{0}<@0";
                        break;
                    case FilterAt.MoreThanEqual:
                        Symbol = "{0}>=@0";
                        break;
                    case FilterAt.LessThanEqual:
                        Symbol = "{0}<=@0";
                        break;
                    case FilterAt.NotEqual:
                        Symbol = "{0}<>@0";
                        break;
                    case FilterAt.Contains:
                        Symbol = "{0}.Contains(@0)";
                        break;
                    case FilterAt.Between:
                        Symbol = "({0}>=@0 AND {1}<=@1)";
                        break;
                    case FilterAt.Empty:
                        Symbol = "String.IsNullOrEmpty({0})";
                        break;
                    case FilterAt.None:
                        goto default;
                    default:
                        Symbol = String.Empty;
                        IsFilter = false;
                        break;
                }
            }
        }
        public string GetCondition()
        {
            if (filterAt == FilterAt.Between)
            {
                return string.Format(this.Symbol, this.DataPropertyName, this.DataPropertyName);
            }
            else
            {
                return string.Format(this.Symbol, this.DataPropertyName);
            }
        }
    }

    [Serializable()]
    public class FilterList : List<IFilter>
    {
        public void Add(string _dataPropertyName, FilterAt _filterAt, object _value)
        {
            var f = new Filter(_dataPropertyName, _filterAt, _value);
            this.Add(f);
        }
    }

    public static class DataFilter
    {
        public static global::System.Data.DataTable GetTable<T>(IQueryable<T> _queryable, FilterList _filterList)
        {
            _queryable = _queryable.ToFilterBy(_filterList);
            return _queryable.ToDataTable();
        }
        public static global::System.Data.DataTable GetTable<T>(IQueryable<T> _queryable, FilterList _filterList, int _takeLimit)
        {
            _queryable = _queryable.ToFilterBy(_filterList).Take(_takeLimit);
            return _queryable.ToDataTable();
        }
        public static IQueryable<T> GetQuery<T>(IQueryable<T> _queryable, FilterList _filterList)
        {
            _queryable = _queryable.ToFilterBy(_filterList);
            return _queryable;
        }

        public static IQueryable<T> ToFilterBy<T>(this IQueryable<T> _queryable, FilterList _filterList)
        {
            if (_filterList != null)
            {
                foreach (var _filter in _filterList.Where(qry => qry.IsFilter == true))
                {
                    if ((_filter.FilterAt != FilterAt.Empty && _filter.Value == null)
                        || ((_filter.FilterAt == FilterAt.Contains) && (_filter.Value.GetType() != typeof(String))))
                        continue;

                    if (_filter.FilterAt == FilterAt.Empty)
                    {
                        _queryable = _queryable.Where(_filter.GetCondition());
                    }
                    else if (_filter.Value.GetType() == typeof(System.DateTime)) //Customize Filter Type DateTime
                    {
                        #region Filter DateTime Type

                        var _format = string.Empty;
                        var dateValue = (DateTime)_filter.Value;
                        var dateDummy = DateTime.Now;

                        var values = dateValue.ToString("yyyyMMdd-HHmmss").Split('-');
                        var firstDate = new DateTime(1753, 1, 7).ToString("yyyyMMdd");

                        if (values[0] != firstDate && values[1] != "000000")
                        {
                            #region Filter DateTime

                            dateDummy = new DateTime(dateValue.Year, dateValue.Month, dateValue.Day, dateValue.Hour, dateValue.Minute, dateValue.Second, 999);

                            switch (_filter.FilterAt)
                            {
                                case FilterAt.Between:

                                    if (_filter.ValueTo != null)
                                    {
                                        var dateValueTo = (DateTime)_filter.ValueTo;
                                        dateDummy = new DateTime(dateValueTo.Year, dateValueTo.Month, dateValueTo.Day, dateValueTo.Hour, dateValueTo.Minute, dateValueTo.Second, 999);
                                    }

                                    _format = string.Format("({0}>=@0 AND {1}<=@1)", _filter.DataPropertyName, _filter.DataPropertyName);
                                    _queryable = _queryable.Where(_format, _filter.Value, dateDummy);

                                    break;
                                case FilterAt.Equal:
                                    _format = string.Format("({0}>=@0 AND {1}<=@1)", _filter.DataPropertyName, _filter.DataPropertyName);
                                    _queryable = _queryable.Where(_format, _filter.Value, dateDummy);

                                    break;
                                case FilterAt.MoreThan:
                                    _queryable = _queryable.Where(_filter.GetCondition(), dateDummy);

                                    break;
                                case FilterAt.LessThanEqual:
                                    _queryable = _queryable.Where(_filter.GetCondition(), dateDummy);

                                    break;
                                case FilterAt.NotEqual:
                                    _format = string.Format("(({0}<@0) OR ({1}>@1))", _filter.DataPropertyName, _filter.DataPropertyName);
                                    _queryable = _queryable.Where(_format, _filter.Value, dateDummy);

                                    break;
                                default:
                                    _queryable = _queryable.Where(_filter.GetCondition(), _filter.Value);
                                    break;
                            }

                            #endregion
                        }
                        else if (values[0] != firstDate)
                        {
                            #region Filter Date Only

                            dateDummy = new DateTime(dateValue.Year, dateValue.Month, dateValue.Day, 23, 59, 59, 999);

                            switch (_filter.FilterAt)
                            {
                                case FilterAt.Between:

                                    if (_filter.ValueTo != null)
                                    {
                                        var dateValueTo = (DateTime)_filter.ValueTo;
                                        dateDummy = new DateTime(dateValueTo.Year, dateValueTo.Month, dateValueTo.Day, 23, 59, 59, 999);
                                    }

                                    _format = string.Format("({0}>=@0 AND {1}<=@1)", _filter.DataPropertyName, _filter.DataPropertyName);
                                    _queryable = _queryable.Where(_format, _filter.Value, dateDummy);

                                    break;
                                case FilterAt.Equal:
                                    _format = string.Format("({0}>=@0 AND {1}<=@1)", _filter.DataPropertyName, _filter.DataPropertyName);
                                    _queryable = _queryable.Where(_format, _filter.Value, dateDummy);

                                    break;
                                case FilterAt.MoreThan:
                                    _queryable = _queryable.Where(_filter.GetCondition(), dateDummy);

                                    break;
                                case FilterAt.LessThanEqual:
                                    _queryable = _queryable.Where(_filter.GetCondition(), dateDummy);

                                    break;
                                case FilterAt.NotEqual:
                                    _format = string.Format("(({0}<@0) OR ({1}>@1))", _filter.DataPropertyName, _filter.DataPropertyName);
                                    _queryable = _queryable.Where(_format, _filter.Value, dateDummy);

                                    break;

                                default:
                                    _queryable = _queryable.Where(_filter.GetCondition(), _filter.Value);
                                    break;
                            }

                            #endregion
                        }
                        else if (values[1] != "000000")
                        {

                        }

                        #endregion
                    }
                    else if (_filter.FilterAt == FilterAt.Between)
                    {
                        if (_filter.ValueTo != null)
                            _queryable = _queryable.Where(_filter.GetCondition(), _filter.Value, _filter.ValueTo);
                        else
                            _queryable = _queryable.Where(_filter.GetCondition(), _filter.Value, _filter.Value);
                    }
                    else
                    {
                        if (_filter.Values != null)
                        {
                            if (_filter.Values.Count() > 0)
                            {
                                var _customFormat = string.Empty;
                                if (_filter.FilterAt == FilterAt.Equal)
                                    _customFormat += "( ";
                                else if (_filter.FilterAt == FilterAt.NotEqual)
                                    _customFormat += "!( ";

                                int inx = 0;
                                while (inx < _filter.Values.Count())
                                {
                                    _customFormat += "(" + _filter.DataPropertyName + "=@" + inx + ")";

                                    inx++;
                                    if (inx < _filter.Values.Count())
                                    {
                                        _customFormat += " OR ";
                                    }

                                }

                                _customFormat += " )";
                                _queryable = _queryable.Where(_customFormat, _filter.Values);
                            }
                        }
                        else
                        {
                            _queryable = _queryable.Where(_filter.GetCondition(), _filter.Value);
                        }
                    }
                }
            }
            return _queryable;
        }
    }

    #endregion
}
