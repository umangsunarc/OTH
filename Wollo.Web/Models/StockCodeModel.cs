using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModel= Wollo.Entities.ViewModels;
using Model = Wollo.Entities.Models;
using Wollo.Base.Entity;

namespace Wollo.Web.Models
{
    public class StockCodeModel : BaseAuditableViewModel
    {
        public List<ViewModel.Stock_Code> StockCode { get; set; }
        public List<Model.User> Users { get; set; }
    }
}