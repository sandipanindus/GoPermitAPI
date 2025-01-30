using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
   public class VisitorParkingVehicleDetails:BaseModel
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string VRMNumber { get; set; }
        [ForeignKey("VisitorBayNo")]
        public int VisitorBayNoId { get; set; }
        public VisitorBayNo VisitorBayNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int VisitorParkingId { get; set; }

    }
}
