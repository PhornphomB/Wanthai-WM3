using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Configuration;

using Prototype.Providers;

namespace _UControls
{

    #region Optional Propertise Grid

    public enum ControlType
    {
        Label = 0,
        Text = 1,
        CommandButton = 2,
        CommandLinkButton = 3
    }

    public enum FieldValueType
    {
        [AttributeEntry("Text")]
        Text = 0,
        [AttributeEntry("Integer")]
        Integer = 1,
        [AttributeEntry("Number")]
        Number = 2,
        [AttributeEntry("DateTime")]
        DateTime = 3,
        [AttributeEntry("Date")]
        Date = 4,
        [AttributeEntry("Time")]
        Time = 5,
        [AttributeEntry("Boolean")]
        Boolean = 6,
        [AttributeEntry("Guid")]
        Guid = 7
    }

    [Serializable()]
    public class GridFilters
    {
        public GridFilters() { }

        public GridFilters(string _labelText, string _dataField, FieldValueType _formatType, string _formatString,
            bool _allowFilter, bool _fixFilter, bool _isShowFilter, FilterAt _defaultFilter, bool _useDropDown,
            DropDownType _dropDownFilterType, int? _filterIndex, int? _filterwidth, bool _isPrimary)
        {
            LabelText = _labelText;
            DataField = _dataField;
            FormatType = _formatType;
            FormatString = _formatString;
            DefaultFilter = _defaultFilter;
            AllowFilter = _allowFilter;
            FixFilter = _fixFilter;
            IsShowFilter = _isShowFilter;
            DropDownFilterType = _dropDownFilterType;
            UseDropDown = _useDropDown;
            FilterIndex = _filterIndex;
            FilterWidth = _filterwidth;

            IsPrimary = _isPrimary;
        }

        public string LabelText { get; set; }
        public string DataField { get; set; }
        public FieldValueType FormatType { get; set; }
        public string FormatString { get; set; }
        public bool IsFilterable { get; set; }
        public FilterAt DefaultFilter { get; set; }
        public bool AllowFilter { get; set; }
        public bool FixFilter { get; set; }
        public bool IsShowFilter { get; set; }
        public int? FilterIndex { get; set; }
        public int? FilterWidth { get; set; }
        public bool UseDropDown { get; set; }
        public DropDownType DropDownFilterType { get; set; }
        public string ContainerID { get; set; }

        public bool IsPrimary { get; set; }

    }

    [Serializable()]
    public class GridIsSort
    {
        public GridIsSort(string _dataField, int _index)
        {
            DataField = _dataField;
            Direction = "ASC";
            Index = _index;
        }

        public string DataField { get; set; }
        public string Direction { get; set; }
        public int Index { get; set; }
    }

    #endregion

    public class GridCustomField : DataControlField
    {
        public GridCustomField()
        {
            base.ItemStyle.VerticalAlign = VerticalAlign.Middle;
        }


        public bool AllowFilter { get; set; }
        public string DataFieldFilter
        {
            get
            {
                if (base.ViewState["DataFieldFilter"] == null)
                    base.ViewState["DataFieldFilter"] = string.Empty;

                return (string)base.ViewState["DataFieldFilter"];
            }
            set
            {
                base.ViewState["DataFieldFilter"] = value;
            }
        }

        public bool AllowSort { get; set; }

        public bool IsConfirm { get; set; }

        public string IsConfirmMessage { get; set; }


        #region Public Properties

        public ControlType ControlType { get; set; }

        //public string ToolTipText
        //{
        //    get
        //    {
        //        object value = base.ViewState["ToolTipText"];

        //        if (value == null)
        //            value = string.Empty;

        //        return value.ToString();
        //    }
        //    set
        //    {
        //        base.ViewState["ToolTipText"] = value;
        //    }
        //}

        public string CommandText
        {
            get
            {
                object value = base.ViewState["CommandText"];

                if (value == null)
                    value = string.Empty;

                return value.ToString();
            }
            set
            {
                base.ViewState["CommandText"] = value;
            }
        }
        public string CommandName
        {
            get
            {
                object value = base.ViewState["CommandName"];

                if (value == null)
                    value = string.Empty;

                return value.ToString();
            }
            set
            {
                base.ViewState["CommandName"] = value;
            }
        }
        public string CommandKeyField
        {
            get
            {
                object value = base.ViewState["CommandKeyField"];

                if (value == null)
                    value = string.Empty;

                return value.ToString();
            }
            set
            {
                base.ViewState["CommandKeyField"] = value;
            }
        }

