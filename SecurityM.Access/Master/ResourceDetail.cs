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
    public class ResourceDetail : AEntityFormCommand<t_com_resource_detail>
    {
        public SecurityM_Entities _Model { get; set; }

        public ResourceDetail()
        {
            this._Model = new SecurityM_Entities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_com_resource_detail; };
        }


        #region ++INSTANCE STATIC++
        public static ResourceDetail Instance
        {
            get
            {
                using (ResourceDetail _Instance = new ResourceDetail())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public override void SetOptionalSaveNew(t_com_resource_detail ent)
        {
            ent.resource_detail_id = Guid.NewGuid().ToString();
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _resource_master_id = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_resource_master_id").Value;

            var result = from rows in this._Model.t_com_resource_detail
                         join local in this._Model.t_com_locale
                         on rows.locale_id equals local.locale_id
                         where rows.resource_master_id == _resource_master_id
                         select new
                         {
                             KeyId = rows.resource_detail_id,
                             rows.locale_id,
                             local.name,
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

        #endregion
    }
}
