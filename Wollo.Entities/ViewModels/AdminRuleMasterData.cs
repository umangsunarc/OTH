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
    [DataContract]
    public class AdminRuleMasterData
    {
        [DataMember]
        public List<Rule_Config_Settings> RuleConfigSettings { get; set; }
        [DataMember]
        public List<Stock_Code> StockCodes { get; set; }
        [DataMember]
        public List<Withdrawl_Fees> WithdrawalFees { get; set; }

        [DataMember]
        public List<Units_Master> UnitMaster { get; set; }

        [DataMember]
        public List<Administration_Fees> AdministrationFees { get; set; }

        //****************************** Label Properties ***********************************************//
        [Display(Name = "Rules", ResourceType = typeof(Resource))]
        public string rules { get; set; }

        [Display(Name = "DashboardRules", ResourceType = typeof(Resource))]
        public string dashboard_rules { get; set; }

        [Display(Name = "WithdrawalFee", ResourceType = typeof(Resource))]
        public string withdrawal_fee { get; set; }

        [Display(Name = "SelectMethod", ResourceType = typeof(Resource))]
        public string select_method { get; set; }

        [Display(Name = "SelectPaymentMethod", ResourceType = typeof(Resource))]
        public string select_payment_method { get; set; }

        [Display(Name = "WithdrawalFees", ResourceType = typeof(Resource))]
        public string withdrawal_fees { get; set; }

        [Display(Name = "SelectRule", ResourceType = typeof(Resource))]
        public string select_rule { get; set; }
 
        [Display(Name = "CurrentRules", ResourceType = typeof(Resource))]
        public string current_rules { get; set; }

        [Display(Name = "Method", ResourceType = typeof(Resource))]
        public string method { get; set; }

        [Display(Name = "Rule", ResourceType = typeof(Resource))]
        public string rule { get; set; }

        [Display(Name = "Fees", ResourceType = typeof(Resource))]
        public string fees { get; set; }

        [Display(Name = "Fee", ResourceType = typeof(Resource))]
        public string fee { get; set; }

        [Display(Name = "Cancel", ResourceType = typeof(Resource))]
        public string cancel { get; set; }

        [Display(Name = "Ok", ResourceType = typeof(Resource))]
        public string ok { get; set; }

        [Display(Name = "AdministrationFee", ResourceType = typeof(Resource))]
        public string administration_fee { get; set; }

        [Display(Name = "SelectStock", ResourceType = typeof(Resource))]
        public string select_stock { get; set; }

        [Display(Name = "SelectStockCode", ResourceType = typeof(Resource))]
        public string select_stock_code { get; set; }

        [Display(Name = "AdminFeeOnBuyer", ResourceType = typeof(Resource))]
        public string administration_fees_on_buyer { get; set; }

        [Display(Name = "AdministrationFeesOnSeller", ResourceType = typeof(Resource))]
        public string administration_fees_on_seller { get; set; }

        [Display(Name = "RuleOn", ResourceType = typeof(Resource))]
        public string rule_on { get; set; }

        [Display(Name = "Buyer", ResourceType = typeof(Resource))]
        public string buyer { get; set; }

        [Display(Name = "Seller", ResourceType = typeof(Resource))]
        public string seller { get; set; }

        [Display(Name = "Stock", ResourceType = typeof(Resource))]
        public string stock { get; set; }

        [Display(Name = "UpdatedOn", ResourceType = typeof(Resource))]
        public string updated_on { get; set; }

        [Display(Name = "TradingRules", ResourceType = typeof(Resource))]
        public string trading_rules { get; set; }

        [Display(Name = "Lot", ResourceType = typeof(Resource))]
        public string lot { get; set; }

        [Display(Name = "MinimumLotToBid", ResourceType = typeof(Resource))]
        public string minimum_lot_to_bid { get; set; }

        [Display(Name = "MinimumRate", ResourceType = typeof(Resource))]
        public string minimum_rate { get; set; }

        [Display(Name = "MinimumLot", ResourceType = typeof(Resource))]
        public string minimum_lot { get; set; }

        [Display(Name = "MinimumBidRate", ResourceType = typeof(Resource))]
        public string minimum_bidding_rate { get; set; }

        [Display(Name = "CurrentAdministrationFeesRule", ResourceType = typeof(Resource))]
        public string current_administration_fees_rule { get; set; }

        [Display(Name = "CurrentTradingRule", ResourceType = typeof(Resource))]
        public string current_trading_rule { get; set; }

        [Display(Name = "BuyerFee", ResourceType = typeof(Resource))]
        public string buyer_fee { get; set; }

        [Display(Name = "SellerFee", ResourceType = typeof(Resource))]
        public string seller_fee { get; set; }

        [Display(Name = "Points", ResourceType = typeof(Resource))]
        public string points { get; set; }

        [Display(Name = "AdministrationFeeOnBuyer", ResourceType = typeof(Resource))]
        public string administration_fee_on_buyer { get; set; }

        [Display(Name = "CurrentWithdrawalFeeRule", ResourceType = typeof(Resource))]
        public string current_withdrawal_fee_rule { get; set; }
        [Display(Name = "Select", ResourceType = typeof(Resource))]
        public string select { get; set; }

        //****************************** Label Properties ***********************************************//

    }
}
