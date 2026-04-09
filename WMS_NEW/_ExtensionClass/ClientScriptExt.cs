using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web.UI;

public enum JMessageType
{
    Warning,
    Accept,
    Notify
}

public static class ClientScriptExt
{
    public static void RegisterStartupScript(this Page thisPage, string script)
    {
        //Register JavaScript In Ajax PostBack

        System.Web.UI.ScriptManager.RegisterStartupScript(thisPage, thisPage.GetType(), Guid.NewGuid().ToString().Replace("-", "_"), script, true);
    }
    public static void ScriptPageRegister(this Page thisPage, string script)
    {
        //Register JavaScript In Ajax PostBack
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(thisPage, thisPage.GetType(), Guid.NewGuid().ToString().Replace("-", "_"), script, true);
    }
    public static void ScriptPageRegister(this Page thisPage, string script, string _keyName)
    {
        //Register JavaScript In Ajax PostBack
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(thisPage, thisPage.GetType(), _keyName, script, true);
    }
    public static void ScriptAlert(this Page thisPage, string message)
    {
        //Register JavaScript In Ajax PostBack
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(thisPage, thisPage.GetType(), "Alert", @"alert('" + message + "');", true);
    }

    public static void ScriptException(this Page thisPage, Exception exception)
    {
        var pageName = Path.GetFileName(thisPage.Request.Path);
        var pageUrl = Path.GetFileName(thisPage.Request.Url.PathAndQuery);

        var msg = string.Empty;
        if (exception.InnerException != null)
        {
            msg = exception.InnerException.Message;
        }
        else if (exception.Message != string.Empty)
        {
            msg = exception.Message;
        }
        else
        {
            msg = exception.ToString();
        }

        var exceptionMsg = "Exception Detail : " + msg + "<br/><br/>Exception Page : " + pageName;
        ScriptJqueryMessage(thisPage, exceptionMsg, JMessageType.Warning, false);
    }


    public static void ToAppendTextScript(this Exception _ex, StringBuilder _sb)
    {
        if (_ex != null)
        {
            _sb.Append("<div style=\"text-align: left;\">Source : " + _ex.Source);
            _sb.Append("<br/>Inner Exception Text : " + _ex.Message.Replace("\r", " ").Replace("\n", "<br/>").Replace("'", "") + "</div><br/>");
            if (_ex.InnerException != null)
            {
                _ex.InnerException.ToAppendTextScript(_sb);
            }
        }
    }
    public static void ToAppendTextScript(this Exception _ex, string _str)
    {
        if (_ex != null)
        {
            _str += "<div style=\"text-align: left;\">Inner Exception Text : <br/>"
              + _ex.Message.Replace("\r", " ").Replace("\n", "<br/>").Replace("'", "") + "</div><br/>";
            _str += "<div style=\"text-align: center;\">Source : <br/>"
                    + _ex.Source.Replace("'", "") + "</div><br/>";
            if (_ex.InnerException != null)
            {
                _ex.InnerException.ToAppendTextScript(_str);
            }
        }
    }

