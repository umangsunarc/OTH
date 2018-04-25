using Wollo.Common.AutoMapper;
using Wollo.Data.Infrastructure;
using Wollo.Data.Repositories;
using Wollo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wollo.Entities.ViewModels;
using System.Diagnostics;
using Wollo.API.Infrastructure.Core;
using Models = Wollo.Entities.Models;
using System.Web.Http.Cors;

namespace Wollo.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TradingController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Models.Transaction_History> _transactionRepository;
        private readonly IEntityBaseRepository<Models.Issue_Cash_Transfer_Master> _issueCashTransferMaster;
        private readonly IEntityBaseRepository<Models.Traded_History_Master> _tradedHistoryRepository;
        private readonly IEntityBaseRepository<Models.Queue_Status_Master> _queueStatusRepository;
        private readonly IEntityBaseRepository<Models.Stock_Code> _stockHistoryRepository;
        private readonly IEntityBaseRepository<Models.Issue_Withdrawel_Permission_Master> _issueWithdrawelPermissionMaster;
        private readonly IEntityBaseRepository<Models.Member_Stock_Details> _memberStockDetailsRepository;
        private readonly IEntityBaseRepository<Models.Wallet_Details> _walletDetailsRepository;
        private readonly IEntityBaseRepository<Models.Units_Master> _unitMasterRepository;
        private readonly IEntityBaseRepository<Models.Market_Rate_Details> _marketRateDetailsRepository;
        private readonly IEntityBaseRepository<Models.Withdrawel_History_Details> _withdrawelHistoryDetailsRepository;
        
        public TradingController(IEntityBaseRepository<Models.Transaction_History> transactionRepository,
            IEntityBaseRepository<Models.Traded_History_Master> tradedHistoryRepository,
            IEntityBaseRepository<Models.Issue_Withdrawel_Permission_Master> issueWithdrawelPermissionMaster,
            IEntityBaseRepository<Models.Issue_Cash_Transfer_Master> issueCashTransferMaster,
            IEntityBaseRepository<Models.Queue_Status_Master> queueStatusRepository,
            IEntityBaseRepository<Models.Stock_Code> stockHistoryRepository,
            IEntityBaseRepository<Models.Member_Stock_Details> memberStockDetailsRepository,
            IEntityBaseRepository<Models.Wallet_Details> walletDetailsRepository,
            IEntityBaseRepository<Models.Units_Master> unitMasterRepository,
            IEntityBaseRepository<Models.Market_Rate_Details> _marketRateDetailsRepository,
            IEntityBaseRepository<Models.Withdrawel_History_Details> withdrawelHistoryDetailsRepository,
             IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _issueCashTransferMaster = issueCashTransferMaster;
            _issueWithdrawelPermissionMaster = issueWithdrawelPermissionMaster;
            _tradedHistoryRepository = tradedHistoryRepository;
            _queueStatusRepository = queueStatusRepository;
            _stockHistoryRepository = stockHistoryRepository;
            _memberStockDetailsRepository = memberStockDetailsRepository;
            _walletDetailsRepository = walletDetailsRepository;
            _unitMasterRepository = unitMasterRepository;
            _withdrawelHistoryDetailsRepository = withdrawelHistoryDetailsRepository;
        }

        /// <summary>
        /// Add bid or ask in transaction history
        /// </summary>
        /// <param name="request"></param>
        /// <param name="objTransactionHistory"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddBidAsk(HttpRequestMessage request, Wollo.Entities.Models.Transaction_History objTransactionHistory)
        {
            HttpResponseMessage response = null;
            //_trsansactionRepository.Add(objTransactionHistory);

            Models.Units_Master unitMasterData = new Models.Units_Master();
            if (_unitMasterRepository.GetAll().Where(x=>x.stock_id==objTransactionHistory.stock_code_id).Any())
            {
                unitMasterData = _unitMasterRepository.GetAll().Where(x=>x.stock_id==objTransactionHistory.stock_code_id).FirstOrDefault();
            }
            else
            {
                unitMasterData.points_equivalent = 1;
                unitMasterData.minimum_lot = 1;
                unitMasterData.minimum_rate = 0.1F;
            }

            //Models.Units_Master unitMasterData = new Models.Units_Master();
            //if (_unitMasterRepository.GetAll().ToList().Count == 1)
            //{
            //    unitMasterData = _unitMasterRepository.GetAll().FirstOrDefault();
            //}
            //else
            //{
            //    unitMasterData.points_equivalent = 1;
            //    unitMasterData.minimum_lot = 1;
            //    unitMasterData.minimum_rate = 0.1F;
            //}

            //Models.Market_Rate_Details initialMarketRateDetails = new Models.Market_Rate_Details();
            //if (_marketRateDetailsRepository.GetAll().ToList().Count == 1)
            //{
            //    initialMarketRateDetails = _marketRateDetailsRepository.GetAll().FirstOrDefault();
            //}
            //else
            //{
            //    initialMarketRateDetails.rate = 0.1F;
            //}

            Debug.WriteLine("Trading Statrted");
            int result = 0;
            //Calculate reward points from lot*rewardpoint
            int points = objTransactionHistory.reward_points * unitMasterData.points_equivalent;
            float rate = (float)Math.Round((double)objTransactionHistory.price / (double)points, 3);
            if (rate >= unitMasterData.minimum_rate && objTransactionHistory.reward_points >= unitMasterData.minimum_lot)
            {
                objTransactionHistory.rate = rate;
                objTransactionHistory.reward_points = points;
                objTransactionHistory.original_order_quantity = points;
                if (objTransactionHistory.queue_action.ToLower() == "ask")
                {
                    if (_memberStockDetailsRepository.FindBy(x => x.user_id == objTransactionHistory.user_id && x.stock_code_id == objTransactionHistory.stock_code_id).Any())
                    {
                        int totalStockInQueue = 0;
                        Wollo.Entities.Models.Member_Stock_Details objPointDetails = _memberStockDetailsRepository.FindBy(x => x.user_id == objTransactionHistory.user_id && x.stock_code_id == objTransactionHistory.stock_code_id).FirstOrDefault();
                        if (_transactionRepository.FindBy(x => x.user_id == objTransactionHistory.user_id && x.stock_code_id == objTransactionHistory.stock_code_id && x.status_id == 1 && x.queue_action.ToLower().Trim()=="ask").Any())
                        {
                            totalStockInQueue = _transactionRepository.GetAll().Where(x => x.user_id == objTransactionHistory.user_id && x.stock_code_id == objTransactionHistory.stock_code_id && x.status_id == 1 && x.queue_action.ToLower().Trim() == "ask").Select(y => y.reward_points).Sum();
                        }
                        int buyableQuantity = objPointDetails.stock_amount - totalStockInQueue;
                        if (objTransactionHistory.reward_points <= buyableQuantity)
                        {
                            objTransactionHistory.status_id = _queueStatusRepository.FindBy(x => x.name == "Queued").FirstOrDefault().id;
                            //objTransactionHistory.rate = (float)Math.Round((double)objTransactionHistory.price / (double)objTransactionHistory.reward_points, 3);
                            objTransactionHistory.created_by = objTransactionHistory.user_id;
                            objTransactionHistory.created_date = DateTime.UtcNow;
                            objTransactionHistory.updated_by = objTransactionHistory.user_id;
                            objTransactionHistory.updated_date = DateTime.UtcNow;
                            _transactionRepository.Add(objTransactionHistory);
                            result = 1;
                        }
                        else
                        {
                            result = 2;
                        }
                    }
                    else
                    {
                        //no member stock details found
                        result = 5;
                    }
                }
                else
                {
                    if (_walletDetailsRepository.FindBy(x => x.user_id == objTransactionHistory.user_id).Any())
                    {
                        float TotalOrderPriceSum = 0;
                        float pendingWithdrawalSum = 0;
                        Wollo.Entities.Models.Wallet_Details objWalletDetails = _walletDetailsRepository.FindBy(x => x.user_id == objTransactionHistory.user_id).FirstOrDefault();
                        if (_transactionRepository.FindBy(x => x.user_id == objTransactionHistory.user_id && x.status_id == 1).Any())
                        {
                            TotalOrderPriceSum = _transactionRepository.GetAll().Where(x => x.user_id == objTransactionHistory.user_id && x.status_id == 1).Select(y => y.price).Sum();
                        }
                        if (_withdrawelHistoryDetailsRepository.FindBy(x => x.withdrawer_user_id == objTransactionHistory.user_id && x.withdrawel_status_id == 1).Any())
                        {
                            pendingWithdrawalSum = _withdrawelHistoryDetailsRepository.GetAll().Where(x => x.withdrawer_user_id == objTransactionHistory.user_id && x.withdrawel_status_id == 1).Select(y => y.amount).Sum();
                        }
                        float buyablePrice = objWalletDetails.cash - TotalOrderPriceSum - pendingWithdrawalSum;
                        if (objTransactionHistory.price <= buyablePrice)
                        {
                            objTransactionHistory.status_id = _queueStatusRepository.FindBy(x => x.name == "Queued").FirstOrDefault().id;
                            //objTransactionHistory.rate = (float)Math.Round((double)objTransactionHistory.price / (double)objTransactionHistory.reward_points, 3);
                            objTransactionHistory.created_by = objTransactionHistory.user_id;
                            objTransactionHistory.created_date = DateTime.UtcNow;
                            objTransactionHistory.updated_by = objTransactionHistory.user_id;
                            objTransactionHistory.updated_date = DateTime.UtcNow;
                            _transactionRepository.Add(objTransactionHistory);
                            result = 1;
                        }
                        else
                        {
                            result = 2;
                        }
                    }
                    else
                    {
                        //No wallet details found for the current user
                        result = 0;
                    }

                }
            }
            else
            {
                if (rate < unitMasterData.minimum_rate)
                {
                    result = 3;
                }
                else
                {
                    result = 4;
                }
                
            }

            response = request.CreateResponse<long>(HttpStatusCode.OK, result);
            return response;
        }

        /// <summary>
        /// Get all transaction history
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllTransactionHistory(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var histories = _transactionRepository.GetAll().ToList();
                List<Wollo.Entities.ViewModels.Transaction_History> lstTransactionHistory = AutoMapperHelper.GetInstance().Map<List<Wollo.Entities.ViewModels.Transaction_History>>(histories);
                lstTransactionHistory = lstTransactionHistory.Select(x => new Transaction_History
                {
                    id = x.id,
                    original_order_quantity = x.original_order_quantity,
                    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    reward_points = x.reward_points,
                    user_id = x.user_id,
                    stock_code_id = x.stock_code_id,
                    StockCode = x.StockCode,
                    price = x.price,
                    rate = x.rate,
                    queue_action = x.queue_action,
                    QueueStatusMaster = x.QueueStatusMaster,
                    status_id = x.status_id
                }).ToList();
                response = request.CreateResponse<IEnumerable<Wollo.Entities.ViewModels.Transaction_History>>(HttpStatusCode.OK, lstTransactionHistory);
                return response;
            });
        }

        ///// <summary>
        ///// Get Latest maximum bid rate and minimum ask rate by selected stock
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="StockId"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public HttpResponseMessage GetLatestBidAskByStock(HttpRequestMessage request, int StockId)
        //{
        //    return CreateHttpResponse(request, () =>
        //    {
        //        HttpResponseMessage response = null;
        //        QueueData model = new QueueData();
        //        var histories = _transactionRepository.GetAll().ToList();
        //        List<Wollo.Entities.ViewModels.Transaction_History> lstTransactionHistory = AutoMapperHelper.GetInstance().Map<List<Wollo.Entities.ViewModels.Transaction_History>>(histories);
        //        List<Traded_History_Master> tradedHistoryResult = new List<Traded_History_Master>();
        //        tradedHistoryResult = AutoMapperHelper.GetInstance().Map<List<Traded_History_Master>>(_tradedHistoryRepository.FindBy(x => x.stock == StockId).ToList());
        //        if (lstTransactionHistory.Where(j => j.status_id == 1 && j.stock_code_id == StockId && j.queue_action == "bid").Any())
        //        {
        //            model.highestBidRate = lstTransactionHistory.Where(j => j.status_id == 1 && j.stock_code_id == StockId && j.queue_action == "bid").Max(i => i.rate);
        //        }
        //        else
        //        {
        //            model.highestBidRate = 0.0F;
        //        }
        //        if (lstTransactionHistory.Where(j => j.status_id == 1 && j.stock_code_id == StockId && j.queue_action == "ask").Any())
        //        {
        //            model.LowestAskRate = lstTransactionHistory.Where(j => j.status_id == 1 && j.stock_code_id == StockId && j.queue_action == "ask").Min(i => i.rate);
        //        }
        //        else
        //        {
        //            model.LowestAskRate = 0.0F;
        //        }
        //        if (tradedHistoryResult.Count > 0)
        //        {
        //            model.lastTradedPrice = tradedHistoryResult.Last().bid_price;
        //        }
        //        response = request.CreateResponse<Wollo.Entities.ViewModels.QueueData>(HttpStatusCode.OK, model);
        //        return response;
        //    });
        //}

        /// <summary>
        /// Cancel transaction
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage CancelTransaction(HttpRequestMessage request, int Id, string userId)
        {
            HttpResponseMessage response = null;
            int result = 0;
            Models.Transaction_History objTransactionHistory = _transactionRepository.FindBy(x => x.id == Id).SingleOrDefault();
            objTransactionHistory.status_id = _queueStatusRepository.FindBy(x => x.name == "Cancelled").FirstOrDefault().id;
            objTransactionHistory.updated_by = userId;
            objTransactionHistory.updated_date = DateTime.UtcNow;
            _transactionRepository.Edit(objTransactionHistory);
            result = 1;
            response = request.CreateResponse<int>(HttpStatusCode.OK, result);
            return response;
        }

        /// <summary>
        /// Update transaction
        /// </summary>
        /// <param name="request"></param>
        /// <param name="objTransactionHistory"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateTransaction(HttpRequestMessage request, Transaction_History objTransactionHistory)
        {
            HttpResponseMessage response = null;
            int result = 0;
            Models.Units_Master unitMasterData = new Models.Units_Master();
            if (_unitMasterRepository.GetAll().ToList().Count == 1)
            {
                unitMasterData = _unitMasterRepository.GetAll().FirstOrDefault();
            }
            else
            {
                unitMasterData.points_equivalent = objTransactionHistory.reward_points;
                unitMasterData.minimum_lot = 1;
                unitMasterData.minimum_rate = 0.1F;
            }
            int points = objTransactionHistory.reward_points;
            objTransactionHistory.rate = (float)Math.Round((double)objTransactionHistory.price / (double)points, 3);
            float currentLot = objTransactionHistory.reward_points / unitMasterData.points_equivalent;
            if (objTransactionHistory.rate >= unitMasterData.minimum_rate && currentLot >= unitMasterData.minimum_lot)
            {
                Models.Transaction_History transactionHistory = _transactionRepository.FindBy(x => x.id == objTransactionHistory.id).SingleOrDefault();
                //transactionHistory.rate = (float)Math.Round((double)objTransactionHistory.price / (double)objTransactionHistory.reward_points, 3);
                transactionHistory.rate = objTransactionHistory.rate;
                transactionHistory.price = objTransactionHistory.price;
                if (transactionHistory.reward_points != objTransactionHistory.reward_points)
                {
                    transactionHistory.reward_points = objTransactionHistory.reward_points;
                    transactionHistory.original_order_quantity = objTransactionHistory.reward_points;
                }
                transactionHistory.updated_by = objTransactionHistory.updated_by;
                transactionHistory.updated_date = DateTime.UtcNow;
                _transactionRepository.Edit(transactionHistory);
                result = 1;
            }
            else
            {
                if (objTransactionHistory.rate < unitMasterData.minimum_rate)
                {
                    result = 3;
                }
                else
                {
                    result = 4;
                }
            }
            response = request.CreateResponse<int>(HttpStatusCode.OK, result);
            return response;
        }

        /// <summary>
        /// Get all transaction history by user Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllTransactionHistoryByUser(HttpRequestMessage request, string userId)
        {
            HttpResponseMessage response = null;
            TradingViewModel model = new TradingViewModel();
            List<Transaction_History> listHistory = AutoMapperHelper.GetInstance().Map<List<Transaction_History>>(_transactionRepository.GetAll().ToList().OrderByDescending(y => y.updated_date));
            List<Stock_Code> lstStockCode = AutoMapperHelper.GetInstance().Map<List<Stock_Code>>(_stockHistoryRepository.GetAll().ToList());
            Units_Master unitMasterData = AutoMapperHelper.GetInstance().Map<Units_Master>(_unitMasterRepository.GetAll().FirstOrDefault());
            if (unitMasterData == null)
            {
                unitMasterData = new Units_Master();
                unitMasterData.minimum_lot = 0;
                unitMasterData.minimum_rate = 0;
                unitMasterData.points_equivalent = 1;
                unitMasterData.unit = "Lot";
                unitMasterData.updated_date = DateTime.UtcNow;
                unitMasterData.created_date = DateTime.UtcNow;
                unitMasterData.created_by = "System";
                unitMasterData.updated_by = "System";
            }
            model.Transaction_History = listHistory;
            model.Stock_Code = lstStockCode;
            int defaultStockId = model.Stock_Code.FirstOrDefault().id;
            model.unit_master = unitMasterData;
            if (listHistory.Where(j => j.status_id == 1 && j.stock_code_id == defaultStockId && j.queue_action == "bid").Any())
            {
                model.queue_data.highestBidRate = listHistory.Where(j => j.status_id == 1 && j.stock_code_id == defaultStockId && j.queue_action == "bid").Max(i => i.rate);
            }
            else
            {
                model.queue_data.highestBidRate = 0.0F;
            }
            if (listHistory.Where(j => j.status_id == 1 && j.stock_code_id == defaultStockId && j.queue_action == "ask").Any())
            {
                model.queue_data.LowestAskRate = listHistory.Where(j => j.status_id == 1 && j.stock_code_id == defaultStockId && j.queue_action == "ask").Min(i => i.rate);
            }
            else
            {
                model.queue_data.LowestAskRate = 0.0F;
            }
            //DateTime date = new DateTime();
            //foreach (Transaction_History history in model.Transaction_History)
            //{
            //    DateTime tmp;
            //    if (DateTime.TryParse(history.created_date.ToString(), out tmp))
            //    {
            //        date = tmp;
            //    }
            //    else
            //    {
            //        date = DateTime.UtcNow;
            //    }
            //    TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.Local);
            //    history.created_date = date;
            //}
            response = request.CreateResponse<TradingViewModel>(HttpStatusCode.OK, model);
            return response;
        }
        //[HttpGet]
        //public HttpResponseMessage GetPortfolioDetails(HttpRequestMessage request, string userId)
        //{
        //    HttpResponseMessage response = null;
        //    List<PortfolioDetails> model = new List<PortfolioDetails>();
        //    Queue_Status_Master queuing = AutoMapperHelper.GetInstance().Map<Queue_Status_Master>(_queueStatusRepository.GetAll().Where(x => x.name.ToLower() == "queued").FirstOrDefault());
        //    Queue_Status_Master completed = AutoMapperHelper.GetInstance().Map<Queue_Status_Master>(_queueStatusRepository.GetAll().Where(x => x.name.ToLower() == "completed").FirstOrDefault());
        //    List<Stock_Code> lstStockCode = AutoMapperHelper.GetInstance().Map<List<Stock_Code>>(_stockHistoryRepository.GetAll().ToList());
        //    List<Member_Stock_Details> memberStockDetails = AutoMapperHelper.GetInstance().Map<List<Member_Stock_Details>>(_memberStockDetailsRepository.GetAll().Where(x => x.user_id == userId).ToList());
        //    List<Transaction_History> lstQueuedTransaction = AutoMapperHelper.GetInstance().Map<List<Transaction_History>>(_transactionRepository.GetAll().Where(x => x.user_id == userId && x.status_id == queuing.id && x.queue_action == "ask").ToList());
        //    List<Transaction_History> lstCompletedTransaction = AutoMapperHelper.GetInstance().Map<List<Transaction_History>>(_transactionRepository.GetAll().Where(x => x.user_id == userId && x.status_id == completed.id && x.queue_action == "ask").ToList());

        //    foreach (var item in lstStockCode)
        //    {
        //        PortfolioDetails detail = new PortfolioDetails();
        //        detail.StockId = lstStockCode.Where(x => x.id == item.id).FirstOrDefault().id;
        //        detail.Holdings = memberStockDetails.Where(x => x.stock_code_id == item.id).FirstOrDefault().stock_amount;
        //        detail.StockCode = item.stock_code;
        //        detail.StockName = item.stock_name;
        //        if (lstQueuedTransaction.Where(x => x.stock_code_id == item.id).Any())
        //        {
        //            detail.BuyableSellableQuantity = detail.Holdings - lstQueuedTransaction.Where(x => x.stock_code_id == item.id).Select(y => y.reward_points).Sum();

        //        }
        //        else
        //        {
        //            detail.BuyableSellableQuantity = detail.Holdings;
        //        }
        //        if (lstCompletedTransaction.Where(x => x.stock_code_id == item.id).Any())
        //        {
        //            detail.LastTransactedPrice = lstCompletedTransaction.Where(x => x.stock_code_id == item.id).OrderByDescending(y => y.updated_date).FirstOrDefault().rate;
        //        }
        //        else
        //        {
        //            detail.LastTransactedPrice = 0.0F;
        //        }
        //        detail.LatestMarketvalue = detail.Holdings * detail.LastTransactedPrice;
        //        model.Add(detail);
        //    }

        //    response = request.CreateResponse<List<PortfolioDetails>>(HttpStatusCode.OK, model);
        //    return response;
        //}
        [HttpGet]
        public HttpResponseMessage GetPortfolioDetails(HttpRequestMessage request, string userId)
        {
            float TotalStockIssueAmount = 0;
            float pendingWithdrawalSum = 0;
            HttpResponseMessage response = null;
            List<PortfolioDetails> model = new List<PortfolioDetails>();
            Issue_Withdrawel_Permission_Master withdrawelPermission = AutoMapperHelper.GetInstance().Map<Issue_Withdrawel_Permission_Master>(_issueWithdrawelPermissionMaster.GetAll().Where(x => x.permission.ToLower() == "in progress").FirstOrDefault());
            List<Issue_Cash_Transfer_Master> issueCashdetail = AutoMapperHelper.GetInstance().Map<List<Issue_Cash_Transfer_Master>>(_issueCashTransferMaster.GetAll().ToList());
            var issueCashDetailUser = issueCashdetail.Where(x => x.receiver_account_id == userId && x.cash_issue_permission_id == withdrawelPermission.id).Select(y => y.cash_issued).Sum();
            Queue_Status_Master queuing = AutoMapperHelper.GetInstance().Map<Queue_Status_Master>(_queueStatusRepository.GetAll().Where(x => x.name.ToLower() == "queued").FirstOrDefault());
            Queue_Status_Master completed = AutoMapperHelper.GetInstance().Map<Queue_Status_Master>(_queueStatusRepository.GetAll().Where(x => x.name.ToLower() == "completed").FirstOrDefault());
            List<Stock_Code> lstStockCode = AutoMapperHelper.GetInstance().Map<List<Stock_Code>>(_stockHistoryRepository.GetAll().ToList());
            List<Member_Stock_Details> memberStockDetails = AutoMapperHelper.GetInstance().Map<List<Member_Stock_Details>>(_memberStockDetailsRepository.GetAll().Where(x => x.user_id == userId).ToList());
            List<Transaction_History> lstQueuedTransaction = AutoMapperHelper.GetInstance().Map<List<Transaction_History>>(_transactionRepository.GetAll().Where(x => x.user_id == userId && x.status_id == queuing.id && x.queue_action == "ask").ToList());
            List<Traded_History_Master> lstCompletedTransaction = AutoMapperHelper.GetInstance().Map<List<Traded_History_Master>>(_tradedHistoryRepository.GetAll().ToList());
            List<Transaction_History> lstQueuedTransactionCash = AutoMapperHelper.GetInstance().Map<List<Transaction_History>>(_transactionRepository.GetAll().ToList());
            var lstQueuedTransactionCashUser = lstQueuedTransactionCash.Where(x => x.user_id == userId && x.status_id == queuing.id && x.queue_action == "bid").Select(y => y.price).Sum();
            Wallet_Details wallatdetail = AutoMapperHelper.GetInstance().Map<Wallet_Details>(_walletDetailsRepository.GetAll().Where(x => x.user_id == userId).FirstOrDefault());
            PortfolioDetails detail1 = new PortfolioDetails();
            if (_withdrawelHistoryDetailsRepository.FindBy(x => x.withdrawer_user_id == userId && x.withdrawel_status_id == 1).Any())
            {
                pendingWithdrawalSum = _withdrawelHistoryDetailsRepository.GetAll().Where(x => x.withdrawer_user_id == userId && x.withdrawel_status_id == 1).Select(y => y.amount).Sum();
            }
                        
            foreach (var item in lstStockCode)
            {
                PortfolioDetails detail = new PortfolioDetails();
                detail.Cash = "Cash";
                detail.Total = "Total";
                detail.LastTransactedPriceCash = 1;
                detail.HoldingCash = wallatdetail.cash;
                if (item.id == lstStockCode.Count)
                {
                    if (issueCashDetailUser > 0 || lstQueuedTransactionCashUser > 0)
                    {
                        detail.BuyableSellableCash = detail.HoldingCash - (issueCashDetailUser + lstQueuedTransactionCashUser + pendingWithdrawalSum);

                    }
                    else
                    {
                        detail.BuyableSellableCash = detail.HoldingCash;
                    }
                }
                if (item.id == lstStockCode.Count)
                {
                    detail.LatestMarketvalueCash = detail.LastTransactedPriceCash * detail.HoldingCash;
                }

                detail.StockId = lstStockCode.Where(x => x.id == item.id).FirstOrDefault().id;
                detail.Holdings = memberStockDetails.Where(x => x.stock_code_id == item.id).FirstOrDefault().stock_amount;
                detail.StockCode = item.stock_code;
                detail.StockName = item.stock_name;
                if (lstQueuedTransaction.Where(x => x.stock_code_id == item.id).Any())
                {
                    detail.BuyableSellableQuantity = detail.Holdings - lstQueuedTransaction.Where(x => x.stock_code_id == item.id).Select(y => y.reward_points).Sum();

                }
                else
                {
                    detail.BuyableSellableQuantity = detail.Holdings;
                }
                if (lstCompletedTransaction.Where(x => x.stock == item.id).Any())
                {
                    //detail.LastTransactedPrice = lstCompletedTransaction.Where(x => x.stock_code_id == item.id).OrderByDescending(y => y.updated_date).FirstOrDefault().rate;
                    detail.LastTransactedPrice = lstCompletedTransaction.Where(x => x.stock == item.id).LastOrDefault().bid_price;
                }
                else
                {
                    detail.LastTransactedPrice = 0.0F;
                }

                detail.LatestMarketvalue = detail.Holdings * detail.LastTransactedPrice;
                TotalStockIssueAmount = TotalStockIssueAmount + detail.LatestMarketvalue;
                if (item.id == lstStockCode.Count)
                {
                    detail.LatestMarketvalueCashTotal = detail.LatestMarketvalueCash + TotalStockIssueAmount;
                }
                model.Add(detail);

            }


            response = request.CreateResponse<List<PortfolioDetails>>(HttpStatusCode.OK, model);
            return response;
        }
    }
}
