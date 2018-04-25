﻿using Wollo.Base.Entity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Wollo.Entities.ViewModels
{
    [DataContract]
    public class Queue_Action_Master : BaseAuditableViewModel
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        [MaxLength(45, ErrorMessage = "Name cannot be longer than 45 characters.")]
        [MinLength(3, ErrorMessage = "Name cannot be smaller than 3 characters.")]
        public string name { get; set; }
    }
}