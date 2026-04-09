using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _UControls
{
    public delegate void TextBoxValueChanged(string _value);

    public partial class InputTextBox : EntityCustomLayout, _IInputText
    {
        public event TextBoxValueChanged TextValueChanged;

        protected void Page_Load(object sender, EventArgs e)
        {
            baseControl.BaseFilterChangedValue += baseControl_BaseFilterChangedValue;
            txtValue.TextChanged += TxtValue_TextChanged;


            if (string.IsNullOrEmpty(KeyEnterName) && TextEnterChanged != null)
                KeyEnterName = "_ENTER_CHANGE";


            if (!Page.IsPostBack)
            {
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.LessThan);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.LessThanEqual);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.MoreThan);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.MoreThanEqual);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.Between);

                txtValue.AutoPostBack = this.AutoPostBack;

                reqValidate.ControlToValidate = txtValue.ID;
                reqValidate.ValidationGroup = this.ValidateGroup;
                reqValidate.Visible = IsPrimary;

                if (!this.FixFilter)
                {
                    this.DefaultFilter = Prototype.Providers.FilterAt.Contains;
                }

                if (this.IsMultiLine)
                    txtValue.TextMode = TextBoxMode.MultiLine;

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
                //else
                //{
                //    this.IsPrimary = false;
                //}

                this.IsPrimary = this.IsPrimary;

                txtValue.Enabled = this.Enabled;

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


                //baseControl.Visible = this.VisibleExt;
                txtValue.Attributes.Add("placeholder", "Enter text");

                this.TextInLine = this.TextInLine;

                ResourceGroup = ResourceGroup;
                ResourceName = ResourceName;
                ResourceValue = ResourceValue;

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

        protected void Page_PreRender(object sender, EventArgs e)
        {
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


        private void TxtValue_TextChanged(object sender, EventArgs e)
        {
            if (this.AutoPostBack && !string.IsNullOrEmpty(txtValue.Text))
            {
                if (TextValueChanged != null)
                {
                    //string mes = this.GetValue();
                    TextValueChanged(txtValue.Text);
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
                        txtValue.CssClass = txtValue.CssClass.Contains("primary") ? txtValue.CssClass : txtValue.CssClass + " primary";
                        reqValidate.ControlToValidate = txtValue.ID;
                        reqValidate.ValidationGroup = this.ValidateGroup;
                        reqValidate.Visible = true;

                        //baseControl.LabelText.IsPrimary = true;
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

        #endregion


        #region Properties Custom By Control

        private string GetValueByControl(TextBox _txt)
        {
            if (_txt.Text.Trim() != string.Empty)
                return _txt.Text.Trim();
            else
                return null;
        }

        public string GetValue()
        {
            return GetValueByControl(txtValue);
        }

        public void SetValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                //อักขระพิเศษห้ามมี < >
                value = value.Replace("<", "[");
                value = value.Replace(">", "]");

            }
            txtValue.Text = value;
        }

        public bool IsMultiLine
        {
            get
            {
                if (ViewState["IsMultiLine"] == null)
                    ViewState["IsMultiLine"] = false;

                return (bool)ViewState["IsMultiLine"];
            }
            set
            {
                ViewState["IsMultiLine"] = value;
            }
        }

        public bool OrNullValue
        {
            get
            {
                if (ViewState["OrNull"] == null)
                    ViewState["OrNull"] = false;

                return (bool)ViewState["OrNull"];
            }
            set
            {
                ViewState["OrNull"] = value;
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

                if (txtValue != null && value > 0)
                    txtValue.MaxLength = value;
            }
        }
        public InputType InputType
        {
            get { return _UControls.InputType.Text; }
        }

        public TextType TextMode
        {
            get
            {
                if (ViewState["TextMode"] == null)
                    ViewState["TextMode"] = TextType.Text;

                return (TextType)ViewState["TextMode"];
            }
            set
            {
                ViewState["TextMode"] = value;
                //if (txtValue != null && Page.IsPostBack)
                //{
                //    switch (value)
                //    {
                //        case TextType.Text:
                //            reqValidate.ControlToValidate = txtValue.ID;
                //            reqValidate.ValidationGroup = this.ValidateGroup;

                //            reqValidateEmail.Visible = false;
                //            reqValidatePhone.Visible = false;
                //            reqValidateNumber.Visible = false;
                //            reqValidate.Visible = true;
                //            break;
                //        case TextType.Email:
                //            reqValidateEmail.ControlToValidate = txtValue.ID;
                //            reqValidateEmail.ValidationGroup = this.ValidateGroup;
                //            reqValidateEmail.Visible = true;
                //            reqValidatePhone.Visible = false;
                //            reqValidateNumber.Visible = false;
                //            break;
                //        case TextType.Phone:
                //            reqValidatePhone.ControlToValidate = txtValue.ID;
                //            reqValidatePhone.ValidationGroup = this.ValidateGroup;
                //            reqValidatePhone.Visible = true;
                //            reqValidateEmail.Visible = false;
                //            reqValidateNumber.Visible = false;
                //            break;
                //        case TextType.Number:
                //            reqValidateNumber.ControlToValidate = txtValue.ID;
                //            reqValidateNumber.ValidationGroup = this.ValidateGroup;
                //            reqValidatePhone.Visible = false;
                //            reqValidateEmail.Visible = false;
                //            reqValidateNumber.Visible = true;
                //            break;
                //        case TextType.Password:
                //            txtValue.Attributes.Add("type", "password");
                //            break;
                //        default:
                //            reqValidate.ControlToValidate = txtValue.ID;
                //            reqValidate.ValidationGroup = this.ValidateGroup;

                //            reqValidateEmail.Visible = false;
                //            reqValidatePhone.Visible = false;
                //            reqValidateNumber.Visible = false;
                //            reqValidate.Visible = false;
                //            break;
                //    }

                //}
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
        public string ClientIDExt
        {
            get
            {
                return txtValue.ClientID;
            }
        }

        #endregion


        #region Properties Implement Difference

        public Prototype.Providers.IFilter GetFilter()
        {
            baseControl.Filter.DataPropertyName = DataFieldValue;
            baseControl.Filter.Value = GetValue();

            return baseControl.Filter;
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


                if (txtValue != null && value > 0)
                    txtValue.Width = value;
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

                if (txtValue != null && value > 0)
                    txtValue.TabIndex = value;
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


                if (txtValue != null)
                    txtValue.Enabled = !value;

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

                    this.Readonly = !value;

                    if (txtValue != null)
                    {
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
            SetValue(Convert.ToString(_value));
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
                SetValue(string.Empty);
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

        public string LabelDisplay
        {
            get
            {
                return baseControl.LabelText.InnerText;
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