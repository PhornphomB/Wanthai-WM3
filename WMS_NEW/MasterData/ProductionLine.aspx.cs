using _UControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Access.MasterData;

namespace WMS_NEW.MasterData
{
    public partial class ProductionLine : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                GridColumnExt1.DropDownQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };

                #region Init PopupEntity 

                var access = new Access.MasterData.ProductionLine();
                var entity = access.Entity;

                #region Init Controls Entity

                //var tabs_attr = new _UControls.EntityTab[]{
                //    new _UControls.EntityTab { TabIndex = 1, TabName = "User Define" },
                //};


                //#region Binding
                //_UControls.InputDropDown Warehouse = new _UControls.InputDropDown() { DataFieldValue = nameof(entity.wh_master_id), ComboType = _UControls.ComboType.Guid, DisplayDefault = "--Select--", IsKey = true, ResourceGroup = "warehouse", ResourceName = "wh_master_id" };
                //Warehouse.MethodQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };

                //#endregion

                //var override_controls = new List<_UControls.EntityCustom>();

                //override_controls.Add(new _UControls.EntityCustom(Warehouse));

                #endregion


                ddlWarehouse.MethodQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };

                //popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = access; };
                //popupEntity1.AutoCreateControlEntity(entity, override_controls);
                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = new Access.MasterData.ProductionLine(); };
                popupEntity1.InitControlStatic();
                GridExt1.PopupEntitySource = popupEntity1;

                var ucUser = new _UControls.InputHidden();
                ucUser.DataFieldValue = "user_id";
                ucUser.SetValue(_SessionVals.UserName);
                GridExt1.AddFilterCustomInputInclude(ucUser);

                if (!Page.IsPostBack)
                {
                    #region Set Initial Filter Grid

                    var ucFirstLoad = new _UControls.InputHidden() { DataFieldValue = "_is_first_load" };
                    ucFirstLoad.SetValue(true);
                    GridExt1.AddFilterCustomInputInclude(ucFirstLoad);
                    #endregion
                }

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