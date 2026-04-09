using _UControls;
using Prototype.Providers;
using Prototype.Providers.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Report.AscxControls;

namespace WMS_NEW.Transaction.Count
{
    public partial class CountReconcil : PageCustom
    {
        protected bool InitLoadTab1
        {
            get
            {
                if (ViewState["InitLoadData1"] == null)
                    ViewState["InitLoadData1"] = false;

                return (bool)ViewState["InitLoadData1"];
            }
            set
            {
                ViewState["InitLoadData1"] = value;
            }
        }

        protected bool InitLoadTab2
        {
            get
            {
                if (ViewState["InitLoadTab2"] == null)
                    ViewState["InitLoadTab2"] = false;

                return (bool)ViewState["InitLoadTab2"];
            }
            set
            {
                ViewState["InitLoadTab2"] = value;
            }
        }

        protected bool InitLoadTab3
        {
            get
            {
                if (ViewState["InitLoadTab3"] == null)
                    ViewState["InitLoadTab3"] = false;

                return (bool)ViewState["InitLoadTab3"];
            }
            set
            {
                ViewState["InitLoadTab3"] = value;
            }
        }

        protected bool InitLoadTab4
        {
            get
            {
                if (ViewState["InitLoadTab4"] == null)
                    ViewState["InitLoadTab4"] = false;

                return (bool)ViewState["InitLoadTab4"];
            }
            set
            {
                ViewState["InitLoadTab4"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ReportViewer.BindingParameter += ReportViewer_BindingParameter;

                #region Binding Event

                comboCountDescOpt.SelectedIndexChanged += ComboCountDescOpt_SelectedIndexChanged;

                GridExt1.GridRowClick += GridExt1_GridRowClick;

                popupCountDetail.CloseClick += popupCountDetail_CloseClick;

                panelTabCountView.TabIndexChanged += PanelTabCountView_TabIndexChanged;

                #endregion

                #region Binging DropDown Property Column Grid

                GridColumnExt8.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQueryCode(_SessionVals.UserName); };
                GridColumnExt2.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("COUNT_STATUS"); };
                GridColumnExt4.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("COUNT_TYPE"); };

                #endregion


