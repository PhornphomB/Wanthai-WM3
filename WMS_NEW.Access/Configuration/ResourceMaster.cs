using ConfigGlobal.Interface;
using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Configuration
{
    public class ResourceMaster : AEntityFormCommand<t_com_resource_master>
    {
        protected WMSEntities _Model { get; set; }

        public ResourceMaster()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_com_resource_master; };
        }

        public override bool ValidateSaveNew(t_com_resource_master ent, ref string msg_validate)
        {
            if (_Model.t_com_resource_master.Any(x => x.resource_group == ent.resource_group && x.resource_name == ent.resource_name))
            {
                msg_validate = "! Resource group and resource name has in system.";
                return false;
            }
            else
                return true;
        }

        public override void SetOptionalSaveNew(t_com_resource_master ent)
        {
            ent.resource_master_id = Guid.NewGuid().ToString();
            ent.create_date = DateTime.Now;
            ent.create_by = _SessionVals.UserName;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_com_resource_master
                         select rows;

            if (this.FilterCustom != null)
            {
                var ent = this.FilterCustom.Where(w => w.DataFieldValue == "_value").FirstOrDefault();
                if (ent != null && ent.Value != null)
                {
                    string value = ent.Value.ToString();

                    result = result.Where(w => w.t_com_resource_detail.Any(a => a.value.Contains(value)));
                }
            }




            var result2 = from rows in result
                          select new
                          {
                              KeyId = rows.resource_master_id,
                              rows.app_id,
                              rows.resource_group,
                              rows.resource_name,
                              rows.default_value,
                              rows.create_by,
                              rows.create_date,
                          };


            return result2;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


        #region Function
        public Resource[] Get_Resource(IEnumerable<IResource> _listResource)
        {
            return this._Model.GetDataBy(this, delegate ()
            {

                string app_id = _SessionVals.AppID;
                string locale_id = _SessionVals.LocaleID;

                var resourceKeys = (from rows in _listResource
                                    select rows.ResourceGroup.ToLower() + "_" + rows.ResourceName.ToLower()).Distinct().ToArray();

                //Delegate Statemant ---
                var result = from rows in this._Model.t_com_resource_master

                             let detail = rows.t_com_resource_detail.FirstOrDefault(qry => qry.locale_id == locale_id)
                             let value = !string.IsNullOrEmpty(detail.locale_id) ? detail.value : rows.default_value

                             where rows.app_id == app_id && resourceKeys.Contains(rows.resource_group.ToLower() + "_" + rows.resource_name.ToLower()) && !string.IsNullOrEmpty(value)
                             select new Resource
                             {
                                 ResourceGroup = rows.resource_group,
                                 ResourceName = rows.resource_name,
                                 ResourceValue = value
                             };

                return result.ToArray();

            });
        }

        public ResourceError[] GetErrorResource()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                string app_id = _SessionVals.AppID;
                string locale_id = _SessionVals.LocaleID;

                //Delegate Statemant ---
                var result = from rows in this._Model.t_com_resource_master

                             let detail = rows.t_com_resource_detail.FirstOrDefault(qry => qry.locale_id == locale_id)
                             let value = !string.IsNullOrEmpty(detail.locale_id) ? detail.value : rows.default_value

                             where rows.app_id == app_id
                                    && rows.resource_group == "message_error" //resourceKeys.Contains(rows.resource_group.ToLower() + "_" + rows.resource_name.ToLower()) 
                                    && !string.IsNullOrEmpty(value)
                             select new ResourceError
                             {
                                 ResourceGroup = rows.resource_group,
                                 ResourceName = rows.resource_name,
                                 ResourceDefault = rows.default_value,
                                 ResourceValue = value
                             };

                return result.ToArray();
            });
        }
        #endregion
    }
}
