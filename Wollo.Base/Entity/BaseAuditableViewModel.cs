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
    public class BaseAuditableViewModel
    {
        [DataMember]
        [Display(Name = "DateTime", ResourceType = typeof(Resource))]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> created_date { get; set; }

        [MaxLength(128)]
        [DataMember]
        public string created_by { get; set; }

        [DataMember]
        [Display(Name = "DateTime", ResourceType = typeof(Resource))]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> updated_date { get; set; }

        [MaxLength(128)]
        [DataMember]
        public string updated_by { get; set; }

        //******************************* Display properties*************************************************//
        //[Display(Name = "", ResourceType = typeof(Resource))]
        //public string language { get; set; }
        [DataMember]
        [Display(Name = "AccountSettings", ResourceType = typeof(Resource))]
        public string layout_account_settings { get; set; }
        [DataMember]
        [Display(Name = "LogOut", ResourceType = typeof(Resource))]
        public string layout_logout { get; set; }
        [DataMember]
        [Display(Name = "Dashboard", ResourceType = typeof(Resource))]
        public string layout_dashboard { get; set; }
        [DataMember]
        [Display(Name = "MenuTrade", ResourceType = typeof(Resource))]
        public string layout_trade { get; set; }
        [DataMember]
        [Display(Name = "Portfolio", ResourceType = typeof(Resource))]
        public string layout_portfolio { get; set; }
        [DataMember]
        [Display(Name = "Trading", ResourceType = typeof(Resource))]
        public string layout_trading { get; set; }
        [DataMember]
        [Display(Name = "RewardPointsTransfer", ResourceType = typeof(Resource))]
        public string layout_reward_point_transfer { get; set; }
        [DataMember]
        [Display(Name = "TransferPoints", ResourceType = typeof(Resource))]
        public string layout_transfer_points { get; set; }
        [DataMember]
        [Display(Name = "History", ResourceType = typeof(Resource))]
        public string layout_history { get; set; }
        [DataMember]
        [Display(Name = "Cash", ResourceType = typeof(Resource))]
        public string layout_cash { get; set; }
        [DataMember]
        [Display(Name = "CashIssue", ResourceType = typeof(Resource))]
        public string layout_cash_issue { get; set; }
        [DataMember]
        [Display(Name = "CashTransactionHistory", ResourceType = typeof(Resource))]
        public string layout_cash_transaction_history { get; set; }
        [DataMember]
        [Display(Name = "FundTopup", ResourceType = typeof(Resource))]
        public string layout_fund_topup { get; set; }
        [DataMember]
        [Display(Name = "Withdrawal", ResourceType = typeof(Resource))]
        public string layout_withdrawal { get; set; }
        [DataMember]
        [Display(Name = "RequestForCash", ResourceType = typeof(Resource))]
        public string layout_request_for_cash { get; set; }
        [DataMember]
        [Display(Name = "CashRequestApproval", ResourceType = typeof(Resource))]
        public string layout_cash_request_approval { get; set; }
        [DataMember]
        [Display(Name = "Points", ResourceType = typeof(Resource))]
        public string layout_points { get; set; }
        [DataMember]
        [Display(Name = "Member", ResourceType = typeof(Resource))]
        public string layout_member { get; set; }
        [DataMember]
        [Display(Name = "Self", ResourceType = typeof(Resource))]
        public string layout_self { get; set; }
        [DataMember]
        [Display(Name = "", ResourceType = typeof(Resource))]
        public string layout_points_request_approval { get; set; }
        [DataMember]
        [Display(Name = "Rules", ResourceType = typeof(Resource))]
        public string layout_rules { get; set; }
        [DataMember]
        [Display(Name = "Notifications", ResourceType = typeof(Resource))]
        public string layout_notifications { get; set; }
        [DataMember]
        [Display(Name = "AdminSettings", ResourceType = typeof(Resource))]
        public string layout_admin_settings { get; set; }
        [DataMember]
        [Display(Name = "MarketRate", ResourceType = typeof(Resource))]
        public string layout_market_rate { get; set; }
        [DataMember]
        [Display(Name = "TradingTime", ResourceType = typeof(Resource))]
        public string layout_trading_time { get; set; }
        [DataMember]
        [Display(Name = "UserandRoleManagement", ResourceType = typeof(Resource))]
        public string layout_user_and_role_management { get; set; }
        [DataMember]
        [Display(Name = "ManageUsers", ResourceType = typeof(Resource))]
        public string layout_manage_users { get; set; }
        [DataMember]
        [Display(Name = "Roles", ResourceType = typeof(Resource))]
        public string layout_roles { get; set; }
        [DataMember]
        [Display(Name = "RolePermissions", ResourceType = typeof(Resource))]
        public string layout_role_permission { get; set; }
        [DataMember]
        [Display(Name = "LogDetails", ResourceType = typeof(Resource))]
        public string layout_log_details { get; set; }

        //**************************************** Display properties end ******************************************//
        

        //**************************************** Validation properties start *************************************//
        [Display(Name = "Email_Required", ResourceType = typeof(Resource))]
        public string Email_Required { get; set; }
        [Display(Name = "FirstName_Required", ResourceType = typeof(Resource))]
        public string FirstName_Required { get; set; }
        [Display(Name = "LastName_Required", ResourceType = typeof(Resource))]
        public string LastName_Required { get; set; }
        [Display(Name = "UName_Required", ResourceType = typeof(Resource))]
        public string UName_Required { get; set; }
        [Display(Name = "IfYouDontHaveAnAccount", ResourceType = typeof(Resource))]
        public string IfYouDontHaveAnAccount { get; set; }
        [Display(Name = "UserAgreementLabel", ResourceType = typeof(Resource))]
        public string UserAgreementLabel { get; set; }
        [Display(Name = "PleaseEnterBidAmount", ResourceType = typeof(Resource))]
        public string PleaseEnterBidAmount { get; set; }
        [Display(Name = "PleaseMakeABidWithLotGreaterThanZero", ResourceType = typeof(Resource))]
        public string PleaseMakeABidWithLotGreaterThanZero { get; set; }
        [Display(Name = "NotEnoughAmountForBid", ResourceType = typeof(Resource))]
        public string NotEnoughAmountForBid { get; set; }
        [Display(Name = "AmountPriceYouEnteredIsInvalid", ResourceType = typeof(Resource))]
        public string AmountPriceYouEnteredIsInvalid { get; set; }
        [Display(Name = "PleaseEnterAskAmount", ResourceType = typeof(Resource))]
        public string PleaseEnterAskAmount { get; set; }
        [Display(Name = "PleaseMakeAnAskWithLotGreaterThanZero", ResourceType = typeof(Resource))]
        public string PleaseMakeAnAskWithLotGreaterThanZero { get; set; }
        [Display(Name = "TransactionCancelledSuccessfully", ResourceType = typeof(Resource))]
        public string TransactionCancelledSuccessfully { get; set; }
        [Display(Name = "YourAskHasBeenPlacedSuccessfully", ResourceType = typeof(Resource))]
        public string YourAskHasBeenPlacedSuccessfully { get; set; }
        [Display(Name = "YourBidHasBeenPlacedSuccessfully", ResourceType = typeof(Resource))]
        public string YourBidHasBeenPlacedSuccessfully { get; set; }
        [Display(Name = "YourAskHasBeenUpdatedSuccessfully", ResourceType = typeof(Resource))]
        public string YourAskHasBeenUpdatedSuccessfully { get; set; }
        [Display(Name = "YourBidHasBeenUpdatedSuccessfully", ResourceType = typeof(Resource))]
        public string YourBidHasBeenUpdatedSuccessfully { get; set; }
        [Display(Name = "TheTransferAmountShouldBeGreaterThanZero", ResourceType = typeof(Resource))]
        public string TheTransferAmountShouldBeGreaterThanZero { get; set; }
        [Display(Name = "PleaseSelectTheDateRange", ResourceType = typeof(Resource))]
        public string PleaseSelectTheDateRange { get; set; }
        [Display(Name = "PleaseSelectEndDate", ResourceType = typeof(Resource))]
        public string PleaseSelectEndDate { get; set; }
        [Display(Name = "PleaseSelectToDate", ResourceType = typeof(Resource))]
        public string PleaseSelectToDate { get; set; }
        [Display(Name = "MakeSureYouHaveSelectedPaymentMethodAndProvidedAmount", ResourceType = typeof(Resource))]
        public string MakeSureYouHaveSelectedPaymentMethodAndProvidedAmount { get; set; }
        [Display(Name = "WithdrawalAddedSuccessfully", ResourceType = typeof(Resource))]
        public string WithdrawalAddedSuccessfully { get; set; }
        [Display(Name = "TopupAddedSuccessfully", ResourceType = typeof(Resource))]
        public string TopupAddedSuccessfully { get; set; }
        [Display(Name = "SomethingWentWrong", ResourceType = typeof(Resource))]
        public string SomethingWentWrong { get; set; }
        [Display(Name = "WithdrawalUpdatedSuccessfully", ResourceType = typeof(Resource))]
        public string WithdrawalUpdatedSuccessfully { get; set; }
        [Display(Name = "LogDetPersonalInformationHasBeenUpdatedSuccessfullyails", ResourceType = typeof(Resource))]
        public string PersonalInformationHasBeenUpdatedSuccessfully { get; set; }
        [Display(Name = "IssuedPointsCannotBeModified", ResourceType = typeof(Resource))]
        public string IssuedPointsCannotBeModified { get; set; }
        [Display(Name = "IssuedPointsCannotBeCancelled", ResourceType = typeof(Resource))]
        public string IssuedPointsCannotBeCancelled { get; set; }
        [Display(Name = "MakesureyouprovidedCorrectDetails", ResourceType = typeof(Resource))]
        public string MakesureyouprovidedCorrectDetails { get; set; }
        [Display(Name = "YouDontHaveEnoughBalanceToIssueCash", ResourceType = typeof(Resource))]
        public string YouDontHaveEnoughBalanceToIssueCash { get; set; }
        [Display(Name = "YourRequestHasBeenSentToSuperAdminForApproval", ResourceType = typeof(Resource))]
        public string YourRequestHasBeenSentToSuperAdminForApproval { get; set; }
        [Display(Name = "YouDoNotHaveSufficientStockAmountToPerformThisAction", ResourceType = typeof(Resource))]
        public string YouDoNotHaveSufficientStockAmountToPerformThisAction { get; set; }
        [Display(Name = "MakeSureYouHaveProvidedDataForAllFields", ResourceType = typeof(Resource))]
        public string MakeSureYouHaveProvidedDataForAllFields { get; set; }
        [Display(Name = "StatusUpdatedSuccessfully", ResourceType = typeof(Resource))]
        public string StatusUpdatedSuccessfully { get; set; }
        [Display(Name = "CashIssuedSuccessfully", ResourceType = typeof(Resource))]
        public string CashIssuedSuccessfully { get; set; }
        [Display(Name = "PointsIssuedSuccessfully", ResourceType = typeof(Resource))]
        public string PointsIssuedSuccessfully { get; set; }
        [Display(Name = "NotificationAddedSuccessfully", ResourceType = typeof(Resource))]
        public string NotificationAddedSuccessfully { get; set; }
        [Display(Name = "NotificationUpdatedSuccessfully", ResourceType = typeof(Resource))]
        public string NotificationUpdatedSuccessfully { get; set; }
        [Display(Name = "ANewUserHasBeenCreated", ResourceType = typeof(Resource))]
        public string ANewUserHasBeenCreated { get; set; }
        [Display(Name = "UserHasBeenDeactivatedSuccessfully", ResourceType = typeof(Resource))]
        public string UserHasBeenDeactivatedSuccessfully { get; set; }
        [Display(Name = "PasswordUpdatedSuccessfully", ResourceType = typeof(Resource))]
        public string PasswordUpdatedSuccessfully { get; set; }
        [Display(Name = "ThePasswordMustBeAtLeastSixCharactersLong", ResourceType = typeof(Resource))]
        public string ThePasswordMustBeAtLeastSixCharactersLong { get; set; }
        [Display(Name = "ConfirmPasswordAndPasswordDoNotMatch", ResourceType = typeof(Resource))]
        public string ConfirmPasswordAndPasswordDoNotMatch { get; set; }
        [Display(Name = "SuperAdminRoleIsAlreadyAssigned", ResourceType = typeof(Resource))]
        public string SuperAdminRoleIsAlreadyAssigned { get; set; }
        [Display(Name = "UserHasBeenActivatedSuccessfully", ResourceType = typeof(Resource))]
        public string UserHasBeenActivatedSuccessfully { get; set; }
        [Display(Name = "InvalidUsernameOrPassword", ResourceType = typeof(Resource))]
        public string InvalidUsernameOrPassword { get; set; }
        [Display(Name = "ThisUserHasBeenDeactivatedByAdmin", ResourceType = typeof(Resource))]
        public string ThisUserHasBeenDeactivatedByAdmin { get; set; }
        [Display(Name = "PhoneNumberIsRequired", ResourceType = typeof(Resource))]
        public string PhoneNumberIsRequired { get; set; }
        [Display(Name = "Address_Required", ResourceType = typeof(Resource))]
        public string Address_Required { get; set; }
        [Display(Name = "City_Required", ResourceType = typeof(Resource))]
        public string City_Required { get; set; }
        [Display(Name = "Zip_Required", ResourceType = typeof(Resource))]
        public string Zip_Required { get; set; }
        [Display(Name = "UserWithThisEmailAddressDoesNotExist", ResourceType = typeof(Resource))]
        public string UserWithThisEmailAddressDoesNotExist { get; set; }
        [Display(Name = "ResetLinkSent", ResourceType = typeof(Resource))]
        public string ResetLinkSent { get; set; }
        [Display(Name = "SelfRequestForCash", ResourceType = typeof(Resource))]
        public string SelfRequestForCash { get; set; }
        [Display(Name = "PointsIssuedToMembers", ResourceType = typeof(Resource))]
        public string PointsIssuedToMembers { get; set; }
        [Display(Name = "SelfRequestForPoints", ResourceType = typeof(Resource))]
        public string SelfRequestForPoints { get; set; }
        //**************************************** Validation properties end *************************************//
        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public string Amount { get; set; }

        [Display(Name = "NewHolidayClosingDate", ResourceType = typeof(Resource))]
        public string NewHolidayClosingDate { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Resource))]
        public string date { get; set; }

        [Display(Name = "NotifyBefore", ResourceType = typeof(Resource))]
        public string NotifyBefore { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resource))]
        public string Description { get; set; }

        [Display(Name = "User", ResourceType = typeof(Resource))]
        public string user { get; set; }


        //**************************************** Add by umang properties   *************************************//





    }
}