        public string DataField
        {
            get
            {
                object value = base.ViewState["DataField"];
                if (value == null)
                    value = string.Empty;

                return value.ToString();
            }
            set
            {
                base.ViewState["DataField"] = value;
                this.OnFieldChanged();
            }
        }

        public FieldValueType FormatType
        {
            get
            {
                object value = base.ViewState["FormatType"];
                if (value != null)
                {
                    return (FieldValueType)value;
                }
                else
                {
                    return FieldValueType.Text;
                }
            }
            set
            {
                base.ViewState["FormatType"] = value;
            }
        }

        public string FormatString
        {
            get
            {
                if (ViewState["FormatString"] == null)
                    return null;
                else
                    return ViewState["FormatString"].ToString();
            }
            set
            {
                base.ViewState["FormatString"] = value;
            }
        }

        public HorizontalAlign FieldTextAlign
        {
            get
            {
                object value = base.ViewState["FieldTextAlign"];
                if (value != null)
                {
                    return (HorizontalAlign)value;
                }
                else
                {
                    return HorizontalAlign.Left;
                }
            }
            set
            {
                base.ViewState["FieldTextAlign"] = value;
                base.ItemStyle.HorizontalAlign = value;
            }
        }

        public int Width
        {
            get
            {
                object value = base.ViewState["ColumnWidth"];
                if (value != null)
                {
                    return Convert.ToInt32(value.ToString());
                }
                else
                {
                    return 101;
                }
            }
            set
            {
                base.ViewState["ColumnWidth"] = value;
            }
        }

        public bool IsLastColumn { get; set; }

        public bool InputTextAutoPostBack { get; set; }

        #endregion


        #region Overriden Life Cycle Methods

        /// <summary>
        /// Overriding the CreateField method is mandatory if you derive from the DataControlField.
        /// </summary>
        /// <returns></returns>
        protected override DataControlField CreateField()
        {
            return new BoundField();
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            //Call the base method.
            base.InitializeCell(cell, cellType, rowState, rowIndex);

            switch (cellType)
            {
                case DataControlCellType.DataCell:
                    this.InitializeDataCell(cell, rowState, rowIndex);
                    break;
                case DataControlCellType.Footer:
                    this.InitializeFooterCell(cell, rowState);
                    break;
                case DataControlCellType.Header:
                    this.InitializeHeaderCell(cell, rowState);
                    break;
            }
        }

        #endregion


        #region Custom Protected Methods

        public event System.EventHandler TextInputChanged;

        protected void txtText_TextChanged(object sender, EventArgs e)
        {
            if (TextInputChanged != null)
                TextInputChanged(sender, e);
        }

