using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class VisitorParking : BaseModel
    {
        [ForeignKey("Site")]
        public int SiteId { get; set; }
        public Site Site { get; set; }
        [ForeignKey("RegisterUser")]
        public int RegisterUserId { get; set; }
        public RegisterUser RegisterUser { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Surname { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        [StringLength(100)]
        public string MobileNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VRMNumber { get; set; }
        public int VisitorBayNoId { get; set; }
        public string Duration { get; set; }
        public string SessionUnit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public bool CCtome { get; set; }
    }
}
