using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wollo.Base.Entity;
using Wollo.Base.LocalResource;

namespace Wollo.Entities.ViewModels
{
    public class HolidayData
    {
        public List<Holiday_Master> lstHolidayMaster { get; set; }
        public List<Holiday_Status_Master> lstHolidayStatusMaster { get; set; }
        public CommonWordsViewModel CommonWordsViewModel { get; set; }

        //****************************** Label Properties ***********************************************//
        [Display(Name = "Notification", ResourceType = typeof(Resource))]
        public string notification { get; set; }

        [Display(Name = "DashboardNotification", ResourceType = typeof(Resource))]
        public string dashboard_notification { get; set; }

        [Display(Name = "HolidayClosingDate", ResourceType = typeof(Resource))]
        public string holiday_closing_date { get; set; }

        [Display(Name = "AddNewHolidayClosingDate", ResourceType = typeof(Resource))]
        public string add_new_holiday_closing_date { get; set; }

        [Display(Name = "NewHolidayClosingDate", ResourceType = typeof(Resource))]
        public string new_holiday_closing_date { get; set; }

        [Display(Name = "ModifyHolidayClosingData", ResourceType = typeof(Resource))]
        public string modify_holiday_closing_data { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Resource))]
        public string date { get; set; }

        [Display(Name = "NotifyBefore", ResourceType = typeof(Resource))]
        public string notify_before { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Resource))]
        public string description { get; set; }

        [Display(Name = "WordsLimit", ResourceType = typeof(Resource))]
        public string words_limit { get; set; }

        [Display(Name = "Cancel", ResourceType = typeof(Resource))]
        public string cancel { get; set; }

        [Display(Name = "Confirm", ResourceType = typeof(Resource))]
        public string confirm { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resource))]
        public string status { get; set; }

        [Display(Name = "Actions", ResourceType = typeof(Resource))]
        public string action { get; set; }

        [Display(Name = "Modify", ResourceType = typeof(Resource))]
        public string modify { get; set; }

        [Display(Name = "ChangeStatus", ResourceType = typeof(Resource))]
        public string change_status { get; set; }

        [Display(Name = "All", ResourceType = typeof(Resource))]
        public string all { get; set; }

        [Display(Name = "Notifications", ResourceType = typeof(Resource))]
        public string notifications { get; set; }

        [Display(Name = "AddNewNotification", ResourceType = typeof(Resource))]
        public string add_new_notification { get; set; }

        //****************************** Label Properties ***********************************************//
    }




}
