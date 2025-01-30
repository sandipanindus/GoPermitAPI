using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
    public class AddVehicleTimeSlotAc
    {
        public string BayNo { get; set; }
        public string VehicleNo { get; set; }
        public string TimeSlot { get; set; }
        public string Status { get; set; }
        public int TenantId { get; set; }
    }
}
