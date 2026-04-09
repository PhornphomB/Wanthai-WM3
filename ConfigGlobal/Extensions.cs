using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConfigGlobal
{
    public static class Extensions
    {

        #region Get Client IPAddress

        public static string GetComputerName()
        {
            try
            {
                var value = System.Net.Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["REMOTE_HOST"]).HostName;

                return value;
            }
            catch (Exception Ex)
            {
                string ErrMsg = Ex.Message;
                return GetIP4Address();
            }
        }

        public static string GetIP4Address()
        {
            try
            {
                string IP4Address = String.Empty;

                foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        IP4Address = IPA.ToString();
                        break;
                    }
                }

                if (IP4Address != String.Empty)
                {
                    return IP4Address;
                }

                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        IP4Address = IPA.ToString();
                        break;
                    }
                }

                return IP4Address;
            }
            catch (Exception Ex)
            {
                string ErrMsg = Ex.Message;
                return Guid.NewGuid().ToString();
            }
        }

        #endregion
    }
}