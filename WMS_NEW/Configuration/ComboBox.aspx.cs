using System;
using System.Collections.Generic;

namespace WMS_NEW.Configuration
{
    public partial class ComboBox : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Init PopupEntity 

                var access = new Access.Configuration.ComboBox();
                var entity = access.Entity;

                #endregion

                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = access; };
                popupEntity1.AutoCreateControlEntity(entity, new List<_UControls.EntityCustom>(), null, nameof(entity.create_datetime));

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