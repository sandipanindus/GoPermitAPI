using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class ParkingBay : BaseModel
    {
        public string Prefix { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int count { get; set; }
        [ForeignKey("Site")]
        public int SiteId { get; set; }
        public Site Site { get; set; }

        public int Section { get; set; }
    }
}
