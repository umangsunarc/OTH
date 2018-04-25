using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using Wollo.API.Infrastructure.Core;
using Wollo.Common.AutoMapper;
using Wollo.Data.Infrastructure;
using Wollo.Data.Repositories;
using Wollo.Entities;
using Wollo.Entities.ViewModels;
using Models = Wollo.Entities.Models;

namespace Wollo.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CashController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Models.Wallet_Details> _walletDetailsRepository;
        private readonly IEntityBaseRepository<Models.Transaction_History> _transactionRepository;
        private readonly IEntityBaseRepository<Models.Topup_Status_Master> _topupStatusMasterRepository;
        private readonly IEntityBaseRepository<Models.Topup_History> _topupHistoryRepository;
        private readonly IEntityBaseRepository<Models.Cash_Transaction_History> _cashTransactionHistoryRepository;
        private readonly IEntityBaseRepository<Models.Transfer_Action_Master> _transferActionMasterHistoryRepository;
        private readonly IEntityBaseRepository<Models.Issue_Cash_Transfer_Master> _issueCashTransferMasterRepository;
        private readonly IEntityBaseRepository<Models.Withdrawel_History_Details> _withdrawelHistoryDetailsRepository;
        private readonly IEntityBaseRepository<Models.Issue_Points_Transfer_Master> _issuePointsTransferMasterHistoryDetailsRepository;
        private readonly IEntityBaseRepository<Models.Issue_Point_Transfer_Detail> _issuePointsTransferDetailHistoryDetailsRepository;
        private readonly IEntityBaseRepository<Models.Stock_Code> _stockCodeDetailsRepository;
        private readonly IEntityBaseRepository<Models.Cash_Transaction_Detail> _cashTransactionDetailHistoryRepository;
        private readonly IEntityBaseRepository<Models.Member_Stock_Details> _memberStockDetailsRepository;
        private readonly IEntityBaseRepository<Models.Withdrawl_Status_Master> _withdrawlStatusMasterRepository;
        private readonly IEntityBaseRepository<Models.Withdrawl_Fees> _withdrawlFeesRepository;
        private readonly IEntityBaseRepository<Models.Withdrawel_History_Master> _withdrawelHistoryMasterRepository;
        private readonly IEntityBaseRepository<Models.Reward_Points_Transfer_Details> _rewardPointsTransferDetailsRepository;
        private readonly IEntityBaseRepository<Models.Issue_Withdrawel_Permission_Master> _issueWithdrawelPermissionMasterRepository;
        private readonly IEntityBaseRepository<Models.User> _userRepository;
        public CashController(IEntityBaseRepository<Models.Wallet_Details> walletDetailsRepository,
            IEntityBaseRepository<Models.Transaction_History> transactionRepository,
            IEntityBaseRepository<Models.Topup_Status_Master> topupStatusMasterRepository,
            IEntityBaseRepository<Models.Topup_History> topupHistoryRepository,
            IEntityBaseRepository<Models.Issue_Points_Transfer_Master> issuePointsTransferMasterHistoryDetailsRepository,
            IEntityBaseRepository<Models.Issue_Cash_Transfer_Master> issueCashTransferMasterRepository,
            IEntityBaseRepository<Models.Stock_Code> stockCodeDetailsRepository,
            IEntityBaseRepository<Models.Issue_Point_Transfer_Detail> issuePointsTransferDetailHistoryDetailsRepository,
            IEntityBaseRepository<Models.Member_Stock_Details> memberStockDetailsRepository,
            IEntityBaseRepository<Models.Reward_Points_Transfer_Details> rewardPointsTransferDetailsRepository,
            IEntityBaseRepository<Models.Cash_Transaction_History> cashTransactionHistoryRepository,
            IEntityBaseRepository<Models.Withdrawel_History_Details> withdrawelHistoryDetailsRepository,
            IEntityBaseRepository<Models.Withdrawl_Status_Master> withdrawlStatusMasterRepository,
            IEntityBaseRepository<Models.Transfer_Action_Master> transferActionMasterHistoryRepository,
            IEntityBaseRepository<Models.Withdrawl_Fees> withdrawlFeesRepository,
            IEntityBaseRepository<Models.User> userRepository,
            IEntityBaseRepository<Models.Withdrawel_History_Master> withdrawelHistoryMasterRepository,
            IEntityBaseRepository<Models.Cash_Transaction_Detail> cashTransactionDetailHistoryRepository,
            IEntityBaseRepository<Models.Issue_Withdrawel_Permission_Master> issueWithdrawelPermissionMasterRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _walletDetailsRepository = walletDetailsRepository;
            _transactionRepository = transactionRepository;
            _topupStatusMasterRepository = topupStatusMasterRepository;
            _topupHistoryRepository = topupHistoryRepository;
            _transferActionMasterHistoryRepository = transferActionMasterHistoryRepository;
            _cashTransactionHistoryRepository = cashTransactionHistoryRepository;
            _withdrawelHistoryDetailsRepository = withdrawelHistoryDetailsRepository;
            _withdrawlStatusMasterRepository = withdrawlStatusMasterRepository;
            _withdrawlFeesRepository = withdrawlFeesRepository;
            _issuePointsTransferMasterHistoryDetailsRepository = issuePointsTransferMasterHistoryDetailsRepository;
            _issuePointsTransferDetailHistoryDetailsRepository = issuePointsTransferDetailHistoryDetailsRepository;
            _memberStockDetailsRepository = memberStockDetailsRepository;
            _stockCodeDetailsRepository = stockCodeDetailsRepository;
            _rewardPointsTransferDetailsRepository = rewardPointsTransferDetailsRepository;
            _withdrawelHistoryMasterRepository = withdrawelHistoryMasterRepository;
            _issueWithdrawelPermissionMasterRepository = issueWithdrawelPermissionMasterRepository;
            _issueCashTransferMasterRepository = issueCashTransferMasterRepository;
            _cashTransactionDetailHistoryRepository = cashTransactionDetailHistoryRepository;
            _userRepository = userRepository;
        }

        //for cash withdrawal
        [HttpPost]
        public HttpResponseMessage RangeFilter(HttpRequestMessage request, Withdrawel_History_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                WithdrawalData objWithdrawalData = new WithdrawalData();
                List<Withdrawel_History_Details> lstWithdrawalHistoryDetails = new List<Withdrawel_History_Details>();
                lstWithdrawalHistoryDetails = AutoMapperHelper.GetInstance().Map<List<Withdrawel_History_Details>>(_withdrawelHistoryDetailsRepository.GetAll().ToList());
                lstWithdrawalHistoryDetails.ToList().ForEach(c => c.WithdrawelHistoryMaster = new Withdrawel_History_Master
                {
                    created_date = c.WithdrawelHistoryMaster.updated_date.HasValue ? c.WithdrawelHistoryMaster.updated_date.Value.ToLocalTime() : c.WithdrawelHistoryMaster.updated_date,
                    updated_date = c.WithdrawelHistoryMaster.updated_date.HasValue ? c.WithdrawelHistoryMaster.updated_date.Value.ToLocalTime() : c.WithdrawelHistoryMaster.updated_date,
                    updated_by = c.WithdrawelHistoryMaster.updated_by,
                    created_by = c.WithdrawelHistoryMaster.created_by,
                    id = c.WithdrawelHistoryMaster.id
                });
                if (master.created_date == master.updated_date)
                {
                    objWithdrawalData.lstWithdrawalHistoryDeatils = lstWithdrawalHistoryDetails.Where(x => x.WithdrawelHistoryMaster.created_date.Value.Date == master.created_date.Value.Date).ToList();
                }

                else
                {
                    objWithdrawalData.lstWithdrawalHistoryDeatils = lstWithdrawalHistoryDetails.Where(x => x.WithdrawelHistoryMaster.created_date.Value.Date >= master.created_date.Value.Date && x.WithdrawelHistoryMaster.created_date.Value.Date <= master.updated_date.Value.Date).ToList();
                }
                //objWithdrawalData.lstWithdrawalHistoryDeatils = lstWithdrawalHistoryDetails.Where(x => x.WithdrawelHistoryMaster.created_date >= master.created_date && x.WithdrawelHistoryMaster.created_date <= master.updated_date).ToList();
                List<Withdrawl_Status_Master> lstWithdrawalStatusMaster = new List<Withdrawl_Status_Master>();
                lstWithdrawalStatusMaster = AutoMapperHelper.GetInstance().Map<List<Withdrawl_Status_Master>>(_withdrawlStatusMasterRepository.GetAll().ToList());
                objWithdrawalData.lstWithdrawalStatusMaster = lstWithdrawalStatusMaster;
                response = request.CreateResponse<WithdrawalData>(HttpStatusCode.OK, objWithdrawalData);
                return response;

            });


        }
        //code ends here

        //for cash transaction range filter
        [HttpPost]
        public HttpResponseMessage RangeFilter1(HttpRequestMessage request, Cash_Transaction_History master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Cash_Transaction_History> lstCashTransactionHistory = AutoMapperHelper.GetInstance().Map<List<Cash_Transaction_History>>(_cashTransactionHistoryRepository.GetAll().ToList());
                lstCashTransactionHistory = lstCashTransactionHistory.Select(x => new Wollo.Entities.ViewModels.Cash_Transaction_History
                {
                    id = x.id,
                    created_date = x.description.ToLower() != "administration fee" && x.description.ToLower() != "ask completed" && x.description.ToLower() != "bid completed" ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.description.ToLower() != "administration fee" && x.description.ToLower() != "ask completed" && x.description.ToLower() != "bid completed" ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    user_id = x.user_id,
                    description = x.description,
                    opening_cash = x.opening_cash,
                    closing_cash = x.closing_cash,
                    transaction_amount = x.transaction_amount,
                    AspNetUsers = x.AspNetUsers
                }).ToList();
                if (master.created_date == master.updated_date)
                {
                    lstCashTransactionHistory = lstCashTransactionHistory.Where(x => x.created_date.Value.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    lstCashTransactionHistory = lstCashTransactionHistory.Where(x => x.created_date.Value.Date >= master.created_date.Value.Date && x.created_date.Value.Date <= master.updated_date.Value.Date).ToList();

                }
                //lstCashTransactionHistory = lstCashTransactionHistory.Where(x => x.created_date >= master.created_date && x.created_date <= master.updated_date).ToList();
                response = request.CreateResponse<List<Cash_Transaction_History>>(HttpStatusCode.OK, lstCashTransactionHistory);
                return response;
            });
        }
        //code ends here

        //for fund-topup range filter
        //[HttpPost]
        public HttpResponseMessage RangeFilter2(HttpRequestMessage request, Topup_History master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                TopUpDetails objTopUpDetails = new TopUpDetails();

                objTopUpDetails.TopupStatusMaster = AutoMapperHelper.GetInstance().Map<List<Topup_Status_Master>>(_topupStatusMasterRepository.GetAll().ToList());
                objTopUpDetails.Topups = AutoMapperHelper.GetInstance().Map<List<Topup_History>>(_topupHistoryRepository.GetAll().ToList());
                objTopUpDetails.Topups = objTopUpDetails.Topups.Select(x => new Wollo.Entities.ViewModels.Topup_History
                {
                    id = x.id,
                    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    user_id = x.user_id,
                    TopupStatusMaster = x.TopupStatusMaster,
                    amount = x.amount,
                    AspnetUsers = x.AspnetUsers,
                    details = x.details,
                    topup_status_id = x.topup_status_id,
                    payment_method = x.payment_method
                }).ToList();

                //cash fund-topup
                if (master.created_date == master.updated_date)
                {
                    objTopUpDetails.Topups = objTopUpDetails.Topups.Where(x => x.created_date.Value.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    objTopUpDetails.Topups = objTopUpDetails.Topups.Where(x => x.created_date.Value.Date >= master.created_date.Value.Date && x.created_date.Value.Date <= master.updated_date.Value.Date).ToList();

                }
                //objTopUpDetails.Topups = objTopUpDetails.Topups.Where(x => x.created_date >= master.created_date && x.created_date <= master.updated_date).ToList();
                response = request.CreateResponse<TopUpDetails>(HttpStatusCode.OK, objTopUpDetails);
                return response;

            });
        }

        // filter cash admin
        //[HttpPost]
        //public HttpResponseMessage RangeFilter3(HttpRequestMessage request, Issue_Cash_Transfer_Master master)
        //{
        //    return CreateHttpResponse(request, () =>
        //    {
        //        HttpResponseMessage response = null;
        //        List<Issue_Cash_Transfer_Master> lstWithdrawalHistoryDetails = new List<Issue_Cash_Transfer_Master>();
        //        lstWithdrawalHistoryDetails = AutoMapperHelper.GetInstance().Map<List<Issue_Cash_Transfer_Master>>(_issueCashTransferMasterRepository.GetAll().ToList());
        //        if (master.created_date == master.updated_date)
        //        {
        //            lstWithdrawalHistoryDetails = lstWithdrawalHistoryDetails.Where(x => x.created_date.Value.Date == master.created_date.Value.Date).ToList();
        //        }
        //        else
        //        {
        //            lstWithdrawalHistoryDetails = lstWithdrawalHistoryDetails.Where(x => x.created_date.Value.Date >= master.created_date.Value.Date && x.created_date.Value.Date <= master.updated_date.Value.Date).ToList();
        //        }
        //        lstWithdrawalHistoryDetails = lstWithdrawalHistoryDetails.Select(x => new Wollo.Entities.ViewModels.Issue_Cash_Transfer_Master
        //        {
        //            id = x.id,
        //            created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
        //            updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
        //            created_by = x.created_by,
        //            updated_by = x.updated_by,
        //            issuer_account_id = x.issuer_account_id,
        //            receiver_account_id = x.receiver_account_id,
        //            ReceiverUser = x.ReceiverUser,
        //            cash_issue_permission_id = x.cash_issue_permission_id,
        //            cash_issued = x.cash_issued,
        //            IssuerUser = x.IssuerUser,
        //            IssueWithdrawelPermissionMaster = x.IssueWithdrawelPermissionMaster,
        //            cash_issued_on_date = x.cash_issued_on_date,
        //        }).ToList();
        //        response = request.CreateResponse<List<Issue_Cash_Transfer_Master>>(HttpStatusCode.OK, lstWithdrawalHistoryDetails);
        //        return response;

        //    });
        //}


        // filter cash admin
        [HttpPost]
        public HttpResponseMessage RangeFilter3(HttpRequestMessage request, Issue_Cash_Transfer_Master master, string userId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Issue_Cash_Transfer_Master> lstWithdrawalHistoryDetails = new List<Issue_Cash_Transfer_Master>();
                lstWithdrawalHistoryDetails = AutoMapperHelper.GetInstance().Map<List<Issue_Cash_Transfer_Master>>(_issueCashTransferMasterRepository.GetAll().Where(x => x.receiver_account_id == userId).ToList());
                if (master.created_date == master.updated_date)
                {
                    lstWithdrawalHistoryDetails = lstWithdrawalHistoryDetails.Where(x => x.created_date.Value.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    lstWithdrawalHistoryDetails = lstWithdrawalHistoryDetails.Where(x => x.created_date.Value.Date >= master.created_date.Value.Date && x.created_date.Value.Date <= master.updated_date.Value.Date).ToList();
                }
                lstWithdrawalHistoryDetails = lstWithdrawalHistoryDetails.Select(x => new Wollo.Entities.ViewModels.Issue_Cash_Transfer_Master
                {
                    id = x.id,
                    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    issuer_account_id = x.issuer_account_id,
                    receiver_account_id = x.receiver_account_id,
                    ReceiverUser = x.ReceiverUser,
                    cash_issue_permission_id = x.cash_issue_permission_id,
                    cash_issued = x.cash_issued,
                    IssuerUser = x.IssuerUser,
                    IssueWithdrawelPermissionMaster = x.IssueWithdrawelPermissionMaster,
                    cash_issued_on_date = x.cash_issued_on_date,
                }).ToList();
                response = request.CreateResponse<List<Issue_Cash_Transfer_Master>>(HttpStatusCode.OK, lstWithdrawalHistoryDetails);
                return response;

            });
        }

        //filter issue cash
        [HttpPost]
        public HttpResponseMessage RangeFilter4(HttpRequestMessage request, Cash_Transaction_History master, string userId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Cash_Transaction_History> objCashDetails = new List<Cash_Transaction_History>();
                string superAdminId = ConfigurationManager.AppSettings["SuperAdminUserId"];
                objCashDetails = AutoMapperHelper.GetInstance().Map<List<Cash_Transaction_History>>(_cashTransactionHistoryRepository.GetAll().ToList());
                if (userId != superAdminId)
                {
                    objCashDetails = objCashDetails.Where(x => x.description.ToLower().Trim() == "cash issued to member by admin" || x.description.ToLower().Trim() == "cash issued to admin by superadmin").ToList();
                }
                else
                {
                    objCashDetails = objCashDetails.Where(x => x.description.ToLower() != "cash issued to self by superadmin" && (x.description.ToLower().Trim() == "cash issued to member by admin" || x.description.ToLower().Trim() == "cash issued to admin by superadmin" || x.description.ToLower().Trim() == "cash issued to member by superadmin")).ToList();
                }
                objCashDetails = objCashDetails.Select(x => new Wollo.Entities.ViewModels.Cash_Transaction_History
                {
                    id = x.id,
                    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    user_id = x.user_id,
                    description = x.description,
                    opening_cash = x.opening_cash,
                    closing_cash = x.closing_cash,
                    transaction_amount = x.transaction_amount,
                    AspNetUsers = x.AspNetUsers
                }).ToList();
                if (master.created_date == master.updated_date)
                {
                    objCashDetails = objCashDetails.Where(x => x.created_date.Value.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    objCashDetails = objCashDetails.Where(x => x.created_date.Value.Date >= master.created_date.Value.Date && x.created_date.Value.Date <= master.updated_date.Value.Date).ToList();
                }
                //objCashDetails = objCashDetails.Where(x => x.created_date >= master.created_date && x.created_date <= master.updated_date).ToList();
                response = request.CreateResponse<List<Cash_Transaction_History>>(HttpStatusCode.OK, objCashDetails);
                return response;
            });
        }

        //for cash request approval
        //[HttpPost]
        public HttpResponseMessage RangeFilter5(HttpRequestMessage request, Issue_Withdrawel_Permission_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Approve_Admin_Cash objApproveAdminCash = new Approve_Admin_Cash();
                List<Issue_Cash_Transfer_Master> objCashDetails = new List<Issue_Cash_Transfer_Master>();
                objCashDetails = AutoMapperHelper.GetInstance().Map<List<Issue_Cash_Transfer_Master>>(_issueCashTransferMasterRepository.GetAll().ToList());
                //List<Issue_Withdrawel_Permission_Master> objpermissionDetails = new List<Issue_Withdrawel_Permission_Master>();
                //objpermissionDetails = AutoMapperHelper.GetInstance().Map<List<Issue_Withdrawel_Permission_Master>>(_issueWithdrawelPermissionMasterRepository.GetAll().ToList());
                objApproveAdminCash.IssueCashTransferMaster = objCashDetails;
                //ApproveAdminCash.IssueWithdrawelTransferMaster = objpermissionDetails;
                objApproveAdminCash.IssueCashTransferMaster = objApproveAdminCash.IssueCashTransferMaster.Select(x => new Wollo.Entities.ViewModels.Issue_Cash_Transfer_Master
                {
                    id = x.id,
                    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    issuer_account_id = x.issuer_account_id,
                    receiver_account_id = x.receiver_account_id,
                    ReceiverUser = x.ReceiverUser,
                    cash_issue_permission_id = x.cash_issue_permission_id,
                    cash_issued = x.cash_issued,
                    IssuerUser = x.IssuerUser,
                    IssueWithdrawelPermissionMaster = x.IssueWithdrawelPermissionMaster,
                    cash_issued_on_date = x.cash_issued_on_date,
                }).ToList();
                if (master.created_date == master.updated_date)
                {
                    objApproveAdminCash.IssueCashTransferMaster = objApproveAdminCash.IssueCashTransferMaster.Where(x => x.created_date.Value.Date == master.created_date).ToList();
                }
                else
                {
                    objApproveAdminCash.IssueCashTransferMaster = objApproveAdminCash.IssueCashTransferMaster.Where(x => x.created_date.Value.Date >= master.created_date.Value.Date && x.created_date.Value.Date <= master.updated_date.Value.Date).ToList();
                }
                //objApproveAdminCash.IssueCashTransferMaster = objApproveAdminCash.IssueCashTransferMaster.Where(x => x.created_date >= master.created_date && x.created_date <= master.updated_date).ToList();
                response = request.CreateResponse<Approve_Admin_Cash>(HttpStatusCode.OK, objApproveAdminCash);
                return response;
            });
        }

        /// <summary>
        /// Get all cash transaction history by user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllCashTransactionsByUser(HttpRequestMessage request, string userId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Cash_Transaction_History> lstCashTransactionHistory = AutoMapperHelper.GetInstance().Map<List<Cash_Transaction_History>>(_cashTransactionHistoryRepository.FindBy(x => x.user_id == userId).ToList());
                response = request.CreateResponse<List<Cash_Transaction_History>>(HttpStatusCode.OK, lstCashTransactionHistory);
                return response;
            });
        }


        [HttpGet]
        public HttpResponseMessage GetStringTest(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                response = request.CreateResponse(HttpStatusCode.OK, "Test String OK");
                return response;
            });
        }
        /// <summary>
        /// Get all cash transaction history
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllCashTransactions(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Cash_Transaction_History> lstCashTransactionHistory = AutoMapperHelper.GetInstance().Map<List<Cash_Transaction_History>>(_cashTransactionHistoryRepository.GetAll().ToList());
                response = request.CreateResponse<List<Cash_Transaction_History>>(HttpStatusCode.OK, lstCashTransactionHistory);
                return response;
            });

        }

        /// <summary>
        /// Get all cash withdrawal history by user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllWithdrawalByUser(HttpRequestMessage request, string userId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                WithdrawalData objWithdrawalData = new WithdrawalData();
                Wallet_Details objWalletDetails = AutoMapperHelper.GetInstance().Map<Wallet_Details>(_walletDetailsRepository.FindBy(x => x.user_id == userId).FirstOrDefault());
                if (objWalletDetails != null)
                {
                    objWithdrawalData.Cash = objWalletDetails.cash;
                }
                List<Withdrawel_History_Details> lstWithdrawalHistoryDetails = new List<Withdrawel_History_Details>();
                lstWithdrawalHistoryDetails = AutoMapperHelper.GetInstance().Map<List<Withdrawel_History_Details>>(_withdrawelHistoryDetailsRepository.FindBy(x => x.withdrawer_user_id == userId).ToList());
                lstWithdrawalHistoryDetails.ToList().ForEach(c => c.WithdrawelHistoryMaster = new Withdrawel_History_Master
                {
                    created_date = c.WithdrawelHistoryMaster.updated_date.HasValue ? c.WithdrawelHistoryMaster.updated_date.Value.ToLocalTime() : c.WithdrawelHistoryMaster.updated_date,
                    updated_date = c.WithdrawelHistoryMaster.updated_date.HasValue ? c.WithdrawelHistoryMaster.updated_date.Value.ToLocalTime() : c.WithdrawelHistoryMaster.updated_date,
                    updated_by = c.WithdrawelHistoryMaster.updated_by,
                    created_by = c.WithdrawelHistoryMaster.created_by,
                    id = c.WithdrawelHistoryMaster.id
                });
                objWithdrawalData.lstWithdrawalHistoryDeatils = lstWithdrawalHistoryDetails;
                List<Withdrawl_Status_Master> lstWithdrawalStatusMaster = new List<Withdrawl_Status_Master>();
                lstWithdrawalStatusMaster = AutoMapperHelper.GetInstance().Map<List<Withdrawl_Status_Master>>(_withdrawlStatusMasterRepository.GetAll().ToList());
                objWithdrawalData.lstWithdrawalStatusMaster = lstWithdrawalStatusMaster;
                response = request.CreateResponse<WithdrawalData>(HttpStatusCode.OK, objWithdrawalData);
                return response;
            });
        }

        /// <summary>
        /// Get all cash withdrawal history
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllWithdrawal(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                WithdrawalData objWithdrawalData = new WithdrawalData();
                List<Withdrawel_History_Details> lstWithdrawalHistoryDetails = new List<Withdrawel_History_Details>();
                lstWithdrawalHistoryDetails = AutoMapperHelper.GetInstance().Map<List<Withdrawel_History_Details>>(_withdrawelHistoryDetailsRepository.GetAll().ToList());
                lstWithdrawalHistoryDetails.ToList().ForEach(c => c.WithdrawelHistoryMaster = new Withdrawel_History_Master
                {
                    created_date = c.WithdrawelHistoryMaster.updated_date.HasValue ? c.WithdrawelHistoryMaster.updated_date.Value.ToLocalTime() : c.WithdrawelHistoryMaster.updated_date,
                    updated_date = c.WithdrawelHistoryMaster.updated_date.HasValue ? c.WithdrawelHistoryMaster.updated_date.Value.ToLocalTime() : c.WithdrawelHistoryMaster.updated_date,
                    updated_by = c.WithdrawelHistoryMaster.updated_by,
                    created_by = c.WithdrawelHistoryMaster.created_by,
                    id = c.WithdrawelHistoryMaster.id
                });
                objWithdrawalData.lstWithdrawalHistoryDeatils = lstWithdrawalHistoryDetails;
                List<Withdrawl_Status_Master> lstWithdrawalStatusMaster = new List<Withdrawl_Status_Master>();
                lstWithdrawalStatusMaster = AutoMapperHelper.GetInstance().Map<List<Withdrawl_Status_Master>>(_withdrawlStatusMasterRepository.GetAll().ToList());
                objWithdrawalData.lstWithdrawalStatusMaster = lstWithdrawalStatusMaster;
                response = request.CreateResponse<WithdrawalData>(HttpStatusCode.OK, objWithdrawalData);
                return response;
            });
        }

        /// <summary>
        /// Get all topup details by user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllTopupDetailsByUser(HttpRequestMessage request, string userId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                TopUpDetails objTopUpDetails = new TopUpDetails();
                Wallet_Details objWalletDetails = AutoMapperHelper.GetInstance().Map<Wallet_Details>(_walletDetailsRepository.FindBy(x => x.user_id == userId).FirstOrDefault());
                if (objWalletDetails != null)
                {
                    objTopUpDetails.Cash = objWalletDetails.cash;
                }
                objTopUpDetails.TopupStatusMaster = AutoMapperHelper.GetInstance().Map<List<Topup_Status_Master>>(_topupStatusMasterRepository.GetAll().ToList());
                if (objTopUpDetails.TopupStatusMaster == null)
                {
                    objTopUpDetails.TopupStatusMaster = new List<Topup_Status_Master>();
                }
                objTopUpDetails.Topups = AutoMapperHelper.GetInstance().Map<List<Topup_History>>(_topupHistoryRepository.FindBy(x => x.user_id == userId).ToList());
                if (objTopUpDetails.Topups == null)
                {
                    objTopUpDetails.Topups = new List<Topup_History>();
                }
                objTopUpDetails.Topups = objTopUpDetails.Topups.Select(x => new Wollo.Entities.ViewModels.Topup_History
                {
                    id = x.id,
                    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    user_id = x.user_id,
                    TopupStatusMaster = x.TopupStatusMaster,
                    amount = x.amount,
                    AspnetUsers = x.AspnetUsers,
                    details = x.details,
                    topup_status_id = x.topup_status_id,
                    payment_method = x.payment_method
                }).ToList();
                response = request.CreateResponse<TopUpDetails>(HttpStatusCode.OK, objTopUpDetails);
                return response;
            });
        }

        /// <summary>
        /// Get all topup details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllTopupDetails(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                TopUpDetails objTopUpDetails = new TopUpDetails();
                objTopUpDetails.TopupStatusMaster = AutoMapperHelper.GetInstance().Map<List<Topup_Status_Master>>(_topupStatusMasterRepository.GetAll().ToList());
                objTopUpDetails.Topups = AutoMapperHelper.GetInstance().Map<List<Topup_History>>(_topupHistoryRepository.GetAll().ToList());
                objTopUpDetails.Topups = objTopUpDetails.Topups.Select(x => new Wollo.Entities.ViewModels.Topup_History
                {
                    id = x.id,
                    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    user_id = x.user_id,
                    TopupStatusMaster = x.TopupStatusMaster,
                    amount = x.amount,
                    AspnetUsers = x.AspnetUsers,
                    details = x.details,
                    topup_status_id = x.topup_status_id,
                    payment_method = x.payment_method
                }).ToList();
                response = request.CreateResponse<TopUpDetails>(HttpStatusCode.OK, objTopUpDetails);
                return response;
            });
        }

        /// <summary>
        /// Add topup 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Topup"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddTopup(HttpRequestMessage request, Topup_History Topup)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                Models.Topup_History objTopup = new Models.Topup_History();
                objTopup = AutoMapperHelper.GetInstance().Map<Models.Topup_History>(Topup);
                objTopup.created_by = Topup.user_id;
                objTopup.created_date = DateTime.UtcNow;
                objTopup.topup_status_id = _topupStatusMasterRepository.FindBy(x => x.status == "Completed").FirstOrDefault().id;
                objTopup.updated_by = Topup.user_id;
                objTopup.updated_date = DateTime.UtcNow;
                _topupHistoryRepository.Add(objTopup);

                Models.Wallet_Details details = _walletDetailsRepository.GetAll().Where(x => x.user_id == objTopup.user_id).FirstOrDefault();
                details.cash = details.cash + objTopup.amount;
                _walletDetailsRepository.Edit(details);
                result = 1;
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }

        /// <summary>
        /// Cancel topup
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CancelTopup(HttpRequestMessage request, Topup_History objTopup)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                List<Topup_Status_Master> lstTopupStatus = AutoMapperHelper.GetInstance().Map<List<Topup_Status_Master>>(_topupStatusMasterRepository.GetAll().ToList());
                Models.Topup_History objTopupHitory = _topupHistoryRepository.FindBy(x => x.id == objTopup.id).SingleOrDefault();
                //Check for status
                if (objTopupHitory.TopupStatusMaster.status != "Completed" && objTopupHitory.TopupStatusMaster.status != "Cancelled")
                {
                    objTopupHitory.topup_status_id = lstTopupStatus.Where(x => x.status == "Cancelled").FirstOrDefault().id;
                    objTopupHitory.updated_by = objTopup.user_id;
                    objTopupHitory.updated_date = DateTime.UtcNow;
                    _topupHistoryRepository.Edit(objTopupHitory);
                    result = 1;
                }
                else
                {
                    result = 2;
                }
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }

        /// <summary>
        /// Edit topup 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Topup"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage EditTopup(HttpRequestMessage request, Topup_History Topup)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                Models.Topup_History objTopup = _topupHistoryRepository.FindBy(x => x.id == Topup.id).SingleOrDefault();
                objTopup.amount = Topup.amount;
                objTopup.payment_method = Topup.payment_method;
                objTopup.updated_by = Topup.user_id;
                objTopup.updated_date = DateTime.UtcNow;
                _topupHistoryRepository.Edit(objTopup);
                result = 1;
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }

        /// <summary>
        /// Get withdrawal fees by method
        /// </summary>
        /// <param name="request"></param>
        /// <param name="WithdrawalMethod"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetWithDrawalFeeByMethod(HttpRequestMessage request, string WithdrawalMethod)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Withdrawl_Fees objWithdrawlFees = AutoMapperHelper.GetInstance().Map<Withdrawl_Fees>(_withdrawlFeesRepository.FindBy(x => x.method == WithdrawalMethod).FirstOrDefault());
                response = request.CreateResponse<Withdrawl_Fees>(HttpStatusCode.OK, objWithdrawlFees);
                return response;
            });
        }

        /// <summary>
        /// Add withdrawal 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Topup"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddWithdrawal(HttpRequestMessage request, Models.Withdrawel_History_Details objWithdrawelHistoryDetails)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                string[] result = new string[2];
                result[0] = "0";

                Models.Wallet_Details objWalletDetails = _walletDetailsRepository.FindBy(x => x.user_id == objWithdrawelHistoryDetails.withdrawer_user_id).FirstOrDefault();
                Models.Withdrawl_Fees objWithdrawlFees = _withdrawlFeesRepository.FindBy(x => x.method == objWithdrawelHistoryDetails.payment_method).FirstOrDefault();
                float TotalOrderPriceSum = 0;
                float pendingWithdrawalSum = 0;
                if (_transactionRepository.FindBy(x => x.user_id == objWithdrawelHistoryDetails.withdrawer_user_id && x.status_id == 1 && x.queue_action.ToLower().Trim() == "bid").Any())
                {
                    TotalOrderPriceSum = _transactionRepository.GetAll().Where(x => x.user_id == objWithdrawelHistoryDetails.withdrawer_user_id && x.status_id == 1 && x.queue_action.ToLower().Trim() == "bid").Select(y => y.price).Sum();
                }
                if (_withdrawelHistoryDetailsRepository.FindBy(x => x.withdrawer_user_id == objWithdrawelHistoryDetails.withdrawer_user_id && x.withdrawel_status_id == 1).Any())
                {
                    pendingWithdrawalSum = _withdrawelHistoryDetailsRepository.GetAll().Where(x => x.withdrawer_user_id == objWithdrawelHistoryDetails.withdrawer_user_id && x.withdrawel_status_id == 1).Select(y => y.amount).Sum();
                }
                float totalWithdrawableSum = objWalletDetails.cash - TotalOrderPriceSum - pendingWithdrawalSum;
                //float totalCashRequired = 0;
                //if (objWithdrawlFees != null && objWithdrawlFees.RuleConfigMaster.rule_name == "Fixed")
                //{
                //    //Case when fixed rule is set
                //    totalCashRequired = objWithdrawelHistoryDetails.amount + objWithdrawlFees.fees;
                //}
                //else if (objWithdrawlFees != null && objWithdrawlFees.RuleConfigMaster.rule_name == "Percent")
                //{
                //    //case when percent rule is set
                //    totalCashRequired = objWithdrawelHistoryDetails.amount + ((objWithdrawlFees.fees / 100) * objWithdrawelHistoryDetails.amount);
                //}
                //else
                //{
                //    //case when no rule is set, then no fees will be applied
                //    objWithdrawlFees = new Models.Withdrawl_Fees();
                //    objWithdrawlFees.fees = 0;
                //    totalCashRequired = objWithdrawelHistoryDetails.amount + ((objWithdrawlFees.fees / 100) * objWithdrawelHistoryDetails.amount);
                //}
                if (objWalletDetails != null && totalWithdrawableSum >= objWithdrawelHistoryDetails.amount)
                {
                    Models.Withdrawel_History_Master objWithdrawelHistoryMaster = new Models.Withdrawel_History_Master();
                    try
                    {
                        objWithdrawelHistoryMaster.created_by = objWithdrawelHistoryDetails.withdrawer_user_id;
                        objWithdrawelHistoryMaster.created_date = DateTime.UtcNow;
                        objWithdrawelHistoryMaster.updated_by = objWithdrawelHistoryDetails.withdrawer_user_id;
                        objWithdrawelHistoryMaster.updated_date = DateTime.UtcNow;
                        _withdrawelHistoryMasterRepository.Add(objWithdrawelHistoryMaster);
                        result[0] = "1";
                    }
                    catch (Exception ex)
                    {
                        result[0] = "3";
                        result[1] = ex.InnerException.Message.ToString();
                    }
                    try
                    {
                        objWithdrawelHistoryDetails.withdrawel_history_id = objWithdrawelHistoryMaster.id;
                        //Change it later accordingly
                        objWithdrawelHistoryDetails.withdrawel_permission_id = _issueWithdrawelPermissionMasterRepository.GetAll().FirstOrDefault().id;
                        objWithdrawelHistoryDetails.withdrawel_status_id = _withdrawlStatusMasterRepository.FindBy(x => x.status == "In Progress").FirstOrDefault().id;
                        //Needs to add account id
                        objWithdrawelHistoryDetails.withdrawer_account_id = 23;
                        _withdrawelHistoryDetailsRepository.Add(objWithdrawelHistoryDetails);
                        result[0] = "1";
                    }
                    catch (Exception ex)
                    {
                        result[0] = "4";
                        result[1] = ex.InnerException.Message.ToString();
                    }
                }
                else
                {
                    result[0] = "2";
                    result[1] = objWalletDetails.cash.ToString();
                }

                response = request.CreateResponse<string[]>(HttpStatusCode.OK, result);
                return response;
            });
        }

        /// <summary>
        /// Cancel withdrawal
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CancelWithdrawal(HttpRequestMessage request, Withdrawel_History_Details objWithdrawelHistoryDetailsModels)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                List<Models.Withdrawl_Status_Master> lstWithdrawlStatusMaster = _withdrawlStatusMasterRepository.GetAll().ToList();
                Models.Withdrawel_History_Details objWithdrawelHistoryDetails = _withdrawelHistoryDetailsRepository.FindBy(x => x.id == objWithdrawelHistoryDetailsModels.id).SingleOrDefault();
                //Check for status
                if (objWithdrawelHistoryDetails.WithdrawlStatusMaster.status != "approved" && objWithdrawelHistoryDetails.WithdrawlStatusMaster.status != "Cancelled")
                {
                    objWithdrawelHistoryDetails.withdrawel_status_id = lstWithdrawlStatusMaster.Where(x => x.status == "Cancelled").FirstOrDefault().id;
                    _withdrawelHistoryDetailsRepository.Edit(objWithdrawelHistoryDetails);
                    Models.Withdrawel_History_Master objWithdrawelHistoryMaster = _withdrawelHistoryMasterRepository.FindBy(x => x.id == objWithdrawelHistoryDetails.withdrawel_history_id).SingleOrDefault();
                    objWithdrawelHistoryMaster.updated_by = objWithdrawelHistoryDetailsModels.withdrawer_user_id;
                    objWithdrawelHistoryMaster.updated_date = DateTime.UtcNow;
                    _withdrawelHistoryMasterRepository.Edit(objWithdrawelHistoryMaster);
                    result = 1;
                }
                else
                {
                    result = 2;
                }
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }

        /// <summary>
        /// Edit withdrawal 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Topup"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage EditWithdrawal(HttpRequestMessage request, Withdrawel_History_Details WithdrawelHistoryDetails)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                string[] result = new string[2];
                result[0] = "0";
                 Models.Withdrawel_History_Details objWithdrawelHistoryDetails = _withdrawelHistoryDetailsRepository.FindBy(x => x.id == WithdrawelHistoryDetails.id).SingleOrDefault();
                Models.Wallet_Details objWalletDetails = _walletDetailsRepository.FindBy(x => x.user_id == WithdrawelHistoryDetails.withdrawer_user_id).FirstOrDefault();
                float TotalOrderPriceSum = 0;
                float pendingWithdrawalSum = 0;
                if (_transactionRepository.FindBy(x => x.user_id == WithdrawelHistoryDetails.withdrawer_user_id && x.status_id == 1 && x.queue_action.ToLower().Trim() == "bid").Any())
                {
                    TotalOrderPriceSum = _transactionRepository.GetAll().Where(x => x.user_id == WithdrawelHistoryDetails.withdrawer_user_id && x.status_id == 1 && x.queue_action.ToLower().Trim() == "bid").Select(y => y.price).Sum();
                }
                if (_withdrawelHistoryDetailsRepository.FindBy(x => x.withdrawer_user_id == WithdrawelHistoryDetails.withdrawer_user_id && x.withdrawel_status_id == 1).Any())
                {
                    pendingWithdrawalSum = _withdrawelHistoryDetailsRepository.GetAll().Where(x => x.withdrawer_user_id == WithdrawelHistoryDetails.withdrawer_user_id && x.withdrawel_status_id == 1).Select(y => y.amount).Sum();
                    pendingWithdrawalSum = pendingWithdrawalSum - objWithdrawelHistoryDetails.amount;
                }
                float totalWithdrawableSum = objWalletDetails.cash - TotalOrderPriceSum - pendingWithdrawalSum;
                if (objWalletDetails != null && objWithdrawelHistoryDetails.amount != WithdrawelHistoryDetails.amount)
                {
                    if (totalWithdrawableSum >= WithdrawelHistoryDetails.amount)
                    {
                        objWithdrawelHistoryDetails.amount = WithdrawelHistoryDetails.amount;
                        objWithdrawelHistoryDetails.payment_method = WithdrawelHistoryDetails.payment_method;
                        _withdrawelHistoryDetailsRepository.Edit(objWithdrawelHistoryDetails);
                        Models.Withdrawel_History_Master objWithdrawelHistoryMaster = _withdrawelHistoryMasterRepository.FindBy(x => x.id == objWithdrawelHistoryDetails.withdrawel_history_id).SingleOrDefault();
                        objWithdrawelHistoryMaster.updated_by = objWithdrawelHistoryDetails.withdrawer_user_id;
                        objWithdrawelHistoryMaster.updated_date = DateTime.UtcNow;
                        _withdrawelHistoryMasterRepository.Edit(objWithdrawelHistoryMaster);
                        result[0] = "1";
                    }
                    else
                    {
                        result[0] = "2";
                        result[1] = objWalletDetails.cash.ToString();
                    }
                }
                else
                {
                    result[0] = "1";
                }
                
                response = request.CreateResponse<string[]>(HttpStatusCode.OK, result);
                return response;
            });
        }

        /// <summary>
        /// Update topup status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ChangeTopupStatus(HttpRequestMessage request, Topup_History objTopup)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                Models.Topup_History objTopupHistory = _topupHistoryRepository.FindBy(x => x.id == objTopup.id).SingleOrDefault();
                objTopupHistory.topup_status_id = objTopup.topup_status_id;
                objTopupHistory.updated_by = objTopup.user_id;
                objTopupHistory.updated_date = DateTime.UtcNow;
                _topupHistoryRepository.Edit(objTopupHistory);
                if (objTopup.topup_status_id == _topupStatusMasterRepository.FindBy(x => x.status == "Completed").FirstOrDefault().id)
                {
                    Models.Wallet_Details objWalletDetails = _walletDetailsRepository.FindBy(x => x.user_id == objTopupHistory.user_id).FirstOrDefault();

                    Models.Cash_Transaction_History objCashTransactionHistory = new Models.Cash_Transaction_History();
                    objCashTransactionHistory.created_by = objTopup.user_id;
                    objCashTransactionHistory.created_date = DateTime.UtcNow;
                    objCashTransactionHistory.description = "Topup added";
                    objCashTransactionHistory.opening_cash = objWalletDetails.cash;
                    objCashTransactionHistory.transaction_amount = objTopupHistory.amount;
                    objCashTransactionHistory.closing_cash = objWalletDetails.cash + objTopupHistory.amount;
                    objCashTransactionHistory.updated_by = objTopup.user_id;
                    objCashTransactionHistory.updated_date = DateTime.UtcNow;
                    objCashTransactionHistory.user_id = objTopupHistory.user_id;
                    _cashTransactionHistoryRepository.Add(objCashTransactionHistory);

                    objWalletDetails.cash = objWalletDetails.cash + objTopupHistory.amount;
                    objWalletDetails.updated_by = objTopup.user_id;
                    objWalletDetails.updated_date = DateTime.UtcNow;
                    _walletDetailsRepository.Edit(objWalletDetails);
                    result = 1;
                }
                else
                {
                    result = 1;
                }
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }

        /// <summary>
        /// update withdrawal status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ChangeWithdrawStatus(HttpRequestMessage request, Withdrawel_History_Details withdrawelHistoryDetails)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                float[] result = new float[2];
                result[0] = 0;
                Models.Withdrawel_History_Details objWithdrawelHistoryDetails = _withdrawelHistoryDetailsRepository.FindBy(x => x.id == withdrawelHistoryDetails.id).SingleOrDefault();
                objWithdrawelHistoryDetails.withdrawel_status_id = withdrawelHistoryDetails.withdrawel_status_id;
                _withdrawelHistoryDetailsRepository.Edit(objWithdrawelHistoryDetails);
                Models.Withdrawel_History_Master objWithdrawelHistoryMaster = _withdrawelHistoryMasterRepository.FindBy(x => x.id == objWithdrawelHistoryDetails.withdrawel_history_id).SingleOrDefault();
                objWithdrawelHistoryMaster.updated_by = withdrawelHistoryDetails.withdrawer_user_id;
                objWithdrawelHistoryMaster.updated_date = DateTime.UtcNow;
                _withdrawelHistoryMasterRepository.Edit(objWithdrawelHistoryMaster);
                if (withdrawelHistoryDetails.withdrawel_status_id == _withdrawlStatusMasterRepository.FindBy(x => x.status == "Approved").FirstOrDefault().id)
                {
                    Models.Wallet_Details objWalletDetails = _walletDetailsRepository.FindBy(x => x.user_id == objWithdrawelHistoryDetails.withdrawer_user_id).FirstOrDefault();
                    Models.Withdrawl_Fees objWithdrawlFees = _withdrawlFeesRepository.FindBy(x => x.method == objWithdrawelHistoryDetails.payment_method).FirstOrDefault();
                    if (objWithdrawlFees == null)
                    {
                        objWithdrawlFees = new Models.Withdrawl_Fees();
                        objWithdrawlFees.RuleConfigMaster = new Models.Rule_Config_Master();
                        objWithdrawlFees.RuleConfigMaster.rule_name = "Fixed";
                        objWithdrawlFees.fees = 0;
                    }
                    float totalCashRequired = 0;
                    float fees = 0;
                    if (objWithdrawlFees.RuleConfigMaster.rule_name == "Fixed")
                    {
                        totalCashRequired = objWithdrawelHistoryDetails.amount;
                        fees = objWithdrawlFees.fees;
                    }
                    else
                    {
                        //totalCashRequired = objWithdrawelHistoryDetails.amount + ((objWithdrawlFees.fees / 100) * objWithdrawelHistoryDetails.amount);
                        totalCashRequired = objWithdrawelHistoryDetails.amount;
                        //fees = (objWithdrawlFees.fees / 100) * objWithdrawelHistoryDetails.amount;
                        fees = (objWithdrawlFees.fees * objWithdrawelHistoryDetails.amount) / 100;
                    }
                    if (objWalletDetails.cash >= totalCashRequired)
                    {
                        Models.Cash_Transaction_History objCashTransactionHistory = new Models.Cash_Transaction_History();
                        objCashTransactionHistory.created_by = withdrawelHistoryDetails.withdrawer_user_id;
                        objCashTransactionHistory.created_date = DateTime.UtcNow;
                        objCashTransactionHistory.description = "Cash withdrawal by user";
                        objCashTransactionHistory.opening_cash = objWalletDetails.cash;
                        objCashTransactionHistory.transaction_amount = totalCashRequired;
                        objCashTransactionHistory.closing_cash = objWalletDetails.cash - totalCashRequired;
                        objCashTransactionHistory.updated_by = withdrawelHistoryDetails.withdrawer_user_id;
                        objCashTransactionHistory.updated_date = DateTime.UtcNow;
                        objCashTransactionHistory.user_id = objWithdrawelHistoryDetails.withdrawer_user_id;
                        _cashTransactionHistoryRepository.Add(objCashTransactionHistory);

                        objWalletDetails.cash = objWalletDetails.cash - totalCashRequired;
                        objWalletDetails.updated_by = withdrawelHistoryDetails.withdrawer_user_id;
                        objWalletDetails.updated_date = DateTime.UtcNow;
                        _walletDetailsRepository.Edit(objWalletDetails);

                        objWalletDetails = _walletDetailsRepository.FindBy(x => x.is_admin == true).FirstOrDefault();
                        //objWalletDetails = _walletDetailsRepository.FindBy(x => x.user_id == withdrawelHistoryDetails.withdrawer_user_id).FirstOrDefault();

                        objCashTransactionHistory = new Models.Cash_Transaction_History();
                        objCashTransactionHistory.created_by = objWithdrawelHistoryDetails.withdrawer_user_id;
                        objCashTransactionHistory.created_date = DateTime.UtcNow;
                        objCashTransactionHistory.description = "Cash withdrawal fee";
                        objCashTransactionHistory.opening_cash = objWalletDetails.cash;
                        objCashTransactionHistory.transaction_amount = fees;
                        objCashTransactionHistory.closing_cash = objWalletDetails.cash + fees;
                        objCashTransactionHistory.updated_by = objWithdrawelHistoryDetails.withdrawer_user_id;
                        objCashTransactionHistory.updated_date = DateTime.UtcNow;
                        objCashTransactionHistory.user_id = objWalletDetails.user_id;
                        _cashTransactionHistoryRepository.Add(objCashTransactionHistory);

                        //***************************Code for Widrawal history details for superadmin*************************************//
                        Models.Cash_Transaction_Detail objCashTransactionDetails = new Models.Cash_Transaction_Detail();
                        objCashTransactionDetails.cash_issued_on_date = DateTime.UtcNow;
                        objCashTransactionDetails.transaction_amount = fees;
                        objCashTransactionDetails.opening_cash = objWalletDetails.cash;
                        objCashTransactionDetails.closing_cash = objWalletDetails.cash + fees;
                        objCashTransactionDetails.description = "Withdrawal by member";
                        objCashTransactionDetails.issuer_account_id = withdrawelHistoryDetails.withdrawer_user_id;
                        objCashTransactionDetails.receiver_account_id = ConfigurationManager.AppSettings["SuperAdminUserId"];
                        _cashTransactionDetailHistoryRepository.Add(objCashTransactionDetails);
                        //***************************Code for Widrawal history details for superadmin*************************************//

                        objWalletDetails.cash = objWalletDetails.cash + fees;
                        objWalletDetails.updated_by = objWithdrawelHistoryDetails.withdrawer_user_id;
                        objWalletDetails.updated_date = DateTime.UtcNow;
                        _walletDetailsRepository.Edit(objWalletDetails);
                        result[0] = 1;
                    }
                    else
                    {
                        result[0] = 4;
                        result[1] = objWalletDetails.cash;
                    }
                }
                else
                {
                    result[0] = 1;
                }
                response = request.CreateResponse<float[]>(HttpStatusCode.OK, result);
                return response;
            });
        }
        // code made by umang 26/07/16 //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllCashDetails(HttpRequestMessage request, string userId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                List<Issue_Cash_Transfer_Master> objCashDetails = new List<Issue_Cash_Transfer_Master>();

                objCashDetails = AutoMapperHelper.GetInstance().Map<List<Issue_Cash_Transfer_Master>>(_issueCashTransferMasterRepository.GetAll().Where(x => x.receiver_account_id == userId).ToList());
                objCashDetails = objCashDetails.Select(x => new Wollo.Entities.ViewModels.Issue_Cash_Transfer_Master
                {
                    id = x.id,
                    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    issuer_account_id = x.issuer_account_id,
                    receiver_account_id = x.receiver_account_id,
                    ReceiverUser = x.ReceiverUser,
                    cash_issue_permission_id = x.cash_issue_permission_id,
                    cash_issued = x.cash_issued,
                    IssuerUser = x.IssuerUser,
                    IssueWithdrawelPermissionMaster = x.IssueWithdrawelPermissionMaster,
                    cash_issued_on_date = x.cash_issued_on_date,
                }).ToList();
                response = request.CreateResponse<List<Issue_Cash_Transfer_Master>>(HttpStatusCode.OK, objCashDetails);
                return response;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Approve(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Approve_Admin_Cash ApproveAdminCash = new Approve_Admin_Cash();
                List<Issue_Cash_Transfer_Master> objCashDetails = new List<Issue_Cash_Transfer_Master>();
                objCashDetails = AutoMapperHelper.GetInstance().Map<List<Issue_Cash_Transfer_Master>>(_issueCashTransferMasterRepository.GetAll().ToList());
                List<Issue_Withdrawel_Permission_Master> objpermissionDetails = new List<Issue_Withdrawel_Permission_Master>();
                objpermissionDetails = AutoMapperHelper.GetInstance().Map<List<Issue_Withdrawel_Permission_Master>>(_issueWithdrawelPermissionMasterRepository.GetAll().ToList());
                ApproveAdminCash.IssueCashTransferMaster = objCashDetails;
                ApproveAdminCash.IssueWithdrawelTransferMaster = objpermissionDetails;
                ApproveAdminCash.IssueCashTransferMaster = ApproveAdminCash.IssueCashTransferMaster.Select(x => new Wollo.Entities.ViewModels.Issue_Cash_Transfer_Master
                {
                    id = x.id,
                    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    issuer_account_id = x.issuer_account_id,
                    receiver_account_id = x.receiver_account_id,
                    ReceiverUser = x.ReceiverUser,
                    cash_issue_permission_id = x.cash_issue_permission_id,
                    cash_issued = x.cash_issued,
                    IssuerUser = x.IssuerUser,
                    IssueWithdrawelPermissionMaster = x.IssueWithdrawelPermissionMaster,
                    cash_issued_on_date = x.cash_issued_on_date
                }).ToList();
                response = request.CreateResponse<Approve_Admin_Cash>(HttpStatusCode.OK, ApproveAdminCash);
                return response;
            });
        }
        /// <summary>
        /// Code Admin Request For Cash
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <summary>
        /// Code Admin Request For Cash
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CashAdmin(HttpRequestMessage request, Issue_Cash_Transfer_Master Cash)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                string superAdminId = ConfigurationManager.AppSettings["SuperAdminUserId"];
                Models.Cash_Transaction_Detail objCashTransactionDetailReceiver = new Models.Cash_Transaction_Detail();
                Models.Wallet_Details objWalletDetails = _walletDetailsRepository.FindBy(x => x.user_id == superAdminId).FirstOrDefault();
                int result = 0;
                if (Cash.issuer_account_id != superAdminId)
                {
                    Models.Issue_Cash_Transfer_Master objCash = new Models.Issue_Cash_Transfer_Master();
                    objCash = AutoMapperHelper.GetInstance().Map<Models.Issue_Cash_Transfer_Master>(Cash);
                    objCash.created_by = Cash.receiver_account_id.ToString();
                    objCash.created_date = DateTime.UtcNow;
                    objCash.updated_by = Cash.receiver_account_id.ToString();
                    objCash.updated_date = DateTime.UtcNow;
                    objCash.is_admin = true;
                    _issueCashTransferMasterRepository.Add(objCash);
                    result = 2;
                }
                else
                {
                    Models.Cash_Transaction_History objCash = new Models.Cash_Transaction_History();
                    objCash.created_by = superAdminId;
                    objCash.created_date = DateTime.UtcNow;
                    objCash.updated_by = superAdminId;
                    objCash.updated_date = DateTime.UtcNow;
                    objCash.user_id = superAdminId;
                    objCash.transaction_amount = Cash.cash_issued;
                    objCash.description = "Cash Issued to Self By SuperAdmin";
                    objCash.opening_cash = objWalletDetails.cash;
                    objCash.closing_cash = objWalletDetails.cash + Cash.cash_issued;
                    _cashTransactionHistoryRepository.Add(objCash);

                    //*******************Code to do an entry in Issue_cash_transfer_master for self issue by admin to show in Cash Request history************************************************//
                    Models.Issue_Withdrawel_Permission_Master permission = _issueWithdrawelPermissionMasterRepository.GetAll().Where(x => x.permission.ToLower() == "approved").FirstOrDefault();
                    Models.Issue_Cash_Transfer_Master issueCashTransferMaster = new Models.Issue_Cash_Transfer_Master();
                    issueCashTransferMaster.cash_issue_permission_id = permission.id;
                    issueCashTransferMaster.cash_issued = Cash.cash_issued;
                    issueCashTransferMaster.cash_issued_on_date = DateTime.UtcNow;
                    issueCashTransferMaster.issuer_account_id = superAdminId;
                    issueCashTransferMaster.receiver_account_id = superAdminId;
                    issueCashTransferMaster.created_date = DateTime.UtcNow;
                    issueCashTransferMaster.updated_by = superAdminId;
                    issueCashTransferMaster.updated_date = DateTime.UtcNow;
                    issueCashTransferMaster.updated_by = superAdminId;
                    _issueCashTransferMasterRepository.Add(issueCashTransferMaster);
                    //*******************Code to do an entry in Issue_cash_transfer_master for self issue by admin to show in Cash Request history************************************************//

                    //need to add later 

                    objCashTransactionDetailReceiver.issuer_account_id = superAdminId;
                    objCashTransactionDetailReceiver.receiver_account_id = superAdminId;
                    objCashTransactionDetailReceiver.cash_issued_on_date = DateTime.UtcNow;
                    objCashTransactionDetailReceiver.opening_cash = objWalletDetails.cash;
                    objCashTransactionDetailReceiver.transaction_amount = Cash.cash_issued;
                    objCashTransactionDetailReceiver.closing_cash = objWalletDetails.cash + Cash.cash_issued;
                    objCashTransactionDetailReceiver.description = "Cash Issued to Self By SuperAdmin";
                    _cashTransactionDetailHistoryRepository.Add(objCashTransactionDetailReceiver);

                    //need to add later
                    objWalletDetails.cash = objWalletDetails.cash + Cash.cash_issued;
                    _walletDetailsRepository.Edit(objWalletDetails);
                    result = 1;
                }
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }
        // code edited by umang on 11/09/16 //
        [HttpPost]
        public HttpResponseMessage ChangeCashStatus(HttpRequestMessage request, Issue_Cash_Transfer_Master objCash)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                Models.Cash_Transaction_Detail objCashTransactionDetailReceiver = new Models.Cash_Transaction_Detail();
                Models.Cash_Transaction_Detail objCashTransactionDetailIssuer = new Models.Cash_Transaction_Detail();
                Models.Issue_Cash_Transfer_Master objCashHistory = _issueCashTransferMasterRepository.FindBy(x => x.id == objCash.id).SingleOrDefault();
                Models.Wallet_Details objWalletDetails = _walletDetailsRepository.FindBy(x => x.user_id == objCashHistory.receiver_account_id).FirstOrDefault();
                //Models.Wallet_Details objWalletDetailsIssuer = _walletDetailsRepository.FindBy(x => x.user_id == objCash.issuer_account_id).FirstOrDefault();
                //if (objWalletDetailsIssuer.cash >= objCashHistory.cash_issued)
                //{
                objCashHistory.cash_issue_permission_id = objCash.cash_issue_permission_id;
                objCashHistory.updated_by = objCash.issuer_account_id;
                objCashHistory.cash_issued_on_date = DateTime.UtcNow;
                objCashHistory.issuer_account_id = objCash.issuer_account_id;
                objCashHistory.updated_date = DateTime.UtcNow;
                _issueCashTransferMasterRepository.Edit(objCashHistory);

                if (objCash.cash_issue_permission_id == _issueWithdrawelPermissionMasterRepository.FindBy(x => x.permission.ToLower() == "approved").FirstOrDefault().id)
                {


                    Models.Cash_Transaction_History objCashTransactionHistory = new Models.Cash_Transaction_History();
                    Models.Cash_Transaction_History objCashTransactionHistoryAdmin = new Models.Cash_Transaction_History();
                    objCashTransactionHistory.created_by = objCash.receiver_account_id;
                    objCashTransactionHistory.created_date = DateTime.UtcNow;
                    objCashTransactionHistory.description = "Cash Issued to Admin By SuperAdmin ";
                    objCashTransactionHistory.opening_cash = objWalletDetails.cash;
                    objCashTransactionHistory.transaction_amount = objCashHistory.cash_issued;
                    objCashTransactionHistory.closing_cash = objWalletDetails.cash + objCashHistory.cash_issued;
                    objCashTransactionHistory.updated_by = objCashHistory.issuer_account_id;
                    objCashTransactionHistory.updated_date = DateTime.UtcNow;
                    objCashTransactionHistory.user_id = objCashHistory.receiver_account_id;
                    _cashTransactionHistoryRepository.Add(objCashTransactionHistory);


                    // need to add later for cash view detail for reciver
                    objCashTransactionDetailReceiver.issuer_account_id = objCashHistory.issuer_account_id;
                    objCashTransactionDetailReceiver.receiver_account_id = objCashHistory.receiver_account_id;
                    objCashTransactionDetailReceiver.cash_issued_on_date = DateTime.UtcNow;
                    objCashTransactionDetailReceiver.opening_cash = objWalletDetails.cash;
                    objCashTransactionDetailReceiver.transaction_amount = objCashHistory.cash_issued;
                    objCashTransactionDetailReceiver.closing_cash = objWalletDetails.cash + objCashHistory.cash_issued;
                    objCashTransactionDetailReceiver.description = "Cash Recived to Admin By SuperAdmin";
                    _cashTransactionDetailHistoryRepository.Add(objCashTransactionDetailReceiver);

                    // need to add later for cash view detail for issuer
                    //objCashTransactionDetailIssuer.issuer_account_id = objCashHistory.issuer_account_id;
                    //objCashTransactionDetailIssuer.receiver_account_id = objCashHistory.receiver_account_id;
                    //objCashTransactionDetailIssuer.cash_issued_on_date = DateTime.UtcNow;
                    //objCashTransactionDetailIssuer.opening_cash = objWalletDetailsIssuer.cash;
                    //objCashTransactionDetailIssuer.transaction_amount = objCashHistory.cash_issued;
                    //objCashTransactionDetailIssuer.closing_cash = objWalletDetailsIssuer.cash - objCashHistory.cash_issued;
                    //objCashTransactionDetailIssuer.description = "Cash Issued to Admin By SuperAdmin";
                    //_cashTransactionDetailHistoryRepository.Add(objCashTransactionDetailIssuer);


                    //need to add later for wallet detail changes

                    objWalletDetails.cash = objWalletDetails.cash + objCashHistory.cash_issued;
                    objWalletDetails.updated_by = objCashHistory.receiver_account_id;
                    objWalletDetails.updated_date = DateTime.UtcNow;
                    _walletDetailsRepository.Edit(objWalletDetails);

                    //**************************** Code to deduct cash from super admin wallet after approving admin's cash issue request*****************//
                    //objWalletDetailsIssuer.cash = objWalletDetailsIssuer.cash - objCashHistory.cash_issued;
                    //objWalletDetails.updated_date = DateTime.UtcNow;
                    //_walletDetailsRepository.Edit(objWalletDetailsIssuer);

                    result = 1;
                }
                else
                {
                    result = 1;
                }
                //}
                //else
                //{
                //    result = 3;
                //}
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }
        //code made by umang on 29/7/16 //

        [HttpPost]
        public HttpResponseMessage AddCashMember(HttpRequestMessage request, Cash_Transaction_History CashTransactionHistory, string id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;

                string superAdminId = ConfigurationManager.AppSettings["SuperAdminUserId"];
                Models.Issue_Withdrawel_Permission_Master permission = _issueWithdrawelPermissionMasterRepository.GetAll().Where(x => x.permission.ToLower() == "approved").FirstOrDefault();
                string receiverUserId = _userRepository.GetAll().Where(x => x.user_name.ToLower().Trim() == id).FirstOrDefault().user_id;
                string receiverUserRole = _walletDetailsRepository.GetAll().Where(x => x.user_id == receiverUserId).FirstOrDefault().AspnetRoles.Name;
                string issuerUserId = _userRepository.GetAll().Where(x => x.user_id.ToLower().Trim() == CashTransactionHistory.updated_by).FirstOrDefault().user_id;
                Models.Wallet_Details objWalletDetailsIssuer = _walletDetailsRepository.FindBy(x => x.user_id == issuerUserId).FirstOrDefault();
                if (objWalletDetailsIssuer.cash >= CashTransactionHistory.transaction_amount)
                {

                    Models.Wallet_Details objWalletDetails = _walletDetailsRepository.FindBy(x => x.user_id == receiverUserId).FirstOrDefault();
                    Models.Issue_Cash_Transfer_Master issueCashTransferMaster = new Models.Issue_Cash_Transfer_Master();
                    Models.Cash_Transaction_History objCashTransactionHistory = new Models.Cash_Transaction_History();
                    Models.Cash_Transaction_Detail objCashTransactionDetailReceiver = new Models.Cash_Transaction_Detail();
                    Models.Cash_Transaction_Detail objCashTransactionDetailIssuer = new Models.Cash_Transaction_Detail();
                    Models.Cash_Transaction_History objCashTransactionHistoryAdmin = new Models.Cash_Transaction_History();
                    if (issuerUserId != superAdminId)
                    {
                        objCashTransactionHistory = AutoMapperHelper.GetInstance().Map<Models.Cash_Transaction_History>(CashTransactionHistory);
                        objCashTransactionHistory.created_by = objCashTransactionHistory.updated_by;
                        objCashTransactionHistory.created_date = DateTime.UtcNow;
                        objCashTransactionHistory.updated_date = DateTime.UtcNow;
                        objCashTransactionHistory.updated_by = objCashTransactionHistory.updated_by;
                        objCashTransactionHistory.description = "Cash Issued To Member By Admin";
                        objCashTransactionHistory.opening_cash = objWalletDetails.cash;
                        objCashTransactionHistory.transaction_amount = objCashTransactionHistory.transaction_amount;
                        objCashTransactionHistory.closing_cash = objWalletDetails.cash + objCashTransactionHistory.transaction_amount;
                        objCashTransactionHistory.user_id = receiverUserId;
                        _cashTransactionHistoryRepository.Add(objCashTransactionHistory);

                        // need to add later for cash view detail for reciver

                        objCashTransactionDetailReceiver.issuer_account_id = issuerUserId;
                        objCashTransactionDetailReceiver.receiver_account_id = receiverUserId;
                        objCashTransactionDetailReceiver.cash_issued_on_date = DateTime.UtcNow;
                        objCashTransactionDetailReceiver.opening_cash = objWalletDetails.cash;
                        objCashTransactionDetailReceiver.transaction_amount = objCashTransactionHistory.transaction_amount;
                        objCashTransactionDetailReceiver.closing_cash = objWalletDetails.cash + objCashTransactionHistory.transaction_amount;
                        objCashTransactionDetailReceiver.description = "Cash Recived To Member By Admin";
                        _cashTransactionDetailHistoryRepository.Add(objCashTransactionDetailReceiver);

                        // need to add later for cash view detail for issuer

                        objCashTransactionDetailIssuer.issuer_account_id = issuerUserId;
                        objCashTransactionDetailIssuer.receiver_account_id = receiverUserId;
                        objCashTransactionDetailIssuer.cash_issued_on_date = DateTime.UtcNow;
                        objCashTransactionDetailIssuer.opening_cash = objWalletDetailsIssuer.cash;
                        objCashTransactionDetailIssuer.transaction_amount = objCashTransactionHistory.transaction_amount;
                        objCashTransactionDetailIssuer.closing_cash = objWalletDetailsIssuer.cash - objCashTransactionHistory.transaction_amount;
                        objCashTransactionDetailIssuer.description = "Cash Issued To Member By Admin";
                        _cashTransactionDetailHistoryRepository.Add(objCashTransactionDetailIssuer);

                    }
                    else
                    {
                        if (receiverUserRole.ToLower() == "super admin 1")
                        {
                            objCashTransactionHistory = AutoMapperHelper.GetInstance().Map<Models.Cash_Transaction_History>(CashTransactionHistory);
                            objCashTransactionHistory.created_by = objCashTransactionHistory.updated_by;
                            objCashTransactionHistory.created_date = DateTime.UtcNow;
                            objCashTransactionHistory.updated_date = DateTime.UtcNow;
                            objCashTransactionHistory.updated_by = objCashTransactionHistory.updated_by;
                            objCashTransactionHistory.description = "Cash Issued To Admin By SuperAdmin";
                            objCashTransactionHistory.opening_cash = objWalletDetails.cash;
                            objCashTransactionHistory.transaction_amount = objCashTransactionHistory.transaction_amount;
                            objCashTransactionHistory.closing_cash = objWalletDetails.cash + objCashTransactionHistory.transaction_amount;
                            objCashTransactionHistory.user_id = receiverUserId;
                            _cashTransactionHistoryRepository.Add(objCashTransactionHistory);

                            // need to add later for cash view detail for reciver

                            objCashTransactionDetailReceiver.issuer_account_id = issuerUserId;
                            objCashTransactionDetailReceiver.receiver_account_id = receiverUserId;
                            objCashTransactionDetailReceiver.cash_issued_on_date = DateTime.UtcNow;
                            objCashTransactionDetailReceiver.opening_cash = objWalletDetails.cash;
                            objCashTransactionDetailReceiver.transaction_amount = objCashTransactionHistory.transaction_amount;
                            objCashTransactionDetailReceiver.closing_cash = objWalletDetails.cash + objCashTransactionHistory.transaction_amount;
                            objCashTransactionDetailReceiver.description = "Cash Recived To Admin By SuperAdmin";
                            _cashTransactionDetailHistoryRepository.Add(objCashTransactionDetailReceiver);

                            // need to add later for cash view detail for issuer

                            objCashTransactionDetailIssuer.issuer_account_id = issuerUserId;
                            objCashTransactionDetailIssuer.receiver_account_id = receiverUserId;
                            objCashTransactionDetailIssuer.cash_issued_on_date = DateTime.UtcNow;
                            objCashTransactionDetailIssuer.opening_cash = objWalletDetailsIssuer.cash;
                            objCashTransactionDetailIssuer.transaction_amount = objCashTransactionHistory.transaction_amount;
                            objCashTransactionDetailIssuer.closing_cash = objWalletDetailsIssuer.cash - objCashTransactionHistory.transaction_amount;
                            objCashTransactionDetailIssuer.description = "Cash Issued To Admin By SuperAdmin";
                            _cashTransactionDetailHistoryRepository.Add(objCashTransactionDetailIssuer);
                        }

                        else
                        {
                            objCashTransactionHistory = AutoMapperHelper.GetInstance().Map<Models.Cash_Transaction_History>(CashTransactionHistory);
                            objCashTransactionHistory.created_by = objCashTransactionHistory.updated_by;
                            objCashTransactionHistory.created_date = DateTime.UtcNow;
                            objCashTransactionHistory.updated_date = DateTime.UtcNow;
                            objCashTransactionHistory.updated_by = objCashTransactionHistory.updated_by;
                            objCashTransactionHistory.description = "Cash Issued To Member By SuperAdmin";
                            objCashTransactionHistory.opening_cash = objWalletDetails.cash;
                            objCashTransactionHistory.transaction_amount = objCashTransactionHistory.transaction_amount;
                            objCashTransactionHistory.closing_cash = objWalletDetails.cash + objCashTransactionHistory.transaction_amount;
                            objCashTransactionHistory.user_id = receiverUserId;
                            _cashTransactionHistoryRepository.Add(objCashTransactionHistory);

                            // need to add later for cash view detail for reciver

                            objCashTransactionDetailReceiver.issuer_account_id = issuerUserId;
                            objCashTransactionDetailReceiver.receiver_account_id = receiverUserId;
                            objCashTransactionDetailReceiver.cash_issued_on_date = DateTime.UtcNow;
                            objCashTransactionDetailReceiver.opening_cash = objWalletDetails.cash;
                            objCashTransactionDetailReceiver.transaction_amount = objCashTransactionHistory.transaction_amount;
                            objCashTransactionDetailReceiver.closing_cash = objWalletDetails.cash + objCashTransactionHistory.transaction_amount;
                            objCashTransactionDetailReceiver.description = "Cash Recived To Member By SuperAdmin";
                            _cashTransactionDetailHistoryRepository.Add(objCashTransactionDetailReceiver);

                            // need to add later for cash view detail for issuer

                            objCashTransactionDetailIssuer.issuer_account_id = issuerUserId;
                            objCashTransactionDetailIssuer.receiver_account_id = receiverUserId;
                            objCashTransactionDetailIssuer.cash_issued_on_date = DateTime.UtcNow;
                            objCashTransactionDetailIssuer.opening_cash = objWalletDetailsIssuer.cash;
                            objCashTransactionDetailIssuer.transaction_amount = objCashTransactionHistory.transaction_amount;
                            objCashTransactionDetailIssuer.closing_cash = objWalletDetailsIssuer.cash - objCashTransactionHistory.transaction_amount;
                            objCashTransactionDetailIssuer.description = "Cash Issued To Member By SuperAdmin";
                            _cashTransactionDetailHistoryRepository.Add(objCashTransactionDetailIssuer);
                        }
                    }

                    issueCashTransferMaster.cash_issue_permission_id = permission.id;
                    issueCashTransferMaster.cash_issued = objCashTransactionHistory.transaction_amount;
                    issueCashTransferMaster.cash_issued_on_date = DateTime.UtcNow;
                    issueCashTransferMaster.issuer_account_id = issuerUserId;
                    issueCashTransferMaster.receiver_account_id = receiverUserId;
                    issueCashTransferMaster.created_date = DateTime.UtcNow;
                    issueCashTransferMaster.updated_by = issuerUserId;
                    issueCashTransferMaster.updated_date = DateTime.UtcNow;
                    issueCashTransferMaster.updated_by = issuerUserId;
                    issueCashTransferMaster.is_admin = false;
                    _issueCashTransferMasterRepository.Add(issueCashTransferMaster);


                    //Needs to add later add cash on reciver wallet 
                    objWalletDetails.cash = objWalletDetails.cash + CashTransactionHistory.transaction_amount;
                    objWalletDetails.updated_by = CashTransactionHistory.updated_by;
                    objWalletDetails.updated_date = DateTime.UtcNow;

                    // Need to add later subtract cash from issuer wallet
                    objWalletDetailsIssuer.cash = objWalletDetailsIssuer.cash - CashTransactionHistory.transaction_amount;
                    _walletDetailsRepository.Edit(objWalletDetails);
                    _walletDetailsRepository.Edit(objWalletDetailsIssuer);
                    result = 1;
                }
                else
                {
                    result = 2;
                }
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }


        /// <summary>
        /// Get cash details
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCashHistoryViewDetails(HttpRequestMessage request, string userId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                string superadminId = ConfigurationManager.AppSettings["SuperAdminUserId"];
                List<Cash_Transaction_Detail> lstCashTransactionHistory = AutoMapperHelper.GetInstance().Map<List<Cash_Transaction_Detail>>(_cashTransactionDetailHistoryRepository.GetAll().ToList());
                if (userId != superadminId)
                {
                    lstCashTransactionHistory = lstCashTransactionHistory.Where(x => ((x.issuer_account_id == userId) || (x.issuer_account_id == superadminId && x.receiver_account_id == userId)) && (x.description.ToLower() == "cash recived to admin by superadmin" || x.description.ToLower() == "cash issued to member by admin")).ToList();
                }
                else
                {
                    lstCashTransactionHistory = lstCashTransactionHistory.Where(x => (x.issuer_account_id == userId || x.receiver_account_id == superadminId) && (x.description.ToLower() == "cash issued to admin by superadmin" || x.description.ToLower() == "cash issued to self by superadmin" || x.description.ToLower() == "cash issued to member by superadmin" || x.description.ToLower() == "traded price difference" || x.description.ToLower() == "administration fee from buyer" || x.description.ToLower() == "administration fee from seller" || x.description.ToLower() == "withdrawal by member")).ToList();
                }
                lstCashTransactionHistory = lstCashTransactionHistory.Select(x => new Wollo.Entities.ViewModels.Cash_Transaction_Detail
                {
                    id = x.id,
                    cash_issued_on_date = x.description.ToLower() == "traded price difference" || x.description.ToLower() == "administration fee from buyer" || x.description.ToLower() == "administration fee from seller" ? x.cash_issued_on_date : x.cash_issued_on_date.ToLocalTime(),
                    IssuerUsers = x.IssuerUsers,
                    ReceiverUser = x.ReceiverUser,
                    description = x.description,
                    opening_cash = x.opening_cash,
                    closing_cash = x.closing_cash,
                    transaction_amount = x.transaction_amount,

                }).ToList();
                response = request.CreateResponse<List<Cash_Transaction_Detail>>(HttpStatusCode.OK, lstCashTransactionHistory);
                return response;
            });
        }

        public HttpResponseMessage GetAllIssueCashHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Cash_Transaction_History> objCashDetails = new List<Cash_Transaction_History>();

                objCashDetails = AutoMapperHelper.GetInstance().Map<List<Cash_Transaction_History>>(_cashTransactionHistoryRepository.GetAll().ToList());
                objCashDetails = objCashDetails.Select(x => new Wollo.Entities.ViewModels.Cash_Transaction_History
                {
                    id = x.id,
                    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    user_id = x.user_id,
                    description = x.description,
                    opening_cash = x.opening_cash,
                    closing_cash = x.closing_cash,
                    transaction_amount = x.transaction_amount,
                    AspNetUsers = x.AspNetUsers
                }).ToList();
                response = request.CreateResponse<List<Cash_Transaction_History>>(HttpStatusCode.OK, objCashDetails);
                return response;
            });
        }

        /// <summary>
        /// Code for topup by member history detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetTopupByMembersHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Topup_Status_Master topupStatus = AutoMapperHelper.GetInstance().Map<Topup_Status_Master>(_topupStatusMasterRepository.GetAll().Where(x => x.status.ToLower() == "completed").FirstOrDefault());
                List<Topup_History> topupByMemberHistory = AutoMapperHelper.GetInstance().Map<List<Topup_History>>(_topupHistoryRepository.GetAll().Where(x => x.topup_status_id == topupStatus.id).ToList());
                topupByMemberHistory = topupByMemberHistory.Select(x => new Wollo.Entities.ViewModels.Topup_History
                {
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    AspnetUsers = x.AspnetUsers,
                    amount = x.amount

                }).ToList();
                response = request.CreateResponse<List<Topup_History>>(HttpStatusCode.OK, topupByMemberHistory);
                return response;
            });
        }

        /// <summary>
        /// code for withdraw out by member history detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpGet]
        public HttpResponseMessage GetWithdrawOutByMembersHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Withdrawl_Status_Master withdrawStatus = AutoMapperHelper.GetInstance().Map<Withdrawl_Status_Master>(_withdrawlStatusMasterRepository.GetAll().Where(x => x.status.ToLower() == "approved").FirstOrDefault());
                List<Withdrawel_History_Details> withdrawOutByMemberHistory = AutoMapperHelper.GetInstance().Map<List<Withdrawel_History_Details>>(_withdrawelHistoryDetailsRepository.GetAll().Where(x => x.withdrawel_status_id == withdrawStatus.id).ToList());
                withdrawOutByMemberHistory = withdrawOutByMemberHistory.Select(x => new Wollo.Entities.ViewModels.Withdrawel_History_Details
                {
                    WithdrawelHistoryMaster = x.WithdrawelHistoryMaster,
                    AspnetUsers = x.AspnetUsers,
                    amount = x.amount

                }).ToList();
                response = request.CreateResponse<List<Withdrawel_History_Details>>(HttpStatusCode.OK, withdrawOutByMemberHistory);
                return response;
            });
        }

        /// <summary>
        /// code for cash issued member history detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpGet]
        public HttpResponseMessage GetCashIssuedToMembersHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Issue_Withdrawel_Permission_Master permission = AutoMapperHelper.GetInstance().Map<Issue_Withdrawel_Permission_Master>(_issueWithdrawelPermissionMasterRepository.GetAll().Where(x => x.permission.ToLower() == "approved").FirstOrDefault());
                //List<Issue_Cash_Transfer_Master> lstMembers = new List<Issue_Cash_Transfer_Master>();
                List<Issue_Cash_Transfer_Master> memberHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Cash_Transfer_Master>>(_issueCashTransferMasterRepository.GetAll().Where(x => x.cash_issue_permission_id == permission.id && x.is_admin==false).ToList());
                //List<Models.Wallet_Details> memberId = new List<Models.Wallet_Details>();
                //memberId = _walletDetailsRepository.GetAll().Where(x => x.AspnetRoles.Name.ToLower() == "member").ToList();
                //foreach (var item in memberId)
                //{
                //    if (memberHistory.Where(x => x.receiver_account_id == item.user_id).Any())
                //    {
                //        var member = memberHistory.Where(x => x.receiver_account_id == item.user_id).ToList();
                //        foreach (var data in member)
                //        {
                //            lstMembers.Add(data);
                //        }
                //    }

                //}
                memberHistory = memberHistory.Select(x => new Wollo.Entities.ViewModels.Issue_Cash_Transfer_Master
                {
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    IssuerUser = x.IssuerUser,
                    ReceiverUser = x.ReceiverUser,
                    cash_issued = x.cash_issued

                }).ToList();
                response = request.CreateResponse<List<Issue_Cash_Transfer_Master>>(HttpStatusCode.OK, memberHistory);
                return response;
            });
        }

        /// <summary>
        /// Code for Wollo Reward Points Transferred Out By Members History
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetWolloRewardPointsTransferredOutByMembersHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Transfer_Action_Master permission = AutoMapperHelper.GetInstance().Map<Transfer_Action_Master>(_transferActionMasterHistoryRepository.GetAll().Where(x => x.name.ToLower() == "rpe to wollo").FirstOrDefault());
                List<Reward_Points_Transfer_Details> rewardPointInHistory = AutoMapperHelper.GetInstance().Map<List<Reward_Points_Transfer_Details>>(_rewardPointsTransferDetailsRepository.GetAll().Where(x => x.transfer_actionid == permission.id).ToList());
                rewardPointInHistory = rewardPointInHistory.Select(x => new Wollo.Entities.ViewModels.Reward_Points_Transfer_Details
                {
                    TransferActionMaster = x.TransferActionMaster,
                    RewardPointTransferMaster = x.RewardPointTransferMaster,
                    points_transferred = x.points_transferred

                }).ToList();
                response = request.CreateResponse<List<Reward_Points_Transfer_Details>>(HttpStatusCode.OK, rewardPointInHistory);
                return response;
            });
        }

        /// <summary>
        /// Code for Wollo Reward Points Transferred In By Members History
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>


        [HttpGet]
        public HttpResponseMessage GetWolloRewardPointsTransferredInByMembersHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Transfer_Action_Master permission = AutoMapperHelper.GetInstance().Map<Transfer_Action_Master>(_transferActionMasterHistoryRepository.GetAll().Where(x => x.name.ToLower() == "wollo to rpe").FirstOrDefault());
                List<Reward_Points_Transfer_Details> rewardPointInHistory = AutoMapperHelper.GetInstance().Map<List<Reward_Points_Transfer_Details>>(_rewardPointsTransferDetailsRepository.GetAll().Where(x => x.transfer_actionid == permission.id).ToList());
                rewardPointInHistory = rewardPointInHistory.Select(x => new Wollo.Entities.ViewModels.Reward_Points_Transfer_Details
                {
                    TransferActionMaster = x.TransferActionMaster,
                    RewardPointTransferMaster = x.RewardPointTransferMaster,
                    points_transferred = x.points_transferred

                }).ToList();
                response = request.CreateResponse<List<Reward_Points_Transfer_Details>>(HttpStatusCode.OK, rewardPointInHistory);
                return response;
            });
        }



        /// <summary>
        /// code for cash issued admin history detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCashIssuedToAdminHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Issue_Withdrawel_Permission_Master permission = AutoMapperHelper.GetInstance().Map<Issue_Withdrawel_Permission_Master>(_issueWithdrawelPermissionMasterRepository.GetAll().Where(x => x.permission.ToLower() == "approved").FirstOrDefault());
                //List<Issue_Cash_Transfer_Master> lstMembers = new List<Issue_Cash_Transfer_Master>();
                List<Issue_Cash_Transfer_Master> memberHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Cash_Transfer_Master>>(_issueCashTransferMasterRepository.GetAll().Where(x => x.cash_issue_permission_id == permission.id && x.is_admin==true).ToList());
                //List<Models.Wallet_Details> AdminId = new List<Models.Wallet_Details>();
                //AdminId = _walletDetailsRepository.GetAll().Where(x => x.AspnetRoles.Name.ToLower() == "super admin 1").ToList();
                //foreach (var item in AdminId)
                //{
                //    if (memberHistory.Where(x => x.receiver_account_id == item.user_id).Any())
                //    {
                //        var member = memberHistory.Where(x => x.receiver_account_id == item.user_id).ToList();
                //        foreach (var data in member)
                //        {
                //            lstMembers.Add(data);
                //        }
                //    }

                //}
                memberHistory = memberHistory.Select(x => new Wollo.Entities.ViewModels.Issue_Cash_Transfer_Master
                {
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    IssuerUser = x.IssuerUser,
                    ReceiverUser = x.ReceiverUser,
                    cash_issued = x.cash_issued

                }).ToList();
                response = request.CreateResponse<List<Issue_Cash_Transfer_Master>>(HttpStatusCode.OK, memberHistory);
                return response;
            });
        }

        /// <summary>
        /// Code In Circulation history detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCashCirculationHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Wallet_Details> walletDetailHistory = AutoMapperHelper.GetInstance().Map<List<Wallet_Details>>(_walletDetailsRepository.GetAll().ToList());
                response = request.CreateResponse<List<Wallet_Details>>(HttpStatusCode.OK, walletDetailHistory);
                return response;
            });
        }
        /// <summary>
        /// Code for wollo Reward Point In Circulation history detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetWolloRewardPointCirculationHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int stockCode = Convert.ToInt32(ConfigurationManager.AppSettings["FirstStock"]);
                List<Member_Stock_Details> rewardPointHistory = AutoMapperHelper.GetInstance().Map<List<Member_Stock_Details>>(_memberStockDetailsRepository.GetAll().Where(x => x.stock_code_id == stockCode).ToList());
                response = request.CreateResponse<List<Member_Stock_Details>>(HttpStatusCode.OK, rewardPointHistory);
                return response;
            });
        }
        /// <summary>
        /// Code for test Reward Point In Circulation history detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetTestRewardPointCirculationHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int stockCode = Convert.ToInt32(ConfigurationManager.AppSettings["SecondStock"]);
                List<Member_Stock_Details> rewardPointHistory = AutoMapperHelper.GetInstance().Map<List<Member_Stock_Details>>(_memberStockDetailsRepository.GetAll().Where(x => x.stock_code_id == stockCode).ToList());
                response = request.CreateResponse<List<Member_Stock_Details>>(HttpStatusCode.OK, rewardPointHistory);
                return response;
            });
        }
        /// <summary>
        /// code for wollo reward points issued to members
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpGet]
        public HttpResponseMessage GetWolloRewardPointsIssuedToMembersHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int stockCode = Convert.ToInt32(ConfigurationManager.AppSettings["FirstStock"]);
                List<Models.Wallet_Details> memberId = new List<Models.Wallet_Details>();
                List<Issue_Point_Transfer_Detail> lstMembers = new List<Issue_Point_Transfer_Detail>();
                memberId = _walletDetailsRepository.GetAll().Where(x => x.AspnetRoles.Name.ToLower() == "member").ToList();
                List<Issue_Point_Transfer_Detail> memberHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Point_Transfer_Detail>>(_issuePointsTransferDetailHistoryDetailsRepository.GetAll().Where(x => x.stock_id == stockCode)).ToList();
                foreach (var item in memberId)
                {
                    if (memberHistory.Where(x => x.receiver_account_id == item.user_id).Any())
                    {
                        var member = memberHistory.Where(x => x.receiver_account_id == item.user_id).ToList();
                        foreach (var data in member)
                        {
                            lstMembers.Add(data);
                        }
                    }

                }
                lstMembers = lstMembers.Where(x => x.description.ToLower() == "point issued to member by admin" || x.description.ToLower() == "point issued to member by superadmin").ToList();
                lstMembers = lstMembers.Select(x => new Wollo.Entities.ViewModels.Issue_Point_Transfer_Detail
                {
                    point_issued_on_date = x.point_issued_on_date.ToLocalTime(),
                    IssuerUsers = x.IssuerUsers,
                    ReceiverUser = x.ReceiverUser,
                    transaction_amount = x.transaction_amount

                }).ToList();

                response = request.CreateResponse<List<Issue_Point_Transfer_Detail>>(HttpStatusCode.OK, lstMembers);
                return response;
            });
        }
        /// <summary>
        /// code for wollo reward points issued into system history
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public HttpResponseMessage GetWolloRewardPointsIssuedIntoSystemHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int stockCode = Convert.ToInt32(ConfigurationManager.AppSettings["FirstStock"]);
                List<Models.Wallet_Details> AdminId = new List<Models.Wallet_Details>();
                List<Issue_Point_Transfer_Detail> lstMembers = new List<Issue_Point_Transfer_Detail>();
                AdminId = _walletDetailsRepository.GetAll().Where(x => x.AspnetRoles.Name.ToLower() == "super admin 1").ToList();
                List<Issue_Point_Transfer_Detail> memberHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Point_Transfer_Detail>>(_issuePointsTransferDetailHistoryDetailsRepository.GetAll().Where(x => x.stock_id == stockCode)).ToList();
                foreach (var item in AdminId)
                {
                    if (memberHistory.Where(x => x.receiver_account_id == item.user_id).Any())
                    {
                        var member = memberHistory.Where(x => x.receiver_account_id == item.user_id).ToList();
                        foreach (var data in member)
                        {
                            lstMembers.Add(data);
                        }
                    }

                }
                lstMembers = lstMembers.Where(x => x.description.ToLower() == "point recived to admin by superadmin").ToList();
                lstMembers = lstMembers.Select(x => new Wollo.Entities.ViewModels.Issue_Point_Transfer_Detail
                {
                    point_issued_on_date = x.point_issued_on_date.ToLocalTime(),
                    IssuerUsers = x.IssuerUsers,
                    ReceiverUser = x.ReceiverUser,
                    transaction_amount = x.transaction_amount

                }).ToList();

                response = request.CreateResponse<List<Issue_Point_Transfer_Detail>>(HttpStatusCode.OK, lstMembers);
                return response;
            });
        }

        /// <summary>
        /// code for test reward points issued to members
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpGet]
        public HttpResponseMessage GetTestRewardPointsIssuedToMembersHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int stockCode = Convert.ToInt32(ConfigurationManager.AppSettings["SecondStock"]);
                List<Models.Wallet_Details> memberId = new List<Models.Wallet_Details>();
                List<Issue_Point_Transfer_Detail> lstMembers = new List<Issue_Point_Transfer_Detail>();
                memberId = _walletDetailsRepository.GetAll().Where(x => x.AspnetRoles.Name.ToLower() == "member").ToList();
                List<Issue_Point_Transfer_Detail> memberHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Point_Transfer_Detail>>(_issuePointsTransferDetailHistoryDetailsRepository.GetAll().Where(x => x.stock_id == stockCode)).ToList();
                foreach (var item in memberId)
                {
                    if (memberHistory.Where(x => x.receiver_account_id == item.user_id).Any())
                    {
                        var member = memberHistory.Where(x => x.receiver_account_id == item.user_id).ToList();
                        foreach (var data in member)
                        {
                            lstMembers.Add(data);
                        }
                    }

                }
                lstMembers = lstMembers.Where(x => x.description.ToLower() == "point issued to member by admin" || x.description.ToLower() == "point issued to member by superadmin").ToList();
                lstMembers = lstMembers.Select(x => new Wollo.Entities.ViewModels.Issue_Point_Transfer_Detail
                {
                    point_issued_on_date = x.point_issued_on_date.ToLocalTime(),
                    IssuerUsers = x.IssuerUsers,
                    ReceiverUser = x.ReceiverUser,
                    transaction_amount = x.transaction_amount

                }).ToList();

                response = request.CreateResponse<List<Issue_Point_Transfer_Detail>>(HttpStatusCode.OK, lstMembers);
                return response;
            });
        }
        /// <summary>
        /// code for test reward points issued into system history
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public HttpResponseMessage GetTestRewardPointsIssuedIntoSystemHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int stockCode = Convert.ToInt32(ConfigurationManager.AppSettings["SecondStock"]);
                List<Models.Wallet_Details> AdminId = new List<Models.Wallet_Details>();
                List<Issue_Point_Transfer_Detail> lstMembers = new List<Issue_Point_Transfer_Detail>();
                AdminId = _walletDetailsRepository.GetAll().Where(x => x.AspnetRoles.Name.ToLower() == "super admin 1").ToList();
                List<Issue_Point_Transfer_Detail> memberHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Point_Transfer_Detail>>(_issuePointsTransferDetailHistoryDetailsRepository.GetAll().Where(x => x.stock_id == stockCode)).ToList();
                foreach (var item in AdminId)
                {
                    if (memberHistory.Where(x => x.receiver_account_id == item.user_id).Any())
                    {
                        var member = memberHistory.Where(x => x.receiver_account_id == item.user_id).ToList();
                        foreach (var data in member)
                        {
                            lstMembers.Add(data);
                        }
                    }

                }
                lstMembers = lstMembers.Where(x => x.description.ToLower() == "point recived to admin by superadmin").ToList();
                lstMembers = lstMembers.Select(x => new Wollo.Entities.ViewModels.Issue_Point_Transfer_Detail
                {
                    point_issued_on_date = x.point_issued_on_date.ToLocalTime(),
                    IssuerUsers = x.IssuerUsers,
                    ReceiverUser = x.ReceiverUser,
                    transaction_amount = x.transaction_amount

                }).ToList();

                response = request.CreateResponse<List<Issue_Point_Transfer_Detail>>(HttpStatusCode.OK, lstMembers);
                return response;
            });
        }

        //******************************Code for filtering bt umang 11 10 2016 ***********************************//
        /// <summary>
        /// filter for cash history view detail
        /// </summary>
        /// <param name="request"></param>
        /// <param name="master"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RangeFilterCashHistoryViewDetail(HttpRequestMessage request, Issue_Cash_Transfer_Master master, string userId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                string superadminId = ConfigurationManager.AppSettings["SuperAdminUserId"];
                List<Cash_Transaction_Detail> lstCashTransactionHistory = AutoMapperHelper.GetInstance().Map<List<Cash_Transaction_Detail>>(_cashTransactionDetailHistoryRepository.GetAll().ToList());

                if (userId != superadminId)
                {
                    lstCashTransactionHistory = lstCashTransactionHistory.Where(x => ((x.issuer_account_id == userId) || (x.issuer_account_id == superadminId && x.receiver_account_id == userId)) && (x.description.ToLower() == "cash recived to admin by superadmin" || x.description.ToLower() == "cash issued to member by admin")).ToList();
                }
                else
                {
                    lstCashTransactionHistory = lstCashTransactionHistory.Where(x => (x.issuer_account_id == userId || x.receiver_account_id == superadminId) && (x.description.ToLower() == "cash issued to admin by superadmin" || x.description.ToLower() == "cash issued to self by superadmin" || x.description.ToLower() == "cash issued to member by superadmin" || x.description.ToLower() == "traded price difference" || x.description.ToLower() == "administration fee from buyer" || x.description.ToLower() == "administration fee from seller" || x.description.ToLower() == "withdrawal by member")).ToList();
                }
                if (master.created_date == master.updated_date)
                {
                    lstCashTransactionHistory = lstCashTransactionHistory.Where(x => x.cash_issued_on_date.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    lstCashTransactionHistory = lstCashTransactionHistory.Where(x => x.cash_issued_on_date.Date >= master.created_date.Value.Date && x.cash_issued_on_date.Date <= master.updated_date.Value.Date).ToList();

                }
                lstCashTransactionHistory = lstCashTransactionHistory.Select(x => new Wollo.Entities.ViewModels.Cash_Transaction_Detail
                {
                    id = x.id,
                    cash_issued_on_date = x.description.ToLower() == "traded price difference" || x.description.ToLower() == "administration fee from buyer" || x.description.ToLower() == "administration fee from seller" ? x.cash_issued_on_date : x.cash_issued_on_date.ToLocalTime(),
                    IssuerUsers = x.IssuerUsers,
                    ReceiverUser = x.ReceiverUser,
                    description = x.description,
                    opening_cash = x.opening_cash,
                    closing_cash = x.closing_cash,
                    transaction_amount = x.transaction_amount,

                }).ToList();

                response = request.CreateResponse<List<Cash_Transaction_Detail>>(HttpStatusCode.OK, lstCashTransactionHistory);
                return response;
            });
        }
        /// <summary>
        /// filter for cash issued into system history
        /// </summary>
        /// <param name="request"></param>
        /// <param name="master"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RangeFilterCashIssuedIntoSystemHistory(HttpRequestMessage request, Issue_Cash_Transfer_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Issue_Withdrawel_Permission_Master permission = AutoMapperHelper.GetInstance().Map<Issue_Withdrawel_Permission_Master>(_issueWithdrawelPermissionMasterRepository.GetAll().Where(x => x.permission.ToLower() == "approved").FirstOrDefault());
                List<Issue_Cash_Transfer_Master> lstMembers = new List<Issue_Cash_Transfer_Master>();
                List<Issue_Cash_Transfer_Master> memberHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Cash_Transfer_Master>>(_issueCashTransferMasterRepository.GetAll().Where(x => x.cash_issue_permission_id == permission.id).ToList());
                List<Models.Wallet_Details> AdminId = new List<Models.Wallet_Details>();
                AdminId = _walletDetailsRepository.GetAll().Where(x => x.AspnetRoles.Name.ToLower() == "super admin 1").ToList();
                foreach (var item in AdminId)
                {
                    if (memberHistory.Where(x => x.receiver_account_id == item.user_id).Any())
                    {
                        var member = memberHistory.Where(x => x.receiver_account_id == item.user_id).ToList();
                        foreach (var data in member)
                        {
                            lstMembers.Add(data);
                        }
                    }

                }
                if (master.created_date == master.updated_date)
                {
                    lstMembers = lstMembers.Where(x => x.cash_issued_on_date.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    lstMembers = lstMembers.Where(x => x.cash_issued_on_date.Date >= master.created_date.Value.Date && x.cash_issued_on_date.Date <= master.updated_date.Value.Date).ToList();

                }
                lstMembers = lstMembers.Select(x => new Wollo.Entities.ViewModels.Issue_Cash_Transfer_Master
                {
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    IssuerUser = x.IssuerUser,
                    ReceiverUser = x.ReceiverUser,
                    cash_issued = x.cash_issued

                }).ToList();
                response = request.CreateResponse<List<Issue_Cash_Transfer_Master>>(HttpStatusCode.OK, lstMembers);
                return response;
            });
        }
        /// <summary>
        /// filter for wollo reward point issued into system history
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RangeFilterWolloRewardPointsIssuedIntoSystemHistory(HttpRequestMessage request, Issue_Cash_Transfer_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int stockCode = Convert.ToInt32(ConfigurationManager.AppSettings["FirstStock"]);
                List<Models.Wallet_Details> AdminId = new List<Models.Wallet_Details>();
                List<Issue_Point_Transfer_Detail> lstMembers = new List<Issue_Point_Transfer_Detail>();
                AdminId = _walletDetailsRepository.GetAll().Where(x => x.AspnetRoles.Name.ToLower() == "super admin 1").ToList();
                List<Issue_Point_Transfer_Detail> memberHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Point_Transfer_Detail>>(_issuePointsTransferDetailHistoryDetailsRepository.GetAll().Where(x => x.stock_id == stockCode)).ToList();
                foreach (var item in AdminId)
                {
                    if (memberHistory.Where(x => x.receiver_account_id == item.user_id).Any())
                    {
                        var member = memberHistory.Where(x => x.receiver_account_id == item.user_id).ToList();
                        foreach (var data in member)
                        {
                            lstMembers.Add(data);
                        }
                    }

                }
                lstMembers = lstMembers.Where(x => x.description.ToLower() == "point issued to admin by superadmin").ToList();
                if (master.created_date == master.updated_date)
                {
                    lstMembers = lstMembers.Where(x => x.point_issued_on_date.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    lstMembers = lstMembers.Where(x => x.point_issued_on_date.Date >= master.created_date.Value.Date && x.point_issued_on_date.Date <= master.updated_date.Value.Date).ToList();

                }
                lstMembers = lstMembers.Select(x => new Wollo.Entities.ViewModels.Issue_Point_Transfer_Detail
                {
                    point_issued_on_date = x.point_issued_on_date.ToLocalTime(),
                    IssuerUsers = x.IssuerUsers,
                    ReceiverUser = x.ReceiverUser,
                    transaction_amount = x.transaction_amount

                }).ToList();

                response = request.CreateResponse<List<Issue_Point_Transfer_Detail>>(HttpStatusCode.OK, lstMembers);
                return response;
            });
        }
        /// <summary>
        /// filter for Test Reward Points Issued Into System History
        /// </summary>
        /// <param name="request"></param>
        /// <param name="master"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RangeFilterTestRewardPointsIssuedIntoSystemHistory(HttpRequestMessage request, Issue_Cash_Transfer_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int stockCode = Convert.ToInt32(ConfigurationManager.AppSettings["SecondStock"]);
                List<Models.Wallet_Details> AdminId = new List<Models.Wallet_Details>();
                List<Issue_Point_Transfer_Detail> lstMembers = new List<Issue_Point_Transfer_Detail>();
                AdminId = _walletDetailsRepository.GetAll().Where(x => x.AspnetRoles.Name.ToLower() == "super admin 1").ToList();
                List<Issue_Point_Transfer_Detail> memberHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Point_Transfer_Detail>>(_issuePointsTransferDetailHistoryDetailsRepository.GetAll().Where(x => x.stock_id == stockCode)).ToList();
                foreach (var item in AdminId)
                {
                    if (memberHistory.Where(x => x.receiver_account_id == item.user_id).Any())
                    {
                        var member = memberHistory.Where(x => x.receiver_account_id == item.user_id).ToList();
                        foreach (var data in member)
                        {
                            lstMembers.Add(data);
                        }
                    }

                }
                lstMembers = lstMembers.Where(x => x.description.ToLower() == "point issued to admin by superadmin").ToList();
                if (master.created_date == master.updated_date)
                {
                    lstMembers = lstMembers.Where(x => x.point_issued_on_date.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    lstMembers = lstMembers.Where(x => x.point_issued_on_date.Date >= master.created_date.Value.Date && x.point_issued_on_date.Date <= master.updated_date.Value.Date).ToList();

                }
                lstMembers = lstMembers.Select(x => new Wollo.Entities.ViewModels.Issue_Point_Transfer_Detail
                {
                    point_issued_on_date = x.point_issued_on_date.ToLocalTime(),
                    IssuerUsers = x.IssuerUsers,
                    ReceiverUser = x.ReceiverUser,
                    transaction_amount = x.transaction_amount

                }).ToList();

                response = request.CreateResponse<List<Issue_Point_Transfer_Detail>>(HttpStatusCode.OK, lstMembers);
                return response;
            });
        }
        /// <summary>
        /// Range Filter Cash Issued To Members History
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RangeFilterCashIssuedToMembersHistory(HttpRequestMessage request, Issue_Cash_Transfer_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Issue_Withdrawel_Permission_Master permission = AutoMapperHelper.GetInstance().Map<Issue_Withdrawel_Permission_Master>(_issueWithdrawelPermissionMasterRepository.GetAll().Where(x => x.permission.ToLower() == "approved").FirstOrDefault());
                List<Issue_Cash_Transfer_Master> lstMembers = new List<Issue_Cash_Transfer_Master>();
                List<Issue_Cash_Transfer_Master> memberHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Cash_Transfer_Master>>(_issueCashTransferMasterRepository.GetAll().Where(x => x.cash_issue_permission_id == permission.id).ToList());
                List<Models.Wallet_Details> memberId = new List<Models.Wallet_Details>();
                memberId = _walletDetailsRepository.GetAll().Where(x => x.AspnetRoles.Name.ToLower() == "member").ToList();
                foreach (var item in memberId)
                {
                    if (memberHistory.Where(x => x.receiver_account_id == item.user_id).Any())
                    {
                        var member = memberHistory.Where(x => x.receiver_account_id == item.user_id).ToList();
                        foreach (var data in member)
                        {
                            lstMembers.Add(data);
                        }
                    }

                }
                if (master.created_date == master.updated_date)
                {
                    lstMembers = lstMembers.Where(x => x.cash_issued_on_date.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    lstMembers = lstMembers.Where(x => x.cash_issued_on_date.Date >= master.created_date.Value.Date && x.cash_issued_on_date.Date <= master.updated_date.Value.Date).ToList();

                }
                lstMembers = lstMembers.Select(x => new Wollo.Entities.ViewModels.Issue_Cash_Transfer_Master
                {
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    IssuerUser = x.IssuerUser,
                    ReceiverUser = x.ReceiverUser,
                    cash_issued = x.cash_issued

                }).ToList();
                response = request.CreateResponse<List<Issue_Cash_Transfer_Master>>(HttpStatusCode.OK, lstMembers);
                return response;
            });
        }
        /// <summary>
        /// Range Filter Wollo Reward Points Issued To Members History
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RangeFilterWolloRewardPointsIssuedToMembersHistory(HttpRequestMessage request, Issue_Points_Transfer_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int stockCode = Convert.ToInt32(ConfigurationManager.AppSettings["FirstStock"]);
                List<Models.Wallet_Details> memberId = new List<Models.Wallet_Details>();
                List<Issue_Point_Transfer_Detail> lstMembers = new List<Issue_Point_Transfer_Detail>();
                memberId = _walletDetailsRepository.GetAll().Where(x => x.AspnetRoles.Name.ToLower() == "member").ToList();
                List<Issue_Point_Transfer_Detail> memberHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Point_Transfer_Detail>>(_issuePointsTransferDetailHistoryDetailsRepository.GetAll().Where(x => x.stock_id == stockCode)).ToList();
                foreach (var item in memberId)
                {
                    if (memberHistory.Where(x => x.receiver_account_id == item.user_id).Any())
                    {
                        var member = memberHistory.Where(x => x.receiver_account_id == item.user_id).ToList();
                        foreach (var data in member)
                        {
                            lstMembers.Add(data);
                        }
                    }

                }
                lstMembers = lstMembers.Where(x => x.description.ToLower() == "point issued to member by admin" || x.description.ToLower() == "point issued to member by superadmin").ToList();
                if (master.created_date == master.updated_date)
                {
                    lstMembers = lstMembers.Where(x => x.point_issued_on_date.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    lstMembers = lstMembers.Where(x => x.point_issued_on_date.Date >= master.created_date.Value.Date && x.point_issued_on_date.Date <= master.updated_date.Value.Date).ToList();

                }
                lstMembers = lstMembers.Select(x => new Wollo.Entities.ViewModels.Issue_Point_Transfer_Detail
                {
                    point_issued_on_date = x.point_issued_on_date.ToLocalTime(),
                    IssuerUsers = x.IssuerUsers,
                    ReceiverUser = x.ReceiverUser,
                    transaction_amount = x.transaction_amount

                }).ToList();

                response = request.CreateResponse<List<Issue_Point_Transfer_Detail>>(HttpStatusCode.OK, lstMembers);
                return response;
            });
        }
        /// <summary>
        /// Range Filter Test Reward Points Issued To Members History
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RangeFilterTestRewardPointsIssuedToMembersHistory(HttpRequestMessage request, Issue_Points_Transfer_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int stockCode = Convert.ToInt32(ConfigurationManager.AppSettings["SecondStock"]);
                List<Models.Wallet_Details> memberId = new List<Models.Wallet_Details>();
                List<Issue_Point_Transfer_Detail> lstMembers = new List<Issue_Point_Transfer_Detail>();
                memberId = _walletDetailsRepository.GetAll().Where(x => x.AspnetRoles.Name.ToLower() == "member").ToList();
                List<Issue_Point_Transfer_Detail> memberHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Point_Transfer_Detail>>(_issuePointsTransferDetailHistoryDetailsRepository.GetAll().Where(x => x.stock_id == stockCode)).ToList();
                foreach (var item in memberId)
                {
                    if (memberHistory.Where(x => x.receiver_account_id == item.user_id).Any())
                    {
                        var member = memberHistory.Where(x => x.receiver_account_id == item.user_id).ToList();
                        foreach (var data in member)
                        {
                            lstMembers.Add(data);
                        }
                    }

                }
                lstMembers = lstMembers.Where(x => x.description.ToLower() == "point issued to member by admin" || x.description.ToLower() == "point issued to member by superadmin").ToList();
                if (master.created_date == master.updated_date)
                {
                    lstMembers = lstMembers.Where(x => x.point_issued_on_date.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    lstMembers = lstMembers.Where(x => x.point_issued_on_date.Date >= master.created_date.Value.Date && x.point_issued_on_date.Date <= master.updated_date.Value.Date).ToList();

                }
                lstMembers = lstMembers.Select(x => new Wollo.Entities.ViewModels.Issue_Point_Transfer_Detail
                {
                    point_issued_on_date = x.point_issued_on_date.ToLocalTime(),
                    IssuerUsers = x.IssuerUsers,
                    ReceiverUser = x.ReceiverUser,
                    transaction_amount = x.transaction_amount

                }).ToList();

                response = request.CreateResponse<List<Issue_Point_Transfer_Detail>>(HttpStatusCode.OK, lstMembers);
                return response;
            });
        }

        /// <summary>
        /// filter Wollo Reward Points Transferred In By Members History
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost]
        public HttpResponseMessage RangeFilterWolloRewardPointsTransferredInByMembersHistory(HttpRequestMessage request, Issue_Points_Transfer_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Transfer_Action_Master permission = AutoMapperHelper.GetInstance().Map<Transfer_Action_Master>(_transferActionMasterHistoryRepository.GetAll().Where(x => x.name.ToLower() == "wollo to rpe").FirstOrDefault());
                List<Reward_Points_Transfer_Details> rewardPointInHistory = AutoMapperHelper.GetInstance().Map<List<Reward_Points_Transfer_Details>>(_rewardPointsTransferDetailsRepository.GetAll().Where(x => x.transfer_actionid == permission.id).ToList());
                if (master.created_date == master.updated_date)
                {
                    rewardPointInHistory = rewardPointInHistory.Where(x => x.TransferActionMaster.updated_date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    rewardPointInHistory = rewardPointInHistory.Where(x => x.TransferActionMaster.updated_date >= master.created_date.Value.Date && x.TransferActionMaster.updated_date <= master.updated_date.Value.Date).ToList();

                }
                rewardPointInHistory = rewardPointInHistory.Select(x => new Wollo.Entities.ViewModels.Reward_Points_Transfer_Details
                {
                    TransferActionMaster = x.TransferActionMaster,
                    RewardPointTransferMaster = x.RewardPointTransferMaster,
                    points_transferred = x.points_transferred

                }).ToList();
                response = request.CreateResponse<List<Reward_Points_Transfer_Details>>(HttpStatusCode.OK, rewardPointInHistory);
                return response;
            });
        }
        /// <summary>
        /// filter Wollo Reward Points Transferred out By Members History
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RangeFilterWolloRewardPointsTransferredOutByMembersHistory(HttpRequestMessage request, Issue_Points_Transfer_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Transfer_Action_Master permission = AutoMapperHelper.GetInstance().Map<Transfer_Action_Master>(_transferActionMasterHistoryRepository.GetAll().Where(x => x.name.ToLower() == "rpe to wollo").FirstOrDefault());
                List<Reward_Points_Transfer_Details> rewardPointInHistory = AutoMapperHelper.GetInstance().Map<List<Reward_Points_Transfer_Details>>(_rewardPointsTransferDetailsRepository.GetAll().Where(x => x.transfer_actionid == permission.id).ToList());
                if (master.created_date == master.updated_date)
                {
                    rewardPointInHistory = rewardPointInHistory.Where(x => x.TransferActionMaster.updated_date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    rewardPointInHistory = rewardPointInHistory.Where(x => x.TransferActionMaster.updated_date >= master.created_date.Value.Date && x.TransferActionMaster.updated_date <= master.updated_date.Value.Date).ToList();

                }
                rewardPointInHistory = rewardPointInHistory.Select(x => new Wollo.Entities.ViewModels.Reward_Points_Transfer_Details
                {
                    TransferActionMaster = x.TransferActionMaster,
                    RewardPointTransferMaster = x.RewardPointTransferMaster,
                    points_transferred = x.points_transferred

                }).ToList();
                response = request.CreateResponse<List<Reward_Points_Transfer_Details>>(HttpStatusCode.OK, rewardPointInHistory);
                return response;
            });
        }

        /// <summary>
        /// filter for topup by member history detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RangeFilterTopupByMembersHistory(HttpRequestMessage request, Issue_Points_Transfer_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Topup_Status_Master topupStatus = AutoMapperHelper.GetInstance().Map<Topup_Status_Master>(_topupStatusMasterRepository.GetAll().Where(x => x.status.ToLower() == "completed").FirstOrDefault());
                List<Topup_History> topupByMemberHistory = AutoMapperHelper.GetInstance().Map<List<Topup_History>>(_topupHistoryRepository.GetAll().Where(x => x.topup_status_id == topupStatus.id).ToList());
                if (master.created_date == master.updated_date)
                {
                    topupByMemberHistory = topupByMemberHistory.Where(x => x.updated_date.Value.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    topupByMemberHistory = topupByMemberHistory.Where(x => x.updated_date.Value.Date >= master.created_date.Value.Date && x.updated_date.Value.Date <= master.updated_date.Value.Date).ToList();

                }
                topupByMemberHistory = topupByMemberHistory.Select(x => new Wollo.Entities.ViewModels.Topup_History
                {
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    AspnetUsers = x.AspnetUsers,
                    amount = x.amount

                }).ToList();
                response = request.CreateResponse<List<Topup_History>>(HttpStatusCode.OK, topupByMemberHistory);
                return response;
            });
        }

        /// <summary>
        /// filter for withdraw out by member history detail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost]
        public HttpResponseMessage RangeFilterWithdrawOutByMembersHistory(HttpRequestMessage request, Issue_Points_Transfer_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Withdrawl_Status_Master withdrawStatus = AutoMapperHelper.GetInstance().Map<Withdrawl_Status_Master>(_withdrawlStatusMasterRepository.GetAll().Where(x => x.status.ToLower() == "approved").FirstOrDefault());
                List<Withdrawel_History_Details> withdrawOutByMemberHistory = AutoMapperHelper.GetInstance().Map<List<Withdrawel_History_Details>>(_withdrawelHistoryDetailsRepository.GetAll().Where(x => x.withdrawel_status_id == withdrawStatus.id).ToList());
                if (master.created_date == master.updated_date)
                {
                    withdrawOutByMemberHistory = withdrawOutByMemberHistory.Where(x => x.WithdrawelHistoryMaster.updated_date.Value.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    withdrawOutByMemberHistory = withdrawOutByMemberHistory.Where(x => x.WithdrawelHistoryMaster.updated_date.Value.Date >= master.created_date.Value.Date && x.WithdrawelHistoryMaster.updated_date.Value.Date <= master.updated_date.Value.Date).ToList();

                }
                withdrawOutByMemberHistory = withdrawOutByMemberHistory.Select(x => new Wollo.Entities.ViewModels.Withdrawel_History_Details
                {
                    WithdrawelHistoryMaster = x.WithdrawelHistoryMaster,
                    AspnetUsers = x.AspnetUsers,
                    amount = x.amount

                }).ToList();
                response = request.CreateResponse<List<Withdrawel_History_Details>>(HttpStatusCode.OK, withdrawOutByMemberHistory);
                return response;
            });
        }
        //**********************************************end here*************************************************//

        [HttpPost]
        public HttpResponseMessage GetSystemCashDetails(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Cash_Transaction_Detail> objTransactionDetails = AutoMapperHelper.GetInstance().Map<List<Cash_Transaction_Detail>>(_cashTransactionDetailHistoryRepository.GetAll().Where(x => x.description.ToLower() == "traded price difference" || x.description.ToLower() == "administration fee from seller" || x.description.ToLower() == "administration fee from buyer" || x.description.ToLower() == "withdrawal by member").ToList());
                objTransactionDetails = objTransactionDetails.Select(x => new Wollo.Entities.ViewModels.Cash_Transaction_Detail
                {
                    id = x.id,
                    issuer_account_id = x.issuer_account_id,
                    IssuerUsers = x.IssuerUsers,
                    receiver_account_id = x.receiver_account_id,
                    ReceiverUser = x.ReceiverUser,
                    cash_issued_on_date = x.cash_issued_on_date.ToLocalTime(),
                    opening_cash = x.opening_cash,
                    transaction_amount = x.transaction_amount,
                    closing_cash = x.closing_cash,
                    description = x.description
                }).ToList();
                response = request.CreateResponse<List<Cash_Transaction_Detail>>(HttpStatusCode.OK, objTransactionDetails);
                return response;
            });
        }

        //for cash withdrawal
        [HttpPost]
        public HttpResponseMessage RangeFilterSystemCashDetails(HttpRequestMessage request, Cash_Transaction_History master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Cash_Transaction_Detail> objTransactionDetails = AutoMapperHelper.GetInstance().Map<List<Cash_Transaction_Detail>>(_cashTransactionDetailHistoryRepository.GetAll().Where(x => x.description.ToLower() == "traded price difference" || x.description.ToLower() == "administration fee from seller" || x.description.ToLower() == "administration fee from buyer" || x.description.ToLower() == "withdrawal by member").ToList());
                objTransactionDetails = objTransactionDetails.Select(x => new Wollo.Entities.ViewModels.Cash_Transaction_Detail
                {
                    id = x.id,
                    issuer_account_id = x.issuer_account_id,
                    IssuerUsers = x.IssuerUsers,
                    receiver_account_id = x.receiver_account_id,
                    ReceiverUser = x.ReceiverUser,
                    cash_issued_on_date = x.cash_issued_on_date.ToLocalTime(),
                    opening_cash = x.opening_cash,
                    transaction_amount = x.transaction_amount,
                    closing_cash = x.closing_cash,
                    description = x.description
                }).ToList();
                if (master.created_date == master.updated_date)
                {
                    objTransactionDetails = objTransactionDetails.Where(x => x.cash_issued_on_date.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    objTransactionDetails = objTransactionDetails.Where(x => x.cash_issued_on_date.Date >= master.created_date.Value.Date && x.cash_issued_on_date.Date <= master.updated_date.Value.Date).ToList();
                }
                response = request.CreateResponse<List<Cash_Transaction_Detail>>(HttpStatusCode.OK, objTransactionDetails);
                return response;

            });
        }

        [HttpPost]
        public HttpResponseMessage RangeFilterUser(HttpRequestMessage request, Withdrawel_History_Master master, string userId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                WithdrawalData objWithdrawalData = new WithdrawalData();
                Wallet_Details objWalletDetails = AutoMapperHelper.GetInstance().Map<Wallet_Details>(_walletDetailsRepository.FindBy(x => x.user_id == userId).FirstOrDefault());
                if (objWalletDetails != null)
                {
                    objWithdrawalData.Cash = objWalletDetails.cash;
                }
                List<Withdrawel_History_Details> lstWithdrawalHistoryDetails = new List<Withdrawel_History_Details>();
                lstWithdrawalHistoryDetails = AutoMapperHelper.GetInstance().Map<List<Withdrawel_History_Details>>(_withdrawelHistoryDetailsRepository.FindBy(x => x.withdrawer_user_id == userId).ToList());
                lstWithdrawalHistoryDetails.ToList().ForEach(c => c.WithdrawelHistoryMaster = new Withdrawel_History_Master
                {
                    created_date = c.WithdrawelHistoryMaster.updated_date.HasValue ? c.WithdrawelHistoryMaster.updated_date.Value.ToLocalTime() : c.WithdrawelHistoryMaster.updated_date,
                    updated_date = c.WithdrawelHistoryMaster.updated_date.HasValue ? c.WithdrawelHistoryMaster.updated_date.Value.ToLocalTime() : c.WithdrawelHistoryMaster.updated_date,
                    updated_by = c.WithdrawelHistoryMaster.updated_by,
                    created_by = c.WithdrawelHistoryMaster.created_by,
                    id = c.WithdrawelHistoryMaster.id
                });
                objWithdrawalData.lstWithdrawalHistoryDeatils = lstWithdrawalHistoryDetails;
                if (master.created_date == master.updated_date)
                {
                    objWithdrawalData.lstWithdrawalHistoryDeatils = lstWithdrawalHistoryDetails.Where(x => x.WithdrawelHistoryMaster.created_date.Value.Date == master.created_date.Value.Date).ToList();
                }

                else
                {
                    objWithdrawalData.lstWithdrawalHistoryDeatils = lstWithdrawalHistoryDetails.Where(x => x.WithdrawelHistoryMaster.created_date.Value.Date >= master.created_date.Value.Date && x.WithdrawelHistoryMaster.created_date.Value.Date <= master.updated_date.Value.Date).ToList();
                }
                List<Withdrawl_Status_Master> lstWithdrawalStatusMaster = new List<Withdrawl_Status_Master>();
                lstWithdrawalStatusMaster = AutoMapperHelper.GetInstance().Map<List<Withdrawl_Status_Master>>(_withdrawlStatusMasterRepository.GetAll().ToList());
                objWithdrawalData.lstWithdrawalStatusMaster = lstWithdrawalStatusMaster;
                response = request.CreateResponse<WithdrawalData>(HttpStatusCode.OK, objWithdrawalData);
                return response;
            });
        }


        [HttpPost]
        public HttpResponseMessage CashTransactionFilterUser(HttpRequestMessage request, Cash_Transaction_History master, string userId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Cash_Transaction_History> lstCashTransactionHistory = AutoMapperHelper.GetInstance().Map<List<Cash_Transaction_History>>(_cashTransactionHistoryRepository.FindBy(x => x.user_id == userId).ToList());
                lstCashTransactionHistory = lstCashTransactionHistory.Select(x => new Wollo.Entities.ViewModels.Cash_Transaction_History
                {
                    id = x.id,
                    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    user_id = x.user_id,
                    description = x.description,
                    opening_cash = x.opening_cash,
                    closing_cash = x.closing_cash,
                    transaction_amount = x.transaction_amount,
                    AspNetUsers = x.AspNetUsers
                }).ToList();
                if (master.created_date == master.updated_date)
                {
                    lstCashTransactionHistory = lstCashTransactionHistory.Where(x => x.created_date.Value.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    lstCashTransactionHistory = lstCashTransactionHistory.Where(x => x.created_date.Value.Date >= master.created_date.Value.Date && x.created_date.Value.Date <= master.updated_date.Value.Date).ToList();

                }
                response = request.CreateResponse<List<Cash_Transaction_History>>(HttpStatusCode.OK, lstCashTransactionHistory);
                return response;
            });
        }
    }
}
