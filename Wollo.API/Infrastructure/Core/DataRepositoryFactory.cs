using Wollo.Data.Repositories;
using Wollo.Web.Infrastructure.Extensions;
using System.Net.Http;
using Wollo.Base.Entity;


namespace Wollo.API.Infrastructure.Core
{
    public class DataRepositoryFactory : IDataRepositoryFactory
    {
        public IEntityBaseRepository<T> GetDataRepository<T>(HttpRequestMessage request) where T :  Entity, new()
        {
            return request.GetDataRepository<T>();
        }
    }

    public interface IDataRepositoryFactory
    {
        IEntityBaseRepository<T> GetDataRepository<T>(HttpRequestMessage request) where T : Entity, new();
    }
}