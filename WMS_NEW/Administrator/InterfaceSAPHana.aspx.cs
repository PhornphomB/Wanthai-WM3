using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Source;

namespace WMS_NEW.Administrator
{
    public partial class InterfaceSAPHana : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ddlProcessingStatus.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("interface_sap_hana_processing_status"); };
                ddlChangeStatus.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("interface_sap_hana_processing_status"); };

                popupEntity1.InitObjectsEvent += () => { popupEntity1.ObjectDataAccess = new Access.Administrator.InterfaceSAPHana(); };
                popupEntity1.InitControlStatic();
                GridExt1.PopupEntitySource = popupEntity1;

                if (!IsPostBack)
                {
                    ddlProcessingStatus.BindDataSource();
                    ddlChangeStatus.BindDataSource();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
            
        }

        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {
            var KeyId = GridExt1.GetListKey();
            if(KeyId.Count == 0)
            {
                Page.MessageWarning("Please select at least one record.");
                return;
            }
            else
            {
                popupChangeStatus.ShowDialog();
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            var Keys = GridExt1.GetListKey();
            List<Guid> keyId = new List<Guid>();
            for (int i = 0; i < Keys.Count; i++)
            {
                keyId.Add(Guid.Parse(Keys[i].KeyId.ToString()));
            }
            var status = ddlChangeStatus.GetValue();
            foreach (var id in keyId)
            {
                using (var _Model = new Source.WMSEntities())
                {
                    var ent = _Model.t_host_export_interface_sap_hana.FirstOrDefault(x => x.interface_id == id);
                    if (ent != null)
                    {
                        ent.processing_status = status;
                        _Model.SaveChanges();
                    }
                }
            }
            Page.MessageSuccess("Change status successfully.");
            GridExt1.Search();
            popupChangeStatus.HideDialog();
        }

        protected void btnExportSAP_Click(object sender, EventArgs e)
        {
            using(var Model = new WMSEntities())
            {
                try
                {
                    Model.usp_interface_export_data_sap_hana();
                    Page.MessageSuccess("Export data to SAP Hana successfully.");
                    GridExt1.Search();
                }
                catch (DbEntityValidationException ex)
                {
                    // สร้างข้อความแสดง Validation Error
                    var validationErrors = ex.EntityValidationErrors
                        .SelectMany(eve => eve.ValidationErrors)
                        .Select(ve => $"Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                    var errorMessage = string.Join("; ", validationErrors);
                    this.Logging = new Prototype.Providers.Logging(this, new Exception(errorMessage));
                    this.RaiseLogging();
                }
                catch (Exception ex)
                {
                    Logging = new Prototype.Providers.Logging(this, ex);
                    RaiseLogging();
                }
                    
            }
        }
    }
}