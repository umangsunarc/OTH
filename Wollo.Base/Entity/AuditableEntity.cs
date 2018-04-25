using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.LocalResource;

namespace Wollo.Base.Entity
{
    [DataContract]
    public abstract class AuditableEntity : Entity, IAuditableEntity
    {
        [DataMember]
        [DefaultValue("getutcdate()")]
        [Display(Name="DateTime", ResourceType = typeof(Resource))]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> created_date { get; set; }


        [MaxLength(128)]
        [DataMember]
        public string created_by { get; set; }

        [DataMember]
        [DefaultValue("getutcdate()")]
        [Display(Name="DateTime", ResourceType = typeof(Resource))]
        public Nullable<DateTime> updated_date { get; set; }

        [MaxLength(128)]
        [DataMember]
        public string updated_by { get; set; }
    }
}
