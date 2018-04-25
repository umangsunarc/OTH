using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Units_Master : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "unit is required.")]

        public string unit { get; set; }

        [DataMember]
        [Required(ErrorMessage = "stock id is required.")]
        [ForeignKey("StockCode")]
        public int stock_id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "points equivalent is required.")]

        public int points_equivalent { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Lot is required.")]

        public int minimum_lot { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Minimum Rate is required.")]

        public float minimum_rate { get; set; }

        [DataMember]
        public virtual Stock_Code StockCode { get; set; }

    }
}
