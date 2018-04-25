using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Wollo.Base.LocalResource;

namespace Wollo.Base.Entity
{
    [DataContract]
    public class LayoutViewModel
    {
        //******************************* Display properties*************************************************//
        //[Display(Name = "", ResourceType = typeof(Resource))]
        //public string language { get; set; }
        [Display(Name = "AccountSettings", ResourceType = typeof(Resource))]
        public string layout_account_settings { get; set; }
        [Display(Name = "LogOut", ResourceType = typeof(Resource))]
        public string layout_logout { get; set; }
        [Display(Name = "Dashboard", ResourceType = typeof(Resource))]
        public string layout_dashboard { get; set; }
        [Display(Name = "MenuTrade", ResourceType = typeof(Resource))]
        public string layout_trade { get; set; }
        [Display(Name = "Portfolio", ResourceType = typeof(Resource))]
        public string layout_portfolio { get; set; }
        [Display(Name = "Trading", ResourceType = typeof(Resource))]
        public string layout_trading { get; set; }
        [Display(Name = "RewardPointsTransfer", ResourceType = typeof(Resource))]
        public string layout_reward_point_transfer { get; set; }
        [Display(Name = "TransferPoints", ResourceType = typeof(Resource))]
        public string layout_transfer_points { get; set; }
        [Display(Name = "History", ResourceType = typeof(Resource))]
        public string layout_history { get; set; }
        [Display(Name = "Cash", ResourceType = typeof(Resource))]
        public string layout_cash { get; set; }
        [Display(Name = "CashIssue", ResourceType = typeof(Resource))]
        public string layout_cash_issue { get; set; }
        [Display(Name = "CashTransactionHistory", ResourceType = typeof(Resource))]
        public string layout_cash_transaction_history { get; set; }
        [Display(Name = "FundTopup", ResourceType = typeof(Resource))]
        public string layout_fund_topup { get; set; }
        [Display(Name = "Withdrawal", ResourceType = typeof(Resource))]
        public string layout_withdrawal { get; set; }
        [Display(Name = "RequestForCash", ResourceType = typeof(Resource))]
        public string layout_request_for_cash { get; set; }
        [Display(Name = "CashRequestApproval", ResourceType = typeof(Resource))]
        public string layout_cash_request_approval { get; set; }
        [Display(Name = "Points", ResourceType = typeof(Resource))]
        public string layout_points { get; set; }
        [Display(Name = "Member", ResourceType = typeof(Resource))]
        public string layout_member { get; set; }
        [Display(Name = "Self", ResourceType = typeof(Resource))]
        public string layout_self { get; set; }
        [Display(Name = "", ResourceType = typeof(Resource))]
        public string layout_points_request_approval { get; set; }
        [Display(Name = "Rules", ResourceType = typeof(Resource))]
        public string layout_rules { get; set; }
        [Display(Name = "Notifications", ResourceType = typeof(Resource))]
        public string layout_notifications { get; set; }
        [Display(Name = "AdminSettings", ResourceType = typeof(Resource))]
        public string layout_admin_settings { get; set; }
        [Display(Name = "MarketRate", ResourceType = typeof(Resource))]
        public string layout_market_rate { get; set; }
        [Display(Name = "TradingTime", ResourceType = typeof(Resource))]
        public string layout_trading_time { get; set; }
        [Display(Name = "UserandRoleManagement", ResourceType = typeof(Resource))]
        public string layout_user_and_role_management { get; set; }
        [Display(Name = "ManageUsers", ResourceType = typeof(Resource))]
        public string layout_manage_users { get; set; }
        [Display(Name = "Roles", ResourceType = typeof(Resource))]
        public string layout_roles { get; set; }
        [Display(Name = "RolePermissions", ResourceType = typeof(Resource))]
        public string layout_role_permission { get; set; }
        [Display(Name = "LogDetails", ResourceType = typeof(Resource))]
        public string layout_log_details { get; set; }
    }
}
