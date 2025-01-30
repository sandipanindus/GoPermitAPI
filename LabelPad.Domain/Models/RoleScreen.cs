using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
   public class RoleScreen:BaseModel
    {
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        [ForeignKey("Screen")]
        public int ScreenId { get; set; }
       
        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Approve { get; set; }
        public bool Reject { get; set; }
        public int ClientId { get; set; }
        public int LoginId { get; set; }
    }
}
