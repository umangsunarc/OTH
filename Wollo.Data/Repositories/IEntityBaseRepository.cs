using Wollo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.Entity;
using System.Data.Entity.Infrastructure;

namespace Wollo.Data.Repositories
{
    //public interface IEntityBaseRepository { }

    public interface IEntityBaseRepository<T>  where T : class, IEntity, new()
    {
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> All { get; }
        IQueryable<T> GetAll();
        //T GetSingle(int id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void EditAll(List<T> entities);
        void SaveChanges();

        //DbRawSqlQuery<T> SQLQuery<T>(string sql, params object[] parameters)
    }
}
