using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wollo.Entities.ViewModels;
using Wollo.Base.Entity;
using System.ComponentModel.DataAnnotations;

namespace Wollo.Web.Models
{
    public class IssueCashTransferMasterModel : BaseAuditableViewModel
    {
        public List<Issue_Cash_Transfer_Master> IssueCashTransferMaster { get; set; }

        
    }

     
}