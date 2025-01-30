using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class BayConfig:BaseModel
    {
        [ForeignKey("ParkingBayNo")]
        public int ParkingBayNoId { get; set; }
        public ParkingBayNo ParkingBayNo { get; set; }
        [ForeignKey("RegisterUser")]
        public int RegisterUserId { get; set; }
        public RegisterUser RegisterUser { get; set; }
        [ForeignKey("Site")]
        public int SiteId { get; set; }
        public Site Site { get; set; }
    }
}
