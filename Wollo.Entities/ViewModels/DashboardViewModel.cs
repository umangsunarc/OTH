using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.Entity;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    public class DashboardViewModel
    {
        public List<BaseAuditableViewModel> BaseAuditableViewModel { get; set; }
        [Display(Name = "CashInCirculation", ResourceType = typeof(Resource))]
        public int? total_cash_in_circulation { get; set; }
        [Display(Name = "TotalTopupByMembers", ResourceType = typeof(Resource))]
        public int? total_topup_by_members { get; set; }
        [Display(Name = "TotalWithdrawOutByMembers", ResourceType = typeof(Resource))]
        public int? total_withdrawal_by_member { get; set; }
        [Display(Name = "CashIssuedToMembers", ResourceType = typeof(Resource))]
        public int? total_cash_issued_to_member { get; set; }
        [Display(Name = "TotalCashIssuedIntoSystem", ResourceType = typeof(Resource))]
        public int? total_cash_issued_to_system { get; set; }
        [Display(Name = "WolloPointInCirculation", ResourceType = typeof(Resource))]
        public int? total_reward_points_in_circulation { get; set; }
        [Display(Name = "WolloPointTransferredInByMember", ResourceType = typeof(Resource))]
        public int? total_reward_points_transferred_in_by_members { get; set; }
        [Display(Name = "WolloPointTransferredOutByMember", ResourceType = typeof(Resource))]
        public int? total_reward_points_transferred_out_by_member { get; set; }

        [Display(Name = "RewardPointsIssuedToSystem", ResourceType = typeof(Resource))]
        public int? total_reward_points_issued_into_system { get; set; }

        [Display(Name = "RewardPointsIssuedToMember", ResourceType = typeof(Resource))]
        public int? total_reward_points_issued_to_member { get; set; }

        [Display(Name = "RPE02PointInCirculation", ResourceType = typeof(Resource))]
        public int? total_issue_points_in_circulation { get; set; }
        [Display(Name = "RPE02PointIssuedToMember", ResourceType = typeof(Resource))]
        public int? total_issue_points_issued_to_member { get; set; }
        [Display(Name = "RPE02PointIssuedInsystem", ResourceType = typeof(Resource))]
        public int? total_issue_points_issued_to_system { get; set; }
        [Display(Name = "TestPointsTransferredInByMembers", ResourceType = typeof(Resource))]
        public int? total_issue_points_transferred_in_by_members { get; set; }
        [Display(Name = "TestPointsTransferredOutByMembers", ResourceType = typeof(Resource))]
        public int? total_issue_points_transferred_out_by_member { get; set; }
        public int reward_points { get; set; }
        public int issue_points { get; set; }
        [Display(Name = "Cash", ResourceType = typeof(Resource))]
        public float cash { get; set; }

        [Display(Name = "SystemCash", ResourceType = typeof(Resource))]
        public float system_cash { get; set; }
        public string rpe_stockcode { get; set; }
        public string rpe_stockname { get; set; }
        public string test_stockcode { get; set; }
        public string test_stockname { get; set; }
        public HolidayData HolidayData { get; set; }
        public LayoutViewModel LayoutViewModel { get; set; }

        //*****************************Properties for labels******************************************//
        [Display(Name = "Welcome", ResourceType = typeof(Resource))]
        public string welcome { get; set; }

        [Display(Name = "Dashboard", ResourceType = typeof(Resource))]
        public string dashboard { get; set; }

        [Display(Name = "ViewDetails", ResourceType = typeof(Resource))]
        public string view_details { get; set; }

        [Display(Name = "NoticesAndMaintenance", ResourceType = typeof(Resource))]
        public string notification { get; set; }

        [Display(Name = "Cash", ResourceType = typeof(Resource))]
        public string cash_data { get; set; }

        [Display(Name = "WolloPointDetails", ResourceType = typeof(Resource))]
        public string reward_points_details { get; set; }

        [Display(Name = "RPE02PointDetails", ResourceType = typeof(Resource))]
        public string issue_point_details { get; set; }

        [Display(Name = "ViewAllCashHistory", ResourceType = typeof(Resource))]
        public string cash_history { get; set; }

        [Display(Name = "ViewAllTrading", ResourceType = typeof(Resource))]
        public string trading_history { get; set; }

        [Display(Name = "TotalCashIssuedIntoSystem", ResourceType = typeof(Resource))]
        public string total_cash_issued_into_system { get; set; }

        [Display(Name = "WolloPointsInCirculation", ResourceType = typeof(Resource))]
        public string wollo_points_in_circulation { get; set; }

        //[Display(Name = "WolloPointsInCirculation", ResourceType = typeof(Resource))]
        //public string wollo_points_in_circulation { get; set; }

        [Display(Name = "WolloPointsTransferredInByMember", ResourceType = typeof(Resource))]
        public string wollo_points_transferred_in_by_member { get; set; }

        [Display(Name = "WolloPointsTransferredOutByMember", ResourceType = typeof(Resource))]
        public string wollo_points_transferred_out_by_member { get; set; }

        [Display(Name = "RewardPointsIssuedToSystem", ResourceType = typeof(Resource))]
        public string reward_points_issued_to_system { get; set; }

        [Display(Name = "RewardPointsIssuedToMember", ResourceType = typeof(Resource))]
        public string reward_points_issued_to_member { get; set; }

        [Display(Name = "RPE02PointsInCirculation", ResourceType = typeof(Resource))]
        public string RPE02_points_in_circulation { get; set; }

        [Display(Name = "RPE02PointsTransferredInByMembers", ResourceType = typeof(Resource))]
        public string RPE02_points_transferred_in_by_members { get; set; }

        [Display(Name = "RPE02PointsTransferredOutByMembers", ResourceType = typeof(Resource))]
        public string RPE02_points_transferred_out_by_members { get; set; }

        [Display(Name = "RPE02PointsIssuedIntosystem", ResourceType = typeof(Resource))]
        public string RPE02_points_issued_into_system { get; set; }

        [Display(Name = "RPE02PointsIssuedToMember", ResourceType = typeof(Resource))]
        public string RPE02_points_issued_to_member { get; set; }

        [Display(Name = "WolloPointDetails", ResourceType = typeof(Resource))]
        public string wollo_point_details { get; set; }

        [Display(Name = " RPE02PointDetails", ResourceType = typeof(Resource))]
        public string RPE02_point_details { get; set; }

        [Display(Name = "ViewAllHistory", ResourceType = typeof(Resource))]
        public string view_all_history { get; set; }

        [Display(Name = "WolloPointsIssuedToSystem", ResourceType = typeof(Resource))]
        public string wollo_points_issued_to_system { get; set; }

        [Display(Name = "WolloPointsIssuedToMembers", ResourceType = typeof(Resource))]
        public string wollo_points_issued_to_members { get; set; }


        //*****************************Properties for labels******************************************//
        public DashboardViewModel()
        {
            BaseAuditableViewModel = new List<BaseAuditableViewModel>();
        }
    }
}
