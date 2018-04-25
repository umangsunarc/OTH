using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.LocalResource;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.Entity;

namespace Wollo.Entities.ViewModels
{
    public class WithdrawalData
    {
        [DataMember]
        public CommonWordsViewModel CommonWordsViewModel { get; set; }
        //***************************** Label Properties *********************************************//
        [DataMember]
        [Display(Name = "CashWithdrawal", ResourceType = typeof(Resource))]
        public string cash_withdrawal { get; set; }

        [DataMember]
        [Display(Name = "DashboardCashWithdrawal", ResourceType = typeof(Resource))]
        public string dashboard_cash_withdrawel { get; set; }

        [DataMember]
        [Display(Name = "MyWithdrawals", ResourceType = typeof(Resource))]
        public string my_withdrawal { get; set; }

        [DataMember]
        [Display(Name = "AddNewWithdrawals", ResourceType = typeof(Resource))]
        public string add_new_withdrawel { get; set; }

        [DataMember]
        [Display(Name = "Details", ResourceType = typeof(Resource))]
        public string details { get; set; }

        [DataMember]
        [Display(Name = "Actions", ResourceType = typeof(Resource))]
        public string actions { get; set; }

        [DataMember]
        [Display(Name = "ViewDetails", ResourceType = typeof(Resource))]
        public string view_details { get; set; }

        [DataMember]
        [Display(Name = "NewWithdrawal", ResourceType = typeof(Resource))]
        public string new_withdrawal { get; set; }

        [DataMember]
        [Display(Name = "ChangeStatus", ResourceType = typeof(Resource))]
        public string change_status { get; set; }

        [DataMember]
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string user_name { get; set; }

        [DataMember]
        [Display(Name = "Status", ResourceType = typeof(Resource))]
        public string status { get; set; }

        [DataMember]
        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public string amount { get; set; }

        [DataMember]
        [Display(Name = "Total", ResourceType = typeof(Resource))]
        public string total { get; set; }

        [DataMember]
        [Display(Name = "Cancel", ResourceType = typeof(Resource))]
        public string cancel { get; set; }

        [DataMember]
        [Display(Name = "Modify", ResourceType = typeof(Resource))]
        public string modify { get; set; }

        [DataMember]
        [Display(Name = "Confirm", ResourceType = typeof(Resource))]
        public string confirm { get; set; }

        [DataMember]
        [Display(Name = "Method", ResourceType = typeof(Resource))]
        public string method { get; set; }

        [DataMember]
        [Display(Name = "WithdrawelFee", ResourceType = typeof(Resource))]
        public string withdrawel_fee { get; set; }

        [DataMember]
        [Display(Name = "Ok", ResourceType = typeof(Resource))]
        public string ok { get; set; }
        [DataMember]
        [Display(Name = "Cash", ResourceType = typeof(Resource))]
        public float Cash { get; set; }
        [Display(Name = "CashEWallet", ResourceType = typeof(Resource))]
        public string cash_e_wallet { get; set; }

        [Display(Name = "Withdrawal", ResourceType = typeof(Resource))]
        public string withdrawal { get; set; }

        //***************************** Label Properties end *****************************************//
        public List<Withdrawl_Status_Master> lstWithdrawalStatusMaster { get; set; }
        public List<Withdrawel_History_Details> lstWithdrawalHistoryDeatils { get; set; }

    }
}
