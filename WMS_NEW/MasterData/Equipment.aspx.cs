using System;
using System.Collections.Generic;

namespace WMS_NEW.MasterData
{
    public partial class Equipment : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Init PopupEntity 

                var access = new Access.MasterData.Equipment();
                var entity = access.Entity;

                #region Init Controls Entity


                #region Binding
                _UControls.InputDropDown Warehouse = new _UControls.InputDropDown() { DataFieldValue = nameof(entity.wh_master_id), ComboType = _UControls.ComboType.Guid, DisplayDefault = "--Select--", IsKey = true };
                Warehouse.MethodQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };

                _UControls.InputDropDown EquipmentType = new _UControls.InputDropDown() { DataFieldValue = nameof(entity.equipment_type), ComboType = _UControls.ComboType.String, DisplayDefault = "--Select--" };
                EquipmentType.MethodQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("mst_equipment_type"); };

                #endregion

                var override_controls = new List<_UControls.EntityCustom>();

                override_controls.Add(new _UControls.EntityCustom(Warehouse));
                override_controls.Add(new _UControls.EntityCustom(EquipmentType));
                override_controls.Add(new _UControls.EntityCustom { DataFieldValue = nameof(entity.equipment_code), IsKey = true });

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