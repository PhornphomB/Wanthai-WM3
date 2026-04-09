using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.MasterData.Transportation
{
    public partial class Carrier : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Init PopupEntity 

                var access = new Access.MasterData.Transportation.Carrier();
                var entity = access.Entity;

                #region Init Controls Entity

                var override_controls = new _UControls.EntityCustom[] {
                   new _UControls.EntityCustom { DataFieldValue = nameof(entity.carrier_code), IsKey = true }
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