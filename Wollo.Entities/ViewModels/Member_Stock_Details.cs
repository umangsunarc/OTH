using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Member_Stock_Details : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        [ForeignKey("AspnetUsers")]
        public string user_id { get; set; }
        [DataMember]
        [ForeignKey("AspnetRoles")]
        public string role_id{get;set;}
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public int stock_amount { get; set; }
        [DataMember]
        public int account_id { get; set; }
        [DataMember]
        [ForeignKey("StockCode")]
        public int stock_code_id { get; set; }
        //[DataMember]
        //public CommonWordsViewModel CommonWordsViewModel { get; set; }
        [DataMember]
        public virtual Stock_Code StockCode { get; set; }

        [DataMember]
        public virtual AspnetUsers AspnetUsers { get; set; }
        [DataMember]
        public virtual AspnetRoles AspnetRoles { get; set; }

        //**********************Label*************************//

        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string username { get; set; }

        [Display(Name = "TestRewardPointsInCirculationHistory", ResourceType = typeof(Resource))]
        public string test_reward_points_in_circulation_history { get; set; }

        [Display(Name = "RewardPoint", ResourceType = typeof(Resource))]
        public string reward_point { get; set; }

        [Display(Name = "DashboardTestRewardPointsInCirculationHistory", ResourceType = typeof(Resource))]
        public string dashboard_test_reward_points_in_circulation_history { get; set; }

        [Display(Name = "WolloRewardPointsInCirculationHistory", ResourceType = typeof(Resource))]
        public string wollo_reward_points_in_circulation_history { get; set; }


        //**********************End***************************//
    }
}
