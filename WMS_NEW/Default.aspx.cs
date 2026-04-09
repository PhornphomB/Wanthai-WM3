using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW
{
    public partial class Default : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            #region Binding Event

            PanelTab1.TabIndexChanged += PanelTab1_TabIndexChanged;


            GridExtItem.GridRowAfterDataBound += GridExtItem_GridRowAfterDataBound;

            #endregion

            #region Initial Data

            ddlWareHouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
            ddlOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
            ddlItemCategory.MethodQueryProperty = delegate () { return Access.MasterData.ItemCategory.Instance.GetQuery(); };
            ddlInventoryStatus.MethodQueryProperty = delegate () { return Access.Transaction.Inventory.InventoryStatus.Instance.GetQueryCode(); ; };
            ddlItem.MethodQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQuery(); };
            #endregion


            InitTextEnterSearch();


            if (!Page.IsPostBack)
            {
                ddlWareHouse.BindDataSource();
                ddlItemCategory.BindDataSource();
                ddlInventoryStatus.BindDataSource();
                ddlOwner.BindDataSource();
                ddlItem.BindDataSource();


                if (string.IsNullOrEmpty(Request.QueryString["invw_iexp"])) // For 188
                    SearchByTabActive(false);

                using (var model = new Source.WMSEntities())
                {
                    var entRule = model.t_wms_rule.Where(w => w.rule_code.ToUpper() == "COLOR_TAB_BEFORE_EXPIRE" && w.is_active == "YES").FirstOrDefault();
                    if (entRule != null && entRule.value != null)
                    {
                        pnRule.Visible = true;
                        lblBeforeExp.Text = "Expiry date less than " + entRule.value + " days";
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
            var iTexts = updateFilter.Controls.OfType<_UControls._IInputText>();

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
            gridEx.AddFilterInputInclude(txtLocation);
            gridEx.AddFilterInputInclude(ddlItem);
            //gridEx.AddFilterInputInclude(txtParentLPN);
            gridEx.AddFilterInputInclude(txtLPN);
            gridEx.AddFilterInputInclude(ddlItemCategory);
            gridEx.AddFilterInputInclude(txtBatch);
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
            gridEx.AddFilterInputInclude(txtQtyAllowcate);
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
                var listCus = updateFilter.Controls.OfType<_UControls._IInputControl>();
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
                {
                    btToggle.Text = btToggle.Text.Replace("Hide", "Show");
                    updateView.Attributes["class"] = "col-sm-12";
                    updateFilter.Visible = false;
                }
                else
                {
                    btToggle.Text = btToggle.Text.Replace("Show", "Hide");
                    updateView.Attributes["class"] = "col-sm-9";
                    updateFilter.Visible = true;
                }

                updateContent.Update();
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
                GridExtItem.Search();
            }
            else if (PanelTab1.TabIndexActive == 2 && ((_isManualSearch) || (!InitLoadTab2)))
            {
                InitLoadTab2 = true;

                Set_Grid(GridExtAging);
                GridExtAging.AddFilterCustomInputInclude(hid_active_load);
                GridExtAging.Search();
            }
            else if (PanelTab1.TabIndexActive == 3 && ((_isManualSearch) || (!InitLoadTab3)))
            {
                InitLoadTab3 = true;

                Set_Grid(GridExtSerial);

                GridExtSerial.AddFilterCustomInputInclude(hid_active_load);
                GridExtSerial.Search();
            }
            else if (PanelTab1.TabIndexActive == 4 && ((_isManualSearch) || (!InitLoadTab4)))
            {
                InitLoadTab4 = true;

                Set_Grid(GridExtItemSummary);
                GridExtItemSummary.AddFilterCustomInputInclude(hid_active_load);
                GridExtItemSummary.Search();
            }
            else if (PanelTab1.TabIndexActive == 5 && ((_isManualSearch) || (!InitLoadTab5)))
            {
                InitLoadTab5 = true;

                Set_Grid(GridExtBatchSummary);
                GridExtBatchSummary.AddFilterCustomInputInclude(hid_active_load);
                GridExtBatchSummary.Search();
            }
            else if (PanelTab1.TabIndexActive == 6 && ((_isManualSearch) || (!InitLoadTab6)))
            {
                InitLoadTab6 = true;

                Set_Grid(GridExtExpirySummary);
                GridExtExpirySummary.AddFilterCustomInputInclude(hid_active_load);
                GridExtExpirySummary.Search();
            }
            else if (PanelTab1.TabIndexActive == 7 && ((_isManualSearch) || (!InitLoadTab7)))
            {
                InitLoadTab7 = true;

                Set_Grid(GridInventoryEmpty);
                GridInventoryEmpty.AddFilterCustomInputInclude(hid_active_load);
                GridInventoryEmpty.RemoveFilterInputInclude(txtLocation);

                GridInventoryEmpty.RemoveFilterInputInclude(txtBatch);
                GridInventoryEmpty.RemoveFilterInputInclude(dtpExpiryDate);
                GridInventoryEmpty.RemoveFilterInputInclude(txtSerial);


                GridInventoryEmpty.Search();
            }
            else if (PanelTab1.TabIndexActive == 8 && ((_isManualSearch) || (!InitLoadTab8)))
            {
                InitLoadTab8 = true;

                Set_Grid(GridInventoryAttribute);

                //GridInventoryAttribute.AddFilterInputInclude(txtAttribute1);
                //GridInventoryAttribute.AddFilterInputInclude(txtAttribute2);
                //GridInventoryAttribute.AddFilterInputInclude(txtAttribute3);
                //GridInventoryAttribute.AddFilterInputInclude(txtAttribute4);
                //GridInventoryAttribute.AddFilterInputInclude(txtAttribute5);

                GridInventoryAttribute.AddFilterCustomInputInclude(hid_active_load);
                GridInventoryAttribute.Search();
            }
            else if (PanelTab1.TabIndexActive == 9 && ((_isManualSearch) || (!InitLoadTab9)))
            {
                InitLoadTab9 = true;

                Set_Grid(gridItemRole);
                gridItemRole.AddFilterCustomInputInclude(hid_active_load);
                gridItemRole.Search();
            }
            // For 188
            else if (PanelTab1.TabIndexActive == 10 && ((_isManualSearch) || (!InitLoadTab10)))
            {
                InitLoadTab10 = true;

                Set_Grid(gridItemExpire);
                gridItemExpire.AddFilterCustomInputInclude(hid_active_load);
                gridItemExpire.Search();
            }
            //................
        }
    }
}