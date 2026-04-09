using System;

namespace WMS_NEW.MasterData
{
    public partial class Warehouse : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Init PopupEntity 

                var access = new Access.MasterData.Warehouse();
                var entity = access.Entity;

                #region Init Controls Entity

                var tabs_attr = new _UControls.EntityTab[]{
                    new _UControls.EntityTab { TabIndex = 1, TabName = "User Define" },
                };

                var override_controls = new _UControls.EntityCustom[] {
                   new _UControls.EntityCustom { DataFieldValue = nameof(entity.wh_id), IsKey = true },
                   new _UControls.EntityCustom { DataFieldValue = nameof(entity.wh_group), IsPrimary = true }
               };
                #endregion


                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = access; };
                popupEntity1.AutoCreateControlEntity(entity, override_controls);

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