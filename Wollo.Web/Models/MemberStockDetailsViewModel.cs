using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wollo.Entities.ViewModels;
using Wollo.Base.Entity;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.LocalResource;

namespace Wollo.Web.Models
{
    public class MemberStockDetailsViewModel:BaseAuditableViewModel
    {
        public List<Member_Stock_Details> MemberStockDetails { get; set; }


        [Display(Name = "Dashboard", ResourceType = typeof(Resource))]
        public string dashboard { get; set; }

    }
}