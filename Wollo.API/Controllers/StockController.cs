using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Wollo.API.Infrastructure.Core;
using Wollo.Common.AutoMapper;
using Wollo.Data.Infrastructure;
using Wollo.Data.Repositories;
using Wollo.Entities;
using Wollo.Entities.ViewModels;
using Models = Wollo.Entities.Models;
using System.Linq;

namespace Wollo.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StockController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Models.Stock_Code> _stockCodeRepository;
        private readonly IEntityBaseRepository<Models.Member_Stock_Details> _stockDetailsRepository;
        public StockController(IEntityBaseRepository<Models.Stock_Code> stockCodeRepository,
            IEntityBaseRepository<Models.Member_Stock_Details> stockDetailsRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _stockCodeRepository = stockCodeRepository;
            _stockDetailsRepository = stockDetailsRepository;
        }

        /// <summary>
        /// Get all stocks
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetStocks(HttpRequestMessage request)
        {
            HttpResponseMessage response = null;
            List<Stock_Code> lstStockCode = null;
            lstStockCode = AutoMapperHelper.GetInstance().Map<List<Stock_Code>>(_stockCodeRepository.GetAll().ToList());
            response = request.CreateResponse<List<Stock_Code>>(HttpStatusCode.OK, lstStockCode);
            return response;
        }

        /// <summary>
        /// Get stock by id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetStockCodeById(HttpRequestMessage request,int id)
        {
            HttpResponseMessage response = null;
            Stock_Code stockCode = new Stock_Code();
            stockCode = AutoMapperHelper.GetInstance().Map<Stock_Code>(_stockCodeRepository.GetAll().Where(x=>x.id==id).FirstOrDefault());
            response = request.CreateResponse<Stock_Code>(HttpStatusCode.OK, stockCode);
            return response;
        }

        /// <summary>
        /// Get member stock details by stock id and user id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetStockDetailsByUserAndStock(HttpRequestMessage request, Member_Stock_Details details)
        {
            HttpResponseMessage response = null;
            Member_Stock_Details stockDetails = new Member_Stock_Details();
            stockDetails = AutoMapperHelper.GetInstance().Map<Member_Stock_Details>(_stockDetailsRepository.GetAll().Where(x => x.user_id == details.user_id && x.stock_code_id == details.stock_code_id).FirstOrDefault());
            response = request.CreateResponse<Member_Stock_Details>(HttpStatusCode.OK, stockDetails);
            return response;
        }
    }
}
