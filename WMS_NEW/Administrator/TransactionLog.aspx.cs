using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Administrator
{
    public partial class TransactionLog : PageCustom
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //Transaction Log
            GridColumnExt1.DropDownQueryProperty = delegate () { return Access.Transaction.TransLog.TransactionType.Instance.GetQuery(); };
            GridColumnExt2.DropDownQueryProperty = delegate () { return Access.Transaction.TransLog.TransactionSubType.Instance.GetQuery(); };
            GridColumnExt6.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQueryCode(_SessionVals.UserName); };
            GridColumnExt7.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQueryCode(_SessionVals.UserName); };
            GridColumnExt8.DropDownQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQueryCode(); };
            GridColumnExt9.DropDownQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQueryCode(); };
            GridColumnExt12.DropDownQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQuery(); };
            GridColumnExt34.DropDownQueryProperty = delegate () { return Access.MasterData.Reason.Instance.GetQueryCode(); };

            //Process Log
            GridColumnExt44.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQueryCode(_SessionVals.UserName); };
        }
    }
}