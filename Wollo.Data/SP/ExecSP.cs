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
    public class ExecSP : IExecSP
           
    {
        private PortalContext dataContext;
        public DbRawSqlQuery<T> SQLQuery<T>(string sql, params object[] parameters)
        {
            var context = new PortalContext();
            return context.Database.SqlQuery<T>(sql, parameters);
        }
    }
}
