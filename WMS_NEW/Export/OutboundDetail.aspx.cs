using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Export
{
    public partial class OutboundDetail : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Initial Peoperty Column Grid

                GridColumnExt1.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQueryCode(_SessionVals.UserName); };
                GridColumnExt2.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQueryCode(_SessionVals.UserName); };
                GridColumnExt3.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty_Display("outbound_order_type"); };
                GridColumnExt5.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty_Display("outbound_order_status"); };

                #endregion

                if (!Page.IsPostBack)
                {
                    hidSessionUser.SetValue(_SessionVals.UserName);
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