using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wollo.Base.Entity;
using Wollo.Base.LocalResource;
using Wollo.Entities.ViewModels;

namespace Wollo.Web.Models
{
    public class TransferPointsViewModel : BaseAuditableViewModel
    {
        public int stock_amount { get; set; }
        public int stock_id { get; set; }
        public int transfer_amount { get; set; }
        public string email { get; set; }
        public string user_id { get; set; }
        public List<Reward_Points_Transfer_Details> RewardPointsTransferDetails { get; set; }
        public CommonWordsViewModel CommonWordsViewModel { get; set; }
        public TransferPointsViewModel()
        {
            RewardPointsTransferDetails = new List<Reward_Points_Transfer_Details>();
        }
        //*****************************************Label Enteries*************************************/
        [Display(Name = "TransferRewardPoints", ResourceType = typeof(Resource))]
        public string transfer_reward_points { get; set; }

        [Display(Name = "DashboardTransferRewardPoints", ResourceType = typeof(Resource))]
        public string dashboard_transfer_reward_points { get; set; }

        [Display(Name = "CurrentStockAmount", ResourceType = typeof(Resource))]
        public string current_stock_amount { get; set; }
        [Display(Name = "SelectEstore", ResourceType = typeof(Resource))]
        public string select_e_store { get; set; }
        [Display(Name = "Transfer", ResourceType = typeof(Resource))]
        public string transfer { get; set; }
        [Display(Name = "TransferAmount", ResourceType = typeof(Resource))]
        public string transfer_amount_label { get; set; }
        [Display(Name = "TransferredFrom", ResourceType = typeof(Resource))]
        public string transfered_from { get; set; }
        [Display(Name = "TransferredAmount", ResourceType = typeof(Resource))]
        public string transfered_amount { get; set; }
        
    }
}