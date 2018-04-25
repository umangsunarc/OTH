using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wollo.Entities.ViewModels;
using Wollo.Base.Entity;

namespace Wollo.Web.Models
{
    public class Trading : BaseAuditableViewModel
    {
        public QueueTradingViewModel QueueTradingViewModel { get; set; }
    }
}