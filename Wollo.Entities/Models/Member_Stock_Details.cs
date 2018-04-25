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
    public class Member_Stock_Details : AuditableEntity
    {
        [DataMember]
        [ForeignKey("AspnetUsers")]
        public string user_id { get; set; }
        [DataMember]
        [ForeignKey("AspnetRoles")]
        public string role_id { get; set; }
        [DataMember] 
        public string email { get; set; }
        [DataMember]
        public int stock_amount { get; set; }
        [DataMember]
        public int account_id { get; set; }
        [DataMember]
        [ForeignKey("StockCode")]
        public int stock_code_id { get; set; }

        [DataMember]
        public virtual Stock_Code StockCode { get; set; }
        [DataMember]
        public virtual AspnetUsers AspnetUsers { get; set; }
        [DataMember]
        public virtual AspnetRoles AspnetRoles { get; set; }
    }
}
