using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Issue_Points_Transfer_Master : BaseAuditableViewModel
    {
        [DataMember]
        [Display(Name = "ID", ResourceType = typeof(Resource))]
        public int id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "issuer user id is required.")]
        [ForeignKey("IssuerUser")]
        [Display(Name = "IssuerUserID", ResourceType = typeof(Resource))]
        public string issuer_user_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "receiver user id is required.")]
        [ForeignKey("AspnetUsers")]
        [Display(Name = "ReceiverUserID", ResourceType = typeof(Resource))]

        public string receiver_user_id { get; set; }
        [DataMember]
        [Display(Name = "ReceiverUserName", ResourceType = typeof(Resource))]

        public string receiver_user_name { get; set; }
        [DataMember]
        [Required(ErrorMessage = "points issue permission id is required.")]
        [Display(Name = "PointsIssuePermissionID", ResourceType = typeof(Resource))]
        public int points_issue_permission_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "points issued action is required.")]
        [Display(Name = "PointsIssued", ResourceType = typeof(Resource))]
        public int points_issued { get; set; }

        [DataMember]
        [Required(ErrorMessage = "issue points on date action is required.")]
        [Display(Name= "DateTime", ResourceType = typeof(Resource))]
        public DateTime issue_points_on_date { get; set; }

        [DataMember]
        [Display(Name = "StockCodeID", ResourceType = typeof(Resource))]
        public int stockCodeId { get; set; }

        [DataMember]
        [Display(Name = "OpeningAmount", ResourceType = typeof(Resource))]
        public Nullable<int> opening_amount { get; set; }

        [DataMember]
        [Display(Name = "ClosingAmount", ResourceType = typeof(Resource))]
        public Nullable<int> closing_amount { get; set; }

        [DataMember]
        [Display(Name = "Description", ResourceType = typeof(Resource))]
        public string description { get; set; }

        [DataMember]
        [Display(Name = "AspnetUsers", ResourceType = typeof(Resource))]
        public virtual AspnetUsers AspnetUsers { get; set; }


        [DataMember]
        
        public virtual AspnetUsers IssuerUser { get; set; }
    }
}
