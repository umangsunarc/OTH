using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.Entity;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Issue_Cash_Master : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [ForeignKey("AspnetUsers")]
        [Required(ErrorMessage = "user id is required.")]
        public string user_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "cash issued date  is required.")]

        public DateTime cash_issued_date { get; set; }

        [DataMember]
        [Required(ErrorMessage = "issue cash expiry date is required.")]
        public DateTime issue_cash_expiry_date { get; set; }

        [DataMember]
        [Required(ErrorMessage = "transfer_id is required.")]
        [ForeignKey("IssueCashTransferMaster")]
        public int transfer_id { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "Status cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Status cannot be smaller than 3 characters.")]
        public string status { get; set; }


        [DataMember]
        public virtual Issue_Cash_Transfer_Master IssueCashTransferMaster { get; set; }

        [DataMember]
        public virtual AspnetUsers AspnetUsers { get; set; }
    }
}
