using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

public static class Extensions
{
    private static Regex regex = new Regex(@"\p{Lu}\p{Ll}*");


    public static bool IsMobileBrowser(this HttpRequest request)
    {
        string u = request.ServerVariables["HTTP_USER_AGENT"];

        var b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase);

        var v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase);

        return (b.IsMatch(u) || v.IsMatch(Left(u, 4)));
    }

    public static string GetTextAutoCreate(this string _text_source)
    {
        if (_text_source.Contains("_"))
            _text_source = _text_source.Split('_').Aggregate((x, y) => x + " " + y.UpperFirst()).UpperFirst();
        else if (_text_source.IsOnCapitals())
            _text_source = _text_source.SplitOnCapitals().Aggregate((x, y) => x + " " + y.UpperFirst()).UpperFirst();
        else
            _text_source = _text_source.UpperFirst();

        return _text_source;
    }

    public static bool IsOnCapitals(this string _text)
    {
        return regex.IsMatch(_text);
    }
    public static IEnumerable<string> SplitOnCapitals(this string _text)
    {
        Regex regex = new Regex(@"\p{Lu}\p{Ll}*");
        foreach (Match match in regex.Matches(_text))
        {
            yield return match.Value;
        }
    }
    public static string UpperFirst(this string s)
    {
        // Check for empty string.
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        // Return char and concat substring.
        return char.ToUpper(s[0]) + s.Substring(1);
    }

    public static string GetCurrentPageName(this HttpRequest request)
    {
        //string sPath = request.Url.AbsolutePath;
        //var oInfo = new System.IO.FileInfo(sPath);
        //string sRet = oInfo.Name;
        //return sRet;

        string currentPageName = System.IO.Path.GetFileName(request.Url.AbsolutePath);
        return currentPageName;
    }

    public static void RedirectDialog(this HttpResponse response, string url)
    {
        try
        {
            Page page = (Page)HttpContext.Current.Handler;

            if (page == null)
            {
                throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
            }

            url = page.ResolveClientUrl(url);
            ScriptManager.RegisterStartupScript(page, typeof(Page), "Redirect", "Popup('" + url + "');", true);
        }
        catch (System.Threading.ThreadAbortException Ex)
        {
            string ErrMsg = Ex.Message;
        }
    }
    public static void Redirect(this HttpResponse response, string url, string target, string windowFeatures)
    {
        try
        {
            if ((String.IsNullOrEmpty(target) || target.Equals("_self", StringComparison.OrdinalIgnoreCase)) && String.IsNullOrEmpty(windowFeatures))
            {
                response.Redirect(url);
            }
            else
            {
                Page page = (Page)HttpContext.Current.Handler;

                if (page == null)
                {
                    throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }
                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page, typeof(Page), "Redirect", script, true);
            }
        }
        catch (System.Threading.ThreadAbortException Ex)
        {
            string ErrMsg = Ex.Message;
        }
    }
    public static void RedirectWithPost(this HttpResponse response, string url, NameValueCollection _datas)
    {
        string html = "<html><head>";
        html += "</head><body onload='document.forms[0].submit()'>";
        html += string.Format("<form name='PostForm' method='POST' action='{0}'>", url);
        foreach (string key in _datas.Keys)
        {
            html += string.Format("<input name='{0}' type='text' value='{1}'>", key, _datas[key]);
        }
        html += "</form></body></html>";

        response.Clear();
        response.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");
        response.HeaderEncoding = Encoding.GetEncoding("ISO-8859-1");
        response.Charset = "ISO-8859-1";
        response.Write(html);
        response.End();
    }

    public static string Left(this string data, int lenth)
    {
        return data.Substring(0, lenth);
    }

    public static string Right(this string data, int lenth)
    {
        return data.Substring(data.Length - lenth, lenth);
    }

    public static readonly string[] SizeSuffixes =
                  { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

    public static string SizeSuffix(Int64 value)
    {
        if (value < 0) { return "-" + SizeSuffix(-value); }

        int i = 0;
        decimal dValue = (decimal)value;
        while (Math.Round(dValue / 1024) >= 1)
        {
            dValue /= 1024;
            i++;
        }

        return string.Format("{0:n1} {1}", dValue, SizeSuffixes[i]);
    }

    #region ConvertType Extension
    public static string FormatDecimal{get;set;}

    public static string Digit_STR { get; set; }
    public static int Digit_INT { get; set; }

    public static DateTime? ToDateTimeExt(this string _value)
    {
        if (_value != null && _value.Trim() != string.Empty)
        {
            return DateTime.ParseExact(_value.Trim(), FieldsStatic.DateFormat, FieldsStatic.CultureInfo);
        }
        else
        {
            return null;
        }
    }
    public static DateTime? ToDateTimeExcel(this string _value)
    {
        if (_value != null && _value.Trim() != string.Empty)
        {
            return DateTime.Parse(_value.Trim(), FieldsStatic.CultureInfo);
        }
        else
        {
            return null;
        }
    }
    public static string ToStringExt(this DateTime? _value)
    {
        if (_value != null)
        {
            return _value.Value.ToStringExt();
        }
        else
        {
            return string.Empty;
        }
    }
    public static string ToStringExt(this DateTime _value)
    {
        return _value.ToString(FieldsStatic.DateFormat, FieldsStatic.CultureInfo);
    }

    public static string ToStringExt(this Decimal? _value)
    {
        if (_value != null)
        {
            return _value.Value.ToString(FormatDecimal);
        }
        else
        {
            return string.Empty;
        }
    }

    public static string ToStringExt(this Decimal _value)
    {
        return _value.ToString(FormatDecimal);


    }



    public static Decimal? ToNumberExt(this string _value)
    {
        if (_value != null && _value.Trim() != string.Empty)
        {
            return Convert.ToDecimal(_value);
        }
        else
        {
            return null;
        }
    }
    public static Int32? ToIntegerExt(this string _value)
    {
        if (_value != null && _value.Trim() != string.Empty)
        {
            return Convert.ToInt32(_value);
        }
        else
        {
            return null;
        }
    }
    public static Decimal? ToDecimalExt(this string _value)
    {
        if (_value != null && _value.Trim() != string.Empty)
        {
            return Convert.ToDecimal(_value);
        }
        else
        {
            return null;
        }
    }

    public static string ToStringExt(this Int32? _value)
    {
        if (_value != null)
        {
            return _value.Value.ToString();
        }
        else
        {
            return string.Empty;
        }
    }

    public static Boolean ToBooleanExt(this string _value)
    {
        if (_value != null && _value.Trim() != string.Empty)
        {
            return Boolean.Parse(_value);
        }
        else
        {
            return false;
        }
    }
    public static string ToStringExt(this Boolean? _value)
    {
        if (_value != null)
        {
            return _value.Value.ToString();
        }
        else
        {
            return string.Empty;
        }
    }

    public static string ToStringExt(this string _value)
    {
        if (_value != null)
        {
            return _value.Trim().ToString();
        }
        else
        {
            return string.Empty;
        }
    }

    #endregion
}