        protected void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState, int rowIndex)
        {

            if ((this.DataField == null) || (this.DataField == string.Empty))
            {
                this.Visible = false;
                return;
            }

            switch (this.ControlType)
            {
                case _UControls.ControlType.Label:

                    var style = "word-wrap: normal; word-break: break-all; white-space: pre-warp;";

                    switch (this.FormatType)
                    {
                        case FieldValueType.Text:
                            FieldTextAlign = HorizontalAlign.Left;
                            break;
                        case FieldValueType.Integer:
                            FieldTextAlign = HorizontalAlign.Right;

                            if (IsLastColumn)
                                style += "padding-right:10px;";
                            break;
                        case FieldValueType.Number:
                            FieldTextAlign = HorizontalAlign.Right;

                            if (IsLastColumn)
                                style += "padding-right:10px;";
                            break;
                        default:
                            FieldTextAlign = HorizontalAlign.Center;
                            break;
                    }

                    Label lblText = new Label();
                    lblText.Attributes.Add("style", style);
                    lblText.DataBinding += new System.EventHandler(lblText_DataBinding);
                    cell.Controls.Add(lblText);

                    break;

                case _UControls.ControlType.Text:

                    TextBox txtText = new TextBox();
                    //txtText.ID = "txtText_" + rowIndex.ToString() + "___" + this.DataField;
                    txtText.ID = this.DataField;
                    txtText.DataBinding += new System.EventHandler(txtText_DataBinding);
                    txtText.Font.Size = new FontUnit(Convert.ToInt32(ConfigurationManager.AppSettings["GridFontSize"]), UnitType.Pixel);
                    txtText.Font.Bold = true;
                    txtText.CssClass = "form-control";
                    //txtText.BackColor = Color.LightGreen;
                    txtText.Attributes.Add("autocomplete", "off"); // disable text input history value

                    if (InputTextAutoPostBack)
                    {
                        txtText.AutoPostBack = true;
                        txtText.TextChanged += new System.EventHandler(txtText_TextChanged);
                    }

                    if (this.Width != 101)
                    {
                        txtText.Attributes.Add("style", "margin-bottom:0px; width:" + this.Width.ToString() + "px;");
                    }
                    else
                    {
                        txtText.Attributes.Add("style", "margin-bottom:0px;");
                    }

                    switch (this.FormatType)
                    {
                        case FieldValueType.Integer:
                            txtText.Attributes.Add("onkeypress", "return onkeyInteger(event);");
                            break;
                        case FieldValueType.Number:
                            txtText.Attributes.Add("onkeypress", "return onkeyNumber(event);");
                            break;
                    }

                    if (this.FormatType != FieldValueType.Date)
                    {
                        cell.Controls.Add(txtText);
                    }
                    else
                    {
                        txtText.Attributes.Add("data-format", FieldsStatic.DateFormat);
                        txtText.Attributes.Add("style", "margin-bottom:0px; width:75px;text-align: center;");

                        var div = new HtmlGenericControl("div");
                        div.Attributes.Add("class", "datepicker_custom input-append grid-input-date");
                        div.Attributes.Add("style", "padding-top:8px;");

                        var span = new HtmlGenericControl("span");
                        span.Attributes.Add("class", "add-on");
                        span.Attributes.Add("style", "cursor: pointer;");

                        var ic = new HtmlGenericControl("i");
                        ic.Attributes.Add("data-date-icon", "icon-calendar");

                        span.Controls.Add(ic);

                        div.Controls.Add(txtText);
                        div.Controls.Add(span);

                        cell.Controls.Add(div);
                    }

                    break;

                case ControlType.CommandButton:

                    Button btAction = new Button();
                    btAction.ID = this.CommandName;
                    btAction.CommandName = this.CommandName;
                    btAction.Text = this.CommandText;
                    btAction.DataBinding += new System.EventHandler(btAction_DataBinding);
                    btAction.CssClass = "btn btn-sm btn-ingrid";

                    if (this.IsConfirm)
                    {
                        if(!string.IsNullOrEmpty(IsConfirmMessage))
                        {
                            btAction.OnClientClick = "if (!confirm('" + IsConfirmMessage + "')) return false;";
                        }
                        else
                        {
                            btAction.OnClientClick = "if (!confirm('Are you sure ?')) return false;";
                        }
                    }
                    //btAction.Font.Size = new FontUnit(Convert.ToInt32(ConfigurationManager.AppSettings["GridFontSize"]), UnitType.Pixel);

                    if (this.Width != 101)
                    {
                        btAction.Attributes.Add("style", "min-width:" + this.Width.ToString() + "px;");
                    }

                    cell.Controls.Add(btAction);

                    break;

                case ControlType.CommandLinkButton:

                    LinkButton linkAction = new LinkButton();
                    linkAction.ID = this.CommandName;
                    linkAction.CommandName = this.CommandName;
                    linkAction.Text = this.CommandText;
                    linkAction.DataBinding += new System.EventHandler(linkAction_DataBinding);
                    linkAction.CssClass = "label label-ingrid";
                    linkAction.OnClientClick = "if (!confirm('Are you sure ?')) return false;";
                    linkAction.Font.Size = new FontUnit(Convert.ToInt32(ConfigurationManager.AppSettings["GridFontSize"]), UnitType.Pixel);
                    linkAction.ForeColor = Color.White;

                    if (this.Width != 101)
                    {
                        linkAction.Attributes.Add("style", "min-width:" + this.Width.ToString() + "px;");
                    }

                    cell.Controls.Add(linkAction);

                    break;
            }

        }
        protected void InitializeHeaderCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            HtmlGenericControl div = new HtmlGenericControl("div");

            //if (!string.IsNullOrEmpty(this.ToolTipText))
            //{
            //    div.Attributes.Add("class", "grid-head-ctrl tipso_tip");
            //    div.Attributes.Add("title", this.ToolTipText);
            //}
            //else
            //{
            //}
            div.Attributes.Add("class", "grid-head-ctrl");

            var labField = new Label();
            labField.Attributes.Add("class", "grid-head-text");
            labField.Text = this.HeaderText;
            div.Controls.Add(labField);

