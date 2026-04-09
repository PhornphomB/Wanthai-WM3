using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.Providers
{
    public interface IEntityFormCommand<TEntity>
    {
        TEntity Entity { get; set; }

        bool Save();
        bool Update();
        TEntity GetByKeyID(object Id);
        TEntity GetEditKeyID(object Id);
    }


    abstract public class AEntityFormCommand<TEntity> : AGridObjectSourceQuery, IEntityFormCommand<TEntity>, IGridCommand, IDisposable where TEntity : class, new()
    {
        public AEntityFormCommand()
        {
            Entity = new TEntity();
        }

        public virtual bool ValidateSaveNew(TEntity ent, ref string msg_validate)
        {
            return true;
        }
        public virtual bool ValidateSaveUpdate(TEntity ent, ref string msg_validate)
        {
            return true;
        }

        public virtual void SetOptionalSaveNew(TEntity ent) { }
        public virtual void SetOptionalSaveUpdate(TEntity ent) { }

        public TEntity Entity { get; set; }
        protected Func<dynamic> DbSetEntities { get; set; }

        public virtual bool Save()
        {
            var msg_validate = string.Empty;
            var is_valid = ValidateSaveNew(Entity, ref msg_validate);

            if (this.GridObjContext.Engaged(this, () => { return !is_valid; }, msg_validate))
                return false;

            return this.GridObjContext.Save(this, delegate ()
            {
                SetOptionalSaveNew(Entity);

                var keys = this.GridObjContext.GetEntityKeys(Entity.GetType());
                foreach (var key in keys)
                {
                    if (key.TypeName.ToLower() == "guid")
                    {
                        Entity.SetPropertyValue(key.Name, Guid.NewGuid());
                    }
                }

                this.GridObjContext.Entry(Entity).State = System.Data.Entity.EntityState.Added;
                return this.GridObjContext.SaveChanges();
            });
        }

        public virtual bool Update()
        {
            var msg_validate = string.Empty;
            var is_valid = ValidateSaveUpdate(Entity, ref msg_validate);

            if (this.GridObjContext.Engaged(this, () => { return !is_valid; }, msg_validate))
                return false;

            return this.GridObjContext.Update(this, delegate ()
            {
                SetOptionalSaveUpdate(Entity);

                this.GridObjContext.Entry(Entity).State = System.Data.Entity.EntityState.Modified;
                return this.GridObjContext.SaveChanges();
            });
        }

        public virtual TEntity GetByKeyID(object Id)
        {
            return this.GridObjContext.GetDataEntityBy(this, delegate ()
            {
                Entity = DbSetEntities().Find(Id);

                return Entity;
            });
        }

        public virtual TEntity GetEditKeyID(object Id)
        {
            return GetByKeyID(Id);
        }


        #region IGridCommand

        public virtual bool DeleteById(object Id)
        {
           return this.GridObjContext.Delete(this, delegate ()
            {
                try
                {
                    var ent = DbSetEntities().Find(Id);
                    if (ent != null)
                    {
                        this.GridObjContext.Entry(ent).State = System.Data.Entity.EntityState.Deleted;
                    }

                    return this.GridObjContext.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            });
        }

        #endregion
    }
}
