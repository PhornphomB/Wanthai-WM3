using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.MasterData
{
    public partial class ItemCategory : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Init PopupEntity 

                var access = new Access.MasterData.ItemCategory();
                var entity = access.Entity;

                #region Init Controls Entity


                #region Binding
                #endregion

                var override_controls = new List<_UControls.EntityCustom>();
                override_controls.Add(new _UControls.EntityCustom { DataFieldValue = nameof(entity.item_category), IsKey = true });

                #endregion


                popupEntity1.AfterNewDataEvent += PopupEntity1_AfterNewDataEvent;
                popupEntity1.AfterSetEditDataEvent += PopupEntity1_AfterSetEditDataEvent;
                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = access; };
                popupEntity1.InitControlStatic();
               // popupEntity1.AutoCreateControlEntity(entity, override_controls);

                GridExt1.PopupEntitySource = popupEntity1;

                #endregion
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void PopupEntity1_AfterNewDataEvent()
        {
            try
            {
                PanelTab1.VisiblePanel(1, false);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void PopupEntity1_AfterSetEditDataEvent()
        {
            try
            {
                var id = (Guid)popupEntity1.KeyFieldValue;
                //ucSubCategory.InitForm(id);
                PanelTab1.VisiblePanel(2, false);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
    }

}