            var linkFilter = new HtmlGenericControl();
            if (this.AllowFilter)
            {
                linkFilter.Attributes.Add("name", "link_filter_name_" + this.DataFieldFilter);
            }
            div.Controls.Add(linkFilter);

            var linkSort = new HtmlGenericControl();
            div.Controls.Add(linkSort);

            var hidField = new Label();
            hidField.Text = this.DataField;
            hidField.Attributes.Add("style", "display:none;");
            div.Controls.Add(hidField);

            var hidWidth = new Label();
            hidWidth.Attributes.Add("style", "display:none;");
            if (this.Width != 101)
            {
                labField.Width = this.Width;
            }
            else
            {
                using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
                {
                    SizeF size = graphics.MeasureString(labField.Text.Trim(), new Font(ConfigurationManager.AppSettings["GridFontStyle"], Convert.ToInt32(ConfigurationManager.AppSettings["GridFontSize"]), FontStyle.Bold, GraphicsUnit.Pixel));
                    labField.Width = Convert.ToInt32(size.Width + (size.Width < 120 ? 14 : 24));

                    var dataMaxLength = Convert.ToInt32(ConfigurationManager.AppSettings["GridDataMaxLength"]);
                    if (size.Width > dataMaxLength)
                        labField.Width = dataMaxLength;
                }
            }

            hidWidth.Text = labField.Width.Value.ToString();
            div.Controls.Add(hidWidth);

