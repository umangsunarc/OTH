using System.Collections.Generic;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Web.Http.Cors;
using Wollo.Data.Repositories;
using Wollo.Entities;
using Wollo.Data.Infrastructure;
using Wollo.API.Infrastructure.Core;
using Wollo.Entities.ViewModels;
using Models = Wollo.Entities.Models;
using System.Linq;
using Wollo.Common.AutoMapper;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MasterController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Models.Stock_Code> _stockCodeRepository;
        private readonly IEntityBaseRepository<Models.Units_Master> _unitMasterRepository;
        private readonly IEntityBaseRepository<Models.Rule_Config_Settings> _ruleConfigSettingsRepository;
        private readonly IEntityBaseRepository<Models.Administration_Fees> _administrationFeesRepository;
        private readonly IEntityBaseRepository<Models.Withdrawl_Fees> _withdrawalFeesRepository;
        private readonly IEntityBaseRepository<Models.Payment_Method> _paymentMethodRepository;
        public MasterController(IEntityBaseRepository<Models.Stock_Code> stockCodeRepository,
            IEntityBaseRepository<Models.Rule_Config_Settings> ruleConfigSettingsRepository,
            IEntityBaseRepository<Models.Units_Master> unitMasterRepository,
            IEntityBaseRepository<Models.Administration_Fees> administrationFeesRepository,
            IEntityBaseRepository<Models.Withdrawl_Fees> withdrawalFeesRepository,
            IEntityBaseRepository<Models.Payment_Method> paymentMethodRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _stockCodeRepository = stockCodeRepository;
            _ruleConfigSettingsRepository = ruleConfigSettingsRepository;
            _administrationFeesRepository = administrationFeesRepository;
            _withdrawalFeesRepository = withdrawalFeesRepository;
            _unitMasterRepository = unitMasterRepository;
            _paymentMethodRepository = paymentMethodRepository;
        }

        /// <summary>
        ///  to get all stock codes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllStockCode(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Stock_Code> lstStockCode = AutoMapperHelper.GetInstance().Map<List<Stock_Code>>(_stockCodeRepository.GetAll().ToList());
                response = request.CreateResponse<List<Stock_Code>>(HttpStatusCode.OK, lstStockCode);

                return response;
            });
        }

        /// <summary>
        ///  to get all payment methods
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllPaymentMethods(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Payment_Method> lstPaymentMethod = AutoMapperHelper.GetInstance().Map<List<Payment_Method>>(_paymentMethodRepository.GetAll().ToList());
                response = request.CreateResponse<List<Payment_Method>>(HttpStatusCode.OK, lstPaymentMethod);
                return response;
            });
        }

        ///// <summary>
        ///// Get all master data for rules
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public HttpResponseMessage GetAllRuleMasterData(HttpRequestMessage request)
        //{
        //    return CreateHttpResponse(request, () =>
        //    {
        //        HttpResponseMessage response = null;
        //        AdminRuleMasterData objRuleData = new AdminRuleMasterData();
        //        List<Stock_Code> lstStockCode = new List<Stock_Code>();
        //        lstStockCode = AutoMapperHelper.GetInstance().Map<List<Stock_Code>>(_stockCodeRepository.GetAll().ToList());
        //        objRuleData.StockCodes = lstStockCode;
        //        List<Rule_Config_Settings> lstRuleConfigSettings = new List<Rule_Config_Settings>();
        //        lstRuleConfigSettings = AutoMapperHelper.GetInstance().Map<List<Rule_Config_Settings>>(_ruleConfigSettingsRepository.FindBy(x => x.rule_type == "Fee").ToList());
        //        objRuleData.RuleConfigSettings = lstRuleConfigSettings;
        //        response = request.CreateResponse<AdminRuleMasterData>(HttpStatusCode.OK, objRuleData);

        //        return response;
        //    });
        //}

        [HttpGet]
        public HttpResponseMessage GetAllRuleMasterData(HttpRequestMessage request)
        {

            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                try
                {
                    AdminRuleMasterData objRuleData = new AdminRuleMasterData();
                    List<Stock_Code> lstStockCode = new List<Stock_Code>();
                    List<Withdrawl_Fees> lstWithdrwal = new List<Withdrawl_Fees>();
                    List<Units_Master> lstUnitMaster = new List<Units_Master>();
                    List<Rule_Config_Settings> lstRuleConfigSettings = new List<Rule_Config_Settings>();
                    List<Administration_Fees> lstAdministrationFee = new List<Administration_Fees>();
                    lstUnitMaster = AutoMapperHelper.GetInstance().Map<List<Units_Master>>(_unitMasterRepository.GetAll().ToList());
                    lstWithdrwal = AutoMapperHelper.GetInstance().Map<List<Withdrawl_Fees>>(_withdrawalFeesRepository.GetAll().ToList());
                    lstStockCode = AutoMapperHelper.GetInstance().Map<List<Stock_Code>>(_stockCodeRepository.GetAll().ToList());
                    objRuleData.WithdrawalFees = lstWithdrwal;
                    objRuleData.UnitMaster = lstUnitMaster;
                    objRuleData.StockCodes = lstStockCode;
                    lstRuleConfigSettings = AutoMapperHelper.GetInstance().Map<List<Rule_Config_Settings>>(_ruleConfigSettingsRepository.FindBy(x => x.rule_type == "Fee").ToList());
                    objRuleData.RuleConfigSettings = lstRuleConfigSettings;
                    lstAdministrationFee = AutoMapperHelper.GetInstance().Map<List<Administration_Fees>>(_administrationFeesRepository.GetAll().ToList());
                    objRuleData.AdministrationFees = lstAdministrationFee;
                    //response = request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");

                    //return response;

                    response = request.CreateResponse<AdminRuleMasterData>(HttpStatusCode.OK, objRuleData);

                    return response;
                }
                catch (System.Exception ex)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException.Message.ToString());

                    return response;
                }
                
            });
        }

    }
}
