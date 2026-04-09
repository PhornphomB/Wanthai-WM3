using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _UControls
{
    public partial class InputDropDown : EntityCustomLayout, _IInputDropDown
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            baseControl.BaseFilterChangedValue += baseControl_BaseFilterChangedValue;

            if (!Page.IsPostBack)
            {
                baseControl.SetDisableFilter(FilterAt.Contains);
                baseControl.SetDisableFilter(FilterAt.LessThan);
                baseControl.SetDisableFilter(FilterAt.LessThanEqual);
                baseControl.SetDisableFilter(FilterAt.MoreThan);
                baseControl.SetDisableFilter(FilterAt.MoreThanEqual);
                baseControl.SetDisableFilter(FilterAt.Between);
                baseControl.SetDisableFilter(FilterAt.Empty);

                if (this.AllowSearchMultiValue)
                {
                    //comboValue.Attributes.Add("class", "selectpicker show-tick");
                    //comboValue.Attributes.Add("multiple data-selected-text-format", "count>3");

                    comboValue.Attributes.Add("class", "selectpicker");
                    comboValue.Attributes.Add("multiple", "multiple");

                }
                else
                {
                    comboValue.Attributes.Add("class", "selectpicker");
                }

                comboValue.Attributes.Add("data-container", "body");
                comboValue.Attributes.Add("data-size", "10");

                if (this.IsPrimary)
                {
                    this.DataStyle = "btn-validate";
                    reqValidate.ControlToValidate = comboValue.ID;
                    reqValidate.ValidationGroup = this.ValidateGroup;
                }
                else
                {
                    if (string.IsNullOrEmpty(this.DataStyle))
                    {
                        this.DataStyle = "btn-default";
                    }

                    reqValidate.Visible = false;
                }

                if (this.ControlWidth > 0)
                {
                    comboValue.Attributes.Add("data-width", this.ControlWidth.ToString() + "px");
                }

                if (this.ControlIndex > 0)
                    comboValue.TabIndex = this.ControlIndex;

                if (!this.Enabled)
                    comboValue.Attributes.Add("disabled", "");
                else
                    comboValue.Attributes.Remove("disabled");

                if (this.Enabled)
                {
                    if (this.Readonly)
                        comboValue.Attributes.Add("disabled", "");
                    else
                        comboValue.Attributes.Remove("disabled");
                }

                if (!DisableSearchData)
                    comboValue.Attributes.Add("data-live-search", "true");

                if (!string.IsNullOrEmpty(this.DataStyle))
                    comboValue.Attributes.Add("data-style", this.DataStyle);

                if (this.AutoPostBack)
                {
                    linkTrigger.Attributes.Add("onclick", "__doPostBack('" + linkTrigger.ClientID + "', 'CHANGE_VAL');");
                }

                comboValue.Attributes.Add("onchange", "javascript:dropDownValueChanged('" + comboValue.ClientID + "','" + hidMultiValue.ClientID + "','#"
                + linkTrigger.ClientID + "'," + this.AutoPostBack.ToString().ToLower() + ")");

                //Set Label
                baseControl.LabelText.IsPrimary = this.IsPrimary;

                if (!string.IsNullOrEmpty(LabelText))
                    baseControl.LabelText.InnerText = LabelText;

                if (!string.IsNullOrEmpty(DataFieldShowValue))
                {
                    baseControl.LabelText.DefaultText = DataFieldShowValue;
                }
                else if (!string.IsNullOrEmpty(DataFieldValue))
                {
                    baseControl.LabelText.DefaultText = DataFieldValue;
                }
                else
                {
                    baseControl.LabelText.DefaultText = this.ResourceName;
                }

                this.TextInLine = this.TextInLine;

                if (baseControl.VisibleBaseContent != VisibleExt)
                    baseControl.VisibleBaseContent = VisibleExt;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e) //Page Stat working after Load Control events
        {
            var clientScript = "$(document).ready(function() {";
            clientScript += "$('#" + comboValue.ClientID + @"').multiselect({
                numberDisplayed: 2,
                includeSelectAllOption: true,
                enableCaseInsensitiveFiltering: true,
                allSelectedText: 'All',
                })";

            if (!Page.IsPostBack)
            {
                // Defult Select All
                if (this.AllowSearchMultiValue)
                    clientScript += ".multiselect('selectAll', false).multiselect('updateButtonText')";

            }
            else //Manual Event Postback of Trigger Control
            {
                var eventControl = Request.Params.Get("__EVENTTARGET");
                var eventArgument = Request.Params.Get("__EVENTARGUMENT");

                if (eventControl == linkTrigger.ClientID && eventArgument != string.Empty)
                {
                    if (eventArgument.ToUpper() == "CHANGE_VAL")
                    {
                        comboValue_SelectedIndexChanged("CHANGE_VAL", EventArgs.Empty);
                    }
                }
            }

            if (this.AllowSearchMultiValue)
            {
                clientScript += ";";

                var values = hidMultiValue.Value.Split(',').Where(val => val.ToLower() != "_load");
                foreach (var val in values)
                {
                    if (!string.IsNullOrEmpty(val))
                    {
                        clientScript += "$('#" + comboValue.ClientID + @"').multiselect('select', '" + val + "');";
                    }
                }

                clientScript += "});";

                Page.ScriptPageRegister(clientScript);
            }
        }


        public void SetEnableFilter(Prototype.Providers.FilterAt filter)
        {
            baseControl.SetEnableFilter(filter);
        }
        public void SetDisableFilter(Prototype.Providers.FilterAt filter)
        {
            baseControl.SetDisableFilter(filter);
        }

        public void Update()
        {
            baseControl.UpdateContent();
        }

        #region Properties Filter

        void baseControl_BaseFilterChangedValue(Prototype.Providers.FilterAt _filterAt)
        {
            if (_filterAt == Prototype.Providers.FilterAt.None)
            {
                if (Filterable == true)
                {
                    Enabled = false;
                }
            }
            else
            {
                if (Enabled == false) Enabled = true;
            }
        }

        public Prototype.Providers.FilterAt DefaultFilter
        {
            get
            {
                return baseControl.DefaultFilter;
            }
            set
            {
                baseControl.DefaultFilter = value;
            }
        }

        public bool Filterable
        {
            get
            {
                if (ViewState["Filterable"] == null)
                    ViewState["Filterable"] = false;

                return (bool)ViewState["Filterable"];
            }
            set
            {
                baseControl.Filterable = value;

                ViewState["Filterable"] = value;
            }
        }

        public bool FixFilter
        {
            get
            {
                return baseControl.FixFilter;
            }
            set
            {
                baseControl.FixFilter = value;
            }
        }

        public bool IsFilter
        {
            get { return baseControl.IsFilter; }
        }

        public void ClearFilter()
        {
            baseControl.ClearFilter();
        }

        #endregion


        #region Properties Control

        public string ValidateMessage
        {
            get
            {
                if (ViewState["ValidateMessage"] == null)
                    ViewState["ValidateMessage"] = string.Empty;

                return (string)ViewState["ValidateMessage"];
            }
            set
            {
                ViewState["ValidateMessage"] = value;
            }
        }
        public string ValidateGroup
        {
            get
            {
                if (ViewState["ValidateGroup"] == null)
                    ViewState["ValidateGroup"] = string.Empty;

                return (string)ViewState["ValidateGroup"];
            }
            set
            {
                ViewState["ValidateGroup"] = value;

                try
                {
                    reqValidate.ValidationGroup = value;
                }
                catch { }
            }
        }
        public string DataFieldValue
        {
            get
            {
                if (ViewState["DataFieldValue"] == null)
                    ViewState["DataFieldValue"] = string.Empty;

                return (string)ViewState["DataFieldValue"];
            }
            set
            {
                ViewState["DataFieldValue"] = value;
            }
        }
        public string DataFieldTempValue
        {
            get
            {
                if (ViewState["DataFieldTempValue"] == null)
                    ViewState["DataFieldTempValue"] = string.Empty;

                return (string)ViewState["DataFieldTempValue"];
            }
            set
            {
                ViewState["DataFieldTempValue"] = value;
            }
        }
        public string DataFieldShowValue
        {
            get
            {
                if (ViewState["DataFieldShowValue"] == null)
                    ViewState["DataFieldShowValue"] = string.Empty;

                return (string)ViewState["DataFieldShowValue"];
            }
            set
            {
                ViewState["DataFieldShowValue"] = value;
            }
        }
        public bool IsPrimary
        {
            get
            {
                if (ViewState["IsPrimary"] == null)
                    ViewState["IsPrimary"] = false;

                return (bool)ViewState["IsPrimary"];
            }
            set
            {
                ViewState["IsPrimary"] = value;

                if (Page != null && Page.IsPostBack && reqValidate != null && comboValue != null)
                {
                    try
                    {
                        if (value)
                        {
                            this.DataStyle = "btn-validate";
                            reqValidate.ControlToValidate = comboValue.ID;
                            reqValidate.ValidationGroup = this.ValidateGroup;
                            reqValidate.Visible = true;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(this.DataStyle))
                            {
                                this.DataStyle = "btn-default";
                            }

                            reqValidate.Visible = false;
                        }
                    }
                    catch { }
                }
            }
        }
        public bool IsKey
        {
            get
            {
                if (ViewState["IsKey"] == null)
                    ViewState["IsKey"] = false;

                return (bool)ViewState["IsKey"];
            }
            set
            {
                ViewState["IsKey"] = value;
            }
        }
        public bool IsStaticValue
        {
            get
            {
                if (ViewState["IsStaticValue"] == null)
                    ViewState["IsStaticValue"] = false;

                return (bool)ViewState["IsStaticValue"];
            }
            set
            {
                ViewState["IsStaticValue"] = value;
            }
        }

        public string LabelText
        {
            get
            {
                return ViewState["LabelText"] == null ? string.Empty : ViewState["LabelText"].ToString();
            }
            set
            {
                ViewState["LabelText"] = value;

                if (baseControl != null)
                    baseControl.LabelText.InnerText = value;
            }
        }

        #endregion


        #region Properties Custom By Control

        public string DataStyle
        {
            get
            {
                if (ViewState["DataStyle"] == null)
                    ViewState["DataStyle"] = string.Empty;

                return (string)ViewState["DataStyle"];
            }
            set
            {
                ViewState["DataStyle"] = value;
            }
        }

        public bool DisableSearchData
        {
            get
            {
                if (ViewState["DisableSearchData"] == null)
                    ViewState["DisableSearchData"] = true;

                return (bool)ViewState["DisableSearchData"];
            }
            set
            {
                ViewState["DisableSearchData"] = value;
            }
        }

        public bool AutoPostBack
        {
            get
            {
                if (ViewState["AutoPostBack"] == null)
                    ViewState["AutoPostBack"] = false;

                return (bool)ViewState["AutoPostBack"];
            }
            set
            {
                ViewState["AutoPostBack"] = value;
            }
        }

        public bool AllowSearchMultiValue
        {
            get
            {
                if (ViewState["AllowSelectMultiValue"] == null)
                    ViewState["AllowSelectMultiValue"] = false;

                return (bool)ViewState["AllowSelectMultiValue"];
            }
            set
            {
                ViewState["AllowSelectMultiValue"] = value;
            }
        }

        public ComboType ComboType
        {
            get
            {
                if (ViewState["ComboType"] == null)
                    ViewState["ComboType"] = ComboType.Guid;

                return (ComboType)ViewState["ComboType"];
            }
            set
            {
                ViewState["ComboType"] = value;
            }
        }

        public string GetText()
        {
            if (this.CountItem > 0)
                return comboValue.SelectedItem.Text;
            else
                return string.Empty;
        }

        public bool IsDefaultValue
        {
            get
            {
                if (ViewState["IsDefaultValue"] == null)
                    ViewState["IsDefaultValue"] = false;

                return (bool)ViewState["IsDefaultValue"];
            }
            set
            {
                ViewState["IsDefaultValue"] = value;
            }
        }

        private dynamic GetValueByString(string _value)
        {
            if (string.IsNullOrEmpty(_value.Trim()))
                return null;

            switch (ComboType)
            {
                case ComboType.String:
                    return _value;

                case ComboType.Integer:
                    if (!_value.IsEmpty())
                        return Convert.ToInt32(_value);
                    break;

                case ComboType.Double:
                    if (!_value.IsEmpty())
                        return Convert.ToDouble(_value);
                    break;

                case ComboType.Decimal:
                    if (!_value.IsEmpty())
                        return Convert.ToDecimal(_value);
                    break;

                case ComboType.Boolean:
                    if (!_value.IsEmpty())
                        return Convert.ToBoolean(_value);
                    break;

                case ComboType.Guid:
                    if (!_value.IsEmpty())
                        return Guid.Parse(_value.ToString());
                    else
                        return Guid.Empty;

                default:
                    return _value;

            }

            if (this.IsDefaultValue)
            {
                if (ComboType == _UControls.ComboType.String)
                    return string.Empty;
                else if (ComboType == _UControls.ComboType.Boolean)
                    return false;
                else
                    return 0;
            }
            else
            {
                return null;
            }
        }

        public dynamic GetValue()
        {
            return GetValueByString(comboValue.SelectedValue);
        }

        public dynamic[] GetValues()
        {
            var values = hidMultiValue.Value.Split(',').Where(val => !string.IsNullOrEmpty(val));
            dynamic[] reValues = new dynamic[values.Count()];

            int inx = 0;
            foreach (var val in values)
            {
                reValues[inx] = GetValueByString(val);
                inx++;
            }

            return reValues;
        }

        public void SetValue(dynamic value)
        {
            try
            {
                if (value == null)
                {
                    comboValue.SelectedIndex = 0;
                }
                else
                {
                    comboValue.SelectedValue = value.ToString();
                }
            }
            catch (Exception ex)
            {

            }

            baseControl.UpdateContent();
            //comboValue_SelectedIndexChanged(null, EventArgs.Empty);
        }
        public void SetText(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    comboValue.SelectedIndex = 0;
                }
                else
                {
                    ListItem listI = comboValue.Items.FindByText(text);
                    if (listI != null)
                    {
                        comboValue.SelectedValue = listI.Value;
                    }
                }
            }
            catch (Exception)
            {

            }

            baseControl.UpdateContent();
            //comboValue_SelectedIndexChanged(null, EventArgs.Empty);
        }

        public virtual void ClearItems()
        {
            this.MethodQueryProperty = delegate () { return new List<Prototype.Providers.Property>().AsQueryable(); };
            this.BindDataSource();
        }

        #endregion


        #region Customize Binding Function

        public Action<dynamic> PostValueChanged { get; set; }

        public Func<IQueryable<Property>> MethodQueryProperty { get; set; }

        protected bool IsBindinged
        {
            get
            {
                if (ViewState["IsBindinged"] == null)
                    ViewState["IsBindinged"] = false;

                return (bool)ViewState["IsBindinged"];
            }
            set
            {
                ViewState["IsBindinged"] = value;
            }
        }

        public int CountItem
        {
            get
            {
                if (comboValue == null)
                    return 0;

                var count = 0;
                if (LoadLazyLimit > 0 && HasDefaultDisplay) count = (comboValue.Items.Count - 2);
                else if ((LoadLazyLimit > 0) || (HasDefaultDisplay)) count = (comboValue.Items.Count - 1);
                else count = comboValue.Items.Count;

                if (count >= 0)
                    return count;
                else
                    return 0;
            }
        }

        protected bool HasDefaultDisplay
        {
            get
            {
                if (ViewState["HasDefaultDisplay"] == null)
                    ViewState["HasDefaultDisplay"] = false;

                return (bool)ViewState["HasDefaultDisplay"];
            }
            set
            {
                ViewState["HasDefaultDisplay"] = value;
            }
        }

        public bool UseDefaultDisplay
        {
            get
            {
                if (ViewState["UseDefaultDisplay"] == null)
                    ViewState["UseDefaultDisplay"] = false;

                return (bool)ViewState["UseDefaultDisplay"];
            }
            set
            {
                ViewState["UseDefaultDisplay"] = value;
            }
        }

        public string DisplayDefault
        {
            get
            {
                return (string)ViewState["DisplayDefault"];
            }
            set
            {
                ViewState["DisplayDefault"] = value;
            }
        }

        public int LoadLazyLimit
        {
            get
            {
                if (ViewState["LoadLazyLimit"] == null)
                    ViewState["LoadLazyLimit"] = 0;

                return ((int)ViewState["LoadLazyLimit"]);
            }
            set
            {
                ViewState["LoadLazyLimit"] = value;
            }
        }

        protected virtual void LoadLazyData()
        {
            if (MethodQueryProperty == null)
                return;

            var _skipRow = CountItem;

            var query = MethodQueryProperty();
            var sourceList = query.Skip(_skipRow).Take(LoadLazyLimit).ToArray();

            IsBindinged = false;

            foreach (var item in sourceList)
            {
                comboValue.Items.Add(new ListItem() { Text = item.Name, Value = item.Code });
            }

            if (LoadLazyLimit > 0)
            {
                var itemLoad = new ListItem() { Value = "_load", Text = "Load more..." };
                comboValue.Items.Remove(itemLoad);
                comboValue.Items.Add(itemLoad);
            }

            IsBindinged = true;

            if (sourceList.Count() == 0)
                Page.MessageWarning("No new data at this time.");

            baseControl.UpdateContent();
        }

        public void BindDataSource()
        {
            BindDataSource(this.DisplayDefault);
        }
        public virtual void BindDataSource(string _displayDefault)
        {
            if (MethodQueryProperty == null)
                return;

            if (this.DisplayDefault != _displayDefault)
                this.DisplayDefault = _displayDefault;

            try
            {
                var query = MethodQueryProperty();

                List<Property> sourceList = null;
                var countRealItem = 0;

                if (LoadLazyLimit > 0)
                {
                    sourceList = query.Take(LoadLazyLimit).ToList();
                    countRealItem = sourceList.Count;

                    sourceList.Add(new Property() { Code = "_load", Name = "Load more..." });
                }
                else
                {
                    sourceList = query.ToList();
                    countRealItem = sourceList.Count;
                }

                IsBindinged = false;
                HasDefaultDisplay = false;

                if ((!string.IsNullOrEmpty(_displayDefault) || this.AllowSearchMultiValue) && ((countRealItem > 1) || (UseDefaultDisplay == true) || (!string.IsNullOrEmpty(this.DataStyle))))
                {
                    Prototype.Providers.Controls.ControlsList.BindListBox(ref comboValue, sourceList, this.DisplayDefault);

                    if (!this.AllowSearchMultiValue)
                    {
                        HasDefaultDisplay = true;
                    }
                }
                else
                {
                    Prototype.Providers.Controls.ControlsList.BindListBoxNoneDefault(ref comboValue, sourceList);
                }
                //Edit by bank 2020-09-11
                //if (countRealItem > 0 && ((this.IsPrimary && countRealItem == 1 && this.UseDefaultDisplay == false) || this.UseDefaultDisplay == false || string.IsNullOrEmpty(this.DisplayDefault)))
                //if (countRealItem > 0 && this.UseDefaultDisplay == false && this.AllowSearchMultiValue == false && (((this.IsPrimary || this.IsKey) && countRealItem == 1) || string.IsNullOrEmpty(this.DisplayDefault)))
                //{
                //    Prototype.Providers.Controls.ControlsList.BindListBoxNoneDefault(ref comboValue, sourceList);
                //    var ent = sourceList.FirstOrDefault();
                //    if (ent != null)
                //    {
                //        var valData = ent.guid_member == Guid.Empty ? (dynamic)ent.value_member : ent.guid_member;
                //        this.SetObjectValue(valData);
                //        this.DefaultValue = valData;
                //        // ent.guid_member != Guid.Empty ? ent.Code : ent.guid_member;
                //        //this.DefaultValue = (dynamic)ent.Code;
                //    }
                //}
                //else
                //{
                //    Prototype.Providers.Controls.ControlsList.BindListBox(ref comboValue, sourceList, this.DisplayDefault);
                //    HasDefaultDisplay = true;
                //    this.DefaultValue = null;

                //}


                IsBindinged = true;

                baseControl.UpdateContent();
                comboValue_SelectedIndexChanged(null, EventArgs.Empty);
            }
            catch
            {

            }
        }

        protected void comboValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsBindinged) return;

            if (this.AutoPostBack && comboValue.SelectedValue != "_load")
            {
                if (PostValueChanged != null)
                    PostValueChanged(this.GetValue());
            }
            else if (this.AutoPostBack && comboValue.SelectedValue == "_load")
            {
                this.LoadLazyData();
            }
        }

        #endregion


        #region Properties Implement Difference

        public Prototype.Providers.IFilter GetFilter()
        {
            baseControl.Filter.DataPropertyName = DataFieldValue;
            baseControl.Filter.Value = GetValue();

            if (this.AllowSearchMultiValue)
                baseControl.Filter.Values = GetValues();

            return baseControl.Filter;
        }

        public InputType InputType
        {
            get { return _UControls.InputType.DropDownNormal; }
        }
        public int MaxLength
        {
            get
            {
                return 0;
            }
            set { }
        }
        public int ControlWidth
        {
            get
            {
                if (ViewState["ControlWidth"] == null)
                    ViewState["ControlWidth"] = 0;

                return (int)ViewState["ControlWidth"];
            }
            set
            {
                ViewState["ControlWidth"] = value;
            }
        }
        public short ControlIndex
        {
            get
            {
                if (ViewState["TabIndex"] == null)
                    ViewState["TabIndex"] = 0;

                return Convert.ToSByte(ViewState["TabIndex"].ToString());
            }
            set
            {
                ViewState["TabIndex"] = value;

                try
                {
                    if (IsPostBack && value > 0)
                        comboValue.TabIndex = value;
                }
                catch { }
            }
        }
        public bool Readonly
        {
            get
            {
                if (ViewState["Readonly"] == null)
                    ViewState["Readonly"] = false;

                return (bool)ViewState["Readonly"];
            }
            set
            {
                ViewState["Readonly"] = value;

                try
                {
                    if (IsPostBack)
                    {
                        if (value)
                            comboValue.Attributes.Add("disabled", "");
                        else
                            comboValue.Attributes.Remove("disabled");
                    }
                }
                catch { }
            }
        }
        public bool Enabled
        {
            get
            {
                if (ViewState["Enabled"] == null)
                    ViewState["Enabled"] = true;

                return (bool)ViewState["Enabled"];
            }
            set
            {
                ViewState["Enabled"] = value;

                try
                {
                    //if (IsPostBack)
                    //{
                    //    if (!value)
                    //        comboValue.Attributes.Add("disabled", "");
                    //    else
                    //        comboValue.Attributes.Remove("disabled");
                    //}

                    comboValue.Enabled = value;

                    if (!value)
                    {
                        if (this.IsPrimary)
                            IsPrimaryState = this.IsPrimary;

                        //this.IsPrimary = false;
                    }
                    else if (IsPrimaryState && !this.IsPrimary)
                    {
                        this.IsPrimary = this.IsPrimaryState;
                    }
                }
                catch { }
            }
        }

        private bool IsPrimaryState
        {
            get
            {
                if (ViewState["IsPrimaryState"] == null)
                    ViewState["IsPrimaryState"] = false;

                return (bool)ViewState["IsPrimaryState"];
            }
            set
            {
                ViewState["IsPrimaryState"] = value;
            }
        }


        public void SetObjectValue(object _value)
        {
            SetValue(_value);
        }

        public object GetObjectValue()
        {
            if (this.AllowSearchMultiValue)
            {
                var arr = string.Join(",", GetValues());
                return arr;
            }
            else
            {
                return GetValue();

            }
        }

        public void Clear()
        {
            if (this.IsStaticValue) return;

            if (comboValue.Items.Count > 0)
            {
                comboValue.SelectedIndex = 0;

                if (DefaultValue != null)
                    SetObjectValue(DefaultValue);

                comboValue_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        public dynamic DefaultValue
        {
            get
            {
                return (dynamic)ViewState["DefaultValue"];
            }
            set
            {
                ViewState["DefaultValue"] = value;
            }
        }

        //public bool VisibleExt
        //{
        //    get
        //    {
        //        if (ViewState["VisibleExt"] == null)
        //            ViewState["VisibleExt"] = true;

        //        return (bool)ViewState["VisibleExt"];
        //    }
        //    set
        //    {
        //        ViewState["VisibleExt"] = value;

        //        try
        //        {
        //            if (IsPostBack)
        //            {
        //                // panelValue.Visible = value;
        //                baseControl.Visible = value;
        //            }
        //        }
        //        catch { }
        //    }
        //}

        #endregion


        public int ControlSequence
        {
            get
            {
                if (ViewState["ControlSequence"] == null)
                    ViewState["ControlSequence"] = -1;

                return (int)ViewState["ControlSequence"];
            }
            set
            {
                ViewState["ControlSequence"] = value;
            }
        }
        public string ControlGroup
        {
            get
            {
                if (ViewState["ControlGroup"] == null)
                    ViewState["ControlGroup"] = string.Empty;

                return (string)ViewState["ControlGroup"];
            }
            set
            {
                ViewState["ControlGroup"] = value;
            }
        }

        public bool ClearByIControl
        {
            get
            {
                if (ViewState["ClearByIControl"] == null)
                    ViewState["ClearByIControl"] = false;

                return (bool)ViewState["ClearByIControl"];
            }
            set
            {
                ViewState["ClearByIControl"] = value;
            }
        }

        public string ResourceGroup
        {
            get
            {
                if (ViewState["ResourceGroup"] == null)
                    ViewState["ResourceGroup"] = string.Empty;

                return (string)ViewState["ResourceGroup"];
            }
            set
            {
                ViewState["ResourceGroup"] = value;

                if (baseControl != null)
                    baseControl.LabelText.ResourceGroup = value;
            }
        }

        public string ResourceName
        {
            get
            {
                if (ViewState["ResourceName"] == null)
                    ViewState["ResourceName"] = string.Empty;

                return (string)ViewState["ResourceName"];
            }
            set
            {
                ViewState["ResourceName"] = value;

                if (baseControl != null)
                    baseControl.LabelText.ResourceName = value;
            }
        }

        public string ResourceValue
        {
            get
            {
                if (ViewState["ResourceValue"] == null)
                    ViewState["ResourceValue"] = string.Empty;

                return (string)ViewState["ResourceValue"];
            }
            set
            {
                ViewState["ResourceValue"] = value;

                if (baseControl != null)
                    baseControl.LabelText.ResourceValue = value;
            }
        }

        public dynamic DefaultSelectedValue { get; set; }

        public void ValueChange()
        {
            comboValue_SelectedIndexChanged(null, EventArgs.Empty);
        }

        public bool TextInLine
        {
            get
            {
                if (ViewState["TextInLine"] == null)
                    ViewState["TextInLine"] = false;

                return (bool)ViewState["TextInLine"];
            }
            set
            {
                if (baseControl != null)
                    baseControl.TextInLine = value;

                ViewState["TextInLine"] = value;
            }
        }

        public string ControlId
        {
            set
            {
                ID = value;
            }
            get
            {
                return ID;
            }
        }

        public void AddAtributeBaseContent(string _key, string _value)
        {
            if (baseControl != null)
                baseControl.AddAtributeBaseContent(_key, _value);
        }

        public string BaseContentCss
        {
            get
            {
                if (baseControl != null)
                    return baseControl.BaseContentCss;
                else
                    return string.Empty;
            }
            set
            {
                if (baseControl != null)
                    baseControl.BaseContentCss = value;
            }
        }

        public bool VisibleLabel
        {
            set
            {
                if (baseControl != null)
                {
                    baseControl.VisibleLabel = value;
                }
            }
        }

        public bool VisibleExt
        {
            get
            {
                if (ViewState["VisibleExt"] == null)
                    ViewState["VisibleExt"] = true;

                return (bool)ViewState["VisibleExt"];
                //return base.Visible;
            }

            set
            {
                ViewState["VisibleExt"] = value;
                //base.Visible = value;

                if (baseControl != null)
                    baseControl.VisibleBaseContent = value;
            }
        }

        public bool DisplaySingleValue
        {
            get
            {
                if (ViewState["DisplaySingleValue"] == null)
                    ViewState["DisplaySingleValue"] = true;

                return (bool)ViewState["DisplaySingleValue"];
            }

            set
            {
                ViewState["DisplaySingleValue"] = value;
            }
        }

    }
}