            cell.Controls.Add(div);
        }
        protected void InitializeFooterCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            CheckBox chkBox = new CheckBox();
            chkBox.Enabled = false;
            cell.Controls.Add(chkBox);
        }


        void lblText_DataBinding(object sender, EventArgs e)
        {
            // get a reference to the control that raised the event
            Label target = (Label)sender;
            Control container = target.NamingContainer;

            // get a reference to the row object
            object dataItem = DataBinder.GetDataItem(container);

            // get the row's value for the named data field only use Eval when it is neccessary
            // to access child object values, otherwise use GetPropertyValue. GetPropertyValue
            // is faster because it does not use reflection

            object dataFieldValue = null;

            if (this.DataField.Contains("."))
            {
                dataFieldValue = DataBinder.Eval(dataItem, this.DataField);
            }
            else
            {
                dataFieldValue = DataBinder.GetPropertyValue(dataItem, this.DataField);
            }

            // set the table cell's text. check for null values to prevent ToString errors
            if (dataFieldValue != null)
            {
                target.Text = GetValueWithFormat(dataFieldValue);

                if (this.Width != 101)
                {
                    target.Width = this.Width;
                }
                else
                {
                    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
                    {
                        SizeF size = graphics.MeasureString(target.Text.Trim(), new Font(ConfigurationManager.AppSettings["GridFontStyle"], Convert.ToInt32(ConfigurationManager.AppSettings["GridFontSize"]), FontStyle.Regular, GraphicsUnit.Pixel));
                        target.Width = Convert.ToInt32(size.Width + (size.Width < 120 ? 14 : 24));

                        var dataMaxLength = Convert.ToInt32(ConfigurationManager.AppSettings["GridDataMaxLength"]);
                        if (size.Width > dataMaxLength)
                            target.Width = dataMaxLength;
                    }
                }
            }
        }
        void txtText_DataBinding(object sender, EventArgs e)
        {
            // get a reference to the control that raised the event
            TextBox target = (TextBox)sender;
            Control container = target.NamingContainer;

            // get a reference to the row object
            object dataItem = DataBinder.GetDataItem(container);

            // get the row's value for the named data field only use Eval when it is neccessary
            // to access child object values, otherwise use GetPropertyValue. GetPropertyValue
            // is faster because it does not use reflection

            object dataFieldValue = null;

            if (this.DataField.Contains("."))
            {
                dataFieldValue = DataBinder.Eval(dataItem, this.DataField);
            }
            else
            {
                dataFieldValue = DataBinder.GetPropertyValue(dataItem, this.DataField);
            }

            // set the table cell's text. check for null values to prevent ToString errors
            if (dataFieldValue != null)
            {
                target.Text = GetValueWithFormat(dataFieldValue);

                using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
                {
                    SizeF size = graphics.MeasureString(target.Text.Trim(), new Font(ConfigurationManager.AppSettings["GridFontStyle"], Convert.ToInt32(ConfigurationManager.AppSettings["GridFontSize"]), FontStyle.Regular, GraphicsUnit.Pixel));
                    target.Width = Convert.ToInt32(size.Width);

                    var dataMaxLength = Convert.ToInt32(ConfigurationManager.AppSettings["GridDataMaxLength"]);
                    if (size.Width > dataMaxLength)
                        target.Width = dataMaxLength;

                }
            }
        }
        void btAction_DataBinding(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.CommandKeyField))
                return;

            Button target = (Button)sender;

            Control container = target.NamingContainer;
            object dataItem = DataBinder.GetDataItem(container);
            object keyFieldValue = null;

            if (this.DataField.Contains("."))
                keyFieldValue = DataBinder.Eval(dataItem, this.CommandKeyField);
            else
                keyFieldValue = DataBinder.GetPropertyValue(dataItem, this.CommandKeyField);

            if (keyFieldValue != null)
            {
                target.CommandArgument = keyFieldValue.ToString();
            }

            if (string.IsNullOrEmpty(target.Text))
            {
                object dataFieldValue = null;

                if (this.DataField.Contains("."))
                {
                    dataFieldValue = DataBinder.Eval(dataItem, this.DataField);
                }
                else
                {
                    dataFieldValue = DataBinder.GetPropertyValue(dataItem, this.DataField);
                }

                // set the table cell's text. check for null values to prevent ToString errors
                if (dataFieldValue != null)
                {
                    target.Text = GetValueWithFormat(dataFieldValue);
                }
            }
        }
        void linkAction_DataBinding(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.CommandKeyField))
                return;

            LinkButton target = (LinkButton)sender;

            Control container = target.NamingContainer;
            object dataItem = DataBinder.GetDataItem(container);
            object keyFieldValue = null;

            if (this.DataField.Contains("."))
                keyFieldValue = DataBinder.Eval(dataItem, this.CommandKeyField);
            else
                keyFieldValue = DataBinder.GetPropertyValue(dataItem, this.CommandKeyField);

            if (keyFieldValue != null)
            {
                target.CommandArgument = keyFieldValue.ToString();
            }

            if (string.IsNullOrEmpty(target.Text))
            {
                object dataFieldValue = null;

                if (this.DataField.Contains("."))
                {
                    dataFieldValue = DataBinder.Eval(dataItem, this.DataField);
                }
                else
                {
                    dataFieldValue = DataBinder.GetPropertyValue(dataItem, this.DataField);
                }

                // set the table cell's text. check for null values to prevent ToString errors
                if (dataFieldValue != null)
                {
                    target.Text = GetValueWithFormat(dataFieldValue);
                }
            }
        }


        private static System.Globalization.CultureInfo CultExpiry = new System.Globalization.CultureInfo("en-US");

        private string GetValueWithFormat(object _dataFieldValue)
        {
            if ((_dataFieldValue == null) || (_dataFieldValue == DBNull.Value)) return string.Empty;

            var valueType = this.FormatType;
            var format = this.FormatString;

            if (string.IsNullOrEmpty(format)) format = null;

            if (this.DataField.ToLower() == "expiry_date" && _dataFieldValue.GetType() == typeof(string))
            {
                _dataFieldValue = DateTime.ParseExact(_dataFieldValue.ToString(), "yyyyMMdd", CultExpiry);
                valueType = FieldValueType.Date;
            }

            switch (valueType)
            {
                case FieldValueType.Integer:
                    return Convert.ToDecimal(_dataFieldValue).ToString(format ?? "0");

                case FieldValueType.Number:
                    //string digit = "0".PadLeft(Extensions.Digit_INT, '0');
                    //return Convert.ToDecimal(_dataFieldValue).ToString(format ?? "0."+ digit + "#######");
                    return Convert.ToDecimal(_dataFieldValue).ToString(format ?? Extensions.FormatDecimal);

                case FieldValueType.DateTime:
                    return Convert.ToDateTime(_dataFieldValue).ToString(format ?? FieldsStatic.DateFormat + " " + FieldsStatic.TimeFormat);

                case FieldValueType.Date:
                    return Convert.ToDateTime(_dataFieldValue).ToString(format ?? FieldsStatic.DateFormat);

                case FieldValueType.Time:
                    return Convert.ToDateTime(_dataFieldValue).ToString(format ?? FieldsStatic.TimeFormat);

                default:
                    return _dataFieldValue.ToString();
            }
        }

        #endregion
    }
}