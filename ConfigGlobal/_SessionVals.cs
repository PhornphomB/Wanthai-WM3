using System.Configuration;
using System.Web;

using WebSecurityApi.DTO;

public static class _SessionVals
{
    public static ResultUser User
    {
        get
        {
            return (ResultUser)HttpContext.Current.Session["User"];
        }
        set
        {
            HttpContext.Current.Session["User"] = value;
        }
    }

    public static string UserName
    {
        get
        {
            if (HttpContext.Current.Session["UserName"] == null)
                HttpContext.Current.Session["UserName"] = string.Empty;

            return HttpContext.Current.Session["UserName"].ToString();
        }
        set
        {
            HttpContext.Current.Session["UserName"] = value;
        }
    }

    public static string GroupID
    {
        get
        {
            if (HttpContext.Current.Session["GroupID"] == null)
                HttpContext.Current.Session["GroupID"] = string.Empty;

            return HttpContext.Current.Session["GroupID"].ToString();
        }
        set
        {
            HttpContext.Current.Session["GroupID"] = value;
        }
    }

    public static string AppID
    {
        get
        {
            if (HttpContext.Current.Session["AppID"] == null)
                HttpContext.Current.Session["AppID"] = ConfigurationManager.AppSettings["APP_ID"];

            return HttpContext.Current.Session["AppID"].ToString();
        }
        set
        {
            HttpContext.Current.Session["AppID"] = value;
        }
    }

    public static string LocaleID
    {
        get
        {
            if (HttpContext.Current.Session["LocaleID"] == null)
                HttpContext.Current.Session["LocaleID"] = string.Empty;

            return HttpContext.Current.Session["LocaleID"].ToString();
        }
        set
        {
            HttpContext.Current.Session["LocaleID"] = value;
        }
    }

    public static string Platform
    {
        get
        {
            if (HttpContext.Current.Session["Platform"] == null)
                HttpContext.Current.Session["Platform"] = ConfigurationManager.AppSettings["PLATFORM"];

            return HttpContext.Current.Session["Platform"].ToString();
        }
        set
        {
            HttpContext.Current.Session["Platform"] = value;
        }
    }

    public static string DeviceID
    {
        get
        {
            if (HttpContext.Current.Session["DeviceID"] == null)
                HttpContext.Current.Session["DeviceID"] = ConfigGlobal.Extensions.GetComputerName();

            return HttpContext.Current.Session["DeviceID"].ToString();
        }
        set
        {
            HttpContext.Current.Session["DeviceID"] = value;
        }
    }

    public static bool IsAdmin
    {
        get
        {
            if (HttpContext.Current.Session["IsAdmin"] == null)
                HttpContext.Current.Session["IsAdmin"] = false;

            return (bool)HttpContext.Current.Session["IsAdmin"];
        }
        set
        {
            HttpContext.Current.Session["IsAdmin"] = value;
        }
    }

    public static void InitialSession()
    {
        User = null;
        UserName = string.Empty;
        GroupID = string.Empty;
        IsAdmin = false;

        HttpContext.Current.Session["MenuView"] = null;
        HttpContext.Current.Session["GridKeepField"] = null;
        HttpContext.Current.Session["Resource_General"] = null;
    }
}