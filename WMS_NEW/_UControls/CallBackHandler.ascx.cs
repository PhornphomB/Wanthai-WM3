using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _UControls
{
    public delegate void RaiseCallbackEventHandler(string[] eventArgs, string HandlerId, ref string CallBackResult, ref string ExcutableClientScript, Exception RaiseException);

    partial class CallBackHandler : System.Web.UI.UserControl, ICallbackEventHandler
    {

        private const string C_CallbackHandlerNumberSpliter = "###";
        private const string C_CallbackArgsSpliter = "$$";
        private const string C_CallbackResultSpliter = "$$";

        string returnValue = string.Empty;

        public event RaiseCallbackEventHandler RaiseCustomCallback;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitJustOnFirstLoad();
            }

            InitOnEveryPostBack();
        }


        #region "Callback  Events"

        public string GetCallbackResult()
        {
            return returnValue;
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            //===============  declarations  ==================
            string[] tempArgs = null;
            string HandlerId = null;
            string[] RealArgs = null;

            string ExcutableClientScript = string.Empty;
            string CallbakcResult = string.Empty;

            Exception RaisedException = null;


            try
            {
                tempArgs = eventArgument.Replace(C_CallbackHandlerNumberSpliter, "~").Split('~');

                HandlerId = tempArgs[1].Split(':')[1];

                // removing extra spliters
                if (tempArgs[0].StartsWith(C_CallbackArgsSpliter))
                    tempArgs[0] = tempArgs[0].Remove(0, C_CallbackArgsSpliter.Length);
                if (tempArgs[0].EndsWith(C_CallbackArgsSpliter))
                    tempArgs[0] = tempArgs[0].Remove(tempArgs[0].Length - C_CallbackArgsSpliter.Length, C_CallbackArgsSpliter.Length);

                //extracting parameters which send from client
                RealArgs = tempArgs[0].Replace(C_CallbackArgsSpliter, "~").Split('~');


            }
            catch (Exception ex)
            {
                RaisedException = ex;
            }


            try
            {
                if (RaiseCustomCallback != null)
                {
                    RaiseCustomCallback(RealArgs, HandlerId, ref CallbakcResult, ref ExcutableClientScript, RaisedException);
                }

                returnValue = string.Format("{0}{1}{2}", CallbakcResult, C_CallbackResultSpliter, ExcutableClientScript);
            }
            catch (Exception ex)
            {
                returnValue = string.Format("{0}{1}{2}", "unexpected error!!!", C_CallbackResultSpliter, "");
            }
        }

        #endregion

        #region "Init"


        protected void InitJustOnFirstLoad()
        {
            //to do ...
        }

        public string MethodAsynCall { get; set; }

        protected void InitOnEveryPostBack()
        {
            ClientScriptManager cm = Page.ClientScript;
            string cbReferenceGeneral = cm.GetCallbackEventReference(this, "arg", "ReceiveCustomCallBackData", "context", true);

            string callbackScript1 = "function  " + MethodAsynCall + "_AsynCallServer(arg, context) {" + cbReferenceGeneral + "; };";
            string callbackScript2 = "function  " + MethodAsynCall + "_asyncall(args,handlerId,receiverdivid) {var _arg = args + '" + C_CallbackHandlerNumberSpliter + "' + 'handlerId:' + handlerId ; " + MethodAsynCall + "_AsynCallServer(_arg, receiverdivid); };";

            cm.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString().Replace("-", "_"), callbackScript1, true);
            cm.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString().Replace("-", "_"), callbackScript2, true);
        }
        public CallBackHandler()
        {
            Load += Page_Load;
        }

        #endregion

    }
}