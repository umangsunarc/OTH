using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class Issue_Points_Transfer_Master : AuditableEntity
    {
        [DataMember]
        [Required(ErrorMessage = "issuer user id is required.")]
        [ForeignKey("IssuerUser")]
        public string issuer_user_id { get; set; }
        [DataMember]
        [Required(ErrorMessage = "receiver user id is required.")]
        [ForeignKey("AspnetUsers")]
        public string receiver_user_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "points issue permission id is required.")]
        public int points_issue_permission_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "points issued action is required.")]
        public int points_issued { get; set; }

        [DataMember]
        [Required(ErrorMessage = "issue points on date action is required.")]
        public DateTime issue_points_on_date { get; set; }
        
        [DataMember]
        public Nullable<int> opening_amount { get; set; }

        [DataMember]
        public Nullable<int> closing_amount { get; set; }

        [DataMember]
        public string description { get; set; }

        public int stockCodeId { get; set; }
        [DataMember]
        public virtual AspnetUsers AspnetUsers { get; set; }

        [DataMember]
        public virtual AspnetUsers IssuerUser { get; set; }

    }
}
