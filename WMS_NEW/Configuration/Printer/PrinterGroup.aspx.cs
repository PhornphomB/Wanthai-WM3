using System;
using System.Collections.Generic;

namespace WMS_NEW.Configuration.Printer
{
    public partial class PrinterGroup : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iColWarehouse.DropDownQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };

                #region Init PopupEntity 

                var access = new Access.Configuration.Printer.PrinterGroup();
                var entity = access.Entity;

                #endregion

                _UControls.InputDropDown Warehouse = new _UControls.InputDropDown() { DataFieldValue = nameof(entity.wh_master_id), ComboType = _UControls.ComboType.Guid, DisplayDefault = "--Select--"};
                Warehouse.MethodQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };
                Warehouse.ResourceGroup = "warehouse";
                Warehouse.ResourceName = "wh_id";

                var override_controls = new List<_UControls.EntityCustom>();

                override_controls.Add(new _UControls.EntityCustom(Warehouse) { RefGlobalVar = (REF) => { Warehouse = REF; } });


                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = access; };
                popupEntity1.AutoCreateControlEntity(entity, override_controls, null,  nameof(entity.location_id));

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