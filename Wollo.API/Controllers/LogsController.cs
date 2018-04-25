using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using Wollo.API.Infrastructure.Core;
using Wollo.Common.UI;
using Wollo.Common.AutoMapper;
using Wollo.Data.Infrastructure;
using Wollo.Data.Repositories;
using Wollo.Entities;
using Wollo.Entities.ViewModels;
using Models = Wollo.Entities.Models;
using System.Linq;
using System;

namespace Wollo.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LogsController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Models.Audit_Log_Master> _auditLogMasterRepository;
        public LogsController(IEntityBaseRepository<Models.Audit_Log_Master> auditLogMasterRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _auditLogMasterRepository = auditLogMasterRepository;
        }

        /// <summary>
        /// Save the details of the requested url in audit_log_master table
        /// </summary>
        [HttpPost]
        public HttpResponseMessage SaveAuditLogDetails(HttpRequestMessage request, Models.Audit_Log_Master model)
        {
            HttpResponseMessage response = null;
            int result = 0;
            try
            {
                //model.ip_address = GetIPAddress.GetVisitorIPAddress();
                _auditLogMasterRepository.Add(model);
                result = 1;
            }
            catch (Exception ex)
            {
                string message = ex.InnerException.ToString();
            }
            response = request.CreateResponse<int>(HttpStatusCode.OK, result);
            return response;
        }

        /// <summary>
        /// Get All Audit Log Details from database
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllLogDetails(HttpRequestMessage request)
        {
            HttpResponseMessage response = null;
            List<Models.Audit_Log_Master> objResult = new List<Models.Audit_Log_Master>();
            List<Audit_Log_Master> objAuditLogs = new List<Audit_Log_Master>();
            try
            {
                objResult = _auditLogMasterRepository.GetAll().ToList();
                objResult = objResult.Select(x => new Models.Audit_Log_Master
                {
                    id = x.id,
                    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    url = x.url,
                    ip_address = x.ip_address,
                    user_id = x.user_id,
                    AspnetUsers = x.AspnetUsers
                }).ToList();
                objAuditLogs = AutoMapperHelper.GetInstance().Map<List<Audit_Log_Master>>(objResult);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException.ToString();
            }
            response = request.CreateResponse<List<Audit_Log_Master>>(HttpStatusCode.OK, objAuditLogs);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage RangeFilterLogs(HttpRequestMessage request, Audit_Log_Master master)
        {
            HttpResponseMessage response = null;
            List<Models.Audit_Log_Master> objResult = new List<Models.Audit_Log_Master>();
            List<Audit_Log_Master> objAuditLogs = new List<Audit_Log_Master>();

            objResult = _auditLogMasterRepository.GetAll().ToList();
            if (master.created_date == master.updated_date)
            {
                objResult = objResult.Where(x => x.created_date.Value.Date == master.created_date.Value.Date).ToList();
            }
            else
            {
                objResult = objResult.Where(x => x.created_date.Value.Date >= master.created_date.Value.Date && x.created_date.Value.Date <= master.updated_date.Value.Date).ToList();

            }
            objResult = objResult.Select(x => new Models.Audit_Log_Master
            {
                id = x.id,
                created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                created_by = x.created_by,
                updated_by = x.updated_by,
                url = x.url,
                ip_address = x.ip_address,
                user_id = x.user_id,
                AspnetUsers = x.AspnetUsers
            }).ToList();
            objAuditLogs = AutoMapperHelper.GetInstance().Map<List<Audit_Log_Master>>(objResult);

            response = request.CreateResponse<List<Audit_Log_Master>>(HttpStatusCode.OK, objAuditLogs);
            return response;
        }

    }
}