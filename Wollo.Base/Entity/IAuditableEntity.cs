using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Base.Entity
{
    public interface IAuditableEntity
    {
        Nullable<DateTime> created_date { get; set; }

        string created_by { get; set; }

        Nullable<DateTime> updated_date { get; set; }

        string updated_by { get; set; }

    }
}
