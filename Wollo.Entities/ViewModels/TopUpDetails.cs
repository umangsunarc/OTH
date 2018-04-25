using Wollo.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class TopUpDetails
    {
        [DataMember]
        [Display(Name = "Cash", ResourceType = typeof(Resource))]
        public float Cash { get; set; }

        [DataMember]
        public CommonWordsViewModel CommonWordsViewModel { get; set; }

        //*****************************Properties for labels******************************************//
        //[DataMember]
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string user_name { get; set; }

        //[DataMember]
        [Display(Name = "Topup", ResourceType = typeof(Resource))]
        public string topup { get; set; }

        //[DataMember]
        [Display(Name = "FundTopup", ResourceType = typeof(Resource))]
        public string fund_topup { get; set; }

        [Display(Name = "AddTopup", ResourceType = typeof(Resource))]
        public string add_topup { get; set; }

        //[DataMember]
        [Display(Name = "DashboardFundTopup", ResourceType = typeof(Resource))]
        public string dashboard_fundtopup { get; set; }

        //[DataMember]
        [Display(Name = "TopupHistory", ResourceType = typeof(Resource))]
        public string topup_history { get; set; }

        //[DataMember]
        [Display(Name = "CashEWallet", ResourceType = typeof(Resource))]
        public string cash_e_wallet { get; set; }

        //[DataMember]
        [Display(Name = "Details", ResourceType = typeof(Resource))]
        public string details { get; set; }

        //[DataMember]
        [Display(Name = "Actions", ResourceType = typeof(Resource))]
        public string actions { get; set; }

        [Display(Name = "ChangeStatus", ResourceType = typeof(Resource))]
        public string change_status { get; set; }

        [Display(Name = "Modify", ResourceType = typeof(Resource))]
        public string modify { get; set; }

        [Display(Name = "Cancel", ResourceType = typeof(Resource))]
        public string cancel { get; set; }

        [Display(Name = "Confirm", ResourceType = typeof(Resource))]
        public string confirm { get; set; }

        [Display(Name = "DateTime", ResourceType = typeof(Resource))]
        public string date_time { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resource))]
        public string status { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public string amount { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string username { get; set; }

        [Display(Name = "Minimum10topup", ResourceType = typeof(Resource))]
        public string minimum_10_topup { get; set; }
      
        //*****************************Properties for labels******************************************//

        [DataMember]
        public List<Topup_Status_Master> TopupStatusMaster { get; set; }

        [DataMember]
        public List<Topup_History> Topups { get; set; }

        [DataMember]
        public AddTopupModel AddTopupModel { get; set; }
        public TopUpDetails()
        {
            AddTopupModel = new AddTopupModel();
        }
    }
}
