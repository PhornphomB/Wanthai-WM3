using _UControls;
using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Report.AscxControls;

namespace WMS_NEW.Transaction.Inventory
{
    public partial class InventoryViewer : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            #region Binding Event

            PanelTab1.TabIndexChanged += PanelTab1_TabIndexChanged;

            ReportViewer.BindingParameter += ReportViewer_BindingParameter;

            GridExtItem.GridRowAfterDataBound += GridExtItem_GridRowAfterDataBound;
            
            #endregion

            #region Initial Data

            ddlWareHouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
            ddlOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
            ddlItemCategory.MethodQueryProperty = delegate () { return Access.MasterData.ItemCategory.Instance.GetQuery(); };
            ddlInventoryStatus.MethodQueryProperty = delegate () { return Access.Transaction.Inventory.InventoryStatus.Instance.GetQueryCode(); ; };
            ddlItem.MethodQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQuery(); };
            ddlLocation.MethodQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetCodeQueryUserWarehouse(); };
            #endregion


            InitTextEnterSearch();


            if (!Page.IsPostBack)
            {
                ddlWareHouse.BindDataSource();
                ddlItemCategory.BindDataSource();
                ddlInventoryStatus.BindDataSource();
                ddlOwner.BindDataSource();
                ddlItem.BindDataSource();
                ddlLocation.BindDataSource();


                if (string.IsNullOrEmpty(Request.QueryString["invw_iexp"])) // For 188
                    SearchByTabActive(false);

                using (var model = new Source.WMSEntities())
                {
                    pnRule.Visible = true;
                    lblExp.Text = "Expired";

                    var entBfExpRule = model.t_wms_rule.Where(w => w.rule_code.ToUpper() == "COLOR_TAB_BEFORE_EXPIRE" && w.is_active == "YES").FirstOrDefault();
                    if (entBfExpRule != null && entBfExpRule.value != null)
                    {
                        pnRule.Visible = true;
                        lblBeforeExp.Text = "Before Expiry date less than " + entBfExpRule.value + " days";
                    }
                    else
                    {
                        pnRule.Visible = false;
                    }
                }
            }
        }

        private void GridExtItem_GridRowAfterDataBound(GridViewRowEventArgs e)
        {
            try
            {
                var isBeforeExp = (bool)DataBinder.Eval(e.Row.DataItem, "isBeforeExp");
                if (isBeforeExp)
                {
                    e.Row.CssClass = "btn-warning";
                }
                var isExp = (bool)DataBinder.Eval(e.Row.DataItem, "isExp");
                if (isExp)
                {
                    e.Row.CssClass = "btn-danger";
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
                // For 188
                if (!Page.IsPostBack && !string.IsNullOrEmpty(Request.QueryString["invw_iexp"]))
                {
                    PanelTab1.ChangeActivePanel(Request.QueryString["invw_iexp"].ToLower() == "true" ? 10 : 6);
                    PanelTab1.UpdateContent();

                    SearchByTabActive(false);
                }
                //
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void InitTextEnterSearch()
        {
            var iTexts = pnFilter.Controls.OfType<_UControls._IInputText>();

            foreach (var itext in iTexts)
            {
                itext.TextEnterChanged += TextEnterSearch;
            }
        }

        void TextEnterSearch(_UControls._IInputText _iText)
        {
            btSearch_Click(null, EventArgs.Empty);

            _iText.Focus();
        }

        void Set_Grid(_UControls.GridViewExt gridEx)
        {
            gridEx.AddFilterInputInclude(ddlWareHouse);
            //gridEx.AddFilterInputInclude(txtLocation);
            gridEx.AddFilterInputInclude(ddlLocation);
            gridEx.AddFilterInputInclude(txtItem);
            //gridEx.AddFilterInputInclude(txtParentLPN);
            gridEx.AddFilterInputInclude(txtLPN);
            gridEx.AddFilterInputInclude(ddlItemCategory);
            gridEx.AddFilterInputInclude(txtBatch);
            gridEx.AddFilterInputInclude(dtpMfgDate);
            gridEx.AddFilterInputInclude(dtpExpiryDate);
            gridEx.AddFilterInputInclude(dtpReceiveDate);
            gridEx.AddFilterInputInclude(ddlInventoryStatus);
            gridEx.AddFilterInputInclude(ddlOwner);
            gridEx.AddFilterInputInclude(txtSerial);
            gridEx.AddFilterInputInclude(txtDGCode);
            gridEx.AddFilterInputInclude(txtItemGrade);
            gridEx.AddFilterInputInclude(txtZone);
            gridEx.AddFilterInputInclude(txtDescription);
            gridEx.AddFilterInputInclude(txtQty);
            gridEx.AddFilterInputInclude(txtQtyAllocate);
        }

        void PanelTab1_TabIndexChanged(int _index)
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
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SearchByTabActive(true);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        protected void btClear_Click(object sender, EventArgs e)
        {
            try
            {
                var listCus = pnFilter.Controls.OfType<_UControls._IInputControl>();
                foreach (var item in listCus)
                {
                    item.Clear();
                    item.Update();
                }

                SearchByTabActive(true);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        protected void btToggle_Click(object sender, EventArgs e)
        {
            try
            {
                if (btToggle.Text.StartsWith("Hide"))
                    btToggle.Text = btToggle.Text.Replace("Hide", "Show");
                else
                    btToggle.Text = btToggle.Text.Replace("Show", "Hide");

                btSearch.Visible = !btSearch.Visible;
                btClear.Visible = !btClear.Visible;

                pnFilter.Visible = !pnFilter.Visible;
                updateFilter.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        #region Properties Tab Load

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

        protected bool InitLoadTab5
        {
            get
            {
                if (ViewState["InitLoadTab5"] == null)
                    ViewState["InitLoadTab5"] = false;

                return (bool)ViewState["InitLoadTab5"];
            }
            set
            {
                ViewState["InitLoadTab5"] = value;
            }
        }

        protected bool InitLoadTab6
        {
            get
            {
                if (ViewState["InitLoadTab6"] == null)
                    ViewState["InitLoadTab6"] = false;

                return (bool)ViewState["InitLoadTab6"];
            }
            set
            {
                ViewState["InitLoadTab6"] = value;
            }
        }

        protected bool InitLoadTab7
        {
            get
            {
                if (ViewState["InitLoadTab7"] == null)
                    ViewState["InitLoadTab7"] = false;

                return (bool)ViewState["InitLoadTab7"];
            }
            set
            {
                ViewState["InitLoadTab7"] = value;
            }
        }

        protected bool InitLoadTab8
        {
            get
            {
                if (ViewState["InitLoadTab8"] == null)
                    ViewState["InitLoadTab8"] = false;

                return (bool)ViewState["InitLoadTab8"];
            }
            set
            {
                ViewState["InitLoadTab8"] = value;
            }
        }

        protected bool InitLoadTab9
        {
            get
            {
                if (ViewState["InitLoadTab9"] == null)
                    ViewState["InitLoadTab9"] = false;

                return (bool)ViewState["InitLoadTab9"];
            }
            set
            {
                ViewState["InitLoadTab9"] = value;
            }
        }

        protected bool InitLoadTab10
        {
            get
            {
                if (ViewState["InitLoadTab10"] == null)
                    ViewState["InitLoadTab10"] = false;

                return (bool)ViewState["InitLoadTab10"];
            }
            set
            {
                ViewState["InitLoadTab10"] = value;
            }
        }

        #endregion


        void SearchByTabActive(bool _isManualSearch)
        {
            if (PanelTab1.TabIndexActive == 1 && ((_isManualSearch) || (!InitLoadTab1)))
            {
                InitLoadTab1 = true;

                Set_Grid(GridExtItem);
                GridExtItem.AddFilterCustomInputInclude(hid_active_load);
                GridExtItem.AddFilterInputInclude(txtAttribute1);
                GridExtItem.Search();
            }
            else if (PanelTab1.TabIndexActive == 2 && ((_isManualSearch) || (!InitLoadTab2)))
            {
                InitLoadTab2 = true;

                Set_Grid(GridExtAging);
                //พี่นัทให้เพิ่ม 27/02/2023
                GridExtAging.AddFilterInputInclude(txtAging);
                GridExtAging.AddFilterInputInclude(txtRemainAging);
                GridExtAging.AddFilterCustomInputInclude(hid_active_load);
    
                GridExtAging.Search();
            }
            //else if (PanelTab1.TabIndexActive == 3 && ((_isManualSearch) || (!InitLoadTab3)))
            //{
            //    InitLoadTab3 = true;

            //    Set_Grid(GridExtSerial);

            //    GridExtSerial.AddFilterCustomInputInclude(hid_active_load);
            //    GridExtSerial.Search();
            //}
            else if (PanelTab1.TabIndexActive == 3 && ((_isManualSearch) || (!InitLoadTab3)))
            {
                InitLoadTab3 = true;

                Set_Grid(GridExtItemSummary);
                GridExtItemSummary.AddFilterCustomInputInclude(hid_active_load);
                GridExtItemSummary.Search();
            }
            else if (PanelTab1.TabIndexActive == 4 && ((_isManualSearch) || (!InitLoadTab4)))
            {
                InitLoadTab4 = true;

                Set_Grid(GridExtBatchSummary);
                GridExtBatchSummary.AddFilterCustomInputInclude(hid_active_load);
                GridExtBatchSummary.Search();
            }
            else if (PanelTab1.TabIndexActive == 5 && ((_isManualSearch) || (!InitLoadTab5)))
            {
                InitLoadTab5 = true;

                Set_Grid(GridExtExpirySummary);
                GridExtExpirySummary.AddFilterCustomInputInclude(hid_active_load);
                GridExtExpirySummary.Search();
            }
            else if (PanelTab1.TabIndexActive == 6 && ((_isManualSearch) || (!InitLoadTab6)))
            {
                InitLoadTab6 = true;

                Set_Grid(GridInventoryEmpty);
                GridInventoryEmpty.AddFilterCustomInputInclude(hid_active_load);
                GridInventoryEmpty.RemoveFilterInputInclude(ddlLocation);

                GridInventoryEmpty.RemoveFilterInputInclude(txtBatch);
                GridInventoryEmpty.RemoveFilterInputInclude(dtpMfgDate);
                GridInventoryEmpty.RemoveFilterInputInclude(dtpExpiryDate);
                GridInventoryEmpty.RemoveFilterInputInclude(txtSerial);


                GridInventoryEmpty.Search();
            }
            else if (PanelTab1.TabIndexActive == 7 && ((_isManualSearch) || (!InitLoadTab7)))
            {
                InitLoadTab7 = true;

                Set_Grid(GridInventoryAttribute);

                GridInventoryAttribute.AddFilterInputInclude(txtAttribute1);
                //GridInventoryAttribute.AddFilterInputInclude(txtAttribute2);
                //GridInventoryAttribute.AddFilterInputInclude(txtAttribute3);
                //GridInventoryAttribute.AddFilterInputInclude(txtAttribute4);
                //GridInventoryAttribute.AddFilterInputInclude(txtAttribute5);

                GridInventoryAttribute.AddFilterCustomInputInclude(hid_active_load);
                GridInventoryAttribute.Search();
            }
            else if (PanelTab1.TabIndexActive == 8 && ((_isManualSearch) || (!InitLoadTab8)))
            {
                InitLoadTab8 = true;

                Set_Grid(gridItemRole);
                gridItemRole.AddFilterCustomInputInclude(hid_active_load);
                gridItemRole.Search();
            }
            // For 188
            else if (PanelTab1.TabIndexActive == 9 && ((_isManualSearch) || (!InitLoadTab9)))
            {
                InitLoadTab9 = true;

                Set_Grid(gridItemExpire);
                gridItemExpire.AddFilterCustomInputInclude(hid_active_load);
                gridItemExpire.Search();
            }
            else if (PanelTab1.TabIndexActive == 10 && ((_isManualSearch) || (!InitLoadTab10)))
            {
                InitLoadTab10 = true;

                Set_Grid(GridExtAgingMfgDate);
                //พี่นัทให้เพิ่ม 27/02/2023
                GridExtAgingMfgDate.AddFilterInputInclude(txtAging);
                GridExtAgingMfgDate.AddFilterInputInclude(txtRemainAging);
                GridExtAgingMfgDate.AddFilterCustomInputInclude(hid_active_load);

                GridExtAgingMfgDate.Search();
            }
            //................
            UpdateQty();
        }
        void UpdateQty()
        {
            double sum_qty = 0;
            double sum_allocate = 0;
            string text_quantity = Access.Configuration.ResourceDetail.GetResource("inventory", "quantity");
            string text_quantity_allocate = Access.Configuration.ResourceDetail.GetResource("inventory", "quantity_allocated");

            if (PanelTab1.TabIndexActive == 1)
            {
                DataTable _dt = GridExtItem.DataSource();
                sum_qty = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity"));
                sum_allocate = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity_allocated"));

                string CssClass = "label-warning";
                string msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity + " : " + sum_qty.ToString(Extensions.FormatDecimal) + "</b></span>";
                string allocate_msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity_allocate + " : " + sum_allocate.ToString(Extensions.FormatDecimal) + "</b></span>";

                GridExtItemQty.Text = msg;
                GridExtItemAllocate.Text = allocate_msg;
                UpdateGridExtItem.Update();
            }
            else if (PanelTab1.TabIndexActive == 2)
            {
                DataTable _dt = GridExtAging.DataSource();
                sum_qty = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity"));
                sum_allocate = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity_allocated"));
               
                string CssClass = "label-warning";
                string msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity + " : " + sum_qty.ToString(Extensions.FormatDecimal) + "</b></span>";
                string allocate_msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity_allocate + " : " + sum_allocate.ToString(Extensions.FormatDecimal) + "</b></span>";

                GridExtAgingQty.Text = msg;
                GridExtAgingAllocate.Text = allocate_msg;
                UpdateGridExtAging.Update();
            }
            //else if (PanelTab1.TabIndexActive == 3)
            //{
            //    DataTable _dt = GridExtSerial.DataSource();
            //    sum_qty = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity"));
            //    sum_allocate = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity_allocated"));

            //    string CssClass = "label-warning";
            //    string msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity + " : " + sum_qty.ToString(Extensions.FormatDecimal) + "</b></span>";
            //    string allocate_msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity_allocate + " : " + sum_allocate.ToString(Extensions.FormatDecimal) + "</b></span>";

            //    GridExtSerialQty.Text = msg;
            //    GridExtSerialAllocate.Text = allocate_msg;
            //    UpdateGridExtSerial.Update();
            //}
            else if (PanelTab1.TabIndexActive == 3)
            {
                DataTable _dt = GridExtItemSummary.DataSource();
                sum_qty = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity"));
                sum_allocate = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity_allocated"));

                string CssClass = "label-warning";
                string msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity + " : " + sum_qty.ToString(Extensions.FormatDecimal) + "</b></span>";
                string allocate_msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity_allocate + " : " + sum_allocate.ToString(Extensions.FormatDecimal) + "</b></span>";

                GridExtItemSummaryQty.Text = msg;
                GridExtItemSummaryAllocate.Text = allocate_msg;
                UpdateGridExtItemSummary.Update();
            }
            else if (PanelTab1.TabIndexActive == 4)
            {
                DataTable _dt = GridExtBatchSummary.DataSource();
                sum_qty = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity"));
                sum_allocate = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity_allocated"));

                string CssClass = "label-warning";
                string msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity + " : " + sum_qty.ToString(Extensions.FormatDecimal) + "</b></span>";
                string allocate_msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity_allocate + " : " + sum_allocate.ToString(Extensions.FormatDecimal) + "</b></span>";

                GridExtBatchSummaryQty.Text = msg;
                GridExtBatchSummaryAllocate.Text = allocate_msg;
                UpdateGridExtBatchSummary.Update();
            }
            else if (PanelTab1.TabIndexActive == 5)
            {
                DataTable _dt = GridExtExpirySummary.DataSource();
                sum_qty = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity"));
                sum_allocate = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity_allocated"));

                string CssClass = "label-warning";
                string msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity + " : " + sum_qty.ToString(Extensions.FormatDecimal) + "</b></span>";
                string allocate_msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity_allocate + " : " + sum_allocate.ToString(Extensions.FormatDecimal) + "</b></span>";

                GridExtExpirySummaryQty.Text = msg;
                GridExtExpirySummaryAllocate.Text = allocate_msg;
                UpdateGridExtExpirySummary.Update();
            }
            else if (PanelTab1.TabIndexActive == 6)
            {
                DataTable _dt = GridInventoryEmpty.DataSource();
                sum_qty = _dt.AsEnumerable().Sum(row => row.Field<int>("qty"));

                string CssClass = "label-warning";
                string msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity + " : " + sum_qty.ToString(Extensions.FormatDecimal) + "</b></span>";

                GridInventoryEmptyQty.Text = msg;
                UpdateGridInventoryEmpty.Update();
            }
            else if (PanelTab1.TabIndexActive == 7)
            {
                DataTable _dt = GridInventoryAttribute.DataSource();
                sum_qty = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity"));
                sum_allocate = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity_allocated"));

                string CssClass = "label-warning";
                string msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity + " : " + sum_qty.ToString(Extensions.FormatDecimal) + "</b></span>";
                string allocate_msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity_allocate + " : " + sum_allocate.ToString(Extensions.FormatDecimal) + "</b></span>";

                GridInventoryAttributeQty.Text = msg;
                GridInventoryAttributeAllocate.Text = allocate_msg;
                UpdateGridInventoryAttribute.Update();
            }
            else if (PanelTab1.TabIndexActive == 8)
            {
                DataTable _dt = gridItemRole.DataSource();
                sum_qty = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity"));

                string CssClass = "label-warning";
                string msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity + " : " + sum_qty.ToString(Extensions.FormatDecimal) + "</b></span>";

                gridItemRoleQty.Text = msg;
                UpdategridItemRole.Update();
            }
            else if (PanelTab1.TabIndexActive == 9)
            {
                DataTable _dt = gridItemExpire.DataSource();
                sum_qty = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity"));
                sum_allocate = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity_allocated"));

                string CssClass = "label-warning";
                string msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity + " : " + sum_qty.ToString(Extensions.FormatDecimal) + "</b></span>";
                string allocate_msg = "<span><b class=\"" + CssClass + "\">"+ text_quantity_allocate + " : " + sum_allocate.ToString(Extensions.FormatDecimal) + "</b></span>";

                gridItemExpireQty.Text = msg;
                gridItemExpireAllocate.Text = allocate_msg;
                UpdategridItemExpire.Update();
            }
            else if (PanelTab1.TabIndexActive == 10)
            {
                DataTable _dt = GridExtAgingMfgDate.DataSource();
                sum_qty = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity"));
                sum_allocate = _dt.AsEnumerable().Sum(row => row.Field<double>("quantity_allocated"));

                string CssClass = "label-warning";
                string msg = "<span><b class=\"" + CssClass + "\">" + text_quantity + " : " + sum_qty.ToString(Extensions.FormatDecimal) + "</b></span>";
                string allocate_msg = "<span><b class=\"" + CssClass + "\">" + text_quantity_allocate + " : " + sum_allocate.ToString(Extensions.FormatDecimal) + "</b></span>";

                GridExtAgingMdfDateQty.Text = msg;
                GridExtAgingMdfDateAllocate.Text = allocate_msg;
                UpdateGridExtAging.Update();
            }
        }
        #region Report Bind Parameters
        protected void btReportStockCard_Click(object sender, EventArgs e)
        {
            try
            {
                ucReportStockCard.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btReport_Click(object sender, EventArgs e)
        {
            try
            {
                ReportViewer.InitialForm("Inventory");
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

            if (_report_id == "5211976B-A8A6-42A9-8C13-A99A972B6DDF")
            {
                var inventoryStatusValues = ddlInventoryStatus.GetValues();
                prms.Add(new ReportParameter { Name = "@in_vchWarehouse", Value = ddlWareHouse.GetValue() == null ? string.Empty : ddlWareHouse.GetText() });
                prms.Add(new ReportParameter { Name = "@in_vchLocation", Value = ddlLocation.GetValue() });
                prms.Add(new ReportParameter { Name = "@in_vchInventoryStatus", Value = inventoryStatusValues.Count() == 0 ? string.Empty : string.Join(",", inventoryStatusValues) });
                prms.Add(new ReportParameter { Name = "@in_vchGrade", Value = txtItemGrade.GetValue() });
                prms.Add(new ReportParameter { Name = "@in_vchDGCode", Value = txtDGCode.GetValue() });
                prms.Add(new ReportParameter { Name = "@in_vchZone", Value = txtZone.GetValue() });
                prms.Add(new ReportParameter { Name = "@in_vchOwner", Value = ddlOwner.GetValue() != null ? ddlOwner.GetValue().ToString() : string.Empty });
                prms.Add(new ReportParameter { Name = "@in_vchExpiryDateTo", Value = dtpExpiryDate.GetValueTo() == null ? string.Empty : dtpExpiryDate.GetValueTo().Value.ToString("yyyyMMdd") });
                prms.Add(new ReportParameter { Name = "@in_vchMFGDateTo", Value = dtpMfgDate.GetValueTo() == null ? string.Empty : dtpMfgDate.GetValueTo().Value.ToString("yyyyMMdd") });
                prms.Add(new ReportParameter { Name = "@in_vchReceiveDateTo", Value = dtpReceiveDate.GetValueTo() == null ? string.Empty : dtpReceiveDate.GetValueTo().Value.ToString("yyyyMMdd") });
                prms.Add(new ReportParameter { Name = "@in_vchItemNumber", Value = txtItem.GetValue() });
                prms.Add(new ReportParameter { Name = "@in_vchParentLPN", Value =/* txtParentLPN.GetValue() */ "" });//
                prms.Add(new ReportParameter { Name = "@in_vchLPN", Value = txtLPN.GetValue() });//
                prms.Add(new ReportParameter { Name = "@in_vchItemCategory", Value = ddlItemCategory.GetValue() != null ? ddlItemCategory.GetText().Split(':')[0] : string.Empty });
                prms.Add(new ReportParameter { Name = "@in_vchLotNumber", Value = txtBatch.GetValue() });
                prms.Add(new ReportParameter { Name = "@in_vchExpiryDate", Value = dtpExpiryDate.GetValue() == null ? string.Empty : dtpExpiryDate.GetValue().Value.ToString("yyyyMMdd") });
                prms.Add(new ReportParameter { Name = "@in_vchMFGDate", Value = dtpMfgDate.GetValue() == null ? string.Empty : dtpMfgDate.GetValue().Value.ToString("yyyyMMdd") });
                prms.Add(new ReportParameter { Name = "@in_vchSerialNumber", Value = txtSerial.GetValue() });
                prms.Add(new ReportParameter { Name = "@in_vchReceiveDate", Value = dtpReceiveDate.GetValue() == null ? string.Empty : dtpReceiveDate.GetValue().Value.ToString("yyyyMMdd") });
                prms.Add(new ReportParameter { Name = "@in_vchUserId", Value = _SessionVals.UserName });
            }
            else if (_report_id == "DEB8B404-3CBF-4EF9-AEFE-B0E7F41928BB")
            {
                prms.Add(new ReportParameter { Name = "@wh_id", Value = ddlWareHouse.GetValue() == null ? string.Empty : ddlWareHouse.GetText() });
                prms.Add(new ReportParameter { Name = "@location", Value = ddlLocation.GetValue() });
                prms.Add(new ReportParameter { Name = "@item", Value = txtItem.GetValue() });
                prms.Add(new ReportParameter { Name = "@LPN", Value = txtLPN.GetValue() });//
                prms.Add(new ReportParameter { Name = "@receive_date", Value = dtpReceiveDate.GetValue() == null ? string.Empty : dtpReceiveDate.GetValue().Value.ToString("yyyyMMdd") });
                prms.Add(new ReportParameter { Name = "@inv_status", Value = ddlInventoryStatus.GetValue() == null ? string.Empty : ddlInventoryStatus.GetText() });
                prms.Add(new ReportParameter { Name = "@Aging", Value = string.Empty });//txtAging.GetValue().ToString()

                string Operator = string.Empty;
                //switch (txtAging.GetFilter().FilterAt)
                //{
                //    case Prototype.Providers.FilterAt.Equal:
                //        Operator = "EQ";
                //        break;
                //    case Prototype.Providers.FilterAt.None:
                //        break;
                //    case Prototype.Providers.FilterAt.MoreThan:
                //        Operator = "UP";
                //        break;
                //    case Prototype.Providers.FilterAt.LessThan:
                //        Operator = "DOWN";
                //        break;
                //    case Prototype.Providers.FilterAt.MoreThanEqual:
                //        Operator = "UP_EQ";
                //        break;
                //    case Prototype.Providers.FilterAt.LessThanEqual:
                //        Operator = "DOWN_EQ";
                //        break;
                //    case Prototype.Providers.FilterAt.NotEqual:
                //        Operator = "NEQ";
                //        break;
                //    default: Operator = string.Empty;
                //        break;
                //}

                prms.Add(new ReportParameter { Name = "@Operator", Value = Operator });
                prms.Add(new ReportParameter { Name = "@in_vchReceiveDateTo", Value = dtpReceiveDate.GetValueTo() == null ? string.Empty : dtpReceiveDate.GetValueTo().Value.ToString("yyyyMMdd") });

            }
            else if (_report_id == "A74468C9-4088-410C-A593-DD865D9EDACF")
            {
                var inventoryStatusValues = ddlInventoryStatus.GetValues();
                prms.Add(new ReportParameter { Name = "@in_vchWarehouse", Value = ddlWareHouse.GetValue() == null ? string.Empty : ddlWareHouse.GetText() });
                prms.Add(new ReportParameter { Name = "@in_vchLocation", Value = ddlLocation.GetValue() });
                prms.Add(new ReportParameter { Name = "@in_vchInventoryStatus", Value = inventoryStatusValues.Count() == 0 ? string.Empty : string.Join(",", inventoryStatusValues) });
                prms.Add(new ReportParameter { Name = "@in_vchGrade", Value = txtItemGrade.GetValue() });
                prms.Add(new ReportParameter { Name = "@in_vchDGCode", Value = txtDGCode.GetValue() });
                prms.Add(new ReportParameter { Name = "@in_vchZone", Value = txtZone.GetValue() });
                prms.Add(new ReportParameter { Name = "@in_vchOwner", Value = ddlOwner.GetValue() != null ? ddlOwner.GetValue().ToString() : string.Empty });
                prms.Add(new ReportParameter { Name = "@in_vchMFGDateTo", Value = dtpMfgDate.GetValueTo() == null ? string.Empty : dtpMfgDate.GetValueTo().Value.ToString("yyyyMMdd") });
                prms.Add(new ReportParameter { Name = "@in_vchExpiryDateTo", Value = dtpExpiryDate.GetValueTo() == null ? string.Empty : dtpExpiryDate.GetValueTo().Value.ToString("yyyyMMdd") });
                prms.Add(new ReportParameter { Name = "@in_vchReceiveDateTo", Value = dtpReceiveDate.GetValueTo() == null ? string.Empty : dtpReceiveDate.GetValueTo().Value.ToString("yyyyMMdd") });
                prms.Add(new ReportParameter { Name = "@in_vchItemNumber", Value = txtItem.GetValue() });
                prms.Add(new ReportParameter { Name = "@in_vchParentLPN", Value = /*txtParentLPN.GetValue()*/ "" });//
                prms.Add(new ReportParameter { Name = "@in_vchLPN", Value = txtLPN.GetValue() });//
                prms.Add(new ReportParameter { Name = "@in_vchItemCategory", Value = ddlItemCategory.GetValue() != null ? ddlItemCategory.GetText().Split(':')[0] : string.Empty });
                prms.Add(new ReportParameter { Name = "@in_vchLotNumber", Value = txtBatch.GetValue() });
                prms.Add(new ReportParameter { Name = "@in_vchMFGDate", Value = dtpMfgDate.GetValue() == null ? string.Empty : dtpMfgDate.GetValue().Value.ToString("yyyyMMdd") });
                prms.Add(new ReportParameter { Name = "@in_vchExpiryDate", Value = dtpExpiryDate.GetValue() == null ? string.Empty : dtpExpiryDate.GetValue().Value.ToString("yyyyMMdd") });
                prms.Add(new ReportParameter { Name = "@in_vchSerialNumber", Value = txtSerial.GetValue() });
                prms.Add(new ReportParameter { Name = "@in_vchReceiveDate", Value = dtpReceiveDate.GetValue() == null ? string.Empty : dtpReceiveDate.GetValue().Value.ToString("yyyyMMdd") });
                prms.Add(new ReportParameter { Name = "@in_vchUserId", Value = _SessionVals.UserName });
            }

            return prms;
        }

        #endregion

    }
}