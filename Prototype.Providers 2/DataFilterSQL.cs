using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Dynamic;
using System.Data.Objects;
using System.Globalization;

namespace Prototype.Providers
{
    public static class DataFilterSQL
    {
        private static CultureInfo _cultureInfo = new CultureInfo("en-US");
        public static void SetCultureDateTime(string _language)
        {
            _cultureInfo = null;
            _cultureInfo = new CultureInfo(_language);
        }

        private static string PredToSQLFormat(this string _predicate, int _countValue)
        {
            if (_predicate.Contains(".Contains(@0)"))
            {
                _predicate = _predicate.Replace(".Contains(@0)", " LIKE {0}");
            }
            else if (_predicate.Contains("IsNullOrEmpty"))
            {
                _predicate = _predicate.Replace("String.IsNullOrEmpty(", " ISNULL(").Replace(")", ",'') = '' ");
            }
            else
            {
                for (int inx = 0; inx < _countValue; inx++)
                {
                    _predicate = _predicate.Replace("@" + inx, "{" + inx + "}");
                }
            }

            return _predicate;
        }
        private static string[] GetSQLValue(this object[] _values)
        {
            string[] arrVals = new string[_values.Count()];
            string strValue;

            for (var inx = 0; inx < _values.Count(); inx++)
            {
                strValue = string.Empty;

                if (_values[inx].GetType() == typeof(DateTime))
                {
                    var _val = (DateTime)_values[inx];
                    if (_val.Hour == 0 && _val.Minute == 0 && _val.Second == 0)
                    {
                        strValue = _val.ToString("yyyy-MM-dd", _cultureInfo);
                    }
                    else
                    {
                        strValue = _val.ToString("yyyy-MM-dd HH:mm:ss.fff", _cultureInfo);
                    }

                    strValue = "'" + strValue + "'";
                }
                else if (_values[inx].GetType() == typeof(String))
                {
                    strValue = "N'" + _values[inx].ToString().ExceptOffensiveText() + "'";
                }
                else if (_values[inx].GetType() == typeof(Boolean))
                {
                    strValue = Convert.ToInt16(_values[inx]).ToString();
                }
                else
                {
                    strValue = _values[inx].ToString();
                }

                arrVals[inx] = strValue;
            }

            return arrVals;
        }
        private static string ExceptOffensiveText(this string _value)
        {
            return _value.Replace("%", "").Replace("'", "").Replace("\"", "");
        }

