using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
   public  class VehicleRegistrationTimeSlot:BaseModel
    {
        public int ParkingBayNo { get; set; }
        public string VehicleNo { get; set; }
        public string TimeSlot { get; set; }
        public string Status { get; set; }
        public int RegisterUserId { get; set; }
        public int VehicleRegistrationId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int IsSaveCount { get; set; }

        public bool IsSentToZatPark { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime? SentToZatparkDateTime { get; set; }
        public string ZatparkResponse { get; set; }
    }
}
