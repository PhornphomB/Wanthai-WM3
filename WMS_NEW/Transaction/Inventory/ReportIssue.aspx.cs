using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Inventory
{
    public partial class ReportIssue : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            iColWarehouse.DropDownQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };
            iColLocation.DropDownQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQueryCode(); };
        }
    }
}