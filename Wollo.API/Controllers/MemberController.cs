using System;
using System.Configuration;
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
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Validation;
using Wollo.Common.UI;
using Wollo.Common.AutoMapper;
using Wollo.Base.Utilities;
using System.Web;

namespace Wollo.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MemberController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Models.Wallet_Details> _walletDetailsRepository;
        private readonly IEntityBaseRepository<Models.Wallet_Master> _walletMasterRepository;
        private readonly IEntityBaseRepository<Models.Member_Stock_Details> _memberStockDetailsRepository;
        private readonly IEntityBaseRepository<Models.Stock_Code> _stockCodeRepository;
        private readonly IEntityBaseRepository<Models.User> _userRepository;
        private readonly IEntityBaseRepository<Models.Country_Master> _countryRepository;
        private readonly IEntityBaseRepository<Models.Wollo_Member_Details> _wolloMemberDetailsRepository;

        public MemberController(
            IEntityBaseRepository<Models.Wallet_Details> walletDetailsRepository,
            IEntityBaseRepository<Models.Wallet_Master> walletMasterRepository,
            IEntityBaseRepository<Models.Member_Stock_Details> memberStockDetailsRepository,
            IEntityBaseRepository<Models.Stock_Code> stockCodeRepository,
            IEntityBaseRepository<Models.User> userRepository,
            IEntityBaseRepository<Models.Country_Master> countryRepository,
            IEntityBaseRepository<Models.Wollo_Member_Details> wolloMemberDetailsRepository,
            IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _walletMasterRepository = walletMasterRepository;
            _walletDetailsRepository = walletDetailsRepository;
            _memberStockDetailsRepository = memberStockDetailsRepository;
            _stockCodeRepository = stockCodeRepository;
            _userRepository = userRepository;
            _countryRepository = countryRepository;
            _wolloMemberDetailsRepository = wolloMemberDetailsRepository;
        }

        /// <summary>
        ///  api to get receive newly created member details from wollo to rpe
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SendMembershipDataFromWolloToRPE(HttpRequestMessage request, Wollo.Entities.ViewModels.WolloMembershipDataViewModel model)
        {
            HttpResponseMessage response = null;
            Result data = new Result();
            int result = 0;
            string message = String.Empty;
            if (_wolloMemberDetailsRepository.FindBy(x => x.email_address.ToLower().Trim() == model.email_address.ToLower().Trim() && x.user_name.ToLower().Trim() == model.username.ToLower().Trim()).Any())
            {
                result = 7;
            }
            else if (_wolloMemberDetailsRepository.FindBy(x => x.email_address.ToLower().Trim() == model.email_address.ToLower().Trim()).Any())
            {
                result = 6;
            }
            else if (_wolloMemberDetailsRepository.FindBy(x => x.user_name.ToLower().Trim() == model.username).Any())
            {
                result = 5;
            }
            else if (_userRepository.FindBy(x => x.email_address.ToLower().Trim() == model.email_address.ToLower().Trim() && x.user_name.ToLower().Trim() == model.username.ToLower().Trim()).Any())
            {
                result = 4;
            }
            else if (_userRepository.FindBy(x => x.email_address.ToLower().Trim() == model.email_address.ToLower().Trim()).Any())
            {
                result = 3;
            }
            else if (_userRepository.FindBy(x => x.user_name.ToLower().Trim() == model.username.ToLower().Trim()).Any())
            {
                result = 2;
            }
            else
            {
                try
                {
                    Models.Wollo_Member_Details objWolloMemberDetails = new Models.Wollo_Member_Details();
                    objWolloMemberDetails.created_date = DateTime.UtcNow;
                    objWolloMemberDetails.updated_date = DateTime.UtcNow;
                    objWolloMemberDetails.created_by = "system";
                    objWolloMemberDetails.updated_by = "system";
                    objWolloMemberDetails.user_name = model.username;
                    objWolloMemberDetails.first_name = model.firstname;
                    objWolloMemberDetails.last_name = model.lastname;
                    objWolloMemberDetails.country_id = model.country_id;
                    objWolloMemberDetails.city = model.city;
                    objWolloMemberDetails.street = String.Join(" ", model.street);
                    objWolloMemberDetails.telephone = model.telephone;
                    objWolloMemberDetails.dob = model.dob;
                    objWolloMemberDetails.region_id = model.region_id;
                    objWolloMemberDetails.region = model.region;
                    objWolloMemberDetails.company = model.company;
                    objWolloMemberDetails.password = model.password;
                    objWolloMemberDetails.confirmation = model.confirmation;
                    objWolloMemberDetails.is_subscribed = model.is_subscribed;
                    objWolloMemberDetails.gender = model.gender;
                    objWolloMemberDetails.postcode = model.postcode;
                    objWolloMemberDetails.email_address = model.email_address;
                    _wolloMemberDetailsRepository.Add(objWolloMemberDetails);
                    result = 1;
                }
                catch (Exception ex)
                {
                    message = ex.InnerException.ToString();
                    result = 0;
                }
            }
            data.status = result;
            if (result == 1)
            {
                string loginPageUrl = ConfigurationManager.AppSettings["LoginPageUrl"];
                Models.User user = new Models.User();
                user.email_address = model.email_address;
                user.user_name = model.username;
                user.password = model.password;
                try
                {
                    Mail.sendMail(user, "Alternate Asset Exchange Registration", "<div><p>Congratulations! You are registered with us. Please navigate to <a href='" + loginPageUrl + "' title='Alternative Asset Excange Platform'>" + loginPageUrl + "</a> to activate your account.Thank you!</p><p><b>User Name: </b>" + model.username + "</p><p><b>Password: </b>" + model.password + "</p></div>");
                    data.msg = "Member Details Transferred successfully.";
                
                }
                catch (Exception ex)
                {
                    data.msg = ex.InnerException.ToString();
                
                }
                response = request.CreateResponse(HttpStatusCode.OK, data);
            }
            else if (result == 2)
            {
                data.msg = "User Name: " + model.username + " already exist in RPE.";
                response = request.CreateResponse(HttpStatusCode.OK, data);
            }
            else if (result == 3)
            {
                data.msg = "Email Address: " + model.email_address + " already exist in RPE.";
                response = request.CreateResponse(HttpStatusCode.OK, data);
            }
            else if (result == 4)
            {
                data.msg = "User with Email Address: " + model.email_address + " and User name: "+model.username+" already exist in RPE.";
                response = request.CreateResponse(HttpStatusCode.OK, data);
            }
            else if(result==5)
            {
                data.msg = "Member details with User Name: " + model.username + " already exist in RPE.";
                response = request.CreateResponse(HttpStatusCode.OK, data);
            }
            else if (result == 6)
            {
                data.msg = "Member details with Email Address: " + model.email_address + " already exist in RPE.";
                response = request.CreateResponse(HttpStatusCode.OK, data);
            }
            else if (result == 7)
            {
                data.msg = "Member details with Email Address: " + model.email_address + " and User name: " + model.username + " already exist in RPE.";
                response = request.CreateResponse(HttpStatusCode.OK, data);
            }
            else
            {
                data.msg = message;
                response = request.CreateResponse(HttpStatusCode.OK, data);
            }
            return response;
        }

        /// <summary>
        ///  api to check if user is from and has been activated from rpe
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUserDetails(HttpRequestMessage request, Models.User user)
        {
            bool[] isWolloUser = new bool[2];
            isWolloUser[0] = false;
            HttpResponseMessage response = null;
            Models.Wollo_Member_Details details = _wolloMemberDetailsRepository.GetAll().Where(x => x.user_name == user.user_name && x.password == user.password).FirstOrDefault();
            if (details != null)
            {
                Models.User userDetails = _userRepository.GetAll().Where(x => x.user_name == user.user_name && x.is_deleted == false).FirstOrDefault();
                if (userDetails == null)
                {
                    isWolloUser[0] = true;
                    isWolloUser[1] = false;
                }
                else
                {
                    isWolloUser[0] = true;
                    isWolloUser[1] = true;
                }
            }
            response = request.CreateResponse(HttpStatusCode.OK, isWolloUser);
            return response;
        }

        /// <summary>
        /// Send link for forgot password
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage ForgotPassword(HttpRequestMessage request, ForgotPasswordViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                Wollo.Entities.Models.User user = _userRepository.GetAll().Where(x => x.email_address == model.Email).FirstOrDefault();
                if (user != null)
                {
                    Cipher objCipher = new Cipher();
                    string token = objCipher.Encrypt(user.user_id.ToString());
                    string email = objCipher.Encrypt(model.Email);
                    string url = ConfigurationManager.AppSettings["ResetLink"] + "Account/ResetPassword/?Token=" + token + "&Email=" + email;
                    Mail.sendMail(user, "Reset Password", "<div><p>We request you to follow the link below to reset the password.</p><p><a href='" + url + "' title='Set Account Passwords'>" + url + "</a></p></div>");
                    response = request.CreateResponse(HttpStatusCode.OK, 1);
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, 2);
                }
                return response;
            });
        }

        /// <summary>
        /// To check if email already exist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage FindEmail(HttpRequestMessage request, string email)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (_userRepository.GetAll().ToList().Count > 0)
                {
                    var userinfo = AutoMapperHelper.GetInstance().Map<User>(_userRepository.FindBy(x => x.email_address == email).FirstOrDefault());
                    response = request.CreateResponse(HttpStatusCode.OK, userinfo);
                    return response;
                }
                else
                {
                    // no data
                }
                return response;
            });
        }

        /// <summary>
        /// Add email id to user table for currently registered user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateUser(HttpRequestMessage request, Models.User model)
        {
            int result = 0;
            HttpResponseMessage response = null;
            Models.User userinfo = new Models.User();
            if (model.email_address == null)
            {
                Models.Wollo_Member_Details details = _wolloMemberDetailsRepository.GetAll().Where(x => x.user_name == model.user_name).FirstOrDefault();
                model.email_address = details.email_address;
                model.password = details.password;
                model.first_name = details.first_name;
                model.last_name = details.last_name;
                model.is_deleted = false;
                model.address = details.city + " " + details.street + " " + details.region + " " + details.region_id + " " + details.postcode;
                model.phone_number = details.telephone;
                model.city = details.city;
                model.country_id = details.country_id;
                model.created_date = details.created_date;
                model.updated_date = details.updated_date;
                model.created_by = details.created_by;
                model.updated_by = details.updated_by;
            }
            try
            {
                userinfo = AutoMapperHelper.GetInstance().Map<Models.User>(_userRepository.FindBy(x => x.user_id == model.user_id).FirstOrDefault()); ;
                if (_userRepository.GetAll().Where(x => x.email_address == model.email_address && x.user_name != model.user_name).Any())
                {
                    result = 2;
                }
                else if (_userRepository.GetAll().Where(x => x.user_id == model.user_id).Any())
                {

                    userinfo.company_id =(int) model.country_id ;
                    userinfo.user_statusid = model.user_statusid = 1;
                    userinfo.first_name = model.first_name;
                    userinfo.last_name = model.last_name;
                    userinfo.email_address = model.email_address;
                    _userRepository.Edit(userinfo);
                    result = 1;
                }
                else
                {
                    model.country_id = null;
                    model.user_statusid = 1;
                    _userRepository.Add(model);
                    result = 1;
                }
            }
            catch (DbEntityValidationException ex)
            {
                string message = ex.InnerException.Message.ToString();
            }
            catch (Exception ex)
            {
                result = 0;
            }

            response = request.CreateResponse<int>(HttpStatusCode.OK, result);
            return response;
        }

        /// <summary>
        /// Add email id to user table for currently registered user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateUser1(HttpRequestMessage request, Models.User model)
        {
            int result = 0;
            HttpResponseMessage response = null;
            //string userId=model.UserId;
            try
            {
                if (_userRepository.GetAll().Where(x => x.email_address == model.email_address && x.user_name != model.user_name).Any())
                {
                    result = 2;
                }
                else if (_userRepository.GetAll().Where(x => x.user_id == model.user_id).Any())
                {
                    model.country_id = 1;
                    model.user_statusid = 1;
                    _userRepository.Edit(model);
                    result = 1;
                }
                else
                {
                    model.country_id = 1;
                    model.user_statusid = 1;
                    _userRepository.Add(model);
                    result = 1;
                }
            }
            catch (DbEntityValidationException ex)
            {
                string message = ex.InnerException.Message.ToString();
            }
            catch (Exception ex)
            {
                result = 0;
            }

            response = request.CreateResponse<int>(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage DeleteUser(HttpRequestMessage request, string userId)
        {
            int result = 0;
            HttpResponseMessage response = null;
            try
            {
                Models.User user = _userRepository.GetAll().Where(x => x.user_id == userId).FirstOrDefault();
                user.is_deleted = !user.is_deleted;
                _userRepository.Edit(user);
                result = 1;
            }
            catch (Exception ex)
            {
                string message = ex.InnerException.Message.ToString();
            }
            response = request.CreateResponse<int>(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage HasSuperAdmin(HttpRequestMessage request, string roleId)
        {
            bool hasSuperAdmin = false;
            HttpResponseMessage response = null;
            try
            {
                if (_walletDetailsRepository.GetAll().Where(x => x.role_id == roleId).Any())
                {
                    Models.Wallet_Details details = _walletDetailsRepository.GetAll().Where(x => x.role_id == roleId).FirstOrDefault();
                    if (_userRepository.GetAll().Where(x => x.user_id == details.user_id && x.is_deleted != true).Any())
                    {
                        hasSuperAdmin = true;
                    }
                    else
                    {
                        hasSuperAdmin = false;
                    }
                }
                
            }
            catch (Exception ex)
            {
                string message = ex.InnerException.Message.ToString();
            }
            response = request.CreateResponse<bool>(HttpStatusCode.OK, hasSuperAdmin);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetAllUsers(HttpRequestMessage request)
        {
            HttpResponseMessage response = null;
            List<Models.User> resultObj = new List<Models.User>();
            //string adminUserId=ConfigurationManager.AppSettings["AdminUserId"];
            //if (_userRepository.GetAll().Where(x => x.user_id != adminUserId).Any())
            //{
            //    resultObj = _userRepository.GetAll().Where(x => x.user_id != adminUserId).ToList();
            //}
            resultObj = _userRepository.GetAll().Where(x=>x.is_deleted==false).ToList();
            response = request.CreateResponse<List<Models.User>>(HttpStatusCode.OK, resultObj);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetUserById(HttpRequestMessage request, string userId)
        {
            HttpResponseMessage response = null;
            Models.User resultObj = new Models.User();
            if (_userRepository.GetAll().Where(x => x.user_id == userId).Any())
            {
                resultObj = _userRepository.GetAll().Where(x => x.user_id == userId).FirstOrDefault();
            }
            response = request.CreateResponse<Models.User>(HttpStatusCode.OK, resultObj);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetUserByName(HttpRequestMessage request, string user_name)
        {
            HttpResponseMessage response = null;
            Models.User resultObj = new Models.User();
            if (_userRepository.GetAll().Where(x => x.user_name == user_name).Any())
            {
                resultObj = _userRepository.GetAll().Where(x => x.user_name == user_name).FirstOrDefault();
            }
            response = request.CreateResponse<Models.User>(HttpStatusCode.OK, resultObj);
            return response;
        }

        /// <summary>
        /// Get All cash/stock data to show on admin dashboard
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllAdminDashboardData(HttpRequestMessage request, string userId)
        {
            HttpResponseMessage response = null;
            DashboardViewModel resultObj = null;
            resultObj = new ExecSP().SQLQuery<DashboardViewModel>("CALL `wollorpe`.`GetAllAdminDashboardData`('" + userId + "');").SingleAsync().Result;
            response = request.CreateResponse<DashboardViewModel>(HttpStatusCode.OK, resultObj);
            return response;
        }

        /// <summary>
        /// Get All cash/stock data to show on member dashboard
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllMemberDashboardData(HttpRequestMessage request, string userId)
        {
            HttpResponseMessage response = null;
            DashboardViewModel resultObj = new DashboardViewModel();
            resultObj = new ExecSP().SQLQuery<DashboardViewModel>("CALL `wollorpe`.`GetAllMemberDashboardData`('" + userId + "');").SingleAsync().Result;
            response = request.CreateResponse<DashboardViewModel>(HttpStatusCode.OK, resultObj);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage CreateMemberWalletAndPointsEntry(HttpRequestMessage request, string userId)
        {
            HttpResponseMessage response = null;
            int result = 0;
            string RoleId = ConfigurationManager.AppSettings["MemberRoleId"];
            try
            {
                string AdminUserId = ConfigurationManager.AppSettings["AdminUserId"];
                Models.Wallet_Details details = new Models.Wallet_Details();
                details.account_id = 1;
                details.created_date = DateTime.Now;
                details.updated_date = DateTime.Now;
                details.created_by = AdminUserId;
                details.updated_by = AdminUserId;
                details.wallet_id = 1;
                details.is_admin = false;
                details.cash = 0;
                details.user_id = userId;
                details.role_id = RoleId;
                _walletDetailsRepository.Add(details);

                List<Models.Stock_Code> lstStock = new List<Models.Stock_Code>();
                lstStock = _stockCodeRepository.GetAll().Where(x => x.stock_code.ToLower() != "issue points").ToList();

                foreach (Models.Stock_Code code in lstStock)
                {
                    Models.Member_Stock_Details stock = new Models.Member_Stock_Details();
                    stock.account_id = 1;
                    stock.created_by = AdminUserId;
                    stock.created_date = DateTime.Now;
                    stock.email = User.Identity.Name;
                    stock.stock_amount = 0;
                    stock.stock_code_id = code.id;
                    stock.updated_by = AdminUserId;
                    stock.updated_date = DateTime.Now;
                    stock.user_id = userId;
                    stock.role_id = RoleId;
                    _memberStockDetailsRepository.Add(stock);
                }
                result = 1;
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            }
            catch (Exception ex)
            {
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            }

        }

        [HttpGet]
        public HttpResponseMessage AccountSettings(HttpRequestMessage request, string userId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (_userRepository.GetAll().ToList().Count > 0)
                {
                    //Wollo.Entities.ViewModels.User userinfo = new Wollo.Entities.ViewModels.User();
                    User userinfo = new User();
                    List<country_master> CountryDetails = new List<country_master>();
                    CountryDetails = AutoMapperHelper.GetInstance().Map<List<country_master>>(_countryRepository.GetAll().ToList());
                    userinfo = AutoMapperHelper.GetInstance().Map<User>(_userRepository.FindBy(x => x.user_id == userId).FirstOrDefault());
                    userinfo.CountryDetails = CountryDetails;
                    response = request.CreateResponse<Wollo.Entities.ViewModels.User>(HttpStatusCode.OK, userinfo);
                    return response;

                }
                else
                {
                    // no data
                }
                return response;
            });

        }

        [HttpPost]
        public HttpResponseMessage AddUpdateAccountSetting(HttpRequestMessage request, Models.User details)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int result = 0;
                if (_userRepository.GetAll().ToList().Count > 0)
                {
                    var userDetails = _userRepository.FindBy(x => x.user_id == details.created_by).FirstOrDefault();
                    try
                    {
                        userDetails.updated_by = details.created_by;
                        userDetails.updated_date = DateTime.UtcNow;
                        userDetails.created_date = DateTime.UtcNow;
                        userDetails.first_name = details.first_name;
                        userDetails.last_name = details.last_name;
                        userDetails.country_code = details.country_code;
                        userDetails.phone_number = details.phone_number;
                        userDetails.address = details.address;
                        userDetails.alternate_address = details.alternate_address;
                        userDetails.country_id = details.country_id;
                        userDetails.city = details.city;
                        userDetails.zip = details.zip;
                        _userRepository.Edit(userDetails);
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }

                }

                result = 1;
                response = request.CreateResponse<int>(HttpStatusCode.OK, result);
                return response;
            });
        }
    }

}
