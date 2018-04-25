using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.LocalResource;

namespace Wollo.Web.Models
{
    public class LandingPageViewModel
    {
        [Display(Name="Home", ResourceType = typeof(Resource))]
        public string Home { get; set; }
        [Display(Name = "AboutUs", ResourceType = typeof(Resource))]
        public string AboutUs { get; set; }
        [Display(Name = "ContactUs", ResourceType = typeof(Resource))]
        public string ContactUs { get; set; }
        [Display(Name = "Login", ResourceType = typeof(Resource))]
        public string login { get; set; }

        [Display(Name = "Register", ResourceType = typeof(Resource))]
        public string register { get; set; }
        //*********************************** Label Properties******************************************//
    }
}