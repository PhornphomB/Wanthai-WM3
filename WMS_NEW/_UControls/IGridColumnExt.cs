using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

using Prototype.Providers;

namespace _UControls
{
    public interface IGridColumnExt : ConfigGlobal.Interface.IResourceInput
    {
        int PrimaryIndex { get; set; }

        #region GridFilters Properties

        Func<string, int, IEnumerable<Property>> DropDownQueryViaStore { get; set; }
        Func<IQueryable<Property>> DropDownQueryProperty { get; set; }
        string DropDownDisplayDefault { get; set; }
        dynamic DropDownSelectedValue { get; set; }
        int? DropDownLazyLimit { get; set; }
        bool? DropDownSearchMultiValue { get; set; }
        bool DropDownAutoPostBack { get; set; }
        Action<dynamic> DropDownPostValueChanged { get; set; }

        string DataFieldFilter { get; set; }
        int? FilterIndex { get; set; }
        FilterAt DefaultFilter { get; set; }
        bool AllowFilter { get; set; }
        bool FixFilter { get; set; }
        bool ShowFilterNow { get; set; }
        bool UseFilterDropDown { get; set; }
        DropDownType DropDownFilterType { get; set; }
        FieldValueType FilterFormatType { get; set; }
        string FilterFormatString { get; set; }
        int? FilterWidth { get; set; }

        bool FilterPrimary { get; set; }

        #endregion

        #region Public Properties

        string CommandText { get; set; }
        string CommandName { get; set; }
        //string ToolTipText { get; set; }

        ControlType ControlType { get; set; }
        int? FieldIndex { get; set; }
        bool AllowSort { get; set; }
        string HeaderText { get; set; }
        string DataField { get; set; }
        FieldValueType FormatType { get; set; }
        string FormatString { get; set; }
        HorizontalAlign FieldTextAlign { get; set; }
        int Width { get; set; }

        bool InputTextAutoPostBack { get; set; }

        bool IsConfirm { get; set; }
        string IsConfirmMessage { get; set; }
        bool IsIncludeInExcel { get; set; }
        #endregion
    }
}
