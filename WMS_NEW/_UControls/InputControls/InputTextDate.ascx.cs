using Prototype.Providers;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _UControls
{
    public delegate void TextDateValueChanged(string _value);

    public partial class InputTextDate : EntityCustomLayout, _IInputTextDate
    {
        public event TextDateValueChanged TextValueChanged;
        public Action<dynamic> PostValueChanged { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            baseControl.BaseFilterChangedValue += baseControl_BaseFilterChangedValue;
            txtValue.TextChanged += TxtValue_TextChanged;


            if (string.IsNullOrEmpty(KeyEnterName) && TextEnterChanged != null)
                KeyEnterName = "_ENTER_CHANGE";


            if (!Page.IsPostBack)
            {
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.Contains);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.Empty);

                txtValue.AutoPostBack = this.AutoPostBack;

                //switch (TextMode)
                //{
                //    case DateTimeType.DateTime:
                //        txtValue.Attributes.Add("placeholder", DateTimeFormat);
                //        txtValueTo.Attributes.Add("placeholder", DateTimeFormat);
                //        break;
                //    case DateTimeType.Date:
                //        txtValue.Attributes.Add("placeholder", DateFormat);
                //        txtValueTo.Attributes.Add("placeholder", DateFormat);
                //        break;
                //    case DateTimeType.Time:
                //        txtValue.Attributes.Add("placeholder", TimeFormat);
                //        txtValueTo.Attributes.Add("placeholder", TimeFormat);
                //        break;
                //}

                panelValueTo.Visible = false;


                if (this.ControlWidth > 0)
                    txtValue.Width = txtValueTo.Width = this.ControlWidth;

                if (this.ControlIndex > 0)
                    txtValue.TabIndex = txtValueTo.TabIndex = this.ControlIndex;

                txtValue.Enabled = txtValueTo.Enabled = this.Enabled;
                // span_picker_from.Visible = span_picker_to.Visible = this.Enabled;

                if (this.Enabled)
                {
                    txtValue.Enabled = txtValueTo.Enabled = !this.Readonly;
                    // span_picker_from.Visible = span_picker_to.Visible = !this.Readonly;
                }

                this.IsPrimary = this.IsPrimary;
                baseControl.LabelText.IsPrimary = this.IsPrimary;


                if (!string.IsNullOrEmpty(LabelText))
                    baseControl.LabelText.InnerText = LabelText;

                baseControl.LabelText.AttibuteFor = txtValue.ClientID;


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

                if (this.AutoPostBack)
                {
                    linkDateTrigger.Attributes.Add("onclick", "__doPostBack('" + linkDateTrigger.ClientID + "', 'CHANGE_VAL');");
                }



                if (this.IsDateNow)
                    SetValue(DateTime.Now);


                //baseControl.Visible = this.VisibleExt;

                baseControl_BaseFilterChangedValue(this.DefaultFilter);

                ResourceGroup = ResourceGroup;
                ResourceName = ResourceName;
                ResourceValue = ResourceValue;

                this.TextInLine = this.TextInLine;


                if (TextEnterChanged != null)
                {
                    txtValue.Attributes.Add("onfocusin", " select();"); // for text barcode scan
                    txtValue.Attributes.Add("onkeydown", "return onTextEnterChange(event,'" + txtValue.ClientID + "','" + txtValue.ClientID + KeyEnterName + "');");
                }
                if (this.IsFocusOut)
                    txtValue.Attributes.Add("onfocusout", "return onFocusOutChange(event,'" + txtValue.ClientID + "','" + txtValue.ClientID + KeyEnterName + "');");
            }
        }

        protected void Page_PreRender(object sender, EventArgs e) //Page Stat working after Load Control events
        {
            var eventControl = Request.Params.Get("__EVENTTARGET");
            var eventArgument = Request.Params.Get("__EVENTARGUMENT");


            if (eventControl == linkDateTrigger.ClientID && eventArgument != string.Empty)
            {
                if (eventArgument.ToUpper() == "CHANGE_VAL")
                {
                    linkDateTrigger_Click("CHANGE_VAL", EventArgs.Empty);
                }
            }


            var clientScript = @" $('document').ready(function () { ";

            var format = string.Empty;

            switch (TextMode)
            {
                case DateTimeType.DateTime:
                    //  clientScript += "$('#" + txtValue.ClientID + @"').datetimepicker({toolbarPlacement: 'top',
                    //showTodayButton: true,
                    //sideBySide: true,
                    //showClear: true,
                    //showClose: true,
                    //useCurrent: true,
                    //keepOpen: true,
                    //locale: 'en',
                    //toolbarPlacement: 'bottom',
                    //format: 'YYYY-MM-DD h:mm:ss',
                    //debug: true});";

                    //format = (DateFormat.ToUpper() + " " + TimeFormat.ToLower());
                    format = (DateFormat.ToUpper() + " " + TimeFormat);

                    clientScript += "$('#" + txtValue.ClientID + @"').datetimepicker({sideBySide: true, useCurrent: false, keepOpen: false, format: '" + format + "'})";
                    if (this.AutoPostBack)
                    {
                        clientScript += ".on('dp.change', function() {";
                        clientScript += "$('#" + linkDateTrigger.ClientID + "').click(); })";
                    }
                    clientScript += "; ";

                    clientScript += "$('#" + txtValueTo.ClientID + "').datetimepicker({sideBySide: true, useCurrent: false, keepOpen: false, format:'" + format + "'});";

                    txtValue.Attributes.Add("placeholder", format);
                    txtValueTo.Attributes.Add("placeholder", format);

                    break;
                case DateTimeType.Date:

                    format = DateFormat.ToUpper();

                    clientScript += "$('#" + txtValue.ClientID + "').datetimepicker({useCurrent: false, keepOpen: false, format:'" + format + "'})";
                    if (this.AutoPostBack)
                    {
                        clientScript += ".on('dp.change', function() {";
                        clientScript += "$('#" + linkDateTrigger.ClientID + "').click(); })";
                    }
                    clientScript += "; ";
                    clientScript += "$('#" + txtValueTo.ClientID + "').datetimepicker({useCurrent: false, keepOpen: false, format:'" + format + "'});";

                    txtValue.Attributes.Add("placeholder", format);
                    txtValueTo.Attributes.Add("placeholder", format);

                    break;
                case DateTimeType.Time:

                    //format = TimeFormat.ToLower();
                    format = TimeFormat;

                    clientScript += "$('#" + txtValue.ClientID + "').datetimepicker({useCurrent: false, keepOpen: false, format:'" + format + "'});";
                    clientScript += "$('#" + txtValueTo.ClientID + "').datetimepicker({useCurrent: false, keepOpen: false, format:'" + format + "'});";

                    txtValue.Attributes.Add("placeholder", format);
                    txtValueTo.Attributes.Add("placeholder", format);

                    break;
            }

            clientScript += "});";

            Page.ScriptPageRegister(clientScript, txtValue.ClientID + "_DateValueChanged");

            string script_mask = string.Empty;
            if (TextMode == DateTimeType.DateTime)
            {
                script_mask = @".inputmask({
                    mask: { 'mask': '##/##/#### ##:##:##' },
                    greedy: false,
                    definitions: { '#': { validator: '[0-9]', cardinality: 1 } }
                    }); ";
            }
            else if (TextMode == DateTimeType.Date)
            {
                script_mask = @".inputmask({
                    mask: { 'mask': '##/##/####' },
                    greedy: false,
                    definitions: { '#': { validator: '[0-9]', cardinality: 1 } }
                    }); ";
            }
            else if (TextMode == DateTimeType.Time)
            {
                script_mask = @".inputmask({
                    mask: { 'mask': '##:##:##' },
                    greedy: false,
                    definitions: { '#': { validator: '[0-9]', cardinality: 1 } }
                    }); ";
            }

            Page.ScriptPageRegister("$(document).ready(function () { " + "$('#" + txtValue.ClientID + "')" + script_mask + " });", txtValue.ClientID + "_Text_InputMask_Date");


            if (Page.IsPostBack)
            {
                if (eventControl == txtValue.ClientID && eventArgument == txtValue.ClientID + KeyEnterName)
                {
                    if (TextEnterChanged != null)
                        TextEnterChanged(this);
                }
            }
        }


        private void TxtValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.AutoPostBack && !string.IsNullOrEmpty(txtValue.Text))
                {
                    if (TextValueChanged != null)
                    {
                        //DateTime? mes = this.GetValue();
                        TextValueChanged(txtValue.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
            if (_filterAt == Prototype.Providers.FilterAt.Between)
            {
                panelValueTo.Visible = true;
                txtValue.Attributes.Add("placeholder", "From value");
                txtValueTo.Attributes.Add("placeholder", "To value");
            }
            else
            {
                panelValueTo.Visible = false;
                txtValue.Attributes.Remove("placeholder");
                txtValueTo.Attributes.Remove("placeholder");
            }

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

                if (txtValue != null)
                {
                    reqValidate.ControlToValidate = txtValue.ID;
                    reqValidate.ValidationGroup = this.ValidateGroup;

                    baseControl.LabelText.IsPrimary = value;

                    if (value && this.Enabled)
                    {
                        txtValue.CssClass = txtValueTo.CssClass = txtValue.CssClass + " primary";
                        reqValidate.Visible = true;
                        //baseControl.LabelText.IsPrimary = true;
                    }
                    else
                    {
                        txtValue.CssClass = txtValueTo.CssClass = txtValue.CssClass.Replace(" primary", "");
                        reqValidate.Visible = false;
                        //baseControl.LabelText.IsPrimary = false;
                    }

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


        public bool OrNullValue
        {
            get
            {
                if (ViewState["OrNullValue"] == null)
                    ViewState["OrNullValue"] = false;

                return (bool)ViewState["OrNullValue"];
            }
            set
            {
                ViewState["OrNullValue"] = value;
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
        //                baseControl.Visible = value;
        //            }
        //        }
        //        catch { }
        //    }
        //}

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

        #endregion


        #region Properties Custom By Control

        public DateTimeType TextMode
        {
            get
            {
                if (ViewState["TextMode"] == null)
                    ViewState["TextMode"] = DateTimeType.DateTime;

                return (DateTimeType)ViewState["TextMode"];
            }
            set
            {
                ViewState["TextMode"] = value;
            }
        }

        public DateCulture DateLanguage
        {
            get
            {
                if (ViewState["DateLanguage"] == null)
                    ViewState["DateLanguage"] = DateCulture.English;

                return (DateCulture)ViewState["DateLanguage"];
            }
            set
            {
                ViewState["DateLanguage"] = value;
            }
        }

        public string DateFormat
        {
            get
            {
                if (ViewState["DateFormat"] == null)
                    ViewState["DateFormat"] = FieldsStatic.DateFormat;

                return (string)ViewState["DateFormat"];
            }
            set
            {
                ViewState["DateFormat"] = value;
            }
        }
        public string TimeFormat
        {
            get
            {
                if (ViewState["TimeFormat"] == null)
                    ViewState["TimeFormat"] = FieldsStatic.TimeFormat;

                return (string)ViewState["TimeFormat"];
            }
            set
            {
                ViewState["TimeFormat"] = value;
            }
        }
        public string DateTimeFormat
        {
            get
            {
                if (ViewState["DateTimeFormat"] == null)
                    ViewState["DateTimeFormat"] = (DateFormat + " " + TimeFormat);

                return (string)ViewState["DateTimeFormat"];
            }
            set
            {
                ViewState["DateTimeFormat"] = value;
            }
        }

        private System.Globalization.CultureInfo CultureInfo
        {
            get
            {
                return new System.Globalization.CultureInfo(DateLanguage.GetAttrEntry().Name);
            }
        }

        private DateTime? GetValueByControl(TextBox _txt)
        {
            if (_txt.Text.Trim() != string.Empty)
            {
                var dateTime = string.Empty;

                switch (TextMode)
                {
                    case DateTimeType.DateTime:
                        dateTime = _txt.Text.Trim();
                        return DateTime.ParseExact(dateTime, DateTimeFormat, CultureInfo);
                    //break;
                    case DateTimeType.Date:
                        dateTime = _txt.Text.Trim();// + " 00:00:00";
                        return DateTime.ParseExact(dateTime, DateFormat, CultureInfo);
                    //break;
                    case DateTimeType.Time:
                        dateTime = new DateTime(1753, 1, 7).ToString(DateFormat, CultureInfo) + " " + _txt.Text.Trim();
                        return DateTime.ParseExact(dateTime, "dd/MM/yyyy " + TimeFormat, CultureInfo);
                        //break;
                }

                return null;
                //return DateTime.ParseExact(dateTime, DateTimeFormat, CultureInfo);
            }
            else
            {
                return null;
            }
        }

        public DateTime? GetValue()
        {
            return GetValueByControl(txtValue);
        }
        public DateTime? GetValueTo()
        {
            return GetValueByControl(txtValueTo);
        }

        public void SetValue(DateTime? value)
        {
            if (value != null)
            {
                switch (TextMode)
                {
                    case DateTimeType.DateTime:
                        txtValue.Text = value.Value.ToString(DateTimeFormat, CultureInfo);
                        break;
                    case DateTimeType.Date:
                        txtValue.Text = value.Value.ToString(DateFormat, CultureInfo);
                        break;
                    case DateTimeType.Time:
                        txtValue.Text = value.Value.ToString(TimeFormat, CultureInfo);
                        break;
                }
            }
            else
            {
                txtValue.Text = string.Empty;
            }
        }


        public bool IsDateNow
        {
            get
            {
                if (ViewState["IsDateNow"] == null)
                    ViewState["IsDateNow"] = false;

                return (bool)ViewState["IsDateNow"];
            }
            set
            {
                ViewState["IsDateNow"] = value;
            }
        }
        public bool IsFocusOut
        {
            get
            {
                if (ViewState["IsFocusOut"] == null)
                    ViewState["IsFocusOut"] = false;

                return (bool)ViewState["IsFocusOut"];
            }
            set
            {
                ViewState["IsFocusOut"] = value;
            }
        }
        #endregion


        #region Properties Implement Difference

        public Prototype.Providers.IFilter GetFilter()
        {
            baseControl.Filter.DataPropertyName = DataFieldValue;
            baseControl.Filter.Value = GetValue();
            baseControl.Filter.ValueTo = GetValueTo();
            return baseControl.Filter;
        }

        public InputType InputType
        {
            get { return _UControls.InputType.TextDate; }
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

                try
                {
                    if (IsPostBack && value > 0)
                        txtValue.Width = txtValueTo.Width = value;
                }
                catch { }
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
                        txtValue.TabIndex = txtValueTo.TabIndex = value;
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
                        txtValue.Enabled = txtValueTo.Enabled = !value;
                    //span_picker_from.Visible = span_picker_to.Visible = !value;
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
                    if (IsPostBack)
                    {
                        txtValue.Enabled = txtValueTo.Enabled = value;
                        //span_picker_from.Visible = span_picker_to.Visible = value;
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
            if (_value == null)
                SetValue(null);
            else
                SetValue(Convert.ToDateTime(_value));
        }

        public object GetObjectValue()
        {
            return GetValue();
        }

        public object GetObjectValueTo()
        {
            return GetValueTo();
        }

        public void Clear()
        {
            if (this.IsStaticValue) return;

            if (this.IsDateNow)
                SetValue(DateTime.Now);
            else if (DefaultValue != null)
                SetObjectValue(DefaultValue);
            else
                SetValue(null);



            txtValueTo.Text = string.Empty;

            if (DefaultFilter == FilterAt.Between)
            {
                panelValueTo.Visible = true;
            }
            else
            {
                DefaultFilter = FilterAt.Equal;
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
        public int ControlSequence
        {
            get
            {
                if (ViewState["ControlSequence"] == null)
                    ViewState["ControlSequence"] = 0;

                return (int)ViewState["ControlSequence"];
            }
            set
            {
                ViewState["ControlSequence"] = value;
            }
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
        #endregion


        #region Interface Resource

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

        #endregion


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

        protected void linkDateTrigger_Click(object sender, EventArgs e)
        {
            if (PostValueChanged != null)
            {
                PostValueChanged(this.GetValue());

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


        public void AddAttribute(string _key, string _value)
        {
            txtValue.Attributes.Add(_key, _value);
        }

        public override void Focus()
        {
            //txtValue.Focus();
            Page.ScriptPageRegister("$(document).ready(function () { $('#" + txtValue.ClientID + "').focus();  });", txtValue.ClientID + "_focus");
            this.Update();
        }


        public string KeyEnterName { get; set; }

        public Action<_IInputText> TextEnterChanged { get; set; }
    }
}