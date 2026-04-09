using ConfigGlobal.Interface;
using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _UControls
{
    public delegate void DropDownValueChanged(dynamic _value);
    public delegate void CheckBoxValueChanged(dynamic _value);

    public interface _IFieldImportant : IResourceInput
    {
        bool IsPrimary { get; set; }
        bool IsKey { get; set; }
        string LabelText { get; set; }
        string ValidateMessage { get; set; }
        short ControlIndex { get; set; }
    }

    public interface _IInputControl : _IFieldImportant
    {
        void SetEnableFilter(Prototype.Providers.FilterAt filter);
        void SetDisableFilter(Prototype.Providers.FilterAt filter);

        IFilter GetFilter();
        bool IsFilter { get; }
        bool Filterable { get; set; }
        bool FixFilter { get; set; }
        void ClearFilter();
        FilterAt DefaultFilter { get; set; }

        string ValidateGroup { get; set; }

        string DataFieldShowValue { get; set; }
        string DataFieldTempValue { get; set; }
        bool IsStaticValue { get; set; }

        bool Enabled { get; set; }
        bool Readonly { get; set; }
        int ControlWidth { get; set; }
        int MaxLength { get; set; }
        InputType InputType { get; }

        void SetObjectValue(object _value);
        object GetObjectValue();

        void Update();
        void Clear();
        void Focus();

        bool ClearByIControl { get; set; }
        string ControlGroup { get; set; }
        int ControlSequence { get; set; }

        bool IsDefaultValue { get; set; }

        dynamic DefaultValue { get; set; }

        bool VisibleExt { get; set; }

        bool TextInLine { get; set; }

        string ControlId { get; set; }

        void AddAtributeBaseContent(string _key, string _value);
        string BaseContentCss { get; set; }
    }

    public interface _IInputDropDown : _IInputControl, _IInputPostBack
    {
        Func<IQueryable<Property>> MethodQueryProperty { get; set; }
        int LoadLazyLimit { get; set; }
        bool AllowSearchMultiValue { get; set; }
        int CountItem { get; }
        string GetText();
        string DisplayDefault { get; set; }
        void BindDataSource();
        void BindDataSource(string _displayDefault);
        void ClearItems();
        ComboType ComboType { get; set; }
        bool UseDefaultDisplay { get; set; }

    }

    public interface _IInputCheckBox : _IInputControl, _IInputPostBack
    {
        bool Checked { get; set; }
        CheckBoxType CheckBoxType { get; set; }
    }

    public interface _IInputPostBack
    {
        bool AutoPostBack { get; set; }
        Action<dynamic> PostValueChanged { get; set; }
        void ValueChange();
    }

    public interface _IInputDropDownViaStore
    {
        Func<string, int, IEnumerable<Property>> MethodQueryViaStore { get; set; }
    }


    public interface _IInputText : _IInputControl
    {
        string KeyEnterName { get; set; }

        Action<_IInputText> TextEnterChanged { get; set; }

        bool IsFocusOut { get; set; }
    }

    public interface _IInputTextDate : _IInputText
    {
        //void SetObjectValueTo(object _value);
        object GetObjectValueTo();

        string DateTimeFormat { get; set; }
        string DateFormat { get; set; }
        string TimeFormat { get; set; }
        DateCulture DateLanguage { get; set; }
        DateTimeType TextMode { get; set; }
    }



    public enum InputType
    {
        CheckBox,
        DropDownNormal,
        DropDownLazy,
        Hidden,
        Text,
        TextDate,
        TextInteger,
        TextNumber
    }

    public enum DropDownType
    {
        Normal,
        LazySearch,
        NormalStore,
        LazyStore
    }

    public enum ComboType
    {
        Guid,
        String,
        Integer,
        Double,
        Decimal,
        Boolean
    }

    public enum NumberType
    {
        Decimal,
        Double
    }

    public enum CheckBoxType
    {
        Boolean,
        String
    }

    public enum DateCulture
    {
        [AttributeEntry("th-TH")]
        Thai,
        [AttributeEntry("en-US")]
        English
    }

    public enum DateTimeType
    {
        DateTime,
        Date,
        Time
    }

    public enum TextType
    {
        Text,
        Email,
        Phone,
        Number,
        Password
    }

}
