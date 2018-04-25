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
    public class Issue_Cash_Transfer_Master : AuditableEntity
    {

        [DataMember]
        [ForeignKey("IssuerUser")]
        public string issuer_account_id { get; set; }
        [DataMember]
        [ForeignKey("ReceiverUser")]
        public string receiver_account_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "cash issued on date is required.")]

        public DateTime cash_issued_on_date { get; set; }

        [DataMember]
        [Required(ErrorMessage = "cash issue permission id is required.")]
        [ForeignKey("IssueWithdrawelPermissionMaster")]
        public int cash_issue_permission_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "cash issued is required.")]

        public float cash_issued { get; set; }
        [DataMember]
        public bool is_admin { get; set; }
        [DataMember]
        public virtual Issue_Withdrawel_Permission_Master IssueWithdrawelPermissionMaster { get; set; }
        [DataMember]
        public virtual AspnetUsers IssuerUser { get; set; }
        [DataMember]
        public virtual AspnetUsers ReceiverUser { get; set; }
    }
}
