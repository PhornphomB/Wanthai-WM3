using _UControls;
using System;
using System.Web.UI;

namespace WMS_NEW.Configuration.AscxControls
{
    public partial class ucResourceDetail : UControlCustom, IFormRelation
    {
        private string resource_master_id
        {
            get
            {
                if (ViewState["resource_master_id"] == null)
                    ViewState["resource_master_id"] = string.Empty;

                return (string)ViewState["resource_master_id"];
            }
            set
            {
                ViewState["resource_master_id"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = new Access.Configuration.ResourceDetail(); };
                popupEntity1.InitControlStatic();

                GridExt1.PopupEntitySource = popupEntity1;

                if (!Page.IsPostBack)
                {
                    ddlLocale.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQuery_Locale(); };
                    ddlLocale.BindDataSource();
                }
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
            try
            {
                var ent = (Source.t_com_resource_master)_obj;

                this.resource_master_id = ent.resource_master_id;
                txtResourcemasterId.SetValue(this.resource_master_id);
                txtResourceGroup.SetValue(ent.resource_group);
                txtResourceName.SetValue(ent.resource_name);

                hid_resource_master_id.SetValue(this.resource_master_id);
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