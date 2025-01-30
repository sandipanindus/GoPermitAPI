using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class TeamUser:BaseModel
    {
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        [ForeignKey("RegisterUser")]
        public int RegisterUserId { get; set; }
    }
}
