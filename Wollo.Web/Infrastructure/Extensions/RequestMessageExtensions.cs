using Wollo.Data.Repositories;
using Wollo.Entities;
//using Wollo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Dependencies;

namespace Wollo.Web.Infrastructure.Extensions
{
    public static class RequestMessageExtensions
    {
        //internal static IMembershipService GetMembershipService(this HttpRequestMessage request)
        //{
        //    return request.GetService<IMembershipService>();
        //}

        internal static IEntityBaseRepository<T> GetDataRepository<T>(this HttpRequestMessage request) where T : class, IEntityBase, new()
        {
            return request.GetService<IEntityBaseRepository<T>>();
        }

        private static TService GetService<TService>(this HttpRequestMessage request)
        {
            IDependencyScope dependencyScope = request.GetDependencyScope();
            TService service = (TService)dependencyScope.GetService(typeof(TService));

            return service;
        }
    }
}