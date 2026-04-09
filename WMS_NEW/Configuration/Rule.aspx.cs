using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Configuration
{
    public partial class Rule : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Init PopupEntity 

                var access = new Access.Configuration.Rule();
                var entity = access.Entity;

                #endregion

                var override_controls = new List<_UControls.EntityCustom>();

                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = access; };
                popupEntity1.AutoCreateControlEntity(entity, new List<_UControls.EntityCustom>(), null, nameof(entity.create_date));


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