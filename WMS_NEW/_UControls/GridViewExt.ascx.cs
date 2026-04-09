using ConfigGlobal.DTO._Global;
using ConfigGlobal.Interface;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using OfficeOpenXml.Style;
using Prototype.Providers;
using Prototype.Providers.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace _UControls
{
    #region Delegate

    public delegate void GridRowDataHandler(GridViewRowEventArgs e);
    public delegate void GridRowCommandHandler(GridViewCommandEventArgs e);
    public delegate void GridRowEventHandler(object _rowKeyValue);
    public delegate bool GridRowValidateHandler(GridViewRowEventArgs e);
    public delegate string GridRowEventTextHandler(object _rowKeyValue, string _rowDataField, string _rowTextValue);
    public delegate void GridExportDataHandler(ref DataTable dtExport);
    public delegate bool GridValidateHandler();
    public delegate List<KeySelect> GridListKeyHandler();

    public delegate void GridExportTemplate();

    public delegate void GridRowExtendEventHandler(object _rowKeyValue, GridIncFieldValue[] _values);

    #endregion

    public enum KeyType
    {
        String = 0,
        Integer = 1,
        Guid = 2
    }

    public enum TextWordSpaceType
    {
        None = 0,
        Underscore = 1,
        CharCapital = 2
    }

    public class GridOptionField
    {
        public bool IsSelect { get; set; }
        public string ColumnName { get; set; }
        public int Index { get; set; }
    }

    public class GridColumnAttr
    {
        public string FieldName { get; set; }
        public string TypeName { get; set; }

        public string HeaderText { get; set; }
    }

    public class GridKeepField
    {
        public string UniqueID { get; set; }
        public List<GridColumnAttr> Columns { get; set; }
    }

    public class GridIncFieldValue
    {
        public string DataField { get; set; }
        public object Value { get; set; }
    }


    public partial class GridViewExt : UserControl
    {
        #region Logging

        public global::Prototype.Providers.Logging Logging;
        public event global::Prototype.Providers.EventHandler EventResulted;

        public delegate object GetObjectHandler();

        public void RaiseLogging()
        {
            this.Logging.Raise(EventResulted);
        }

        public void PlugEventResult(dynamic _objectEventResulted)
        {
            _objectEventResulted.EventResulted += new Prototype.Providers.EventHandler(_Object_EventResulted);
        }

        private void _Object_EventResulted(object sender, Prototype.Providers.EventArgsCustom e)
        {
            Page.ShowEventLogging(e);
        }

        #endregion

        #region Custom Template

        private ITemplate _template1 = null;

        [TemplateContainer(typeof(TemplateControl))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate CustomSearchTemplate
        {
            get
            {
                return _template1;
            }
            set
            {
                _template1 = value;
            }
        }

        private ITemplate _template2 = null;

        [TemplateContainer(typeof(TemplateControl))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate CustomCommandTemplate
        {
            get
            {
                return _template2;
            }
            set
            {
                _template2 = value;
            }
        }

        private ITemplate _template3 = null;

        [TemplateContainer(typeof(TemplateControl))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate CustomColumnTemplate
        {
            get
            {
                return _template3;
            }
            set
            {
                _template3 = value;
            }
        }

        private ITemplate _template4 = null;

        [TemplateContainer(typeof(TemplateControl))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate CustomColumnGroupTemplate
        {
            get
            {
                return _template4;
            }
            set
            {
                _template4 = value;
            }
        }



        #endregion


        List<Resource> _resourceGeneral = new List<Resource>();

        public event GridValidateHandler GridSearchValidate;
        public event System.EventHandler GridSearching;
        public event System.EventHandler GridSearched;

        public event System.EventHandler GridDeleted;

        public event GridListKeyHandler GridListKeyDBCustom;
       
        string GridUniqueID
        {
            get
            {
                return Request.GetCurrentPageName().Replace(".aspx", "") + "_" + gvView.UniqueID;
            }
        }

        void Page_Init(object sender, EventArgs e)
        {

            #region Initial Template

            if (_template1 != null)
            {
                _template1.InstantiateIn(placeCustomSearch);
            }
            if (_template2 != null)
            {
                _template2.InstantiateIn(placeCustomCommand);
            }
            if (_template3 != null)
            {
                _template3.InstantiateIn(placeCustomColumns);
            }
            if (_template4 != null)
            {
                _template4.InstantiateIn(placeCustomColumnGroups);
            }


            #endregion

            Page.InitComplete += Page_InitComplete;
        }

        private void Page_InitComplete(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    _gridKeepField = (List<GridKeepField>)Session["GridKeepField"];
                    var hasGrid = _gridKeepField.FirstOrDefault(x => x.UniqueID == GridUniqueID);
                    if (hasGrid != null)
                    {
                        _gridKeepField.Remove(hasGrid);

                        Session["GridKeepField"] = _gridKeepField;
                    }
                }

                InitColumnsByI();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.EventResulted += new Prototype.Providers.EventHandler(_Object_EventResulted);

                comboLimit.SelectedIndexChanged += ComboLimit_SelectedIndexChanged;

                if (GridRefreshClick == null)
                    PagerGridExt1.RefreshClick += btSearchConfirm_Click;
                else
                    PagerGridExt1.RefreshClick += GridRefreshClick;

                this.IsTriggerSearch = true;

                if (_resourceGeneral.Count == 0)
                {
                    _resourceGeneral = (Session["Resource_General"] as List<ConfigGlobal.Interface.Resource>);
                }

                if (!Page.IsPostBack)
                {
                    // Set First Initial PageLoad Before InitialGridFilter
                    InitDisplayResourceByI();

                    // New Option 05-2016
                    SetValidateGroupInput();
                }

                InitialGridFilter(); //Initial Grid Filter Control for Can Save ViewState of Control Postback in this Page

                if (PopupEntitySource != null)
                    PopupEntitySource.RaiseEntitySaved += PopupEntitySource_RaiseEntitySaved;

                if (!Page.IsPostBack)
                {
                    if (IsFirstShowFilter == null)
                        IsFirstShowFilter = true;

                    if (DisableExport == null)
                        DisableExport = false;

                    if (VisibleExportTemplate == null)
                        VisibleExportTemplate = false;

                    btTemplate.Visible = VisibleExportTemplate.Value;

                    panelExport.Visible = !DisableExport.Value;

                    //if (this.LimitPageCustom == 0)
                    //    this.LimitPageCustom = 50;


                    if (GridWidth.Value > 0)
                        gvView.Width = GridWidth;
                    else
                        gvView.Width = new Unit(_gridColumnsWidth, UnitType.Pixel);

                    #region Initial Resource Language
                    //Hide Filter
                    var res_HideFilter = _resourceGeneral.FirstOrDefault(qry => qry.ResourceGroup == "General" && qry.ResourceName == "Hide Filter");
                    if (res_HideFilter != null) btSearch.Text = res_HideFilter.ResourceValue;
                    else btSearch.Text = "Hide Filter";

                    //Search
                    var res_Hide = _resourceGeneral.FirstOrDefault(qry => qry.ResourceGroup == "General" && qry.ResourceName == "Search");
                    if (res_Hide != null) btSearchConfirm.Text = res_Hide.ResourceValue;

                    //Template Excel
                    var res_Template = _resourceGeneral.FirstOrDefault(qry => qry.ResourceGroup == "General" && qry.ResourceName == "TemplateExcel");
                    if (res_Template != null) btTemplate.Text = res_Template.ResourceValue;

                    #endregion

                    if (this.LimitPageCustom > 0)
                    {
                        comboLimit.Visible = false;
                        gvView.PageSize = this.LimitPageCustom;
                    }
                    else
                    {
                        //Limit
                        var listLimit = new List<Prototype.Providers.Property>
                         {
                            new Prototype.Providers.Property() { Code = "20", Name = "Limit 20" },
                            new Prototype.Providers.Property() { Code = "50", Name = "Limit 50" },
                            new Prototype.Providers.Property() { Code = "100", Name = "Limit 100" },
                             //new Prototype.Providers.Property() { Code = "500", Name = "Limit 500" }
                         }; ;

                        comboLimit.AutoPostBack = true;

                        ControlsList.BindListBoxNoneDefault(ref comboLimit, listLimit);

                        gvView.PageSize = Convert.ToInt32(comboLimit.SelectedValue);
                    }

                    if (GridFilterInitValue != null)
                        GridFilterInitValue(null, EventArgs.Empty);

                    this.IsTriggerSearch = !DisableFirstSearch;

                    if (DisableFirstSearch == false)
                        this.Search(); // Initial Search

                    #region New Option 03/1026

                    if (GridFilters.Count == 0)
                    {
                        DisableSearch = true;
                    }

                    //if (ShowOptionViewColumn)
                    //{
                    //    ViewOptionField();

                    //    btOptField.Visible = true;
                    //    panelOptField.CssClass = "panel-option-field";
                    //}

                    #endregion

                    if (!this.IsFirstShowFilter.Value)
                    {
                        panelContentFilter.Visible = false;

                        var res_Filter = _resourceGeneral.FirstOrDefault(qry => qry.ResourceGroup == "General" && qry.ResourceName == "Filter");
                        if (res_Filter != null) btSearch.Text = res_Filter.ResourceValue;
                        else btSearch.Text = "Filter";
                    }

                    btNew.Visible = NewVisible && ((PopupEntitySource != null) || (NewClick != null));


                }

                ColumnFreezeLength = 0;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e) //Page Stat working after Load Control events
        {
            try
            {
                var eventControl = Request.Params.Get("__EVENTTARGET");
                var eventArgument = Request.Params.Get("__EVENTARGUMENT");

                if ((!string.IsNullOrEmpty(eventArgument) && eventArgument.ToUpper().Contains(this.SEARCH_ENTER_KEY.ToUpper())) == false) // Not Eq Enter Search
                    BindScriptClient();


                if (!Page.IsPostBack)
                {

                }
                else //Manual Event Postback of Trigger Control
                {
                    if (eventControl == gvView.ClientID && eventArgument != string.Empty)
                    {
                        var args = eventArgument.Split('|');

                        if (args[0].ToUpper() == "FILTER") //Grid Filter Click
                        {
                            GridFilterChange(args[1]);
                        }
                        else if (args[0].ToUpper() == "SORT") //Grid Sort Click
                        {
                            GridSorting("SORT", args[1]);
                        }
                        else if (args[0].ToUpper() == "UNSORT") //Grid Unsort Click
                        {
                            GridSorting("UNSORT", args[1]);
                        }
                    }
                }

                if (!this.GridFilters.Any(x => x.IsShowFilter) && btSearch.Text == "Hide Filter")
                {
                    btSearch.Enabled = false;
                    updateContentSearch.Update();

                    btSearch_Click(null, EventArgs.Empty);
                }
                else if (IsGridColumnFilter && btSearch.Text == "Filter")
                {
                    btSearch.Enabled = true;
                    updateContentSearch.Update();

                    btSearch_Click(null, EventArgs.Empty);
                }

                if (panelContentFilter.Visible && (DisableSearchAll || DisableSearch))
                    panelContentFilter.Visible = false;

                SetGridHeaderFilterStyle();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void BindScriptClient()
        {
            #region Initial Grid FixHeader and Scrollbar

            if (!Request.Browser.IsMobileDevice)
            {
                //panelContentPage.Attributes.Remove("style");

                //if (this.AutoSize == true)
                //{
                //    if (this.GridRowCountByPage > 0)
                //        panelContentPage.Attributes.Add("style", "padding-right: 10px !important;");
                //    else
                //        panelContentPage.Attributes.Add("style", "padding-right: 6px !important;");
                //}
                //else
                //{
                //    if ((Page.Master as WMS_NEW.Layout).MenuTogglerHide)
                //    {
                //        if (this.GridRowCountByPage > 0)
                //            panelContentPage.Attributes.Add("style", "padding-right: 10px !important;");
                //        else
                //            panelContentPage.Attributes.Add("style", "padding-right: 6px !important;");
                //    }

                //    updateContentPage.Update();
                //}

                SetGridHeaderFixScroll();
            }

            #endregion

            #region Set Style Color Grid SelectBox

            if (this.GridAllowSelectBox && !this.GridHighlightAllRow)
            {
                Page.ScriptPageRegister("$('document').ready(function () { checkSelectAllRow('#" + gvView.ClientID + "','#" +
                                  hidSelectRowCountByPage.ClientID + "'); setTableStyleColor('#" + gvView.ClientID + "'); });", gvView.ClientID + "_Style_SelectBox");
            }

            #endregion


            Page.ScriptPageRegister("$('document').ready(function () { gridRowClickHighlight('#" + gvView.ClientID + "'); });", gvView.ClientID + "_gridRowClickHighlight");
        }

        void SetGridHeaderFixScroll()
        {
            if (this.GridRowCountByPage > 0)
            {
                var clientScript = @" $('document').ready(function () {";
                clientScript += "$('#" + gvView.ClientID + "').gridviewScroll({" +
                    "width: '99.9%'," +
                    "height: 550,";

                clientScript += "startVertical: $('#" + hfGridView1SV.ClientID + "').val()," +
                    "startHorizontal: $('#" + hfGridView1SH.ClientID + "').val()," +
                    "onScrollVertical: function (delta) {" +
                        "$('#" + hfGridView1SV.ClientID + "').val(delta);" +
                    "}," +
                    "onScrollHorizontal: function (delta) {" +
                        "$('#" + hfGridView1SH.ClientID + "').val(delta);" +
                    "}" +
                "});";

                clientScript += "});";

                Page.ScriptPageRegister(clientScript, gvView.ClientID + "_HeaderFixScroll");
            }

            //updateContentView.Update();
        }

        void SetGridHeaderFilterStyle()
        {
            #region Set Style Grid Header Filter

            this._gridFilters = this.GridFilters;

            if (this._gridFilters.Count() > 0)
            {
                var hasFilters = this._gridFilters.Select(se => new
                {
                    DataField = se.DataField,
                    Active = se.IsShowFilter
                }).ToList();

                var serializer = new JavaScriptSerializer();
                string jsonString = serializer.Serialize(hasFilters);
                hidFieldHasFilter.Value = jsonString;

                Page.ScriptPageRegister("$('document').ready(function () { gridSetStyleHeaderFilter('" + gvView.ClientID + "','" + hidFieldHasFilter.ClientID + "'); });", gvView.ClientID + "_Style_HeaderFilter");
            }

            #endregion
        }


        private void ComboLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack && gvView.PageSize != Convert.ToInt32(comboLimit.SelectedValue))
                {
                    gvView.PageSize = Convert.ToInt32(comboLimit.SelectedValue);
                    gvView.PageIndex = 0;
                    DataBind();
                }
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
                if (btSearch.Text == "Filter")
                {
                    var res_hideFilter = _resourceGeneral.FirstOrDefault(qry => qry.ResourceGroup == "General" && qry.ResourceName == "Hide Filter");
                    if (res_hideFilter != null) btSearch.Text = res_hideFilter.ResourceValue;
                    else btSearch.Text = "Hide Filter";
                }
                else
                    btSearch.Text = "Filter";

                panelContentFilter.Visible = !panelContentFilter.Visible;
                updateContentFilter.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btSearchConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridSearching != null)
                    GridSearching(sender, e);

                this.Search();

                if (GridSearched != null)
                    GridSearched(sender, e);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btSearchClose_Click(object sender, EventArgs e)
        {
            try
            {
                btSearch.Text = "Filter";
                updateContentSearch.Update();

                panelContentFilter.Visible = false;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private bool IsGridColumnFilter { get; set; }


        #region Properties Optional

        public int LimitPageCustom
        {
            get
            {
                if (ViewState["LimitPageCustom"] == null)
                    ViewState["LimitPageCustom"] = 0;

                return (int)ViewState["LimitPageCustom"];
            }
            set
            {
                ViewState["LimitPageCustom"] = value;
            }
        }

        public bool GridHighlightAllRow
        {
            get
            {
                if (ViewState["GridHighlightAllRow"] == null)
                    ViewState["GridHighlightAllRow"] = false;

                return (bool)ViewState["GridHighlightAllRow"];
            }
            set
            {
                ViewState["GridHighlightAllRow"] = value;
            }
        }

        public bool AutoSize { get; set; }
        public bool DisableRowNo
        {
            set
            {
                ViewState["DisableRowNo"] = value;
            }
            get
            {
                if (ViewState["DisableRowNo"] == null)
                    ViewState["DisableRowNo"] = false;

                return (bool)ViewState["DisableRowNo"];
            }
        }
        public bool DisableSearchAll
        {
            set
            {
                ViewState["DisableSearchAll"] = value;

                panelSearch.Visible = !value;
                panelContentFilter.Visible = !value;
            }
            get
            {
                if (ViewState["DisableSearchAll"] == null)
                    ViewState["DisableSearchAll"] = false;

                return (bool)ViewState["DisableSearchAll"];
            }
        }
        public bool DisableSearch
        {
            set
            {
                ViewState["DisableSearch"] = value;

                btSearch.Visible = !value;
                panelContentFilter.Visible = !value;
            }
            get
            {
                if (ViewState["DisableSearch"] == null)
                    ViewState["DisableSearch"] = false;

                return (bool)ViewState["DisableSearch"];
            }
        }
        public bool? DisableExport
        {
            get
            {
                return (bool?)ViewState["DisableExport"];
            }
            set
            {
                ViewState["DisableExport"] = value;
            }
        }

        public bool? VisibleExportTemplate
        {
            get
            {
                return (bool?)ViewState["VisibleExportTemplate"];
            }
            set
            {
                ViewState["VisibleExportTemplate"] = value;
            }
        }



        public Unit GridWidth { get; set; }

        public HorizontalAlign PageAlign
        {
            set
            {
                PagerGridExt1.Align = value;
            }
        }

        public HorizontalAlign GridAlign
        {
            set
            {
                gvView.HorizontalAlign = value;
            }
        }

        public bool GridAllowSelectBox
        {
            get
            {
                if (ViewState["GridAllowSelectBox"] == null)
                    ViewState["GridAllowSelectBox"] = false;

                return (bool)ViewState["GridAllowSelectBox"];
            }
            set
            {
                ViewState["GridAllowSelectBox"] = value;
                gvView.Columns[1].Visible = value;
            }
        }

        //2024-02-20 Krit add new allow/hide select all
        public bool GridAllowShowSelectBoxAll {
            get {
                if (ViewState["GridAllowShowSelectBoxAll"] == null)
                    ViewState["GridAllowShowSelectBoxAll"] = false;

                return (bool)ViewState["GridAllowShowSelectBoxAll"];
            }
            set {
                ViewState["GridAllowShowSelectBoxAll"] = value;               
            }
        }

        public bool GridAllowRowClick
        {
            set
            {
                gvView.Columns[2].Visible = value;
            }
        }
        public bool GridAllowRowEdit
        {
            get
            {
                if (ViewState["GridAllowRowEdit"] == null)
                    ViewState["GridAllowRowEdit"] = false;

                return (bool)ViewState["GridAllowRowEdit"];
            }
            set
            {
                ViewState["GridAllowRowEdit"] = value;
                gvView.Columns[3].Visible = value;
            }
        }
        public bool GridAllowRowDelete
        {
            get
            {
                if (ViewState["GridAllowRowDelete"] == null)
                    ViewState["GridAllowRowDelete"] = false;

                return (bool)ViewState["GridAllowRowDelete"];
            }
            set
            {
                ViewState["GridAllowRowDelete"] = value;
                gvView.Columns[4].Visible = value;
            }
        }

        public bool DisableFirstSearch { get; set; }
        public bool? IsFirstShowFilter
        {
            get
            {
                return (bool?)ViewState["IsFirstShowFilter"];
            }
            set
            {
                ViewState["IsFirstShowFilter"] = value;
            }
        }

        #region New Option 03/2016

        public bool AutoGenerateColumn { get; set; }
        public TextWordSpaceType AutoGenHeaderTextSpace { get; set; }

        public string AutoGenColumnIndexs_SEARCH { get; set; }
        public string AutoGenColumnIndexs_SORT { get; set; }
        public string AutoGenColumnFields_Exclude { get; set; }

        public string AutoGenColumnFields_SEARCH { get; set; }

        //public bool ShowOptionViewColumn { get; set; }

        public int ColumnFreezeLength { get; set; }

        public bool ShowAllFilter { get; set; }
        public bool ShowAllSort { get; set; }

        #endregion

        #endregion


        #region ObjectDataSource Properties

        public string SourceAssemblyName
        {
            get
            {
                if (ViewState["SourceAssemblyName"] == null)
                    ViewState["SourceAssemblyName"] = string.Empty;

                return (string)ViewState["SourceAssemblyName"];
            }
            set
            {
                ViewState["SourceAssemblyName"] = value;
            }
        }
        public string SourceClassName
        {
            get
            {
                return objDataSource.TypeName;
            }
            set
            {
                objDataSource.TypeName = value;
            }
        }

        private List<_IInputControl> FilterControlInclude { get; set; }
        private List<_IInputControl> FilterCustomControlInclude { get; set; }

        public void AddFilterInputInclude(_IInputControl _iCtrl)
        {
            if (FilterControlInclude == null)
                FilterControlInclude = new List<_IInputControl>();

            FilterControlInclude.Add(_iCtrl);
        }
        public void AddFilterCustomInputInclude(_IInputControl _iCtrl)
        {
            if (FilterCustomControlInclude == null)
                FilterCustomControlInclude = new List<_IInputControl>();

            FilterCustomControlInclude.Add(_iCtrl);
        }

        public void RemoveFilterInputInclude(_IInputControl _iCtrl)
        {
            if (FilterControlInclude == null)
                return;

            FilterControlInclude.Remove(_iCtrl);
        }

        IEnumerable<_IInputControl> GetGridIFilterControls()
        {
            var ctrlsShowFilter = GridFilters.Where(qry => qry.IsShowFilter == true);

            var iControls = EntityExt.IControls.Where(x => ctrlsShowFilter.Any(any => any.DataField == x.DataFieldValue));

            return iControls;
        }

        IEnumerable<_IInputControl> GetIFilterCustomControls()
        {
            var iControls = EntityExt.IControls.Where(x => !GridFilters.Any(any => any.DataField == x.DataFieldValue));

            return iControls;
        }

        int CountSearchControls
        {
            get
            {
                var gridFilter = GetGridIFilterControls().Count();
                var gridFilterCustom = GetIFilterCustomControls().Count();

                return (gridFilterCustom + gridFilter);
            }
        }

        public void Search()
        {
            var isValidated = true;

            if (GridSearchValidate != null)
                isValidated = GridSearchValidate();

            if (isValidated)
            {
                SetGridFilterForSearch();
                SetCustomFilterForSearch();
                SearchBinding();
            }
        }
        public void Search(FilterList _filterGridOptions)
        {
            this.FilterGridOptions = _filterGridOptions;
            SearchBinding();
        }

        public void SetGridFilterForSearch()
        {
            var iControlList = GetGridIFilterControls();

            var _listGrid = new Prototype.Providers.FilterList();

            foreach (_UControls._IInputControl iCtrl in iControlList)
            {
                if (iCtrl.IsFilter == true && ((iCtrl.GetObjectValue() != null) || (iCtrl.GetFilter().FilterAt == FilterAt.Empty)))
                    _listGrid.Add(iCtrl.GetFilter());
            }

            #region Extendsion Include Control Alternate Page

            if (FilterControlInclude != null)
            {
                foreach (var iCtrl in FilterControlInclude)
                {
                    if (iCtrl.IsFilter == true && ((iCtrl.GetObjectValue() != null) || (iCtrl.GetFilter().FilterAt == FilterAt.Empty)))
                        _listGrid.Add(iCtrl.GetFilter());
                }
            }

            #endregion

            this.FilterGridOptions = _listGrid;
        }

        public void SetCustomFilterForSearch()
        {
            var iControlList = GetIFilterCustomControls();
            var customSearch = placeCustomSearch.Controls.OfType<_IInputControl>();

            var _listCustom = new Prototype.Providers.FilterCustom();

            foreach (var iCtrl in iControlList)
            {
                var cusiCtrl = customSearch.FirstOrDefault(x => x.DataFieldValue == iCtrl.DataFieldValue);
                if (cusiCtrl != null)
                {
                    var refVal = cusiCtrl.GetObjectValue();

                    if (cusiCtrl.InputType == InputType.Hidden && refVal != null)
                        iCtrl.SetObjectValue(refVal);
                }

                if (!(iCtrl is _IInputTextDate))
                {
                    _listCustom.Add(iCtrl.DataFieldValue, iCtrl.GetObjectValue(), iCtrl.DefaultFilter);
                }
                else
                {
                    var iDate = (_IInputTextDate)iCtrl;
                    _listCustom.Add(iDate.DataFieldValue, iDate.GetObjectValue(), iDate.GetObjectValueTo(), iCtrl.DefaultFilter);
                }
            }

            #region Extendsion Include Control Alternate Page

            if (FilterCustomControlInclude != null)
            {
                foreach (var iCtrl in FilterCustomControlInclude)
                {
                    if (!(iCtrl is _IInputTextDate))
                    {
                        _listCustom.Add(iCtrl.DataFieldValue, iCtrl.GetObjectValue(), iCtrl.DefaultFilter);
                    }
                    else
                    {
                        var iDate = (_IInputTextDate)iCtrl;
                        _listCustom.Add(iDate.DataFieldValue, iDate.GetObjectValue(), iDate.GetObjectValueTo(), iCtrl.DefaultFilter);
                    }
                }
            }

            #endregion

            this.FilterCtrlOptions = _listCustom;
        }

        private void SearchBinding()
        {
            if (this.GridAllowSelectBox)
            {
                DeleteAllKey(); //Reset key row selected by search
            }

            if (string.IsNullOrEmpty(gvView.DataSourceID)) // If this cast after call method 'DataUnBind'
            {
                gvView.DataSourceID = objDataSource.ID;
            }

            gvView.PageIndex = 0;
            gvView.DataBind();

            BindScriptClient();

            updateContentView.Update();
            updateContentFilter.Update();
            updateContentPage.Update();
        }

        public IQueryable<KeyType> GetAllKeyQuery<KeyType>()
        {
            try
            {
                var objHand = Activator.CreateInstance(this.SourceAssemblyName, this.SourceClassName);
                var func = (Prototype.Providers.IGridKeyData)objHand.Unwrap();

                return func.GetQueryAllKey<KeyType>(this.FilterGridOptions, this.FilterCtrlOptions, KeyField);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();

                return null;
            }
        }
        public List<KeyType> GetAllKeyList<KeyType>()
        {
            return this.GetAllKeyQuery<KeyType>().ToList();
        }

        public FilterList FilterGridOptions
        {
            get
            {
                if (ViewState["FilterGridOptions"] == null)
                    return new Prototype.Providers.FilterList();
                else
                    return (Prototype.Providers.FilterList)ViewState["FilterGridOptions"];
            }
            set
            {
                ViewState["FilterGridOptions"] = value;
            }
        }
        public FilterCustom FilterCtrlOptions
        {
            get
            {
                if (ViewState["FilterCtrlOptions"] == null)
                    return new Prototype.Providers.FilterCustom();
                else
                    return (Prototype.Providers.FilterCustom)ViewState["FilterCtrlOptions"];
            }
            set
            {
                ViewState["FilterCtrlOptions"] = value;
            }
        }

        public bool IsTriggerSearch = false;
        protected void objDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            try
            {
                if (this.IsTriggerSearch == false) // Block Case GridView Auto Binding
                {
                    e.Cancel = true;
                }

                e.InputParameters["_filterGrid"] = this.FilterGridOptions;
                e.InputParameters["_filterCustom"] = this.FilterCtrlOptions;
                e.InputParameters["_sortDefault"] = this.GridSortDefault;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        protected void objDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                Logging = new Prototype.Providers.Logging(this, e.Exception);
                RaiseLogging();

                e.ExceptionHandled = true;
            }
        }

        #endregion


        #region Method GridView

        GridViewRow gvRow;

        public void UpdateContent()
        {
            updateContentView.Update();
            updateContentPage.Update();
            updateContentFilter.Update();
            updateContentCommand.Update();
        }

        public void UpdateCommand()
        {
            updateContentCommand.Update();
        }

        public override void DataBind()
        {
            base.DataBind();

            if (this.GridAllowSelectBox)
            {
                DeleteAllKey(); //Reset key row selected by search
            }

            if (string.IsNullOrEmpty(gvView.DataSourceID)) // If this cast after call method 'DataUnBind'
            {
                gvView.DataSourceID = objDataSource.ID;
            }

            gvView.DataBind();

            BindScriptClient();

            UpdateContent();
        }

        public void DataUnBind()
        {
            if (this.GridAllowSelectBox)
            {
                DeleteAllKey(); //Reset key row selected by search
            }

            gvView.DataSourceID = null;
            gvView.DataSource = null;
            gvView.DataBind();

            BindScriptClient();

            UpdateContent();
        }


        #region Functions Customize

        #region Properties

        public string GridSortDefault
        {
            get
            {
                if (ViewState["GridSortDefault"] == null)
                    ViewState["GridSortDefault"] = string.Empty;

                return (string)ViewState["GridSortDefault"];
            }
            set
            {
                ViewState["GridSortDefault"] = value;
            }
        }

        private List<string> GridSorts { get; set; }

        private GridIsSort _isSortField = null;
        private List<GridIsSort> _gridIsSort = null;
        private List<GridIsSort> GridIsSort
        {
            get
            {
                if (ViewState["GridIsSort"] == null)
                    ViewState["GridIsSort"] = new GridIsSort[0];
                return new List<GridIsSort>((GridIsSort[])ViewState["GridIsSort"]);
            }
            set
            {
                ViewState["GridIsSort"] = value.ToArray();
            }
        }

        private List<GridFilters> _gridFilters = null;
        private List<GridFilters> GridFilters
        {
            get
            {
                if (ViewState["GridFilters"] == null)
                    ViewState["GridFilters"] = new GridFilters[0];
                return new List<GridFilters>((GridFilters[])ViewState["GridFilters"]);
            }
            set
            {
                ViewState["GridFilters"] = value.ToArray();
            }
        }

        double _gridColumnsWidth = 0;
        List<IGridColumnExt> IColumnList;

        public int IColumnCount
        {
            get
            {
                if (IColumnList != null)
                    return IColumnList.Count;
                else
                    return 0;
            }
        }

        List<IGridColumnGroup> IColumnGroupList;

        List<string> FieldExcludeList;
        List<IGridColumnExt> IColumnCustom;

        List<GridKeepField> _gridKeepField;

        private List<GridColumnAttr> _gridAutoColumnAttr = null;
        private List<GridColumnAttr> GridAutoColumnAttr
        {
            get
            {
                _gridKeepField = (List<GridKeepField>)Session["GridKeepField"];

                var hasGrid = _gridKeepField.FirstOrDefault(x => x.UniqueID == GridUniqueID);
                if (hasGrid != null)
                {
                    return hasGrid.Columns;
                }
                else
                {
                    return new List<GridColumnAttr>();
                }
            }
            set
            {
                _gridKeepField = (List<GridKeepField>)Session["GridKeepField"];

                var hasGrid = _gridKeepField.FirstOrDefault(x => x.UniqueID == GridUniqueID);
                if (hasGrid != null)
                {
                    hasGrid.Columns = value;
                }
                else
                {
                    var keep = new GridKeepField();
                    keep.UniqueID = GridUniqueID;
                    keep.Columns = value;
                    _gridKeepField.Add(keep);

                    Session["GridKeepField"] = _gridKeepField;
                }
            }
        }

        public event GridRowEventTextHandler GridRowTextChanged;
        public event System.EventHandler GridFilterInitValue;

        #endregion


        #region New Option 05/2016

        void SetValidateGroupInput()
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "_");

            btSearchConfirm.ValidationGroup = guid;
            FilterValidateId = guid;
        }

        private string FilterValidateId
        {
            get
            {
                return ViewState["FilterValidateId"].ToString();
            }
            set
            {
                ViewState["FilterValidateId"] = value;
            }
        }

        public GridViewRowCollection Rows
        {
            get
            {
                return gvView.Rows;
            }
        }

        #endregion

        void col_TextInputChanged(object sender, EventArgs e)
        {
            var txtText = (TextBox)sender;
            gvRow = (txtText.NamingContainer as GridViewRow);

            object _key = (gvRow.FindControl("hidKey") as HiddenField).Value;

            var _inKeyField = (txtText.ID.IndexOf("___") + 3);
            string _dataField = txtText.ID.Substring(_inKeyField);

            if (GridRowTextChanged != null && txtText != null)
            {
                var value = GridRowTextChanged(_key, _dataField, txtText.Text);
                if (value != txtText.Text)
                {
                    txtText.Text = value;
                }
            }
        }

        private void InitDisplayResourceByI()
        {

            _gridFilters = GridFilters;

            foreach (var fil in _gridFilters)
            {
                var iCol = IColumnList.FirstOrDefault(x => ((x.DataField == fil.DataField) || (x.DataFieldFilter == fil.DataField)) && !string.IsNullOrEmpty(x.ResourceValue));
                if (iCol != null)
                    fil.LabelText = iCol.ResourceValue;
            }

            GridFilters = _gridFilters;

            if (gvView.Columns.Count == 5) return;

            foreach (var map in IColumnList.Where(wh => ((wh.ControlType == ControlType.Label) || (wh.ControlType == ControlType.Text))
                                                         && !string.IsNullOrEmpty(wh.ResourceValue)).OrderBy(or => or.FieldIndex))
            {
                gvView.Columns[map.FieldIndex.Value].HeaderText = map.ResourceValue;
            }


        }

        private void InitColumnsByI()
        {
            if (placeCustomColumnGroups != null && IColumnGroupList == null)
            {
                IColumnGroupList = new List<IGridColumnGroup>();
                placeCustomColumnGroups.Controls.FindControlsDeepByType(ref IColumnGroupList);
            }

            if (IColumnList == null)
            {
                IColumnList = new List<IGridColumnExt>();

                FieldExcludeList = new List<string>();

                // New Option 03/2016
                if (AutoGenerateColumn)
                {
                    #region Set Field Auto Generate Columns

                    try
                    {
                        string[] autoIndexsSearch = new string[0];
                        string[] autoIndexsSort = new string[0];

                        string[] autoFieldsSearch = new string[0];

                        if (!string.IsNullOrEmpty(AutoGenColumnIndexs_SEARCH))
                            autoIndexsSearch = AutoGenColumnIndexs_SEARCH.Split(',');

                        if (!string.IsNullOrEmpty(AutoGenColumnIndexs_SORT))
                            autoIndexsSort = AutoGenColumnIndexs_SORT.Split(',');

                        if (!string.IsNullOrEmpty(AutoGenColumnFields_Exclude))
                            FieldExcludeList = AutoGenColumnFields_Exclude.Split(',').ToList();

                        if (!string.IsNullOrEmpty(AutoGenColumnFields_SEARCH))
                            autoFieldsSearch = AutoGenColumnFields_SEARCH.Split(',');

                        if (IColumnCustom == null)
                            IColumnCustom = new List<IGridColumnExt>();

                        if (placeCustomColumns != null)
                        {
                            placeCustomColumns.Controls.FindControlsDeepByType(ref IColumnCustom);

                            if (IColumnCustom.Count > 0)
                            {
                                var iFieldExclude = IColumnCustom.Where(x => !string.IsNullOrEmpty(x.DataFieldFilter)
                                                                        && !FieldExcludeList.Any(fld => fld == x.DataFieldFilter))
                                                               .Select(se => se.DataFieldFilter).ToList();

                                FieldExcludeList.AddRange(iFieldExclude);
                            }
                        }

                        _gridAutoColumnAttr = GridAutoColumnAttr;


                        #region Set Initial AutoColumns

                        if (_gridAutoColumnAttr.Count == 0)
                        {
                            var objHand = Activator.CreateInstance(this.SourceAssemblyName, this.SourceClassName);

                            try
                            {
                                var iQueryFunc = (IQueryExtend)objHand.Unwrap();
                                var iQuery = iQueryFunc.QueryViewRaw();

                                Type type = iQuery.ElementType;
                                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                                Type propType;
                                foreach (PropertyInfo prop in properties)
                                {
                                    propType = prop.PropertyType;
                                    if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    {
                                        propType = Nullable.GetUnderlyingType(propType);
                                    }

                                    _gridAutoColumnAttr.Add(new GridColumnAttr() { FieldName = prop.Name, TypeName = propType.Name });
                                }
                            }
                            catch
                            {
                                SetCustomFilterForSearch();

                                var iStoreFunc = (IStoreExtend)objHand.Unwrap();
                                var dt = iStoreFunc.QueryViewRaw(FilterCtrlOptions);

                                foreach (DataColumn dataCol in dt.Columns)
                                {
                                    _gridAutoColumnAttr.Add(new GridColumnAttr() { FieldName = dataCol.ColumnName, TypeName = dataCol.DataType.Name });
                                }
                            }


                            _gridAutoColumnAttr = _gridAutoColumnAttr.Where(x => (x.FieldName != KeyField)
                                                       && !FieldExcludeList.Any(field => field == x.FieldName)).ToList();

                            string headerText;
                            foreach (var _col in _gridAutoColumnAttr)
                            {
                                headerText = _col.FieldName;
                                _col.HeaderText = headerText.GetTextAutoCreate();
                            }

                            GridAutoColumnAttr = _gridAutoColumnAttr;
                        }

                        #endregion


                        int auto_inx = 1;
                        GridColumnExt _colAuto;

                        foreach (var prop in _gridAutoColumnAttr)
                        {
                            _colAuto = new GridColumnExt();
                            _colAuto.DataField = prop.FieldName;
                            _colAuto.HeaderText = prop.HeaderText;
                            _colAuto.AllowFilter = ShowAllFilter;
                            _colAuto.AllowSort = ShowAllSort;

                            if (prop.TypeName == typeof(int).Name)
                                _colAuto.FormatType = FieldValueType.Integer;
                            else if ((prop.TypeName == typeof(decimal).Name) || (prop.TypeName == typeof(double).Name))
                                _colAuto.FormatType = FieldValueType.Number;
                            else if (prop.TypeName == typeof(DateTime).Name)
                                _colAuto.FormatType = FieldValueType.Date;

                            if (autoIndexsSearch.Any(x => x.Trim() == auto_inx.ToString()))
                            {
                                _colAuto.AllowFilter = true;
                                _colAuto.ShowFilterNow = true;
                            }
                            else if (autoIndexsSearch.Any(x => x.Trim() == "0"))
                            {
                                _colAuto.AllowFilter = true;
                                _colAuto.ShowFilterNow = true;
                            }

                            if (!_colAuto.ShowFilterNow && autoFieldsSearch.Any(x => x == _colAuto.DataField))
                            {
                                _colAuto.AllowFilter = true;
                                _colAuto.ShowFilterNow = true;
                            }

                            if (!ShowAllSort)
                            {
                                if (autoIndexsSort.Any(x => x.Trim() == auto_inx.ToString()))
                                    _colAuto.AllowSort = true;
                                else if (autoIndexsSort.Any(x => x.Trim() == "0"))
                                    _colAuto.AllowSort = true;
                            }

                            IColumnList.Add(_colAuto);
                            auto_inx++;
                        }



                        #region Set custom properties columns from aspx page
                        if (IColumnCustom.Count > 0)
                        {
                            int _inx;
                            foreach (var iColCus in IColumnCustom)
                            {
                                var iColAuto = IColumnList.FirstOrDefault(x => x.DataField == iColCus.DataField);
                                if (iColAuto != null)
                                {
                                    if (string.IsNullOrEmpty(iColCus.HeaderText))
                                        iColCus.HeaderText = iColAuto.HeaderText;

                                    if (!iColCus.AllowFilter)
                                        iColCus.AllowFilter = iColAuto.AllowFilter;

                                    if (!iColCus.AllowSort)
                                        iColCus.AllowSort = iColAuto.AllowFilter;

                                    if (!iColCus.ShowFilterNow)
                                        iColCus.ShowFilterNow = iColAuto.ShowFilterNow;

                                    // ลบ Column Auto แล้วแทนที่ด้วย Column Custom
                                    _inx = IColumnList.IndexOf(iColAuto);

                                    IColumnList.RemoveAt(_inx);
                                    IColumnList.Insert(_inx, iColCus);
                                    //.......................................
                                }
                            }
                        }

                        #endregion

                        var page_ms = (Page.Master as WMS_NEW.Layout);
                        page_ms.iResourceList.AddRange(IColumnList);
                    }
                    catch (Exception ex)
                    {
                        Logging = new Prototype.Providers.Logging(this, ex);
                        RaiseLogging();
                    }

                    #endregion
                }
                else
                {
                    if (placeCustomColumns != null)
                        placeCustomColumns.Controls.FindControlsDeepByType(ref IColumnList);

                    //MOdify Bank 13/12/2019
                    //IColumnList = IColumnList.Where(w => w.DataField != "parent_lpn").ToList();
                    //using (var model = new WMS_NEW.Source.WMSEntities())
                    //{
                    //    bool isRule = model.t_wms_rule.Any(a => a.rule_code == "SHOW_COLUMN_SERIAL_NUMBER" && a.value == "YES");
                    //    if (!isRule)
                    //    {
                    //        IColumnList = IColumnList.Where(w => w.DataField != "serial_number").ToList();
                    //    }
                    //}

                    //IColumnList = IColumnList.Where(w => w.DataField != "attribute1").ToList();
                    //IColumnList = IColumnList.Where(w => w.DataField != "attribute2").ToList();
                    //IColumnList = IColumnList.Where(w => w.DataField != "attribute3").ToList();
                    //IColumnList = IColumnList.Where(w => w.DataField != "attribute4").ToList();
                    //IColumnList = IColumnList.Where(w => w.DataField != "attribute5").ToList();
                    //----- END ------------

                    //CHAMP 15/12/2023 พี่นัทให้แก้ว่าถ้า User ที่ใช้ Mapping แค่ 1 WH และ Owner ให้ซ่อนคอลัมน์
                    using (var model = new WMS_NEW.Source.WMSEntities())
                    {
                        if (model.t_wms_wh_user.Where(w => w.user_id == _SessionVals.UserName && w.is_active == "YES").Count() == 1)
                            IColumnList = IColumnList.Where(wh => !(wh.ResourceGroup.ToUpper().Trim() == "WAREHOUSE" && wh.ResourceName.ToUpper().Trim() == "WH_ID")).ToList();
                        if (model.t_wms_owner_user.Where(w => w.user_id == _SessionVals.UserName && w.is_active == "YES").Count() == 1)
                            IColumnList = IColumnList.Where(wh => !(wh.ResourceGroup.ToUpper().Trim() == "ONWER" && wh.ResourceName.ToUpper().Trim() == "ONWER_CODE")).ToList();
                    }


                    if (IColumnList.Count > 0)
                    {
                        var iFieldExclude = IColumnList.Where(x => !string.IsNullOrEmpty(x.DataFieldFilter)).Select(se => se.DataFieldFilter).ToList();

                        FieldExcludeList.AddRange(iFieldExclude);
                    }
                }
            }

            int filterInx = 1;
            int inx = 5;
            GridCustomField col = null;

            if (_gridFilters == null)
                _gridFilters = new List<_UControls.GridFilters>();

            if (GridSorts == null)
                GridSorts = new List<string>();

            foreach (var map in IColumnList)
            {
                #region Set Other Properties Column

                if (map.FieldIndex.IsNull())
                {
                    // Set Index for Display ResourceValue into Header Grid
                    map.FieldIndex = inx;
                    inx++;
                }

                if (map.FilterIndex.IsNull())
                {
                    // Set Index for Filter Control of Grid
                    map.FilterIndex = filterInx;
                    filterInx++;
                }

                col = new GridCustomField();
                col.CloneObject(map);

                switch (col.ControlType)
                {
                    case ControlType.Label:
                        break;
                    case ControlType.Text:
                        col.TextInputChanged += col_TextInputChanged;
                        break;
                    case ControlType.CommandButton:

                        //if (!string.IsNullOrEmpty(map.ResourceValue)) col.CommandText = map.ResourceValue;
                        col.CommandKeyField = this.KeyField;
                        break;
                    case ControlType.CommandLinkButton:

                        //if (!string.IsNullOrEmpty(map.ResourceValue)) col.CommandText = map.ResourceValue;
                        col.CommandKeyField = this.KeyField;
                        break;
                }

                #endregion

                AddGridColumn(map, col);
            }

            if (col != null)
                col.IsLastColumn = true;

            GridFilters = _gridFilters;
        }

        public void AddIColumnCustom(IEnumerable<IGridColumnExt> _iColumns)
        {
            if (IColumnCustom == null)
                IColumnCustom = new List<IGridColumnExt>();

            IColumnCustom.AddRange(_iColumns);
        }

        public void AddGridColumn(IGridColumnExt _iCol, GridCustomField _column)
        {
            if (string.IsNullOrEmpty(_column.HeaderText))
                _column.HeaderText = _column.DataField;

            if ((_column.ControlType == ControlType.CommandButton) || (_column.ControlType == ControlType.CommandLinkButton))
                _column.HeaderText = string.Empty;

            if (string.IsNullOrEmpty(_column.DataFieldFilter))
                _column.DataFieldFilter = _column.DataField;

            gvView.Columns.Add(_column);

            if (!Page.IsPostBack)
            {
                _gridColumnsWidth += _column.Width;
            }

            if (string.IsNullOrEmpty(GridSortDefault) && _column.ControlType == ControlType.Label)
                GridSortDefault = _column.DataField;

            if (_iCol.AllowFilter)
            {
                var filDataField = _column.DataFieldFilter;

                var filFormatType = _column.FormatType;
                if (_iCol.FilterFormatType != FieldValueType.Text)
                {
                    filFormatType = _iCol.FilterFormatType;
                }

                var filFormatString = _column.FormatString;
                if (!string.IsNullOrEmpty(_iCol.FilterFormatString))
                {
                    filFormatString = _iCol.FilterFormatString;
                }

                _gridFilters.Add(new GridFilters(_column.HeaderText, filDataField, filFormatType, filFormatString,
                    true, _iCol.FixFilter, _iCol.ShowFilterNow, _iCol.DefaultFilter, _iCol.UseFilterDropDown,
                    _iCol.DropDownFilterType, _iCol.FilterIndex, _iCol.FilterWidth, _iCol.FilterPrimary));
            }

            if (_column.AllowSort)
            {
                GridSorts.Add(_column.DataField);
            }
        }

        public void AllowShowColumnIndex(int _colIndex, bool IsShow)
        {
            gvView.Columns[(_colIndex + 5)].Visible = IsShow;
        }

        public void GridChangeColumnName(int _columnIndex, string _dataField, string _headerText)
        {
            if (!Page.IsPostBack)
            {
                return;
            }

            if (IColumnList == null)
            {
                IColumnList = new List<IGridColumnExt>();
                placeCustomColumns.Controls.FindControlsDeepByType<IGridColumnExt>(ref IColumnList);
            }

            var column = IColumnList.FirstOrDefault(qry => qry.DataField == _dataField);
            if (column != null)
            {
                column.HeaderText = _headerText;
            }

            gvView.Columns[(5 + _columnIndex)].HeaderText = _headerText;

            foreach (var _container in this.GridFilterContainer)
            {
                var listDataField = GridFilters.Where(qry => qry.DataField == _dataField).Select(se => _container.ID + "_" + "tableGridSearch_Row_Cell_Label_" + se.DataField);

                var rowsCtrl = _container.Controls.OfType<Table>().First().Controls.OfType<TableRow>();
                foreach (var row in rowsCtrl)
                {
                    var cellCtrl = row.Controls.OfType<TableCell>().FirstOrDefault(cell => listDataField.Contains(cell.ID));
                    if (cellCtrl != null)
                    {
                        cellCtrl.Text = _headerText;
                        return;
                    }
                }
            }
        }

        public void GridColumnRefreshFilter(IGridColumnExt _iCol)
        {
            if (!Page.IsPostBack)
                return;

            var ictrl = (_IInputDropDown)EntityExt.IControls.FirstOrDefault(x => (x.InputType == InputType.DropDownNormal || x.InputType == InputType.DropDownLazy)
                                                                              && (x.DataFieldValue == _iCol.DataField || x.DataFieldValue == _iCol.DataFieldFilter));
            if (ictrl != null)
                ictrl.BindDataSource();
        }

        public void GridFilterSetValue(IGridColumnExt _iCol, object _value)
        {
            var _iDataField = !string.IsNullOrEmpty(_iCol.DataFieldFilter) ? _iCol.DataFieldFilter : _iCol.DataField;

            foreach (var _container in this.GridFilterContainer)
            {
                var listDataField = GridFilters.Where(qry => qry.DataField == _iDataField).Select(se => _container.ID + "_" + "tableGridSearch_Row_Cell_Control_" + se.DataField);

                var rowsCtrl = _container.Controls.OfType<Table>().First().Controls.OfType<TableRow>();
                foreach (var row in rowsCtrl)
                {
                    var cellCtrl = row.Controls.OfType<TableCell>().FirstOrDefault(cell => listDataField.Contains(cell.ID));
                    if (cellCtrl != null)
                    {
                        var iCtrl = cellCtrl.Controls.OfType<_UControls._IInputControl>().First();
                        iCtrl.SetObjectValue(_value);

                        return;
                    }
                }
            }
        }

        public void GridClearAllFilters()
        {
            foreach (var _container in this.GridFilterContainer)
            {
                var rowsCtrl = _container.Controls.OfType<Table>().First().Controls.OfType<TableRow>();
                foreach (var row in rowsCtrl)
                {
                    var cellCtrl = row.Controls.OfType<TableCell>().FirstOrDefault(cell => cell.ID.StartsWith(_container.ID + "_" + "tableGridSearch_Row_Cell_Control_"));
                    if (cellCtrl != null)
                    {
                        var iCtrl = cellCtrl.Controls.OfType<_UControls._IInputControl>().First();
                        iCtrl.Clear();
                    }
                }
            }

            updateContentFilter.Update();
        }

        List<Panel> GridFilterContainer { get; set; }


        private EntityControlExtend EntityExt = new EntityControlExtend();

        void InitialGridFilter()
        {
            try
            {
                _gridFilters = GridFilters;

                const string field_is_active = "is_active";
                int control_idex = 1;

                var override_controls = new List<EntityCustom>();

                if (placeCustomSearch.Controls.Count > 0)
                {
                    foreach (var cusiCtrl in placeCustomSearch.Controls.OfType<_IInputControl>())
                    {
                        cusiCtrl.ControlIndex = (short)control_idex;

                        if (!Page.IsPostBack && cusiCtrl.IsPrimary)
                            cusiCtrl.ValidateGroup = FilterValidateId;

                        override_controls.Add(new EntityCustom(cusiCtrl) { ControlIndex = cusiCtrl.ControlIndex });

                        control_idex++;
                    }
                }

                foreach (var gridFilter in _gridFilters.OrderBy(qry => qry.FilterIndex)) //Sort by FilterIndex
                {
                    _IInputControl iInput = null;

                    #region Initial Input Filter

                    if (gridFilter.UseDropDown == false && gridFilter.DataField.ToLower() != field_is_active)
                    {
                        switch (gridFilter.FormatType)
                        {
                            case FieldValueType.Text:

                                iInput = new InputTextBox();
                                break;
                            case FieldValueType.Integer:

                                iInput = new InputTextInteger();
                                break;
                            case FieldValueType.Number:

                                var numFilter = new InputTextNumber();
                                numFilter.NumberType = NumberType.Double;

                                if (!string.IsNullOrEmpty(gridFilter.FormatString))
                                    numFilter.NumberDegit = gridFilter.FormatString.ToUpper().Split('.').Last().Length;
                                //numFilter.NumberDegit = Convert.ToInt32(gridFilter.FormatString.ToUpper().Replace("N", ""));

                                iInput = numFilter;
                                break;
                            case FieldValueType.DateTime:

                                var dateTimeFilter = new InputTextDate();
                                dateTimeFilter.TextMode = DateTimeType.DateTime;

                                if (!string.IsNullOrEmpty(gridFilter.FormatString))
                                    dateTimeFilter.DateTimeFormat = gridFilter.FormatString;

                                iInput = dateTimeFilter;
                                break;
                            case FieldValueType.Date:

                                var dateFilter = new InputTextDate();
                                dateFilter.TextMode = DateTimeType.Date;

                                if (!string.IsNullOrEmpty(gridFilter.FormatString))
                                    dateFilter.DateFormat = gridFilter.FormatString;

                                iInput = dateFilter;
                                break;
                            case FieldValueType.Time:

                                var timeFilter = new InputTextDate();
                                timeFilter.TextMode = DateTimeType.Time;

                                if (!string.IsNullOrEmpty(gridFilter.FormatString))
                                    timeFilter.TimeFormat = gridFilter.FormatString;

                                iInput = timeFilter;
                                break;
                        }
                    }
                    else
                    {
                        _IInputDropDown _plugIDropDown = null;

                        #region Initial DropDown Type

                        if (gridFilter.UseDropDown == false && gridFilter.DataField.ToLower() == field_is_active)
                        {
                            gridFilter.FormatType = FieldValueType.Text;

                            _plugIDropDown = new InputDropDown();
                            _plugIDropDown.UseDefaultDisplay = true;

                            if (!Page.IsPostBack)
                            {
                                _plugIDropDown.DisplayDefault = "--All--";
                                _plugIDropDown.MethodQueryProperty = delegate () { return new ActiveType().AsQueryable(); };
                            }
                        }
                        else if (gridFilter.DropDownFilterType == DropDownType.Normal)
                        {
                            _plugIDropDown = new InputDropDown();
                        }
                        else if (gridFilter.DropDownFilterType == DropDownType.LazySearch)
                        {
                            _plugIDropDown = new InputDropDownHD();
                        }


                        #region Set Properties data pass Interface DropDown

                        if (_plugIDropDown != null)
                        {
                            switch (gridFilter.FormatType)
                            {
                                case FieldValueType.Text:
                                    _plugIDropDown.ComboType = ComboType.String;
                                    break;
                                case FieldValueType.Integer:
                                    _plugIDropDown.ComboType = ComboType.Integer;
                                    break;
                                case FieldValueType.Boolean:
                                    _plugIDropDown.ComboType = ComboType.Boolean;
                                    break;
                                case FieldValueType.Number:
                                    _plugIDropDown.ComboType = ComboType.Decimal;
                                    break;
                                case FieldValueType.Guid:
                                    _plugIDropDown.ComboType = ComboType.Guid;
                                    break;
                            }

                            var filDrop = IColumnList.First(qry => (qry.DataFieldFilter == gridFilter.DataField) || (qry.DataField == gridFilter.DataField));

                            if (filDrop.DropDownLazyLimit != null)
                                _plugIDropDown.LoadLazyLimit = filDrop.DropDownLazyLimit.Value;

                            if (filDrop.DropDownSearchMultiValue != null)
                                _plugIDropDown.AllowSearchMultiValue = filDrop.DropDownSearchMultiValue.Value;

                            if (!string.IsNullOrEmpty(filDrop.DropDownDisplayDefault))
                                _plugIDropDown.DisplayDefault = filDrop.DropDownDisplayDefault;

                            if (filDrop.DropDownAutoPostBack == true && filDrop.DropDownPostValueChanged != null)
                            {
                                _plugIDropDown.AutoPostBack = true;
                                _plugIDropDown.PostValueChanged += delegate (dynamic _value) { filDrop.DropDownPostValueChanged(_value); };
                            }

                            if (filDrop.DropDownSelectedValue != null)
                                _plugIDropDown.DefaultValue = filDrop.DropDownSelectedValue;

                            if (filDrop.DropDownQueryProperty != null)
                                _plugIDropDown.MethodQueryProperty = filDrop.DropDownQueryProperty;
                        }

                        #endregion

                        #endregion

                        iInput = _plugIDropDown;
                    }

                    #endregion


                    iInput.DataFieldValue = gridFilter.DataField;


                    if (!Page.IsPostBack) //Set initial filter default at one PageLoad
                    {
                        iInput.IsPrimary = gridFilter.IsPrimary;

                        if (iInput.IsPrimary)
                            iInput.ValidateGroup = FilterValidateId;

                        if (gridFilter.FilterWidth != null)
                            iInput.ControlWidth = gridFilter.FilterWidth.Value;
                    }

                    override_controls.Add(new EntityCustom(iInput) { ControlIndex = (short)control_idex });
                    control_idex++;
                }

                EntityExt.LayoutAutoGenColFix = 6;
                EntityExt.AutoCreateControlEntity(this.TemplateControl, ref panelGridSearch, null, override_controls, null);

                foreach (var iCtrl in EntityExt.IControls)//ใช้ control เดียวกับ popup ทำให้เกิด default value
                {
                    var gridFilter = _gridFilters.FirstOrDefault(x => x.DataField == iCtrl.DataFieldValue);

                    if (!Page.IsPostBack)
                    {
                        if (iCtrl.InputType == InputType.DropDownNormal || iCtrl.InputType == InputType.DropDownLazy)
                        {
                            var _plugIDropDown = (_IInputDropDown)iCtrl;

                            //--Original Source
                            //_plugIDropDown.UseDefaultDisplay = true;
                            //_plugIDropDown.BindDataSource();

                            //if (_plugIDropDown.DefaultValue != null)
                            //    _plugIDropDown.SetObjectValue(_plugIDropDown.DefaultValue);
                            //---------------------------

                            //--Modify Bank 2020-01-10
                            _plugIDropDown.UseDefaultDisplay = true;

                            if (gridFilter != null && gridFilter.DataField.ToLower() == field_is_active)
                            {
                                _plugIDropDown.DisplayDefault = "-- All --";
                            }
                            _plugIDropDown.BindDataSource();
                            _plugIDropDown.Clear();
                        }

                        if (gridFilter != null)
                        {
                            iCtrl.LabelText = gridFilter.LabelText;

                            iCtrl.Filterable = true;
                            iCtrl.DefaultFilter = gridFilter.DefaultFilter;
                            iCtrl.FixFilter = gridFilter.FixFilter;

                            // Set Initial first Show Filter Control
                            if (gridFilter.IsShowFilter && iCtrl.Enabled == true && (iCtrl.DefaultFilter == FilterAt.None || iCtrl.DefaultFilter == FilterAt.Empty))
                                iCtrl.Enabled = false;
                        }
                    }

                    if (gridFilter != null)
                    {
                        iCtrl.VisibleExt = gridFilter.IsShowFilter;
                        (iCtrl as Control).Parent.Visible = gridFilter.IsShowFilter;
                    }

                    if (iCtrl.InputType == InputType.Text || iCtrl.InputType == InputType.TextDate || iCtrl.InputType == InputType.TextInteger || iCtrl.InputType == InputType.TextNumber)
                    {
                        var iInputText = ((_IInputText)iCtrl);
                        iInputText.KeyEnterName = this.SEARCH_ENTER_KEY;
                        iInputText.TextEnterChanged += TextFlterEnterChange;
                    }
                }


                if (placeCustomSearch.Controls.Count > 0)
                {
                    foreach (var cusiCtrl in placeCustomSearch.Controls.OfType<_IInputControl>())
                    {
                        var iCtrl = EntityExt.IControls.FirstOrDefault(x => x.DataFieldValue == cusiCtrl.DataFieldValue);
                        if (iCtrl != null)
                        {
                            if (!Page.IsPostBack)
                            {
                                if (iCtrl.InputType != InputType.Hidden)
                                {
                                    iCtrl.Filterable = true;
                                    iCtrl.DefaultFilter = cusiCtrl.DefaultFilter;
                                    iCtrl.FixFilter = cusiCtrl.FixFilter;
                                }

                                if (cusiCtrl.DefaultValue != null)
                                    iCtrl.DefaultValue = cusiCtrl.DefaultValue;

                                var refVal = cusiCtrl.GetObjectValue();
                                if (refVal != null)
                                    iCtrl.SetObjectValue(refVal);
                            }

                            iCtrl.VisibleExt = true;
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

        void GridFilterChange(string _dataField)
        {
            try
            {
                _gridFilters = GridFilters;

                var result = _gridFilters.First(qry => qry.DataField == _dataField);
                result.IsShowFilter = !result.IsShowFilter;

                GridFilters = _gridFilters;

                var iControls = EntityExt.IControls;

                foreach (var gridFilter in GridFilters)
                {
                    var iCtrl = iControls.FirstOrDefault(x => x.DataFieldValue == gridFilter.DataField);
                    if (iCtrl != null)
                    {
                        iCtrl.VisibleExt = gridFilter.IsShowFilter;
                        (iCtrl as Control).Parent.Visible = gridFilter.IsShowFilter;

                        if (gridFilter.IsShowFilter)
                        {
                            if (iCtrl.Enabled == true && (iCtrl.DefaultFilter == FilterAt.None || iCtrl.DefaultFilter == FilterAt.Empty))
                                iCtrl.Enabled = false;
                        }
                    }
                }

                IsGridColumnFilter = true;
                updateContentFilter.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void GridSorting(string _sortEvent, string _dataField)
        {
            _gridIsSort = GridIsSort;

            if (_sortEvent == "SORT")
            {
                _isSortField = this._gridIsSort.FirstOrDefault(qry => qry.DataField == _dataField);
                if (_isSortField != null)
                {
                    if (_isSortField.Direction == "ASC")
                        _isSortField.Direction = "DESC";
                    else
                        _isSortField.Direction = "ASC";
                }
                else
                {
                    _isSortField = new GridIsSort(_dataField, (_gridIsSort.Count + 1));
                    _gridIsSort.Add(_isSortField);
                }
            }
            else if (_sortEvent == "UNSORT")
            {
                _isSortField = this._gridIsSort.FirstOrDefault(qry => qry.DataField == _dataField);
                if (_isSortField != null)
                {
                    _gridIsSort.Remove(_isSortField);
                }
            }

            var sortParameter = string.Empty;
            var resetIndex = 1;
            foreach (var item in _gridIsSort.OrderBy(or => or.Index))
            {
                sortParameter += item.DataField + " " + item.Direction + ",";

                item.Index = resetIndex;
                resetIndex++;
            }

            DeleteAllKey();
            GridIsSort = _gridIsSort;

            gvView.PageIndex = 0;
            gvView.Sort(sortParameter.TrimEnd(','), SortDirection.Ascending);

            updateContentPage.Update();
        }

        #endregion

        #region Function Select CheckBox

        public void DeleteAllKey()
        {
            SelectRowCountByPage = 0;
            ListKey = new List<KeySelect>();
        }

        public void DeleteKey(object _key)
        {
            _listKey = ListKey;
            var _countSelectRow = SelectRowCountByPage;

            var hasKey = _listKey.FirstOrDefault(qry => qry.KeyId.ToString() == _key.ToString());
            if (hasKey != null)
            {
                _listKey.Remove(hasKey);
                _countSelectRow--;
            }

            ListKey = _listKey;
            SelectRowCountByPage = _countSelectRow;
        }

        protected int GridRowCountByPage
        {
            get { return gvView.Rows.Count; }
        }

        //bool isBySelectAll = true;
        int SelectRowCountByPage
        {
            get
            {
                return Convert.ToInt32(hidSelectRowCountByPage.Value);
            }
            set
            {
                hidSelectRowCountByPage.Value = value.ToString();
            }
        }

        #endregion

        #region Function Manage KeyID

        private object ParseKeyValue(string _value)
        {
            if (_value == null)
                return null;

            object val = null;

            switch (KeyType)
            {
                case KeyType.Guid:
                    val = Guid.Parse(_value);
                    break;
                case KeyType.Integer:
                    val = Convert.ToInt32(_value);
                    break;
                case KeyType.String:
                    val = _value;
                    break;
            }

            return val;
        }

        public List<KeySelect> GetListKey()
        {
            return this.ListKey;
        }
        public int CountListKey()
        {
            return this.ListKey.Count;
        }

        private List<KeySelect> _listKey = null;
        private List<KeySelect> ListKey
        {
            get
            {
                var serializer = new JavaScriptSerializer();

                if (string.IsNullOrEmpty(hidGridCheckVals.Value))
                {
                    string jsonString = serializer.Serialize(new List<KeySelect>());
                    hidGridCheckVals.Value = jsonString;
                }

                var obj = (List<KeySelect>)serializer.Deserialize(hidGridCheckVals.Value, typeof(List<KeySelect>));
                return obj;
            }
            set
            {
                var serializer = new JavaScriptSerializer();
                string jsonString = serializer.Serialize(value);
                hidGridCheckVals.Value = jsonString;
            }
        }

        private List<KeySelect> _listKeyDBActive = null;
        private List<KeySelect> ListKeyDBActive
        {
            get
            {
                var serializer = new JavaScriptSerializer();

                if (string.IsNullOrEmpty(hidGridDBVals.Value))
                {
                    string jsonString = serializer.Serialize(new List<KeySelect>());
                    hidGridDBVals.Value = jsonString;
                }

                var obj = (List<KeySelect>)serializer.Deserialize(hidGridDBVals.Value, typeof(List<KeySelect>));
                return obj;
            }
            set
            {
                var serializer = new JavaScriptSerializer();
                string jsonString = serializer.Serialize(value);
                hidGridDBVals.Value = jsonString;
            }
        }

        public KeyType KeyType { get; set; }
        public string KeyField { get; set; }
        public string KeyFieldSelect { get; set; }

        #endregion

        #region Function Constant

        public event GridRowDataHandler GridHeaderAfterDataBound;
        public event GridRowDataHandler GridRowAfterDataBound;
        public event GridRowValidateHandler GridRowCanSelectValidate;
        public event GridRowValidateHandler GridRowCanEditValidate;
        public event GridRowValidateHandler GridRowCanDeleteValidate;

        public event GridRowCommandHandler GridRowCommandClick;
        public event GridRowEventHandler GridRowClick;
        public event GridRowEventHandler GridRowEdit;
        public event GridRowEventHandler GridRowDelete;
        public event GridRowEventHandler GridRowAfterDeleted;


        public event GridRowExtendEventHandler GridRowExtendClick;
        public event GridRowExtendEventHandler GridRowCommandExtendClick;

        public event GridExportTemplate GridExportTemplate;

        int _gridRecordNo = 0;
        int _gridCountSelectRow = 0;

        protected void gvView_PreRender(object sender, EventArgs e)
        {
            //This comment because page render state very slower
            // UpdateContent();
        }

        protected void gvView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            object value = ParseKeyValue(e.CommandArgument.ToString());

            if (e.CommandName == "SEL")
            {
                if (GridRowClick != null)
                    GridRowClick(value);

                if (GridRowExtendClick != null)
                {
                    var ext_values = GetRowExtendValues(e);
                    GridRowExtendClick(value, ext_values);
                }
            }
            else if (e.CommandName == "EDI")
            {
                if (PopupEntitySource != null)
                    PopupEntitySource.Edit(value);

                if (GridRowEdit != null)
                    GridRowEdit(e.CommandArgument);
            }
            else if (e.CommandName == "DEL")
            {
                if (GridRowDelete == null)
                {
                    try
                    {
                        var objHand = Activator.CreateInstance(this.SourceAssemblyName, this.SourceClassName);

                        var func = (IGridCommand)objHand.Unwrap();
                        if (func != null)
                        {
                            this.PlugEventResult(func);

                            var deleted = func.DeleteById(value);
                            if (deleted)
                            {
                                if (GridDeleted != null)
                                    GridDeleted(null, EventArgs.Empty);

                                DataBind();
                            }

                            func = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging = new Prototype.Providers.Logging(this, ex);
                        RaiseLogging();
                    }
                }
                else
                {
                    GridRowDelete(value);
                }

                if (GridRowAfterDeleted != null)
                    GridRowAfterDeleted(value);
            }
            else
            {
                if (GridRowCommandClick != null)
                    GridRowCommandClick(e);

                if (GridRowCommandExtendClick != null)
                {
                    var ext_values = GetRowExtendValues(e);
                    GridRowCommandExtendClick(value, ext_values);
                }
            }
        }

        protected void gvView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                #region Initial Resource Language
                var res_No = _resourceGeneral.FirstOrDefault(qry => qry.ResourceGroup == "General" && qry.ResourceName == "rows");
                Label lblNo = (Label)e.Row.FindControl("labHeader");
                lblNo.Text = res_No.ResourceValue;
                lblNo.Visible = !DisableRowNo;

                var res_ViewText = _resourceGeneral.FirstOrDefault(qry => qry.ResourceGroup == "General" && qry.ResourceName == "View");
                if (res_ViewText != null)
                    e.Row.Cells[2].Text = res_ViewText.ResourceValue;

                var res_EditText = _resourceGeneral.FirstOrDefault(qry => qry.ResourceGroup == "General" && qry.ResourceName == "Edit");
                if (res_EditText != null)
                    e.Row.Cells[3].Text = res_EditText.ResourceValue;

                var res_DeleteText = _resourceGeneral.FirstOrDefault(qry => qry.ResourceGroup == "General" && qry.ResourceName == "Delete");
                if (res_DeleteText != null)
                    e.Row.Cells[4].Text = res_DeleteText.ResourceValue;

                #endregion

                #region Header

                _listKey = ListKey;

                if (GridListKeyDBCustom == null)
                    _listKeyDBActive = new List<KeySelect>(); // Clear ทุกครั้งที่ Bind ข้อมูลใหม่
                else
                    _listKeyDBActive = GridListKeyDBCustom();

                _gridRecordNo = gvView.PageIndex;

                if (_gridRecordNo > 0)
                    _gridRecordNo = (_gridRecordNo * gvView.PageSize);

                _gridCountSelectRow = 0; //Reset count row selected by search or new page

                #region Header SelectBox

                if (this.GridAllowSelectBox)
                {
                    var cmdSelectAll = (CheckBox)e.Row.FindControl("cmdSelectAll");
                    cmdSelectAll.Attributes.Add("onclick", String.Format("checkboxAllClick(this,'#{0}','#{1}','#{2}','#{3}');",
                                                        cmdSelectAll.ClientID, hidGridCheckVals.ClientID, hidGridDBVals.ClientID, hidSelectRowCountByPage.ClientID));
                    //2024-02-20 Krit add new allow/hide select all
                    cmdSelectAll.Visible = GridAllowShowSelectBoxAll;
                }

                #endregion

                #region Cusomize Header

                _gridFilters = GridFilters;
                _gridIsSort = GridIsSort;

                // Set Custom Column Style
                for (int i = 5; i <= (gvView.Columns.Count - 1); i++)
                {
                    TableCell cell = e.Row.Cells[i];

                    var div = cell.Controls.OfType<HtmlGenericControl>().First();
                    if (div == null) continue;

                    var hidField = div.Controls.OfType<Label>().Skip(1).First();
                    var hidWidth = div.Controls.OfType<Label>().Skip(2).First();
                    var columnWidth = Convert.ToInt32(hidWidth.Text);

                    if (IColumnGroupList.Count > 0)
                    {
                        var hasPrefix = IColumnGroupList.FirstOrDefault(x => hidField.Text.StartsWith(x.DataFieldPrefix)
                        && !string.IsNullOrEmpty(x.HeaderCssStyle));
                        if (hasPrefix != null)
                        {
                            cell.Attributes.Add("style", hasPrefix.HeaderCssStyle);
                        }
                    }

                    #region Create Header Filter

                    var linkFilter = (HtmlGenericControl)div.Controls.OfType<HtmlGenericControl>().First();

                    var filterFieldName = linkFilter.Attributes["name"];
                    if (filterFieldName != null && _gridFilters.Any(wh => wh.DataField == filterFieldName.Replace("link_filter_name_", "")))
                    {
                        columnWidth += 26; // set width filter button
                    }

                    // create header filter at => gridHeaderFilter.js

                    #endregion

                    #region Create Header Sort

                    if (GridSorts.Any(field => field.Equals(hidField.Text)))
                    {
                        columnWidth += 26;

                        var linkSort = (HtmlGenericControl)div.Controls.OfType<HtmlGenericControl>().Skip(1).First();

                        linkSort.Attributes.Add("title", "Sort");
                        linkSort.Attributes.Add("data-placement", "bottom");
                        linkSort.Attributes.Add("onclick", "__doPostBack('" + gvView.ClientID + "','SORT|" + hidField.Text + "');");
                        linkSort.Attributes.Add("onmousedown", "if(event.button == 2){ __doPostBack('" + gvView.ClientID + "','UNSORT|" + hidField.Text + "'); }");

                        _isSortField = this._gridIsSort.FirstOrDefault(field => field.DataField == hidField.Text);
                        if (_isSortField != null)
                        {
                            linkSort.Attributes.Add("class", "grid-sort-active");

                            if (_isSortField.Direction == "ASC")
                                linkSort.InnerHtml += "<i class=\"fa fa-sort-asc\"></i>";
                            else
                                linkSort.InnerHtml += "<i class=\"fa fa-sort-desc\"></i>";

                            linkSort.InnerHtml += "<div class=\"index-sort\">" + _isSortField.Index.ToString() + "</div>";
                        }
                        else
                        {
                            linkSort.Attributes.Add("class", "grid-sort");
                            linkSort.InnerHtml = "<i class=\"fa fa-sort\"></i>";
                        }
                    }

                    #endregion

                    div.Attributes.Add("style", String.Format("width:{0}px; min-width:{1}px;", columnWidth, columnWidth));
                }

                #endregion

                if (GridHeaderAfterDataBound != null)
                    GridHeaderAfterDataBound(e);

                #endregion
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region Row

                _gridRecordNo++;

                var labNo = ((Label)e.Row.Cells[0].FindControl("labNo"));
                labNo.Text = _gridRecordNo.ToString();
                labNo.Visible = !DisableRowNo;
                #region Grid Validate Allow Row Edit

                if (this.GridAllowRowEdit)
                {
                    if (GridRowCanEditValidate != null)
                    {
                        var cmdEdit = (LinkButton)e.Row.FindControl("cmdEdit");
                        cmdEdit.Enabled = GridRowCanEditValidate(e);

                        if (!cmdEdit.Enabled)
                        {
                            cmdEdit.CommandName = null;
                            cmdEdit.CommandArgument = null;
                            cmdEdit.OnClientClick = null;
                            cmdEdit.CssClass = "command-grid-disabled";
                        }
                    }
                }

                #endregion

                #region Grid Validate Allow Row Delete

                if (this.GridAllowRowDelete)
                {
                    if (GridRowCanDeleteValidate != null)
                    {
                        var cmdDelete = (LinkButton)e.Row.FindControl("cmdDelete");
                        cmdDelete.Enabled = GridRowCanDeleteValidate(e);

                        if (!cmdDelete.Enabled)
                        {
                            cmdDelete.CommandName = null;
                            cmdDelete.CommandArgument = null;
                            cmdDelete.OnClientClick = null;
                            cmdDelete.CssClass = "command-grid-disabled";
                        }
                    }
                }

                #endregion

                #region Grid Validate Allow Row SelectBox

                if (this.GridAllowSelectBox)
                {
                    var cmdSelect = (CheckBox)e.Row.Cells[1].FindControl("cmdSelect");

                    cmdSelect.Attributes.Add("onclick", string.Format("checkboxClick(this,'#{0}','#{1}','#{2}');",
                                                        hidGridCheckVals.ClientID, hidGridDBVals.ClientID, hidSelectRowCountByPage.ClientID));

                    if (GridRowCanSelectValidate != null)
                    {
                        cmdSelect.Enabled = GridRowCanSelectValidate(e);
                        if (!cmdSelect.Enabled)
                        {
                            cmdSelect.Attributes.Remove("onclick");
                        }
                    }
                }

                #endregion

                if (this.GridHighlightAllRow)
                {
                    e.Row.Attributes["class"] = "highlight";
                }

                #region Grid Allow Row Manual SelectBox

                if (this.GridAllowSelectBox)
                {
                    var keyRow = (object)DataBinder.Eval(e.Row.DataItem, this.KeyField);

                    // ถ้ามีการดึงค่าในการ Select Key เดิมมาจาก DB
                    if (!string.IsNullOrEmpty(this.KeyFieldSelect))
                    {
                        //เก็บเฉพาะ Key ที่อยู่ใน DB Select Key = true เพื่อเอาไว้สำหรับเปรียบเทียบกับ Key ที่ใช้งานจริงในหน้านั้นๆ
                        if ((bool)DataBinder.Eval(e.Row.DataItem, this.KeyFieldSelect))
                        {
                            if (!_listKeyDBActive.Any(qry => qry.KeyId.ToString() == keyRow.ToString()))
                                _listKeyDBActive.Add(new KeySelect() { KeyId = keyRow, Active = true });
                        }
                    }

                    var hasChecked = false;
                    var hasSelect = _listKey.FirstOrDefault(qry => qry.KeyId.ToString() == keyRow.ToString());

                    if (hasSelect != null) //ถ้ามีค่าอยู่ใน ListKey
                        hasChecked = hasSelect.Active;
                    else //ถ้าไม่ให้เปรียบเทียบกับ Select Key ใน DB
                        hasChecked = _listKeyDBActive.Any(qry => qry.KeyId.ToString() == keyRow.ToString());

                    if (hasChecked)
                    {
                        ((CheckBox)e.Row.Cells[1].FindControl("cmdSelect")).Checked = true;
                        //e.Row.Attributes["class"] = "highlight";

                        _gridCountSelectRow++;
                    }

                }

                #endregion


                if (GridRowAfterDataBound != null)
                    GridRowAfterDataBound(e);

                #endregion
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                #region Footer

                ListKey = _listKey;
                ListKeyDBActive = _listKeyDBActive;

                SelectRowCountByPage = _gridCountSelectRow;

                //if (SelectRowCountByPage > 0) //ถ้า Binding Data ใหม่แล้ว ไม่มีการเลือก Select Rows ใน Page นั้นๆไม่ต้องทำการเช็ค Select All
                //{
                //    CheckSelectAllRows();
                //}

                #endregion
            }
        }

        protected void gvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvView.PageIndex = e.NewPageIndex;
            updateContentView.Update();
        }

        #endregion

        #endregion


        #region Export Data

        protected void linkToExcel_Click(object sender, EventArgs e)
        {
            ExportData("xls");
        }
        protected void linkToCSV_Click(object sender, EventArgs e)
        {
            ExportData("csv");
        }

        public string ExportFileName { get; set; }
        public bool ExportWithSortData { get; set; }
        public ExportSourceType ExportSourceType { get; set; }

        public event GridExportDataHandler GridPreExportData;

        void ExportData(string _typeExp)
        {
            DataTable _dt = null;

            try
            {
                var nameSpaces = this.SourceClassName.Split('.');
                var objHand = Activator.CreateInstance(this.SourceAssemblyName, this.SourceClassName);

                var func = (Prototype.Providers.IGridExportData)objHand.Unwrap();
                this.PlugEventResult(func);

                //if (!ExportWithSortData)
                _dt = func.GetExportData(this.FilterGridOptions, this.FilterCtrlOptions, this.ExportSourceType);
                //else
                //{
                //    _gridIsSort = GridIsSort;
                //    var sortParameter = _gridIsSort.OrderBy(or => or.Index)
                //                                   .Aggregate(String.Empty, (curr, next) => curr + "," + (next.DataField + " " + next.Direction));

                //    _dt = func.GetExportData(sortParameter.TrimStart(','), GridSortDefault
                //        , this.FilterGridOptions, this.FilterCtrlOptions, this.ExportSourceType);

                //}

                if (!ExportWithSortData)
                {
                    DataView dv = _dt.DefaultView;

                    if (this.GridIsSort.Count > 0)
                    {
                        var fields_sort = string.Empty;

                        foreach (var item in GridIsSort)
                        {
                            fields_sort += item.DataField + " " + item.Direction + ",";
                        }

                        fields_sort = fields_sort.Substring(0, fields_sort.Length - 1);
                        dv.Sort = fields_sort;
                    }
                    else if (!string.IsNullOrEmpty(GridSortDefault))
                    {
                        dv.Sort = GridSortDefault;
                    }

                    _dt = dv.ToTable();
                }


                if (GridPreExportData != null)
                    GridPreExportData(ref _dt);


                List<string> listColumnDT = new List<string>();
                foreach (DataColumn item in _dt.Columns)
                {
                    var isGrid = IColumnList.Any(a => a.DataField.ToLower() == item.ColumnName.ToLower() && a.IsIncludeInExcel);
                    if (isGrid)
                    {
                        listColumnDT.Add(item.ColumnName);
                    }
                    else
                    {
                        FieldExcludeList.Add(item.ColumnName);
                    }

                }

                #region  New Option 03/2016


                if (FieldExcludeList.Count > 0)
                {
                    foreach (var field in FieldExcludeList)
                    {
                        var hasName = _dt.Columns.Cast<DataColumn>().Any(c => c.ColumnName.ToLower() == field.ToLower());
                        if (hasName)
                            _dt.Columns.Remove(field);
                    }
                }

                try
                {
                    _dt.Columns.Remove(KeyField);
                }
                catch { }


                if (this.AutoGenerateColumn)
                {
                    (Page.Master as WMS_NEW.Layout).GetResourceByAutoI();
                }

                //Set Column Index
                //===========================
                int index = 0;
                foreach (var item in IColumnList)
                {
                    if (listColumnDT.Contains(item.DataField))
                    {
                        _dt.Columns[item.DataField].SetOrdinal(index);
                        index++;
                    }
                }



                foreach (DataColumn item in _dt.Columns)
                {
                    //try
                    //{
                    var colHead = IColumnList.Where(wh => wh.DataField == item.ColumnName).FirstOrDefault();
                    if (colHead != null)
                    {
                        string headText = string.IsNullOrEmpty(colHead.ResourceValue) ? (string.IsNullOrEmpty(colHead.HeaderText) ? colHead.DataField : colHead.HeaderText) : colHead.ResourceValue;

                        var hasName = _dt.Columns.Cast<DataColumn>().Any(c => c.ColumnName == headText);
                        if (!hasName)
                            item.ColumnName = headText;

                    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    Logging = new Prototype.Providers.Logging(this, ex);
                    //    RaiseLogging();
                    //}
                }


                #endregion

                if (_dt != null && _dt.Rows.Count > 0)
                {
                    var filename = nameSpaces[nameSpaces.Count() - 1];

                    if (!string.IsNullOrEmpty(ExportFileName))
                        filename = ExportFileName;

                    this.ToExcel(_dt, filename + ".xlsx");
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
            finally
            {
                _dt = null;
            }
        }

        private void ToExcel(DataTable dt, string Filename)
        {
            MemoryStream ms = DataTableToExcelXlsx(dt, "Sheet1");
            ms.WriteTo(HttpContext.Current.Response.OutputStream);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Filename);
            HttpContext.Current.Response.StatusCode = 200;
            HttpContext.Current.Response.End();
        }

        private static MemoryStream DataTableToExcelXlsx(DataTable table, string sheetName)
        {
            MemoryStream Result = new MemoryStream();
            ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(sheetName);

            ws.Cells["A1"].LoadFromDataTable(table, true);

            var countCol = 1;
            foreach (DataColumn tableCol in table.Columns)
            {
                if (tableCol.DataType == typeof(DateTime))
                {
                    using (ExcelRange col = ws.Cells[2, countCol, 2 + table.Rows.Count, countCol])
                    {
                        col.Style.Numberformat.Format = FieldsStatic.DateFormat + " " + FieldsStatic.TimeFormat;
                        col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                }

                countCol++;
            }

            pack.SaveAs(Result);
            return Result;
        }

        public bool IsReusable
        {
            get { return false; }
        }

        #endregion


        #region Option Field / Grid Column

        //void ViewOptionField()
        //{
        //    var _gridOptionField = new List<GridOptionField>();

        //    int inx = 5; // Referance grid column next index = 5
        //    foreach (var iCol in IColumnList)
        //    {
        //        _gridOptionField.Add(new GridOptionField()
        //        {
        //            IsSelect = true,
        //            ColumnName = iCol.HeaderText,
        //            Index = inx
        //        });

        //        inx++;
        //    }

        //    gridOptField.DataSource = _gridOptionField;
        //    gridOptField.DataBind();
        //}

        //protected void btOptField_Click(object sender, EventArgs e)
        //{
        //    panelOptField.Visible = !panelOptField.Visible;
        //}

        //protected void btOptSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        bool hasChanged = false;
        //        foreach (GridViewRow row in gridOptField.Rows)
        //        {
        //            var index = Convert.ToInt16((row.Cells[0].FindControl("hidOptIndex") as HiddenField).Value);
        //            var check = (row.Cells[0].FindControl("chkOptSelect") as CheckBox).Checked;


        //            if (gvView.Columns[index].Visible != check)
        //            {
        //                gvView.Columns[index].Visible = check;

        //                if (!hasChanged)
        //                    hasChanged = true;
        //            }
        //        }

        //        if (hasChanged)
        //            updateContentView.Update();

        //        panelOptField.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging = new Prototype.Providers.Logging(this, ex);
        //        RaiseLogging();
        //    }
        //}

        #endregion


        #region New Function 20191024

        public IPopupEntity PopupEntitySource { get; set; }

        public event System.EventHandler NewClick;

        public bool NewVisible
        {
            get
            {
                if (ViewState["NewVisible"] == null)
                    ViewState["NewVisible"] = true;

                return (bool)ViewState["NewVisible"];
            }
            set
            {
                if (btNew != null)
                    btNew.Visible = value;

                ViewState["NewVisible"] = value;
            }
        }

        protected void btNew_Click(object sender, EventArgs e)
        {
            if (PopupEntitySource != null)
                PopupEntitySource.New();

            if (NewClick != null)
                NewClick(sender, e);
        }


        private void PopupEntitySource_RaiseEntitySaved(bool _saveStatus)
        {
            if (_saveStatus)
            {
                this.DataBind();
            }
        }

        #endregion


        public void ClearFilter(IGridColumnExt _iCol)
        {

            var iCtrl = EntityExt.IControls.FirstOrDefault(x => x.DataFieldValue == _iCol.DataFieldFilter || x.DataFieldValue == _iCol.DataField);
            if (iCtrl == null)
            {
                return;
            }

            iCtrl.Clear();
            iCtrl.Update();

        }

        public void ClearFilters()
        {
            if(this.GridFilters == null)
            {
                return;
            }

            foreach (var _container in this.GridFilters)
            {
                var iCtrl = EntityExt.IControls.FirstOrDefault(x => x.DataFieldValue == _container.DataField);
                if (iCtrl == null)
                {
                    return;
                }

                iCtrl.Clear();
                iCtrl.Update();
            }
        }

        public event System.EventHandler GridRefreshClick;


        string SEARCH_ENTER_KEY
        {
            get
            {
                if (ViewState["SEARCH_ENTER_KEY"] == null)
                    ViewState["SEARCH_ENTER_KEY"] = "_" + gvView.ClientID + "_SEARCH_ENTER_KEY";

                return (string)ViewState["SEARCH_ENTER_KEY"];
            }
        }

        void TextFlterEnterChange(_IInputText _iInputText)
        {
            btSearchConfirm_Click(null, EventArgs.Empty);

            _iInputText.Focus();
        }


        public string IncludeValueFields
        {
            get
            {
                string concatenated = string.Join(",", gvView.DataKeyNames);
                return concatenated;
            }
            set
            {
                gvView.DataKeyNames = value.Split(',');
            }
        }

        private GridIncFieldValue[] GetRowExtendValues(GridViewCommandEventArgs e)
        {
            var ctrl = (Control)e.CommandSource;
            var gvr = (GridViewRow)ctrl.NamingContainer;

            var ext_values = new GridIncFieldValue[gvView.DataKeyNames.Count()];

            int inx = 0;
            foreach (var field in gvView.DataKeyNames)
            {
                var val = gvView.DataKeys[gvr.RowIndex].Values[field];

                ext_values[inx] = new GridIncFieldValue();
                ext_values[inx].DataField = field;
                ext_values[inx].Value = val;

                inx++;
            }

            return ext_values;
        }

        protected void btTemplate_Click(object sender, EventArgs e)
        {
            if (GridExportTemplate != null)
            {
                GridExportTemplate();
            }
        }
        public DataTable DataSource()
        {
            var objHand = Activator.CreateInstance(this.SourceAssemblyName, this.SourceClassName);
            var func = (Prototype.Providers.IGridExportData)objHand.Unwrap();

            var _dt = func.GetExportData(this.FilterGridOptions, this.FilterCtrlOptions, this.ExportSourceType);

            if (GridPreExportData != null)
                GridPreExportData(ref _dt);

            //==============================
            //====== Sort
            DataView dv = _dt.DefaultView;
            string sort = string.Empty;
            if (this.GridIsSort.Count > 0)
            {
                foreach (var item in GridIsSort)
                {
                    sort += item.DataField + " " + item.Direction + ",";
                }

                sort = sort.Substring(0, sort.Length - 1);
                dv.Sort = sort;
            }
            else
            {
                if (!string.IsNullOrEmpty(GridSortDefault))
                {
                    dv.Sort = GridSortDefault;
                }
            }


            return dv.ToTable();
        }
    }
}