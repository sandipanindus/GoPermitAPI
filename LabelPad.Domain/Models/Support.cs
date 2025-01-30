using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class Support:BaseModel
    {
        public string Issue { get; set; }
        [ForeignKey("RegisterUser")]
        public int RegisterUserId { get; set; }
        public RegisterUser RegisterUser { get; set; }
        public int ParentId { get; set; }
        public string Subject { get; set; }
        public bool IsRead { get; set; }
        public int TicketId { get; set; }
        public string Status { get; set; }
    }
}