        private static string Where(this string _sqlCommand, string _predicate, params object[] _values)
        {
            var SQL_SYN = new[] { "LIKE", "ISNULL" };

            _predicate = _predicate.PredToSQLFormat(_values.Count());

            if (_sqlCommand != string.Empty)
                _sqlCommand += " AND ";

            if (SQL_SYN.Any(word => _predicate.Contains(word)) == false)
            {
                _sqlCommand += String.Format(_predicate, GetSQLValue(_values));
            }
            else if (_predicate.Contains(SQL_SYN[0]))
            {
                _sqlCommand += String.Format(_predicate, "N'%" + _values[0].ToString().ExceptOffensiveText() + "%'");
            }
            else if (_predicate.Contains(SQL_SYN[1]))
            {
                _sqlCommand += _predicate;
            }

            return _sqlCommand;
        }
        public static string GetSQLCondition(this FilterList _filterList)
        {
            string _sqlCommand = string.Empty;

            if (_filterList != null)
            {
                foreach (var _filter in _filterList.Where(qry => qry.IsFilter == true))
                {
                    if (!_filter.DataPropertyName.StartsWith("["))
                        _filter.DataPropertyName = string.Format("[{0}]", _filter.DataPropertyName);

                    else if ((_filter.FilterAt != FilterAt.Empty && _filter.Value == null)
                        || ((_filter.FilterAt == FilterAt.Contains) && (_filter.Value.GetType() != typeof(System.String))))
                        continue;

                    var type = _filter.Value.GetType();
                    if ((type == typeof(Guid)) || (type == typeof(Guid?)))
                    {
                        var value_old = _filter.Value;
                        _filter.Value = null;
                        _filter.Value = value_old.ToString();
                    }

                    if (_filter.FilterAt == FilterAt.Empty)
                    {
                        _sqlCommand = _sqlCommand.Where(_filter.GetCondition(), _filter.Value);
                    }
                    else
                    {

                        if (_filter.Value.GetType() == typeof(System.DateTime)) //Customize Filter Type DateTime
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

                                dateDummy = new DateTime(dateValue.Year, dateValue.Month, dateValue.Day, dateValue.Hour, dateValue.Minute, dateValue.Second, 998);

                                switch (_filter.FilterAt)
                                {
                                    case FilterAt.Between:

                                        if (_filter.ValueTo != null)
                                        {
                                            var dateValueTo = (DateTime)_filter.ValueTo;
                                            dateDummy = new DateTime(dateValueTo.Year, dateValueTo.Month, dateValueTo.Day, dateValueTo.Hour, dateValueTo.Minute, dateValueTo.Second, 998);
                                        }

                                        _format = string.Format("({0}>=@0 AND {1}<=@1)", _filter.DataPropertyName, _filter.DataPropertyName);
                                        _sqlCommand = _sqlCommand.Where(_format, _filter.Value, dateDummy);

                                        break;
                                    case FilterAt.Equal:
                                        _format = string.Format("({0}>=@0 AND {1}<=@1)", _filter.DataPropertyName, _filter.DataPropertyName);
                                        _sqlCommand = _sqlCommand.Where(_format, _filter.Value, dateDummy);

                                        break;
                                    case FilterAt.MoreThan:
                                        _sqlCommand = _sqlCommand.Where(_filter.GetCondition(), dateDummy);

                                        break;
                                    case FilterAt.LessThanEqual:
                                        _sqlCommand = _sqlCommand.Where(_filter.GetCondition(), dateDummy);

                                        break;
                                    case FilterAt.NotEqual:
                                        _format = string.Format("(({0}<@0) OR ({1}>@1))", _filter.DataPropertyName, _filter.DataPropertyName);
                                        _sqlCommand = _sqlCommand.Where(_format, _filter.Value, dateDummy);

                                        break;
                                    default:
                                        _sqlCommand = _sqlCommand.Where(_filter.GetCondition(), _filter.Value);
                                        break;
                                }

                                #endregion
                            }
                            else if (values[0] != firstDate)
                            {
                                #region Filter Date Only

                                dateDummy = new DateTime(dateValue.Year, dateValue.Month, dateValue.Day, 23, 59, 59, 998);

                                switch (_filter.FilterAt)
                                {
                                    case FilterAt.Between:

                                        if (_filter.ValueTo != null)
                                        {
                                            var dateValueTo = (DateTime)_filter.ValueTo;
                                            dateDummy = new DateTime(dateValueTo.Year, dateValueTo.Month, dateValueTo.Day, 23, 59, 59, 998);
                                        }

                                        _format = string.Format("({0}>=@0 AND {1}<=@1)", _filter.DataPropertyName, _filter.DataPropertyName);
                                        _sqlCommand = _sqlCommand.Where(_format, _filter.Value, dateDummy);

                                        break;
                                    case FilterAt.Equal:
                                        _format = string.Format("({0}>=@0 AND {1}<=@1)", _filter.DataPropertyName, _filter.DataPropertyName);
                                        _sqlCommand = _sqlCommand.Where(_format, _filter.Value, dateDummy);

                                        break;
                                    case FilterAt.MoreThan:
                                        _sqlCommand = _sqlCommand.Where(_filter.GetCondition(), dateDummy);

                                        break;
                                    case FilterAt.LessThanEqual:
                                        _sqlCommand = _sqlCommand.Where(_filter.GetCondition(), dateDummy);

                                        break;
                                    case FilterAt.NotEqual:
                                        _format = string.Format("(({0}<@0) OR ({1}>@1))", _filter.DataPropertyName, _filter.DataPropertyName);
                                        _sqlCommand = _sqlCommand.Where(_format, _filter.Value, dateDummy);

                                        break;

                                    default:
                                        _sqlCommand = _sqlCommand.Where(_filter.GetCondition(), _filter.Value);
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
                                _sqlCommand = _sqlCommand.Where(_filter.GetCondition(), _filter.Value, _filter.ValueTo);
                            else
                                _sqlCommand = _sqlCommand.Where(_filter.GetCondition(), _filter.Value, _filter.Value);
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
                                    _sqlCommand = _sqlCommand.Where(_customFormat, _filter.Values);
                                }
                            }
                            else
                            {
                                _sqlCommand = _sqlCommand.Where(_filter.GetCondition(), _filter.Value);
                            }
                        }
                    }
                }
            }
            return _sqlCommand;
        }
    }
}
