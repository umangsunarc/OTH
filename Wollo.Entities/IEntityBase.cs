using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities
{
    public interface IEntityBase
    {
        int ID { get; set; }
    }
}
