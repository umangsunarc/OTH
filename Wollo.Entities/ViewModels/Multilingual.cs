using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Wollo.Base.LocalResource;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wollo.Entities.ViewModels
{
    class Multilingual
    {
        [Display(Name = "Search", ResourceType = typeof(Resource))]
        public string search { get; set; }

        [Display(Name = "Show", ResourceType = typeof(Resource))]
        public string show { get; set; }

        [Display(Name = "Entries", ResourceType = typeof(Resource))]
        public string entries { get; set; }

        [Display(Name = "Trading", ResourceType = typeof(Resource))]
        public string trading { get; set; }

        [Display(Name = "LastTraded", ResourceType = typeof(Resource))]
        public string last_traded { get; set; }

        [Display(Name = "DayHigh", ResourceType = typeof(Resource))]
        public string day_high { get; set; }

        [Display(Name = "PrevClose", ResourceType = typeof(Resource))]
        public string prev_close { get; set; }

        [Display(Name = "TotalVolume", ResourceType = typeof(Resource))]
        public string total_volume { get; set; }

        [Display(Name = "fiftyTwoWeeks", ResourceType = typeof(Resource))]
        public string fiftytwo_Weeks { get; set; }

        [Display(Name = "DayLow", ResourceType = typeof(Resource))]
        public string day_low { get; set; }

        [Display(Name = "Open", ResourceType = typeof(Resource))]
        public string open { get; set; }

        [Display(Name = "NoOfTraded", ResourceType = typeof(Resource))]
        public string no_of_traded { get; set; }

        [Display(Name = "LotSize", ResourceType = typeof(Resource))]
        public string lot_size { get; set; }

        [Display(Name = "Bid", ResourceType = typeof(Resource))]
        public string bid { get; set; }

        [Display(Name = "Ask", ResourceType = typeof(Resource))]
        public string ask { get; set; }

        [Display(Name = "DashboardFundTopup", ResourceType = typeof(Resource))]
        public string dashboard_fund_topup { get; set; }

        [Display(Name = "CashRequest", ResourceType = typeof(Resource))]
        public string cash_request { get; set; }

        [Display(Name = "RequestCash", ResourceType = typeof(Resource))]
        public string request_cash { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public string amount { get; set; }

        [Display(Name = "PointsIssued", ResourceType = typeof(Resource))]
        public string points_issued { get; set; }

        [Display(Name = "IssueNewRewardPoints", ResourceType = typeof(Resource))]
        public string issuenewrewardpoints { get; set; }

        [Display(Name = "Stock", ResourceType = typeof(Resource))]
        public string stock { get; set; }

        [Display(Name = "TransferPoints", ResourceType = typeof(Resource))]
        public string transfer_points { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resource))]
        public string status { get; set; }

        [Display(Name = "IssuerUserName", ResourceType = typeof(Resource))]
        public string Issuer_User_Name { get; set; }

        [Display(Name = "ReceiverUserName", ResourceType = typeof(Resource))]
        public string receiver_user_name { get; set; }

        [Display(Name = "AdministrationFees", ResourceType = typeof(Resource))]
        public string administration_fees { get; set; }

        [Display(Name = "AdministrationFeesOnBuyer", ResourceType = typeof(Resource))]
        public string administration_fees_on_buyer { get; set; }

        [Display(Name = "AdministrationFeesOnSeller", ResourceType = typeof(Resource))]
        public string administration_fees_on_seller { get; set; }

        [Display(Name = "CurrentAdministrationFeesRule", ResourceType = typeof(Resource))]
        public string current_administration_fees_rule { get; set; }

        [Display(Name = "UpdatedOn", ResourceType = typeof(Resource))]
        public string updated_on { get; set; }

        [Display(Name = "RuleOn", ResourceType = typeof(Resource))]
        public string rule_on { get; set; }

        [Display(Name = "Rule", ResourceType = typeof(Resource))]
        public string rule { get; set; }

        [Display(Name = "Buyer", ResourceType = typeof(Resource))]
        public string buyer { get; set; }

        [Display(Name = "Seller", ResourceType = typeof(Resource))]
        public string seller { get; set; }

        [Display(Name = "Fixed", ResourceType = typeof(Resource))]
        public string Fixed { get; set; }

        [Display(Name = "Percent", ResourceType = typeof(Resource))]
        public string percent { get; set; }

        [Display(Name = "WithdrawalFees", ResourceType = typeof(Resource))]
        public string withdrawal_fees { get; set; }

        [Display(Name = "SelectMethod", ResourceType = typeof(Resource))]
        public string select_method { get; set; }

        [Display(Name = "Method", ResourceType = typeof(Resource))]
        public string Method { get; set; }

        [Display(Name = "Manual", ResourceType = typeof(Resource))]
        public string manual { get; set; }

        [Display(Name = "BankTransfer", ResourceType = typeof(Resource))]
        public string bank_transfer { get; set; }

        [Display(Name = "Cheque", ResourceType = typeof(Resource))]
        public string cheque { get; set; }

        [Display(Name = "TradingRules", ResourceType = typeof(Resource))]
        public string trading_rules { get; set; }

        [Display(Name = "Lot", ResourceType = typeof(Resource))]
        public string lot { get; set; }

        [Display(Name = "MinimumLotToBid", ResourceType = typeof(Resource))]
        public string minimum_lot_to_bid { get; set; }

        [Display(Name = "MinimumBidRate", ResourceType = typeof(Resource))]
        public string minimum_bid_rate { get; set; }

        [Display(Name = "CurrentTradingRule", ResourceType = typeof(Resource))]
        public string current_trading_rule { get; set; }

        [Display(Name = "Notifications", ResourceType = typeof(Resource))]
        public string notifications { get; set; }

        [Display(Name = "DashboardNotification", ResourceType = typeof(Resource))]
        public string dashboard_notification { get; set; }

        [Display(Name = "TradingTime", ResourceType = typeof(Resource))]
        public string trading_time { get; set; }

        [Display(Name = "Update", ResourceType = typeof(Resource))]
        public string update { get; set; }

        [Display(Name = "AdminSettings", ResourceType = typeof(Resource))]
        public string admin_settings { get; set; }

        [Display(Name = "MarketRate", ResourceType = typeof(Resource))]
        public string market_rate { get; set; }

        [Display(Name = "InitialMarketRate", ResourceType = typeof(Resource))]
        public string initial_market_rate { get; set; }

        [Display(Name = "ManageUsers", ResourceType = typeof(Resource))]
        public string manage_users { get; set; }

        [Display(Name = "CreateUser", ResourceType = typeof(Resource))]
        public string create_user { get; set; }

        [Display(Name = "User", ResourceType = typeof(Resource))]
        public string user { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string user_name { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Resource))]
        public string first_name { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Resource))]
        public string last_name { get; set; }

        [Display(Name = "EmailAddress", ResourceType = typeof(Resource))]
        public string email_address { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string password { get; set; }

        [Display(Name = "Roles", ResourceType = typeof(Resource))]
        public string roles { get; set; }

        [Display(Name = "RoleName", ResourceType = typeof(Resource))]
        public string role_name { get; set; }

        [Display(Name = "AddNewRole", ResourceType = typeof(Resource))]
        public string add_new_role { get; set; }

        [Display(Name = "RolePermissions", ResourceType = typeof(Resource))]
        public string role_permissions { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string name { get; set; }

        [Display(Name = "Options", ResourceType = typeof(Resource))]
        public string options { get; set; }

        [Display(Name = "EditPermission", ResourceType = typeof(Resource))]
        public string edit_permission { get; set; }

        [Display(Name = "Details", ResourceType = typeof(Resource))]
        public string details { get; set; }

        [Display(Name = "LogDetails", ResourceType = typeof(Resource))]
        public string log_details { get; set; }

        [Display(Name = "URL", ResourceType = typeof(Resource))]
        public string url { get; set; }

        [Display(Name = "IpAddress", ResourceType = typeof(Resource))]
        public string ip_address { get; set; }

        [Display(Name = "UserId", ResourceType = typeof(Resource))]
        public string user_id { get; set; }

        [Display(Name = "Createddate", ResourceType = typeof(Resource))]
        public string created_date { get; set; }

        [Display(Name = "Forapriceof", ResourceType = typeof(Resource))]
        public string for_a_price_of { get; set; }

        [Display(Name = "PriceperLot", ResourceType = typeof(Resource))]
        public string price_per_lot { get; set; }

        [Display(Name = "TotalPoints", ResourceType = typeof(Resource))]
        public string total_points { get; set; }
    }
}
