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
    public class IssuePointTransferDetailViewModel :BaseAuditableViewModel
    {
        public List<Issue_Point_Transfer_Detail> IssueCashTransferDetails { get; set; }

        [Display(Name = "IssuePointsIssuedintoSystem", ResourceType = typeof(Resource))]
        public string issuepointsissuedintosystem { get; set; }

        [Display(Name = "IssuePointsIssuedToMembers", ResourceType = typeof(Resource))]
        public string issuepointsissuedtomembers { get; set; }

         [Display(Name = "Dashboard", ResourceType = typeof(Resource))]
        public string dashboard { get; set; }

        

        
    }
}