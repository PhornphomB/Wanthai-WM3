using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using WebSecurityApi.DTO;

namespace WMS_NEW
{
    public class Global : System.Web.HttpApplication
    {

        void InitUserDevices()
        {
           
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            InitUserDevices();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            InitUserDevices();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (FieldsStatic.SessionTimeout == 0)
                FieldsStatic.SessionTimeout = Session.Timeout;

            Session["LOGON_DATE"] = DateTime.MinValue;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            try
            {
                AccessSystem.ClearUsersTimeout();

                Access.Transaction.OutbndManifest.OutbndManifestExt.Instance.ClearTransportData(Session.SessionID);
            }
            catch (Exception ex)
            {

            }
        }
    }
}