using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Mod
{
    public partial class InventoryReprint : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                GridExt1.GridRowAfterDataBound += GridExt1_GridRowAfterDataBound;
                GridExt1.GridRowCommandClick += GridExt1_GridRowCommandClick;

                #region Initial Peoperty Column Grid

                GridColumnExt16.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQueryCode(_SessionVals.UserName); };
                GridColumnExt35.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };

                #endregion
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        private void GridExt1_GridRowAfterDataBound(GridViewRowEventArgs e)
        {
            try
            {
                var btnReprint = (e.Row.FindControl("RE_PRINT") as Button);
                btnReprint.OnClientClick = "";
                btnReprint.CssClass = "btn btn-sm btn-info btn-ingrid";
            }
            catch (Exception ex)
            {
                this.Logging = new Prototype.Providers.Logging(this, ex);
                base.RaiseLogging();
            }
        }

        private void GridExt1_GridRowCommandClick(GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "RE_PRINT")
                {
                    Guid id = Guid.Parse(e.CommandArgument.ToString());

                    ucInventoryPrint.InitialForm(id);
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