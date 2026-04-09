using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class SubstringIndex
    {
        public int StartIndex { get; set; }
        public int SubLength { get; set; }
    }

    public static class StringExtension
    {
        public static string SubstringTrim(this string _str, int _startIndex)
        {
            return _str.Substring(_startIndex).Trim();
        }
        public static string SubstringTrim(this string _str, int _startIndex, int _subLength)
        {
            return _str.Substring(_startIndex, _subLength).Trim();
        }

        public static string SubstringMultiVal(this string _str, SubstringIndex[] _indexs, string _separator = "")
        {
            var returnStr = string.Empty;
            foreach (var item in _indexs)
            {
                returnStr += _str.Substring(item.StartIndex, item.SubLength);
            }

            return returnStr;
        }
    }

}
