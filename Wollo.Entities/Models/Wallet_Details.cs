using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.Models
{
    [DataContract]
    public class Wallet_Details : AuditableEntity
    {
        [DataMember]
        [ForeignKey("WalletMaster")]
        public int wallet_id { get; set; }

        [DataMember]
        public float cash { get; set; }
        [DataMember]
        public int account_id { get; set; }
        [DataMember]
        [ForeignKey("AspnetRoles")]
        public string role_id { get; set; }
        [DataMember]
        [ForeignKey("AspnetUsers")]
        public string user_id { get; set; }
        [DataMember]
        public bool is_admin { get; set; }

        [DataMember]
        public virtual Wallet_Master WalletMaster { get; set; }

        [DataMember]
        public virtual AspnetUsers AspnetUsers { get; set; }
        [DataMember]
        public virtual AspnetRoles AspnetRoles { get; set; }
    }
}
