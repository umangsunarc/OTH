using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wollo.Entities.ViewModels;
using Wollo.Base.Entity;


namespace Wollo.Web.Models
{
    public class WithdrawelHistoryDetailViewModel:BaseAuditableViewModel
    {
        public List<Withdrawel_History_Details> WithdrawelHistoryDetails { get; set; }
    }
}