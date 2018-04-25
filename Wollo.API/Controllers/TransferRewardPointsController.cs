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
using System.Collections.Generic;
using Wollo.Common.AutoMapper;
using System;

namespace Wollo.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TransferRewardPointsController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Models.RPE_Portal_User> _rPEPortalUserRepository;
        private readonly IEntityBaseRepository<Models.Reward_Points_Transfer_Details> _pointsTransferDetailsRepository;
        private readonly IEntityBaseRepository<Models.Reward_Points_Transfer_Master> _pointsTransferMasterRepository;
        private readonly IEntityBaseRepository<Models.Member_Stock_Details> _stockDetailsrRepository;
        private readonly IEntityBaseRepository<Models.User> _userRepository;
        private readonly IEntityBaseRepository<Models.Transfer_Action_Master> _transferActionRepository;
        private readonly IEntityBaseRepository<Models.Stock_Code> _stockCodeRepository;
        public TransferRewardPointsController(IEntityBaseRepository<Models.RPE_Portal_User> rPEPortalUserRepository,
            IEntityBaseRepository<Models.Reward_Points_Transfer_Details> pointsTransferDetailsRepository,
            IEntityBaseRepository<Models.Reward_Points_Transfer_Master> pointsTransferMasterRepository,
            IEntityBaseRepository<Models.Member_Stock_Details> stockDetailsrRepository,
            IEntityBaseRepository<Models.Transfer_Action_Master> transferActionRepository,
            IEntityBaseRepository<Models.Stock_Code> stockCodeRepository,
            IEntityBaseRepository<Models.User> userRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _rPEPortalUserRepository = rPEPortalUserRepository;
            _pointsTransferDetailsRepository = pointsTransferDetailsRepository;
            _pointsTransferMasterRepository =  pointsTransferMasterRepository;
            _stockDetailsrRepository = stockDetailsrRepository;
            _userRepository = userRepository;
            _transferActionRepository = transferActionRepository;
            _stockCodeRepository = stockCodeRepository;
        }

        //for filtering
        [HttpPost]
        public HttpResponseMessage RangeFilter(HttpRequestMessage request, Issue_Cash_Transfer_Master master)
        {
            HttpResponseMessage response = null;
            //TransferPointsViewModel objTransferPointsViewModel = new TransferPointsViewModel();
            List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details> details = new List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>();
            details = AutoMapperHelper.GetInstance().Map<List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>>(_pointsTransferDetailsRepository.GetAll().ToList());
            foreach (Wollo.Entities.ViewModels.Reward_Points_Transfer_Details item in details)
            {
                item.RewardPointTransferMaster.created_date = item.RewardPointTransferMaster.created_date.HasValue ? item.RewardPointTransferMaster.created_date.Value.ToLocalTime() : item.RewardPointTransferMaster.created_date;
            }
            if (master.created_date == master.updated_date)
            {
                details = details.Where(x => x.RewardPointTransferMaster.created_date.Value.Date == master.created_date.Value.Date).ToList();
            }
            else
            {
                details = details.Where(x => x.RewardPointTransferMaster.created_date.Value.Date >= master.created_date.Value.Date && x.RewardPointTransferMaster.created_date.Value.Date <= master.updated_date.Value.Date).ToList();

            }
            response = request.CreateResponse<List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>>(HttpStatusCode.OK, details);
            return response;
        }

        ////for filtering member side
        //[HttpGet]
        //public HttpResponseMessage RangeFilter1(HttpRequestMessage request, Reward_Points_Transfer_Details master , string userId)
        //{
        //    HttpResponseMessage response = null;
        //    List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details> details = new List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>();
        //    details = AutoMapperHelper.GetInstance().Map<List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>>(_pointsTransferDetailsRepository.GetAll().Where(x => x.RewardPointTransferMaster.user_id == userId).ToList());
        //    foreach (Wollo.Entities.ViewModels.Reward_Points_Transfer_Details item in details)
        //    {
        //        item.RewardPointTransferMaster.created_date = item.RewardPointTransferMaster.created_date.HasValue ? item.RewardPointTransferMaster.created_date.Value.ToLocalTime() : item.RewardPointTransferMaster.created_date;
        //    }
        //    response = request.CreateResponse<List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>>(HttpStatusCode.OK, details);
        //    return response;
        //}

        //for filtering admin side
        [HttpPost]
        public HttpResponseMessage RangeFilter1(HttpRequestMessage request, Issue_Cash_Transfer_Master master, string userId)
        {
            HttpResponseMessage response = null;
            List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details> details = new List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>();
            details = AutoMapperHelper.GetInstance().Map<List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>>(_pointsTransferDetailsRepository.GetAll().Where(x => x.RewardPointTransferMaster.user_id == userId).ToList());
            foreach (Wollo.Entities.ViewModels.Reward_Points_Transfer_Details item in details)
            {
                item.RewardPointTransferMaster.created_date = item.RewardPointTransferMaster.created_date.HasValue ? item.RewardPointTransferMaster.created_date.Value.ToLocalTime() : item.RewardPointTransferMaster.created_date;
            }
            if (master.created_date == master.updated_date)
            {
                details = details.Where(x => x.RewardPointTransferMaster.created_date.Value.Date == master.created_date.Value.Date).ToList();
            }
            else
            {
                details = details.Where(x => x.RewardPointTransferMaster.created_date.Value.Date >= master.created_date.Value.Date && x.RewardPointTransferMaster.created_date.Value.Date <= master.updated_date.Value.Date).ToList();

            }
            response = request.CreateResponse<List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>>(HttpStatusCode.OK, details);
            return response;
        }

        /// <summary>
        ///  api to get points from wollo to rpe
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage TransferPointsFromWolloToRPE(HttpRequestMessage request, Wollo.Entities.ViewModels.PointTransferViewModel model)
        {
            HttpResponseMessage response = null;
            Result data = new Result();
            int result = 0;
            if (_userRepository.FindBy(x => x.email_address == model.email).Any())
            {
                if (_stockCodeRepository.GetAll().Where(x => x.stock_code.ToLower().Trim() == model.store_code.Trim()).Any())
                {
                    int stockId = _stockCodeRepository.GetAll().Where(x => x.stock_code.ToLower() == model.store_code).FirstOrDefault().id;
                    Models.User objUser = _userRepository.FindBy(x => x.email_address == model.email).FirstOrDefault();
                    Models.Member_Stock_Details details = _stockDetailsrRepository.GetAll().Where(x => x.user_id == objUser.user_id && x.StockCode.id == stockId).FirstOrDefault();
                    details.stock_amount = details.stock_amount + model.stock_amount;
                    _stockDetailsrRepository.Edit(details);
                    Models.Reward_Points_Transfer_Master objTransferMaster = new Models.Reward_Points_Transfer_Master();
                    objTransferMaster.created_date = DateTime.UtcNow;
                    objTransferMaster.updated_date = DateTime.UtcNow;
                    objTransferMaster.points = model.stock_amount;
                    objTransferMaster.created_by = details.user_id;
                    objTransferMaster.user_id = details.user_id;
                    objTransferMaster.updated_by = details.user_id;
                    _pointsTransferMasterRepository.Add(objTransferMaster);
                    Models.Reward_Points_Transfer_Details objTransferDetails = new Models.Reward_Points_Transfer_Details();
                    objTransferDetails.points_transferred = model.stock_amount;
                    objTransferDetails.pointstransfer_id = objTransferMaster.id;
                    objTransferDetails.transfer_actionid = _transferActionRepository.GetAll().Where(x => x.name.ToLower() == "wollo to rpe").FirstOrDefault().id;
                    _pointsTransferDetailsRepository.Add(objTransferDetails);
                    result = 1;
                }
                else
                {
                    result = 2;
                }
            }
            else
            {
                result = 0;
            }
            data.status = result;
            if (result == 1)
            {               
                data.msg = "Reward Points Transferred successfully.";
                response = request.CreateResponse(HttpStatusCode.OK, data);
            }
            else if (result == 2)
            {
                data.msg = "Stock Code:" + model.store_code + " does not exist in RPE.";
                response = request.CreateResponse(HttpStatusCode.OK, data);
            }
            else
            {
                data.msg = "Customer with this email does not exist. Make sure you are a registered user at RPE";
                response = request.CreateResponse(HttpStatusCode.OK, data);
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage TransferPointsFromRPEToWollo(HttpRequestMessage request, Models.Member_Stock_Details points)
        {
            HttpResponseMessage response = null;
            Result data = new Result();
            string result = "";
            if (_userRepository.FindBy(x => x.email_address == points.email).Any())
            {
                try
                {
                    Models.User objUser = _userRepository.FindBy(x => x.email_address == points.email).FirstOrDefault();
                    Models.Member_Stock_Details details = _stockDetailsrRepository.GetAll().Where(x => x.user_id == objUser.user_id && x.StockCode.stock_code == points.StockCode.stock_code).FirstOrDefault();
                    //Models.Member_Stock_Details details = _stockDetailsrRepository.GetAll().Where(x => x.email == points.email && x.StockCode.id == 1).FirstOrDefault();
                    details.stock_amount = details.stock_amount - points.stock_amount;
                    _stockDetailsrRepository.Edit(details);
                    Models.Reward_Points_Transfer_Master objTransferMaster = new Models.Reward_Points_Transfer_Master();
                    objTransferMaster.created_date = DateTime.UtcNow;
                    objTransferMaster.updated_date = DateTime.UtcNow;
                    objTransferMaster.points = points.stock_amount;
                    objTransferMaster.created_by = points.user_id;
                    objTransferMaster.user_id = points.user_id;
                    objTransferMaster.updated_by = points.user_id;
                    _pointsTransferMasterRepository.Add(objTransferMaster);
                    Models.Reward_Points_Transfer_Details objTransferDetails = new Models.Reward_Points_Transfer_Details();
                    objTransferDetails.points_transferred = points.stock_amount;
                    objTransferDetails.pointstransfer_id = objTransferMaster.id;
                    objTransferDetails.transfer_actionid = _transferActionRepository.GetAll().Where(x => x.name.ToLower() == "rpe to wollo").FirstOrDefault().id;
                    _pointsTransferDetailsRepository.Add(objTransferDetails);
                    result = "success";
                    response = request.CreateResponse<string>(HttpStatusCode.OK, result);
                    return response;
                }
                catch(Exception ex)
                {
                    string message = ex.InnerException.Message.ToString();
                    response = request.CreateResponse(HttpStatusCode.BadRequest, message);
                    return response;
                }
            }
            else
            {
                response = request.CreateResponse(HttpStatusCode.BadRequest, "User does not exist.");
                return response;
            }
            
        }

        [HttpGet]
        public HttpResponseMessage GetAllPointsTransferHistoryByUser(HttpRequestMessage request, string userId)
        {
            HttpResponseMessage response = null;
            List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details> details = new List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>();
            details = AutoMapperHelper.GetInstance().Map<List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>>(_pointsTransferDetailsRepository.GetAll().Where(x=>x.RewardPointTransferMaster.user_id==userId).ToList());
            foreach (Wollo.Entities.ViewModels.Reward_Points_Transfer_Details item in details)
            {
                item.RewardPointTransferMaster.created_date = item.RewardPointTransferMaster.created_date.HasValue ? item.RewardPointTransferMaster.created_date.Value.ToLocalTime() : item.RewardPointTransferMaster.created_date;
            }
            response = request.CreateResponse<List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>>(HttpStatusCode.OK, details);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetAllPointsTransferHistory(HttpRequestMessage request)
        {
            HttpResponseMessage response = null;
            List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details> details = new List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>();
            details = AutoMapperHelper.GetInstance().Map<List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>>(_pointsTransferDetailsRepository.GetAll().ToList());
            foreach (Wollo.Entities.ViewModels.Reward_Points_Transfer_Details item in details)
            {
                item.RewardPointTransferMaster.created_date = item.RewardPointTransferMaster.created_date.HasValue ? item.RewardPointTransferMaster.created_date.Value.ToLocalTime() : item.RewardPointTransferMaster.created_date;
            }
            response = request.CreateResponse<List<Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>>(HttpStatusCode.OK, details);
            return response;
        }

    }
}
