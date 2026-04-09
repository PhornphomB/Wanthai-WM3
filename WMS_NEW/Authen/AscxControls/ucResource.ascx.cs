using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Authen.AscxControls
{
    public partial class ucResource : UControlCustom, _UControls.IFormRelation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Initial Input Data

                ddlLocal.MethodQueryProperty = delegate () { return global::SecurityM.Access.PropertyCollection.Locale.Instance.GetQueryProperty(); };

                #endregion

                popup1.InitObjectsEvent += () => { popup1.ObjectDataAccess = new SecurityM.Access.Master.ResourceDetail(); };
                popup1.InitControlStatic();

                GridExt1.PopupEntitySource = popup1;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        public Action<dynamic> UpdateParent { get; set; }

        public void InitForm(dynamic _obj)
        {
            var dto = (SecurityM.Source.t_com_menu)_obj;
            var resource_master_id = dto.resource_master_id;

            if (string.IsNullOrEmpty(resource_master_id))
            {
                var ent = (SecurityM.Source.t_com_resource_master)SecurityM.Access.Master.ResourceMaster.Instance.GetDataByMenuID(dto.menu_id);
                if (ent != null)
                {
                    resource_master_id = ent.resource_master_id;
                }
            }

            hid_resource_ms_id.SetValue(resource_master_id);
            hid_grid_resource_ms_id.SetValue(resource_master_id);

            GridExt1.Search();
        }
    }
}