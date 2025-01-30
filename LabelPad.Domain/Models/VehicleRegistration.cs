using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class VehicleRegistration : BaseModel
    {
        [StringLength(50)]
        public string Make { get; set; }
        [StringLength(50)]
        public string Model { get; set; }
        [StringLength(50)]
        public int ParkingBayNo { get; set; }
        [StringLength(50)]
        public string ConfigNo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [ForeignKey("RegisterUser")]
        public int RegisterUserId { get; set; }
        public string VRM { get; set; }
        public RegisterUser RegisterUser { get; set; }
        public int IsSaveCount { get; set; }
        public bool IsSentToZatPark { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime? SentToZatparkDateTime { get; set; }
        public string ZatparkResponse { get; set; }

    }
}
