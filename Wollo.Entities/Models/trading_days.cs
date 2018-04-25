using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Wollo.Base.Entity;

namespace Wollo.Entities.Models
{
    public class trading_days : Entity
    {
        public string name { get; set; }
        public bool is_selected { get; set; }
    }
}
