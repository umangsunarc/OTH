using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Wallet_Master : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "user id  is required.")]
        public int user_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "cash transfer id  is required.")]
        [ForeignKey("IssueCashTransferMaster")]
        public int cash_transfer_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "cash issued date is required.")]
        public DateTime cash_issued_date { get; set; }

        [DataMember]
        [Required(ErrorMessage = "cash amount  is required.")]
        public int cash_amount { get; set; }

        [DataMember]
        public virtual Issue_Cash_Transfer_Master IssueCashTransferMaster { get; set; }
    }
}
