using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wollo.Entities.ViewModels;
using Wollo.Base.Entity;


namespace Wollo.Web.Models
{
    public class CashTransactionDetailViewModel : BaseAuditableViewModel
    {
        public List<Cash_Transaction_Detail> CashTransactionDetails { get; set; }
    }
}