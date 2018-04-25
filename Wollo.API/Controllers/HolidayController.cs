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
using System.Configuration;

namespace Wollo.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HolidayController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Models.Holiday_Master> _holidayMasterRepository;
        private readonly IEntityBaseRepository<Models.Holiday_Status_Master> _holidayStatusMasterRepository;
        public HolidayController(IEntityBaseRepository<Models.Holiday_Master> holidayMasterRepository,
            IEntityBaseRepository<Models.Holiday_Status_Master> holidayStatusMasterRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _holidayMasterRepository = holidayMasterRepository;
            _holidayStatusMasterRepository = holidayStatusMasterRepository;
        }

        /// <summary>
        /// Get all holidays
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllHolidayData(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                HolidayData objHolidayData = new HolidayData();
                List<Holiday_Master> lstHolidayMaster = new List<Holiday_Master>();
                List<Wollo.Entities.Models.Holiday_Master> master = new List<Models.Holiday_Master>();
                lstHolidayMaster = AutoMapperHelper.GetInstance().Map<List<Holiday_Master>>(_holidayMasterRepository.GetAll());
                DateTime currentDate = DateTime.Now;
                foreach (Holiday_Master data in lstHolidayMaster)
                {
                    if (data.holiday_date.Date < currentDate.Date)
                    {
                        var holidayDetail = _holidayMasterRepository.FindBy(x => x.id == data.id).FirstOrDefault();
                        holidayDetail.holiday_statusid = data.holiday_statusid = 3;
                        holidayDetail.updated_by = data.updated_by = ConfigurationManager.AppSettings["SuperAdminUserId"];
                        holidayDetail.updated_date = data.updated_date = DateTime.UtcNow;
                        _holidayMasterRepository.Edit(holidayDetail);  
                        //data.updated_by = ConfigurationManager.AppSettings["SuperAdminUserId"];
                        //data.updated_date = DateTime.UtcNow;
                        //Wollo.Entities.Models.Holiday_Master holiday = new Models.Holiday_Master();
                        //holiday = AutoMapperHelper.GetInstance().Map<Wollo.Entities.Models.Holiday_Master>(data);
                        //master.Add(holiday);
                        //string status = _holidayStatusMasterRepository.GetAll().Where(x => x.id == 3).FirstOrDefault().status;
                        //data.HolidayStatusMaster.status = status;
                        //Models.Holiday_Master master = AutoMapperHelper.GetInstance().Map<Holiday_Master>(data);
                        //_holidayMasterRepository.Edit();
                    }
                }
                //_holidayMasterRepository.EditAll(master);
                //lstHolidayMaster = AutoMapperHelper.GetInstance().Map<List<Holiday_Master>>(master);
                //lstHolidayMaster = AutoMapperHelper.GetInstance().Map<List<Holiday_Master>>(_holidayMasterRepository.GetAll().ToList());
                objHolidayData.lstHolidayMaster = lstHolidayMaster;
                List<Holiday_Status_Master> lstHolidayStatusMaster = new List<Holiday_Status_Master>();
                lstHolidayStatusMaster = AutoMapperHelper.GetInstance().Map<List<Holiday_Status_Master>>(_holidayStatusMasterRepository.GetAll().ToList());
                objHolidayData.lstHolidayStatusMaster = lstHolidayStatusMaster;
                
                response = request.CreateResponse<HolidayData>(HttpStatusCode.OK, objHolidayData);
                return response;
            });
        }

        /// <summary>
        /// for filtering in a date range
        /// </summary>
        /// <param name="request"></param>
        /// <param name="master"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RangeFilter(HttpRequestMessage request, Holiday_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                HolidayData objHolidayData = new HolidayData();
                List<Holiday_Master> lstHolidayMaster = new List<Holiday_Master>();
                lstHolidayMaster = AutoMapperHelper.GetInstance().Map<List<Holiday_Master>>(_holidayMasterRepository.GetAll().ToList());
                objHolidayData.lstHolidayMaster = lstHolidayMaster;
                List<Holiday_Status_Master> lstHolidayStatusMaster = new List<Holiday_Status_Master>();
                lstHolidayStatusMaster = AutoMapperHelper.GetInstance().Map<List<Holiday_Status_Master>>(_holidayStatusMasterRepository.GetAll().ToList());
                objHolidayData.lstHolidayStatusMaster = lstHolidayStatusMaster;
                if (master.created_date == master.updated_date)
                {
                    objHolidayData.lstHolidayMaster = lstHolidayMaster.Where(x => x.holiday_date.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    objHolidayData.lstHolidayMaster = lstHolidayMaster.Where(x => x.holiday_date >= master.created_date.Value.Date && x.holiday_date <= master.updated_date.Value.Date).ToList();
                }

               // objHolidayData.lstHolidayMaster = lstHolidayMaster.Where(x => x.holiday_date >= master.created_date.Value.Date && x.holiday_date <= master.updated_date.Value.Date).ToList();
                response = request.CreateResponse<HolidayData>(HttpStatusCode.OK, objHolidayData);
                return response;
            });
        }

        ///// <summary>
        ///// Get all holidays
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public HttpResponseMessage GetAllHolidayData(HttpRequestMessage request)
        //{
        //    return CreateHttpResponse(request, () =>
        //    {
        //        HttpResponseMessage response = null;
        //        HolidayData objHolidayData = new HolidayData();
        //        List<Holiday_Master> lstHolidayMaster = new List<Holiday_Master>();
        //        List<Wollo.Entities.Models.Holiday_Master> master = new List<Models.Holiday_Master>();
        //        lstHolidayMaster = AutoMapperHelper.GetInstance().Map<List<Holiday_Master>>(_holidayMasterRepository.GetAll());
        //        DateTime currentDate = DateTime.Now;
        //        foreach (Holiday_Master data in lstHolidayMaster)
        //        {
        //            if (data.holiday_date.Date < currentDate.Date)
        //            {
        //                data.holiday_statusid = 3;
        //                //data.updated_by = ConfigurationManager.AppSettings["SuperAdminUserId"];
        //                //data.updated_date = DateTime.UtcNow;
        //                //data.HolidayStatusMaster.id = data.holiday_statusid;
        //                //Wollo.Entities.Models.Holiday_Master holiday = new Models.Holiday_Master();
        //                //holiday = AutoMapperHelper.GetInstance().Map<Wollo.Entities.Models.Holiday_Master>(data);
        //                //master.Add(holiday);
        //                //_holidayMasterRepository.Edit(holiday);
        //                //string status = _holidayStatusMasterRepository.GetAll().Where(x => x.id == 3).FirstOrDefault().status;
        //                //data.HolidayStatusMaster.status = status;
        //                //Models.Holiday_Master master = AutoMapperHelper.GetInstance().Map<Holiday_Master>(data);
        //                //_holidayMasterRepository.Edit();
        //            }
        //        }
        //        _holidayMasterRepository.EditAll(master);
        //        lstHolidayMaster = AutoMapperHelper.GetInstance().Map<List<Holiday_Master>>(master);
        //        //lstHolidayMaster = AutoMapperHelper.GetInstance().Map<List<Holiday_Master>>(_holidayMasterRepository.GetAll().ToList());
        //        objHolidayData.lstHolidayMaster = lstHolidayMaster;
        //        List<Holiday_Status_Master> lstHolidayStatusMaster = new List<Holiday_Status_Master>();
        //        lstHolidayStatusMaster = AutoMapperHelper.GetInstance().Map<List<Holiday_Status_Master>>(_holidayStatusMasterRepository.GetAll().ToList());
        //        objHolidayData.lstHolidayStatusMaster = lstHolidayStatusMaster;

        //        response = request.CreateResponse<HolidayData>(HttpStatusCode.OK, objHolidayData);
        //        return response;
        //    });
        //}


        public int UpdateHolidayStatus(Holiday_Master data)
        {
            Wollo.Entities.Models.Holiday_Master master = new Models.Holiday_Master();
            master = AutoMapperHelper.GetInstance().Map<Wollo.Entities.Models.Holiday_Master>(data);
            master.holiday_statusid = 3;
            master.updated_by = ConfigurationManager.AppSettings["SuperAdminUserId"];
            master.updated_date = DateTime.UtcNow;
            _holidayMasterRepository.Edit(master);
            return 1;
        }

        /// <summary>
        /// Add holiday
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddHoliday(HttpRequestMessage request, Holiday_Master holidayMaster)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                Models.Holiday_Master objHolidayMaster = new Models.Holiday_Master();
                objHolidayMaster.created_by = holidayMaster.created_by;
                objHolidayMaster.created_date = DateTime.UtcNow;
                objHolidayMaster.description = holidayMaster.description;
                objHolidayMaster.holiday_date = holidayMaster.holiday_date;
                objHolidayMaster.holiday_statusid = _holidayStatusMasterRepository.FindBy(x => x.status == "Upcoming").FirstOrDefault().id;
                objHolidayMaster.notify_before = holidayMaster.notify_before;
                objHolidayMaster.updated_by = holidayMaster.created_by;
                objHolidayMaster.updated_date = DateTime.UtcNow;
                _holidayMasterRepository.Add(objHolidayMaster);
                result = 1;
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }

        /// <summary>
        /// change holiday status
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ChangeHolidayStatus(HttpRequestMessage request, Holiday_Master holidayMaster)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                Models.Holiday_Master objHolidayMaster = _holidayMasterRepository.FindBy(x => x.id == holidayMaster.id).FirstOrDefault();
                objHolidayMaster.holiday_statusid = holidayMaster.holiday_statusid;
                objHolidayMaster.updated_by = holidayMaster.updated_by;
                objHolidayMaster.updated_date = DateTime.UtcNow;
                _holidayMasterRepository.Edit(objHolidayMaster);
                result = 1;
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }

        /// <summary>
        /// Modify holiday
        /// </summary>
        /// <param name="request"></param>
        /// <param name="holidayMaster"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage EditHoliday(HttpRequestMessage request, Holiday_Master holidayMaster)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                Models.Holiday_Master objHolidayMaster = _holidayMasterRepository.FindBy(x => x.id == holidayMaster.id).FirstOrDefault();
                objHolidayMaster.description = holidayMaster.description;
                objHolidayMaster.holiday_date = holidayMaster.holiday_date;
                objHolidayMaster.notify_before = holidayMaster.notify_before;
                objHolidayMaster.updated_by = holidayMaster.updated_by;
                objHolidayMaster.updated_date = DateTime.UtcNow;
                _holidayMasterRepository.Edit(objHolidayMaster);
                result = 1;
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }
    }
}