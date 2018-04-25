using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Entities.Models
{
    public class Dashboard
    {
        public int? total_cash_in_circulation { get; set; }
        public int? total_topup_by_members { get; set; }
        public int? total_withdrawal_by_member { get; set; }
        public int? total_cash_issued_to_member { get; set; }
        public int? total_cash_issued_to_system { get; set; }
        public int? total_reward_points_in_circulation { get; set; }
        public int? total_reward_points_transferred_in_by_members { get; set; }
        public int? total_reward_points_transferred_out_by_member { get; set; }
        public int? total_issue_points_in_circulation { get; set; }
        public int? total_issue_points_issued_to_member { get; set; }
        public int? total_issue_points_issued_to_system { get; set; }
        public int reward_points { get; set; }
        public int issue_points { get; set; }
        public int cash { get; set; }
    }
}
