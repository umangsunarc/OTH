using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wollo.Entities.ViewModels
{
    public class LayoutViewModel
    {
        [Display(Name = "", ResourceType = typeof(Resource))]
        public string language { get; set; }
        [Display(Name = "AccountSettings", ResourceType = typeof(Resource))]
        public string account_settings { get; set; }
        [Display(Name = "LogOut", ResourceType = typeof(Resource))]
        public string logout { get; set; }
        [Display(Name = "Dashboard", ResourceType = typeof(Resource))]
        public string dashboard { get; set; }
        [Display(Name = "Trade", ResourceType = typeof(Resource))]
        public string trade { get; set; }
        [Display(Name = "Portfolio", ResourceType = typeof(Resource))]
        public string portfolio { get; set; }
        [Display(Name = "Trading", ResourceType = typeof(Resource))]
        public string trading { get; set; }
        [Display(Name = "RewardPointsTransfer", ResourceType = typeof(Resource))]
        public string reward_point_transfer { get; set; }
        [Display(Name = "TransferPoints", ResourceType = typeof(Resource))]
        public string transfer_points { get; set; }
        [Display(Name = "History", ResourceType = typeof(Resource))]
        public string history { get; set; }
        [Display(Name = "Cash", ResourceType = typeof(Resource))]
        public string cash { get; set; }
        [Display(Name = "CashIssue", ResourceType = typeof(Resource))]
        public string cash_issue { get; set; }
        [Display(Name = "CashTransactionHistory", ResourceType = typeof(Resource))]
        public string cash_transaction_history { get; set; }
        [Display(Name = "FundTopup", ResourceType = typeof(Resource))]
        public string fund_topup { get; set; }
        [Display(Name = "Withdrawal", ResourceType = typeof(Resource))]
        public string withdrawal { get; set; }
        [Display(Name = "RequestForCash", ResourceType = typeof(Resource))]
        public string request_for_cash { get; set; }
        [Display(Name = "CashRequestApproval", ResourceType = typeof(Resource))]
        public string cash_request_approval { get; set; }
        [Display(Name = "Points", ResourceType = typeof(Resource))]
        public string points { get; set; }
        [Display(Name = "Member", ResourceType = typeof(Resource))]
        public string member { get; set; }
        [Display(Name = "Self", ResourceType = typeof(Resource))]
        public string self { get; set; }
        [Display(Name = "", ResourceType = typeof(Resource))]
        public string points_request_approval { get; set; }
        [Display(Name = "Rules", ResourceType = typeof(Resource))]
        public string rules { get; set; }
        [Display(Name = "Notifications", ResourceType = typeof(Resource))]
        public string notifications { get; set; }
        [Display(Name = "AdminSettings", ResourceType = typeof(Resource))]
        public string admin_settings { get; set; }
        [Display(Name = "MarketRate", ResourceType = typeof(Resource))]
        public string market_rate { get; set; }
        [Display(Name = "TradingTime", ResourceType = typeof(Resource))]
        public string trading_time { get; set; }
        [Display(Name = "UserandRoleManagement", ResourceType = typeof(Resource))]
        public string user_and_role_management { get; set; }
        [Display(Name = "ManageUsers", ResourceType = typeof(Resource))]
        public string manage_users { get; set; }
        [Display(Name = "Roles", ResourceType = typeof(Resource))]
        public string roles { get; set; }
        [Display(Name = "RolePermissions", ResourceType = typeof(Resource))]
        public string role_permission { get; set; }
        [Display(Name = "LogDetails", ResourceType = typeof(Resource))]
        public string log_details { get; set; }

        //****************************** Validation properties *****************************************//
        //IfYouDontHaveAnAccount
        //    Email_Required
        //    FirstName_Required
        //    LastName_Required
        //    UName_Required
        //    UserAgreementLabel
        //    PleaseEnterBidAmount
        //    PleaseMakeABidWithLotGreaterThanZero
        //    NotEnoughAmountForBid
        //    AmountPriceYouEnteredIsInvalid
        //    PleaseEnterAskAmount
        //    PleaseMakeAnAskWithLotGreaterThanZero
        //    TransactionCancelledSuccessfully
        //    YourAskHasBeenPlacedSuccessfully
        //    YourBidHasBeenPlacedSuccessfully
        //    YourAskHasBeenUpdatedSuccessfully
        //    YourBidHasBeenUpdatedSuccessfully
        //    TheTransferAmountShouldBeGreaterThanZero
        //    PleaseSelectTheDateRange
        //    PleaseSelectEndDate
        //    PleaseSelectToDate
        //    MakeSureYouHaveSelectedPaymentMethodAndProvidedAmount
        //    WithdrawalAddedSuccessfully
        //    TopupAddedSuccessfully
        //    SomethingWentWrong
        //    WithdrawalUpdatedSuccessfully
        //    PersonalInformationHasBeenUpdatedSuccessfully
        //    IssuedPointsCannotBeModified
        //    IssuedPointsCannotBeCancelled
        //    MakesureyouprovidedCorrectDetails
        //    YouDontHaveEnoughBalanceToIssueCash
        //    NoFeesAvailableForThisMethod
        //    YourRequestHasBeenSentToSuperAdminForApproval
        //    YouDoNotHaveSufficientStockAmountToPerformThisAction
        //    MakeSureYouHaveProvidedDataForAllFields
        //    RuleCreatedSuccessfully
        //    StatusUpdatedSuccessfully
        //    CashIssuedSuccessfully
        //    PointsIssuedSuccessfully
        //    NotificationAddedSuccessfully
        //    NotificationUpdatedSuccessfully
        //    ANewUserHasBeenCreated
        //    UserHasBeenDeactivatedSuccessfully
        //    PasswordUpdatedSuccessfully
        //    ThePasswordMustBeAtLeastSixCharactersLong
        //    ConfirmPasswordAndPasswordDoNotMatch
        //    SuperAdminRoleIsAlreadyAssigned
        //    UserHasBeenActivatedSuccessfully
        //    InvalidUsernameOrPassword
        //    ThisUserHasBeenDeactivatedByAdmin
        //    PhoneNumberIsRequired
        //    Address_Required
        //    City_Required
        //    Zip_Required
        //    UserWithThisEmailAddressDoesNotExist
        //    ResetLinkSent
        //    SelfRequestForCash
        //    PointsIssuedToMembers
        //    SelfRequestForPoints

    }
}
