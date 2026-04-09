using System;
using System.Web.UI.WebControls;

namespace _UControls
{
    public partial class InputHidden : EntityCustomLayout, _IInputControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public void SetEnableFilter(Prototype.Providers.FilterAt filter)
        {

        }
        public void SetDisableFilter(Prototype.Providers.FilterAt filter)
        {

        }

        public void Update()
        {

        }

        private dynamic KeyValue
        {
            get
            {
                return (dynamic)ViewState["KeyValue"];
            }
            set
            {
                ViewState["KeyValue"] = value;
            }
        }

        public dynamic GetValue()
        {
            return KeyValue;
        }
        public void SetValue(dynamic value)
        {
            KeyValue = value;
        }

        public Prototype.Providers.IFilter GetFilter()
        {
            return null;
        }
        public bool IsFilter { get; set; }
        public bool Filterable { get; set; }
        public bool FixFilter { get; set; }

        public void ClearFilter()
        {

        }
        public Prototype.Providers.FilterAt DefaultFilter { get; set; }

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
            }
        }

        public short ControlIndex { get; set; }

        public InputType InputType
        {
            get { return _UControls.InputType.Hidden; }
        }
        public int MaxLength
        {
            get
            {
                return 0;
            }
            set { }
        }
        public int ControlWidth { get; set; }

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

        public string LabelText
        {
            get
            {
                if (ViewState["LabelText"] == null)
                    ViewState["LabelText"] = string.Empty;

                return (string)ViewState["LabelText"];
            }
            set
            {
                ViewState["LabelText"] = value;
            }
        }

        public bool TextInLine { get; set; }

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

        }

        public string BaseContentCss { get; set; }

        public bool VisibleExt { get; set; }
        public string ResourceGroup { get; set; }
        public string ResourceName { get; set; }
        public string ResourceValue { get; set; }
    }
}