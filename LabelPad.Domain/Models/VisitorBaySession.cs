using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
   public  class VisitorBaySession:BaseModel
    {
        [ForeignKey("Site")]
        public int SiteId { get; set; }
        public Site Site { get; set; }
        public string Duration { get; set; }
        public string SessionUnit { get; set; }
    }
}
