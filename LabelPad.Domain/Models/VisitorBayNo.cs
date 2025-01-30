using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class VisitorBayNo:BaseModel
    {
        public string BayName { get; set; }
        [ForeignKey("VisitorBay")]
        public int VisitorBayId { get; set; }
        public VisitorBay VisitorBay { get; set; }
        public int SiteId { get; set; }
        public string Section { get; set; }
        public ICollection<VisitorParkingVehicleDetails> VisitorParkingVehicleDetails { get; set; }


    }
}
