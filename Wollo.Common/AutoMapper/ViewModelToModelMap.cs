using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Wollo.Entities.Models;

namespace Wollo.Common.AutoMapper
{
    public class ViewModelToModelMap : Profile
    {
        /// <summary>
        /// Configure automapper
        /// </summary>
        protected override void Configure()
        {
           
            base.CreateMap<Wollo.Entities.Models.Reward_Points_Transfer_Details, Wollo.Entities.ViewModels.Reward_Points_Details>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Queue_Action_Master, Wollo.Entities.ViewModels.Queue_Action_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Queue_Status_Master, Wollo.Entities.ViewModels.Queue_Status_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Stock_Code, Wollo.Entities.ViewModels.Stock_Code>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Transaction_History, Wollo.Entities.ViewModels.Transaction_History>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Wallet_Details, Wollo.Entities.ViewModels.Wallet_Details>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Wallet_Master, Wollo.Entities.ViewModels.Wallet_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Cash_Transaction_History, Wollo.Entities.ViewModels.Cash_Transaction_History>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Topup_History, Wollo.Entities.ViewModels.Topup_History>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Topup_Status_Master, Wollo.Entities.ViewModels.Topup_Status_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Member_Stock_Details, Wollo.Entities.ViewModels.Member_Stock_Details>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Issue_Points_Master, Wollo.Entities.ViewModels.Issue_Points_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Issue_Points_Transfer_Master, Wollo.Entities.ViewModels.Issue_Points_Transfer_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Withdrawel_History_Details, Wollo.Entities.ViewModels.Withdrawel_History_Details>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Withdrawel_History_Master, Wollo.Entities.ViewModels.Withdrawel_History_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Rule_Config_Master, Wollo.Entities.ViewModels.Rule_Config_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Issue_Withdrawel_Permission_Master, Wollo.Entities.ViewModels.Issue_Withdrawel_Permission_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Withdrawl_Status_Master, Wollo.Entities.ViewModels.Withdrawl_Status_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Withdrawl_Fees, Wollo.Entities.ViewModels.Withdrawl_Fees>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Rule_Config_Settings, Wollo.Entities.ViewModels.Rule_Config_Settings>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Traded_History_Master, Wollo.Entities.ViewModels.Traded_History_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Holiday_Status_Master, Wollo.Entities.ViewModels.Holiday_Status_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Holiday_Master, Wollo.Entities.ViewModels.Holiday_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.AspnetUsers, Wollo.Entities.ViewModels.AspnetUsers>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Units_Master, Wollo.Entities.ViewModels.Units_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Issue_Cash_Transfer_Master, Wollo.Entities.ViewModels.Issue_Cash_Transfer_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Administration_Fees, Wollo.Entities.ViewModels.Administration_Fees>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.User, Wollo.Entities.ViewModels.User>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Member_Status_Master, Wollo.Entities.ViewModels.Member_Status_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Country_Master, Wollo.Entities.ViewModels.country_master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Market_Rate_Details, Wollo.Entities.ViewModels.Market_Rate_Details>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Reward_Points_Transfer_Master, Wollo.Entities.ViewModels.Reward_Points_Transfer_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Reward_Points_Transfer_Details, Wollo.Entities.ViewModels.Reward_Points_Transfer_Details>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Transfer_Action_Master, Wollo.Entities.ViewModels.Transfer_Action_Master>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Payment_Method, Wollo.Entities.ViewModels.Payment_Method>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Cash_Transaction_Detail, Wollo.Entities.ViewModels.Cash_Transaction_Detail>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Issue_Point_Transfer_Detail, Wollo.Entities.ViewModels.Issue_Point_Transfer_Detail>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.AspnetRoles, Wollo.Entities.ViewModels.AspnetRoles>().ReverseMap();
            base.CreateMap<Wollo.Entities.Models.Audit_Log_Master, Wollo.Entities.ViewModels.Audit_Log_Master>().ReverseMap();
        }
    }
}
