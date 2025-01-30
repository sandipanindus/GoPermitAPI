using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class Ping:BaseModel
    {
        [ForeignKey("RegisterUser")]
        public int RegisterUserId { get; set; }
        public virtual RegisterUser RegisterUser { get; set; }
        [ForeignKey("Site")]
        public int SiteId { get; set; }
        public virtual Site Site { get; set; }
        public DateTime Date { get; set; }
    }
}
