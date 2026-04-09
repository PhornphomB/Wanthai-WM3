using System;
using System.Collections.Generic;
using System.Web.UI;

namespace WMS_NEW.Configuration
{
    public partial class ResourceMaster : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Init PopupEntity 

                var access = new Access.Configuration.ResourceMaster();
                var entity = access.Entity;

                #endregion

                var override_controls = new List<_UControls.EntityCustom>();

                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = access; };
                popupEntity1.InitControlStatic();


                GridExt1.PopupEntitySource = popupEntity1;
                if (!Page.IsPostBack)
                {
                    hidApplicationID.DefaultValue = _SessionVals.AppID;
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }
}