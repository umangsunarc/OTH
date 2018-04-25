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
using System;
using System.Configuration;

namespace Wollo.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class IssuePointController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Models.Issue_Points_Master> _issuePointsMasterRepository;
        private readonly IEntityBaseRepository<Models.Issue_Points_Transfer_Master> _issuePointsTransferMasterRepository;
        private readonly IEntityBaseRepository<Models.Member_Stock_Details> _memberStockDetailsRepository;
        private readonly IEntityBaseRepository<Models.User> _userRepository;
        private readonly IEntityBaseRepository<Models.Stock_Code> _stockCodeRepository;
        private readonly IEntityBaseRepository<Models.Issue_Point_Transfer_Detail> _issuePointTransferDetailRepository;
        private readonly IEntityBaseRepository<Models.Topup_Status_Master> _topupStatusMasterRepository;
        private readonly IEntityBaseRepository<Models.Issue_Withdrawel_Permission_Master> _issueWithdrawelPermissionMaster;

        public IssuePointController(IEntityBaseRepository<Models.Issue_Points_Master> issuePointsMasterRepository,
            IEntityBaseRepository<Models.Issue_Points_Transfer_Master> issuePointsTransferMasterRepository,
            IEntityBaseRepository<Models.Member_Stock_Details> memberStockDetailsRepository,
            IEntityBaseRepository<Models.User> userRepository,
            IEntityBaseRepository<Models.Issue_Point_Transfer_Detail> issuePointTransferDetailRepository,
             IEntityBaseRepository<Models.Stock_Code> stockCodeRepository,
            IEntityBaseRepository<Models.Issue_Withdrawel_Permission_Master> issueWithdrawelPermissionMaster,
            IEntityBaseRepository<Models.Topup_Status_Master> topupStatusMasterRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _issuePointsMasterRepository = issuePointsMasterRepository;
            _issuePointsTransferMasterRepository = issuePointsTransferMasterRepository;
            _memberStockDetailsRepository = memberStockDetailsRepository;
            _userRepository = userRepository;
            _issuePointTransferDetailRepository = issuePointTransferDetailRepository;
            _issueWithdrawelPermissionMaster = issueWithdrawelPermissionMaster;
            _topupStatusMasterRepository = topupStatusMasterRepository;
            _stockCodeRepository = stockCodeRepository;
        }

        //for filtering in sub-menu self
        //for filtering in sub-menu self
        [HttpPost]
        public HttpResponseMessage RangeFilter(HttpRequestMessage request, Issue_Points_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                IssuePointDetails objIssuePointDetails = new IssuePointDetails();
                List<Issue_Points_Transfer_Master> IssuePointMaster = new List<Issue_Points_Transfer_Master>();
                IssuePointMaster = AutoMapperHelper.GetInstance().Map<List<Issue_Points_Transfer_Master>>(_issuePointsTransferMasterRepository.FindBy(x => x.stockCodeId == master.stock_code_id).ToList());
                if (master.created_date == master.updated_date)
                {
                    IssuePointMaster = IssuePointMaster.Where(x => x.created_date.Value.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    IssuePointMaster = IssuePointMaster.Where(x => x.created_date.Value.Date >= master.created_date.Value.Date && x.created_date.Value.Date <= master.updated_date.Value.Date).ToList();
                }
                IssuePointMaster = IssuePointMaster.Select(x => new Wollo.Entities.ViewModels.Issue_Points_Transfer_Master
                {
                    id = x.id,
                    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    issuer_user_id = x.issuer_user_id,
                    receiver_user_id = x.receiver_user_id,
                    receiver_user_name = x.receiver_user_name,
                    AspnetUsers = x.AspnetUsers,
                    points_issue_permission_id = x.points_issue_permission_id,
                    points_issued = x.points_issued,
                    issue_points_on_date = x.issue_points_on_date.ToLocalTime(),
                    stockCodeId = x.stockCodeId,
                }).ToList();
                // IssuePointMaster = IssuePointMaster.Where(x => x.created_date >= master.created_date && x.created_date <= master.updated_date).ToList();
                objIssuePointDetails.IssuePointTransferMaster = IssuePointMaster;
                response = request.CreateResponse<IssuePointDetails>(HttpStatusCode.OK, objIssuePointDetails);
                return response;
            });
        }

        //filter last sub-menu
        [HttpPost]
        public HttpResponseMessage RangeFilter1(HttpRequestMessage request, Issue_Points_Transfer_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                AdminIsuuePointsViewModel objIssuePoint = new AdminIsuuePointsViewModel();
                List<Issue_Points_Transfer_Master> IssuePointMaster = new List<Issue_Points_Transfer_Master>();
                IssuePointMaster = AutoMapperHelper.GetInstance().Map<List<Issue_Points_Transfer_Master>>(_issuePointsTransferMasterRepository.FindBy(x => x.stockCodeId == 1).ToList());
                if (master.created_date == master.updated_date)
                {
                    IssuePointMaster = IssuePointMaster.Where(x => x.created_date.Value.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    IssuePointMaster = IssuePointMaster.Where(x => x.created_date.Value.Date >= master.created_date.Value.Date && x.created_date.Value.Date <= master.updated_date.Value.Date).ToList();
                }

                // IssuePointMaster = IssuePointMaster.Where(x => x.created_date >= master.created_date && x.created_date <= master.updated_date).ToList();
                objIssuePoint.IssuePointsTransferMaster = IssuePointMaster;
                response = request.CreateResponse<AdminIsuuePointsViewModel>(HttpStatusCode.OK, objIssuePoint);
                return response;
            });
        }

        /// <summary>
        /// for member
        /// </summary>
        /// <param name="request"></param>
        /// <param name="master"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RangeFilter9(HttpRequestMessage request, Issue_Points_Master master)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                IssuePointDetails objIssuePointDetails = new IssuePointDetails();
                List<Issue_Points_Master> IssuePointMaster = new List<Issue_Points_Master>();
                IssuePointMaster = AutoMapperHelper.GetInstance().Map<List<Issue_Points_Master>>(_issuePointsMasterRepository.FindBy(x => x.stock_code_id == master.stock_code_id).ToList());
                if (master.created_date == master.updated_date)
                {
                    IssuePointMaster = IssuePointMaster.Where(x => x.created_date.Value.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    IssuePointMaster = IssuePointMaster.Where(x => x.created_date.Value.Date >= master.created_date.Value.Date && x.created_date.Value.Date <= master.updated_date.Value.Date).ToList();
                }

                // IssuePointMaster = IssuePointMaster.Where(x => x.created_date >= master.created_date && x.created_date <= master.updated_date).ToList();
                objIssuePointDetails.IssuePointsMaster = IssuePointMaster;
                response = request.CreateResponse<IssuePointDetails>(HttpStatusCode.OK, objIssuePointDetails);
                return response;
            });
        }
        /// <summary>
        ///  Get all issue points by user and stock code
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetIssuePointsHistory(HttpRequestMessage request, int stockCodeId, string userId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                IssuePointDetails objIssuePointDetails = new IssuePointDetails();
                List<Issue_Points_Master> issuepointMaster = new List<Issue_Points_Master>();
                List<Issue_Points_Transfer_Master> issuePointTransferMaster  = new List<Issue_Points_Transfer_Master>();
                objIssuePointDetails.IssuePointsMaster = issuepointMaster;
                objIssuePointDetails.IssuePointTransferMaster = issuePointTransferMaster;
                objIssuePointDetails.IssuePointsMaster = AutoMapperHelper.GetInstance().Map<List<Issue_Points_Master>>(_issuePointsMasterRepository.FindBy(x => x.stock_code_id == stockCodeId && x.user_id == userId).ToList());
                objIssuePointDetails.TotalPoints = objIssuePointDetails.IssuePointsMaster.Sum(x => x.IssuePointsTransferMaster.points_issued);
                objIssuePointDetails.RemainingPoints = objIssuePointDetails.IssuePointsMaster.Where(x => x.status != "Cancelled" && x.status != "In Progress").Sum(x => x.IssuePointsTransferMaster.points_issued);
                objIssuePointDetails.CancelledPoints = objIssuePointDetails.IssuePointsMaster.Where(x => x.status == "Cancelled").Sum(x => x.IssuePointsTransferMaster.points_issued);
                response = request.CreateResponse<IssuePointDetails>(HttpStatusCode.OK, objIssuePointDetails);
                return response;
            });
        }

        /// <summary>
        ///  Get all self issue points history  and stock code
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllIssuePointsHistorySelf(HttpRequestMessage request, int stockCodeId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                IssuePointDetails objIssuePointDetails = new IssuePointDetails();
                List<Issue_Points_Master> issuepointMaster = new List<Issue_Points_Master>();
                List<Issue_Points_Transfer_Master> issuePointTransferMaster = new List<Issue_Points_Transfer_Master>();
                objIssuePointDetails.IssuePointsMaster = issuepointMaster;
                objIssuePointDetails.IssuePointTransferMaster = issuePointTransferMaster;
                string adminUserId = ConfigurationManager.AppSettings["AdminUserId"];
                objIssuePointDetails.IssuePointTransferMaster = AutoMapperHelper.GetInstance().Map<List<Issue_Points_Transfer_Master>>(_issuePointsTransferMasterRepository.FindBy(x => x.stockCodeId == stockCodeId).ToList());
                objIssuePointDetails.IssuePointTransferMaster = objIssuePointDetails.IssuePointTransferMaster.Select(x => new Wollo.Entities.ViewModels.Issue_Points_Transfer_Master
                {
                    id = x.id,
                    created_date = x.created_date.HasValue ? x.created_date.Value.ToLocalTime() : x.created_date,
                    updated_date = x.updated_date.HasValue ? x.updated_date.Value.ToLocalTime() : x.updated_date,
                    created_by = x.created_by,
                    updated_by = x.updated_by,
                    AspnetUsers=x.AspnetUsers,
                    issue_points_on_date = x.issue_points_on_date.ToLocalTime(),
                    issuer_user_id = x.issuer_user_id,
                    points_issue_permission_id = x.points_issue_permission_id,
                    points_issued = x.points_issued,
                    receiver_user_id = x.receiver_user_id,
                    receiver_user_name=x.receiver_user_name,
                    stockCodeId=x.stockCodeId
                }).ToList();
                //objIssuePointDetails.TotalPoints = objIssuePointDetails.IssuePointsMaster.Sum(x => x.IssuePointsTransferMaster.points_issued);
                //objIssuePointDetails.RemainingPoints = objIssuePointDetails.IssuePointsMaster.Where(x => x.status != "Cancelled" && x.status != "In Progress").Sum(x => x.IssuePointsTransferMaster.points_issued);
                //objIssuePointDetails.CancelledPoints = objIssuePointDetails.IssuePointsMaster.Where(x => x.status == "Cancelled").Sum(x => x.IssuePointsTransferMaster.points_issued);
                response = request.CreateResponse<IssuePointDetails>(HttpStatusCode.OK, objIssuePointDetails);
                return response;
            });
        }

        /// <summary>
        ///  Get all issue points by stock code
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllIssuePointsHistory(HttpRequestMessage request, int stockCodeId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                IssuePointDetails objIssuePointDetails = new IssuePointDetails();
                List<Issue_Points_Master> issuepointMaster = new List<Issue_Points_Master>();
                List<Issue_Points_Transfer_Master> issuePointTransferMaster = new List<Issue_Points_Transfer_Master>();
                objIssuePointDetails.IssuePointsMaster = issuepointMaster;
                objIssuePointDetails.IssuePointTransferMaster = issuePointTransferMaster;
                objIssuePointDetails.IssuePointsMaster = AutoMapperHelper.GetInstance().Map<List<Issue_Points_Master>>(_issuePointsMasterRepository.FindBy(x => x.stock_code_id == stockCodeId).ToList());
                response = request.CreateResponse<IssuePointDetails>(HttpStatusCode.OK, objIssuePointDetails);
                return response;
            });
        }

        /// <summary>
        /// Add issue points 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Topup"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddIssuePoint(HttpRequestMessage request, Issue_Points_Transfer_Master issuePointsTransferMaster)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                string superAdminUserId = ConfigurationManager.AppSettings["SuperAdminUserId"];
                Issue_Withdrawel_Permission_Master withdrawelPermission = AutoMapperHelper.GetInstance().Map<Issue_Withdrawel_Permission_Master>(_issueWithdrawelPermissionMaster.GetAll().Where(x => x.permission.ToLower() == "approved").FirstOrDefault());
                string receiverUserId = _userRepository.GetAll().Where(x => x.user_name.ToLower().Trim() == issuePointsTransferMaster.receiver_user_name.ToLower().Trim()).FirstOrDefault().user_id;
                Models.Issue_Points_Transfer_Master objIssuePointsTransferMaster = new Models.Issue_Points_Transfer_Master();
                objIssuePointsTransferMaster = AutoMapperHelper.GetInstance().Map<Models.Issue_Points_Transfer_Master>(issuePointsTransferMaster);
                Models.Issue_Points_Transfer_Master objIssuePointsTransferMasterAdmin = new Models.Issue_Points_Transfer_Master();
                objIssuePointsTransferMasterAdmin = AutoMapperHelper.GetInstance().Map<Models.Issue_Points_Transfer_Master>(issuePointsTransferMaster);
                Models.Member_Stock_Details objMemberStockDetails = null;
                objMemberStockDetails = _memberStockDetailsRepository.FindBy(x => x.user_id == receiverUserId && x.stock_code_id == issuePointsTransferMaster.stockCodeId).FirstOrDefault();
                Models.Member_Stock_Details objMemberStockDetailsAdmin = null;
                objMemberStockDetailsAdmin = _memberStockDetailsRepository.FindBy(x => x.user_id == issuePointsTransferMaster.issuer_user_id && x.stock_code_id == issuePointsTransferMaster.stockCodeId).FirstOrDefault();
                if (objMemberStockDetailsAdmin.stock_amount >= objIssuePointsTransferMaster.points_issued)
                {
                    string receiverUserRole = _memberStockDetailsRepository.GetAll().Where(x => x.user_id == receiverUserId).FirstOrDefault().AspnetRoles.Name;
                    Models.Issue_Point_Transfer_Detail objPointTransactionDetailReceiver = new Models.Issue_Point_Transfer_Detail();
                    Models.Issue_Point_Transfer_Detail objpointTransactionDetailIssuer = new Models.Issue_Point_Transfer_Detail();
                    if (issuePointsTransferMaster.issuer_user_id != superAdminUserId)
                    {
                        objIssuePointsTransferMaster.created_by = objIssuePointsTransferMaster.issuer_user_id;
                        objIssuePointsTransferMaster.created_date = DateTime.UtcNow;
                        objIssuePointsTransferMaster.issue_points_on_date = DateTime.UtcNow;
                        objIssuePointsTransferMaster.receiver_user_id = receiverUserId;
                        objIssuePointsTransferMaster.points_issue_permission_id = withdrawelPermission.id;
                        objIssuePointsTransferMaster.opening_amount = objMemberStockDetails.stock_amount;
                        objIssuePointsTransferMaster.closing_amount = objMemberStockDetails.stock_amount + issuePointsTransferMaster.points_issued;
                        objIssuePointsTransferMaster.description = "Point Issued To Member By Admin";
                        objIssuePointsTransferMaster.updated_by = objIssuePointsTransferMaster.issuer_user_id;
                        objIssuePointsTransferMaster.updated_date = DateTime.UtcNow;
                        _issuePointsTransferMasterRepository.Add(objIssuePointsTransferMaster);


                        // need to add later for point view detail for reciver

                        objPointTransactionDetailReceiver.issuer_account_id = objIssuePointsTransferMaster.issuer_user_id;
                        objPointTransactionDetailReceiver.receiver_account_id = receiverUserId;
                        objPointTransactionDetailReceiver.point_issued_on_date = DateTime.UtcNow;
                        objPointTransactionDetailReceiver.opening_amount = objMemberStockDetails.stock_amount;
                        objPointTransactionDetailReceiver.transaction_amount = issuePointsTransferMaster.points_issued;
                        objPointTransactionDetailReceiver.closing_amount = objMemberStockDetails.stock_amount + issuePointsTransferMaster.points_issued;
                        objPointTransactionDetailReceiver.description = "Point Recived To Member By Admin";
                        objPointTransactionDetailReceiver.stock_id = issuePointsTransferMaster.stockCodeId;
                        _issuePointTransferDetailRepository.Add(objPointTransactionDetailReceiver);

                        // need to add later for point view detail for issuer


                        objpointTransactionDetailIssuer.issuer_account_id = objIssuePointsTransferMaster.issuer_user_id;
                        objpointTransactionDetailIssuer.receiver_account_id = receiverUserId;
                        objpointTransactionDetailIssuer.point_issued_on_date = DateTime.UtcNow;
                        objpointTransactionDetailIssuer.opening_amount = objMemberStockDetailsAdmin.stock_amount;
                        objpointTransactionDetailIssuer.transaction_amount = issuePointsTransferMaster.points_issued;
                        objpointTransactionDetailIssuer.closing_amount = objMemberStockDetailsAdmin.stock_amount - issuePointsTransferMaster.points_issued;
                        objpointTransactionDetailIssuer.description = "Point Issued To Member By Admin";
                        objpointTransactionDetailIssuer.stock_id = issuePointsTransferMaster.stockCodeId;
                        _issuePointTransferDetailRepository.Add(objpointTransactionDetailIssuer);

                    }
                    else
                    {
                        if (receiverUserRole.ToLower() == "super admin 1")
                        {
                            objIssuePointsTransferMaster.created_by = objIssuePointsTransferMaster.issuer_user_id;
                            objIssuePointsTransferMaster.created_date = DateTime.UtcNow;
                            objIssuePointsTransferMaster.issue_points_on_date = DateTime.UtcNow;
                            objIssuePointsTransferMaster.receiver_user_id = receiverUserId;
                            objIssuePointsTransferMaster.points_issue_permission_id = withdrawelPermission.id;
                            objIssuePointsTransferMaster.opening_amount = objMemberStockDetails.stock_amount;
                            objIssuePointsTransferMaster.closing_amount = objMemberStockDetails.stock_amount + issuePointsTransferMaster.points_issued;
                            objIssuePointsTransferMaster.description = "Point Issued To Admin By SuperAdmin";
                            objIssuePointsTransferMaster.updated_by = objIssuePointsTransferMaster.issuer_user_id;
                            objIssuePointsTransferMaster.updated_date = DateTime.UtcNow;
                            _issuePointsTransferMasterRepository.Add(objIssuePointsTransferMaster);


                            // need to add later for point view detail for reciver

                            objPointTransactionDetailReceiver.issuer_account_id = objIssuePointsTransferMaster.issuer_user_id;
                            objPointTransactionDetailReceiver.receiver_account_id = receiverUserId;
                            objPointTransactionDetailReceiver.point_issued_on_date = DateTime.UtcNow;
                            objPointTransactionDetailReceiver.opening_amount = objMemberStockDetails.stock_amount;
                            objPointTransactionDetailReceiver.transaction_amount = issuePointsTransferMaster.points_issued;
                            objPointTransactionDetailReceiver.closing_amount = objMemberStockDetails.stock_amount + issuePointsTransferMaster.points_issued;
                            objPointTransactionDetailReceiver.description = "Point Recived To Admin By SuperAdmin";
                            objPointTransactionDetailReceiver.stock_id = issuePointsTransferMaster.stockCodeId;
                            _issuePointTransferDetailRepository.Add(objPointTransactionDetailReceiver);

                            // need to add later for point view detail for issuer


                            objpointTransactionDetailIssuer.issuer_account_id = objIssuePointsTransferMaster.issuer_user_id;
                            objpointTransactionDetailIssuer.receiver_account_id = receiverUserId;
                            objpointTransactionDetailIssuer.point_issued_on_date = DateTime.UtcNow;
                            objpointTransactionDetailIssuer.opening_amount = objMemberStockDetailsAdmin.stock_amount;
                            objpointTransactionDetailIssuer.transaction_amount = issuePointsTransferMaster.points_issued;
                            objpointTransactionDetailIssuer.closing_amount = objMemberStockDetailsAdmin.stock_amount - issuePointsTransferMaster.points_issued;
                            objpointTransactionDetailIssuer.description = "Point Issued To Admin By SuperAdmin";
                            objpointTransactionDetailIssuer.stock_id = issuePointsTransferMaster.stockCodeId;
                            _issuePointTransferDetailRepository.Add(objpointTransactionDetailIssuer);
                        }
                        else
                        {
                            objIssuePointsTransferMaster.created_by = objIssuePointsTransferMaster.issuer_user_id;
                            objIssuePointsTransferMaster.created_date = DateTime.UtcNow;
                            objIssuePointsTransferMaster.issue_points_on_date = DateTime.UtcNow;
                            objIssuePointsTransferMaster.receiver_user_id = receiverUserId;
                            objIssuePointsTransferMaster.points_issue_permission_id = withdrawelPermission.id;
                            objIssuePointsTransferMaster.opening_amount = objMemberStockDetails.stock_amount;
                            objIssuePointsTransferMaster.closing_amount = objMemberStockDetails.stock_amount + issuePointsTransferMaster.points_issued;
                            objIssuePointsTransferMaster.description = "Point Issued To Member By SuperAdmin";
                            objIssuePointsTransferMaster.updated_by = objIssuePointsTransferMaster.issuer_user_id;
                            objIssuePointsTransferMaster.updated_date = DateTime.UtcNow;
                            _issuePointsTransferMasterRepository.Add(objIssuePointsTransferMaster);


                            // need to add later for point view detail for reciver

                            objPointTransactionDetailReceiver.issuer_account_id = objIssuePointsTransferMaster.issuer_user_id;
                            objPointTransactionDetailReceiver.receiver_account_id = receiverUserId;
                            objPointTransactionDetailReceiver.point_issued_on_date = DateTime.UtcNow;
                            objPointTransactionDetailReceiver.opening_amount = objMemberStockDetails.stock_amount;
                            objPointTransactionDetailReceiver.transaction_amount = issuePointsTransferMaster.points_issued;
                            objPointTransactionDetailReceiver.closing_amount = objMemberStockDetails.stock_amount + issuePointsTransferMaster.points_issued;
                            objPointTransactionDetailReceiver.description = "Point Recived To Member By SuperAdmin";
                            objPointTransactionDetailReceiver.stock_id = issuePointsTransferMaster.stockCodeId;
                            _issuePointTransferDetailRepository.Add(objPointTransactionDetailReceiver);

                            // need to add later for point view detail for issuer


                            objpointTransactionDetailIssuer.issuer_account_id = objIssuePointsTransferMaster.issuer_user_id;
                            objpointTransactionDetailIssuer.receiver_account_id = receiverUserId;
                            objpointTransactionDetailIssuer.point_issued_on_date = DateTime.UtcNow;
                            objpointTransactionDetailIssuer.opening_amount = objMemberStockDetailsAdmin.stock_amount;
                            objpointTransactionDetailIssuer.transaction_amount = issuePointsTransferMaster.points_issued;
                            objpointTransactionDetailIssuer.closing_amount = objMemberStockDetailsAdmin.stock_amount - issuePointsTransferMaster.points_issued;
                            objpointTransactionDetailIssuer.description = "Point Issued To Member By SuperAdmin";
                            objpointTransactionDetailIssuer.stock_id = issuePointsTransferMaster.stockCodeId;
                            _issuePointTransferDetailRepository.Add(objpointTransactionDetailIssuer);
                        }

                    }

                    Models.Issue_Points_Master objIssuePointsMaster = new Models.Issue_Points_Master();
                    objIssuePointsMaster.created_by = objIssuePointsTransferMaster.issuer_user_id;
                    objIssuePointsMaster.created_date = DateTime.UtcNow;

                    //Needs to add later
                    objIssuePointsMaster.issue_points_expiry_date = DateTime.UtcNow.AddYears(1);
                    objIssuePointsMaster.points_issued_date = DateTime.UtcNow;
                    if (objIssuePointsTransferMaster.issuer_user_id == objIssuePointsTransferMaster.receiver_user_id)
                    {
                        objIssuePointsMaster.status = "Cancelled";
                    }
                    else
                    {
                        objIssuePointsMaster.status = "Completed";
                    }
                    objIssuePointsMaster.stock_code_id = issuePointsTransferMaster.stockCodeId;
                    objIssuePointsMaster.transfer_id = objIssuePointsTransferMaster.id;
                    objIssuePointsMaster.updated_by = objIssuePointsTransferMaster.issuer_user_id;
                    objIssuePointsMaster.updated_date = DateTime.UtcNow;
                    objIssuePointsMaster.user_id = objIssuePointsTransferMaster.receiver_user_id;
                    _issuePointsMasterRepository.Add(objIssuePointsMaster);


                    if (objMemberStockDetails == null)
                    {
                        objMemberStockDetails = new Models.Member_Stock_Details();
                        //Needs to be added later
                        objMemberStockDetails.account_id = 1;
                        objMemberStockDetails.created_by = objIssuePointsTransferMaster.issuer_user_id;
                        objMemberStockDetails.created_date = DateTime.UtcNow;
                        objMemberStockDetails.stock_amount = objIssuePointsTransferMaster.points_issued;
                        objMemberStockDetails.stock_code_id = issuePointsTransferMaster.stockCodeId;
                        objMemberStockDetails.updated_by = objIssuePointsTransferMaster.issuer_user_id;
                        objMemberStockDetails.updated_date = DateTime.UtcNow;
                        objMemberStockDetails.user_id = objIssuePointsTransferMaster.receiver_user_id;
                        _memberStockDetailsRepository.Add(objMemberStockDetails);
                    }
                    else
                    {
                        objMemberStockDetails.stock_amount = objMemberStockDetails.stock_amount + objIssuePointsTransferMaster.points_issued;
                        objMemberStockDetails.updated_by = objIssuePointsTransferMaster.issuer_user_id;
                        objMemberStockDetails.updated_date = DateTime.UtcNow;
                        _memberStockDetailsRepository.Edit(objMemberStockDetails);

                        objMemberStockDetailsAdmin.stock_amount = objMemberStockDetailsAdmin.stock_amount - objIssuePointsTransferMaster.points_issued;
                        objMemberStockDetailsAdmin.updated_by = objIssuePointsTransferMaster.issuer_user_id;
                        objMemberStockDetailsAdmin.updated_date = DateTime.UtcNow;
                        _memberStockDetailsRepository.Edit(objMemberStockDetailsAdmin);
                    }
                    result = 1;
                }
                else
                {
                    result = 2;
                }
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }


        /// <summary>
        /// Cancel issue points 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CancelIssuePoint(HttpRequestMessage request, Issue_Points_Transfer_Master objIssuePointsTransferMaster)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                Models.Issue_Points_Master objIssuePointsMaster = _issuePointsMasterRepository.FindBy(x => x.id == objIssuePointsTransferMaster.id).FirstOrDefault();
                result = new ExecSP().SQLQuery<int>("CALL `wollorpe`.`IsPointTransactionForUserExist`(" + objIssuePointsMaster.id + "," + objIssuePointsMaster.stock_code_id + ",'" + objIssuePointsMaster.user_id + "');").SingleOrDefault();
                if (result == 0)
                {
                    
                    Models.Member_Stock_Details objMemberStockDetails = new Models.Member_Stock_Details();
                    Models.Member_Stock_Details objIssuerStockDetails = new Models.Member_Stock_Details();
                    objMemberStockDetails = _memberStockDetailsRepository.FindBy(x => x.user_id == objIssuePointsMaster.user_id && x.stock_code_id == objIssuePointsMaster.stock_code_id).FirstOrDefault();

                    objMemberStockDetails.stock_amount = objMemberStockDetails.stock_amount - objIssuePointsMaster.IssuePointsTransferMaster.points_issued;
                    objMemberStockDetails.updated_by = objIssuePointsTransferMaster.updated_by;
                    objMemberStockDetails.updated_date = DateTime.UtcNow;
                    _memberStockDetailsRepository.Edit(objMemberStockDetails);

                    objIssuerStockDetails = _memberStockDetailsRepository.FindBy(x => x.user_id == objIssuePointsMaster.IssuePointsTransferMaster.issuer_user_id && x.stock_code_id == objIssuePointsMaster.stock_code_id).FirstOrDefault();

                    objIssuerStockDetails.stock_amount = objIssuerStockDetails.stock_amount + objIssuePointsMaster.IssuePointsTransferMaster.points_issued;
                    objIssuerStockDetails.updated_by = objIssuePointsTransferMaster.updated_by;
                    objIssuerStockDetails.updated_date = DateTime.UtcNow;
                    _memberStockDetailsRepository.Edit(objIssuerStockDetails);

                    objIssuePointsMaster.status = "Cancelled";
                    objIssuePointsMaster.updated_by = objIssuePointsTransferMaster.updated_by;
                    objIssuePointsMaster.updated_date = DateTime.UtcNow;
                    _issuePointsMasterRepository.Edit(objIssuePointsMaster);

                    result = 1;                
                }
                else
                {
                    result = 2;
                }
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }

        /// <summary>
        /// Edit issue points  
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Topup"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage EditIssuePoint(HttpRequestMessage request, Issue_Points_Transfer_Master issuePointsTransferMaster)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                Models.Issue_Points_Transfer_Master objIssuePointsTransferMaster = _issuePointsTransferMasterRepository.FindBy(x => x.id == issuePointsTransferMaster.id).FirstOrDefault();
                
                Models.Issue_Points_Master model = _issuePointsMasterRepository.FindBy(x => x.transfer_id == objIssuePointsTransferMaster.id).FirstOrDefault();
                
                result = new ExecSP().SQLQuery<int>("CALL `wollorpe`.`IsPointTransactionForUserExist`(" + model.id + "," + objIssuePointsTransferMaster.stockCodeId + ",'" + objIssuePointsTransferMaster.receiver_user_id + "');").SingleOrDefault();
                
                if (result == 0)
                {
                    Models.Member_Stock_Details objMemberStockDetails = null;
                    objMemberStockDetails = _memberStockDetailsRepository.FindBy(x => x.user_id == issuePointsTransferMaster.receiver_user_id && x.stock_code_id == issuePointsTransferMaster.stockCodeId).FirstOrDefault();
                    objMemberStockDetails.stock_amount = (objMemberStockDetails.stock_amount - objIssuePointsTransferMaster.points_issued) + issuePointsTransferMaster.points_issued;
                    objMemberStockDetails.updated_date = DateTime.UtcNow;
                    objMemberStockDetails.updated_by = issuePointsTransferMaster.updated_by;
                    _memberStockDetailsRepository.Edit(objMemberStockDetails);

                    Models.Member_Stock_Details objAdminStockDetails = null;
                    objAdminStockDetails = _memberStockDetailsRepository.FindBy(x => x.user_id == objIssuePointsTransferMaster.issuer_user_id && x.stock_code_id == issuePointsTransferMaster.stockCodeId).FirstOrDefault();
                    if (issuePointsTransferMaster.points_issued != objIssuePointsTransferMaster.points_issued)
                    {
                        if (issuePointsTransferMaster.points_issued > objIssuePointsTransferMaster.points_issued)
                        {
                            objAdminStockDetails.stock_amount = (objAdminStockDetails.stock_amount - (issuePointsTransferMaster.points_issued - objIssuePointsTransferMaster.points_issued));
                            objAdminStockDetails.updated_date = DateTime.UtcNow;
                            objAdminStockDetails.updated_by = issuePointsTransferMaster.updated_by;
                            _memberStockDetailsRepository.Edit(objAdminStockDetails);
                            result = 1;
                        }
                        else
                        {
                            objAdminStockDetails.stock_amount = (objAdminStockDetails.stock_amount + (objIssuePointsTransferMaster.points_issued - issuePointsTransferMaster.points_issued));
                            objAdminStockDetails.updated_date = DateTime.UtcNow;
                            objAdminStockDetails.updated_by = issuePointsTransferMaster.updated_by;
                            _memberStockDetailsRepository.Edit(objAdminStockDetails);
                            result = 1;
                        }
                    }
                    else
                    {
                        //old amount and modified point quantity is same
                        result = 1;
                    }

                    objIssuePointsTransferMaster.points_issued = issuePointsTransferMaster.points_issued;
                    objIssuePointsTransferMaster.updated_date = DateTime.UtcNow;
                    _issuePointsTransferMasterRepository.Edit(objIssuePointsTransferMaster);
   
                    
                }
                else
                {
                    result = 2;
                }
                response = request.CreateResponse<int>(HttpStatusCode.OK, result); return response;
            });
        }
        // code made by umang on 26/07/16 //
        /// <summary>
        /// Code for Add issue point self
        /// </summary>
        /// <param name="request"></param>
        /// <param name="issuePointsTransferMaster"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddIssuePointSelf(HttpRequestMessage request, Issue_Points_Transfer_Master issuePointsTransferMaster)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                Issue_Withdrawel_Permission_Master withdrawelPermission = AutoMapperHelper.GetInstance().Map<Issue_Withdrawel_Permission_Master>(_issueWithdrawelPermissionMaster.GetAll().Where(x => x.permission.ToLower() == "approved").FirstOrDefault());
                Issue_Withdrawel_Permission_Master withdrawelPermissionInProgress = AutoMapperHelper.GetInstance().Map<Issue_Withdrawel_Permission_Master>(_issueWithdrawelPermissionMaster.GetAll().Where(x => x.permission.ToLower() == "in progress").FirstOrDefault());

                Models.Issue_Points_Transfer_Master objIssuePointsTransferMaster = new Models.Issue_Points_Transfer_Master();
                objIssuePointsTransferMaster = AutoMapperHelper.GetInstance().Map<Models.Issue_Points_Transfer_Master>(issuePointsTransferMaster);
                Models.Member_Stock_Details objMemberStockDetailsSuperAdmin = null;
                objMemberStockDetailsSuperAdmin = _memberStockDetailsRepository.FindBy(x => x.user_id == objIssuePointsTransferMaster.issuer_user_id && x.stock_code_id == objIssuePointsTransferMaster.stockCodeId).FirstOrDefault();
                string issuarId = objIssuePointsTransferMaster.issuer_user_id;
                string receverId = objIssuePointsTransferMaster.receiver_user_id;
                Models.Issue_Point_Transfer_Detail objPointTransactionDetailReceiver = new Models.Issue_Point_Transfer_Detail();
                if (issuarId != receverId)
                {

                    objIssuePointsTransferMaster.created_by = objIssuePointsTransferMaster.issuer_user_id;
                    objIssuePointsTransferMaster.created_date = DateTime.UtcNow;
                    objIssuePointsTransferMaster.issue_points_on_date = DateTime.UtcNow;
                    objIssuePointsTransferMaster.points_issue_permission_id = withdrawelPermissionInProgress.id;
                    objIssuePointsTransferMaster.updated_by = objIssuePointsTransferMaster.issuer_user_id;
                    objIssuePointsTransferMaster.updated_date = DateTime.UtcNow;
                    objIssuePointsTransferMaster.description = "Request send to superadmin";
                    _issuePointsTransferMasterRepository.Add(objIssuePointsTransferMaster);

                    result = 2;

                }
                else
                {
                    objIssuePointsTransferMaster.created_by = objIssuePointsTransferMaster.issuer_user_id;
                    objIssuePointsTransferMaster.created_date = DateTime.UtcNow;
                    objIssuePointsTransferMaster.issue_points_on_date = DateTime.UtcNow;
                    objIssuePointsTransferMaster.points_issue_permission_id = withdrawelPermission.id;
                    objIssuePointsTransferMaster.updated_by = objIssuePointsTransferMaster.issuer_user_id;
                    objIssuePointsTransferMaster.updated_date = DateTime.UtcNow;
                    objIssuePointsTransferMaster.opening_amount = objMemberStockDetailsSuperAdmin.stock_amount;
                    objIssuePointsTransferMaster.closing_amount = objMemberStockDetailsSuperAdmin.stock_amount + objIssuePointsTransferMaster.points_issued;
                    objIssuePointsTransferMaster.description = "Point Issued to Self By SuperAdmin";
                    _issuePointsTransferMasterRepository.Add(objIssuePointsTransferMaster);

                    //need to add later foe point view detail history

                    objPointTransactionDetailReceiver.issuer_account_id = objIssuePointsTransferMaster.issuer_user_id;
                    objPointTransactionDetailReceiver.receiver_account_id = objIssuePointsTransferMaster.issuer_user_id;
                    objPointTransactionDetailReceiver.point_issued_on_date = DateTime.UtcNow;
                    objPointTransactionDetailReceiver.opening_amount = objMemberStockDetailsSuperAdmin.stock_amount;
                    objPointTransactionDetailReceiver.transaction_amount = issuePointsTransferMaster.points_issued;
                    objPointTransactionDetailReceiver.closing_amount = objMemberStockDetailsSuperAdmin.stock_amount + objIssuePointsTransferMaster.points_issued;
                    objPointTransactionDetailReceiver.description = "Point Issued to Self By SuperAdmin";
                    objPointTransactionDetailReceiver.stock_id = issuePointsTransferMaster.stockCodeId;
                    _issuePointTransferDetailRepository.Add(objPointTransactionDetailReceiver);

                    objMemberStockDetailsSuperAdmin.stock_amount = objMemberStockDetailsSuperAdmin.stock_amount + objIssuePointsTransferMaster.points_issued;
                    _memberStockDetailsRepository.Edit(objMemberStockDetailsSuperAdmin);

                    result = 1;
                }
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }

    
        public HttpResponseMessage GetPointIssueRequest(HttpRequestMessage request)
        {
            HttpResponseMessage response = null;
            AdminIsuuePointsViewModel model = new AdminIsuuePointsViewModel();
            List<Issue_Points_Transfer_Master> lstIssuePointRequest = new List<Issue_Points_Transfer_Master>();
            List<Topup_Status_Master> lstTopupStatus = new List<Topup_Status_Master>();
            if (_issuePointsTransferMasterRepository.GetAll().ToList().Count > 0)
            {
                lstIssuePointRequest = AutoMapperHelper.GetInstance().Map<List<Issue_Points_Transfer_Master>>(_issuePointsTransferMasterRepository.GetAll().ToList());
            }
            if (_topupStatusMasterRepository.GetAll().ToList().Count > 0)
            {
                lstTopupStatus = AutoMapperHelper.GetInstance().Map<List<Topup_Status_Master>>(_topupStatusMasterRepository.GetAll().ToList());
            }
            if (_issueWithdrawelPermissionMaster.GetAll().ToList().Count > 0)
            {
                model.IssueWithdrawelPermissionMaster = AutoMapperHelper.GetInstance().Map<List<Issue_Withdrawel_Permission_Master>>(_issueWithdrawelPermissionMaster.GetAll().ToList());
            }
            model.IssuePointsTransferMaster = lstIssuePointRequest;
            model.TopupStatusMaster = lstTopupStatus;
            response = request.CreateResponse<AdminIsuuePointsViewModel>(HttpStatusCode.OK, model);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage ChangeSelfIssuePointStatus(HttpRequestMessage request, Issue_Points_Transfer_Master model)
        {
            int result = 0;
            HttpResponseMessage response = null;
            string superAdminUserId = ConfigurationManager.AppSettings["SuperAdminUserId"];
            Issue_Withdrawel_Permission_Master status = AutoMapperHelper.GetInstance().Map<Issue_Withdrawel_Permission_Master>(_issueWithdrawelPermissionMaster.GetAll().Where(x => x.id == model.points_issue_permission_id).FirstOrDefault());
            Issue_Withdrawel_Permission_Master withdrawelPermission = AutoMapperHelper.GetInstance().Map<Issue_Withdrawel_Permission_Master>(_issueWithdrawelPermissionMaster.GetAll().Where(x => x.permission.ToLower() == "approved").FirstOrDefault());
            Models.Issue_Points_Transfer_Master issuePointTransferMaster = new Models.Issue_Points_Transfer_Master();
            Models.Issue_Points_Transfer_Master issuePointTransferMasterSuperAdmin = new Models.Issue_Points_Transfer_Master();
            issuePointTransferMasterSuperAdmin = AutoMapperHelper.GetInstance().Map<Models.Issue_Points_Transfer_Master>(model);
            issuePointTransferMaster = AutoMapperHelper.GetInstance().Map<Models.Issue_Points_Transfer_Master>(_issuePointsTransferMasterRepository.GetAll().Where(x => x.id == model.id).FirstOrDefault());
            Models.Issue_Points_Master issuePointMaster = new Models.Issue_Points_Master();
            if (model.points_issue_permission_id == withdrawelPermission.id)
            {


                Models.Member_Stock_Details objMemberStockDetails = null;
                objMemberStockDetails = _memberStockDetailsRepository.FindBy(x => x.user_id == issuePointTransferMaster.receiver_user_id && x.stock_code_id == issuePointTransferMaster.stockCodeId).FirstOrDefault();
                Models.Member_Stock_Details objMemberStockDetailsSuperAdmin = null;
                //objMemberStockDetailsSuperAdmin = _memberStockDetailsRepository.FindBy(x => x.user_id == superAdminUserId && x.stock_code_id == model.stockCodeId).FirstOrDefault();
                //if (objMemberStockDetailsSuperAdmin.stock_amount > issuePointTransferMaster.points_issued)
                //{
                    Models.Issue_Point_Transfer_Detail objPointTransactionDetailReceiver = new Models.Issue_Point_Transfer_Detail();
                    Models.Issue_Point_Transfer_Detail objpointTransactionDetailIssuer = new Models.Issue_Point_Transfer_Detail();
                    try
                    {

                        issuePointTransferMaster.points_issue_permission_id = model.points_issue_permission_id;
                        issuePointTransferMaster.issuer_user_id = superAdminUserId;
                        issuePointTransferMaster.opening_amount = objMemberStockDetails.stock_amount;
                        issuePointTransferMaster.description = "Point Recived to Admin By SuperAdmin";
                        issuePointTransferMaster.closing_amount = objMemberStockDetails.stock_amount + issuePointTransferMaster.points_issued;

                        _issuePointsTransferMasterRepository.Edit(issuePointTransferMaster);


                        // need to add later for point view detail for reciver

                        objPointTransactionDetailReceiver.issuer_account_id = superAdminUserId;
                        objPointTransactionDetailReceiver.receiver_account_id = issuePointTransferMaster.receiver_user_id;
                        objPointTransactionDetailReceiver.point_issued_on_date = DateTime.UtcNow;
                        objPointTransactionDetailReceiver.opening_amount = objMemberStockDetails.stock_amount;
                        objPointTransactionDetailReceiver.transaction_amount = issuePointTransferMaster.points_issued;
                        objPointTransactionDetailReceiver.closing_amount = objMemberStockDetails.stock_amount + issuePointTransferMaster.points_issued;
                        objPointTransactionDetailReceiver.description = "Point Recived to Admin By SuperAdmin";
                        objPointTransactionDetailReceiver.stock_id = issuePointTransferMaster.stockCodeId;
                        _issuePointTransferDetailRepository.Add(objPointTransactionDetailReceiver);

                        // need to add later for point view detail for issuer
                        //objpointTransactionDetailIssuer.issuer_account_id = superAdminUserId;
                        //objpointTransactionDetailIssuer.receiver_account_id = issuePointTransferMaster.receiver_user_id;
                        //objpointTransactionDetailIssuer.point_issued_on_date = DateTime.UtcNow;
                        //objpointTransactionDetailIssuer.opening_amount = objMemberStockDetailsSuperAdmin.stock_amount;
                        //objpointTransactionDetailIssuer.transaction_amount = issuePointTransferMaster.points_issued;
                        //objpointTransactionDetailIssuer.closing_amount = objMemberStockDetailsSuperAdmin.stock_amount - issuePointTransferMaster.points_issued;
                        //objpointTransactionDetailIssuer.description = "Point Issued to Admin By SuperAdmin";
                        //objpointTransactionDetailIssuer.stock_id = issuePointTransferMaster.stockCodeId;
                        //_issuePointTransferDetailRepository.Add(objpointTransactionDetailIssuer);


                        issuePointMaster.created_date = DateTime.UtcNow;
                        issuePointMaster.updated_date = DateTime.UtcNow;
                        issuePointMaster.created_by = model.issuer_user_id;
                        issuePointMaster.updated_by = model.issuer_user_id;
                        issuePointMaster.issue_points_expiry_date = DateTime.UtcNow;
                        issuePointMaster.points_issued_date = DateTime.UtcNow;
                        issuePointMaster.transfer_id = model.id;
                        issuePointMaster.user_id = issuePointTransferMaster.receiver_user_id;
                        issuePointMaster.stock_code_id = issuePointTransferMaster.stockCodeId;
                        issuePointMaster.status = "Completed";
                        _issuePointsMasterRepository.Add(issuePointMaster);


                        if (objMemberStockDetails == null)
                        {
                            objMemberStockDetails = new Models.Member_Stock_Details();
                            //Needs to be added later
                            objMemberStockDetails.account_id = 1;
                            objMemberStockDetails.created_by = issuePointTransferMaster.issuer_user_id;
                            objMemberStockDetails.created_date = DateTime.UtcNow;
                            objMemberStockDetails.stock_amount = issuePointTransferMaster.points_issued;
                            objMemberStockDetails.stock_code_id = issuePointTransferMaster.stockCodeId;
                            objMemberStockDetails.updated_by = issuePointTransferMaster.issuer_user_id;
                            objMemberStockDetails.updated_date = DateTime.UtcNow;
                            objMemberStockDetails.user_id = issuePointTransferMaster.receiver_user_id;
                            _memberStockDetailsRepository.Add(objMemberStockDetails);
                        }
                        else
                        {
                            objMemberStockDetails.stock_amount = objMemberStockDetails.stock_amount + issuePointTransferMaster.points_issued;
                            objMemberStockDetails.updated_by = issuePointTransferMaster.issuer_user_id;
                            objMemberStockDetails.updated_date = DateTime.UtcNow;
                            _memberStockDetailsRepository.Edit(objMemberStockDetails);

                            //******************** Deduct points approved by superadmin requested by admin from super admin's wallet*****************************//
                            //objMemberStockDetailsSuperAdmin.stock_amount = objMemberStockDetailsSuperAdmin.stock_amount - issuePointTransferMaster.points_issued;
                            //objMemberStockDetailsSuperAdmin.updated_by = issuePointTransferMaster.issuer_user_id;
                            //objMemberStockDetailsSuperAdmin.updated_date = DateTime.UtcNow;
                            //_memberStockDetailsRepository.Edit(objMemberStockDetailsSuperAdmin);


                        }
                        result = 1;

                    }
                    catch (Exception ex)
                    {
                        result = 0;
                        string messsage = ex.InnerException.ToString();
                    }
                //}
                //else
                //{
                //    result = 2;
                //}
            }
            else
            {
                issuePointTransferMaster.points_issue_permission_id = model.points_issue_permission_id;
                issuePointTransferMaster.updated_date = DateTime.UtcNow;
                _issuePointsTransferMasterRepository.Edit(issuePointTransferMaster);
                result = 1;
            }
            response = request.CreateResponse<int>(HttpStatusCode.OK, result);
            return response;
        }

        //below code made by umang on 14-09-2016
        /// <summary>
        /// Code For RewardPointViewDetail history
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetPointHistoryViewDetails(HttpRequestMessage request, string userId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                string superAdminId = ConfigurationManager.AppSettings["SuperAdminUserId"];
                int stockId = Convert.ToInt32(ConfigurationManager.AppSettings["FirstStock"]);
                List<Issue_Point_Transfer_Detail> lstPointTransactionHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Point_Transfer_Detail>>(_issuePointTransferDetailRepository.GetAll()).ToList();
                if (userId != superAdminId)
                {
                    lstPointTransactionHistory = lstPointTransactionHistory.Where(x => ((x.issuer_account_id == userId) || (x.issuer_account_id == superAdminId && x.receiver_account_id == userId)) && (x.description.ToLower() == "point recived to admin by superadmin" || x.description.ToLower() == "point issued to member by admin") && (x.stock_id == stockId)).ToList();
                }
                else
                {
                    lstPointTransactionHistory = lstPointTransactionHistory.Where(x => (x.issuer_account_id == userId) && (x.description.ToLower() == "point issued to admin by superadmin" || x.description.ToLower() == "point issued to self by superadmin" || x.description.ToLower() == "point issued to member by superadmin") && (x.stock_id == stockId)).ToList();
                }
                lstPointTransactionHistory = lstPointTransactionHistory.Select(x => new Wollo.Entities.ViewModels.Issue_Point_Transfer_Detail
                {
                    id = x.id,
                    point_issued_on_date = x.point_issued_on_date.ToLocalTime(),
                    IssuerUsers = x.IssuerUsers,
                    ReceiverUser = x.ReceiverUser,
                    opening_amount = x.opening_amount,
                    closing_amount = x.closing_amount,
                    transaction_amount = x.transaction_amount,

                }).ToList();

                response = request.CreateResponse<List<Issue_Point_Transfer_Detail>>(HttpStatusCode.OK, lstPointTransactionHistory);
                return response;
            });
        }

        [HttpGet]
        public HttpResponseMessage GetTestRewardPointHistoryViewDetails(HttpRequestMessage request, string userId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                string superAdminId = ConfigurationManager.AppSettings["SuperAdminUserId"];
                int stockId = Convert.ToInt32(ConfigurationManager.AppSettings["SecondStock"]);
                List<Issue_Point_Transfer_Detail> lstPointTransactionHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Point_Transfer_Detail>>(_issuePointTransferDetailRepository.GetAll()).ToList();
                if (userId != superAdminId)
                {
                    lstPointTransactionHistory = lstPointTransactionHistory.Where(x => ((x.issuer_account_id == userId) || (x.issuer_account_id == superAdminId && x.receiver_account_id == userId)) && (x.description.ToLower() == "point recived to admin by superadmin" || x.description.ToLower() == "point issued to member by admin") && (x.stock_id == stockId)).ToList();
                }
                else
                {
                    lstPointTransactionHistory = lstPointTransactionHistory.Where(x => (x.issuer_account_id == userId) && (x.description.ToLower() == "point issued to admin by superadmin" || x.description.ToLower() == "point issued to self by superadmin" || x.description.ToLower() == "point issued to member by superadmin") && (x.stock_id == stockId)).ToList();
                }
                lstPointTransactionHistory = lstPointTransactionHistory.Select(x => new Wollo.Entities.ViewModels.Issue_Point_Transfer_Detail
                {
                    id = x.id,
                    point_issued_on_date = x.point_issued_on_date.ToLocalTime(),
                    IssuerUsers = x.IssuerUsers,
                    ReceiverUser = x.ReceiverUser,
                    opening_amount = x.opening_amount,
                    closing_amount = x.closing_amount,
                    transaction_amount = x.transaction_amount,

                }).ToList();

                response = request.CreateResponse<List<Issue_Point_Transfer_Detail>>(HttpStatusCode.OK, lstPointTransactionHistory);
                return response;
            });
        }


        //******************************Code for filtering bt umang 11 10 2016 ***********************************//
        /// <summary>
        /// range filter for Reward point history view detail
        /// </summary>
        /// <param name="request"></param>
        /// <param name="master"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RangeFilterRewardPointHistoryViewDetail(HttpRequestMessage request, Issue_Points_Transfer_Master master, string userId)
        {
            HttpResponseMessage response = null;
            string superAdminId = ConfigurationManager.AppSettings["SuperAdminUserId"];
            int stockId = Convert.ToInt32(ConfigurationManager.AppSettings["FirstStock"]);
            List<Issue_Point_Transfer_Detail> lstPointTransactionHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Point_Transfer_Detail>>(_issuePointTransferDetailRepository.GetAll()).ToList();
            if (userId != superAdminId)
            {
                lstPointTransactionHistory = lstPointTransactionHistory.Where(x => ((x.issuer_account_id == userId) || (x.issuer_account_id == superAdminId && x.receiver_account_id == userId)) && (x.description.ToLower() == "point recived to admin by superadmin" || x.description.ToLower() == "point issued to member by admin") && (x.stock_id == stockId)).ToList();
            }
            else
            {
                lstPointTransactionHistory = lstPointTransactionHistory.Where(x => (x.issuer_account_id == userId) && (x.description.ToLower() == "point issued to admin by superadmin" || x.description.ToLower() == "point issued to self by superadmin" || x.description.ToLower() == "point issued to member by superadmin") && (x.stock_id == stockId)).ToList();
            }
            if (master.created_date == master.updated_date)
            {
                lstPointTransactionHistory = lstPointTransactionHistory.Where(x => x.point_issued_on_date.Date == master.created_date.Value.Date).ToList();
            }
            else
            {
                lstPointTransactionHistory = lstPointTransactionHistory.Where(x => x.point_issued_on_date.Date >= master.created_date.Value.Date && x.point_issued_on_date.Date <= master.updated_date.Value.Date).ToList();

            }
            lstPointTransactionHistory = lstPointTransactionHistory.Select(x => new Wollo.Entities.ViewModels.Issue_Point_Transfer_Detail
            {
                id = x.id,
                point_issued_on_date = x.point_issued_on_date.ToLocalTime(),
                IssuerUsers = x.IssuerUsers,
                ReceiverUser = x.ReceiverUser,
                opening_amount = x.opening_amount,
                closing_amount = x.closing_amount,
                transaction_amount = x.transaction_amount,

            }).ToList();

            response = request.CreateResponse<List<Issue_Point_Transfer_Detail>>(HttpStatusCode.OK, lstPointTransactionHistory);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage RangeFilterTestPointHistoryViewDetail(HttpRequestMessage request, Issue_Points_Transfer_Master master, string userId)
        {
            HttpResponseMessage response = null;
            string superAdminId = ConfigurationManager.AppSettings["SuperAdminUserId"];
            int stockId = Convert.ToInt32(ConfigurationManager.AppSettings["SecondStock"]);
            List<Issue_Point_Transfer_Detail> lstPointTransactionHistory = AutoMapperHelper.GetInstance().Map<List<Issue_Point_Transfer_Detail>>(_issuePointTransferDetailRepository.GetAll()).ToList();
            if (userId != superAdminId)
            {
                lstPointTransactionHistory = lstPointTransactionHistory.Where(x => ((x.issuer_account_id == userId) || (x.issuer_account_id == superAdminId && x.receiver_account_id == userId)) && (x.description.ToLower() == "point recived to admin by superadmin" || x.description.ToLower() == "point issued to member by admin") && (x.stock_id == stockId)).ToList();
            }
            else
            {
                lstPointTransactionHistory = lstPointTransactionHistory.Where(x => (x.issuer_account_id == userId) && (x.description.ToLower() == "point issued to admin by superadmin" || x.description.ToLower() == "point issued to self by superadmin" || x.description.ToLower() == "point issued to member by superadmin") && (x.stock_id == stockId)).ToList();
            }
            if (master.created_date == master.updated_date)
            {
                lstPointTransactionHistory = lstPointTransactionHistory.Where(x => x.point_issued_on_date.Date == master.created_date.Value.Date).ToList();
            }
            else
            {
                lstPointTransactionHistory = lstPointTransactionHistory.Where(x => x.point_issued_on_date.Date >= master.created_date.Value.Date && x.point_issued_on_date.Date <= master.updated_date.Value.Date).ToList();

            }
            lstPointTransactionHistory = lstPointTransactionHistory.Select(x => new Wollo.Entities.ViewModels.Issue_Point_Transfer_Detail
            {
                id = x.id,
                point_issued_on_date = x.point_issued_on_date.ToLocalTime(),
                IssuerUsers = x.IssuerUsers,
                ReceiverUser = x.ReceiverUser,
                opening_amount = x.opening_amount,
                closing_amount = x.closing_amount,
                transaction_amount = x.transaction_amount,

            }).ToList();

            response = request.CreateResponse<List<Issue_Point_Transfer_Detail>>(HttpStatusCode.OK, lstPointTransactionHistory);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage RangeFilterUser(HttpRequestMessage request, Issue_Points_Master master, string userId)
        {
            return CreateHttpResponse(request, () =>
            {

                HttpResponseMessage response = null;
                IssuePointDetails objIssuePointDetails = new IssuePointDetails();
                List<Issue_Points_Master> issuepointMaster = new List<Issue_Points_Master>();
                List<Issue_Points_Transfer_Master> issuePointTransferMaster = new List<Issue_Points_Transfer_Master>();
                objIssuePointDetails.IssuePointsMaster = issuepointMaster;
                objIssuePointDetails.IssuePointTransferMaster = issuePointTransferMaster;
                objIssuePointDetails.IssuePointsMaster = AutoMapperHelper.GetInstance().Map<List<Issue_Points_Master>>(_issuePointsMasterRepository.FindBy(x => x.stock_code_id == master.stock_code_id && x.user_id == userId).ToList());
                if (master.created_date == master.updated_date)
                {
                    objIssuePointDetails.IssuePointsMaster = objIssuePointDetails.IssuePointsMaster.Where(x => x.created_date.Value.Date == master.created_date.Value.Date).ToList();
                }
                else
                {
                    objIssuePointDetails.IssuePointsMaster = objIssuePointDetails.IssuePointsMaster.Where(x => x.created_date.Value.Date >= master.created_date.Value.Date && x.created_date.Value.Date <= master.updated_date.Value.Date).ToList();
                }
                objIssuePointDetails.TotalPoints = objIssuePointDetails.IssuePointsMaster.Sum(x => x.IssuePointsTransferMaster.points_issued);
                objIssuePointDetails.RemainingPoints = objIssuePointDetails.IssuePointsMaster.Where(x => x.status != "Cancelled" && x.status != "In Progress").Sum(x => x.IssuePointsTransferMaster.points_issued);
                objIssuePointDetails.CancelledPoints = objIssuePointDetails.IssuePointsMaster.Where(x => x.status == "Cancelled").Sum(x => x.IssuePointsTransferMaster.points_issued);
                
                response = request.CreateResponse<IssuePointDetails>(HttpStatusCode.OK, objIssuePointDetails);
                return response;
            });
        }



        //**********************************************end here*************************************************//


    }
}
