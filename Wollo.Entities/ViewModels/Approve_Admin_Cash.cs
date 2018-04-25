using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.Entity;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
     [DataContract]
    public class Approve_Admin_Cash
    {
         [DataMember]
         public List<Issue_Cash_Transfer_Master> IssueCashTransferMaster { get; set; }

         [DataMember]
         public List<Issue_Withdrawel_Permission_Master> IssueWithdrawelTransferMaster { get; set; }

         [DataMember]
         public CommonWordsViewModel CommonWordsViewModel { get; set; } 
         public Approve_Admin_Cash()
         {
             IssueCashTransferMaster = new List<Issue_Cash_Transfer_Master>();
             IssueWithdrawelTransferMaster = new List<Issue_Withdrawel_Permission_Master>();
         }

         //****************************** Label Properties ***********************************************//
         [Display(Name = "UserName", ResourceType = typeof(Resource))]
         public string user_name { get; set; }

        //****************************** Label Properties ***********************************************//
    }
}