                if (!Page.IsPostBack)
                {
                    hidSessionUser.SetValue(_SessionVals.UserName);

                    var list = new List<Prototype.Providers.Property>();
                    list.Add(new Prototype.Providers.Property() { Code = "", Name = "View All" });
                    list.Add(new Prototype.Providers.Property() { Code = "EQ", Name = "Stock Equal" });
                    list.Add(new Prototype.Providers.Property() { Code = "NOT_EQ", Name = "Stock Not Equal" });

                    comboCountDescOpt.AutoPostBack = true;
                    Prototype.Providers.Controls.ControlsList.BindListBoxNoneDefault(ref comboCountDescOpt, list);
                    //SearchByTabActive(false);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    //SearchByTabActive(false);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private void PanelTabCountView_TabIndexChanged(int _index)
        {
            try
            {
                SearchByTabActive(false);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
            ;
        }

        private void ComboCountDescOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hiddesc_view_opt.SetValue(comboCountDescOpt.SelectedValue);

                if (Page.IsPostBack)
                {
                    gridCountDesc.Search();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void popupCountDetail_CloseClick(object sender, EventArgs e)
        {
            try
            {
                hiddetail_count_master_id.SetValue(Guid.Empty);
                hiddesc_count_master_id.SetValue(Guid.Empty);
                hiddesc_view_opt.SetValue("");
                hidrec_count_master_id.SetValue(Guid.Empty);
                hidrec_count_master_id_rfid.SetValue(Guid.Empty);

                InitLoadTab1 = false;
                InitLoadTab2 = false;
                InitLoadTab3 = false;
                InitLoadTab4 = false;
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
                var id = Guid.Parse(_rowKeyValue.ToString());
                BindData(id);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void BindData(Guid _id)
        {
            using (var _acc = new Access.Transaction.Count.CountPlan())
            {
                base.PlugEventResult(_acc);

                var dto = _acc.GetDTOByKeyID(_id);
                if (dto != null)
                {
                    hidCountMasterID.SetValue(dto.count_master_id);

                    txtWarehouseID.SetValue(dto.wh_id);
                    txtCountID.SetValue(dto.count_id);
                    txtCreateDate.SetValue(dto.create_date);
                    txtCountStatus.SetValue(dto.count_status);
                    txtCountType.SetValue(dto.count_type);

                    txtDesc.SetValue(dto.description);
                    txtCreateBy.SetValue(dto.create_by);
                    txtCloseBy.SetValue(dto.close_by);
                    txtCloseDate.SetValue(dto.close_date);
                    txtCloseRemark.SetValue(dto.close_remark);

                    if (dto.count_status == "CLOSE")
                    {
                        txtCloseRemark.Enabled = false;
                        btClosePlan.Enabled = false;
                    }
                    else
                    {
                        txtCloseRemark.Readonly = false;
                        btClosePlan.Enabled = true;
                    }
                    var ent = _acc.GetTotalSummary(dto.count_master_id);
                    if (ent != null)
                    {
                        lblSumCount.InnerText = ent.sum_count.ToString();
                        lblSumDiff.InnerText = ent.sum_diff.ToString();
                        lblSumStock.InnerText = ent.sum_stock.ToString();
                    }
                    var ent_pallet = _acc.GetTotalSummaryPallet(dto.count_master_id);
                    if (ent_pallet != null)
                    {
                        lblPlanPallet.InnerText = ent_pallet.sum_plan_pallet.ToString();
                        lblPalletCount.InnerText = ent_pallet.sum_pallet_count.ToString();
                        lblDIffPlus.InnerText = ent_pallet.sum_diff_plus.ToString();
                        lblDiffMinus.InnerText = ent_pallet.sum_diff_minus.ToString();
                        lblDiffZero.InnerText = ent_pallet.count_diff_zero.ToString();
                    }

                    hiddetail_count_master_id.SetValue(dto.count_master_id);
                    //gridCountDetail.Search();

                    comboCountDescOpt.SelectedIndex = 0;
                    hiddesc_count_master_id.SetValue(dto.count_master_id);
                    hiddesc_view_opt.SetValue("");
                    //gridCountDesc.Search();

                    hidrec_count_master_id.SetValue(dto.count_master_id);
                    //gridCountReconcil.Search();

                    panelTabCountView.ChangeActivePanel(4);

                    hidrec_count_master_id_rfid.SetValue(dto.count_master_id);
                    //gridCountReconcilRfid.Search();

                    

                    SearchByTabActive(true);

                    popupCountDetail.ShowDialog();


                }
            }
        }

        protected void btClosePlan_Click(object sender, EventArgs e)
        {
            try
            {
                using (var _acc = new Access.Transaction.Count.CountPlan())
                {
                    base.PlugEventResult(_acc);

                    string whID = txtWarehouseID.GetValue();
                    string countID = txtCountID.GetValue();
                    string closeRemark = txtCloseRemark.GetValue();

                    string errMsg = string.Empty;
                    string errCode = string.Empty;

                    _acc.ClosePlan(whID, countID, closeRemark, out errMsg, out errCode);

                    if (errCode == "0")
                    {
                        txtCloseRemark.Enabled = false;
                        txtCloseRemark.Update();

                        btClosePlan.Enabled = false;
                        popupCountDetail.UpdateCommand();

                        GridExt1.DataBind();

                        Page.MessageSuccess(errMsg);
                    }
                    else
                    {
                        Page.MessageWarning(errMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                var id = Guid.Parse(hidCountMasterID.GetValue().ToString());
                BindData(id);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        void SearchByTabActive(bool _isManualSearch)
        {
            if (panelTabCountView.TabIndexActive == 1 && ((_isManualSearch) || (!InitLoadTab1)))
            {
                InitLoadTab1 = true;
                gridCountDetail.Search();
            }
            if (panelTabCountView.TabIndexActive == 2 && ((_isManualSearch) || (!InitLoadTab2)))
            {
                InitLoadTab2 = true;
                gridCountDesc.Search();
            }
            if (panelTabCountView.TabIndexActive == 3 && ((_isManualSearch) || (!InitLoadTab3)))
            {
                InitLoadTab3 = true;
                gridCountReconcil.AddFilterCustomInputInclude(hid_active_load);
                gridCountReconcil.Search();
            }
            if (panelTabCountView.TabIndexActive == 4 && ((_isManualSearch) || (!InitLoadTab4)))
            {
                InitLoadTab4 = true;
                gridCountReconcilRfid.Search();
            }
        }

                #region Function Report Viewer

                protected void btPrint_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer.InitialForm("Count");
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

            if (_report_id == "BF781504-5C5E-4393-96FA-E08F8D8A7070")
            {
                prms.Add(new ReportParameter { Name = "@countID", Value = txtCountID.GetValue() });
                prms.Add(new ReportParameter { Name = "@wh_id", Value = txtWarehouseID.GetValue() });
            }
            else if (_report_id == "B19D0A94-93AC-4BDA-9F59-C876BE9EFB14")
            {
                prms.Add(new ReportParameter { Name = "@v_vchCountID", Value = (string)hidCountMasterID.GetValue().ToString() });
            }
            else if (_report_id == "484F431F-6A16-4C98-80A5-12CFA11B2C36")
            {
                prms.Add(new ReportParameter { Name = "@count_id", Value = txtCountID.GetValue() });
            }
            else if (_report_id == "2A43D998-79BF-4132-A27E-A5AA9178F2B0")
            {
                prms.Add(new ReportParameter { Name = "@count_id", Value = txtCountID.GetValue() });
            }
            else if (_report_id == "F54ADFDF-7350-4F88-8ECC-2D74FDE73999")
            {
                prms.Add(new ReportParameter { Name = "@count_id", Value = txtCountID.GetValue() });
            }


            return prms;
        }

        #endregion
    }
}