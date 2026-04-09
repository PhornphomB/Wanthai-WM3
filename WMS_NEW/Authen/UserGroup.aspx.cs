using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Authen
{
    public partial class UserGroup : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Initial Peoperty Column Grid

                GridColumnExt5.DropDownQueryProperty = delegate () { return new global::ConfigGlobal.DTO._Global.ActiveType().AsQueryable(); };

                #endregion

                if (!Page.IsPostBack)
                {
                    hidApplicationID.DefaultValue = _SessionVals.AppID;

                    hidSessionAppID.SetValue(_SessionVals.AppID);
                    hidSessionIsAdmin.SetValue(_SessionVals.IsAdmin);
                }

                popup1.InitObjectsEvent += () => { popup1.ObjectDataAccess = new SecurityM.Access.Master.UserGroup(); };
                popup1.InitControlStatic();

                GridExt1.PopupEntitySource = popup1;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }
}