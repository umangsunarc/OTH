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

    public interface IExecSP
    {
        DbRawSqlQuery<T> SQLQuery<T>(string sql, params object[] parameters);
    }
}
