using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Configuration.Printer
{
    public partial class PrinterMapping : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iColGroupPrint.DropDownQueryProperty = delegate () { return Access.Configuration.Printer.PrinterGroup.Instance.GetQuery(); };
                iColPrint.DropDownQueryProperty = delegate () { return Access.Configuration.Printer.Printer.Instance.GetQuery(); };

                #region Init PopupEntity 

                var access = new Access.Configuration.Printer.PrinterMapping();
                var entity = access.Entity;

                #endregion

                _UControls.InputDropDown ddlPrint = new _UControls.InputDropDown() { DataFieldValue = nameof(entity.printer_id), ComboType = _UControls.ComboType.Guid, DisplayDefault = "--Select--" };
                ddlPrint.MethodQueryProperty = delegate () { return Access.Configuration.Printer.Printer.Instance.GetQuery(); };
                ddlPrint.ResourceGroup = "Printer";
                ddlPrint.ResourceName = "printer_name";

                _UControls.InputDropDown ddlGroupPrint = new _UControls.InputDropDown() { DataFieldValue = nameof(entity.group_id), ComboType = _UControls.ComboType.Guid, DisplayDefault = "--Select--" };
                ddlGroupPrint.MethodQueryProperty = delegate () { return Access.Configuration.Printer.PrinterGroup.Instance.GetQuery(); };
                ddlGroupPrint.ResourceGroup = "PrinterGroup";
                ddlGroupPrint.ResourceName = "group_name";

                var override_controls = new List<_UControls.EntityCustom>();
                override_controls.Add(new _UControls.EntityCustom(ddlPrint));
                override_controls.Add(new _UControls.EntityCustom(ddlGroupPrint));


                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = access; };
                popupEntity1.AutoCreateControlEntity(entity, override_controls);

                GridExt1.PopupEntitySource = popupEntity1;


            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }

}