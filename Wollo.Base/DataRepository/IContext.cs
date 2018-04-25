using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Wollo.Base.DataRepository
{
    public interface IContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        int SaveChanges();
    }
}