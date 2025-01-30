using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class ParkingBayNo:BaseModel
    {
        public string BayName { get; set; }
        public int ParkingBayId { get; set; }
        public int SiteId { get; set; }
        public string Section { get; set; }
        public int RegisterUserId { get; set; }
        public int MaxVehiclesPerBay { get; set; }
        public bool Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //public string VRM { get; set; }

        public ICollection<BayConfig> BayConfigs { get; set; }
    }
}
