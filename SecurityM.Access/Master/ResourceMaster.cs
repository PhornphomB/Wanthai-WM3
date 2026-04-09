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
    public class ResourceMaster : AGridObjectSourceQuery, IEntityCommandForm
    {
        public SecurityM_Entities _Model { get; set; }

        public ResourceMaster()
        {
            this._Model = new SecurityM_Entities();

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static ResourceMaster Instance
        {
            get
            {
                using (ResourceMaster _Instance = new ResourceMaster())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Inherit IEntityCommandForm

        public bool Save(object _objectEntity)
        {
            var ent = (t_com_resource_master)_objectEntity;

            return this._Model.Save(this, delegate()
            {
                ent.resource_master_id = Guid.NewGuid().ToString();
                ent.create_date = DateTime.Now;

                this._Model.t_com_resource_master.Add(ent);

                return this._Model.SaveChanges();
            });
        }

        public bool Update(object _objectEntity)
        {
            var ent = (t_com_resource_master)_objectEntity;

            return this._Model.Update(this, delegate()
            {
                return this._Model.SaveChanges();
            });
        }

        public object GetByKeyID(object Id)
        {
            return GetEditKeyID(Id);
        }

        public object GetEditKeyID(object Id)
        {
            var realId = Id.ToString();

            return this._Model.GetDataEntityBy<t_com_resource_master>(this, delegate()
            {
                var ent = (from rows in this._Model.t_com_resource_master
                           where rows.resource_master_id == realId
                           select rows).FirstOrDefault();
                return ent;
            });
        }

        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _resource_master_id = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_resource_master_id").Value;

            var result = from rows in this._Model.t_com_resource_master
                         where rows.resource_master_id == _resource_master_id
                         select new
                         {
                             KeyId = rows.resource_master_id,
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


        public bool DeleteByKey(string Id)
        {
            return this._Model.Update(this, delegate()
            {
                var result = this._Model.t_com_resource_master.FirstOrDefault(qry => qry.resource_master_id == Id);
                if (result != null)
                {
                    this._Model.t_com_resource_master.Remove(result);
                }

                return this._Model.SaveChanges();
            });
        }

        public object GetDataByMenuID(object Id)
        {
            var realId = Id.ToString();

            return this._Model.GetDataEntityBy<t_com_resource_master>(this, delegate()
            {
                var ent = (from rows in this._Model.t_com_resource_master
                           where rows.resource_name == realId
                           select rows).FirstOrDefault();
                return ent;
            });
        }
    }
}
