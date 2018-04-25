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
    class Company_Stock_Relation_Master
    {
         [DataMember]
         public int id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "company id is required.")]
        [ForeignKey("CompanyCode")]
        public int company_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "stock id is required.")]
        [ForeignKey("StockCode")]
        public int stock_id { get; set; }

        [DataMember]
        public virtual Company_Code CompanyCode { get; set; }

        [DataMember]
        public virtual Stock_Code StockCode { get; set; }
    }
}
