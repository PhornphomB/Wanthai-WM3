using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _UControls
{

    public partial class InputCheckBox : EntityCustomLayout, _IInputCheckBox
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            baseControl.BaseFilterChangedValue += baseControl_BaseFilterChangedValue;

            if (!Page.IsPostBack)
            {
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.Contains);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.LessThan);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.LessThanEqual);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.MoreThan);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.MoreThanEqual);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.Between);
                baseControl.SetDisableFilter(Prototype.Providers.FilterAt.Empty);

                if (this.IsPrimary)
                    chkValue.CssClass = "primary";

                if (this.ControlWidth > 0)
                    chkValue.Width = this.ControlWidth;

                if (this.ControlIndex > 0)
                    chkValue.TabIndex = this.ControlIndex;

                chkValue.Enabled = this.Enabled;

                if (this.Enabled)
                    chkValue.Enabled = !this.Readonly;

                if (!string.IsNullOrEmpty(this.Text))
                    chkValue.Text = this.Text;

                switch (this.CheckBoxType)
                {
                    case CheckBoxType.Boolean:
                        SetValueByParameter(this.Checked);
                        break;
                    case CheckBoxType.String:
                        SetValueByParameter(this.Checked ? "YES" : "NO");
                        break;
                }

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

                if (this.AutoPostBack)
                {
                    if (this.Enabled)
                    {
                        labelSwitch.Attributes.Add("onclick", "doPostBackAsync('" + chkValue.ClientID + "', '_TRIGGER_CHECKED');");
                        labelHandle.Attributes.Add("onclick", "doPostBackAsync('" + chkValue.ClientID + "', '_TRIGGER_CHECKED');");
                    }
                    else
                    {
                        labelSwitch.Attributes.Remove("onclick");
                        labelHandle.Attributes.Remove("onclick");
                    }
                }

                this.TextInLine = this.TextInLine;

                var _style = "cursor: pointer;";
                if (!chkValue.Enabled)
                    _style = "cursor: no-drop;";

                labelSwitch.Attributes.Add("style", _style);
                labelHandle.Attributes.Add("style", _style);

                if (baseControl.VisibleBaseContent != VisibleExt)
                    baseControl.VisibleBaseContent = VisibleExt;
            }
            else
            {
                if (LastChangedValue != null)
                {
                    SetValue(LastChangedValue);
                    LastChangedValue = null;
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e) //Page Stat working after Load Control events
        {
            chkValue.InputAttributes["class"] = "switch-input";
            //var clientScript = @" $('document').ready(function () {";
            //clientScript += "checkbox('" + chkValue.ClientID + "')";
            //clientScript += "});";

            //Page.ScriptPageRegister(clientScript, chkValue.ClientID + "_ScriptCheckBox");
            if (!Enabled)
            {
                if (chkValue.Checked)
                {
                    var str = "background: #28B463 !important;border-color: #28B463;cursor: no-drop;padding-top: 4px; padding-left: 5px;color: white;cursor: no-drop;";
                    labelSwitch.Attributes.Add("style", str);
                    labelHandle.Attributes.Add("style", "left: 30px;cursor: no-drop;");
                    labelSwitch.InnerText = "YES"; // ตั้งค่าข้อความเริ่มต้น
                }
                else
                {
                    labelSwitch.Attributes.Add("style", "cursor: no-drop;");
                    labelHandle.Attributes.Add("style", "cursor: no-drop;");
                    labelSwitch.InnerText = ""; // ตั้งค่าข้อความเริ่มต้น
                }
            }
            else
            {
                labelSwitch.Attributes.Remove("style");
                labelHandle.Attributes.Remove("style");

                // คืนค่า InnerText เป็นค่าเดิม
                labelSwitch.InnerText = "";
            }

            if (Page.IsPostBack)
            {
                var eventControl = Request.Params.Get("__EVENTTARGET");
                var eventArgument = Request.Params.Get("__EVENTARGUMENT");

                if (eventControl == chkValue.ClientID && eventArgument == "_TRIGGER_CHECKED")
                {
                    chkValue.Checked = !chkValue.Checked;
                    ChkValue_CheckedChanged(sender, e);
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

                if (Page.IsPostBack)
                {
                    try
                    {
                        if (value)
                        {
                            chkValue.CssClass = "primary";
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

        #endregion


        #region Properties Custom By Control

        public void ValueChange()
        {
            ChkValue_CheckedChanged(null, EventArgs.Empty);
        }

        private void ChkValue_CheckedChanged(object sender, EventArgs e)
        {
            if (PostValueChanged != null)
            {
                var value = GetValue();
                LastChangedValue = value;
                PostValueChanged(value);
            }
        }

        public Action<dynamic> PostValueChanged { get; set; }

        private dynamic LastChangedValue
        {
            get
            {
                return (dynamic)ViewState["LastChangedValue"];
            }
            set
            {
                ViewState["LastChangedValue"] = value;
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

        public CheckBoxType CheckBoxType
        {
            get
            {
                if (ViewState["CheckBoxType"] == null)
                    ViewState["CheckBoxType"] = CheckBoxType.Boolean;

                return (CheckBoxType)ViewState["CheckBoxType"];
            }
            set
            {
                ViewState["CheckBoxType"] = value;
            }
        }

        public string Text
        {
            get
            {
                if (ViewState["Text"] == null)
                    ViewState["Text"] = string.Empty;

                return (string)ViewState["Text"];
            }
            set
            {
                ViewState["Text"] = value;

                try
                {
                    if (IsPostBack)
                        chkValue.Text = value;
                }
                catch { }
            }
        }

        public bool Checked
        {
            get
            {
                try
                {
                    if (ViewState["Checked"] == null)
                        ViewState["Checked"] = false;

                    return (bool)ViewState["Checked"];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                try
                {
                    ViewState["Checked"] = value;

                    if (chkValue != null)
                    {
                        chkValue.Checked = value;
                        if (this.AutoPostBack)
                        {
                            var val = GetValue();
                            LastChangedValue = val;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
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

        private dynamic GetValueByControl(CheckBox _chk)
        {
            switch (CheckBoxType)
            {
                case CheckBoxType.Boolean:
                    return _chk.Checked;
                case CheckBoxType.String:
                    if (_chk.Checked)
                        return "YES";
                    else
                        return "NO";
            }

            if (this.IsDefaultValue)
            {
                if (CheckBoxType == _UControls.CheckBoxType.String)
                    return "NO";
                else
                    return false;
            }
            else
            {
                return null;
            }
        }
        private void SetValueByParameter(dynamic _value)
        {
            if (_value == null)
            {
                chkValue.Checked = false;
            }
            else
            {
                switch (CheckBoxType)
                {
                    case CheckBoxType.Boolean:
                        chkValue.Checked = _value;
                        break;
                    case CheckBoxType.String:
                        if (_value.ToUpper() == "YES")
                            chkValue.Checked = true;
                        else if (_value.ToUpper() == "NO")
                            chkValue.Checked = false;
                        break;
                }
            }

            baseControl.UpdateContent();
        }

        public dynamic GetValue()
        {
            return GetValueByControl(chkValue);
        }
        public void SetValue(dynamic value)
        {
            SetValueByParameter(value);
            //chkValue_CheckedChanged(null, EventArgs.Empty);
        }

        //protected void chkValue_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.AutoPostBack && PostValueChanged != null)
        //        PostValueChanged(this.GetValue());
        //}

        #endregion


        #region Properties Implement Difference

        public Prototype.Providers.IFilter GetFilter()
        {
            baseControl.Filter.DataPropertyName = DataFieldValue;
            baseControl.Filter.Value = GetValue();
            return baseControl.Filter;
        }

        public InputType InputType
        {
            get { return _UControls.InputType.CheckBox; }
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
                        chkValue.Width = value;
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
                        chkValue.TabIndex = value;
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
                    if (IsPostBack && chkValue != null)
                    {
                        chkValue.Enabled = !value;

                        var _style = "cursor: pointer;";
                        if (value)
                            _style = "cursor: no-drop;";

                        labelSwitch.Attributes.Add("style", _style);
                        labelHandle.Attributes.Add("style", _style);

                        if (!value)
                        {
                            labelSwitch.Attributes.Add("onclick", "doPostBackAsync('" + chkValue.ClientID + "', '_TRIGGER_CHECKED');");
                            labelHandle.Attributes.Add("onclick", "doPostBackAsync('" + chkValue.ClientID + "', '_TRIGGER_CHECKED');");
                        }
                        else
                        {
                            labelSwitch.Attributes.Remove("onclick");
                            labelHandle.Attributes.Remove("onclick");
                        }
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
                    if (IsPostBack && chkValue != null)
                    {
                        chkValue.Enabled = value;

                        var _style = "cursor: pointer;";
                        if (!value)
                            _style = "cursor: no-drop;";

                        labelSwitch.Attributes.Add("style", _style);
                        labelHandle.Attributes.Add("style", _style);

                        if (value)
                        {
                            labelSwitch.Attributes.Add("onclick", "doPostBackAsync('" + chkValue.ClientID + "', '_TRIGGER_CHECKED');");
                            labelHandle.Attributes.Add("onclick", "doPostBackAsync('" + chkValue.ClientID + "', '_TRIGGER_CHECKED');");
                        }
                        else
                        {
                            labelSwitch.Attributes.Remove("onclick");
                            labelHandle.Attributes.Remove("onclick");
                        }
                    }
                }
                catch { }
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
            LastChangedValue = null;

            if (this.IsStaticValue) return;

            if (DefaultValue != null)
                SetObjectValue(DefaultValue);
            else
                switch (CheckBoxType)
                {
                    case CheckBoxType.Boolean:
                        SetValue(false);
                        break;
                    case CheckBoxType.String:
                        SetValue("NO");
                        break;
                }


            ChkValue_CheckedChanged(null, EventArgs.Empty);
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

        #endregion



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
    }
}