using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Wallet_Details : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        [ForeignKey("WalletMaster")]
        public int wallet_id { get; set; }

        [DataMember]
        [Display(Name = "Cash", ResourceType = typeof(Resource))]
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

        //*************************************Label for properties************************************//
        [Display(Name = "CashInCirculationHistory", ResourceType = typeof(Resource))]
        public string cash_in_circulation_history { get; set; }

        [Display(Name = "DashboardCashInCirculationHistory", ResourceType = typeof(Resource))]
        public string dashboard_cash_in_circulation_history { get; set; }
        //*************************************end of properties***************************************//
    }
}
