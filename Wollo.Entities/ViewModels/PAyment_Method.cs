using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.ViewModels
{
    public class Payment_Method : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string method { get; set; }
    }
}
