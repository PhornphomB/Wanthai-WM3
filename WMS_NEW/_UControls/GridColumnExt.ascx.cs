using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Prototype.Providers;

namespace _UControls
{
    public partial class GridColumnExt : System.Web.UI.UserControl, IGridColumnExt
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ControlType = _UControls.ControlType.Label;
        }

        public int PrimaryIndex { get; set; }

        #region GridFilters Properties

        public Func<string, int, IEnumerable<Property>> DropDownQueryViaStore { get; set; }
        public Func<IQueryable<Property>> DropDownQueryProperty { get; set; }
        public string DropDownDisplayDefault { get; set; }
        public dynamic DropDownSelectedValue { get; set; }
        public int? DropDownLazyLimit { get; set; }
        public bool? DropDownSearchMultiValue { get; set; }
        public bool DropDownAutoPostBack { get; set; }
        public Action<dynamic> DropDownPostValueChanged { get; set; }

        public DropDownType DropDownFilterType { get; set; }
        public string DataFieldFilter { get; set; }
        public int? FilterIndex { get; set; }
        public FilterAt DefaultFilter { get; set; }
        public bool AllowFilter { get; set; }
        public bool FixFilter { get; set; }
        public bool ShowFilterNow { get; set; }
        public bool UseFilterDropDown { get; set; }
        public FieldValueType FilterFormatType { get; set; }
        public string FilterFormatString { get; set; }
        public int? FilterWidth { get; set; }

        public bool FilterPrimary { get; set; }

        #endregion

        #region Public Properties

        public string CommandText { get; set; }
        public string CommandName { get; set; }
        //public string ToolTipText { get; set; }

        public ControlType ControlType { get; set; }
        public int? FieldIndex { get; set; }
        public bool AllowSort { get; set; }
        public string HeaderText { get; set; }
        public string DataField { get; set; }
        public FieldValueType FormatType { get; set; }
        public string FormatString { get; set; }
        public HorizontalAlign FieldTextAlign
        {
            get
            {
                if (ViewState["FieldTextAlign"] == null)
                    ViewState["FieldTextAlign"] = HorizontalAlign.Left;

                return (HorizontalAlign)ViewState["FieldTextAlign"];
            }
            set
            {
                ViewState["FieldTextAlign"] = value;
            }
        }
        public int Width
        {
            get
            {
                if (ViewState["Width"] == null)
                    ViewState["Width"] = 101;

                return (int)ViewState["Width"];
            }
            set
            {
                ViewState["Width"] = value;
            }
        }

        public bool InputTextAutoPostBack { get; set; }

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
            }
        }

        public string DataFieldValue
        {
            get
            {
                return DataField;
            }
            set
            {
                DataField = value;
            }
        }

        public bool IsConfirm
        {
            get
            {
                if (ViewState["IsConfirm"] == null)
                    ViewState["IsConfirm"] = true;

                return (bool)ViewState["IsConfirm"];
            }
            set
            {
                ViewState["IsConfirm"] = value;
            }
        }
        public bool IsIncludeInExcel
        {
            get
            {
                if (ViewState["IsIncludeInExcel"] == null)
                    ViewState["IsIncludeInExcel"] = true;

                return (bool)ViewState["IsIncludeInExcel"];
            }
            set
            {
                ViewState["IsIncludeInExcel"] = value;
            }
        }
        public string IsConfirmMessage
        {
            get
            {
                if (ViewState["IsConfirmMessage"] == null)
                    ViewState["IsConfirmMessage"] = string.Empty;

                return (string)ViewState["IsConfirmMessage"];
            }
            set
            {
                ViewState["IsConfirmMessage"] = value;
            }
        }
        #endregion
    }
}