using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        PortalContext dbContext;

        public PortalContext Init()
        {
            return dbContext ?? (dbContext = new PortalContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
