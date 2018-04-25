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
    public class CommonWordsViewModel
    {
        [Display(Name = "Refresh", ResourceType = typeof(Resource))]
        public string refresh { get; set; }

        [Display(Name = "Filter", ResourceType = typeof(Resource))]
        public string filter { get; set; }

        [Display(Name = "Submit", ResourceType = typeof(Resource))]
        public string submit { get; set; }

        [Display(Name = "Select", ResourceType = typeof(Resource))]
        public string select { get; set; }

        [Display(Name = "Update", ResourceType = typeof(Resource))]
        public string update { get; set; }

        [Display(Name = "From", ResourceType = typeof(Resource))]
        public string from { get; set; }

        [Display(Name = "To", ResourceType = typeof(Resource))]
        public string to { get; set; }
    }
}
