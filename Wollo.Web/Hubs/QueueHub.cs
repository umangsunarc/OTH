using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Configuration;
using Wollo.Entities.ViewModels;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Wollo.Web.Hubs
{
    public class QueueHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public async Task Send(string name, int? stockId, string data,string userId)
        {
            //********************* For fisrt time the queue screen will show reward point queue by default***************//
            if (stockId == null)
            {
                stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
            }
            
            Wollo.Entities.ViewModels.TradingViewModel tradingObj = new Wollo.Entities.ViewModels.TradingViewModel();
            tradingObj = await GetAllTransactionHistoryByUser(userId);

            Wollo.Entities.ViewModels.QueueDataViewModel queueObj = new Wollo.Entities.ViewModels.QueueDataViewModel();
            queueObj = await GetTransactionHistoryByStock(Convert.ToInt32(stockId));

            tradingObj.Transaction_History = tradingObj.Transaction_History.Select(x => new Wollo.Entities.ViewModels.Transaction_History
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

            
            Wollo.Entities.ViewModels.QueueTradingViewModel queueTradingObj = new Wollo.Entities.ViewModels.QueueTradingViewModel();
            queueTradingObj.Bid = queueObj.Bid;
            queueTradingObj.Ask = queueObj.Ask;
            queueTradingObj.TradedHistory = queueObj.TradedHistory;
            queueTradingObj.QueueData = queueObj.QueueData;
            queueTradingObj.Stock_Code = tradingObj.Stock_Code;
            queueTradingObj.unit_master = queueObj.UnitMaster;
            queueTradingObj.Transaction_History = tradingObj.Transaction_History;


            string jsonData = "";
            switch (name)
            {
                case "bid": jsonData = JsonConvert.SerializeObject(queueTradingObj.Bid.GroupBy(item => item.rate).Select(x => new { Rate = x.Key, Points = x.Sum(y => y.reward_points), Total = x.Count(), stock_code_id = x.FirstOrDefault().stock_code_id })); break;
                case "ask": jsonData = JsonConvert.SerializeObject(queueTradingObj.Ask.GroupBy(item => item.rate).Select(x => new { Rate = x.Key, Points = x.Sum(y => y.reward_points), Total = x.Count(), stock_code_id = x.FirstOrDefault().stock_code_id })); break;
                case "completed": jsonData = JsonConvert.SerializeObject(queueTradingObj.TradedHistory.ToList()); break;
                case "GetQueueData": jsonData = JsonConvert.SerializeObject(queueTradingObj.QueueData); break;
                default: break;
            }
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, jsonData);
        }

        //public void Send(string name, int? stockId, string data)
        //{
        //    if (stockId == null)
        //    {
        //        stockId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultStockId"]);
        //    }
        //    //Note: This is temp method to implement SignalR functionality. It must be updated at API Level to get related data only
        //    HttpClient client = new HttpClient();
        //    string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
        //    string url = domain + "api/Queue/GetAllTransactionHistoryByStock?stockId=" + stockId;
        //    client.BaseAddress = new Uri(url);
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    HttpResponseMessage responseMessage = client.GetAsync(url).Result;
        //    QueueDataViewModel resultObj = null;
        //    if (responseMessage.IsSuccessStatusCode)
        //    {
        //        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
        //        resultObj = JsonConvert.DeserializeObject<QueueDataViewModel>(responseData);

        //    }
        //    string jsonData = "";
        //    switch (name)
        //    {
        //        case "bid": jsonData = JsonConvert.SerializeObject(resultObj.Bid.GroupBy(item => item.rate).Select(x => new { Rate = x.Key, Points = x.Sum(y => y.reward_points), Total = x.Count(), stock_code_id = x.FirstOrDefault().stock_code_id })); break;
        //        case "ask": jsonData = JsonConvert.SerializeObject(resultObj.Ask.GroupBy(item => item.rate).Select(x => new { Rate = x.Key, Points = x.Sum(y => y.reward_points), Total = x.Count(), stock_code_id = x.FirstOrDefault().stock_code_id })); break;
        //        case "completed": jsonData = JsonConvert.SerializeObject(resultObj.TradedHistory.ToList()); break;
        //        case "GetQueueData": jsonData = JsonConvert.SerializeObject(resultObj.QueueData); break;
        //        default: break;
        //    }
        //    // Call the broadcastMessage method to update clients.
        //    Clients.All.broadcastMessage(name, jsonData);
        //}

        public async Task<TradingViewModel> GetAllTransactionHistoryByUser(string userId)
        {
            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string GetAllTransactionHistorByUserUrl = domain + "api/Trading/GetAllTransactionHistoryByUser?userId=" + userId;
            client.BaseAddress = new Uri(GetAllTransactionHistorByUserUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Message = await client.GetAsync(GetAllTransactionHistorByUserUrl);
            Wollo.Entities.ViewModels.TradingViewModel resultObj = null;
            if (Message.IsSuccessStatusCode)
            {
                var responseData = Message.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<Wollo.Entities.ViewModels.TradingViewModel>(responseData);
                resultObj.Stock_Code = resultObj.Stock_Code.Where(x => x.stock_code.ToLower() != "issue points").ToList();
                resultObj.Transaction_History = resultObj.Transaction_History.Where(x => x.user_id == userId).ToList();
                foreach (Wollo.Entities.ViewModels.Stock_Code code in resultObj.Stock_Code)
                {
                    code.full_name = code.stock_code + "-" + code.stock_name;
                }
            }
            return resultObj;
        }

        public async Task<QueueDataViewModel> GetTransactionHistoryByStock(int stockId)
        {

            HttpClient client = new HttpClient();
            string domain = ConfigurationManager.AppSettings["AppLocalUrl"];
            string url = domain + "api/Queue/GetAllTransactionHistoryByStock?stockId=" + stockId;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            QueueDataViewModel resultObj = new QueueDataViewModel();
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                resultObj = JsonConvert.DeserializeObject<QueueDataViewModel>(responseData);
            }
            return resultObj;
        }
    }
}