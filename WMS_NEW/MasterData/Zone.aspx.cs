using System;
using System.Collections.Generic;

namespace WMS_NEW.MasterData
{
    public partial class Zone : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iColWarehouse.DropDownQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };
                iColZoneType.DropDownQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("mst_zone_type"); };

                #region Init PopupEntity 

                var access = new Access.MasterData.Zone();
                var entity = access.Entity;

                #region Init Controls Entity

                var tabs_attr = new _UControls.EntityTab[]{
                    new _UControls.EntityTab { TabIndex = 1, TabName = "User Define" },
                };


                #region Binding
                _UControls.InputDropDown Warehouse = new _UControls.InputDropDown() { DataFieldValue = nameof(entity.wh_master_id), ComboType = _UControls.ComboType.Guid, DisplayDefault = "--Select--", IsKey = true, ResourceGroup = "warehouse", ResourceName = "wh_master_id" };
                Warehouse.MethodQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };

                _UControls.InputDropDown ZoneType = new _UControls.InputDropDown() { DataFieldValue = nameof(entity.zone_type), ComboType = _UControls.ComboType.String, DisplayDefault = "--Select--" };
                ZoneType.MethodQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("mst_zone_type"); };

                #endregion

                var override_controls = new List<_UControls.EntityCustom>();

                override_controls.Add(new _UControls.EntityCustom(Warehouse));
                override_controls.Add(new _UControls.EntityCustom(ZoneType));
                override_controls.Add(new _UControls.EntityCustom { DataFieldValue = nameof(entity.zone), IsKey = true });

                #endregion



                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = access; };
                popupEntity1.AutoCreateControlEntity(entity, override_controls);

                GridExt1.PopupEntitySource = popupEntity1;

                var ucUser = new _UControls.InputHidden();
                ucUser.DataFieldValue = "user_id";
                ucUser.SetValue(_SessionVals.UserName);
                GridExt1.AddFilterCustomInputInclude(ucUser);

                #endregion
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }
}