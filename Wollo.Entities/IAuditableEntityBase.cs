using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities
{
    public interface IAuditableEntityBase
    {
        Nullable<DateTime> created_date { get; set; }

        string created_by { get; set; }

        Nullable<DateTime> updated_date { get; set; }

        string updated_by { get; set; }
    }
}
