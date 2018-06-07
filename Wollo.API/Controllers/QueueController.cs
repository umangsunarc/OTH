using Wollo.Common.AutoMapper;
using Wollo.Data.Infrastructure;
using Wollo.Data.Repositories;
using Wollo.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wollo.Entities.ViewModels;
using Wollo.API.Infrastructure.Core;
using Models = Wollo.Entities.Models;
using System.Web.Http.Cors;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;

namespace Wollo.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QueueController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Models.Transaction_History> _transactionRepository;
        private readonly IEntityBaseRepository<Models.Traded_History_Master> _tradedHistoryMasterRepository;
        private readonly IEntityBaseRepository<Models.Stock_Code> _stockCodeRepository;
        private readonly IEntityBaseRepository<Models.Holiday_Master> _holidayMasterRepository;
        private readonly IEntityBaseRepository<Models.Holiday_Status_Master> _holidayStatusMasterRepository;
        private readonly IEntityBaseRepository<Models.trading_days> _tradingDaysRepository;
        private readonly IEntityBaseRepository<Models.Trading_Time_Details> _tradingTimeRepository;
        private readonly IEntityBaseRepository<Models.Units_Master> _unitMasterRepository;
        public QueueController(IEntityBaseRepository<Models.Transaction_History> transactionRepository,
            IEntityBaseRepository<Models.Traded_History_Master> tradedHistoryMasterRepository,
            IEntityBaseRepository<Models.Stock_Code> stockCodeRepository,
            IEntityBaseRepository<Models.Holiday_Master> holidayMasterRepository,
            IEntityBaseRepository<Models.Holiday_Status_Master> holidayStatusMasterRepository,
            IEntityBaseRepository<Models.Trading_Time_Details> tradingTimeRepository,
            IEntityBaseRepository<Models.trading_days> tradingDaysRepository,
            IEntityBaseRepository<Models.Units_Master> unitMasterRepository,
             IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _tradedHistoryMasterRepository = tradedHistoryMasterRepository;
            _stockCodeRepository = stockCodeRepository;
            _holidayMasterRepository = holidayMasterRepository;
            _holidayStatusMasterRepository = holidayStatusMasterRepository;
            _tradingDaysRepository = tradingDaysRepository;
            _tradingTimeRepository = tradingTimeRepository;
            _unitMasterRepository = unitMasterRepository;
        }

        /// <summary>
        /// Get all transaction history
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllTransactionHistory(HttpRequestMessage request)
        {
            HttpResponseMessage response = null;
            List<Transaction_History> lstTransactionHistory = null;
            lstTransactionHistory = AutoMapperHelper.GetInstance().Map<List<Transaction_History>>(_transactionRepository.GetAll().ToList());
            response = request.CreateResponse<List<Transaction_History>>(HttpStatusCode.OK, lstTransactionHistory);
            return response;
        }


        /// <summary>
        /// Get all transaction history by stock Id
        /// <summary>
        /// Get all transaction history by stock Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllTransactionHistoryByStock(HttpRequestMessage request, int stockId)
        {
            QueueData queueData = new QueueData();
            QueueDataViewModel model = new QueueDataViewModel();
            List<Traded_History_Master> tradedHistoryResult = new List<Traded_History_Master>();
            List<Transaction_History> result = new List<Transaction_History>();
            result = AutoMapperHelper.GetInstance().Map<List<Transaction_History>>(_transactionRepository.FindBy(x => x.stock_code_id == stockId && x.status_id != 3).ToList().OrderByDescending(y => y.updated_date));
            tradedHistoryResult = AutoMapperHelper.GetInstance().Map<List<Traded_History_Master>>(_tradedHistoryMasterRepository.FindBy(x => x.stock == stockId).ToList());
            List<Stock_Code> stock_code = AutoMapperHelper.GetInstance().Map<List<Stock_Code>>(_stockCodeRepository.GetAll().ToList());
            Units_Master unitMasterData = AutoMapperHelper.GetInstance().Map<Units_Master>(_unitMasterRepository.GetAll().Where(x => x.stock_id == stockId).FirstOrDefault());
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
            HttpResponseMessage response = null;
            DateTime today = DateTime.Today.Date;
            DateTime? lastTradedTime = new DateTime();
            DateTime previousYearDate = DateTime.Today.AddYears(-1).Date;
            var yesterday = DateTime.Today.AddDays(-1);
            if (result.Where(j => j.status_id == 1 && j.queue_action == "bid").Any())
            {
                queueData.highestBidRate = result.Where(j => j.status_id == 1 && j.queue_action == "bid").Max(i => i.rate);
            }
            else
            {
                queueData.highestBidRate = 0.0F;
            }
            if (result.Where(j => j.status_id == 1 && j.queue_action == "ask").Any())
            {
                queueData.LowestAskRate = result.Where(j => j.status_id == 1 && j.queue_action == "ask").Min(i => i.rate);
            }
            else
            {
                queueData.LowestAskRate = 0.0F;
            }
            if (tradedHistoryResult.Count > 0)
            {
                if (tradedHistoryResult.Where(x => x.created_date >= previousYearDate).Any())
                {
                    float previousYearMinRate = tradedHistoryResult.Where(x => x.created_date >= previousYearDate).Select(y => y.bid_price).Min();
                    float previousYearMaxRate = tradedHistoryResult.Where(x => x.created_date >= previousYearDate).Select(y => y.bid_price).Max();
                    queueData.oneYearMinMaxRate = previousYearMinRate.ToString("0.00") + "-" + previousYearMaxRate.ToString("0.00");
                }
                else
                {
                    queueData.oneYearMinMaxRate = 0.0F + "-" + 0.0F;
                }
                if (tradedHistoryResult.Where(x => x.created_date >= yesterday && x.created_date < today).Any())
                {
                    queueData.prevClose = tradedHistoryResult.Where(x => x.created_date >= yesterday).Select(y => y.bid_price).LastOrDefault();
                }
                else
                {
                    queueData.prevClose = tradedHistoryResult.Select(y => y.bid_price).LastOrDefault();
                }
                if (tradedHistoryResult.Where(x => x.created_date > today).Any())
                {
                    lastTradedTime = tradedHistoryResult.Where(x => x.created_date > today).Select(y => y.updated_date).LastOrDefault();
                    queueData.dayHigh = tradedHistoryResult.Where(x => x.created_date > today).Select(y => y.bid_price).Max(); //Maximum traded bid price of current date
                    queueData.dayLow = tradedHistoryResult.Where(x => x.created_date > today).Select(y => y.bid_price).Min();  //Minimum traded bid price of current date
                    queueData.open = tradedHistoryResult.Where(x => x.created_date > today).Select(y => y.bid_price).FirstOrDefault(); //Bid price at which first trading happened on current date
                    queueData.noOfTraded = tradedHistoryResult.Where(x => x.created_date > today).Count(); //Total number of trading happened on today date
                    queueData.lastTradedPrice = tradedHistoryResult.Where(x => x.created_date > today).Select(y => y.bid_price).LastOrDefault();
                    //string datePart = String.Format("{0:dddd, MMMM d, yyyy HH:mm:ss}", lastTradedTime);
                    //queueData.lastTradedTime = String.Format("{0:F}", lastTradedTime) + " GMT +8";
                    queueData.lastTradedTime = String.Format("{0:dddd, MMMM d, yyyy HH:mm:ss}", lastTradedTime) + " GMT +8";
                    queueData.totatlVolume = tradedHistoryResult.Where(x => x.created_date > today).Select(y => y.amount).Sum(); //Total volume traded on current date
                    if (tradedHistoryResult.Where(x => x.created_date > today).Count() >= 2)
                    {
                        var secondLastTradedPrice = tradedHistoryResult.Where(x => x.created_date > today).Reverse().Skip(1).Take(1).Select(y => y.bid_price).LastOrDefault();
                        queueData.lastTradedPriceDifference = secondLastTradedPrice - queueData.lastTradedPrice;
                        queueData.lastTradedPricePercentDifference = ((secondLastTradedPrice - queueData.lastTradedPrice) / queueData.lastTradedPrice) * 100;
                    }
                    else
                    {
                        queueData.lastTradedPriceDifference = queueData.lastTradedPrice;
                        queueData.lastTradedPricePercentDifference = 0;
                    }
                    List<Traded_History_Master> data = tradedHistoryResult.Where(x => x.created_date > today).ToList();
                    queueData.valueTraded = 0.0F;
                    foreach (var item in data)
                    {
                        queueData.valueTraded += item.bid_price * item.amount;
                    }
                }
                else
                {
                    lastTradedTime = tradedHistoryResult.Select(y => y.updated_date).LastOrDefault();
                    queueData.lastTradedPrice = tradedHistoryResult.Select(y => y.bid_price).LastOrDefault(); //Last traded bid price of current date
                    //string datePart = String.Format("{0:dddd, MMMM d, yyyy HH:mm:ss}", lastTradedTime);
                    //queueData.lastTradedTime = String.Format("{0:F}", lastTradedTime) + " GMT +8";
                    queueData.lastTradedTime = String.Format("{0:dddd, MMMM d, yyyy HH:mm:ss}", lastTradedTime) + " GMT +8";
                    if (tradedHistoryResult.Count() >= 2)
                    {
                        var secondLastTradedPrice = tradedHistoryResult.Where(x => x.created_date != null).Reverse().Skip(1).Take(1).Select(y => y.bid_price).LastOrDefault();
                        queueData.lastTradedPriceDifference = secondLastTradedPrice - queueData.lastTradedPrice;
                        queueData.lastTradedPricePercentDifference = ((secondLastTradedPrice - queueData.lastTradedPrice) / queueData.lastTradedPrice) * 100;
                    }
                    else
                    {
                        queueData.lastTradedPriceDifference = 0.0F;
                        queueData.lastTradedPricePercentDifference = 0.0f;
                    }
                    queueData.dayHigh = 0.0F;
                    queueData.dayLow = 0.0F;
                    queueData.open = 0.0F;
                    queueData.noOfTraded = 0;
                    queueData.valueTraded = 0.0F;
                    queueData.totatlVolume = 0;
                }
            }
            else
            {
                queueData.dayHigh = 0.0F;
                queueData.dayLow = 0.0F;
                queueData.open = 0.0F;
                queueData.noOfTraded = 0;
                queueData.valueTraded = 0.0F;
                queueData.oneYearMinMaxRate = 0.0F + "-" + 0.0F;
                queueData.lastTradedPrice = 0.0F;
                queueData.prevClose = 0.0F;
                queueData.totatlVolume = 0;
                queueData.lastTradedPriceDifference = 0.0F;
                queueData.lastTradedPricePercentDifference = 0.0f;
            }
            var totalQueuingMemberCount = result.Count();
            float bidMemberCount = result.Where(i => i.queue_action == "bid").Count();
            float askMemberCount = result.Where(i => i.queue_action == "ask").Count();
            queueData.bidMemberPercent = (bidMemberCount / totalQueuingMemberCount) * 100;
            queueData.askMemberPercent = 100 - queueData.bidMemberPercent;
            model.TradedHistory = tradedHistoryResult.OrderByDescending(y => y.updated_date).ToList();
            model.Bid = result.Where(x => x.queue_action == "bid" && x.status_id == 1).OrderByDescending(y => y.rate).ToList();
            model.Ask = result.Where(x => x.queue_action == "ask" && x.status_id == 1).OrderBy(y => y.rate).ToList();
            model.QueueData = queueData;
            model.StockCode = stock_code;
            model.UnitMaster = unitMasterData;
            model.Bid = model.Bid.Select(x => new Transaction_History
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
            model.Ask = model.Ask.Select(x => new Transaction_History
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
            response = request.CreateResponse<QueueDataViewModel>(HttpStatusCode.OK, model);
            return response;
        }
        /// <summary>
        /// Match queue
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage MatchQueue(HttpRequestMessage request, int stockId, string userId)
        {
            HttpResponseMessage response = null;
            string[] result = new string[2];
            try
            {
                DateTime dt = DateTime.UtcNow;
                string day = (dt.DayOfWeek.ToString()).ToLower();
                bool IsDayOfWeakAllowed = _tradingDaysRepository.FindBy(x => x.name.ToLower() == day && x.is_selected == true).Any();
                TimeSpan? start = new TimeSpan();
                TimeSpan? end = new TimeSpan();
                if (_tradingTimeRepository.GetAll().ToList().Count == 1)
                {
                    start = _tradingTimeRepository.GetAll().FirstOrDefault().start_time;
                    end = _tradingTimeRepository.GetAll().FirstOrDefault().end_time;
                }
                else
                {
                    start = new TimeSpan(Convert.ToInt16(ConfigurationManager.AppSettings["StartTimeHour"]), Convert.ToInt16(ConfigurationManager.AppSettings["StartTimeMin"]), 0);
                    end = new TimeSpan(Convert.ToInt16(ConfigurationManager.AppSettings["EndTimeHour"]), Convert.ToInt16(ConfigurationManager.AppSettings["EndTimeMin"]), 0);
                }

                TimeSpan now = DateTime.Now.TimeOfDay;
                Models.Holiday_Status_Master objHolidayStatus = _holidayStatusMasterRepository.FindBy(x => x.status == "Active").FirstOrDefault();
                List<Holiday_Master> lstHolidayMaster = AutoMapperHelper.GetInstance().Map<List<Holiday_Master>>(_holidayMasterRepository.FindBy(x => x.holiday_statusid == objHolidayStatus.id).ToList());
                if (now > start && now < end && lstHolidayMaster.Count <= 0 && IsDayOfWeakAllowed)
                {
                    //match found
                    result[0] = (new ExecSP().SQLQuery<int>("CALL `wollorpe`.`MatchQueue`(" + stockId + ",'" + userId + "', 2);").SingleOrDefault()).ToString();
                    result[1] = "success";
                }
                else
                {
                    result[0] = "1";
                    result[1] = "waiting";
                }
                response = request.CreateResponse<string[]>(HttpStatusCode.OK, result);
                return response;
            }
            catch (Exception ex)
            {
                result[0] = ex.InnerException.Message.ToString();
                result[1] = "fail";
                response = request.CreateResponse<string[]>(HttpStatusCode.OK, result);
                return response;
            }

        }

        /// <summary>
        /// Get all transaction history by stock Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllQueueTradingDataByStock(HttpRequestMessage request, Transaction_History history)
        {
            QueueTradingViewModel model = new QueueTradingViewModel();
            string userId = history.user_id;
            int stockId = history.stock_code_id;
            QueueData queueData = new QueueData();
            List<Transaction_History> listHistory = new List<Transaction_History>();
            List<Transaction_History> result = new List<Transaction_History>();
            List<Traded_History_Master> tradedHistoryResult = new List<Traded_History_Master>();
            //****************************** Transaction History by user to show in myorders table************************************************************//
            if (_transactionRepository.GetAll().Where(x => x.user_id == userId).Any())
            {
                listHistory = AutoMapperHelper.GetInstance().Map<List<Transaction_History>>(_transactionRepository.GetAll().Where(x => x.user_id == userId).ToList());
            }
            //****************************** Transaction History by stock to show in bid/ask queue************************************************************//
            result = AutoMapperHelper.GetInstance().Map<List<Transaction_History>>(_transactionRepository.FindBy(x => x.stock_code_id == stockId && x.status_id != 3).ToList());
            //****************************** Transaction History with completed status to show in traded history table************************************************************//
            tradedHistoryResult = AutoMapperHelper.GetInstance().Map<List<Traded_History_Master>>(_tradedHistoryMasterRepository.FindBy(x => x.stock == stockId).ToList());
            List<Stock_Code> stock_code = AutoMapperHelper.GetInstance().Map<List<Stock_Code>>(_stockCodeRepository.GetAll().ToList());
            HttpResponseMessage response = null;
            if (result.Where(j => j.status_id == 1 && j.queue_action == "bid").Any())
            {
                queueData.highestBidRate = result.Where(j => j.status_id == 1 && j.queue_action == "bid").Max(i => i.rate);
            }
            else
            {
                queueData.highestBidRate = 0.0F;
            }
            if (result.Where(j => j.status_id == 1 && j.queue_action == "ask").Any())
            {
                queueData.LowestAskRate = result.Where(j => j.status_id == 1 && j.queue_action == "ask").Min(i => i.rate);
            }
            else
            {
                queueData.LowestAskRate = 0.0F;
            }
            var totalQueuingMemberCount = result.Count();
            float bidMemberCount = result.Where(i => i.queue_action == "bid").Count();
            float askMemberCount = result.Where(i => i.queue_action == "ask").Count();
            queueData.bidMemberPercent = (bidMemberCount / totalQueuingMemberCount) * 100;
            queueData.askMemberPercent = 100 - queueData.bidMemberPercent;
            model.TradedHistory = tradedHistoryResult;
            model.Bid = result.Where(x => x.queue_action == "bid" && x.status_id == 1).OrderByDescending(y => y.rate).ToList();
            model.Ask = result.Where(x => x.queue_action == "ask" && x.status_id == 1).OrderBy(y => y.rate).ToList();
            model.QueueData = queueData;
            model.StockCode = stock_code;
            model.Transaction_History = listHistory;
            model.Transaction_History = model.Transaction_History.Select(x => new Transaction_History
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
            model.Bid = model.Bid.Select(x => new Transaction_History
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
            model.Ask = model.Ask.Select(x => new Transaction_History
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
            response = request.CreateResponse<QueueTradingViewModel>(HttpStatusCode.OK, model);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage config(HttpRequestMessage request)
        {
            
            HttpResponseMessage response = null;            
            Config obj = new Config();
            obj.supports_group_request = false;
            obj.supports_search = true;
            obj.supports_marks = false;
            obj.supportedResolutions = new string[] { "1", "15", "30", "60", "D", "2D", "3D", "W", "3W", "M", "6M" };
            obj.exchanges = new Exchanges[] { new Exchanges { name = "BITFINEX", value = "BITFINEX", desc = "BITFINEX" } };
            obj.symbolsTypes = new SymbolsTypes[] { new SymbolsTypes { name = "Crypto", value = "crypto" } };          
            response = request.CreateResponse<Config>(HttpStatusCode.OK, obj);
            return response;
        }
        //[HttpGet]
        //public HttpResponseMessage symbol_info(HttpRequestMessage request, string group = "BITFINEX")
        //{

        //    HttpResponseMessage response = null;
        //    SymbolGroupAPI obj = new SymbolGroupAPI();
           
        //            obj.symbol = new string[] {"BTCUSD", "ETHUSD"};
        //            obj.description = new string[] {"Bitcoin / Dollar","Ethereum / Dollar"};
        //            obj.exchangeListed = "BITFINEX";
        //            obj.exchangeTraded = "BITFINEX";
        //            obj.minmovement = 1;
        //            obj.minmovement2 = 0;
        //            obj.pricescale = new int[] {10,100};
        //            obj.hasDwm = true;
        //            obj.hasIntraday = true;
        //            obj.hasNoVolume = new bool[] { false, false };
        //            obj.type = new string[] {"crypto","crypto"} ;
        //            obj.ticker = new string[] { "BTCUSD", "ETHUSD" };
        //            obj.timezone = "Asia/Hong_Kong";
        //            obj.sessionRegular = "0900-1600";
               
        //    response = request.CreateResponse<SymbolGroupAPI>(HttpStatusCode.OK, obj);
        //    return response;        
        //}

        [HttpGet]
        public HttpResponseMessage symbols(HttpRequestMessage request, string symbol = "BTCUSD")
        {

            HttpResponseMessage response = null;
            SymbolAPI obj = new SymbolAPI();

            obj.name = "BTCUSD";
            obj.description = "Bitcoin / Dollar";
            obj.has_intraday = false;
            obj.has_no_volume = false;
            obj.minmov = 1;
            obj.minmov2 = 0;
            obj.pricescale = 100;
            obj.pointvalue = 1;
            obj.supported_resolutions = new string[] { "D", "2D", "3D", "W", "3W", "M", "6M" };
            obj.type = "crypto";
            obj.ticker = "BTCUSD";
            obj.timezone = "Asia/Hong_Kong";
            obj.session = "0900-1600";

            response = request.CreateResponse<SymbolAPI>(HttpStatusCode.OK, obj);
            return response;
        }


        [HttpGet]
        public HttpResponseMessage history(HttpRequestMessage request, string symbol, string resolution, long from, long to)
        {
            int stockId = 1;
            QueueData queueData = new QueueData();
            QueueDataViewModel model = new QueueDataViewModel();
            List<Traded_History_Master> tradedHistoryResult = new List<Traded_History_Master>();
            List<Transaction_History> result = new List<Transaction_History>();
            result = AutoMapperHelper.GetInstance().Map<List<Transaction_History>>(_transactionRepository.FindBy(x => x.stock_code_id == stockId && x.status_id != 3).ToList().OrderByDescending(y => y.updated_date));
            tradedHistoryResult = AutoMapperHelper.GetInstance().Map<List<Traded_History_Master>>(_tradedHistoryMasterRepository.FindBy(x => x.stock == stockId).ToList());
            List<Stock_Code> stock_code = AutoMapperHelper.GetInstance().Map<List<Stock_Code>>(_stockCodeRepository.GetAll().ToList());
            Units_Master unitMasterData = AutoMapperHelper.GetInstance().Map<Units_Master>(_unitMasterRepository.GetAll().Where(x => x.stock_id == stockId).FirstOrDefault());
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
            HttpResponseMessage response = null;
            DateTime today = DateTime.Today.Date;
            DateTime? lastTradedTime = new DateTime();
            DateTime previousYearDate = DateTime.Today.AddYears(-1).Date;
            var yesterday = DateTime.Today.AddDays(-1);
            if (result.Where(j => j.status_id == 1 && j.queue_action == "bid").Any())
            {
                queueData.highestBidRate = result.Where(j => j.status_id == 1 && j.queue_action == "bid").Max(i => i.rate);
            }
            else
            {
                queueData.highestBidRate = 0.0F;
            }
            if (result.Where(j => j.status_id == 1 && j.queue_action == "ask").Any())
            {
                queueData.LowestAskRate = result.Where(j => j.status_id == 1 && j.queue_action == "ask").Min(i => i.rate);
            }
            else
            {
                queueData.LowestAskRate = 0.0F;
            }
            if (tradedHistoryResult.Count > 0)
            {
                if (tradedHistoryResult.Where(x => x.created_date >= previousYearDate).Any())
                {
                    float previousYearMinRate = tradedHistoryResult.Where(x => x.created_date >= previousYearDate).Select(y => y.bid_price).Min();
                    float previousYearMaxRate = tradedHistoryResult.Where(x => x.created_date >= previousYearDate).Select(y => y.bid_price).Max();
                    queueData.oneYearMinMaxRate = previousYearMinRate.ToString("0.00") + "-" + previousYearMaxRate.ToString("0.00");
                }
                else
                {
                    queueData.oneYearMinMaxRate = 0.0F + "-" + 0.0F;
                }
                if (tradedHistoryResult.Where(x => x.created_date >= yesterday && x.created_date < today).Any())
                {
                    queueData.prevClose = tradedHistoryResult.Where(x => x.created_date >= yesterday).Select(y => y.bid_price).LastOrDefault();
                }
                else
                {
                    queueData.prevClose = tradedHistoryResult.Select(y => y.bid_price).LastOrDefault();
                }
                if (tradedHistoryResult.Where(x => x.created_date > today).Any())
                {
                    lastTradedTime = tradedHistoryResult.Where(x => x.created_date > today).Select(y => y.updated_date).LastOrDefault();
                    queueData.dayHigh = tradedHistoryResult.Where(x => x.created_date > today).Select(y => y.bid_price).Max(); //Maximum traded bid price of current date
                    queueData.dayLow = tradedHistoryResult.Where(x => x.created_date > today).Select(y => y.bid_price).Min();  //Minimum traded bid price of current date
                    queueData.open = tradedHistoryResult.Where(x => x.created_date > today).Select(y => y.bid_price).FirstOrDefault(); //Bid price at which first trading happened on current date
                    queueData.noOfTraded = tradedHistoryResult.Where(x => x.created_date > today).Count(); //Total number of trading happened on today date
                    queueData.lastTradedPrice = tradedHistoryResult.Where(x => x.created_date > today).Select(y => y.bid_price).LastOrDefault();
                    //string datePart = String.Format("{0:dddd, MMMM d, yyyy HH:mm:ss}", lastTradedTime);
                    //queueData.lastTradedTime = String.Format("{0:F}", lastTradedTime) + " GMT +8";
                    queueData.lastTradedTime = String.Format("{0:dddd, MMMM d, yyyy HH:mm:ss}", lastTradedTime) + " GMT +8";
                    queueData.totatlVolume = tradedHistoryResult.Where(x => x.created_date > today).Select(y => y.amount).Sum(); //Total volume traded on current date
                    if (tradedHistoryResult.Where(x => x.created_date > today).Count() >= 2)
                    {
                        var secondLastTradedPrice = tradedHistoryResult.Where(x => x.created_date > today).Reverse().Skip(1).Take(1).Select(y => y.bid_price).LastOrDefault();
                        queueData.lastTradedPriceDifference = secondLastTradedPrice - queueData.lastTradedPrice;
                        queueData.lastTradedPricePercentDifference = ((secondLastTradedPrice - queueData.lastTradedPrice) / queueData.lastTradedPrice) * 100;
                    }
                    else
                    {
                        queueData.lastTradedPriceDifference = queueData.lastTradedPrice;
                        queueData.lastTradedPricePercentDifference = 0;
                    }
                    List<Traded_History_Master> data = tradedHistoryResult.Where(x => x.created_date > today).ToList();
                    queueData.valueTraded = 0.0F;
                    foreach (var item in data)
                    {
                        queueData.valueTraded += item.bid_price * item.amount;
                    }
                }
                else
                {
                    lastTradedTime = tradedHistoryResult.Select(y => y.updated_date).LastOrDefault();
                    queueData.lastTradedPrice = tradedHistoryResult.Select(y => y.bid_price).LastOrDefault(); //Last traded bid price of current date
                    //string datePart = String.Format("{0:dddd, MMMM d, yyyy HH:mm:ss}", lastTradedTime);
                    //queueData.lastTradedTime = String.Format("{0:F}", lastTradedTime) + " GMT +8";
                    queueData.lastTradedTime = String.Format("{0:dddd, MMMM d, yyyy HH:mm:ss}", lastTradedTime) + " GMT +8";
                    if (tradedHistoryResult.Count() >= 2)
                    {
                        var secondLastTradedPrice = tradedHistoryResult.Where(x => x.created_date != null).Reverse().Skip(1).Take(1).Select(y => y.bid_price).LastOrDefault();
                        queueData.lastTradedPriceDifference = secondLastTradedPrice - queueData.lastTradedPrice;
                        queueData.lastTradedPricePercentDifference = ((secondLastTradedPrice - queueData.lastTradedPrice) / queueData.lastTradedPrice) * 100;
                    }
                    else
                    {
                        queueData.lastTradedPriceDifference = 0.0F;
                        queueData.lastTradedPricePercentDifference = 0.0f;
                    }
                    queueData.dayHigh = 0.0F;
                    queueData.dayLow = 0.0F;
                    queueData.open = 0.0F;
                    queueData.noOfTraded = 0;
                    queueData.valueTraded = 0.0F;
                    queueData.totatlVolume = 0;
                }
            }
            else
            {
                queueData.dayHigh = 0.0F;
                queueData.dayLow = 0.0F;
                queueData.open = 0.0F;
                queueData.noOfTraded = 0;
                queueData.valueTraded = 0.0F;
                queueData.oneYearMinMaxRate = 0.0F + "-" + 0.0F;
                queueData.lastTradedPrice = 0.0F;
                queueData.prevClose = 0.0F;
                queueData.totatlVolume = 0;
                queueData.lastTradedPriceDifference = 0.0F;
                queueData.lastTradedPricePercentDifference = 0.0f;
            }
            var totalQueuingMemberCount = result.Count();
            float bidMemberCount = result.Where(i => i.queue_action == "bid").Count();
            float askMemberCount = result.Where(i => i.queue_action == "ask").Count();
            queueData.bidMemberPercent = (bidMemberCount / totalQueuingMemberCount) * 100;
            queueData.askMemberPercent = 100 - queueData.bidMemberPercent;
            model.TradedHistory = tradedHistoryResult.OrderByDescending(y => y.updated_date).ToList();
            model.Bid = result.Where(x => x.queue_action == "bid" && x.status_id == 1).OrderByDescending(y => y.rate).ToList();
            model.Ask = result.Where(x => x.queue_action == "ask" && x.status_id == 1).OrderBy(y => y.rate).ToList();
            model.QueueData = queueData;
            model.StockCode = stock_code;
            model.UnitMaster = unitMasterData;
            model.Bid = model.Bid.Select(x => new Transaction_History
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
            model.Ask = model.Ask.Select(x => new Transaction_History
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

          //  HttpResponseMessage response = null;
            HistoryAPI obj = new HistoryAPI();
            obj.o = new double[] { 1, 1, 4, 8,3 };
            obj.c = new double[] { 1,1,1,5,15};
            obj.h = new double[] { 1, 1, 4, 8,19 };
            obj.l = new double[] { 1, 1, 1, 3,1 };
            
            obj.s = "ok";
            obj.t = new long[] { 1496102400,
        1496188405,
        1496275415,
        1496361490,
        159636990,
         };
            obj.v = new long[] {1,
        1,
        8,
        1,
        9
        };

            response = request.CreateResponse<HistoryAPI>(HttpStatusCode.OK, obj);
            return response;
        }
      }
 }