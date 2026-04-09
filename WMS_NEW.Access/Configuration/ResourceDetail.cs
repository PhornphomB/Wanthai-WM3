using Prototype.Providers;
using System;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Configuration
{
    public class ResourceDetail : AEntityFormCommand<t_com_resource_detail>
    {
        protected WMSEntities _Model { get; set; }

        public ResourceDetail()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_com_resource_detail; };
        }

        public override bool ValidateSaveNew(t_com_resource_detail ent, ref string msg_validate)
        {
            if (_Model.t_com_resource_detail.Any(x => x.resource_master_id == ent.resource_master_id && x.locale_id == ent.locale_id))
            {
                msg_validate = "! Local has in system.";
                return false;
            }
            else
                return true;
        }

        public override void SetOptionalSaveNew(t_com_resource_detail ent)
        {
            ent.resource_detail_id = Guid.NewGuid().ToString();
            ent.create_date = DateTime.Now;
            ent.create_by = _SessionVals.UserName;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {

            string resource_master_id = string.Empty;
            if (this.FilterCustom != null)
            {
                var ent = this.FilterCustom.Where(w => w.DataFieldValue == "resource_master_id").FirstOrDefault();
                if (ent != null && ent.Value != null)
                {
                    resource_master_id = ent.Value.ToString();
                }
            }
            var result = from rows in this._Model.t_com_resource_detail
                         where rows.resource_master_id == resource_master_id
                         select new
                         {
                             KeyId = rows.resource_detail_id,
                             rows.resource_master_id,
                             rows.locale_id,
                             rows.t_com_locale.locale,
                             rows.t_com_locale.name,
                             rows.value,
                             rows.create_by,
                             rows.create_date
                         };


            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }
        public static string GetResource(string ResourceGroup, string ResourceName)
        {
            string ResourceValue = ResourceName;
            using (WMSEntities _model = new WMSEntities())
            {
                var res_group = _model.t_com_resource_master.Where(q => q.resource_group == ResourceGroup && q.resource_name == ResourceName).FirstOrDefault();
                if (res_group != null)
                {
                    var resource = _model.t_com_resource_detail.Where(q => q.resource_master_id == res_group.resource_master_id
                                    && q.locale_id == _SessionVals.LocaleID).FirstOrDefault();
                    if (resource != null)
                        ResourceValue = resource.value;
                    else
                        ResourceValue = res_group.default_value;
                }
            }
            return ResourceValue;
        }
        #endregion


    }
}
