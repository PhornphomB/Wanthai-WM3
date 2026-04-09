using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Inventory
{
    public partial class InventoryOutboundAllocate : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GridColumnExt5.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
            GridColumnExt6.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
            GridColumnExt8.DropDownQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetCodeQueryUserWarehouse(); };
            GridColumnExt11.DropDownQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQuery(); };
        }
    }
}