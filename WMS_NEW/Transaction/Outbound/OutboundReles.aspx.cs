using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Report.AscxControls;

namespace WMS_NEW.Transaction.Outbound
{
    public partial class OutboundReles : PageCustom
    {
        Access.Transaction.Outbound.OutboundRelease _accRelease = null;

        delegate void dg_Search();
        event dg_Search eSearch;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                eSearch += new dg_Search(RefesrhData);
                PickListDetail1.dg_CallBackSearch = eSearch;

                _accRelease = new Access.Transaction.Outbound.OutboundRelease();
                this.PlugEventResult(_accRelease);

                ReportViewer1.BindingParameter += ReportViewer_BindingParameter;

                #region Binding Event

                GridExt1.GridRowClick += GridExt1_GridRowClick;
                GridExt1.GridRowCommandClick += GridExt1_GridRowCommandClick;
                GridExt1.GridRowAfterDataBound += GridExt1_GridRowAfterDataBound;
                GridExt1.GridSearching += GridExt1_GridSearching;

                popupConfirmRelease.CloseClick += popupConfirmRelease_CloseClick;

                #endregion

                #region Initial Peoperty Column Grid

                GridColumnExt1.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQueryCode(_SessionVals.UserName); };
                GridColumnExt2.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                GridColumnExt3.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty_Display("outbound_order_type"); };
                GridColumnExt5.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty_Display("outbound_order_status"); };
                GridColumnExt9.DropDownQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQuery(); };

                if (!string.IsNullOrEmpty(Request.QueryString["ord_stat"]))
                {
                    GridColumnExt5.DropDownSelectedValue = Request.QueryString["ord_stat"].ToString();
                }

                #endregion

                #region Initial Input Data

                //Search
                ddlCategory.MethodQueryProperty = delegate () { return Access.MasterData.ItemCategory.Instance.GetQuery(); };

                #endregion


                if (!Page.IsPostBack)
                {
                    //Search
                    hidSessionUser.SetValue(_SessionVals.UserName);
                    hidIsFirstLoad.SetValue("YES");

                    PICK_TYPE = !string.IsNullOrEmpty(Request.QueryString["dir_pick"]) ? PICK_TYPE_USR : PICK_TYPE_SYS;
                    hidPickType.SetValue(PICK_TYPE);


                    btConfirmBackOrder.Visible = _accRelease._Model.t_wms_rule.FirstOrDefault(w => w.rule_code == "RULE_ALLOW_BACK_ORDER") == null ? false : (_accRelease._Model.t_wms_rule.FirstOrDefault(w => w.rule_code == "RULE_ALLOW_BACK_ORDER").value == "YES" ? true : false);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void GridExt1_GridSearching(object sender, EventArgs e)
        {
            if (hidIsFirstLoad.GetValue() == "YES")
                hidIsFirstLoad.SetValue(string.Empty);
        }

        void GridExt1_GridRowAfterDataBound(GridViewRowEventArgs e)
        {
            try
            {
                var order_status = (string)DataBinder.Eval(e.Row.DataItem, "order_status");
                var allow_unrel = (bool)DataBinder.Eval(e.Row.DataItem, "allow_unrel");
                var allow_release_sys = (bool)DataBinder.Eval(e.Row.DataItem, "allow_release");

                var btRel = (Button)e.Row.FindControl("RELEASE");
                btRel.CssClass = "btn btn-sm btn-success btn-ingrid";

                var btUnrel = (Button)e.Row.FindControl("UNREL");
                btUnrel.CssClass = "btn btn-sm btn-warning btn-ingrid";
                btUnrel.Enabled = allow_unrel;

                if ((PICK_TYPE == PICK_TYPE_SYS && !allow_release_sys) || (PICK_TYPE == PICK_TYPE_USR && order_status != "OPEN")) {
                    btRel.Enabled = false;
                } else {
                    //Kritsada : WanThai : 2024-03-04 : ถ้า Order type เป็นตาม Rule RULE_CHECKER_ORDER_TYPE ให้เช็คว่ามีการ Assgin order หรือยัง ถ้ายังจะยังไม่ให้กดปุ่ม Release ได้
                    var allow_release_order_type_checker = (bool)DataBinder.Eval(e.Row.DataItem, "allow_release_order_type_checker");
                    var assign_order_count = (int)DataBinder.Eval(e.Row.DataItem, "assign_order_count");
                    if (allow_release_order_type_checker) {
                        if (assign_order_count == 0) {
                            btRel.Enabled = false;
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

        void GridExt1_GridRowClick(object _rowKeyValue)
        {
            try
            {
                var id = (Guid)_rowKeyValue;
                var dto = Access.Transaction.Outbound.Outbound.Instance.GetDtoByKeyId(id);

                var dto_new = new { dto = dto, is_pick_system = PICK_TYPE == PICK_TYPE_SYS };

                PickListDetail1.InitForm(dto_new);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void GridExt1_GridRowCommandClick(GridViewCommandEventArgs e)
        {
            try
            {
                var id = Guid.Parse(e.CommandArgument.ToString());
                if (e.CommandName == "RELEASE")
                {
                    ReleaseKeyActive = id;
                    CheckRelease();
                }
                else if (e.CommandName == "UNREL")
                {
                    Unrelease(id);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void popupConfirmRelease_CloseClick(object sender, EventArgs e)
        {
            try
            {
                gvConfirmRelease.DataSource = null;
                gvConfirmRelease.DataBind();

                ReleaseKeyActive = Guid.Empty;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btComfirm_Click(object sender, EventArgs e)
        {
            try
            {
                var is_success = SaveRelease();
                if (is_success)
                {
                    gvConfirmRelease.DataSource = null;
                    gvConfirmRelease.DataBind();

                    popupConfirmRelease.HideDialog();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        #region Function Release

        const string PICK_TYPE_SYS = "SYSTEM";
        const string PICK_TYPE_USR = "USER";

        protected string PICK_TYPE
        {
            get
            {
                if (ViewState["PICK_TYPE"] == null)
                    ViewState["PICK_TYPE"] = string.Empty;

                return ViewState["PICK_TYPE"].ToString();
            }
            set
            {
                ViewState["PICK_TYPE"] = value;
            }
        }

        protected Guid ReleaseKeyActive
        {
            get
            {
                if (ViewState["ReleaseKeyActive"] == null)
                    ViewState["ReleaseKeyActive"] = Guid.Empty;

                return (Guid)ViewState["ReleaseKeyActive"];
            }
            set
            {
                ViewState["ReleaseKeyActive"] = value;
            }
        }

        void CheckRelease()
        {
            if (PICK_TYPE == PICK_TYPE_SYS)
            {
                var result = _accRelease.CheckItemQtyPreRelease(ReleaseKeyActive);
                if (result.Count() > 0)
                {
                    gvConfirmRelease.DataSource = result;
                    gvConfirmRelease.DataBind();

                    popupConfirmRelease.HeaderText = "Confirm Release Order";
                    popupConfirmRelease.ShowDialog();
                    return;
                }
                else
                {
                    SaveRelease();
                }
            }
            else
            {
                SaveRelease();
            }
        }

        bool SaveRelease(bool is_back_order = false)
        {
            var is_success = false;

            if (PICK_TYPE == PICK_TYPE_SYS)
            {
                is_success = _accRelease.ReleaseOrderBySystem(ReleaseKeyActive);
            }
            else
            {
                is_success = _accRelease.ReleaseOrderByUser(ReleaseKeyActive);
            }

            var entOrder = _accRelease._Model.t_wms_outbound_master.FirstOrDefault(w => w.outbound_order_master_id == ReleaseKeyActive);
            if (entOrder != null && is_back_order)
            {
                entOrder.is_back_order = "YES";
                _accRelease._Model.SaveChanges();
            }

            if (is_success)
            {
                GridExt1.DataBind();
                ReleaseKeyActive = Guid.Empty;

                Page.MessageSuccess("Release Order Success.");
            }

            return is_success;
        }

        private void Unrelease(Guid id)
        {
            if (_accRelease.UnreleaseOrder(id))
            {
                GridExt1.DataBind();

                Page.MessageSuccess("Unrelease Order Success.");
            }
        }

        #endregion


        #region Function Report Viewer

        protected void btPrint_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.InitialForm("Outbound_Release");
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        List<ReportParameter> ReportViewer_BindingParameter(string _report_id)
        {
            var prms = new List<ReportParameter>();

            GridExt1.SetGridFilterForSearch();
            var filters = GridExt1.FilterGridOptions;

            GridExt1.SetCustomFilterForSearch();
            var entCate = GridExt1.FilterCtrlOptions.Where(w => w.DataFieldValue == ddlCategory.DataFieldValue).FirstOrDefault();
            if (entCate != null && entCate.Value != null)
            {
                filters.Add(new Prototype.Providers.Filter("category_id", Prototype.Providers.FilterAt.Equal, entCate.Value.ToString()));
            }

            var entItem = GridExt1.FilterCtrlOptions.Where(w => w.DataFieldValue == txtItemNumber.DataFieldValue).FirstOrDefault();
            if (entItem != null && entItem.Value != null)
            {
                filters.Add(new Prototype.Providers.Filter("item_number", Prototype.Providers.FilterAt.Contains, entItem.Value.ToString()));
            }

            //var _condition = " AND " + Prototype.Providers.DataFilterSQL.GetSQLCondition(filters).Replace("'", "*").Replace("=", "$").Replace("%", "[tnecrep]");
            var _condition = Prototype.Providers.DataFilterSQL.GetSQLCondition(filters).Replace("'", "*").Replace("=", "$").Replace("%", "[tnecrep]").Replace("#", "%23");
            if (!string.IsNullOrEmpty(_condition))
            {
                _condition = " AND " + _condition;
            }
            prms.Add(new ReportParameter() { Name = "@prmCondition", Value = _condition });


            if (_report_id.ToUpper() == "8053ae30-dba3-4418-848b-0b05ab5cc64f".ToUpper())
                prms.Clear();

            return prms;
        }

        #endregion

        protected void btConfirmBackOrder_Click(object sender, EventArgs e)
        {
            try
            {
                var is_success = SaveRelease(true);
                if (is_success)
                {
                    gvConfirmRelease.DataSource = null;
                    gvConfirmRelease.DataBind();

                    popupConfirmRelease.HideDialog();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private void RefesrhData()
        {
            GridExt1.Search();
        }
    }
}