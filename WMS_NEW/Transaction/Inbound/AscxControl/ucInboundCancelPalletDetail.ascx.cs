using _UControls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW._ExtensionClass;
using WMS_NEW.Report.AscxControls;
using WMS_NEW.Source;

namespace WMS_NEW.Transaction.Inbound.AscxControl
{
    public partial class ucInboundCancelPalletDetail : UControlCustom, IFormRelation
    {
        #region ++ DELEGATE ++
        delegate void dg_Search();
        event dg_Search eSearch;

        public Delegate dg_CallBackSearch;

        #endregion
        public string array_print_label_id
        {
            get
            {

                return (string)ViewState["array_print_label_id"];
            }
            set
            {
                ViewState["array_print_label_id"] = value;
            }
        }
        private Guid wh_master_id
        {
            get
            {
                if (ViewState["wh_master_id"] == null)
                    ViewState["wh_master_id"] = Guid.Empty;

                return (Guid)ViewState["wh_master_id"];
            }
            set
            {
                ViewState["wh_master_id"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            iColIsPrint.DropDownQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("print_status"); };
            iColProductionLine.DropDownQueryProperty = delegate () { return Access.MasterData.ProductionLine.Instance.GetQuery(this.wh_master_id); };
            GridExt1.GridRowCommandClick += GridExt1_GridRowCommandClick;
            GridExt1.GridRowAfterDataBound += GridExt1_GridRowAfterDataBound;
            iColIsCancel.DropDownQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("print_status"); };
        }

        private void GridExt1_GridRowAfterDataBound(GridViewRowEventArgs e)
        {
            try
            {
                //var isPrint = (string)DataBinder.Eval(e.Row.DataItem, "is_print");
                //if (isPrint == "YES")
                //{
                //    e.Row.CssClass = "table-success";
                //}
                var isCancel = (string)DataBinder.Eval(e.Row.DataItem, "is_cancelled");
                var control = e.Row.FindControl("CancelBtn") as Button;
                if (control != null)
                {
                    if (isCancel == "YES")
                    {
                        control.Text = "Uncancel";
                    }
                    else
                    {
                        control.Text = "Cancel";
                    }
                }

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public Action<dynamic> UpdateParent { get; set; }

        public void InitForm(dynamic _obj)
        {
            try
            {

                var ent = (Source.t_wms_inbound_master)_obj;
                gridInboundOrderMasterId.SetValue(ent.inbound_order_master_id);
                wh_master_id = ent.wh_master_id;
                var ucFirstLoad = new _UControls.InputHidden() { DataFieldValue = "_is_first_load" };
                ucFirstLoad.SetValue(true);
                GridExt1.AddFilterCustomInputInclude(ucFirstLoad);
                GridExt1.ClearFilters();
                GridExt1.Search();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private void GridExt1_GridRowCommandClick(GridViewCommandEventArgs e)
        {
            try
            {
                //GridExt1.SetCustomFilterForSearch();
                //var productionLine = GridExt1.FilterGridOptions.FirstOrDefault(x => x.DataPropertyName == "production_line");
                //if (productionLine == null || productionLine.Value == null)
                //{
                //    Page.MessageWarning(clsResource.GetResource("print_label", "validate_production_line"));
                //    return;
                //}
                var keyID = Guid.Parse(e.CommandArgument.ToString());
                if (e.CommandName == "CancelBtn")
                {
                    using(var db = new WMSEntities())
                    {
                        var ent = db.t_wms_print_label.FirstOrDefault(x => x.print_label_id == keyID);
                        if (ent != null)
                        {
                            if(ent.is_cancelled == "NO")
                            {
                                if (ent.is_interface_hana == "YES")
                                {
                                    ent.is_interface_hana = "NO";
                                }
                            }
                            else
                            {
                                if (ent.is_interface_hana != null)
                                {
                                    ent.is_interface_hana = ent.is_interface_hana == "YES" ? "NO" : "YES";
                                }
                            }

                            ent.is_cancelled = ent.is_cancelled == "YES" ? "NO" : "YES";
                            ent.update_by = _SessionVals.UserName;
                            ent.update_date = DateTime.Now;
                            db.SaveChanges();
                            //db.usp_interface_hana_inbound_print();
                            GridExt1.Search();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                this.Logging = new Prototype.Providers.Logging(this, ex);
                this.RaiseLogging();
            }
        }
    }
}