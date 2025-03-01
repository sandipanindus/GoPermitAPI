using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class Site:BaseModel
    {
        [StringLength(200)]
        public string SiteName { get; set; }
        public string SiteAddress { get; set; }
        [StringLength(200)]
        public string City { get; set; }
        [StringLength(200)]
        public string State { get; set; }
        [StringLength(20)]
        public string Zipcode { get; set; }
        [StringLength(200)]
        public string ContactPersonName { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        [StringLength(20)]
        public string ContactNumber { get; set; }
        [StringLength(20)]
        public string MobileNumber { get; set; }
        [StringLength(100)]
        public string TenantParkingBays { get; set; }
        [StringLength(100)]
        public string VisitorParkingBays { get; set; }

        public int NoOfTotalBay { get; set; }
        public int IndustryId { get; set; }
        public int ParkingBaySectionsOrFloors { get; set; }
        [StringLength(50)]
        public string ParkingBaySeperator { get; set; }

        public int VisitorSectionsOrFloors { get; set; }
        [StringLength(50)]
        public string VisitorSeperator { get; set; }
        public int MaxVehiclesPerBay { get; set; }

        public int OperatorId { get; set; }
        public string ZatparkSitecode { get; set; }
        public bool ManageParkingAvailble { get; set; }
        public bool visitorParkingAvailble { get; set; }
        public bool Zatparklogs24hrs { get; set; }


        public ICollection<BayConfig> BayConfigs { get; set; }
        public ICollection<ParkingBay> ParkingBays { get; set; }
        public ICollection<VisitorBay> VisitorBays { get; set; }
        public ICollection<VisitorParking> VisitorParkings { get; set; }
        public ICollection<VisitorBaySession> VisitorBaySessions { get; set; }
        public virtual ICollection<Ping> Pings { get; set; }
    }
}
