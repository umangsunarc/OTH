using Wollo.Data.Infrastructure;
using Wollo.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.Entity;

namespace Wollo.Data.Repositories
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T>
            where T : class, IEntity, new()
    {

        private PortalContext dataContext;

        #region Properties
        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected PortalContext DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }
        public EntityBaseRepository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
        }
        #endregion
        public virtual IQueryable<T> GetAll()
        {
            return DbContext.Set<T>().AsNoTracking<T>();
        }
        public virtual IQueryable<T> All
        {
            get
            {
                return GetAll();
            }
        }
        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }
        //public T GetSingle(int id)
        //{
        //    return GetAll().FirstOrDefault(x => x.ID == id);
        //}
        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Where(predicate);
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
            DbContext.Set<T>().Add(entity);
            SaveChanges();
        }
        public virtual void Edit(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
            SaveChanges();
            dbEntityEntry.State = EntityState.Detached;
        }


        public virtual void EditAll(List<T> entities)
        {
            foreach (var t in entities)
            {
                DbContext.Entry<T>(t).State = EntityState.Modified;
                //dbEntityEntry.State = EntityState.Modified;
                //DbContext.Entry<T>.Attach(dbEntityEntry);                 
            }
            SaveChanges();
            
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
            SaveChanges();
        }

        public virtual void SaveChanges()
        {
           DbContext.SaveChanges();
            
        }

        //public DbRawSqlQuery<T> SQLQuery<T>(string sql, params object[] parameters)
        //{
        //    var context = new PortalContext();
        //    return context.Database.SqlQuery<T>(sql, parameters);
        //}
    }
}
