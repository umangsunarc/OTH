using System.Collections.Generic;
using Wollo.Base.Entity;

namespace Wollo.Base.DataRepository
{
    public interface IEntityRepository<T> : IRepository
        where T : BaseEntity
    {
        T Create(T entity);
        List<T> Create(List<T> entity);
        T Update(T entity);
        List<T> Update(List<T> entity);
        void Delete(T entity);
        IEnumerable<T> GetAll();
    }
}