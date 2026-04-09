using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Export
{
    public partial class InventoryAdjust : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Initial Peoperty Column Grid

            GridColumnExt1.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQueryCode(_SessionVals.UserName); };
            GridColumnExt3.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQueryCode(_SessionVals.UserName); };
            GridColumnExt5.DropDownQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQueryCode(); };
            GridColumnExt17.DropDownQueryProperty = delegate () { return Access.MasterData.Reason.Instance.GetQueryCode(); };

            #endregion

            if (!Page.IsPostBack)
            {

            }
        }
    }
}