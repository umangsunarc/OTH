using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using Wollo.API.Infrastructure.Core;
using Wollo.Common.AutoMapper;
using Wollo.Data.Infrastructure;
using Wollo.Data.Repositories;
using Wollo.Entities;
using Wollo.Entities.ViewModels;
using Models = Wollo.Entities.Models;
using System.Linq;
using System;
using Wollo.Common.UI;
using Wollo.Base.Utilities;


namespace Wollo.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AdminSettingController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Models.Trading_Time_Details> _tradingTimeDetailsRepository;
        private readonly IEntityBaseRepository<Models.trading_days> _tradingDaysRepository;
        private readonly IEntityBaseRepository<Models.Market_Rate_Details> _marketRateDetailsRepository;
        public AdminSettingController(IEntityBaseRepository<Models.Trading_Time_Details> tradingTimeDetailsRepository,
            IEntityBaseRepository<Models.trading_days> tradingDaysRepository,
            IEntityBaseRepository<Models.Market_Rate_Details> marketRateDetailsRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _tradingTimeDetailsRepository = tradingTimeDetailsRepository;
            _tradingDaysRepository = tradingDaysRepository;
            _marketRateDetailsRepository = marketRateDetailsRepository;
        }

        
        [HttpGet]
        public HttpResponseMessage GetCurrentTradingTimeDetails(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (_tradingTimeDetailsRepository.GetAll().ToList().Count == 1)
                {
                    var currentTradingTime = _tradingTimeDetailsRepository.GetAll().FirstOrDefault();
                    response = request.CreateResponse<Wollo.Entities.Models.Trading_Time_Details>(HttpStatusCode.OK, currentTradingTime);
                    return response;
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, "No data found");
                    return response;
                }
            });
        }

        [HttpGet]
        public HttpResponseMessage GetCurrentMarketRateDetails(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (_marketRateDetailsRepository.GetAll().ToList().Count == 1)
                {
                    var currentMarketRate = _marketRateDetailsRepository.GetAll().FirstOrDefault();
                    response = request.CreateResponse<Wollo.Entities.Models.Market_Rate_Details>(HttpStatusCode.OK, currentMarketRate);
                    return response;
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, "No data found");
                    return response;
                }
            });
        }

        [HttpGet]
        public HttpResponseMessage GetWeakDays(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var tradingDays= _tradingDaysRepository.GetAll().ToList();
                response = request.CreateResponse<List<Models.trading_days>>(HttpStatusCode.OK, tradingDays);
                return response;
            });

        }

        [HttpPost]
        public HttpResponseMessage UpdateTradingTime(HttpRequestMessage request, Trading_Time_Details details)
        {
            int result = 0;
            if (_tradingTimeDetailsRepository.GetAll().ToList().Count==1)
            {
                var tradingTimeDetails = _tradingTimeDetailsRepository.GetAll().FirstOrDefault();
                tradingTimeDetails.start_time = details.start_time.TimeOfDay;
                tradingTimeDetails.end_time = details.end_time.TimeOfDay;
                tradingTimeDetails.created_by = details.user_id;
                tradingTimeDetails.updated_by = details.user_id;
                tradingTimeDetails.updated_date = DateTime.UtcNow;
                return CreateHttpResponse(request, () =>
                {
                    HttpResponseMessage response = null;
                    //********************************* Update Trading Time*******************************************//
                    _tradingTimeDetailsRepository.Edit(tradingTimeDetails);

                    //******************************** Update Trading Days********************************************//
                    List<Models.trading_days> tradingDays = new List<Models.trading_days>();
                    tradingDays = _tradingDaysRepository.GetAll().ToList();
                    foreach (Models.trading_days days in tradingDays)
                    {
                        days.is_selected = false;
                    }
                    foreach (WeakDays weekDays in details.Days)
                    {
                        if (tradingDays.Where(x => x.id == weekDays.Id).Any())
                        {
                            //tradingDays.Where(x => x.id == id).FirstOrDefault().is_selected = true;
                            Models.trading_days days = _tradingDaysRepository.FindBy(x => x.id == weekDays.Id).FirstOrDefault();
                            days.is_selected = weekDays.IsSelected;
                            _tradingDaysRepository.Edit(days);
                        }
                    }
                    result = 1;
                    response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                    return response;
                });
            }
            else
            {
                Models.Trading_Time_Details tradingTimeDetails = new Models.Trading_Time_Details();
                //Models.Trading_Time_Details tradingTimeDetails = new Models.Trading_Time_Details();
                tradingTimeDetails.start_time = details.start_time.TimeOfDay;
                tradingTimeDetails.end_time = details.end_time.TimeOfDay;
                tradingTimeDetails.created_by = details.user_id;
                tradingTimeDetails.created_date = DateTime.UtcNow;
                tradingTimeDetails.updated_by = details.user_id;
                tradingTimeDetails.updated_date = DateTime.UtcNow;
                return CreateHttpResponse(request, () =>
                {
                    HttpResponseMessage response = null;
                    //********************************* Update Trading Time*******************************************//
                    _tradingTimeDetailsRepository.Add(tradingTimeDetails);
                    result = 1;
                    response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                    return response;
                });
            }

        }

        [HttpPost]
        public HttpResponseMessage UpdateMarketRate(HttpRequestMessage request, Wollo.Entities.Models.Market_Rate_Details details)
        {
            int result = 0;
            if (_marketRateDetailsRepository.GetAll().ToList().Count == 1)
            {
                var marketRateDetails = _marketRateDetailsRepository.GetAll().FirstOrDefault();
                marketRateDetails.rate = details.rate;
                marketRateDetails.updated_by = details.updated_by;
                marketRateDetails.updated_date = DateTime.UtcNow;
                return CreateHttpResponse(request, () =>
                {
                    HttpResponseMessage response = null;
                    //********************************* Update Trading Time*******************************************//
                    _marketRateDetailsRepository.Edit(marketRateDetails);
                    result = 1;
                    response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                    return response;
                });
            }
            else
            {
                Models.Market_Rate_Details marketRateDetails = new Models.Market_Rate_Details();
                //Models.Trading_Time_Details tradingTimeDetails = new Models.Trading_Time_Details();
                marketRateDetails.rate = details.rate;
                marketRateDetails.created_by = details.created_by;
                marketRateDetails.created_date = DateTime.UtcNow;
                marketRateDetails.updated_by = details.created_by;
                marketRateDetails.updated_date = DateTime.UtcNow;
                return CreateHttpResponse(request, () =>
                {
                    HttpResponseMessage response = null;
                    //********************************* Update Trading Time*******************************************//
                    _marketRateDetailsRepository.Add(marketRateDetails);
                    result = 1;
                    response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                    return response;
                });
            }
        }

    }
}