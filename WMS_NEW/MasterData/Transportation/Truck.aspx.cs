using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.MasterData.Transportation
{
    public partial class Truck : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iColTruckType.DropDownQueryProperty = delegate () { return new Access.MasterData.Transportation.TruckType().GetQuery(); };
                iColCarrier.DropDownQueryProperty = delegate () { return new Access.MasterData.Transportation.Carrier().GetQuery(); };

                #region Init PopupEntity 

                var access = new Access.MasterData.Transportation.Truck();
                var entity = access.Entity;

                #region Init Controls Entity

                var tabs_attr = new _UControls.EntityTab[]{
                    new _UControls.EntityTab { TabIndex = 1, TabName = "User Define" },
                };


                #region Binding
                _UControls.InputDropDown TruckType = new _UControls.InputDropDown() { DataFieldValue = nameof(entity.truck_type_id), ComboType = _UControls.ComboType.Guid, DisplayDefault = "--Select--" };
                TruckType.MethodQueryProperty = delegate () { return new Access.MasterData.Transportation.TruckType().GetQuery(); };
                TruckType.ResourceGroup = "TruckType";
                TruckType.ResourceName = "truck_type";

                _UControls.InputDropDown Carrier = new _UControls.InputDropDown() { DataFieldValue = nameof(entity.carrier_id), ComboType = _UControls.ComboType.Guid, DisplayDefault = "--Select--" };
                Carrier.MethodQueryProperty = delegate () { return new Access.MasterData.Transportation.Carrier().GetQuery(); };
                Carrier.ResourceGroup = "carrier";
                Carrier.ResourceName = "carrier_code";


                #endregion

                var override_controls = new List<_UControls.EntityCustom>();

                override_controls.Add(new _UControls.EntityCustom(TruckType));
                override_controls.Add(new _UControls.EntityCustom(Carrier));
                override_controls.Add(new _UControls.EntityCustom { DataFieldValue = nameof(entity.truck_name), IsKey = true });

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