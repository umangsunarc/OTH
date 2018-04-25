using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Entities.ViewModels;
using Wollo.Base.LocalResource;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.Entity;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class AdminIsuuePointsViewModel
    {
        [DataMember]
        public List<Issue_Points_Transfer_Master> IssuePointsTransferMaster { get; set; }
        [DataMember]
        public List<Topup_Status_Master> TopupStatusMaster { get; set; }
        [DataMember]
        public List<Issue_Withdrawel_Permission_Master> IssueWithdrawelPermissionMaster { get; set; }
        [DataMember]
        public CommonWordsViewModel CommonWordsViewModel { get; set; } 
        [Display(Name = "AdminName", ResourceType = typeof(Resource))]
        public string admin_name { get; set; }

        [Display(Name = "Actions", ResourceType = typeof(Resource))]
        public string actions { get; set; }

        [Display(Name = "ChangeStatus", ResourceType = typeof(Resource))]
        public string change_status { get; set; }

        [Display(Name = "Cancel", ResourceType = typeof(Resource))]
        public string cancel { get; set; }

        [Display(Name = "Confirm", ResourceType = typeof(Resource))]
        public string confirm { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string user_name { get; set; }
        public AdminIsuuePointsViewModel()
        {
            IssuePointsTransferMaster = new List<Issue_Points_Transfer_Master>();
            TopupStatusMaster = new List<Topup_Status_Master>();
        }

        //******************************Label for properties*************************//
        [Display(Name = "DashboardPointIssueRequest", ResourceType = typeof(Resource))]
        public string dashboard_point_issue_request { get; set; }
        //******************************end of properties***************************//
    }
}
