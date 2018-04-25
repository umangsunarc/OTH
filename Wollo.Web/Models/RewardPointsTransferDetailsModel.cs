using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wollo.Base.Entity;
using Wollo.Base.LocalResource;
using Wollo.Entities.ViewModels;

namespace Wollo.Web.Models
{
    public class RewardPointsTransferDetailsModel : BaseAuditableViewModel
    {
        public List<Reward_Points_Transfer_Details> RewardPointsTransferDetails { get; set; }


        //****************** multi language ********************//
        [Display(Name = "WolloPointsTransferredOutByMember", ResourceType = typeof(Resource))]
        public string wollopointstransferredoutbymember { get; set; }

        [Display(Name = "WolloPointTransferredInByMember", ResourceType = typeof(Resource))]
        public string wollopointtransferredinbymember { get; set; }

        [Display(Name = "Dashboard", ResourceType = typeof(Resource))]
        public string dashboard { get; set; }

    }
}