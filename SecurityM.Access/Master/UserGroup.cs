using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

using SecurityM.Source;
using Prototype.Providers;

namespace SecurityM.Access.Master
{
    public class UserGroup : AEntityFormCommand<t_com_user_group>
    {
        public SecurityM_Entities _Model { get; set; }

        public UserGroup()
        {
            this._Model = new SecurityM_Entities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_com_user_group; };
        }


        #region ++INSTANCE STATIC++
        public static UserGroup Instance
        {
            get
            {
                using (UserGroup _Instance = new UserGroup())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public override void SetOptionalSaveNew(t_com_user_group ent)
        {
            ent.user_group_id = Guid.NewGuid().ToString();
        }

        public override bool ValidateSaveNew(t_com_user_group ent, ref string msg_validate)
        {
            if (this._Model.t_com_user_group.Any(qry => qry.name == ent.name && qry.app_id == ent.app_id))
            {
                //var res_group = _Model.t_com_resource_master.Where(q => q. == ResourceGroup && q.resource_name == ResourceName).FirstOrDefault();
                //if (res_group != null)
                //{
                //    var resource = _Model.t_com_resource_detail.Where(q => q.resource_master_id == res_group.resource_master_id
                //                    && q.locale_id == _SessionVals.LocaleID).FirstOrDefault();
                //    if (resource != null)
                //        msg_validate = resource.value;
                //}
                msg_validate = "! User Group and has in system.";
                return false;
            }
            else
                return true;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var app_id = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_appID").Value;
            var isAdmin = Convert.ToBoolean(this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_isAdmin").Value);

            var result = from rows in this._Model.t_com_user_group
                         where rows.app_id == app_id
                         select new
                         {
                             KeyID = rows.user_group_id,
                             rows.name,
                             rows.app_id,
                             rows.t_com_application.application_name,
                             rows.description,
                             rows.is_active,
                             rows.create_date,
                             rows.create_by,
                         };

            if (!isAdmin)
            {
                result = result.Where(w => w.app_id == app_id);
            }

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


        public IQueryable<Property> GetQueryByAppId(string _app_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_com_user_group
                             where rows.is_active == "YES" && rows.app_id == _app_id
                             orderby rows.user_group_id
                             select new Property
                             {
                                 value_member = rows.user_group_id,
                                 display_member = rows.name
                             };

                return result;
            });
        }
    }
}
