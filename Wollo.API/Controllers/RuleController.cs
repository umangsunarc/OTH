using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Wollo.API.Infrastructure.Core;
using Wollo.Data.Infrastructure;
using Wollo.Data.Repositories;
using Wollo.Entities;
using Wollo.Entities.ViewModels;
using Models = Wollo.Entities.Models;
using System.Linq;

namespace Wollo.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RuleController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Models.Withdrawl_Fees> _withdrawlFeesRepository;
        private readonly IEntityBaseRepository<Models.Administration_Fees> _administrationFeesRepository;
        private readonly IEntityBaseRepository<Models.Units_Master> _unitMasterRepository;
        private readonly IEntityBaseRepository<Models.Stock_Code> _stockRepository;

        public RuleController(IEntityBaseRepository<Models.Withdrawl_Fees> withdrawlFeesRepository,
            IEntityBaseRepository<Models.Units_Master> unitMasterRepository,
            IEntityBaseRepository<Models.Administration_Fees> administrationFeesRepository,
            IEntityBaseRepository<Models.Stock_Code> stockRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _withdrawlFeesRepository = withdrawlFeesRepository;
            _administrationFeesRepository = administrationFeesRepository;
            _unitMasterRepository = unitMasterRepository;
            _stockRepository = stockRepository;
        }

        /// <summary>
        /// Update or add withdrawal rule
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddUpdateWithdrawalRule(HttpRequestMessage request, Withdrawl_Fees withdrawlFee)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                Models.Withdrawl_Fees objWithdrawlFee = _withdrawlFeesRepository.FindBy(x => x.method == withdrawlFee.method).FirstOrDefault();
                if (objWithdrawlFee == null)
                {
                    objWithdrawlFee = new Models.Withdrawl_Fees();
                    objWithdrawlFee.created_by = withdrawlFee.updated_by;
                    objWithdrawlFee.created_date = DateTime.UtcNow;
                    objWithdrawlFee.fees = withdrawlFee.fees;
                    objWithdrawlFee.method = withdrawlFee.method;
                    objWithdrawlFee.updated_by = withdrawlFee.updated_by;
                    objWithdrawlFee.updated_date = DateTime.UtcNow;
                    objWithdrawlFee.withdrawl_rule_id = withdrawlFee.withdrawl_rule_id;
                    _withdrawlFeesRepository.Add(objWithdrawlFee);
                }
                else
                {
                    objWithdrawlFee.fees = withdrawlFee.fees;
                    objWithdrawlFee.updated_by = withdrawlFee.updated_by;
                    objWithdrawlFee.updated_date = DateTime.UtcNow;
                    objWithdrawlFee.withdrawl_rule_id = withdrawlFee.withdrawl_rule_id;
                    _withdrawlFeesRepository.Edit(objWithdrawlFee);
                }
                result = 1;
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);

                return response;
            });
        }

        /// <summary>
        /// Update or add adminstration rule
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddUpdateAdminRule(HttpRequestMessage request, List<Administration_Fees> lstAdministrationFees)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                foreach (Administration_Fees item in lstAdministrationFees)
                {
                    Models.Administration_Fees objAdminFee = _administrationFeesRepository.FindBy(x => x.stock_code_id == item.stock_code_id && x.apply_on == item.apply_on).FirstOrDefault();
                    if (objAdminFee != null)
                    {
                        objAdminFee.fees = item.fees;
                        objAdminFee.rule_id = item.rule_id;
                        objAdminFee.updated_by = item.created_by;
                        objAdminFee.updated_date = DateTime.UtcNow;
                        _administrationFeesRepository.Edit(objAdminFee);
                    }
                    else
                    {
                        objAdminFee = new Models.Administration_Fees();
                        objAdminFee.apply_on = item.apply_on;
                        objAdminFee.created_by = item.created_by;
                        objAdminFee.created_date = DateTime.UtcNow;
                        objAdminFee.fees = item.fees;
                        objAdminFee.rule_id = item.rule_id;
                        objAdminFee.stock_code_id = item.stock_code_id;
                        objAdminFee.updated_by = item.created_by;
                        objAdminFee.updated_date = DateTime.UtcNow;
                        _administrationFeesRepository.Add(objAdminFee);
                    }
                }
                result = 1;
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);

                return response;
            });
        }

        /// <summary>
        /// Add/Update Lot settings
        /// </summary>
        /// <param name="request"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddUpdateTradingRule(HttpRequestMessage request, Units_Master details)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                int stockLength = _stockRepository.GetAll().Count();

                if (_unitMasterRepository.GetAll().ToList().Count == stockLength)
                {
                    var unitTradingDetails = _unitMasterRepository.FindBy(x=> x.stock_id==details.stock_id).FirstOrDefault();
                    unitTradingDetails.updated_by = details.created_by;
                    unitTradingDetails.updated_date = DateTime.UtcNow;
                    unitTradingDetails.points_equivalent = details.points_equivalent;
                    unitTradingDetails.minimum_lot = details.minimum_lot;
                    unitTradingDetails.minimum_rate = details.minimum_rate;
                    _unitMasterRepository.Edit(unitTradingDetails);
                }
                else
                {
                    var unitDetails = _unitMasterRepository.FindBy(x => x.stock_id == details.stock_id).FirstOrDefault();
                    if (unitDetails != null)
                    {                            
                            unitDetails.updated_by = details.created_by;
                            unitDetails.updated_date = DateTime.UtcNow;
                            unitDetails.points_equivalent = details.points_equivalent;
                            unitDetails.minimum_lot = details.minimum_lot;
                            unitDetails.minimum_rate = details.minimum_rate;
                            _unitMasterRepository.Edit(unitDetails);
                     }
                     else
                     {
                            Models.Units_Master unitTradingDetails = new Models.Units_Master();
                            unitTradingDetails.unit = "Lot";
                            unitTradingDetails.stock_id = details.stock_id;
                            unitTradingDetails.created_date = DateTime.UtcNow;
                            unitTradingDetails.created_by = details.created_by;
                            unitTradingDetails.updated_by = details.created_by;
                            unitTradingDetails.updated_date = DateTime.UtcNow;
                            unitTradingDetails.points_equivalent = details.points_equivalent;
                            unitTradingDetails.minimum_lot = details.minimum_lot;
                            unitTradingDetails.minimum_rate = details.minimum_rate;
                            _unitMasterRepository.Add(unitTradingDetails);
                        }
                    }
                    

                result = 1;
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }
        [HttpGet]
        public HttpResponseMessage GetTradingRule(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (_unitMasterRepository.GetAll().ToList().Count == 2)
                {
                    var unitTradingDetails = _unitMasterRepository.GetAll();
                    //  Units_Master data = AutoMapperHelper.GetInstance().Map<Units_Master>(unitTradingDetails);
                    response = request.CreateResponse<object>(HttpStatusCode.OK, unitTradingDetails);
                    return response;

                }
                else
                {
                    // no data
                }
                return response;
            });

        }
    }
}
