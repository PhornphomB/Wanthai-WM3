using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WMS_NEW._ExtensionClass
{
    public static class clsResource
    {
        public static string GetResource(string ResourceGroup, string ResourceName)
        {
            string ResourceValue = ResourceName;
            using (WMS_NEW.Source.WMSEntities _model = new WMS_NEW.Source.WMSEntities())
            {
                var res_group = _model.t_com_resource_master.Where(q => q.resource_group == ResourceGroup && q.resource_name == ResourceName).FirstOrDefault();
                if (res_group != null)
                {
                    var resource = _model.t_com_resource_detail.Where(q => q.resource_master_id == res_group.resource_master_id
                                    && q.locale_id == _SessionVals.LocaleID).FirstOrDefault();
                    if (resource != null)
                        ResourceValue = resource.value;
                }
            }
            return ResourceValue;
        }
    }
}