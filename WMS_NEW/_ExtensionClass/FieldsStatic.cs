using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

public static class FieldsStatic
{

    private static System.Globalization.CultureInfo _cultureInfo = null;
    public static System.Globalization.CultureInfo CultureInfo
    {
        get
        {
            if (_cultureInfo == null)
            {
                _cultureInfo = new System.Globalization.CultureInfo(ConfigurationManager.AppSettings["UICulture"]);
            }

            return _cultureInfo;
        }
    }

    private static string _datetimeformat = string.Empty;
    public static string DateTimeFormat
    {
        get
        {
            if (_datetimeformat == string.Empty)
            {
                _datetimeformat = ConfigurationManager.AppSettings["DateFormat"] + " " + ConfigurationManager.AppSettings["TimeFormat"];
            }

            return _datetimeformat;
        }
    }

    private static string _dateformat = string.Empty;
    public static string DateFormat
    {
        get
        {
            if (_dateformat == string.Empty)
            {
                _dateformat = ConfigurationManager.AppSettings["DateFormat"];
            }

            return _dateformat;
        }
    }

    private static string _timeformat = string.Empty;
    public static string TimeFormat
    {
        get
        {
            if (_timeformat == string.Empty)
            {
                _timeformat = ConfigurationManager.AppSettings["TimeFormat"];
            }

            return _timeformat;
        }
    }

    public static string ApplicationVersion
    {
        get
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var version = asm.GetName().Version;

            return version.ToString();

        }

    }

    private static string _folderupload = string.Empty;
    public static string FolderUpload
    {
        get
        {
            if (_folderupload == string.Empty)
            {
                _folderupload = ConfigurationManager.AppSettings["FolderImages"];
            }

            return _folderupload;
        }
    }

    private static string _foldertemp = string.Empty;
    public static string FolderTemp
    {
        get
        {
            if (_foldertemp == string.Empty)
            {
                _foldertemp = ConfigurationManager.AppSettings["FolderTemp"];
            }

            return _foldertemp;
        }
    }

    public static int SessionTimeout = 0;
}