    public static void ShowEventLogging(this Page thisPage, Prototype.Providers.EventArgsCustom e)
    {
        try
        {
            if (e.Logging.EntryType == EventLogEntryType.Error)
            {
                var sb = new StringBuilder();
                using (var acc = new WMS_NEW.Access.Configuration.ResourceMaster())
                {
                    //this.PlugEventResult(acc);
                    var _resourceList = acc.GetErrorResource();

                    if (e.Logging.Exception != null)
                    {
                        string defaultMessage = "";
                        foreach (var res in _resourceList)
                        {
                            if (res.ResourceName == "default")
                            {
                                defaultMessage = res.ResourceValue;
                                continue;
                            }

                            if (e.Logging.ExceptionMessage.Replace("\r", " ").Replace("\n", "<br/>").Replace("'", "").Contains(res.ResourceDefault))
                            {
                                sb.Append("<div style=\"text-align: left;\">" + res.ResourceValue.ToString());
                                sb.Append("</div><br/>");
                            }
                        }

                        //sb.Append("<div style=\"text-align: left;\">Source : " + e.Logging.Object.GetType().ToString());
                        //sb.Append("<br/>Exception Text : " + e.Logging.ExceptionMessage.Replace("\r", " ").Replace("\n", "<br/>").Replace("'", "") + "</div><br/>");
                        //e.Logging.Exception.InnerException.ToAppendTextScript(sb);

                        if(sb.Length == 0)
                        {
                            sb.Append("<div style=\"text-align: left;\">" + defaultMessage);
                            sb.Append("</div><br/>");
                        }
                    }
                }
                 
                ScriptJqueryMessage(thisPage, sb.ToString(), JMessageType.Warning, false);
            }
            else if (e.Logging.EntryType == EventLogEntryType.Warning)
            {
                ScriptJqueryMessage(thisPage, e.Logging.Description, JMessageType.Warning, false);
            }
            else if (e.Logging.EntryType == EventLogEntryType.Information)
            {
                ScriptJqueryMessage(thisPage, e.Logging.Description, JMessageType.Notify, false);
            }
            else if (e.Logging.EntryType == EventLogEntryType.SuccessAudit)
            {
                ScriptJqueryMessage(thisPage, e.Logging.Description, JMessageType.Accept, true);
            }
        }
        catch
        {
            ScriptJqueryMessage(thisPage,
                "An error occurred. Please contact the system administrator.<br/>เกิดข้อผิดพลาด โปรดติดต่อผู้ดูแลระบบ", JMessageType.Warning, false);
        }
    }

    public static void MessageSuccess(this Page thisPage, string message)
    {
        thisPage.ScriptJqueryMessage(message, JMessageType.Accept, true);
    }
    public static void MessageInfo(this Page thisPage, string message)
    {
        thisPage.ScriptJqueryMessage(message, JMessageType.Notify, false);
    }
    public static void MessageWarning(this Page thisPage, string message)
    {
        thisPage.ScriptJqueryMessage(message, JMessageType.Warning, false);
    }

    public static void ScriptJqueryMessage(this Page thisPage, string message, JMessageType msgType, bool _isAutoHide)
    {
        string contentStyle = string.Empty;
        string headTitle = string.Empty;

        switch (msgType)
        {
            case JMessageType.Accept:
                contentStyle = "success";
                headTitle = "Success";
                break;
            case JMessageType.Notify:
                contentStyle = "info";
                headTitle = "Notification";
                break;
            case JMessageType.Warning:
                contentStyle = "danger";
                headTitle = "Warning";
                break;
            default:
                break;
        }


        var is_noti = msgType != JMessageType.Accept;

        message = message.Replace("\r\n", "<br/>");

        string AppendElement = "<div id=\"frmMsg\" class=\"card card-inverse card-" + contentStyle + " card-message card-message-" + contentStyle + " message-noti message-noti-" + contentStyle + "\">"
               + "<div class=\"card-header font-weight-bold" + (is_noti ? "" : " text-center") + "\">" + (is_noti ? headTitle + "<div class=\"float-right\" style=\"margin-top:-0.15rem;\"><button id=\"btClose\" type=\"button\" class=\"close text-white\" data-dismiss=\"alert\"><i class=\"fa fa-close\"></i></button></div>" : message) + "</div>"
               + (is_noti ? "<div id=\"divMsg\" class=\"card-block\"> " + message + "</div><div class=\"card-message-footer float-right pr-4 pb-2\"><button id=\"btClosebBottom\" type=\"button\" class=\"btn btn-sm btn-" + contentStyle + "\" data-dismiss=\"alert\">OK</button></div><div class=\"clearfix\"></div>" : "")
               + "</div>";

        AppendElement = AppendElement.Replace("'", "");
        ScriptManager.RegisterStartupScript(thisPage, thisPage.GetType(),
            "JqueryMessage", @"$(document).ready(function() {" +
               @"$('#frmMsg').remove();" +
               @"$('body').append('" + AppendElement + "');" +
               @"$('#btClose').bind('click', function() { $('#frmMsg').fadeOut(300); } );" +
               @"$('#btClosebBottom').bind('click', function() { $('#frmMsg').fadeOut(300); } );" +
               @"$('#frmMsg').fadeIn(300)" + ((_isAutoHide == true) ? ".delay(2000).fadeOut(300)" : "") + ";});", true);
    }
}
