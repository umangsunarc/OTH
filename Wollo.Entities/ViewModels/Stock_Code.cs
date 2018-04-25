using Wollo.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Stock_Code : BaseAuditableViewModel
    {
        [DataMember]
        [Display(Name = "ID", ResourceType = typeof(Resource))]

        public int id { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "Name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Name cannot be smaller than 3 characters.")]
        [Display(Name = "StockName", ResourceType = typeof(Resource))]
        public string stock_name { get; set; }

        [DataMember]
        [MaxLength(45, ErrorMessage = "Name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Name cannot be smaller than 3 characters.")]
        [Display(Name = "StockCode", ResourceType = typeof(Resource))]
        public string stock_code { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(Resource))]
        public string full_name { get; set; }

        //********************************************** label properties *****************************************************//

        [DataMember]
        [Display(Name = "PointsIssued", ResourceType = typeof(Resource))]
        public string points_issued { get; set; }
        [DataMember]
        [Display(Name = "Points", ResourceType = typeof(Resource))]
        public string points { get; set; }
        [DataMember]
        [Display(Name = "IssueNewRewardPoints", ResourceType = typeof(Resource))]
        public string issue_new_reward_points { get; set; }
        [DataMember]
        [Display(Name = "DashboardPointsIssued", ResourceType = typeof(Resource))]
        public string dashboard_points_issued { get; set; }
        [DataMember]
        [Display(Name = "Confirm", ResourceType = typeof(Resource))]
        public string confirm { get; set; }
        [DataMember]
        [Display(Name = "Cancel", ResourceType = typeof(Resource))]
        public string cancel { get; set; }
        [DataMember]
        [Display(Name = "SelectStock", ResourceType = typeof(Resource))]
        public string select_stock { get; set; }
        [DataMember]
        [Display(Name = "DashboardPointsIssuedSelf", ResourceType = typeof(Resource))]
        public string dashboard_points_issued_self { get; set; }

        //********************************************** label properties *****************************************************//



    }
}
