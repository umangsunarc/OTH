using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using TrackerEnabledDbContext.Common.Interfaces;

namespace Wollo.Base.DataRepository
{
    public abstract class BaseEntityRepository<T> : IEntityRepository<T> where T : Base.Entity.BaseEntity
    {
        protected IContext Context;
        protected IDbSet<T> Dbset;

        public BaseEntityRepository(IContext context)
        {
            Context = context;
            Dbset = Context.Set<T>();
        }

        public virtual T Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Dbset.Add(entity);
            //Context.SaveChanges(entity.UserId);
            Context.SaveChanges();
            return entity;
        }

        public List<T> Create(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            foreach (var entity in entities)
            {
                Dbset.Add(entity);
            }

            //Context.SaveChanges(entities.FirstOrDefault().UserId);

            Context.SaveChanges();

            return entities;
        }


        public virtual T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Context.Entry(entity).State = EntityState.Modified;
            //Context.SaveChanges(entity.UserId);

            Context.SaveChanges();

            return entity;
        }

        public virtual List<T> Update(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            foreach (var entity in entities)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }

            //Context.SaveChanges(entities.FirstOrDefault().UserId);
            Context.SaveChanges();

            return entities;
        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Dbset.Remove(entity);

            //Context.SaveChanges(entity.UserId);
            Context.SaveChanges();
        }

        public virtual void Delete(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            foreach (var entity in entities)
            {
                Dbset.Remove(entity);
            }

            //Context.SaveChanges(entities.FirstOrDefault().UserId);
            Context.SaveChanges();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Dbset.AsEnumerable();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var query = Context.Set<T>().Where(predicate);
            return query;
        }
    }
}
