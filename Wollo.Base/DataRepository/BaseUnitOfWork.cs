using System;
using TrackerEnabledDbContext.Common.Interfaces;

namespace Wollo.Base.DataRepository
{
    public class BaseUnitOfWork : IDisposable
    {
        protected IContext Context = null;
        protected bool IsDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed && disposing)
            {
                Context.Dispose();
            }

            this.IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
