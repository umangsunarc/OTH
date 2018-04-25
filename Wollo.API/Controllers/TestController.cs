using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using Wollo.API.Infrastructure.Core;
using Wollo.Common.AutoMapper;
using Wollo.Data;
using Wollo.Data.Infrastructure;
using Wollo.Data.Repositories;
using Wollo.Entities;
using Wollo.Entities.ViewModels;
using Models = Wollo.Entities.Models;

namespace Wollo.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TestController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Models.Wallet_Details> _walletDetailsRepository;

        public TestController(IEntityBaseRepository<Models.Wallet_Details> walletDetailsRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _walletDetailsRepository = walletDetailsRepository;
        }

        /// <summary>
        /// Get all cash transaction history by user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public List<Models.Wallet_Details> GetTestData()
        {
            return _walletDetailsRepository.GetAll().ToList();
        }

        [HttpGet]
        public List<Models.Wallet_Details> GetTestData2()
        {
            PortalContext context = new PortalContext();


            return context.WalletDetailsSet.ToList();
        }


    }
}