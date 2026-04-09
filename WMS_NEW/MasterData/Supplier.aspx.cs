using System;
using System.Collections.Generic;

namespace WMS_NEW.MasterData
{
    public partial class Supplier : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iColOwner.DropDownQueryProperty = delegate () { return new Access.MasterData.Owner().GetQuery(_SessionVals.UserName); };

                #region Init PopupEntity 

                var access = new Access.MasterData.Supplier();
                var entity = access.Entity;

                #region Init Controls Entity

                var tabs_attr = new _UControls.EntityTab[]{
                    new _UControls.EntityTab { TabIndex = 1, TabName = "User Define",ResourceGroup="supplier",ResourceName="tab_user_define",IsAutoUserDefine=true },
                };

                #region Binding
                _UControls.InputDropDown Owner = new _UControls.InputDropDown() { DataFieldValue = nameof(entity.owner_id), ComboType = _UControls.ComboType.Guid, DisplayDefault = "--Select--" };
                Owner.MethodQueryProperty = delegate () { return new Access.MasterData.Owner().GetQuery(_SessionVals.UserName); };
                Owner.ResourceGroup = "owner";
                Owner.ResourceName = "owner_code";
                #endregion

                var override_controls = new List<_UControls.EntityCustom>();
                override_controls.Add(new _UControls.EntityCustom(Owner));
                override_controls.Add(new _UControls.EntityCustom { DataFieldValue = nameof(entity.supplier_code), IsKey = true });

                #endregion


                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = access; };
                popupEntity1.AutoCreateControlEntity(entity, override_controls,tabs_attr);

                GridExt1.PopupEntitySource = popupEntity1;

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