using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Withdrawel_History_Master : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }
    }
}
