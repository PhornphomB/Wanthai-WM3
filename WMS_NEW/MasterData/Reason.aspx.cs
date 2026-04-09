using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.MasterData
{
    public partial class Reason : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iColReasonType.DropDownQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("mst_reason_type"); };

                #region Init PopupEntity 

                var access = new Access.MasterData.Reason();
                var entity = access.Entity;

                #region Init Controls Entity

                var tabs_attr = new _UControls.EntityTab[]{
                    new _UControls.EntityTab { TabIndex = 1, TabName = "User Define" },
                };

                #region Binding

                _UControls.InputDropDown ReasonType = new _UControls.InputDropDown() { DataFieldValue = nameof(entity.reason_type), ComboType = _UControls.ComboType.String, DisplayDefault = "--Select--"};
                ReasonType.MethodQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("mst_reason_type"); };

                #endregion

                var override_controls = new List<_UControls.EntityCustom>();

                override_controls.Add(new _UControls.EntityCustom(ReasonType));
                override_controls.Add(new _UControls.EntityCustom { DataFieldValue = nameof(entity.reason_code), IsKey = true });

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