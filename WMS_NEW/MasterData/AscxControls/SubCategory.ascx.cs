using _UControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.MasterData.AscxControls
{
    public partial class SubCategory : UControlCustom, IFormRelation
    {   
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Init PopupEntity 

                //var access = new Access.MasterData.SubCategory();
                //var entity = access.Entity;

                #region Init Controls Entity


                #region Binding
                #endregion

                //var override_controls = new List<_UControls.EntityCustom>();

                //override_controls.Add(new _UControls.EntityCustom { DataFieldValue = nameof(entity.sub_category), IsKey = true });

                #endregion
                popupEntity1.PreSaveEntityEvent += PopupEntity1_PreSaveEntityEvent;
                popupEntity1.AfterNewDataEvent += PopupEntity1_AfterNewDataEvent;
                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = new Access.MasterData.SubCategory(); };
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
                txtSubCategory.Clear();
                txtDescription.Clear();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void PopupEntity1_PreSaveEntityEvent()
        {
            var _objectEntity = (popupEntity1.ObjectDataAccess as Access.MasterData.SubCategory).Entity;
            
        }

        public Action<dynamic> UpdateParent { get; set; }

        public void InitForm(dynamic _obj)
        {
            try
            {
                var objCate = (Source.t_wms_category)_obj;

                hidCategory.SetValue(objCate.category_id);
                hidCategoryId.SetValue(objCate.category_id);
                GridExt1.Search();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


    }
}