using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Authen
{
    public partial class UserLogon : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                GridExt1.GridRowCanSelectValidate += GridExt1_GridRowCanSelectValidate;

                GridColumnExt5.DropDownQueryProperty = delegate () { return new global::ConfigGlobal.DTO._Global.ActiveType().AsQueryable(); };

                if (!Page.IsPostBack)
                {
                    hidSessionAppID.SetValue(_SessionVals.AppID);
                }
            }
            catch (Exception ex)
            {
                this.Logging = new Prototype.Providers.Logging(this, ex);
                this.RaiseLogging();
            }
        }

        private bool GridExt1_GridRowCanSelectValidate(GridViewRowEventArgs e)
        {
            var allow_clear = (bool)DataBinder.Eval(e.Row.DataItem, "allow_clear");
            return allow_clear;
        }

        protected void btClear_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridExt1.CountListKey() == 0)
                {
                    this.MessageWarning("! Please select user for clear logon.");
                    return;
                }

                using (var _acc = new SecurityM.Access.Config.UserLogon())
                {
                    this.PlugEventResult(_acc);

                    var users = GridExt1.GetListKey().Select(se => se.KeyId.ToString()).ToArray();

                    if (_acc.ClearUserLogon(users, _SessionVals.AppID))
                    {
                        GridExt1.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
                this.Logging = new Prototype.Providers.Logging(this, ex);
                this.RaiseLogging();
            }
        }
    }
}