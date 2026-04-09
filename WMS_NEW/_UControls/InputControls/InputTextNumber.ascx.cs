using Prototype.Providers;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _UControls
{
    public partial class InputTextNumber : EntityCustomLayout, _IInputText
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            baseControl.BaseFilterChangedValue += baseControl_BaseFilterChangedValue;


            if (string.IsNullOrEmpty(KeyEnterName) && TextEnterChanged != null)
                KeyEnterName = "_ENTER_CHANGE";


            if (!Page.IsPostBack)
            {
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.Contains);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.Between);
                baseControl.SetDisableFilter(FilterAt.Empty);

                // txtValue.Attributes.Add("onkeypress", "return onkeyNumber(event);");


                if (this.IsDefaultValue)
                {
                    //== Old Version
                    //var lengthDegit = this.NumberDegit == 0 ? 1 : this.NumberDegit;
                    var lengthDegit = Extensions.Digit_INT;

                    if (lengthDegit != 0)
                    {
                        txtValue.Attributes.Add("onblur", "defaultNumber('" + txtValue.ClientID + "',this.value," + lengthDegit + ");");
                    }
                }

                if (this.MaxLength > 0)
                    txtValue.MaxLength = this.MaxLength;

                if (this.ControlWidth > 0)
                    txtValue.Width = this.ControlWidth;

                if (this.ControlIndex > 0)
                    txtValue.TabIndex = this.ControlIndex;

                if (this.Enabled)
                {
                    txtValue.Enabled = !this.Readonly;
                }
                else
                {
                    this.IsPrimary = false;
                }

                txtValue.Enabled = this.Enabled;

                this.IsPrimary = this.IsPrimary;

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

                //if (this.VisibleExt)
                //{

                //}

                //panelValue.Attributes.Add("style", this.VisibleExt ? "display:block" : "display:none;");
                txtValue.Attributes.Add("placeholder", "Enter number");


                ResourceGroup = ResourceGroup;
                ResourceName = ResourceName;
                ResourceValue = ResourceValue;

                this.TextInLine = this.TextInLine;


                if (baseControl.VisibleBaseContent != VisibleExt)
                    baseControl.VisibleBaseContent = VisibleExt;


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
            //$('#currency').inputmask({

            string placeHolder = "0".PadLeft(NumberDegit, '0');
            placeHolder = "0." + placeHolder;

            var script_mask = string.Empty;
            //script_mask = @"$('.text-number').inputmask('decimal', {rightAlign: false, allowMinus: false, digits: " + NumberDegit.ToString() + " });";
            //script_mask = @"$('#" + txtValue.ClientID + "').inputmask({regex: '^(\\d+(\\,\\d{3})*(\\.\\d{1,"+ NumberDegit.ToString() + "})?)?$'}); ";
            script_mask = @"$('#" + txtValue.ClientID + "').inputmask('currency', {prefix:'',rightAlign: false, allowMinus: false, digits: " + NumberDegit.ToString() + " });";
            //script_mask = @"$('#" + txtValue.ClientID + "').inputmask('decimal', {'alias':'decimal', 'groupSeparator': ',','autoGroup':true,'digitsOptional': false,'placeholder': '" + placeHolder + "' rightAlign: false, allowMinus: false, digits: " + NumberDegit.ToString() + " });";

            Page.ScriptPageRegister("$(document).ready(function () { " + script_mask + " });", txtValue.ClientID + "_Text_InputMask_Number");


            if (Page.IsPostBack)
            {
                var eventControl = Request.Params.Get("__EVENTTARGET");
                var eventArgument = Request.Params.Get("__EVENTARGUMENT");

                if (eventControl == txtValue.ClientID && eventArgument == txtValue.ClientID + KeyEnterName)
                {
                    if (TextEnterChanged != null)
                        TextEnterChanged(this);
                }
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


                if (txtValue != null)
                {
                    reqValidate.ControlToValidate = txtValue.ID;
                    reqValidate.ValidationGroup = this.ValidateGroup;

                    baseControl.LabelText.IsPrimary = value;

                    if (value && this.Enabled)
                    {
                        txtValue.CssClass = txtValue.CssClass + " primary";
                        //baseControl.LabelText.IsPrimary = true;
                        reqValidate.Visible = true;
                    }
                    else
                    {
                        txtValue.CssClass = txtValue.CssClass.Replace(" primary", "");
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
        //                // panelValue.Visible = value;
        //                panelValue.Attributes.Add("style", value ? "display:block" : "display:none;");
        //            }
        //        }
        //        catch { }
        //    }
        //}
        #endregion


        #region Properties Custom By Control

        public int NumberDegit
        {
            get
            {
                if (ViewState["NumberDegit"] == null)
                {
                    ViewState["NumberDegit"] = Extensions.Digit_INT;
                }

                return (int)ViewState["NumberDegit"];
            }
            set
            {
                ViewState["NumberDegit"] = value;
            }
        }

        public NumberType NumberType
        {
            get
            {
                if (ViewState["NumberType"] == null)
                    ViewState["NumberType"] = NumberType.Double;

                return (NumberType)ViewState["NumberType"];
            }
            set
            {
                ViewState["NumberType"] = value;
            }
        }

        private dynamic GetValueByControl(TextBox _txt)
        {
            switch (NumberType)
            {
                case NumberType.Double:
                    if (!_txt.Text.Trim().IsEmpty())
                        return Convert.ToDouble(_txt.Text.Trim());
                    break;

                case NumberType.Decimal:
                    if (!_txt.Text.Trim().IsEmpty())
                        return Convert.ToDecimal(_txt.Text.Trim());
                    break;
            }

            return null;
        }

        public dynamic GetValue()
        {
            return GetValueByControl(txtValue);
        }

        public void SetValue(dynamic value)
        {
            if (value != null)
            {
                //var lengthDegit = this.NumberDegit == 0 ? 1 : this.NumberDegit;
                var lengthDegit = this.NumberDegit == 0 ? Extensions.Digit_INT : this.NumberDegit;

                try
                {
                    var values = (value.ToString() as string).Split('.');
                    if (values.Count() > 1 && this.NumberDegit == 0)
                    {
                        lengthDegit = values[1].Length;
                    }
                }
                catch { }

                var degitDefault = "#,##0.";
                for (var i = 1; i <= lengthDegit; i++)
                    degitDefault += "0";

                txtValue.Text = value.ToString(degitDefault);
            }
            else
            {
                txtValue.Text = string.Empty;
            }
        }

        public int MaxLength
        {
            get
            {
                if (ViewState["MaxLength"] == null)
                    ViewState["MaxLength"] = 0;

                return (int)ViewState["MaxLength"];
            }
            set
            {
                ViewState["MaxLength"] = value;

                try
                {
                    if (value > 0)
                        txtValue.MaxLength = value;
                }
                catch { }
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
        public InputType InputType
        {
            get { return _UControls.InputType.TextNumber; }
        }

        #endregion


        #region Properties Implement Difference

        public Prototype.Providers.IFilter GetFilter()
        {
            baseControl.Filter.DataPropertyName = DataFieldValue;
            baseControl.Filter.Value = GetValue();
            return baseControl.Filter;
        }

        public int ControlLength
        {
            get
            {
                if (ViewState["MaxLength"] == null)
                    ViewState["MaxLength"] = 0;

                return (int)ViewState["MaxLength"];
            }
            set
            {
                ViewState["MaxLength"] = value;

                try
                {
                    if (value > 0)
                        txtValue.MaxLength = value;
                }
                catch { }
            }
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
                        txtValue.Width = value;
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
                        txtValue.TabIndex = value;
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
                        txtValue.Enabled = !value;
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
                        txtValue.Enabled = value;

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
            return GetValue();
        }

        public void Clear()
        {
            if (this.IsStaticValue) return;

            if (DefaultValue != null)
                SetObjectValue(DefaultValue);
            else
                SetValue(null);
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