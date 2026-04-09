using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Prototype.Providers;

namespace _UControls
{
    public class GridColumnExtLite : IGridColumnExt
    {
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

        private HorizontalAlign _fieldTextAlign = HorizontalAlign.Left;
        public HorizontalAlign FieldTextAlign
        {
            get
            {
                return _fieldTextAlign;
            }
            set
            {
                _fieldTextAlign = value;
            }
        }

        private int _width = 101;
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        public bool InputTextAutoPostBack { get; set; }

        #endregion

        #region Interface Resource

        private string _resourceGroup = string.Empty;
        public string ResourceGroup
        {
            get
            {
                return _resourceGroup;
            }
            set
            {
                _resourceGroup = value;
            }
        }

        private string _resourceName = string.Empty;
        public string ResourceName
        {
            get
            {
                return _resourceName;
            }
            set
            {
                _resourceName = value;
            }
        }

        private string _resourceValue = string.Empty;
        public string ResourceValue
        {
            get
            {
                return _resourceValue;
            }
            set
            {
                _resourceValue = value;
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

        private bool _isConfirm = true;
        public bool IsConfirm
        {
            get
            {
                return _isConfirm;
            }
            set
            {
                _isConfirm = value;
            }
        }
        private bool _IsIncludeInExcel = true;
        public bool IsIncludeInExcel
        {
            get
            {
                return _IsIncludeInExcel;
            }
            set
            {
                _IsIncludeInExcel = value;
            }
        }
        public string IsConfirmMessage 
        {
            get
            {
                return IsConfirmMessage;
            }
            set
            {
                IsConfirmMessage = value;
            }
        }
        #endregion
    }
}