using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
     [DataContract]
    class Issue_Points_Details : BaseAuditableViewModel
    {
         [DataMember]
         [Display(Name = "ID", ResourceType = typeof(Resource))]
         public int id { get; set; }
        
        [DataMember]
        [Required(ErrorMessage = "account id is required.")]
        [Display(Name = "AccountID", ResourceType = typeof(Resource))]
        public int account_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "points is required.")]
        [Display(Name = "Points", ResourceType = typeof(Resource))]
        public int points { get; set; }
    }
}
