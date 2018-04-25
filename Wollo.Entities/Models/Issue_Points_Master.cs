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
    public class Issue_Points_Master : AuditableEntity
    {
        [DataMember]
        [ForeignKey("AspnetUsers")]
        [Required(ErrorMessage = "user id is required.")]
        public string user_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "points issued date  is required.")]

        public DateTime points_issued_date { get; set; }

        [DataMember]
        [Required(ErrorMessage = "issue points expiry date is required.")]
        public DateTime issue_points_expiry_date { get; set; }

        [DataMember]
        [ForeignKey("StockCode")]
        public int stock_code_id { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "Status cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Status cannot be smaller than 3 characters.")]
        public string status { get; set; }

        [DataMember]
        [Required(ErrorMessage = "transfer_id is required.")]
        [ForeignKey("IssuePointsTransferMaster")]
        public int transfer_id { get; set; }
        [DataMember]
        public virtual Issue_Points_Transfer_Master IssuePointsTransferMaster { get; set; }
        [DataMember]
        public virtual Stock_Code StockCode { get; set; }
        [DataMember]
        public virtual AspnetUsers AspnetUsers { get; set; }
    }
}
